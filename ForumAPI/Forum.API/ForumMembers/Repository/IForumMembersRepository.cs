using Forum.API.ForumMembers.DTOs;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;

namespace Forum.API.ForumMembers.Repository
{
    public interface IForumMembersRepository
    {
        Task<PaginationResult<ForumMemberDto>> GetMembersAsync(MemberParams memberParams);
        Task<ForumMemberDto> GetCurrentMemberAsync(string memberId);
        Task UpdateMemberAsync(UpdateForumMemberDto updateForumMemberDto);
    }
}