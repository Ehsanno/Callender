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

        public  IEnumerable<User> GetAllUsers()
        {
            return  _context.Users.ToList();
        }
        public  IEnumerable<Suggest> GetAllSuggest()
        {
            return  _context.Suggest.ToList();
        }
        public  IEnumerable<Carrier> GetAllCarrier()
        {
            return _context.Carrier.ToList();
        }
        public IEnumerable<SuggestCarrier> GetAllSuggestCarrier()
        {
            return  _context.SuggestCarrier.ToList();
        }

        //Get User By Id
        public async Task<User> GetUserByUserId(string Userid)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.ID == Userid);
        }

        //Get SuggestCarrier By Id
        public async Task<SuggestCarrier> GetSuggestCarrierByCarrierID(string CarrieriD)
        {
            return await _context.SuggestCarrier.FirstOrDefaultAsync(p => p.CarrierID == CarrieriD);
        }

        //Get User By UserName
        public async Task<User> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserName == userName);
        }

        //Get UserCarrier By ID
        public async Task<object> GetUserCarrierByID(string CarrierID)
        {
            return await _context.UserCarrier.Where(p => p.CarrierID == CarrierID).ToListAsync();
        }
        ////Get UserCarrier By ID
        //public async Task<UserCarrier> GetUserCarrierByUserID(string UserID)
        //{
        //    return await _context.UserCarrier.FirstOrDefaultAsync(p => p.UserID == UserID);
        //}
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
        public async void CreateUser(User cmd)
        {
             await _context.Users.AddAsync(cmd);
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
        public async void PostRole(UserRole cmd)
        {
            await _context.UserRole.AddAsync(cmd);
        }

        //AddAsync New Role 
        public async void AddRole(Role cmd)
        {
            await _context.Roles.AddAsync(cmd);
        }
        //check sign up information
        public async Task<bool> CheckSignUpInformation(string phoneNumber, string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == phoneNumber);
            var user1 = await _context.Users.FirstOrDefaultAsync(y => y.UserName == username);
            if (user is null && user1 is null) return true;
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
        public async void SetUsersToken(Token cmd)
        {
            await _context.Token.AddAsync(cmd);
        }

        public async Task<bool> GetTokenByToken(string token)
        {
            var validator = await _context.Token.ToListAsync();
            var found = validator.Find(x => x.UserToken == token);
            if (validator is null)
                return false;
            if (found.TokenValidator== 0)
                return false;
            return true;

        }

        //Get Suggest By StartDate
        public async Task<Suggest> GetSuggestByDate(DateTime StartDate)
        {
            return await _context.Suggest.FirstOrDefaultAsync(s => s.StartDate == StartDate);
        }

        //Get Carrier By ID
        public async Task<Carrier> GetCarrierById(string CarrierID)
        {
            return await _context.Carrier.FirstOrDefaultAsync(s => s.ID == CarrierID);
        }

        //set Suggest
        public async void SetSuggest(Suggest cmd)
        {
           await _context.Suggest.AddAsync(cmd);
        }

        //set Carrier 
        public async void SetCarrier(Carrier cmd)
        {
            await _context.Carrier.AddAsync(cmd);
        }

        //set SuggestCarrier
        public async void SetSuggestCarrier(SuggestCarrier cmd)
        {
            await _context.SuggestCarrier.AddAsync(cmd);
        }
        //set UserCarrier
        public async void SetUserCarrier(UserCarrier cmd)
        {
            await _context.UserCarrier.AddAsync(cmd);
        }

        public async void AddSubPremum(SubPremum cmd)
        {
            await _context.Subpremum.AddAsync(cmd);
        }
    }
}
