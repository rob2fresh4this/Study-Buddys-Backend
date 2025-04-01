using Microsoft.AspNetCore.Mvc;
using Study_Buddys_Backend.Models;
using Study_Buddys_Backend.Services;

namespace Study_Buddys_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommunityControllers : ControllerBase
    {
        private readonly CommunityServices _communityServices;

        public CommunityControllers(CommunityServices communityServices)
        {
            _communityServices = communityServices;
        }

        [HttpGet("getAllCommunities")]
        public async Task<IActionResult> GetAllCommunities()
        {
            var communities = await _communityServices.GetAllCommunitiesAsync();
            if (communities == null || !communities.Any())
            {
                return BadRequest(new { Success = false, Message = "No communities found or error retrieving data" });
            }
            return Ok(new { Success = true, Communities = communities });
        }

        [HttpPost("addCommunity")]
        public async Task<IActionResult> AddCommunity([FromBody] CommunityModel community)
        {
            if (await _communityServices.AddCommunityAsync(community)) return Ok(new { Success = true });
            return BadRequest(new { Success = false });
        }
    }
}