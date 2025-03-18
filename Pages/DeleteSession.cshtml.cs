using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    [IgnoreAntiforgeryToken]
    public class DeleteSessionModel : PageModel
    {
        private readonly IFilmRepository _context;

        [BindProperty]
        public int SessionId { get; set; }

        public List<FilmSession> Sessions { get; set; }

        public DeleteSessionModel(IFilmRepository context)
        {
            _context = context;
        }

        public async void OnGet()
        {
            Sessions =  (await _context.GetAllSessionsAsync()).ToList();
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
