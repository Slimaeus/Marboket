using Marboket.Application.ItemUnits.Dtos;
using Marboket.Domain.Entities;
using Marboket.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marboket.Presentation.Endpoints.Api.ItemUnits;

public sealed class ItemUnitEndpoints(RouteGroupBuilder group) : EntityEndpoints<int, ItemUnit, ItemUnitDto, CreateItemUnitDto, UpdateItemUnitDto>("Units", group)
{
    public override void MapEndpoints()
    {
        EntityGroup.MapPost("seed", async (ApplicationDbContext context) =>
        {
            await context.ItemUnits.ExecuteDeleteAsync();
            await context.ItemUnits.AddRangeAsync(
                [
                    new(1, "gram", "g"),
                    new(2, "kí", "kg"),
                    new(3, "lít", "l"),
                    new(4, "bao"),
                ]);

            await context.SaveChangesAsync();
        });
    }
}
