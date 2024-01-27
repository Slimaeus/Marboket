using Marboket.Application.Products.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Presentation.Endpoints.Api.Products;

public sealed class ProductEndpoints(RouteGroupBuilder group)
    : EntityEndpoints<Guid, Product, ProductDto, CreateProductDto, UpdateProductDto>("Products", group)
{
    protected override void UpdateEntityBeforeAdd(Product entity, CreateProductDto request)
    {
        entity.AddPrice(request.ItemUnitId, request.UnitAmount, request.Price);
    }
}
