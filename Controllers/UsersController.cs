using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Services.UserService;
using WebApi.Services.UserService.Dto;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody]AuthenticateModel model)
        {
            if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return BadRequest(new { message = "Username or password is incorrect" });

            var user = await _userService.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole("Role.Admin"))
                return Forbid();

            var user =  await _userService.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto UserData)
        {
            try
            {
                await _userService.Create(UserData);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Role.Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }
    }
}
