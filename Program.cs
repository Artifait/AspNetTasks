using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var manager = new BookManager("books.json");
var app = builder.Build();

manager.LoadBooks();

string styleBlock = @"
<style>
    body {
        background-color: #121212;
        color: #ffffff;
        font-family: Arial, sans-serif;
        margin: 0;
        padding: 20px;
    }
    h1 {
        margin-bottom: 20px;
    }
    .container {
        max-width: 800px;
        margin: 0 auto;
    }
    .book-list {
        display: flex;
        margin-top: 30px;
        flex-wrap: wrap;
        gap: 20px;
        
    }
    .book {
        background-color: #1e1e1e;
        border: 1px solid #333;
        padding: 15px;
        border-radius: 4px;
        width: 200px;
        margin-bottom: 20px;
    }
    a {
        color: #BB86FC;
        text-decoration: none;
    }
    a:hover {
        text-decoration: underline;
    }
    form {
        background-color: #1e1e1e;
        padding: 20px;
        border-radius: 4px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
    }
    label {
        display: block;
        margin-bottom: 5px;
        font-weight: bold;
    }
    input, textarea, select {
        background-color: #333;
        color: #fff;
        border: 1px solid #555;
        padding: 10px;
        border-radius: 4px;
        width: 100%;
        box-sizing: border-box;
        margin-bottom: 10px;
    }
    .btn {
        background-color: #6200ee;
        color: #fff;
        border: none;
        padding: 10px 20px;
        border-radius: 4px;
        cursor: pointer;
        margin-top: 10px;
        margin-bottom: 20px;
    }
    .form-group {
        margin-bottom: 20px;
    }
</style>
";

app.MapGet("/", HandleRequestHome);
app.MapGet("/book/{id:int}", HandleRequestBook);
app.MapGet("/add", HandleRequestAddBookForm);

// API для добавления новой книги (принимает JSON)
app.MapPost("/api/book", HandlePostAddBook);

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
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>Список книг</h1>
        <a href='/add' class='btn'>Добавить книгу</a>
        <div class='book-list'>
            {booksHtml}
        </div>
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

    var bookId = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
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
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>{book.Title}</h1>
        <p><strong>Автор:</strong> {book.Author}</p>
        <p><strong>Год выпуска:</strong> {book.Year}</p>
        <p><strong>Жанр:</strong> {book.Genre}</p>
        <p><strong>ISBN:</strong> {book.Isbn}</p>
        <a href='/' class='btn'>Назад</a>
    </div>
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

async Task HandleRequestAddBookForm(HttpContext context)
{
    var response = context.Response;
    response.Headers.ContentLanguage = "ru-RU";
    response.Headers.ContentType = "text/html; charset=utf-8";

    var funcBody = @"{e.preventDefault();
            const formData = {
                Title: document.getElementById('title').value,
                Author: document.getElementById('author').value,
                Year: parseInt(document.getElementById('year').value),
                Genre: document.getElementById('genre').value,
                Isbn: document.getElementById('isbn').value
            };
            const response = await fetch('/api/book', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });
            if (response.ok) {
                alert('Книга успешно добавлена!');
                window.location.href = '/';
            } else {
                alert('Ошибка при добавлении книги');
            }
        }";
    var html = $@"
<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Добавить книгу</title>
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>Добавить новую книгу</h1>
        <form id='addBookForm'>
            <div class='form-group'>
                <label for='title'>Название:</label>
                <input type='text' id='title' name='Title' required>
            </div>
            <div class='form-group'>
                <label for='author'>Автор:</label>
                <input type='text' id='author' name='Author' required>
            </div>
            <div class='form-group'>
                <label for='year'>Год выпуска:</label>
                <input type='number' id='year' name='Year' required>
            </div>
            <div class='form-group'>
                <label for='genre'>Жанр:</label>
                <input type='text' id='genre' name='Genre' required>
            </div>
            <div class='form-group'>
                <label for='isbn'>ISBN:</label>
                <input type='text' id='isbn' name='Isbn' required>
            </div>
            <a href='/' class='btn'>Назад</a>
            <button type='submit' class='btn'>Добавить книгу</button>
        </form>
    </div>
    <script>
        document.getElementById('addBookForm').addEventListener('submit', async function(e) {funcBody});
    </script>
</body>
</html>";
    await response.WriteAsync(html);
}

async Task HandlePostAddBook(HttpContext context)
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var newBook = JsonConvert.DeserializeObject<Book>(body);
    if (newBook == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Неверные данные книги");
        return;
    }
    manager.AddBook(newBook);
    context.Response.StatusCode = 201;
    await context.Response.WriteAsync("Книга успешно добавлена");
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
                <h2>{book.Title}</h2>
                <p>{book.Author}, {book.Year}</p>
                <a href='/book/{book.Id}'>Подробнее</a>
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
