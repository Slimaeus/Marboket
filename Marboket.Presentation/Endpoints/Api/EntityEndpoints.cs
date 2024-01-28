using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marboket.Domain.Common;
using Marboket.Domain.Common.Collections;
using Marboket.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marboket.Presentation.Endpoints.Api;

public abstract class EntityEndpoints<TId, TEntity, TDto> : IEndpoints
    where TEntity : class, IEntity<TId>
{
    public EntityEndpoints(string groupName, RouteGroupBuilder apiGroup)
    {
        var group = apiGroup.MapGroup(groupName)
            .WithTags(groupName);
        group.MapGet("", HandleGetList);

        var type = typeof(TId).Name.ToLower() switch
        {
            "int32" => ":int",
            "string" => "",
            string value => $":{value}"
        };
        var idGroup = group.MapGroup($"{{id{type}}}");
        idGroup.MapGet("", HandleGet);

        EntityGroup = group;
        IdGroup = idGroup;

        MapEndpoints();
        GroupName = groupName;
    }

    public RouteGroupBuilder EntityGroup { get; set; }
    public RouteGroupBuilder IdGroup { get; set; }
    public string GroupName { get; }

    public virtual void MapEndpoints() { }

    private async Task<Ok<IPagedList<TDto>>> HandleGetList(
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper)
    {
        IList<TDto> entities = context.Set<TEntity>()
            .AsSplitQuery()
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .ToList();

        var pagedList = entities.ToPagedList(1, 10, await context.Set<TEntity>().CountAsync());
        return TypedResults.Ok(pagedList);
    }

    private async Task<Results<Ok<TDto>, NotFound>> HandleGet(
        [FromRoute] TId id,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var entityDto = await context.Set<TEntity>()
            .Where(x => x.Id != null && x.Id.Equals(id))
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);
        if (entityDto is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(mapper.Map<TDto>(entityDto));
    }
}

public abstract class EntityEndpoints<TId, TEntity, TDto, TCreateDto, TUpdateDto> : EntityEndpoints<TId, TEntity, TDto>
    where TEntity : class, IEntity<TId>
{
    public EntityEndpoints(string groupName, RouteGroupBuilder apiGroup) : base(groupName, apiGroup)
    {
        EntityGroup.MapPost("", HandleCreate);

        IdGroup.MapPut("", HandleUpdate);
        IdGroup.MapDelete("", HandleDelete);

    }
    private async Task<Created<TDto>> HandleCreate(
        [FromBody] TCreateDto request,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        TEntity entity = mapper.Map<TEntity>(request);
        context.Set<TEntity>().Add(entity);

        UpdateEntityBeforeAdd(entity, request);

        await context.SaveChangesAsync(cancellationToken);

        var entityDto = await context.Set<TEntity>()
            .Where(x => x.Id != null && x.Id.Equals(entity.Id))
            .ProjectTo<TDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        return TypedResults.Created($"api/{GroupName}", entityDto);
    }
    private async Task<Results<NotFound, NoContent>> HandleUpdate(
        [FromBody] TUpdateDto request,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper, [FromRoute] TId id,
        CancellationToken cancellationToken)
    {
        var entity = await context
            .Set<TEntity>()
            .FindAsync([id], cancellationToken: cancellationToken);
        if (entity is null)
        {
            return TypedResults.NotFound();
        }
        mapper.Map(request, entity);
        await context.SaveChangesAsync(cancellationToken);
        return TypedResults.NoContent();
    }
    private async Task<Results<NotFound, Ok<TDto>>> HandleDelete(
        [FromRoute] TId id,
        [FromServices] ApplicationDbContext context,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var entity = await context
            .Set<TEntity>()
            .FindAsync([id], cancellationToken: cancellationToken);
        if (entity is null)
        {
            return TypedResults.NotFound();
        }
        context.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(mapper.Map<TDto>(entity));
    }
    protected virtual void UpdateEntityBeforeAdd(TEntity entity, TCreateDto request) { }
}