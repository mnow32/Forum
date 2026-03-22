using AutoMapper;
using Forum.API.Topics.DTOs;

namespace Forum.API.Topics
{
    public class TopicsProfile : Profile
    {
        public TopicsProfile()
        {
            CreateMap<CreateTopicDto, Topic>();
            CreateMap<UpdateTopicDto, Topic>()
                .ForMember(dest => dest.Title, options => options.Condition(src => src.Title is not null))
                .ForMember(dest => dest.Description, options => options.Condition(src => src.Description is not null))
                .ForMember(dest => dest.UpdatedAt, options => options.MapFrom(scr => DateTime.UtcNow)); ; 
            CreateMap<Topic, TopicDto>();
        }
    }
}
