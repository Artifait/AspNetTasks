using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    [IgnoreAntiforgeryToken]
    public class EditFilmModel : PageModel
    {
        private readonly IFilmRepository _context;

        public EditFilmModel(IFilmRepository context)
        {
            _context = context;
        }

        [BindProperty]
        public Film Film { get; set; }

        public async Task OnGetAsync(int id)
        {
            Film = await _context.GetFilmAsync(id);
            if (Film == null)
            {
                RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateFilmAsync(Film);
            return RedirectToPage("/FilmDetails", new { id = Film.Id });
        }
    }
}
