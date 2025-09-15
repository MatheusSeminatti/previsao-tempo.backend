using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using previsao_tempo.Data;
using previsao_tempo.Entities;
using previsao_tempo.Data.Repositories;
using previsao_tempo.Services;

var builder = WebApplication.CreateBuilder(args);

TokenService.Secret = Environment.GetEnvironmentVariable("PrivateKey");

// CORS
var myPolicyName = "CORSPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myPolicyName,
      configurePolicy: policy =>
      {
          policy.WithOrigins(builder.Configuration.GetValue<string>("AllowedOrigins")!.Split(";"))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
      });
});

// Entity Framework
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration["PrevisaoTempoConnection"]));

// Add services to the container.
builder.Services.AddHttpClient("OpenWeatherGeo", client => {
    client.BaseAddress = new Uri(builder.Configuration["OpenWeatherGeo"]!);
});
builder.Services.AddHttpClient("OpenWeatherCurrent", client => {
    client.BaseAddress = new Uri(builder.Configuration["OpenWeatherCurrent"]!);
});
builder.Services.AddHttpClient("OpenWeatherDaily", client => {
    client.BaseAddress = new Uri(builder.Configuration["OpenWeatherDaily"]!);
});

builder.Services.AddScoped<ICidadesFavoritasRepository, CidadesFavoritasRepository>();


builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789( )@-._&ãõáéíóúâêôç";
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // Obriga uso do HTTPs
    options.RequireHttpsMetadata = false;

    // Salva os dados de login no AuthenticationProperties
    options.SaveToken = true;

    // Configurações para leitura do Token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Chave que usamos para gerar o Token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["PrivateKey"]!)),
        // Validações externas
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
