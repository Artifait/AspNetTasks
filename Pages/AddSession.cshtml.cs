using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    [IgnoreAntiforgeryToken]
    public class AddSessionModel : PageModel
    {
        private readonly IFilmRepository _context;

        [BindProperty]
        public FilmSession FilmSession { get; set; }

        public List<Film> Films { get; set; }

        public AddSessionModel(IFilmRepository context)
        {
            _context = context;
        }

        public async void OnGet()
        {
            Films = (await _context.GetAllFilmsAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _context.AddFilmSessionAsync(FilmSession);
                return RedirectToPage("/Index"); 
            }
            return Page();
        }
    }
}
