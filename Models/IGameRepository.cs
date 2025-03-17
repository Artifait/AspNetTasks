namespace AspNetTasks.Models
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetGamesAsync(string? author, string? genre);
        Task<Game?> GetGameByIdAsync(int id);
        Task<Game> AddGameAsync(Game game);
        Task<Game> UpdateGameAsync(Game game);
        Task DeleteGameAsync(Game game);
    }
}
