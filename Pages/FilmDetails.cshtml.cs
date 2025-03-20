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

        public async Task OnGetAsync(int id)
        {
            Film = await _context.GetFilmAsync(id);
        }
    }

}
