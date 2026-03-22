using Forum.API.ForumMembers.DTOs;

namespace Forum.API.ForumMembers
{
    public interface IForumMembersRepository
    {
        Task<IEnumerable<ForumMemberDto>> GetAllMembersAsync();
    }
}