using Marboket.Application.Common.Mapping;
using Marboket.Application.Prices.Dtos;
using Marboket.Application.Products.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Application.Prices;

public sealed class MappingProfile : BaseEntityMappingProfile<Price, PriceDto, CreatePriceDto, UpdatePriceDto>
{
    public MappingProfile()
    {
        CreateMap<Product, ProductInPriceDto>();
    }
}

