
using Newtonsoft.Json;
using AspNetTasks.Models;

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
    <title>Библиотека книг</title>
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>Библиотека книг</h1>
        <a href='/add' class='btn'>Добавить книгу</a>
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
                    alert('Ошибка при удалении книги');
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
        <p><strong>Автор:</strong> {book.Author}</p>
        <p><strong>Год выпуска:</strong> {book.Year}</p>
        <p><strong>Жанр:</strong> {book.Genre}</p>
        <p><strong>ISBN:</strong> {book.Isbn}</p>
        <img src='{book.ImageUrl}' alt='{book.Title}'>
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
    response.Headers.ContentType = "text/html; charset=utf-8";
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
            <div class='form-group'>
                <label for='imageUrl'>Ссылка на изображение:</label>
                <input type='url' id='imageUrl' name='ImageUrl' required>
            </div>
            <a href='/' class='btn'>Назад</a>
            <button type='submit' class='btn'>Добавить книгу</button>
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
                alert('Книга успешно добавлена!');
                window.location.href = '/';
            }} else {{
                alert('Ошибка при добавлении книги');
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
    <title>Редактировать книгу</title>
    {styleBlock}
</head>
<body>
    <div class='container'>
        <h1>Редактировать книгу</h1>
        <form id='editBookForm'>
            <div class='form-group'>
                <label for='title'>Название:</label>
                <input type='text' id='title' name='Title' value='{book.Title}' required>
            </div>
            <div class='form-group'>
                <label for='author'>Автор:</label>
                <input type='text' id='author' name='Author' value='{book.Author}' required>
            </div>
            <div class='form-group'>
                <label for='year'>Год выпуска:</label>
                <input type='number' id='year' name='Year' value='{book.Year}' required>
            </div>
            <div class='form-group'>
                <label for='genre'>Жанр:</label>
                <input type='text' id='genre' name='Genre' value='{book.Genre}' required>
            </div>
            <div class='form-group'>
                <label for='isbn'>ISBN:</label>
                <input type='text' id='isbn' name='Isbn' value='{book.Isbn}' required>
            </div>
            <div class='form-group'>
                <label for='imageUrl'>Ссылка на изображение:</label>
                <input type='url' id='imageUrl' name='ImageUrl' value='{book.ImageUrl}' required>
            </div>
            <a href='/' class='btn'>Назад</a>
            <button type='submit' class='btn'>Сохранить изменения</button>
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
                alert('Книга успешно обновлена!');
                window.location.href = '/';
            }} else {{
                alert('Ошибка при обновлении книги');
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
        await response.WriteAsync("Книга не найдена");
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
        await context.Response.WriteAsync("Неверные данные книги");
        return;
    }
    manager.AddBook(newBook);
    context.Response.StatusCode = 201;
    await context.Response.WriteAsync("Книга успешно добавлена");
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
        await context.Response.WriteAsync("Неверные данные книги");
        return;
    }

    manager.EditBook(bookId, updatedBook);
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Книга успешно обновлена");
}

async Task HandleDeleteBook(HttpContext context)
{
    var bookId = int.Parse(context.Request.RouteValues["id"]!.ToString()!);
    manager.DeleteBook(bookId);
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Книга успешно удалена");
}
