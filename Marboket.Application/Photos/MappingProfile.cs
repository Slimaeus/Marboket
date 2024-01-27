using AutoMapper;
using Marboket.Domain.Entities;

namespace Marboket.Application.Photos;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Photo, string>()
            .ConvertUsing(x => x.Url);
    }
}

