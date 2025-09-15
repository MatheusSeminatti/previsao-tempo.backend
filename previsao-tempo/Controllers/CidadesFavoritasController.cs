using Microsoft.AspNetCore.Authorization;
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
        private readonly ICidadesFavoritasRepository _cidadesFavoritasRepository;
        public CidadesFavoritasController(ICidadesFavoritasRepository cidadesFavoritasRepository)
        {
            _cidadesFavoritasRepository = cidadesFavoritasRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCidadesFavoritas()
        {
            var lstCidades = await _cidadesFavoritasRepository.GetAsync("42c6cdb3-81a0-4776-87f6-f37e7d31928e");
            return Ok(lstCidades);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarCidadeFavorita(AddCidadeFavoritaDTO cidadeFavoritaDTO)
        {
            CidadeFavorita cidadeFavorita = new CidadeFavorita
            {
                Estado = cidadeFavoritaDTO.Estado,
                Nome = cidadeFavoritaDTO.Nome,
                Pais = cidadeFavoritaDTO.Pais,
                Latitude = cidadeFavoritaDTO.Latitude,
                Longitude = cidadeFavoritaDTO.Longitude,
                UsuarioId = "42c6cdb3-81a0-4776-87f6-f37e7d31928e"
            };

            await _cidadesFavoritasRepository.AdicionarAsync(cidadeFavorita);
            return Ok(cidadeFavorita);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverCidadeFavorita(int id)
        {
            await _cidadesFavoritasRepository.RemoverAsync(id);
            return Ok();
        }
    }
}
