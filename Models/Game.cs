using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public string FileKey { get; set; }     
    }

    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options)
            : base(options)
        { }

        public DbSet<Game> Games { get; set; }
    }
}
