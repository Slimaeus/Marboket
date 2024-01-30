namespace Marboket.Application.Accounts.Dtos;

public sealed record LoginDto(string UserName, string Password, bool IsPersistent);