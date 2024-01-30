using Marboket.Application.Common.Dtos;

namespace Marboket.Application.Accounts.Dtos;

public sealed record AccountDto(Guid? Id, string? UserName, string? Email) : IEntityDto
{
    public string? Token { get; set; }
    public string? AvatarUrl { get; set; }
}

