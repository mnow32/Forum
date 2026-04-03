using AutoMapper;
using Forum.API.Authentication.ForumUsers.DTOs;
using Forum.API.ForumMembers;

namespace Forum.API.Authentication.ForumUsers
{
    public class ForumUserProfile : Profile
    {
        public ForumUserProfile()
        {
            CreateMap<ForumUser, ForumUserDto>();

            CreateMap<RegisterDto, ForumUser>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.Email))
                .ForMember(dest => dest.Member, options => options.MapFrom(
                    src => new ForumMember
                    {
                        DisplayName = src.DisplayName,
                        Email = src.Email,
                    }));
        }
    }
}
