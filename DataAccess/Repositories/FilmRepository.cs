using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetTasks.DataAccess.Repositories
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
            var film = await _context.Films.Include(f => f.Sessions).FirstOrDefaultAsync(f => f.Id == filmId);
            if (film != null)
            {
                // Удаляем все связанные сеансы перед удалением фильма
                _context.FilmSessions.RemoveRange(film.Sessions);
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
            var session = await _context.FilmSessions
                .Include(s => s.Seats)
                .Include(s => s.RowPositions)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session != null)
            {
                // Удаляем все связанные места и позиции рядов перед удалением сеанса
                _context.Seats.RemoveRange(session.Seats);
                _context.RowPositions.RemoveRange(session.RowPositions);
                _context.FilmSessions.Remove(session);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FilmSession>> GetAllSessionsAsync()
        {
            return await _context.FilmSessions
                .Include(s => s.Film)
                .Include(s => s.Seats)
                .Include(s => s.RowPositions)
                .ToListAsync();
        }

        public async Task<FilmSession?> GetFilmSessionAsync(int id)
        {
            return await _context.FilmSessions
                .Include(s => s.Film)
                .Include(s => s.Seats)
                .Include(s => s.RowPositions)
                .FirstOrDefaultAsync(s => s.Id == id);
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

        public async Task UpdateFilmSessionAsync(FilmSession session)
        {
            var existingSession = await _context.FilmSessions
                .Include(s => s.Seats)
                .Include(s => s.RowPositions)
                .FirstOrDefaultAsync(s => s.Id == session.Id);

            if (existingSession != null)
            {
                existingSession.StartTime = session.StartTime;
                existingSession.EndTime = session.EndTime;

                // Обновляем места (удаляем старые, добавляем новые)
                _context.Seats.RemoveRange(existingSession.Seats);
                await _context.Seats.AddRangeAsync(session.Seats);

                // Обновляем позиции рядов (удаляем старые, добавляем новые)
                _context.RowPositions.RemoveRange(existingSession.RowPositions);
                await _context.RowPositions.AddRangeAsync(session.RowPositions);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Film>> SearchFilmsAsync(Expression<Func<Film, bool>> filterExpression)
        {
            return await _context.Films
                .Include(f => f.Sessions)
                .Where(filterExpression)
                .ToListAsync();
        }

        public async Task<int> GetReservedSeatsCountAsync(int sessionId)
        {
            return await _context.Seats
                .Where(s => s.FilmSessionId == sessionId && s.IsReserved)
                .CountAsync();
        }
    }
}
