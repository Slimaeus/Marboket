namespace Marboket.Application.Prices.Dtos;

public sealed record CreatePriceDto(double PricePerUnit, double UnitAmount, bool? IsActive, Guid ProductId, int ItemUnitId);

