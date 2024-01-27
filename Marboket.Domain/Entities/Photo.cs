using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class Photo : BaseEntity<string>
{
    public Photo()
    {
        Id = Guid.NewGuid().ToString();
    }
    public Photo(string url)
    {
        Id = Guid.NewGuid().ToString();
        Url = url;
    }
    public string Url { get; set; } = string.Empty;

    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
}

