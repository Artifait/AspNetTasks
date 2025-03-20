using AspNetTasks.Application;
using AspNetTasks.DataAccess.Entities;
using System.Linq.Expressions;

namespace AspNetTasks.DataAccess.Filters
{
    public class SearchFilter : ISearchFilter
    {
        public string? Name { get; set; }
        public string? FilmMaker { get; set; }
        public string? Style { get; set; }
        public string? Summary { get; set; }
        public DateTime? SessionStartDate { get; set; }
        public DateTime? SessionEndDate { get; set; }

        public virtual bool IsMatch(Film film)
        {
            if (!string.IsNullOrWhiteSpace(Name) && !film.Name.Contains(Name))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(FilmMaker) && !film.FilmMaker.Contains(FilmMaker))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Style) && !film.Style.Contains(Style))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Summary) && !film.Summary.Contains(Summary))
            {
                return false;
            }

            if (SessionStartDate.HasValue && SessionEndDate.HasValue &&
                !film.Sessions.Any(s => s.StartTime >= SessionStartDate && s.EndTime <= SessionEndDate))
            {
                return false;
            }

            return true;
        }

        public virtual Expression<Func<Film, bool>> Filter => film =>
            (string.IsNullOrEmpty(Name) || film.Name.Contains(Name)) &&
            (string.IsNullOrEmpty(FilmMaker) || film.FilmMaker.Contains(FilmMaker)) &&
            (string.IsNullOrEmpty(Style) || film.Style.Contains(Style)) &&
            (string.IsNullOrEmpty(Summary) || film.Summary.Contains(Summary)) &&
            (!SessionStartDate.HasValue || !SessionEndDate.HasValue ||
             film.Sessions.Any(s => s.StartTime >= SessionStartDate && s.EndTime <= SessionEndDate));
    }
}
