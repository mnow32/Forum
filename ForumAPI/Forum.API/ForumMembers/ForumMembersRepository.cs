using AutoMapper;
using Forum.API.Data;
using Forum.API.ForumMembers.DTOs;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.ForumMembers
{
    public class ForumMembersRepository(ForumDbContext dbContext, IMapper mapper) : IForumMembersRepository
    {
        public async Task<PaginationResult<ForumMemberDto>> GetMembersAsync(MemberParams memberParams)
        {
            var query = dbContext.Members.AsQueryable().AsNoTracking(); 

            query = query.Where(m => m.Id != memberParams.CurrentMemberId);
            if(memberParams.DisplayName.Length > 0)
            {
                query = query.Where(m => m.DisplayName.Contains(memberParams.DisplayName));
            }

            var result = await PaginationHelper.CreatePagingAsync(query, memberParams.PageNumber, memberParams.PageSize);
            
            //var memberDtos = mapper.Map<IEnumerable<ForumMemberDto>>(result.Items);
            return new PaginationResult<ForumMemberDto>
            {
                Metadata = result.Metadata,
                Items = mapper.Map<List<ForumMemberDto>>(result.Items)
            };
        }
    }
}
