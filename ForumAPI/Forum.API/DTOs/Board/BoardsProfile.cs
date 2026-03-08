using AutoMapper;
using Forum.API.Entities;

namespace Forum.API.DTOs
{
    public class BoardsProfile : Profile
    {
        public BoardsProfile()
        {
            CreateMap<CreateBoardDto, Board>();
            CreateMap<UpdateBoardDto, Board>()
                .ForMember(dest => dest.Name, options => options.Condition(src => src.Name is not null))
                .ForMember(dest => dest.Description, options => options.Condition(src => src.Description is not null));
            CreateMap<Board, BoardDto>();
        }
    }
}
