using AutoMapper;
using Forum.API.Posts.DTOs;

namespace Forum.API.Posts
{
    public class PostsProfile : Profile
    {
        public PostsProfile()
        {
            CreateMap<CreatePostDto, Post>();
            CreateMap<UpdatePostDto, Post>();
            CreateMap<Post, PostDto>();
        }
    }
}
