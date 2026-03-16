using AutoMapper;
using Forum.API.Entities;
using Forum.API.ForumUsers.DTOs;

namespace Forum.API.ForumUsers
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
