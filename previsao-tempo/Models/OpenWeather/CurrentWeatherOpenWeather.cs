namespace previsao_tempo.Models.OpenWeather
{
    public class CurrentWeatherOpenWeather
    {
        public CoordOpenWeather coord { get; set; }
        public List<WeatherOpenWeather> weather { get; set; }
        public MainOpenWeather main { get; set; }
    }
}
