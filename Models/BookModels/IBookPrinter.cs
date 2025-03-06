
namespace AspNetTasks.Models.BookModels
{
    public interface IBookPrinter
    {
        void Print(Book book);
    }

    public class BookFilePrinter : IBookPrinter
    {
        public void Print(Book book)
        {
            Console.WriteLine($"Book \"{book.Title}\" by {book.Author} printed to file");
        }
    }

    public class BookScreenPrinter : IBookPrinter
    {
        public void Print(Book book)
        {
            Console.WriteLine($"Book \"{book.Title}\" by {book.Author} printed to screen");
        }
    }
}
