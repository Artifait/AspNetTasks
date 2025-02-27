
using Newtonsoft.Json;

namespace AspNetTasks.Models
{
    public class BookManager
    {
        private readonly string _filePath;
        private List<Book> _books;

        public BookManager(string filePath)
        {
            _filePath = filePath;
            _books = [];
        }

        public void LoadBooks()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
            }
        }

        public void SaveBooks()
        {
            var json = JsonConvert.SerializeObject(_books, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public List<Book> GetBooks() => _books;

        public string GetBooksHtml()
        {
            var booksHtml = string.Empty;
            foreach (var book in _books)
            {
                booksHtml += $@"
            <div class='book'>
                <img src='{book.ImageUrl}' alt='{book.Title}'>
                <h2>{book.Title}</h2>
                <p>{book.Author}, {book.Year}</p>
                <a href='/book/{book.Id}' class='btn'>Подробнее</a>
                <a href='/edit/{book.Id}' class='btn'>Редактировать</a>
                <button onclick='deleteBook({book.Id})' class='btn'>Удалить</button>
            </div>";
            }
            return booksHtml;
        }

        public Book GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);

        public void AddBook(Book newBook)
        {
            newBook.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(newBook);
            SaveBooks();
        }

        public void EditBook(int id, Book updatedBook)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.Year = updatedBook.Year;
                book.Genre = updatedBook.Genre;
                book.Isbn = updatedBook.Isbn;
                book.ImageUrl = updatedBook.ImageUrl;
                SaveBooks();
            }
        }

        public void DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
                SaveBooks();
            }
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Isbn { get; set; }
        public string ImageUrl { get; set; }
    }
}
