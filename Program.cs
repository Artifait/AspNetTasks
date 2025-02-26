using AspNetTasks.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BookManager>(new BookManager("books.json"));
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
