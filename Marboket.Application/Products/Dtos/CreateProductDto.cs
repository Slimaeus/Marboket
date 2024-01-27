namespace Marboket.Application.Products.Dtos;

public sealed record CreateProductDto(string Name, string? Description, string? FriendlyUrlName, int ItemUnitId, double UnitAmount, double Price, string? PhotoUrl);

