using AutoMapper;
using Callender.Date.PasswordHasher;
using Callender.Date.Repo;
using Callender.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Controller
{
    [Route("api/")]
    [ApiController]
    public class CallenderController : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICallenderRepo _repository;
        private readonly IMapper _mapper;

        public CallenderController(ICallenderRepo repository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }
        //sign up user
        [HttpPost("user/signup/")]
        public async Task<IActionResult> CreateCommand(AccountCreateDto commandCreateDto)
        {
            if (await _repository.CheckSignUpInformation(commandCreateDto.Phone, commandCreateDto.UserName) == false)
                return BadRequest();
            string passwordHash = await _passwordHasher.HashlPassword(commandCreateDto.Pass);
            commandCreateDto.Pass = passwordHash;
            var commandModel = _mapper.Map<User>(commandCreateDto);
            _repository.CreateUser(commandModel);
            var role = _mapper.Map<UserRole>(commandModel);
            role.UserID = commandModel.ID;
            await SetRolebyDefualt(role);
            await _repository.SaveChanges();
            return CreatedAtRoute(nameof(GetUserByUserId), new { UserID = commandModel.ID }, commandModel);
        }
        public async Task<IActionResult> SetRolebyDefualt(UserRole roleAccess)
        {
            var res = await _repository.SetRolebydefualt(roleAccess.UserID, roleAccess.RoleID);
            if (!res.IsSuccessful)
            {
                return StatusCode(res.Type, res);
            }
            var setrole = _mapper.Map<UserRole>(roleAccess);
            _repository.PostRole(setrole);
            //    await _repository.SaveChanges();
            return Ok(res);
        }
        //get user by id
        //[Authorize(Roles = "Admin,Moderator")]
        [HttpGet("user/{Userid}", Name = "GetUserByUserId")]
        public async Task<IActionResult> GetUserByUserId(string Userid)
        {
            var userinfo = await _repository.GetUserByUserId(Userid);
            if (userinfo != null)
            {
                return Ok(userinfo);
            }
            return NotFound();
        }
    }
}
