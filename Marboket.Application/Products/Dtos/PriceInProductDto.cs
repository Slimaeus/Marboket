using Marboket.Application.Common.Dtos;
using Marboket.Application.ItemUnits.Dtos;

namespace Marboket.Application.Prices.Dtos;

public sealed record PriceInProductDto(Guid? Id, double? PricePerUnit, double? UnitAmount, bool? IsActive, ItemUnitDto? ItemUnit) : IEntityDto;

