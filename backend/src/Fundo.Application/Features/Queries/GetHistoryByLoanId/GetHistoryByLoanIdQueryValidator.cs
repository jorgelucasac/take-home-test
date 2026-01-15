using FluentValidation;
using Fundo.Application.Pagination;

namespace Fundo.Application.Features.Queries.GetHistoryByLoanId;

public class GetHistoryByLoanIdQueryValidator : AbstractValidator<GetHistoryByLoanIdQuery>
{
    private readonly PaginationQueryValidator _paginationValidator = new();

    public GetHistoryByLoanIdQueryValidator()
    {
        RuleFor(x => x.LoanId)
            .GreaterThan(0).WithMessage("LoanId must be greater than 0.");

        RuleFor(x => x.Pagination).SetValidator(_paginationValidator);
    }
}
