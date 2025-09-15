using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using previsao_tempo.Entities;
using previsao_tempo.Models.DTO;
using previsao_tempo.Services;

namespace previsao_tempo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsuariosController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpPost("Criar")]
        public async Task<IActionResult> Criar(CreateUsuarioDTO createUsuarioDTO)
        {
            var createResult = await this._userManager!
                .CreateAsync(new ApplicationUser
                {
                    UserName = createUsuarioDTO.Nome,
                    Email = createUsuarioDTO.Email
                }, createUsuarioDTO.Senha);

            if (!createResult.Succeeded)
                return BadRequest(new
                {
                    errors = new
                    {
                        Criacao = createResult.Errors.Select(x => x.Description).ToList()
                    }
                });

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await this._userManager!
                .FindByEmailAsync(loginDTO.Email);

            if (user == null)
                return StatusCode(401);

            if (!user.LockoutEnabled)
                return StatusCode(401);

            var signInResult = await this._signInManager!
                .PasswordSignInAsync(user, loginDTO.Senha, false, false);

            if (!signInResult.Succeeded)
                return StatusCode(401);

            return Ok(new
            {
                usuario = new
                {
                    nome = user.UserName,
                    email = user.Email
                },
                token = TokenService.Generate(user)
            });
        }
    }
}
