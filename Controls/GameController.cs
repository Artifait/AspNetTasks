using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Controls
{
    public class GameController : Controller
    {
        private readonly ApplicationContext _context;

        public GameController(ApplicationContext context)
        {
            _context = context;
        }

        // Главная страница магазина игр
        public IActionResult Index()
        {
            var games = _context.Games.ToList();
            return View(games);
        }

        // Страница добавления игры
        [HttpGet]
        public IActionResult AddGame() => View();

        [HttpPost]
        public async Task<IActionResult> AddGame(Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(game);
        }

        // Страница редактирования игры
        [HttpGet]
        public async Task<IActionResult> EditGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> EditGame(int id, Game updatedGame)
        {
            if (id != updatedGame.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(updatedGame).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(updatedGame);
        }

        // Страница удаления игры
        [HttpGet]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Страница для отображения игры по ID
        public async Task<IActionResult> ShowGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }
    }
}
