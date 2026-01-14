using Fundo.Application.Features.Shared;
using Fundo.Application.Pagination;
using Fundo.Application.Repositories;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Features.Queries.GetHistoryByLoanId;

public sealed class GetHistoryByLoanIdHandler : IRequestHandler<GetHistoryByLoanIdQuery, Result<PaginatedResponse<LoanHistoryResponse>>>
{
    private readonly ILoanHistoryRepository _loanHistoryRepository;

    public GetHistoryByLoanIdHandler(ILoanHistoryRepository loanHistoryRepository)
    {
        _loanHistoryRepository = loanHistoryRepository;
    }

    public async Task<Result<PaginatedResponse<LoanHistoryResponse>>> Handle(GetHistoryByLoanIdQuery request, CancellationToken cancellationToken)
    {
        var paginatedHistories = await _loanHistoryRepository.GetByLoanIdPagedAsync(
            request.LoanId,
            request.Pagination,
            cancellationToken
        );

        if (paginatedHistories.IsEmpty)
        {
            return BuildEmptyHistoryResponse(request);
        }

        PaginatedResponse<LoanHistoryResponse> response = BuildPaginatedHistoryResponse(request, paginatedHistories);

        return Result.Success(response);
    }

    private static PaginatedResponse<LoanHistoryResponse> BuildPaginatedHistoryResponse(GetHistoryByLoanIdQuery request, PaginatedResponse<Domain.Entities.LoanHistory> paginatedHistories)
    {
        var historiesResponse = paginatedHistories.Items
            .Select(LoanHistoryResponse.FromEntity)
            .ToList();

        var response = PaginatedResponse<LoanHistoryResponse>.Create(
            historiesResponse,
            request.Pagination.PageNumber,
            request.Pagination.PageSize,
            paginatedHistories.TotalItems);
        return response;
    }

    private static Result<PaginatedResponse<LoanHistoryResponse>> BuildEmptyHistoryResponse(GetHistoryByLoanIdQuery request)
    {
        return Result.Success(
            PaginatedResponse<LoanHistoryResponse>.Empty(
                request.Pagination.PageNumber,
                request.Pagination.PageSize
            )
        );
    }
}