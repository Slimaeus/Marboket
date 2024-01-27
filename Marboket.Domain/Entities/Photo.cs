using Marboket.Domain.Common;

namespace Marboket.Domain.Entities;

public class Photo : BaseEntity<string>
{
    public string Url { get; set; } = string.Empty;
}

