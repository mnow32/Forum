namespace Forum.API.ForumMembers.DTOs
{
    public class ForumMemberDto
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Descripiton { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; } 
    }
}
