using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class Photo : BaseEntity<string>
{
    public Photo()
    {
        Id = Guid.NewGuid().ToString();
    }
    public Photo(string url) : this(Guid.NewGuid().ToString(), url)
    {
    }

    public Photo(string id, string url)
    {
        Id = id;
        Url = url;
    }
    public string Url { get; set; } = string.Empty;

    public Guid ProductId { get; set; }
    public virtual Product? Product { get; set; }
}

