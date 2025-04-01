using Microsoft.EntityFrameworkCore;
using Study_Buddys_Backend.Context;
using Study_Buddys_Backend.Models;

namespace Study_Buddys_Backend.Services
{
    public class CommunityServices
    {
        private readonly DataContext _dataContext;

        public CommunityServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<CommunityModel>> GetAllCommunitiesAsync()
        {
            return await _dataContext.Communitys.ToListAsync();
        }

        public async Task<bool> AddCommunityAsync(CommunityModel community)
        {
            await _dataContext.Communitys.AddAsync(community);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateCommunityAsync(CommunityModel community)
        {
            _dataContext.Communitys.Update(community);
            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}