using Marboket.Application.Common.Mapping;
using Marboket.Application.ItemUnits.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Application.ItemUnits;

public sealed class MappingProfile : BaseEntityMappingProfile<ItemUnit, ItemUnitDto, CreateItemUnitDto, UpdateItemUnitDto>;
