namespace AspNetTasks.DataAccess.Entities
{
    public class FilmSession
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public Film? Film { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
