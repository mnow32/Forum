using Forum.API.Topics;
using System.Text.Json.Serialization;

namespace Forum.API.Photos.Entities
{
    public class TopicPhoto : Photo
    {
        public int TopicId { get; set; }

        [JsonIgnore]
        public Topic Topic { get; set; } = null!;
    }
}
