using AutoMapper;
using Forum.API.ForumMembers.DTOs;

namespace Forum.API.ForumMembers
{
    public class ForumMembersProfile : Profile
    {
        public ForumMembersProfile()
        {
            CreateMap<ForumMember, ForumMemberDto>();
        }
    }
}
