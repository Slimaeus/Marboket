using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marboket.Application.Products.Dtos;
using Marboket.Domain.Entities;
using Marboket.Infrastructure.Photos;
using Marboket.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Marboket.Presentation.Endpoints.Api.Products;

public sealed class ProductEndpoints(RouteGroupBuilder group)
    : EntityEndpoints<Guid, Product, ProductDto, CreateProductDto, UpdateProductDto>("Products", group)
{
    public override void MapEndpoints()
    {
        var photosGroup = IdGroup.MapGroup("Photos");

        photosGroup.MapPost("", HandleAddPhoto)
            .DisableAntiforgery();

        photosGroup.MapDelete("{photoId}", HandleRemovePhoto);
    }

    private async Task<Results<Ok<ProductDto>, BadRequest, NotFound>> HandleAddPhoto(
        [FromRoute] Guid id,
        [FromForm] IFormFile file,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IPhotoService photoService,
        CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync([id], cancellationToken: cancellationToken);
        if (product is null)
        {
            return TypedResults.NotFound();
        }

        var (isSuccess, data) = await photoService.AddPhoto(file, "marboket/products");
        if (!isSuccess || data is null)
        {
            return TypedResults.BadRequest();
        }

        var (photoId, url) = data.Value;

        product.AddPhoto(photoId, url);

        await context.SaveChangesAsync(cancellationToken);

        var productDto = await context.Products
            .Where(x => x.Id.Equals(product.Id))
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return TypedResults.Ok(productDto);
    }

    private async Task<Results<Ok<string>, NotFound, BadRequest<string>>> HandleRemovePhoto(
        [FromRoute] Guid id,
        [FromRoute] string photoId,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IPhotoService photoService,
        CancellationToken cancellationToken)
    {
        var decodedPhotoId = WebUtility.UrlDecode(photoId);

        var product = await context.Products
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        if (product is null)
        {
            return TypedResults.NotFound();
        }

        if (!product.Photos.Any(x => string.Equals(x.Id, decodedPhotoId)))
        {
            return TypedResults.NotFound();
        }

        var (isSuccess, message) = await photoService.DeletePhoto(decodedPhotoId);
        if (!isSuccess || message is null)
        {
            return TypedResults.BadRequest(message);
        }

        var photo = product.RemovePhoto(decodedPhotoId);

        if (photo is null)
        {
            return TypedResults.NotFound();
        }

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(photo.Url);
    }

    protected override void UpdateEntityBeforeAdd(Product entity, CreateProductDto request)
    {
        entity.AddPrice(request.ItemUnitId, request.UnitAmount, request.Price);
        if (!string.IsNullOrEmpty(request.PhotoUrl))
            entity.AddPhoto(request.PhotoUrl);
    }
}
