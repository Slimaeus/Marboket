using Marboket.Application.Common.Dtos;

namespace Marboket.Application.Photos.Dtos;

public sealed record PhotoDto(string? Id, string? Url, ProductInPhotoDto? Product) : IEntityDto<string?>;

