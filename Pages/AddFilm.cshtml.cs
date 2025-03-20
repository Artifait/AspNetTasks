using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    [IgnoreAntiforgeryToken]
    public class AddFilmModel : PageModel
    {
        private readonly IFilmRepository _context;

        [BindProperty]
        public Film Film { get; set; }

        public AddFilmModel(IFilmRepository context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _context.AddFilmAsync(Film);
                return RedirectToPage("/Index"); // ѕосле добавлени€ возвращаемс€ на главную страницу
            }
            return Page();
        }
    }
}
