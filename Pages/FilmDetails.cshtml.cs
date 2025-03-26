using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    public class FilmDetailsModel : PageModel
    {
        private readonly IFilmRepository _context;

        public FilmDetailsModel(IFilmRepository context)
        {
            _context = context;
        }

        public Film Film { get; set; }
        public Dictionary<int, int> ReservedSeats { get; set; } = new();

        public async Task OnGetAsync(int id)
        {
            Film = await _context.GetFilmAsync(id);

            if (Film?.Sessions != null)
            {
                foreach (var session in Film.Sessions)
                {
                    ReservedSeats[session.Id] = await _context.GetReservedSeatsCountAsync(session.Id);
                }
            }
        }
    }
}
