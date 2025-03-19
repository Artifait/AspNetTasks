namespace AspNetTasks.Models
{
    public class SearchFilter
    {
        public string? Name { get; set; }
        public string? FilmMaker { get; set; }
        public string? Style { get; set; }
        public string? Summary { get; set; }
        public DateTime? SessionStartDate { get; set; }
        public DateTime? SessionEndDate { get; set; }
    }
}
