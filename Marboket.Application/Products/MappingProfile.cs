using Marboket.Application.Common.Mapping;
using Marboket.Application.Photos.Dtos;
using Marboket.Application.Prices.Dtos;
using Marboket.Application.Products.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Application.Products;

public sealed class MappingProfile
    : BaseEntityMappingProfile<Product, ProductDto, CreateProductDto, UpdateProductDto>
{
    public MappingProfile()
    {
        CreateMap<Price, PriceInProductDto>();
        CreateMap<Photo, PhotoInProductDto>();
    }
};

