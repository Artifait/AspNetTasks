
namespace AspNetTasks.Models
{
    public class User
    {
        public string Login;
        public string PasswordHash;
    }

    public class UserDb
    {
        private List<User> _users = [];
        private PasswordManager _hasher;

        public UserDb(PasswordManager hasher) {
            _hasher = hasher;
        }

        public bool RegisterUser(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) {
                return false;
            }
            if (_users.Select(user => user.Login).Contains(login)) {
                return false;
            }
            var hashPassword = _hasher.HashPassword(password);

            User user = new() {
                Login = login,
                PasswordHash = hashPassword
            };

            _users.Add(user);
            return true;
        }

        public List<User> GetUsers() => _users;
    }
}
