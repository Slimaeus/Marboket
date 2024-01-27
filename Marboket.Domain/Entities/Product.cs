﻿using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? FriendlyUrlName { get; set; }
    public string? Description { get; set; }


    public virtual ICollection<Price> Prices { get; set; } = new HashSet<Price>();
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>();

    public void AddPrice(int itemUnitId, double amount, double price)
    {
        Prices.Add(new Price { ItemUnitId = itemUnitId, UnitAmount = amount, PricePerUnit = price });
    }
}

