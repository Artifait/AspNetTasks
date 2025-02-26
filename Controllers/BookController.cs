using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTasks.Controllers
{
    public class BookController : Controller
    {
        private readonly BookManager _manager;

        public BookController(BookManager manager)
        {
            _manager = manager;
        }

        public IActionResult Details(int id)
        {
            var book = _manager.GetBookById(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                _manager.AddBook(book);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var book = _manager.GetBookById(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(int id, Book book)
        {
            if (ModelState.IsValid)
            {
                _manager.EditBook(id, book);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _manager.DeleteBook(id);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
