using Marboket.Application.Common.Mapping;
using Marboket.Application.Products.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Application.Products;

public sealed class MappingProfile : BaseEntityMappingProfile<Product, ProductDto, CreateProductDto, UpdateProductDto>;

