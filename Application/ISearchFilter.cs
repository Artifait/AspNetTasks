using AspNetTasks.DataAccess.Entities;
using System.Linq.Expressions;

namespace AspNetTasks.Application
{
    public interface ISearchFilter
    {
        bool IsMatch(Film film);
        Expression<Func<Film, bool>> Filter { get; }
    }
}
