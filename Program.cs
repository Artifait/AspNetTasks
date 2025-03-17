using AspNetTasks.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlite("Data Source=GameStore.db"));

builder.Services.AddScoped<IGameRepository, GameRepository>();

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
    // context.Database.Migrate(); // При необходимости раскомментировать миграцию
}

app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapGet("/games", async (string? author, string? genre, IGameRepository repository) =>
{
    var games = await repository.GetGamesAsync(author, genre);
    return Results.Ok(games);
});

app.MapGet("/games/{id}", async (int id, IGameRepository repository) =>
{
    var game = await repository.GetGameByIdAsync(id);

    if (game is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
});

app.MapPost("/games", async (Game game, IGameRepository repository) =>
{
    var addedGame = await repository.AddGameAsync(game);
    return Results.Created($"/games/{addedGame.Id}", addedGame);
});

app.MapPut("/games/{id}", async (HttpRequest request, int id, IGameRepository repository) =>
{
    var existingGame = await repository.GetGameByIdAsync(id);
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

    await repository.UpdateGameAsync(existingGame);
    return Results.Ok(existingGame);
});

app.MapDelete("/games/{id}", async (int id, IGameRepository repository) =>
{
    var game = await repository.GetGameByIdAsync(id);
    if (game is null)
    {
        return Results.NotFound();
    }
    await repository.DeleteGameAsync(game);
    return Results.NoContent();
});

app.MapPost("/upload", async (HttpRequest request, IGameRepository repository) =>
{
    var file = request.Form.Files.FirstOrDefault(); // Извлекаем файл из формы
    var name = request.Form["name"];
    var genre = request.Form["genre"];
    var author = request.Form["author"];

    if (file is null)
    {
        return Results.BadRequest("No file uploaded.");
    }

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

    var addedGame = await repository.AddGameAsync(game);
    return Results.Created($"/games/{addedGame.Id}", addedGame);
}).DisableAntiforgery();

app.Run();