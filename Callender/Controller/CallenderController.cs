using AutoMapper;
using Callender.Data.TokenGenerator;
using Callender.Date.PasswordHasher;
using Callender.Date.Repo;
using Callender.Model.Authentication;
using Callender.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Callender.Controller
{
    [Route("api/")]
    [ApiController]
    public class CallenderController : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICallenderRepo _repository;
        private readonly IMapper _mapper;
        private readonly AccessToken _accessToken;
        public CallenderController(ICallenderRepo repository, IPasswordHasher passwordHasher, IMapper mapper, AccessToken accessToken)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _accessToken = accessToken;
        }
        //sign up user
        [HttpPost("user/signup/")]
        public async Task<IActionResult> CreateCommand(AccountCreateDto commandCreateDto)
        {
            UserRole role=new();
            if (await _repository.CheckSignUpInformation(commandCreateDto.Phone, commandCreateDto.UserName) == false)
                return BadRequest();
            string passwordHash = await _passwordHasher.HashlPassword(commandCreateDto.Pass);
            commandCreateDto.Pass = passwordHash;
            var commandModel = _mapper.Map<User>(commandCreateDto);
            _repository.CreateUser(commandModel);
            role.UserID= commandModel.ID;
            await _repository.SaveChanges();
            await SetRolebyDefualt(role);
            
            return CreatedAtRoute(nameof(GetUserByUserId), new { UserID = commandModel.ID }, commandModel);
        }

        //LOG IN 
        [HttpPost("user/login/")]
        public async Task<IActionResult> LoginByUsers(LoginInformationDto loginInformationDto)
        {
            Token tokenDto = new();
            if (await _repository.CheckLoginInformation(loginInformationDto.Pass, loginInformationDto.UserName) == false)
                return Unauthorized();
            var user = await _repository.GetUserByUserName(loginInformationDto.UserName);      
            var check = await _repository.Roleacces(user.ID);
            var rolesaccess = await _repository.RoleCheck(check.RoleID);
            string accessToken = _accessToken.GenerateToken(user, rolesaccess);
            tokenDto.UserID = user.ID;
            tokenDto.UserToken = accessToken;
            _repository.SetUsersToken(tokenDto);
            await _repository.SaveChanges();
            return Ok(new AuthenticationUserResponses()
            {
                AccessToken = accessToken,
            });
        }
        
        //set role by default
        public async Task<IActionResult> SetRolebyDefualt(UserRole roleAccess)
        {
            var res = await _repository.SetRolebydefualt(roleAccess.UserID, roleAccess.RoleID);
            if (!res.IsSuccessful)
            {
                return StatusCode(res.Type, res);
            }
            var setrole = _mapper.Map<UserRole>(roleAccess);
            _repository.PostRole(setrole);
                await _repository.SaveChanges();
            return Ok(res);
        }
        
        //get user by id
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("user/{Userid}", Name = "GetUserByUserId")]
        public async Task<IActionResult> GetUserByUserId(string Userid)
        {
            var userinfo = await _repository.GetUserByUserId(Userid);
            if (userinfo == null)
            {
                return NotFound();
            }
            return Ok(userinfo);
        }
        
        // Get Suggests
        [Authorize]
        [HttpGet("Suggest")]
        public IActionResult GetSuggests()
        {
            var suggestinfo = _repository.GetAllSuggest();
            return Ok(suggestinfo);
        }
       
        //get Suggest By ID
        [Authorize]
        [HttpGet("Suggest/{suggestID}", Name= "GetSuggestById")]
        public async Task<IActionResult> GetSuggestById(string suggestID)
        {
            var suggestinfo = await _repository.GetSuggestById(suggestID);
            if(suggestinfo==null)
            {
                return NotFound();
            }
            return Ok(suggestinfo);
        }
        
        //set Suggest 
        [Authorize]
        [HttpPost("Suggest")]
        public async Task<IActionResult> SetSuggest(SetSuggest suggest)
        {
            string UserID = HttpContext.User.FindFirstValue(ClaimTypes.SerialNumber);
            var suggest1 = _mapper.Map<Suggest>(suggest);
            suggest1.UserID = UserID;
            _repository.SetSuggest(suggest1);
            await _repository.SaveChanges();
            return Ok(suggest1);
        }
        
        //set Carrier
        [Authorize]
        [HttpPost("Carrier")]
        public async Task<IActionResult> SetCarrier(SetCarrier setCarrier)
        {
            var setCarrier1 = _mapper.Map<Suggest>(setCarrier);
            _repository.SetSuggest(setCarrier1);
            await _repository.SaveChanges();
            return Ok(setCarrier1);
        }
        
    }
}
