using Marboket.Application.Common.Dtos;

namespace Marboket.Application.Photos.Dtos;

public sealed record ProductInPhotoDto(Guid? Id, string? Name, string? Description, string? FriendlyUrlName) : IEntityDto;