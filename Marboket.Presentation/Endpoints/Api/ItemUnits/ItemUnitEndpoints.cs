using Marboket.Application.ItemUnits.Dtos;
using Marboket.Domain.Entities;
using Marboket.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marboket.Presentation.Endpoints.Api.ItemUnits;

public sealed class ItemUnitEndpoints(RouteGroupBuilder group)
    : EntityEndpoints<int, ItemUnit, ItemUnitDto, CreateItemUnitDto, UpdateItemUnitDto>("Units", group)
{
    public override IQueryable<ItemUnit> Filter(IQueryable<ItemUnit> source, string searchString)
        => source.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
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
                    new(5, "túi"),
                    new(6, "cây"),
                    new(7, "gói"),
                    new(8, "dây"),
                    new(9, "bịch"),
                    new(10, "chai"),
                    new(11, "lon"),
                    new(12, "hộp"),
                    new(13, "bình"),
                    new(14, "cục"),
                    new(15, "quả"),
                    new(16, "trái"),
                    new(17, "vỉ"),
                    new(18, "thùng"),
                    new(19, "khía"),
                    new(20, "hủ")
                ]);

            await context.SaveChangesAsync();
        });
    }
}
