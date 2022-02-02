using Callender.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Callender.Date.Repo
{
    public interface ICallenderRepo
    {
        IEnumerable<User> GetAllUsers(); //getusers
        Task<User> GetUserByUserId(string Userid);//get User by Id
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
        Task<IBasicResponse> GetTokensById(string tokken); //Get Users Token
    }
    public class IBasicResponse
    {
        public int Type { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string Message { get; set; }
    }
}
