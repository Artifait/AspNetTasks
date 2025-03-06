
namespace AspNetTasks.Models.BookModels
{
    public class BookViewer
    {
        private readonly IBookPrinter _printer;

        public BookViewer(IBookPrinter printer)
        {
            _printer = printer;
        }

        public void View(Book book)
        {
            _printer.Print(book);
        }
    }
}
