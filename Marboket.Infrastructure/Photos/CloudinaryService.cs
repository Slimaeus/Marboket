using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Marboket.Infrastructure.Photos;

public sealed class CloudinaryService(Cloudinary cloudinary) : IPhotoService
{
    public async Task<(bool, (string, string)?)> AddPhoto(IFormFile file, string folder = "")
    {
        if (file is { Length: > 0 })
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                Folder = folder,
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("crop")
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return (true, (uploadResult.PublicId, uploadResult.SecureUrl.ToString()));
        }

        return (false, null);
    }

    public async Task<(bool, string?)> DeletePhoto(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok"
            ? (true, result.Result)
            : (false, null);
    }
}