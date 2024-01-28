using Marboket.Application.Common.Dtos;
using Marboket.Application.Photos.Dtos;
using Marboket.Application.Prices.Dtos;

namespace Marboket.Application.Products.Dtos;

public sealed record ProductDto(Guid? Id, string? Name, string? Description, string? FriendlyUrlName, List<PriceInProductDto>? Prices, List<PhotoInProductDto> Photos) : IEntityDto;
