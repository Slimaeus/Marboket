﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marboket.Application.Prices.Dtos;
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

        var pricesGroup = IdGroup.MapGroup("Prices");
        pricesGroup.MapPost("", HandleAddPrice);
        pricesGroup.MapDelete("{priceId:guid}", HandleRemovePrice);
    }
    public override IQueryable<Product> Filter(IQueryable<Product> source, string searchString)
        => source.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
    private static string _photoPath = "marboket/products";

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

        var (path, url) = data.Value;

        var photoId = path.Replace(_photoPath + "/", "");

        product.AddPhoto(photoId, url);

        await context.SaveChangesAsync(cancellationToken);

        var productDto = await context.Products
            .Where(x => x.Id.Equals(product.Id))
            .AsNoTracking()
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return TypedResults.Ok(productDto);
    }

    private async Task<Results<Ok<ProductDto>, NotFound, BadRequest<string>>> HandleRemovePhoto(
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

        var isRemoveSuccess = product.RemovePhoto(decodedPhotoId);

        if (!isRemoveSuccess)
        {
            return TypedResults.NotFound();
        }

        await context.SaveChangesAsync(cancellationToken);

        var productDto = await context.Products
            .Where(x => x.Id.Equals(product.Id))
            .AsNoTracking()
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return TypedResults.Ok(productDto);
    }

    private async Task<Results<Ok<ProductDto>, NotFound, BadRequest>> HandleAddPrice(
        [FromRoute] Guid id,
        [FromBody] CreatePriceDto request,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var product = await context.Products
            .Include(x => x.Prices)
            .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

        if (product is null)
        {
            return TypedResults.NotFound();
        }

        Price price = new(request.ItemUnitId, request.UnitAmount, request.PricePerUnit)
        {
            ProductId = product.Id
        };
        await context.Prices.AddAsync(price, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        var productDto = await context.Products
            .Where(x => x.Id.Equals(product.Id))
            .AsNoTracking()
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return TypedResults.Ok(productDto);
    }

    private async Task<Results<Ok<PriceDto>, NotFound, BadRequest>> HandleRemovePrice(
        [FromRoute] Guid id,
        [FromRoute] Guid priceId,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var product = await context.Products
            .Include(x => x.Prices)
            .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        if (product is null)
        {
            return TypedResults.NotFound();
        }

        var price = product.RemovePrice(priceId);

        if (price is null)
        {
            return TypedResults.NotFound();
        }

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(mapper.Map<PriceDto>(price));
    }

    protected override void UpdateEntityBeforeAdd(Product entity, CreateProductDto request)
    {
        entity.AddPrice(request.ItemUnitId, request.UnitAmount, request.Price);
        if (!string.IsNullOrEmpty(request.PhotoUrl))
            entity.AddPhoto(request.PhotoUrl);
    }
}
