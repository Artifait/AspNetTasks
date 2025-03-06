
using Newtonsoft.Json;
using AspNetTasks.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
    var manager = new BookManager("books.json");
    manager.LoadBooks();
    return manager;
});

var app = builder.Build();

app.UseCustomHeaders(); 

app.UseWhen(ctx => ctx.Request.Path.Equals("/") &&
                   ctx.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase),
    appBuilder => { appBuilder.UseHomePage(); });

app.UseWhen(ctx => ctx.Request.Path.Equals("/books") &&
                   ctx.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase),
    appBuilder => { appBuilder.UseBooksList(); });

app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments("/book") &&
                   ctx.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase),
    appBuilder => { appBuilder.UseBookDetail(); });

app.UseWhen(ctx => ctx.Request.Path.Equals("/books/add") &&
                   ctx.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase),
    appBuilder => { appBuilder.UseAddBookForm(); });

app.UseWhen(ctx => ctx.Request.Path.Equals("/books/add") &&
                   ctx.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase),
    appBuilder => { appBuilder.UseAddBook(); });

app.Run();

public static class HtmlTemplates
{
    public static string StyleBlock = @"
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
}

public static class LibraryMiddlewareExtensions
{
    public static IApplicationBuilder UseHomePage(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HomePageMiddleware>();
    }
    public static IApplicationBuilder UseBooksList(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BooksListMiddleware>();
    }
    public static IApplicationBuilder UseBookDetail(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BookDetailMiddleware>();
    }
    public static IApplicationBuilder UseAddBookForm(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AddBookFormMiddleware>();
    }
    public static IApplicationBuilder UseAddBook(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AddBookMiddleware>();
    }
}

public class HomePageMiddleware
{
    private readonly RequestDelegate _next;
    public HomePageMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        string html = $@"<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Добро пожаловать</title>
    {HtmlTemplates.StyleBlock}
</head>
<body>
    <div class='container'>
        <h1>Добро пожаловать в онлайн библиотеку</h1>
        <a href='/books' class='btn'>Список книг</a>
        <a href='/books/add' class='btn'>Добавить книгу</a>
    </div>
</body>
</html>";
        await context.Response.WriteAsync(html);
    }
}

public class BooksListMiddleware
{
    private readonly RequestDelegate _next;
    private readonly BookManager _manager;
    public BooksListMiddleware(RequestDelegate next, BookManager manager)
    {
        _next = next;
        _manager = manager;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        var booksHtml = _manager.GetBooksHtml(); 
        string html = $@"<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Список книг</title>
    {HtmlTemplates.StyleBlock}
</head>
<body>
    <div class='container'>
        <h1>Список книг</h1>
        <a href='/' class='btn'>Главная</a>
        <div class='book-list'>
            {booksHtml}
        </div>
    </div>
    <script>
        async function deleteBook(bookId) {{
            if (confirm('Вы уверены, что хотите удалить эту книгу?')) {{
                const response = await fetch('/api/book/' + bookId, {{ method: 'DELETE' }});
                if (response.ok) {{
                    alert('Книга успешно удалена!');
                    window.location.href = '/';
                }} else {{
                    alert('Зачем вы это делаете???');
                }}
            }}
        }}
    </script>
</body>
</html>";
        await context.Response.WriteAsync(html);
    }
}

public class BookDetailMiddleware
{
    private readonly RequestDelegate _next;
    private readonly BookManager _manager;
    public BookDetailMiddleware(RequestDelegate next, BookManager manager)
    {
        _next = next;
        _manager = manager;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value; // Ожидается, например, /book/1
        if (path != null && path.StartsWith("/book/"))
        {
            var idPart = path.Substring("/book/".Length);
            if (int.TryParse(idPart, out int bookId))
            {
                var book = _manager.GetBookById(bookId);
                if (book != null)
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    string html = $@"<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{book.Title}</title>
    {HtmlTemplates.StyleBlock}
</head>
<body>
    <div class='container'>
        <h1>{book.Title}</h1>
        <p><strong>Автор:</strong> {book.Author}</p>
        <p><strong>Год выпуска:</strong> {book.Year}</p>
        <p><strong>Жанр:</strong> {book.Genre}</p>
        <p><strong>ISBN:</strong> {book.Isbn}</p>
        <img src='{book.ImageUrl}' alt='{book.Title}'>
        <a href='/books' class='btn'>Назад</a>
    </div>
</body>
</html>";
                    await context.Response.WriteAsync(html);
                    return;
                }
            }
        }
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Книга не найдена");
    }
}

public class AddBookFormMiddleware
{
    private readonly RequestDelegate _next;
    public AddBookFormMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        string html = $@"<!DOCTYPE html>
<html lang='ru'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Добавить книгу</title>
    {HtmlTemplates.StyleBlock}
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
            <div class='form-group'>
                <label for='imageUrl'>Ссылка на изображение:</label>
                <input type='url' id='imageUrl' name='ImageUrl' required>
            </div>
            <a href='/' class='btn'>Назад</a>
            <button type='submit' class='btn'>Добавить книгу</button>
        </form>
    </div>
    <script>
        document.getElementById('addBookForm').addEventListener('submit', async function(e) {{
            e.preventDefault();
            const formData = {{
                Title: document.getElementById('title').value,
                Author: document.getElementById('author').value,
                Year: parseInt(document.getElementById('year').value),
                Genre: document.getElementById('genre').value,
                Isbn: document.getElementById('isbn').value,
                ImageUrl: document.getElementById('imageUrl').value
            }};
            const response = await fetch('/books/add', {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify(formData)
            }});
            if (response.ok) {{
                alert('Книга успешно добавлена!');
                window.location.href = '/books';
            }} else {{
                alert('Ошибка при добавлении книги');
            }}
        }});
    </script>
</body>
</html>";
        await context.Response.WriteAsync(html);
    }
}

public class AddBookMiddleware
{
    private readonly RequestDelegate _next;
    private readonly BookManager _manager;
    public AddBookMiddleware(RequestDelegate next, BookManager manager)
    {
        _next = next;
        _manager = manager;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        try
        {
            var newBook = JsonConvert.DeserializeObject<Book>(body);
            if (newBook == null)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = "Неверные данные книги" }));
                return;
            }
            _manager.AddBook(newBook);
            context.Response.StatusCode = 201;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "Книга успешно добавлена" }));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = ex.Message }));
        }
    }
}