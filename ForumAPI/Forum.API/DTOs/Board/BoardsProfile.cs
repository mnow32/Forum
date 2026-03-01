using AutoMapper;
using Forum.API.Entities;

namespace Forum.API.DTOs
{
    public class BoardsProfile : Profile
    {
        public BoardsProfile()
        {
            CreateMap<CreateBoardDto, Board>();
            CreateMap<UpdateBoardDto, Board>();
            CreateMap<Board, BoardDto>();
        }
    }
}
