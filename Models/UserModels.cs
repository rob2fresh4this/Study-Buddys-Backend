namespace Study_Buddys_Backend.Models
{
    public class UserModels
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }

        // Communitys Joined and Requests
        // public List<string>? OwnedCommunitys { get; set; } = new List<string>();
        // public List<string>? JoinedCommunitys { get; set; } = new List<string>();
        // public List<string>? CommunityRequests { get; set; } = new List<string>();
    }
}