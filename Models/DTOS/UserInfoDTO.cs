namespace Study_Buddys_Backend.Models.DTOS
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public List<int> OwnedCommunitys { get; set; } = new List<int>();
        public List<int> JoinedCommunitys { get; set; } = new List<int>();
        public List<int> CommunityRequests { get; set; } = new List<int>();
    }

}