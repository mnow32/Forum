using AutoMapper;
using Forum.API.Data;
using Forum.API.Exceptions.Models;
using Forum.API.ForumMembers.DTOs;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Photos;
using Forum.API.Photos.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.ForumMembers.Repository
{
    public class ForumMembersRepository(ForumDbContext dbContext, IPhotoService photoService, IMapper mapper) : IForumMembersRepository
    {
        public async Task<PaginationResult<ForumMemberDto>> GetMembersAsync(MemberParams memberParams)
        {
            var query = dbContext.Members.AsQueryable().Include(m => m.Photo).OrderBy(m => m.CreatedAt).AsNoTracking(); 

            query = query.Where(m => m.Id != memberParams.CurrentMemberId);
            if(memberParams.DisplayName.Length > 0)
            {
                query = query.Where(m => m.DisplayName.Contains(memberParams.DisplayName));
            }

            var result = await PaginationHelper.CreatePagingAsync(query, memberParams.PageNumber, memberParams.PageSize);
            
            return new PaginationResult<ForumMemberDto>
            {
                Metadata = result.Metadata,
                Items = mapper.Map<List<ForumMemberDto>>(result.Items)
            };
        }

        public async Task<ForumMemberDto> GetCurrentMemberAsync(string memberId)
        {
            var member = await dbContext.Members.Include(m => m.Photo).AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId);
            if(member is null)
            {
                throw new NotFoundException($"Read failed - couldn't find Member with id: {memberId}");
            }
            var memberDto = mapper.Map<ForumMemberDto>(member);
            return memberDto;
        }

        public async Task UpdateMemberAsync(UpdateForumMemberDto updateForumMemberDto)
        {
            var member = await dbContext.Members.Include(m => m.Photo).FirstOrDefaultAsync(m => m.Id == updateForumMemberDto.Id);
            if(member is null)
            {
                throw new NotFoundException($"Update failed - couldn't find Member with id: {updateForumMemberDto.Id}");
            }
            mapper.Map(updateForumMemberDto, member);
            if(updateForumMemberDto.Photo is not null)
            {
                if(member.Photo is not null)
                {
                    dbContext.Photos.Remove(member.Photo);
                }
                var uploadResult = await photoService.UploadMemberPhotoAsync(updateForumMemberDto.Photo);
                if(uploadResult.Error is not null)
                {
                    throw new CloudinaryException(uploadResult.Error.Message);
                }
                member.Photo = new MemberPhoto()
                {
                    Url = uploadResult.SecureUrl.AbsoluteUri,
                    PublicId = uploadResult.PublicId,
                };
            }
            await dbContext.SaveChangesAsync();

        }

    }
}
