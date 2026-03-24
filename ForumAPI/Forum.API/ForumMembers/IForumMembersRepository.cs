using Forum.API.ForumMembers.DTOs;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;

namespace Forum.API.ForumMembers
{
    public interface IForumMembersRepository
    {
        Task<PaginationResult<ForumMemberDto>> GetMembersAsync(MemberParams memberParams);
    }
}