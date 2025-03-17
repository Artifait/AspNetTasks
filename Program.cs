
using AspNetTasks.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlite("Data Source=GameStore.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; 
});

var app = builder.Build();

app.UseStaticFiles(); 


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    context.Database.EnsureCreated();
    //context.Database.Migrate();
}

app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapGet("/games", async (string? author, string? genre, GameStoreContext context) =>
{
    var games = context.Games.AsQueryable();

    if (!string.IsNullOrEmpty(author))
    {
        games = games.Where(g => g.Author.Equals(author));
    }

    if (!string.IsNullOrEmpty(genre))
    {
        games = games.Where(g => g.Genre.Equals(genre));
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

app.MapPut("/games/{id}", async (HttpRequest request, int id, GameStoreContext context) =>
{
    var existingGame = await context.Games.FindAsync(id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }

    var form = await request.ReadFormAsync();
    existingGame.Name = form["name"];
    existingGame.Genre = form["genre"];
    existingGame.Author = form["author"];

    var file = form.Files.FirstOrDefault();
    if (file != null)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        var newFileKey = Guid.NewGuid().ToString() + fileExtension;
        var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", newFileKey);

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        if (!string.IsNullOrEmpty(existingGame.FileKey))
        {
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", existingGame.FileKey);
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }
        }
        existingGame.FileKey = newFileKey;
    }

    await context.SaveChangesAsync();

    return Results.Ok(existingGame);
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

    var fileKey = Guid.NewGuid().ToString() + fileExtension;
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileKey);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var game = new Game
    {
        Name = name,      
        Genre = genre,     
        Author = author,   
        FileKey = fileKey, 
    };

    context.Games.Add(game);
    await context.SaveChangesAsync();

    return Results.Created($"/games/{game.Id}", game);
}).DisableAntiforgery();

app.Run();
