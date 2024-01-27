namespace Marboket.Application.Common.Dtos;

public interface IEntityDto<TId>
{
    TId Id { get; }
}

public interface IEntityDto : IEntityDto<Guid?>;