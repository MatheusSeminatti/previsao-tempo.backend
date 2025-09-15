namespace previsao_tempo.Models.DTO
{
    public class ClimaDTO
    {
        public string Descricao { get; set; }
        public string Url_Icone { get; set; }
        public decimal Temperatura { get; set; }
        public decimal Temperatura_Minima { get; set; }
        public decimal Temperatura_Maxima { get; set; }
        public decimal Umidade { get; set; }
    }
}
