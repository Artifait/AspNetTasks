using AspNetTasks.Models;

namespace AspNetTasks
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        // Можно добавить дополнительные методы для работы с пользователями
    }
}
