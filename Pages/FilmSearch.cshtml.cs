using AspNetTasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetTasks.Pages
{
    public class FilmSearchModel : PageModel
    {
        private readonly IFilmRepository _repository;

        public FilmSearchModel(IFilmRepository repository, ISearchFilter filter)
        {
            _repository = repository;
            Filter = filter;
        }

        public List<Film> Films { get; set; } = [];
        public ISearchFilter Filter { get; set; } 
        
        public async Task OnGetAsync()
        {
            Films = (await _repository.SearchFilmsAsync(Filter.Filter)).ToList();
        }
    }
}
