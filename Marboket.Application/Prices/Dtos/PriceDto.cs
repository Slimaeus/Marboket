using Marboket.Application.Common.Dtos;
using Marboket.Application.ItemUnits.Dtos;
using Marboket.Application.Products.Dtos;

namespace Marboket.Application.Prices.Dtos;

public sealed record PriceDto(Guid? Id, double? PricePerUnit, double? UnitAmount, bool? IsActive, ProductInPriceDto Product, ItemUnitDto? ItemUnit) : IEntityDto;

