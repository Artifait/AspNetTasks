namespace AspNetTasks.Models
{
    public class AuthManager
    {
        private PasswordManager _passwordManager;
        private UserDb _db;
        public AuthManager(UserDb db, PasswordManager manager) 
        {
            _db = db;
            _passwordManager = manager;
        }

        public bool AuthUser(string login, string password, LogString? logger = null)
        {
            var users = _db.GetUsers();

            if (!users.Select(user => user.Login).Contains(login))
            {
                logger?.Invoke($"Попытка входа, под не существующем логином: {login}.");
                return false;
            }

            var user = users.Where(user => user.Login == login).FirstOrDefault();
            return user == null ? false : _passwordManager.VerifyHashedPassword(user.PasswordHash, password);
        }
    }
}
