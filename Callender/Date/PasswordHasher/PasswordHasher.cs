using System.Threading.Tasks;

namespace Callender.Date.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        public Task<string> HashlPassword(string password)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
        }

        public Task<bool> VarifyPassword(string password, string passwordhash)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, passwordhash));
        }
    }
}
