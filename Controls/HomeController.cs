using AspNetTasks.Models;
using AspNetTasks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AspNetTasks.Controls
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        // Главная страница
        public IActionResult Index()
        {
            // Передаем в ViewData информацию о текущем пользователе и статусе сессии
            ViewData["IsAuthenticated"] = HttpContext.Session.GetInt32("UserId") != null;
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            return View();
        }

        // Страница регистрации
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Username == model.Username);
                if (userExists)
                {
                    ModelState.AddModelError(string.Empty, "Username already exists.");
                    return View(model);
                }

                // Хешируем пароль
                var passwordHash = HashPassword(model.Password);

                var user = new User
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    PasswordHash = passwordHash
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // Страница логина
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null && VerifyPassword(model.Password, user.PasswordHash))
                {
                    // Успешный вход, сохраняем сессию
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username); // Сохраняем имя пользователя в сессии
                    return RedirectToAction("Index", "Game");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        // Логаут
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }

}
