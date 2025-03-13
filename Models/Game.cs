using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilmMaker { get; set; }
        public string Style { get; set; }
        public string Summary { get; set; }

        public List<FilmSession> Sessions { get; set; } = new List<FilmSession>();
    }

    public class FilmSession
    { 
        public int Id { get; set; }
        public int FilmId { get; set; }
        public Film? Film { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        { }

        public DbSet<Film> Films { get; set; }
        public DbSet<FilmSession> FilmSessions { get; set; }

        public async Task AddFilmAsync(Film film)
        {
            await Films.AddAsync(film);
            await SaveChangesAsync();
        }

        public async Task RemoveFilmAsync(int filmId)
        {
            var film = await Films.FindAsync(filmId);
            if (film != null)
            {
                Films.Remove(film);
                await SaveChangesAsync();
            }
        }

        public async Task AddFilmSessionAsync(FilmSession session)
        {
            await FilmSessions.AddAsync(session);
            await SaveChangesAsync();
        }

        public async Task RemoveFilmSessionAsync(int sessionId)
        {
            var session = await FilmSessions.FindAsync(sessionId);
            if (session != null)
            {
                FilmSessions.Remove(session);
                await SaveChangesAsync();
            }
        }
    }
}
