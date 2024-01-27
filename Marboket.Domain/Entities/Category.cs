using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class Category : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string? FriendlyUrlName { get; set; }

    public int ParentCategoryId { get; set; }
    public virtual Category? ParentCategory { get; set; }

    public virtual ICollection<Category> ChildCategories { get; set; } = new HashSet<Category>();
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new HashSet<ProductCategory>();
}

