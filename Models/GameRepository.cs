using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Models
{
    public class GameRepository : IGameRepository
    {
        private readonly GameStoreContext _context;

        public GameRepository(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetGamesAsync(string? author, string? genre)
        {
            var query = _context.Games.AsQueryable();

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(g => g.Author.Equals(author));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(g => g.Genre.Equals(genre));
            }

            return await query.ToListAsync();
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<Game> AddGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game> UpdateGameAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task DeleteGameAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
}
