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
}
