using Microsoft.EntityFrameworkCore;
using previsao_tempo.Entities;

namespace previsao_tempo.Data.Repositories
{
    public class CidadesFavoritasRepository : ICidadesFavoritasRepository
    {
        private readonly AppDbContext _context;
        public CidadesFavoritasRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task AdicionarAsync(CidadeFavorita CidadeFavorita)
        {
            await _context.CidadesFavoritas
                .AddAsync(CidadeFavorita);

            await _context.SaveChangesAsync();
        }

        public async Task<List<CidadeFavorita>> GetAsync(string UsuarioId)
        {
            return await _context.CidadesFavoritas
                .Where(x => x.UsuarioId == UsuarioId)
                .ToListAsync();
        }

        public async Task RemoverAsync(int Id)
        {
            await _context.CidadesFavoritas
                .Where(x => x.Id == Id)
                .ExecuteDeleteAsync();
        }
    }

    public interface ICidadesFavoritasRepository
    {
        Task<List<CidadeFavorita>> GetAsync(string UsuarioId);
        Task AdicionarAsync(CidadeFavorita CidadeFavorita);
        Task RemoverAsync(int Id);
    }
}
