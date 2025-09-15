using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using previsao_tempo.Models.Cidades;
using previsao_tempo.Models.DTO;
using previsao_tempo.Models.OpenWeather;
using previsao_tempo.Utils;

namespace previsao_tempo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClimaController : Controller
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClientDaily;
        private readonly HttpClient _httpClientCurrent;

        public ClimaController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _config = config;
            _httpClientDaily = httpClientFactory.CreateClient("OpenWeatherDaily");
            _httpClientCurrent = httpClientFactory.CreateClient("OpenWeatherCurrent");
        }

        [HttpGet]
        public async Task<IActionResult> GetPrevisaoTempo([FromQuery] LatitudeLongitudeDTO latitudeLongitudeDTO)
        {
            var response = await _httpClientCurrent
                .GetAsync($"?lat={latitudeLongitudeDTO.Latitude}&lon={latitudeLongitudeDTO.Longitude}&appid={_config["api-key"]}&lang=pt_br&units=metric");

            string responseBody = await response.Content.ReadAsStringAsync();

            CurrentWeatherOpenWeather? current = JsonSerializer.Deserialize<CurrentWeatherOpenWeather>(responseBody)!;

            if (current == null)
                return NotFound();

            ClimaDTO climaAtual = new ClimaDTO
            {
                Descricao = current.weather[0].description,
                Url_Icone = _config["icon-url"] + current.weather[0].icon + "@2x.png",
                Temperatura = current.main.temp,
                Temperatura_Minima = current.main.temp_min,
                Temperatura_Maxima = current.main.temp_max,
                Umidade = current.main.humidity
            };

            return Ok(climaAtual);
        }

        [HttpGet("Previsao")]
        public async Task<IActionResult> GetPrevisaoTempoDias([FromQuery] LatitudeLongitudeDTO latitudeLongitudeDTO)
        {
            List<PrevisaoClimaDTO> previsaoDias = new List<PrevisaoClimaDTO>();
            var response = await _httpClientDaily
                .GetAsync($"?lat={latitudeLongitudeDTO.Latitude}&lon={latitudeLongitudeDTO.Longitude}&appid={_config["api-key"]}&lang=pt_br&units=metric");

            string responseBody = await response.Content.ReadAsStringAsync();

            DailyWeatherDataOpenWeather? lstDaily = JsonSerializer.Deserialize<DailyWeatherDataOpenWeather>(responseBody)!;

            if (lstDaily == null)
                return NotFound();

            foreach (var daily in lstDaily.list)
            {
                previsaoDias.Add(new PrevisaoClimaDTO
                {
                    Data = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(daily.dt).ToLocalTime(),
                    Clima = new ClimaDTO
                    {
                        Descricao = daily.weather[0].description,
                        Url_Icone = _config["icon-url"] + daily.weather[0].icon + "@2x.png",
                        Temperatura = daily.main.temp,
                        Temperatura_Minima = daily.main.temp_min,
                        Temperatura_Maxima = daily.main.temp_max,
                        Umidade = daily.main.humidity
                    }
                });
            }

            List<int> horas = new List<int> { 0, 3, 6, 9, 12, 15, 18, 21 };

            int? proximaHora = horas.Where(x => x > DateTime.Now.Hour).First();

            if (!proximaHora.HasValue)
                proximaHora = 0;

            return Ok(previsaoDias
                .Where(x => x.Data.Hour == proximaHora
                    && x.Data > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59)));
        }
    }
}
