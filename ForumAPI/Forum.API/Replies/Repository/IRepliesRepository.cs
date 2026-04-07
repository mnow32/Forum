using Forum.API.Replies.DTOs;

namespace Forum.API.Replies.Repository
{
    public interface IRepliesRepository
    {
        Task<int> CreateReplyAsync(CreateReplyDto createReplyDto);
        Task DeleteReplyAsync(int replyId);
        Task<IEnumerable<ReplyDto>> GetRepliesByPostIdAsync(int postId);
        Task UpdateReplyAsync(int replyId, UpdateReplyDto updateReplyDto);
    }
}