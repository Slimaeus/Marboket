using AutoMapper;
using Marboket.Application.Photos.Dtos;
using Marboket.Domain.Entities;
using Marboket.Infrastructure.Photos;
using Marboket.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Marboket.Presentation.Endpoints.Api;

public class PhotoEndpoints(RouteGroupBuilder apiGroup) : EntityEndpoints<string, Photo, PhotoDto>("Photos", apiGroup)
{
    public override void MapEndpoints()
    {
        base.MapEndpoints();

        IdGroup.MapDelete("", HandleRemovePhoto);
    }

    private async Task<Results<Ok<string>, NotFound, BadRequest<string>>> HandleRemovePhoto(
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
            return TypedResults.NotFound();
        }

        var (isSuccess, message) = await photoService.DeletePhoto(decodedPhotoId);
        if (!isSuccess || message is null)
        {
            return TypedResults.BadRequest(message);
        }

        context.Remove(photo);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(photo.Url);
    }
}
