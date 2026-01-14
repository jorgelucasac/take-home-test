namespace Fundo.WebApi.Transport.Requests;

public sealed record PaginationRequest(int PageNumber = 1, int PageSize = 20);