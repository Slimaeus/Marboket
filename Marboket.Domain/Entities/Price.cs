using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class Price : BaseEntity
{
    public Price()
    {

    }

    public Price(int itemUnitId, double amount, double price)
    {
        ItemUnitId = itemUnitId;
        UnitAmount = amount;
        PricePerUnit = price;
    }
    public double PricePerUnit { get; set; }
    public double UnitAmount { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public int ItemUnitId { get; set; }
    public virtual ItemUnit? ItemUnit { get; set; }
}

