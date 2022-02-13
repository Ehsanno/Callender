using Callender.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Callender.Date.Repo
{
    public interface ICallenderRepo
    {
        IEnumerable<User> GetAllUsers(); //getusers
        IEnumerable<Suggest> GetAllSuggest(); //getusers
        Task<User> GetUserByUserId(string Userid);//get User by Id
        Task<Suggest> GetSuggestById(string SuggestID);//Get Suggest By ID
        void SetSuggest(Suggest cmd); //Set Suggest 
        Task<Role> RoleCheck(string roleid);//check roles by roleId
        Task<UserRole> Roleacces(string Id);// check users Roles
        void CreateUser(User cmd); // create users
        Task<bool> SaveChanges();//save changes
        Task<IBasicResponse> SetRole(string userid, string roleid);//set role for users
        Task<IBasicResponse> SetRolebydefualt(string userid, string roleid);//set member role for user when they signup
        void PostRole(UserRole cmd);//set new role for users
        void AddRole(Role cmd);//create new role
        Task<bool> CheckSignUpInformation(string phoneNumber, string username);//check sign up information
        Task<bool> CheckLoginInformation(string email, string username);//check sign in information
        Task<bool> CheckIsAdmin(string UserName);//user manager
        Task<User> GetUserByUserName(string userName);// Get User By UserName
        void SetUsersToken(Token cmd); // Get User Token
        Task<IBasicResponse> GetTokensByID(string ID); //Get Users Token
        Task<bool> GetTokenByToken(string token); //check valid token
        void SetCarrier(Carrier cmd); //Set Carrier 

    }
    public class IBasicResponse
    {
        public int Type { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string Message { get; set; }
    }
}
