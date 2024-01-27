namespace Marboket.Application.Prices.Dtos;

public sealed record UpdatePriceDto(double? PricePerUnit, double? UnitAmount, bool? IsActive, int? ItemUnitId);
