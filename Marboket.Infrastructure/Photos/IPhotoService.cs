using Microsoft.AspNetCore.Http;

namespace Marboket.Infrastructure.Photos;
public interface IPhotoService
{
    Task<(bool, (string, string)?)> AddPhoto(IFormFile file, string folder = "");
    Task<(bool, string?)> DeletePhoto(string publicId);
}