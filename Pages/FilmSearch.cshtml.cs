using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    public class FilmSearchModel : PageModel
    {
        private readonly IFilmRepository _repository;

        public FilmSearchModel(IFilmRepository repository)
        {
            _repository = repository;
        }

        public List<Film> Films { get; set; } = new List<Film>();
        public SearchFilter Filter { get; set; } = new SearchFilter();

        public async Task OnGetAsync(SearchFilter filter)
        {
            Films = (await _repository.SearchFilmsAsync(filter)).ToList();
        }
    }
}
