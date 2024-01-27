using Marboket.Application.Common.Dtos;

namespace Marboket.Application.ItemUnits.Dtos;

public sealed record ItemUnitDto(int? Id, string? Name, string? Alias) : IEntityDto<int?>;
