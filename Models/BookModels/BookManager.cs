
namespace AspNetTasks.Models.BookModels
{
    public class BookManager
    {
        private List<Book> _books;
        private readonly BookViewer _viewer;
        private readonly IBooksExtractorFromText _extractor;

        public BookManager(IBooksExtractorFromText extractor, IBookPrinter printer)
        {
            _viewer = new BookViewer(printer);
            _extractor = extractor;
            _books = new List<Book>();
        }

        public void LoadBooks(string filePath)
        {
            string fileText = File.ReadAllText(filePath);
            _books = _extractor.Extract(fileText);
        }

        public void ViewAllBooks()
        {
            _books.ForEach(_viewer.View);
        }
    }
}
