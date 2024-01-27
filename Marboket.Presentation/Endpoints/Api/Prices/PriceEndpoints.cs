using Marboket.Application.Prices.Dtos;
using Marboket.Domain.Entities;

namespace Marboket.Presentation.Endpoints.Api.Prices;

public sealed class PriceEndpoints(RouteGroupBuilder group) : EntityEndpoints<Guid, Price, PriceDto, CreatePriceDto, UpdatePriceDto>("Prices", group);