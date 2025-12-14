using FluentValidation;

namespace Fundo.Application.Features.Queries.GetLoanById;

public class GetLoanByIdQueryValidator : AbstractValidator<GetLoanByIdQuery>
{
    public GetLoanByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Loan ID must be provided.");
    }
}