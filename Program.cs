
using AspNetTasks.Application;
using AspNetTasks.DataAccess;
using AspNetTasks.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(options =>
    options.UseSqlite("Data Source=Cinema.db"));

builder.Services.AddScoped<IFilmRepository, FilmRepository>();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();
app.Run();
