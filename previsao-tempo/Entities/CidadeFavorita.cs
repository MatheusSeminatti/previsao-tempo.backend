using System.ComponentModel.DataAnnotations.Schema;

namespace previsao_tempo.Entities
{
    [Table("CidadeFavorita")]
    public class CidadeFavorita
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }
    }
}
