using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Pages
{
    public class DeleteSessionModel : PageModel
    {
        private readonly CinemaContext _context;

        [BindProperty]
        public int SessionId { get; set; }

        public List<FilmSession> Sessions { get; set; }

        public DeleteSessionModel(CinemaContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Sessions = _context.FilmSessions.Include(s => s.Film).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SessionId > 0)
            {
                await _context.RemoveFilmSessionAsync(SessionId);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
