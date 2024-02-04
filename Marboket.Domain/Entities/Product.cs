using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? FriendlyUrlName { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
    public virtual ICollection<Price> Prices { get; set; } = new HashSet<Price>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>();

    public void AddPrice(int itemUnitId, double amount, double price)
        => Prices.Add(new(itemUnitId, amount, price));
    public void AddPhoto(string url)
        => Photos.Add(new(url));
    public void AddPhoto(string id, string url)
        => Photos.Add(new(id, url));
    public bool RemovePhoto(string id)
    {
        var photo = Photos.SingleOrDefault(p => p.Id == id);
        if (photo is null) return false;
        Photos.Remove(photo);
        return true;
    }
    public Price? RemovePrice(Guid id)
    {
        var price = Prices.SingleOrDefault(p => p.Id == id);
        if (price is null) return null;
        Prices.Remove(price);
        return price;
    }
}

