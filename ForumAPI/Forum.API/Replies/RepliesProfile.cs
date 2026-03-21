using AutoMapper;
using Forum.API.Replies.DTOs;

namespace Forum.API.Replies
{
    public class RepliesProfile : Profile
    {
        public RepliesProfile()
        {
            CreateMap<CreateReplyDto, Reply>();
            CreateMap<UpdateReplyDto, Reply>()
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(scr => DateTime.UtcNow));
            CreateMap<Reply, ReplyDto>();
        }
    }
}
