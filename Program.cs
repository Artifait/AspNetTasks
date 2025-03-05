
using Newtonsoft.Json;
using AspNetTasks.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserDb>();
builder.Services.AddTransient<PasswordManager>();
builder.Services.AddTransient<AuthManager>();
builder.Services.AddScoped<LoggerAuthRequest>();

var app = builder.Build();

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

app.MapGet("/Auth/{name}&{password}", (string name, string password, AuthManager auther, LoggerAuthRequest logger) => {
    var result = auther.AuthUser(name, password, logger.LogString);

    if(result)
        logger.LogString($"Удачная попытка входа, под логином: {name}");

    return Results.Text(result ? "Добро пожаловать милорд." : "НЕТ ВЫ НЕ ПРАВЫ, либо пароль либо логин не верны.");
});

app.MapGet("/Reg/{name}&{password}", (string name, string password, UserDb db) => {
    return Results.Text(db.RegisterUser(name, password) ? "Всё мы вас зарегали." : "Не удалось зарегистрировать нового пользователя.");
});

app.Run();
