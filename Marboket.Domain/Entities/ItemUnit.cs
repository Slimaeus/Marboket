using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class ItemUnit : BaseEntity<int>
{
    public ItemUnit(int id, string name, string? alias) : this(id, name)
    {
        Alias = alias;
    }
    public ItemUnit(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }
    public ItemUnit()
    {

    }
    public string Name { get; set; } = string.Empty;
    public string? Alias { get; set; }
}

