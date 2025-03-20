using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using AspNetTasks.DataAccess.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    public class FilmSearchModel : PageModel
    {
        private readonly IFilmRepository _filmRepository;

        public FilmSearchModel(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
            Filter = new SearchFilter();
        }

        [BindProperty(SupportsGet = true)]
        public SearchFilter Filter { get; set; }

        public IEnumerable<Film> Films { get; set; } = [];

        public async Task OnGetAsync()
        {
            if (Filter == null)
            {
                Filter = new SearchFilter();
            }

            Films = await _filmRepository.SearchFilmsAsync(Filter.Filter);
        }
    }
}
