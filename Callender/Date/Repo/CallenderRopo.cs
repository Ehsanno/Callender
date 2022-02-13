using Callender.Data;
using Callender.Date.PasswordHasher;
using Callender.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Date.Repo
{
    public class CallenderRopo : ICallenderRepo
    {
        private readonly UserContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public CallenderRopo(UserContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public IEnumerable<Suggest> GetAllSuggest()
        {
            return _context.Suggest.ToList();
        }
        public IEnumerable<Carrier> GetAllCarrier()
        {
            return _context.Carrier.ToList();
        }
        //Get User By Id
        public async Task<User> GetUserByUserId(string Userid)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.ID == Userid);
        }
        //Get User By UserName
        public async Task<User> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserName == userName);
        }
        //get token by id
        public async Task<Token> GetTokenById(string TokenId)
        {
            return await _context.Token.FirstOrDefaultAsync(s => s.ID == TokenId);
        }
        public async Task<Role> RoleCheck(string roleid)
        {
            return await _context.Roles.FirstOrDefaultAsync(s => s.ID == roleid);
        }
        public async Task<UserRole> Roleacces(string Id)
        {
            return await _context.UserRole.FirstOrDefaultAsync(s => s.UserID == Id);
        }
        //create user
        public  void CreateUser(User cmd)
        {
             _context.Users.Add(cmd);
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
        //Set Role
        public async Task<IBasicResponse> SetRole(string userid, string roleid)
        {
            // Get User From db
            var userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.ID == userid);

            // Check User Data
            if (userFromDb is null)
                return new IBasicResponse { Type = 404, Message = "User Not Found !", IsSuccessful = false };

            // Admin Role IS = 2 !
            return new IBasicResponse { Message = "User Role Updated ." };
        }
        //set Defualt By Defualt
        public async Task<IBasicResponse> SetRolebydefualt(string userid, string roleid)
        {
            // Get User From db
            var userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.ID == userid);

            // Admin Role IS = 2 !
            return new IBasicResponse { Message = "User Role Updated ." };
        }
        // Set Role To DataBase
        public  void PostRole(UserRole cmd)
        {
             _context.UserRole.Add(cmd);
        }

        //Add New Role 
        public  void AddRole(Role cmd)
        {
            _context.Roles.Add(cmd);
        }
        //check sign up information
        public async Task<bool> CheckSignUpInformation(string phoneNumber, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phoneNumber);
            var user1 = await _context.Users.FirstOrDefaultAsync(y => y.UserName == username);
            if (user is null || user1 is null) return true;
            return false;
        }

        //check signin information
        public async Task<bool> CheckLoginInformation(string pass, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(z => z.UserName == username);
            if(user is null)
                return false; 
            bool isCorrectPassword = await _passwordHasher.VarifyPassword(pass, user.Pass);
            if (!isCorrectPassword)      
                return false;
            return true;
        }
        //User Manger
        public async Task<bool> CheckIsAdmin(string UserName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (user is null) return false;
            // var roles = await _context.UserRole.FirstOrDefaultAsyncAsync(x => x.UserID == user.ID);
            var roles = _context.UserRole.ToList().Where(x => x.UserID == user.ID && x.RoleID == "1");
            if (roles == null)
                return false;
            return true;
        }
        //push tokens for user id 
        public void SetUsersToken(Token cmd)
        {
              _context.Token.Add(cmd);
        }
        public async Task<IBasicResponse> GetTokensByID(string usertoken)
        {

            if (string.IsNullOrEmpty(usertoken)) return new IBasicResponse { Message = "OK" };

            var tokenFromDb = await _context.Token.FirstOrDefaultAsync(x => x.UserToken == usertoken && x.TokenValidator == 0);
            if (tokenFromDb is null) return new IBasicResponse { Message = "NOT VALID" };

            return new IBasicResponse { Message = "OK" };
        }

        public async Task<bool> GetTokenByToken(string token)
        {
          var validator = await _context.Token.FirstOrDefaultAsync(x => x.UserToken == token);
            if (validator is null)
                return false;
            if (validator.TokenValidator == 1)
                return false;
            return true;
        }

        //Get Suggest By ID
        public async Task<Suggest> GetSuggestById(string SuggestID)
        {
            return await _context.Suggest.FirstOrDefaultAsync(s => s.ID == SuggestID);
        }

        //Get Carrier By ID
        public async Task<Carrier> GetCarrierById(string CarrierID)
        {
            return await _context.Carrier.FirstOrDefaultAsync(s => s.ID == CarrierID);
        }

        //set Suggest By ID
        public void SetSuggest(Suggest cmd)
        {
            _context.Suggest.Add(cmd);
        }

        public void SetCarrier(Carrier cmd)
        {
            _context.Carrier.Add(cmd);
        }
    }
}
