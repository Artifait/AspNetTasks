using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Models
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        { }

        public DbSet<Film> Films { get; set; }
        public DbSet<FilmSession> FilmSessions { get; set; }
    }
}
