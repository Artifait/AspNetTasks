namespace AspNetTasks.Models
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAllFilmsAsync();
        Task<IEnumerable<FilmSession>> GetAllSessionsAsync();
        Task<Film?> GetFilmAsync(int id);

        Task AddFilmAsync(Film film);
        Task RemoveFilmAsync(int filmId);

        Task AddFilmSessionAsync(FilmSession session);
        Task RemoveFilmSessionAsync(int sessionId);

        Task UpdateFilmAsync(Film film);
        Task<IEnumerable<Film>> SearchFilmsAsync(SearchFilter filter);
    }
}
