using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<Film?> GetFilmAsync(int id)
        {
            return await _context.Films
                                 .Include(f => f.Sessions)
                                 .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task UpdateFilmAsync(Film film)
        {
            var existingFilm = await _context.Films.FindAsync(film.Id);
            if (existingFilm != null)
            {
                existingFilm.Name = film.Name;
                existingFilm.FilmMaker = film.FilmMaker;
                existingFilm.Style = film.Style;
                existingFilm.Summary = film.Summary;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Film>> SearchFilmsAsync(Expression<Func<Film, bool>> filterExpression)
        {
            IQueryable<Film> query = _context.Films.Include(f => f.Sessions)
                                                   .Where(filterExpression);

            return await query.ToListAsync();
        }
    }
}
