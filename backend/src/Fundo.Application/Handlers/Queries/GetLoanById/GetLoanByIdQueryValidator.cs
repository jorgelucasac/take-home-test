using FluentValidation;

namespace Fundo.Application.Handlers.Queries.GetLoanById;

public class GetLoanByIdQueryValidator : AbstractValidator<GetLoanByIdQuery>
{
    public GetLoanByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Loan ID must be provided.");
    }
}