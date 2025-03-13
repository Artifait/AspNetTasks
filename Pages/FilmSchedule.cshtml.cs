using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Pages
{
    public class FilmScheduleModel : PageModel
    {
        private readonly CinemaContext _context;

        public FilmScheduleModel(CinemaContext context)
        {
            _context = context;
        }

        // Список фильмов с сеансами
        public List<Film> Films { get; set; } = new List<Film>();

        public async Task OnGetAsync()
        {
            // Загрузка фильмов с их сеансами
            Films = await _context.Films
                .Include(f => f.Sessions)
                .ToListAsync();
        }
    }
}
