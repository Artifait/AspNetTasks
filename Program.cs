using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var manager = new BookManager("books.json");
var app = builder.Build();

manager.LoadBooks();

string styleBlock = @"
<style>
    body {
        background-color: #121212;
        color: #ffffff;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        margin: 0;
        padding: 20px;
        box-sizing: border-box;
    }
    h1 {
        margin-bottom: 20px;
        font-size: 2em;
        text-align: center;
    }
    .container {
        max-width: 1200px;
        margin: 0 auto;
        padding: 20px;
    }
    .book-list {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: 20px;
        margin-top: 30px;
    }
    .book {
        background-color: #1e1e1e;
        border: 1px solid #333;
        padding: 15px;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
        display: flex;
        flex-direction: column;
        align-items: center;
        transition: transform 0.2s;
    }
    .book:hover {
        transform: scale(1.03);
    }
    .book img {
        max-width: 100%;
        height: auto;
        border-radius: 4px;
        margin-bottom: 10px;
    }
    a {
        color: #BB86FC;
        text-decoration: none;
    }
    a:hover {
        text-decoration: underline;
    }
    .btn {
        background-color: #6200ee;
        color: #fff;
        border: none;
        padding: 10px 20px;
        border-radius: 4px;
        cursor: pointer;
        text-align: center;
        display: inline-block;
        margin: 10px 0;
    }
    form {
        background-color: #1e1e1e;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
        width: 100%;
        max-width: 600px;
        margin: 20px auto;
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
        margin-bottom: 15px;
    }
    .form-group {
        margin-bottom: 20px;
    }
    @media (max-width: 768px) {
        .container {
            padding: 10px;
        }
        form {
            padding: 15px;
        }
    }
</style>
";

app.UseCustomHeaders();

app.MapGet("/", HandleRequestHome);
app.MapGet("/book/{id:int}", HandleRequestBook);
app.MapGet("/add", HandleRequestAddBookForm);
app.MapGet("/edit/{id:int}", HandleRequestEditBookForm);

app.MapPost("/api/book", HandlePostAddBook);
app.MapPost("/api/book/{id:int}", HandlePostEditBook);
app.MapDelete("/api/book/{id:int}", HandleDeleteBook);

app.Run();

async Task HandleRequestHome(HttpContext context)
{
    var response = context.Response;
    var booksHtml = manager.GetBooksHtml();
    var html = $@"
<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>���������� ����</title>
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>���������� ����</h1>
        <a href='/add' class='btn'>�������� �����</a>
        <div class='book-list'>
            {booksHtml}
        </div>
    </div>
    <script>
        async function deleteBook(bookId) {{
            if (confirm('�� �������, ��� ������ ������� ��� �����?')) {{
                const response = await fetch('/api/book/' + bookId, {{ method: 'DELETE' }});
                if (response.ok) {{
                    alert('����� ������� �������!');
                    window.location.href = '/';
                }} else {{
                    alert('������ ��� �������� �����');
                }}
            }}
        }}
    </script>
</body>
</html>";
    await response.WriteAsync(html);
}

async Task HandleRequestBook(HttpContext context)
{
    var response = context.Response;
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
        <p><strong>�����:</strong> {book.Author}</p>
        <p><strong>��� �������:</strong> {book.Year}</p>
        <p><strong>����:</strong> {book.Genre}</p>
        <p><strong>ISBN:</strong> {book.Isbn}</p>
        <img src='{book.ImageUrl}' alt='{book.Title}'>
        <a href='/' class='btn'>�����</a>
    </div>
</body>
</html>";
        await response.WriteAsync(html);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsync("����� �� �������");
    }
}

async Task HandleRequestAddBookForm(HttpContext context)
{
    var response = context.Response;
    response.Headers.ContentType = "text/html; charset=utf-8";
    var html = $@"
<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>�������� �����</title>
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>�������� ����� �����</h1>
        <form id='addBookForm'>
            <div class='form-group'>
                <label for='title'>��������:</label>
                <input type='text' id='title' name='Title' required>
            </div>
            <div class='form-group'>
                <label for='author'>�����:</label>
                <input type='text' id='author' name='Author' required>
            </div>
            <div class='form-group'>
                <label for='year'>��� �������:</label>
                <input type='number' id='year' name='Year' required>
            </div>
            <div class='form-group'>
                <label for='genre'>����:</label>
                <input type='text' id='genre' name='Genre' required>
            </div>
            <div class='form-group'>
                <label for='isbn'>ISBN:</label>
                <input type='text' id='isbn' name='Isbn' required>
            </div>
            <div class='form-group'>
                <label for='imageUrl'>������ �� �����������:</label>
                <input type='url' id='imageUrl' name='ImageUrl' required>
            </div>
            <a href='/' class='btn'>�����</a>
            <button type='submit' class='btn'>�������� �����</button>
        </form>
    </div>
    <script>
        document.getElementById('addBookForm').addEventListener('submit', async function(e) {{e.preventDefault();
            const formData = {{
                Title: document.getElementById('title').value,
                Author: document.getElementById('author').value,
                Year: parseInt(document.getElementById('year').value),
                Genre: document.getElementById('genre').value,
                Isbn: document.getElementById('isbn').value,
                ImageUrl: document.getElementById('imageUrl').value
            }};
            const response = await fetch('/api/book', {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify(formData)
            }});
            if (response.ok) {{
                alert('����� ������� ���������!');
                window.location.href = '/';
            }} else {{
                alert('������ ��� ���������� �����');
            }}
        }});
    </script>
</body>
</html>";
    await response.WriteAsync(html);
}

async Task HandleRequestEditBookForm(HttpContext context)
{
    var response = context.Response;
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
    <title>������������� �����</title>
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>������������� �����</h1>
        <form id='editBookForm'>
            <div class='form-group'>
                <label for='title'>��������:</label>
                <input type='text' id='title' name='Title' value='{book.Title}' required>
            </div>
            <div class='form-group'>
                <label for='author'>�����:</label>
                <input type='text' id='author' name='Author' value='{book.Author}' required>
            </div>
            <div class='form-group'>
                <label for='year'>��� �������:</label>
                <input type='number' id='year' name='Year' value='{book.Year}' required>
            </div>
            <div class='form-group'>
                <label for='genre'>����:</label>
                <input type='text' id='genre' name='Genre' value='{book.Genre}' required>
            </div>
            <div class='form-group'>
                <label for='isbn'>ISBN:</label>
                <input type='text' id='isbn' name='Isbn' value='{book.Isbn}' required>
            </div>
            <div class='form-group'>
                <label for='imageUrl'>������ �� �����������:</label>
                <input type='url' id='imageUrl' name='ImageUrl' value='{book.ImageUrl}' required>
            </div>
            <a href='/' class='btn'>�����</a>
            <button type='submit' class='btn'>��������� ���������</button>
        </form>
    </div>
    <script>
        document.getElementById('editBookForm').addEventListener('submit', async function(e) {{e.preventDefault();
            const formData = {{
                Title: document.getElementById('title').value,
                Author: document.getElementById('author').value,
                Year: parseInt(document.getElementById('year').value),
                Genre: document.getElementById('genre').value,
                Isbn: document.getElementById('isbn').value,
                ImageUrl: document.getElementById('imageUrl').value
            }};
            const response = await fetch('/api/book/{book.Id}', {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify(formData)
            }});
            if (response.ok) {{
                alert('����� ������� ���������!');
                window.location.href = '/';
            }} else {{
                alert('������ ��� ���������� �����');
            }}
        }});
    </script>
</body>
</html>";
        await response.WriteAsync(html);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsync("����� �� �������");
    }
}

async Task HandlePostAddBook(HttpContext context)
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var newBook = JsonConvert.DeserializeObject<Book>(body);
    if (newBook == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("�������� ������ �����");
        return;
    }
    manager.AddBook(newBook);
    context.Response.StatusCode = 201;
    await context.Response.WriteAsync("����� ������� ���������");
}

async Task HandlePostEditBook(HttpContext context)
{
    var bookId = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var updatedBook = JsonConvert.DeserializeObject<Book>(body);

    if (updatedBook == null)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("�������� ������ �����");
        return;
    }

    manager.EditBook(bookId, updatedBook);
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("����� ������� ���������");
}

async Task HandleDeleteBook(HttpContext context)
{
    var bookId = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
    manager.DeleteBook(bookId);
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("����� ������� �������");
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
                <img src='{book.ImageUrl}' alt='{book.Title}'>
                <h2>{book.Title}</h2>
                <p>{book.Author}, {book.Year}</p>
                <a href='/book/{book.Id}' class='btn'>���������</a>
                <a href='/edit/{book.Id}' class='btn'>�������������</a>
                <button onclick='deleteBook({book.Id})' class='btn'>�������</button>
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

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomHeaders(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            context.Response.Headers.ContentLanguage = "ru-RU";
            context.Response.Headers.ContentType = "text/html; charset=utf-8";
            await next();
        });
    }
}
