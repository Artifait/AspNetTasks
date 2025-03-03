namespace AspNetTasks.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; } 
        public DateOnly Birthday { get; set; }
    }
}
