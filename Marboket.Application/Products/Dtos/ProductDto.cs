using Marboket.Application.Common.Dtos;
using Marboket.Application.Prices.Dtos;

namespace Marboket.Application.Products.Dtos;

public sealed record ProductDto(Guid? Id, string? Name, string? Description, string? FriendlyUrlName, List<PriceDto>? Prices, List<string> Photos) : IEntityDto;

