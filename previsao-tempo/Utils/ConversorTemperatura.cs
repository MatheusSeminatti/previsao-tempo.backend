namespace previsao_tempo.Utils
{
    public static class ConversorTemperatura
    {
        public static decimal KelvinParaCelsius(decimal TemperaturaKelvin)
        {
            return TemperaturaKelvin - Convert.ToDecimal(273.15);
        }
    }
}
