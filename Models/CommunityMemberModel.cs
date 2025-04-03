namespace Study_Buddys_Backend.Models
{
    public class CommunityMemberModel
    {
        public int Id { get; set; } // Primary Key
        public int UserId { get; set; }
        public string Role { get; set; } = "student"; // Default role
    }
}
