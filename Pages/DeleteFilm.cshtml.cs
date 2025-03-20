using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    [IgnoreAntiforgeryToken]
    public class DeleteFilmModel : PageModel
    {
        private readonly IFilmRepository _context;

        [BindProperty]
        public int FilmId { get; set; }

        public List<Film> Films { get; set; }

        public DeleteFilmModel(IFilmRepository context)
        {
            _context = context;
        }

        public async void OnGet()
        {
            Films = (await _context.GetAllFilmsAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FilmId > 0)
            {
                await _context.RemoveFilmAsync(FilmId);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
