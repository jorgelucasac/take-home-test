using Fundo.Application.Features.Shared;
using Fundo.Application.Pagination;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Features.Queries.GetHistoryByLoanId;

public class GetHistoryByLoanIdQuery : IRequest<Result<PaginatedResponse<LoanHistoryResponse>>>
{
    public int LoanId { get; }
    public PaginationQuery Pagination { get; }

    public GetHistoryByLoanIdQuery(int loanId, int pageNumber = 1, int pageSize = 20)
    {
        LoanId = loanId;
        Pagination = new PaginationQuery(pageNumber, pageSize);
    }
}