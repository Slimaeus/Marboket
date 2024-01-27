using Marboket.Application.Common.Dtos;

namespace Marboket.Application.Products.Dtos;

public sealed record ProductInPriceDto(Guid? Id, string? Name, string? Description, string? FriendlyUrlName) : IEntityDto;

