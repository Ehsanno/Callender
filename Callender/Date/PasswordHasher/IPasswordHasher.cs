using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Date.PasswordHasher
{
    public interface IPasswordHasher
    {
        Task<string> HashlPassword(string pass);
        Task<bool> VarifyPassword(string password, string passwordhash);
    }
}
