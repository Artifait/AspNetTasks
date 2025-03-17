using AspNetTasks.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace AspNetTasks.Controls
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email и пароль обязательны");
                return View();
            }
            // Вычисляем хэш введённого пароля
            var passwordHash = ComputeSha256Hash(password);
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null || user.PasswordHash != passwordHash)
            {
                ModelState.AddModelError("", "Неверные учётные данные");
                return View();
            }

            // Формирование набора клаймов
            var claims = new List<Claim>
              {
                   new Claim(ClaimTypes.Name, user.Email),
                   new Claim(ClaimTypes.Role, user.Role)
              };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email и пароль обязательны");
                return View();
            }
            var passwordHash = ComputeSha256Hash(password);
            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                Role = "user" // стандартная роль для новых пользователей
            };
            await _userService.CreateUserAsync(user);
            return RedirectToAction("Login");
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Простой пример страницы AccessDenied
        public IActionResult AccessDenied()
        {
            return Content("Access Denied");
        }

        // Метод вычисления SHA256 хэша
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
