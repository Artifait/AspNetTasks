
namespace AspNetTasks.Models.BookModels
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string AdditionalInfo { get; set; }

        public Book(string title, string author, string genre, int year, string additionalInfo)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            AdditionalInfo = additionalInfo;
        }
    }
}
