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
            return await _dataContext.Communitys.Include(c => c.CommunityMembers).ToListAsync();
        }


        public async Task<bool> AddCommunityAsync(CommunityModel community)
        {
            await _dataContext.Communitys.AddAsync(community);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateCommunityAsync(CommunityModel community)
        {
            var existingCommunity = await _dataContext.Communitys.FindAsync(community.Id);
            if (existingCommunity == null) return false;

            // Preserve CommunityMembers & CommunityRequests
            community.CommunityMembers = existingCommunity.CommunityMembers;
            community.CommunityRequests = existingCommunity.CommunityRequests;

            // Update only the allowed fields
            _dataContext.Entry(existingCommunity).CurrentValues.SetValues(community);

            return await _dataContext.SaveChangesAsync() != 0;
        }


        public async Task<bool> AddMemberToCommunityAsync(int communityId, int userId, string role = "student")
        {
            var community = await _dataContext.Communitys
                .Include(c => c.CommunityMembers) // Make sure members are loaded
                .FirstOrDefaultAsync(c => c.Id == communityId);

            if (community == null) return false;

            // Ensure the member is not already in the community
            if (!community.CommunityMembers.Any(m => m.UserId == userId))
            {
                community.CommunityMembers.Add(new CommunityMemberModel
                {
                    UserId = userId,
                    Role = role
                });

                return await _dataContext.SaveChangesAsync() != 0;
            }
            return false;
        }



        public async Task<bool> RemoveMemberFromCommunityAsync(int communityId, int userId)
        {
            var community = await GetCommunityByIdAsync(communityId);
            if (community == null) return false;

            if (community.CommunityMembers != null)
            {
                var member = community.CommunityMembers.FirstOrDefault(m => m.UserId == userId);
                if (member != null)
                {
                    community.CommunityMembers.Remove(member);
                    int result = await _dataContext.SaveChangesAsync();
                    return result != 0;
                }
            }

            return false;
        }



        public async Task<CommunityModel?> GetCommunityByIdAsync(int communityId)
        {
            return await _dataContext.Communitys
                .Include(c => c.CommunityMembers) // Ensure members are included
                .FirstOrDefaultAsync(c => c.Id == communityId);
        }



        public async Task<bool> AddRequestToCommunityAsync(int communityId, int userId)
        {
            var community = await _dataContext.Communitys.FindAsync(communityId);
            if (community == null) return false;

            // Ensure list is initialized
            if (community.CommunityRequests == null)
            {
                community.CommunityRequests = new List<int>();
            }

            if (!community.CommunityRequests.Contains(userId))
            {
                community.CommunityRequests.Add(userId);
                _dataContext.Communitys.Update(community); // Explicit update
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> RemoveRequestFromCommunityAsync(int communityId, int userId)
        {
            var community = await _dataContext.Communitys.FindAsync(communityId);
            if (community == null) return false;

            if (community.CommunityRequests != null && community.CommunityRequests.Contains(userId))
            {
                community.CommunityRequests.Remove(userId);
                _dataContext.Communitys.Update(community); // Explicit update
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> ApproveRequestAsync(int communityId, int userId, bool approve)
        {
            var community = await _dataContext.Communitys.FindAsync(communityId);
            if (community == null) return false;

            if (approve)
            {
                // Add user to members
                await AddMemberToCommunityAsync(communityId, userId);
                await RemoveRequestFromCommunityAsync(communityId, userId);
            }
            else
            {
                // Remove user from requests
                await RemoveRequestFromCommunityAsync(communityId, userId);
            }

            return true;
        }


        // only for testing purposes
        public async Task<bool> ClearCommunityMembersAsync(int communityId)
        {
            var community = await _dataContext.Communitys
                .Include(c => c.CommunityMembers) // Make sure members are included
                .FirstOrDefaultAsync(c => c.Id == communityId);

            if (community == null) return false;

            community.CommunityMembers.Clear(); // Remove all members
            community.CommunityMemberCount = 0; // Reset count

            return await _dataContext.SaveChangesAsync() > 0; // Ensure changes are saved
        }



    }
}