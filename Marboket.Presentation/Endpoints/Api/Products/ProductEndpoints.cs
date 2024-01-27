using Marboket.Application.Products.Dtos;
using Marboket.Domain.Entities;
using Marboket.Infrastructure.Photos;
using Microsoft.AspNetCore.Mvc;

namespace Marboket.Presentation.Endpoints.Api.Products;

public sealed class ProductEndpoints(RouteGroupBuilder group)
    : EntityEndpoints<Guid, Product, ProductDto, CreateProductDto, UpdateProductDto>("Products", group)
{
    public override void MapEndpoints()
    {
        EntityGroup.MapGet("photo", async ([FromServices] IPhotoService photoService) =>
        {
            return (await photoService.DeletePhoto("a")).Item1;
        });
    }
    protected override void UpdateEntityBeforeAdd(Product entity, CreateProductDto request)
    {
        entity.AddPrice(request.ItemUnitId, request.UnitAmount, request.Price);
        if (!string.IsNullOrEmpty(request.PhotoUrl))
            entity.AddPhoto(request.PhotoUrl);
    }
}
