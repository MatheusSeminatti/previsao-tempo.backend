using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using previsao_tempo.Data.Repositories;
using previsao_tempo.Entities;
using previsao_tempo.Models.DTO;

namespace previsao_tempo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CidadesFavoritasController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICidadesFavoritasRepository _cidadesFavoritasRepository;
        public CidadesFavoritasController(ICidadesFavoritasRepository cidadesFavoritasRepository, 
            UserManager<ApplicationUser> userManager)
        {
            _cidadesFavoritasRepository = cidadesFavoritasRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCidadesFavoritas()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null)
                return Unauthorized();

            var user = await this._userManager.FindByEmailAsync(email);

            if (user == null)
                return Unauthorized();

            var lstCidades = await _cidadesFavoritasRepository.GetAsync(user.Id);
            return Ok(lstCidades);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarCidadeFavorita(AddCidadeFavoritaDTO cidadeFavoritaDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null)
                return Unauthorized();

            var user = await this._userManager.FindByEmailAsync(email);

            if (user == null)
                return Unauthorized();

            CidadeFavorita cidadeFavorita = new CidadeFavorita
            {
                Estado = cidadeFavoritaDTO.Estado,
                Nome = cidadeFavoritaDTO.Nome,
                Pais = cidadeFavoritaDTO.Pais,
                Latitude = cidadeFavoritaDTO.Latitude,
                Longitude = cidadeFavoritaDTO.Longitude,
                UsuarioId = user.Id
            };

            await _cidadesFavoritasRepository.AdicionarAsync(cidadeFavorita);
            return Ok(cidadeFavorita);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverCidadeFavorita(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null)
                return Unauthorized();

            var user = await this._userManager.FindByEmailAsync(email);

            if (user == null)
                return Unauthorized();

            var lstCidades = await _cidadesFavoritasRepository.GetAsync(user.Id);

            if(lstCidades.Where(x => x.Id == id).Count() == 0)
                return Unauthorized();

            await _cidadesFavoritasRepository.RemoverAsync(id);
            return Ok();
        }
    }
}
