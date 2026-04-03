using Forum.API.ForumMembers;
using System.Text.Json.Serialization;

namespace Forum.API.Photos.Entities
{
    public class MemberPhoto : Photo
    {
        public string MemberId { get; set; } = null!;

        [JsonIgnore]
        public ForumMember Member { get; set; } = null!;
    }
}
