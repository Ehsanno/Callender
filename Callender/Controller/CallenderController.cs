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
        public async Task<IActionResult> CreateUser(AccountCreateDto commandCreateDto)
        {
            UserRole role=new();
            if (await _repository.CheckSignUpInformation(commandCreateDto.Email, commandCreateDto.UserName) == false)
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
        
        //get users
        [Authorize]
        [HttpGet("user")]
        public IActionResult GetUsers()
        {
            var userinfo = _repository.GetAllUsers();
            return Ok(userinfo);
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

        //set Suggest 
        [Authorize]
        [HttpPost("Suggest")]
        public async Task<IActionResult> SetSuggest(SetSuggest suggest)
        {
            SuggestCarrier suggestCarrier = new();
            string UserID = HttpContext.User.FindFirstValue(ClaimTypes.SerialNumber);
            var suggestinfo = _mapper.Map<Suggest>(suggest);
            suggestinfo.UserID = UserID;
            _repository.SetSuggest(suggestinfo);
            await _repository.SaveChanges();
            return Ok(suggestinfo);
        }

        //set suggestcarrier
        [Authorize]
        [HttpPost("suggestCarrier")]
        public async Task<IActionResult> SetSuggestCarrier(SuggestCarrier suggestCarrier)
        {
            _repository.SetSuggestCarrier(suggestCarrier);
            await _repository.SaveChanges();
            return Ok(suggestCarrier    );
        }

        // Get Suggests
        [Authorize]
        [HttpGet("Suggest")]
        public IActionResult GetSuggests()
        {
            var suggestinfo = _repository.GetAllSuggest();
            return Ok(suggestinfo);
        }
       
        //get Suggest By startdate
        [Authorize]
        [HttpGet("Suggest/{StartDate}", Name= "GetSuggestByDate")]
        public async Task<IActionResult> GetSuggestByDate(DateTime StartDate)
        {
            var suggestinfo = await _repository.GetSuggestByDate(StartDate);
            if(suggestinfo==null)
            {
                return NotFound();
            }
            return Ok(suggestinfo);
        }
        
        //set Carrier
        [Authorize]
        [HttpPost("Carrier")]
        public async Task<IActionResult> SetCarrier(Carrier setCarrier)
        {
            _repository.SetCarrier(setCarrier);
            await _repository.SaveChanges();
            return Ok(setCarrier);
        }
        
        //get Carrier
        [Authorize]
        [HttpGet("Carrier")]
        public IActionResult GetCarriers()
        {
            var carrierinfo = _repository.GetAllCarrier();
            return Ok(carrierinfo);
        }

        /*Get Carrier By ID
        [Authorize]
        [HttpGet("Carrier/{CarrierID}")]
        public async Task<IActionResult> GetCarrierByID(string CarrierID)
        {
            var suggestinfo = await _repository.GetCarrierById(CarrierID);
            if (suggestinfo == null)
            {
                return NotFound();
            }
            return Ok(suggestinfo);
        }*/

        //Get SuggestCarrier By CarrierID
        [Authorize]
        [HttpGet("SuggestCarrier/{CarrierID}")]
        public async Task<IActionResult> GetSuggestCarrierByID(string CarrierID)
        {
            var suggestcarrierinfo = await _repository.GetSuggestCarrierByCarrierID(CarrierID);
            if (suggestcarrierinfo == null)
            {
                return NotFound();
            }
            return Ok(suggestcarrierinfo);
        }
        
        // Get SuggestCarriers
        [Authorize]
        [HttpGet("SuggestCarrier")]
        public IActionResult GetSuggestCarriers()
        {
            var SuggestCarrierinfo = _repository.GetAllSuggestCarrier();
            return Ok(SuggestCarrierinfo);
        }

        //Set UserCarrier
        [Authorize]
        [HttpPost("UserCarrier")]
        public async Task<IActionResult> SetUserCarrier(SetUserCarrier setuserCarrier)
        {
            string UserID = HttpContext.User.FindFirstValue(ClaimTypes.SerialNumber);
            var userCarrier = _mapper.Map<UserCarrier>(setuserCarrier);
            userCarrier.UserID = UserID;
            _repository.SetUserCarrier(userCarrier);
            await _repository.SaveChanges();
            return Ok(userCarrier);
        }

        //Get UserCarrier By ID
        [Authorize]
        [HttpGet("UserCarrier/{CarrierID}")]
        public async Task<IActionResult> GetUserCarrier(string CarrierID)
        {
            var usercarrierinfo = await _repository.GetUserCarrierByID(CarrierID);
            if (usercarrierinfo == null)
            {
                return NotFound();
            }
            return Ok(usercarrierinfo);
        }

        //Add Subpremume By premume
        [Authorize]
        [HttpPost("SubPremum")]
        public async Task<IActionResult> AddSubPremum(string UserName)
        {
            SubPremum subPremum = new();
            var UserInfo = await _repository.GetUserByUserName(UserName);
            string UserID = HttpContext.User.FindFirstValue(ClaimTypes.SerialNumber);
            subPremum.PremiumID = UserID;
            subPremum.SubPremiumID = UserInfo.ID;
            _repository.AddSubPremum(subPremum);
            await _repository.SaveChanges();
            return Ok(subPremum);
        }
    }
}