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

        [HttpGet("getUserInfo/{id}")]
        public async Task<IActionResult> GetUserInfo(int id)
        {
            var userInfo = await _userServices.GetAllUserInfoAsync(id);

            if (userInfo == null)
                return NotFound(new { Success = false, Message = "User not found" });

            return Ok(new { Success = true, User = userInfo });
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDTO user)
        {
            if (await _userServices.RegisterUser(user)) return Ok(new { Success = true });
            return BadRequest(new { Success = false });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDTO user)
        {
            string token = await _userServices.LoginUser(user);
            if (token != null) return Ok(new { Success = true, Token = token });
            return BadRequest(new { Success = false });
        }

        [HttpPut("EditUserCommunities/{id}")]
        public async Task<IActionResult> EditUserCommunities(int id, [FromBody] EditCommunitiesDTO editCommunities)
        {
            var success = await _userServices.EditUserCommunitiesAsync(id, editCommunities.OwnedCommunityIds, editCommunities.JoinedCommunityIds, editCommunities.CommunityRequestIds);

            if (!success)
            {
                return BadRequest(new { Success = false, Message = "Failed to update user communities" });
            }
            return Ok(new { Success = true, Message = "User communities updated successfully" });
        }

        [HttpPost("AddCommunityToUser/{id}")]
        public async Task<IActionResult> AddCommunityToUser(int id, [FromBody] AddCommunityDTO addCommunity)
        {
            var success = await _userServices.AddCommunityToUserAsync(id, addCommunity.OwnedCommunityIds, addCommunity.JoinedCommunityIds, addCommunity.CommunityRequestIds);

            if (!success)
            {
                return BadRequest(new { Success = false, Message = "Failed to add community to user" });
            }
            return Ok(new { Success = true, Message = "Community added to user successfully" });
        }

    }
}