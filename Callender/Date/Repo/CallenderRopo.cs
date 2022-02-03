using Callender.Data;
using Callender.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Date.Repo
{
    public class CallenderRopo:ICallenderRepo
    {
        private readonly UserContext _context;

        public CallenderRopo(UserContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
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
        public Task<bool> SaveChanges()
        {
            return Task.FromResult(_context.SaveChanges() >= 0);
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
        public async Task<bool> CheckLoginInformation(string email, string username)
        {
            var user2 = await _context.Users.FirstOrDefaultAsync(z => z.UserName == username);
            //var user1 = await _context.Users.FirstOrDefaultAsyncAsync(x => x.Email == email);
            //if (user1 is null || user2 is null) return false;
            //if (user2.UserName == user1.UserName && user1.Email == user2.Email && user1.Pass == user2.Pass)
            //    return true;
            return false;
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
        public async void SetUsersToken(Token cmd)
        {
             _context.Token.Add(cmd);
            await SaveChanges();
        }
        public async Task<IBasicResponse> GetTokensById(string usertoken)
        {

            if (string.IsNullOrEmpty(usertoken)) return new IBasicResponse { Message = "OK" };

            var tokenFromDb = await _context.Token.FirstOrDefaultAsync(x => x.UserToken == usertoken && x.TokenValidator == 0);
            if (tokenFromDb is null) return new IBasicResponse { Message = "NOT VALID" };

            return new IBasicResponse { Message = "OK" };
        }
        
    }
}
