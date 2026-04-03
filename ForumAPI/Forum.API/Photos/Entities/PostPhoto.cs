using Forum.API.Posts;
using System.Text.Json.Serialization;

namespace Forum.API.Photos.Entities
{
    public class PostPhoto : Photo
    {
        public int PostId { get; set; }

        [JsonIgnore]
        public Post Post { get; set; } = null!;
    }
}
