namespace AspNetTasks.DataAccess.Entities
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
}
