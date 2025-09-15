using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using previsao_tempo.Entities;

namespace previsao_tempo.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<CidadeFavorita> CidadesFavoritas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CidadeFavorita>()
                .HasOne(x => x.Usuario)
                .WithMany(x => x.CidadesFavoritas)
                .HasForeignKey(x => x.UsuarioId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<CidadeFavorita>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 15);

            builder.Entity<CidadeFavorita>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 15);
        }
    }
}
