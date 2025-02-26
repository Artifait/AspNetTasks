using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTasks.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookManager _manager;

        public HomeController(BookManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var books = _manager.GetBooks();
            return View(books);
        }
    }
}
