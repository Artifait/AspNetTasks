using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Models
{
    public class FilmRepository : IFilmRepository
    {
        private readonly CinemaContext _context;

        public FilmRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task AddFilmAsync(Film film)
        {
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFilmAsync(int filmId)
        {
            var film = await _context.Films.FindAsync(filmId);
            if (film != null)
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Film>> GetAllFilmsAsync()
        {
            return await _context.Films.Include(f => f.Sessions).ToListAsync();
        }

        public async Task AddFilmSessionAsync(FilmSession session)
        {
            await _context.FilmSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFilmSessionAsync(int sessionId)
        {
            var session = await _context.FilmSessions.FindAsync(sessionId);
            if (session != null)
            {
                _context.FilmSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FilmSession>> GetAllSessionsAsync()
        {
            return await _context.FilmSessions.Include(s => s.Film).ToListAsync();
        }
    }
}
