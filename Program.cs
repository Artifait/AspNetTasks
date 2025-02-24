using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var manager = new BookManager("books.json");
var app = builder.Build();

manager.LoadBooks();

app.MapGet("/", HandleRequestHome);
app.MapGet("/book/{id:int}", HandleRequestBook);
app.Run();

async Task HandleRequestHome(HttpContext context)
{
    var response = context.Response;
    response.Headers.ContentLanguage = "ru-RU";
    response.Headers.ContentType = "text/html; charset=utf-8";

    var booksHtml = manager.GetBooksHtml();
    var html = $@"
    <!DOCTYPE html>
    <html lang='ru'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Список книг</title>
        <link rel='stylesheet' href='/css/styles.css'>
    </head>
    <body>
        <h1>Список книг</h1>
        <div class='book-list'>
            {booksHtml}
        </div>
    </body>
    </html>";

    await response.WriteAsync(html);
}

async Task HandleRequestBook(HttpContext context)
{
    var response = context.Response;
    response.Headers.ContentLanguage = "ru-RU";
    response.Headers.ContentType = "text/html; charset=utf-8";

    var bookId = int.Parse(context.Request.RouteValues["id"].ToString()!);
    var book = manager.GetBookById(bookId);

    if (book != null)
    {
        var html = $@"
        <!DOCTYPE html>
        <html lang='ru'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>{book.Title}</title>
            <link rel='stylesheet' href='/css/styles.css'>
        </head>
        <body>
            <h1>{book.Title}</h1>
            <p><strong>Автор:</strong> {book.Author}</p>
            <p><strong>Год выпуска:</strong> {book.Year}</p>
            <p><strong>Жанр:</strong> {book.Genre}</p>
            <p><strong>ISBN:</strong> {book.Isbn}</p>
            <a href='/'>Назад</a>
        </body>
        </html>";

        await response.WriteAsync(html);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsync("Книга не найдена");
    }
}

public class BookManager
{
    private readonly string _filePath;
    private List<Book> _books;

    public BookManager(string filePath)
    {
        _filePath = filePath;
        _books = new List<Book>();
    }

    public void LoadBooks()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
        }
    }

    public List<Book> GetBooks() => _books;

    public string GetBooksHtml()
    {
        var booksHtml = string.Empty;
        foreach (var book in _books)
        {
            booksHtml += $@"
            <div class='book'>
                <h2>{book.Title}</h2>
                <p>{book.Author}, {book.Year}</p>
                <a href='/book/{book.Id}'>Подробнее</a>
            </div>";
        }
        return booksHtml;
    }

    public Book GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public string Isbn { get; set; }
}
