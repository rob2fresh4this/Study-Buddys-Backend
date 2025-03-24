using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> RegisterUser(UserDTO user)
        {
            if(await _userServices.RegisterUser(user)) return Ok();
            return BadRequest();
        }
    }
}