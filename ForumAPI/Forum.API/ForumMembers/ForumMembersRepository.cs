using AutoMapper;
using Forum.API.Data;
using Forum.API.ForumMembers.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.ForumMembers
{
    public class ForumMembersRepository(ForumDbContext dbContext, IMapper mapper) : IForumMembersRepository
    {
        public async Task<IEnumerable<ForumMemberDto>> GetAllMembersAsync()
        {
            var members = await dbContext.Members.AsNoTracking().ToListAsync();
            var memberDtos = mapper.Map<IEnumerable<ForumMemberDto>>(members);
            return memberDtos;
        }
    }
}
