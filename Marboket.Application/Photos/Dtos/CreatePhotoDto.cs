using Microsoft.AspNetCore.Http;

namespace Marboket.Application.Photos.Dtos;

public sealed record CreatePhotoDto(Guid ProductId, IFormFile File);