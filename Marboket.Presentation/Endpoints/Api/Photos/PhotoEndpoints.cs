using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marboket.Application.Photos.Dtos;
using Marboket.Application.Products.Dtos;
using Marboket.Domain.Entities;
using Marboket.Infrastructure.Photos;
using Marboket.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Marboket.Presentation.Endpoints.Api.Photos;

public class PhotoEndpoints(RouteGroupBuilder apiGroup)
    : EntityEndpoints<string, Photo, PhotoDto>("Photos", apiGroup)
{
    public override void MapEndpoints()
    {
        base.MapEndpoints();

        EntityGroup.MapPost("", HandleCreatePhoto)
            .DisableAntiforgery();

        IdGroup.MapDelete("", HandleRemovePhoto);
    }

    private static string _photoPath = "marboket/products";

    private async Task<Results<Created<PhotoDto>, NotFound, BadRequest>> HandleCreatePhoto(
        [FromForm] Guid productId,
        [FromForm] IFormFile file,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IPhotoService photoService,
        CancellationToken cancellationToken)
    {
        var request = new CreatePhotoDto(productId, file);
        var product = await context.Products.FindAsync([request.ProductId], cancellationToken: cancellationToken);
        if (product is null)
        {
            return TypedResults.NotFound();
        }

        var (isSuccess, data) = await photoService.AddPhoto(request.File, _photoPath);
        if (!isSuccess || data is null)
        {
            return TypedResults.BadRequest();
        }

        var (path, url) = data.Value;

        var photoId = path.Replace(_photoPath + "/", "");

        product.AddPhoto(photoId, url);

        await context.SaveChangesAsync(cancellationToken);

        var photoDto = await context.Photos
            .Where(x => x.Id != null && x.Id.Equals(photoId))
            .ProjectTo<PhotoDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return TypedResults.Created($"api/{GroupName}", photoDto);
    }

    private async Task<Results<Ok<ProductDto>, NotFound<string>, BadRequest<string>>> HandleRemovePhoto(
    [FromRoute] string id,
    [FromServices] ApplicationDbContext context,
    [FromServices] IMapper mapper,
    [FromServices] IPhotoService photoService,
    CancellationToken cancellationToken)
    {
        var decodedPhotoId = WebUtility.UrlDecode(id);


        var photo = await context.Photos
            .FindAsync([decodedPhotoId], cancellationToken);

        if (photo is null)
        {
            return TypedResults.NotFound($"Photo is null {decodedPhotoId}");
        }

        var fullPath = _photoPath + "/" + decodedPhotoId;

        var (isSuccess, message) = await photoService.DeletePhoto(fullPath);
        if (!isSuccess || message is null)
        {
            return TypedResults.BadRequest(message);
        }

        context.Remove(photo);
        await context.SaveChangesAsync(cancellationToken);

        var productDto = await context.Products
            .Where(x => x.Id.Equals(photo.ProductId))
            .AsNoTracking()
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return TypedResults.Ok(productDto);
    }
}
