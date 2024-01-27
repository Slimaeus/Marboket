namespace Marboket.Domain.Entities;

public class ProductCategory
{
    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }
}

