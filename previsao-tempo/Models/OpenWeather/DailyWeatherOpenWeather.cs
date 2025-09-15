namespace previsao_tempo.Models.OpenWeather
{
    public class DailyWeatherOpenWeather
    {
        public long dt { get; set; }
        public MainOpenWeather main { get; set; }
        public List<WeatherOpenWeather> weather { get; set; }
    }
}
