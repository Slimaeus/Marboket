using AutoMapper;
using Marboket.Application.Photos.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Application.Photos;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Photo, PhotoDto>();

        CreateMap<Photo, string>()
            .ConvertUsing(x => x.Url);
        CreateMap<Product, ProductInPhotoDto>();
    }
}

