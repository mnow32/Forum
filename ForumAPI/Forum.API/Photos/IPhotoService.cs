using CloudinaryDotNet.Actions;

namespace Forum.API.Photos
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadMemberPhotoAsync(IFormFile photo);
        Task<DeletionResult> DeleteMemberPhotoAsync(string publicId);
        Task<IEnumerable<ImageUploadResult>> BulkUploadContentPhotosAsync(IFormFileCollection photos);
    }
}
