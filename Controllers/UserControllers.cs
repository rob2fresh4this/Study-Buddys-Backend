using Microsoft.AspNetCore.Mvc;
using Study_Buddys_Backend.Models;
using Study_Buddys_Backend.Models.DTOS;
using Study_Buddys_Backend.Services;

namespace Study_Buddys_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserControllers : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserControllers(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody]UserDTO user)
        {
            if(await _userServices.RegisterUser(user)) return Ok(new { Success = true });
            return BadRequest(new { Success = false });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody]UserDTO user)
        {
            string token = await _userServices.LoginUser(user);
            if (token != null) return Ok(new { Success = true, Token = token });
            return BadRequest(new { Success = false });
        }

        

    }
}