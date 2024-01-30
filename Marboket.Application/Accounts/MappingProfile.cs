using AutoMapper;
using Marboket.Application.Accounts.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Application.Accounts;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, AccountDto>();
    }
}

