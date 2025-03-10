using AspNetTasks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlite("Data Source=GameStore.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery(options =>
{
    // Настройка политики, если требуется
    options.HeaderName = "X-CSRF-TOKEN"; // имя заголовка для передачи токена
});

var app = builder.Build();

// Настроим обработку статичных файлов
app.UseStaticFiles(); // Теперь все статические файлы будут обслуживаться из wwwroot


// Автоматическое создание базы данных при старте приложения
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    context.Database.EnsureCreated();
    context.Database.Migrate();
}

app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapGet("/games", async (string? author, string? genre, GameStoreContext context) =>
{
    var games = context.Games.AsQueryable();

    if (!string.IsNullOrEmpty(author))
    {
        games = games.Where(g => g.Author.Contains(author));
    }

    if (!string.IsNullOrEmpty(genre))
    {
        games = games.Where(g => g.Genre.Contains(genre));
    }

    return Results.Ok(await games.ToListAsync());
});

app.MapGet("/games/{id}", async (int id, GameStoreContext context) =>
{
    var game = await context.Games.FindAsync(id);

    if (game is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
});

app.MapPost("/games", async (Game game, GameStoreContext context) =>
{
    context.Games.Add(game);
    await context.SaveChangesAsync();
    return Results.Created($"/games/{game.Id}", game);
});

app.MapPut("/games/{id}", async (int id, Game game, GameStoreContext context) =>
{
    var existingGame = await context.Games.FindAsync(id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    existingGame.Name = game.Name;
    existingGame.Genre = game.Genre;
    existingGame.Author = game.Author;
    existingGame.FileKey = game.FileKey;

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/games/{id}", async (int id, GameStoreContext context) =>
{
    var game = await context.Games.FindAsync(id);

    if (game is null)
    {
        return Results.NotFound();
    }

    context.Games.Remove(game);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPost("/upload", async (HttpRequest request, GameStoreContext context) =>
{
    var file = request.Form.Files.FirstOrDefault(); // Извлекаем файл из формы
    var name = request.Form["name"]; // Извлекаем название игры
    var genre = request.Form["genre"]; // Извлекаем жанр игры
    var author = request.Form["author"]; // Извлекаем автора игры

    if (file is null)
    {
        return Results.BadRequest("No file uploaded.");
    }

    // Получаем расширение файла
    var fileExtension = Path.GetExtension(file.FileName);

    // Генерация уникального ключа для файла с расширением
    var fileKey = Guid.NewGuid().ToString() + fileExtension;
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileKey);

    // Сохранение файла на сервере
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    // Создание новой записи в базе данных для игры
    var game = new Game
    {
        Name = name,       // Название игры из формы
        Genre = genre,     // Жанр игры из формы
        Author = author,   // Автор игры из формы
        FileKey = fileKey, // Уникальный ключ для файла с расширением
    };

    context.Games.Add(game);
    await context.SaveChangesAsync();

    return Results.Created($"/games/{game.Id}", game);
}).DisableAntiforgery();

app.Run();
