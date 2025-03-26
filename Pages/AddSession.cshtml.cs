using AspNetTasks.DataAccess;
using AspNetTasks.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AspNetTasks.Pages
{
    [IgnoreAntiforgeryToken]
    public class AddSessionModel : PageModel
    {
        private readonly CinemaContext _context;

        public AddSessionModel(CinemaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FilmSession FilmSession { get; set; }

        // Для отображения списка фильмов
        public List<Film> Films { get; set; }

        // Сюда придет JSON-конфигурация зала (ряды и количество мест)
        [BindProperty]
        public string SeatingConfiguration { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Films = await _context.Films.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Films = await _context.Films.ToListAsync();
                return Page();
            }

            // Проверка пересечения времени для выбранного фильма
            var overlappingSession = _context.FilmSessions
                .FirstOrDefault(fs => fs.FilmId == FilmSession.FilmId
                                   && fs.StartTime < FilmSession.EndTime
                                   && fs.EndTime > FilmSession.StartTime);
            if (overlappingSession != null)
            {
                ModelState.AddModelError(string.Empty, "Этот сеанс пересекается с другим.");
                Films = await _context.Films.ToListAsync();
                return Page();
            }

            var config = JsonSerializer.Deserialize<List<RowPosition>>(SeatingConfiguration);

            if (config != null)
            {
                foreach (var row in config)
                {
                    FilmSession.RowPositions.Add(new RowPosition
                    {
                        RowNumber = row.RowNumber,
                        X = row.X,
                        Y = row.Y
                    });
                }
            }

            _context.FilmSessions.Add(FilmSession);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    // Вспомогательный класс для десериализации конфигурации зала
    public class SeatingRowConfig
    {
        public int RowNumber { get; set; }
        public int SeatCount { get; set; }
    }
}
