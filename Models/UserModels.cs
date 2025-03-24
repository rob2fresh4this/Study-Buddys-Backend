namespace Study_Buddys_Backend.Models
{
    public class UserModels
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
    }
}