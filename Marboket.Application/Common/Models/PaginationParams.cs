namespace Marboket.Application.Common.Models;

public record PaginationParams(
    int? PageNumber = 1,
    int? PageSize = 100,
    string? SearchString = null);