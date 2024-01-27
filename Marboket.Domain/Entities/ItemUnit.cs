using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class ItemUnit : BaseEntity<int>
{
    public ItemUnit(string name, string? alias) : this(name)
    {
        Alias = alias;
    }
    public ItemUnit(string name) : this()
    {
        Name = name;
    }
    public ItemUnit()
    {

    }
    public string Name { get; set; } = string.Empty;
    public string? Alias { get; set; }
}

