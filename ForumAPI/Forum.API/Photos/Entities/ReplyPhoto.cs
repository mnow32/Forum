using Forum.API.Replies;
using System.Text.Json.Serialization;

namespace Forum.API.Photos.Entities
{
    public class ReplyPhoto : Photo
    {
        public int ReplyId { get; set; }

        [JsonIgnore]
        public Reply Reply { get; set; } = null!;
    }
}
