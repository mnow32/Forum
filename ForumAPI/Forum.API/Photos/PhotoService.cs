using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Forum.API.Photos
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            Account account = new(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }
        public async Task<ImageUploadResult> UploadMemberPhotoAsync(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();
            if(photo.Length > 0 )
            {
                await using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation().Height(200).Width(200).Crop("fill"),
                    Folder = "member-photos"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeleteMemberPhotoAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<IEnumerable<ImageUploadResult>> BulkUploadContentPhotosAsync(IFormFileCollection photos)
        {
            
            var uploadTasks = photos.Select(async photo =>
            {
                await using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill"),
                    Folder = "content-photos"
                };
                return await _cloudinary.UploadAsync(uploadParams);
            });
            var results = await Task.WhenAll(uploadTasks);
            return results;
        }

        public async Task<IEnumerable<DeletionResult>> BulkDeleteContentPhotosAsync(IEnumerable<string> publicIds)
        {
            var deleteTasks = new List<Task<DeletionResult>>();
            foreach (var publicId in publicIds)
            {
                deleteTasks.Add(DeleteMemberPhotoAsync(publicId));
            }
            var result = await Task.WhenAll(deleteTasks);
            return result;
        }

    }
}
