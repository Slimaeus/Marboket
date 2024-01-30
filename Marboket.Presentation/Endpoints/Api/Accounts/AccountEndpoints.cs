using AutoMapper;
using Marboket.Application.Accounts.Dtos;
using Marboket.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Marboket.Presentation.Endpoints.Api.Accounts;

public sealed class AccountEndpoints : IEndpoints
{
    public AccountEndpoints(RouteGroupBuilder apiGroup)
    {
        var group = apiGroup.MapGroup("Accounts")
            .WithTags("Accounts");

        group.MapPost("register", HandleRegister);
        group.MapPost("login", HandleLogin);
    }

    private async Task<Results<Ok<AccountDto>, BadRequest<IdentityResult>>> HandleRegister(
        [FromBody] RegisterDto registration,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {

        var email = registration.Email;

        if (string.IsNullOrEmpty(email))
        {
            return TypedResults.BadRequest(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        var user = new ApplicationUser
        {
            Email = email,
            UserName = registration.UserName
        };
        var result = await userManager.CreateAsync(user, registration.Password);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest(result);
        }

        var accountDto = mapper.Map<AccountDto>(user);

        return TypedResults.Ok(accountDto);
    }
    private async Task<Results<Ok<AccountDto>, UnauthorizedHttpResult>> HandleLogin(
        [FromBody] LoginDto login,
        [FromServices] UserManager<ApplicationUser> userManager,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(login.UserName);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }
        var result = await userManager.CheckPasswordAsync(user, login.Password);

        if (!result)
        {
            return TypedResults.Unauthorized();
        }

        var accountDto = mapper.Map<AccountDto>(user);
        return TypedResults.Ok(accountDto);
    }
}
