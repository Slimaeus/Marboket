namespace Marboket.Application.Accounts.Dtos;

public sealed record RegisterDto(string Email, string UserName, string Password, string ConfirmPassword);

