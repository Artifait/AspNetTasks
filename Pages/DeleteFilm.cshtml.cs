using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    public class DeleteFilmModel : PageModel
    {
        private readonly CinemaContext _context;

        [BindProperty]
        public int FilmId { get; set; }

        public List<Film> Films { get; set; }

        public DeleteFilmModel(CinemaContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Films = _context.Films.ToList();
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
