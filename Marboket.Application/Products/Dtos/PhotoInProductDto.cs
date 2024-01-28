using Marboket.Application.Common.Dtos;

namespace Marboket.Application.Photos.Dtos;

public sealed record PhotoInProductDto(string? Id, string? Url) : IEntityDto<string?>;

