using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Pages
{
    public class FilmScheduleModel : PageModel
    {
        private readonly IFilmRepository _context;

        public FilmScheduleModel(IFilmRepository context)
        {
            _context = context;
        }

        // Список фильмов с сеансами
        public List<Film> Films { get; set; } = [];

        public async Task OnGetAsync()
        {
            // Загрузка фильмов с их сеансами
            Films = (await _context.GetAllFilmsAsync()).ToList();
        }
    }
}
