using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using previsao_tempo.Models.Cidades;
using previsao_tempo.Models.DTO;

namespace previsao_tempo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadesController : Controller
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClientGeo;
        public CidadesController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientGeo = httpClientFactory.CreateClient("OpenWeatherGeo");
        }

        [HttpGet]
        public async Task<IActionResult> GetCidade([FromQuery] string nome)
        {
            var response = await _httpClientGeo.GetAsync($"?q={nome.Trim()}&limit={5}&appid={_config["api-key"]}");
            string responseBody = await response.Content.ReadAsStringAsync();

            List<CityOpenWeather>? cidades = JsonSerializer.Deserialize<List<CityOpenWeather>>(responseBody);

            if (cidades == null)
                return NotFound();

            return Ok(cidades.Select(x => new GeoCidadeDTO
            {
                Nome = x.name,
                Estado = x.state,
                Pais = x.country,
                Latitude = x.lat,
                Longitude = x.lon
            }));
        }
    }
}
