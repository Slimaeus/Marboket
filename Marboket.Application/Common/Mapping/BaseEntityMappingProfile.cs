using AutoMapper;

namespace Marboket.Application.Common.Mapping;

public abstract class BaseEntityMappingProfile<TEntity, TDto, TCreateDto, TUpdateDto>
    : Profile
{
    public BaseEntityMappingProfile()
    {
        CreateMap<TEntity, TDto>();
        CreateMap<TCreateDto, TEntity>();
        CreateMap<TUpdateDto, TEntity>()
            .ForAllMembers(options => options.Condition((src, des, srcValue, desValue) => srcValue is not null));
    }
}

