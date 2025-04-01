namespace Study_Buddys_Backend.Models
{
    public class CommunityModel
    {
        public int CommunityID { get; set; }
        public int CommunityOwnerID { get; set; }
        public bool CommunityIfOwner { get; set; }
        public bool CommunityIsPublic { get; set; }
        public bool CommunityIsDeleted { get; set; }
        public string? CommunityOwnerName { get; set; }
        public string? CommunityName { get; set; }
        public string? CommunitySubject { get; set; }
        public int CommunityMemberCount { get; set; }
        public string? CommunityDifficulty { get; set; }
        public string? CommunityDescription { get; set; }
    }
}