using Microsoft.AspNetCore.Identity;

namespace previsao_tempo.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<CidadeFavorita>? CidadesFavoritas { get; set; }
    }
}
