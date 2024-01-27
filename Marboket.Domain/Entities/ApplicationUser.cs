using Marboket.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Marboket.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IEntity
{
}

