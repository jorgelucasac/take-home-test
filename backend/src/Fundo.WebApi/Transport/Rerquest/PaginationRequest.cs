namespace Fundo.WebApi.Transport.Rerquest;

public sealed record PaginationRequest(int PageNumber = 1, int PageSize = 20);