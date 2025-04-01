namespace Study_Buddys_Backend.Models.DTOS
{
    public class EditCommunitiesDTO
    {
        public List<int>? OwnedCommunityIds { get; set; }
        public List<int>? JoinedCommunityIds { get; set; }
        public List<int>? CommunityRequestIds { get; set; }
    }
}