using FluentValidation;

namespace Fundo.Application.Features.Commands.ApplyPayment;

public class ApplyPaymentCommandValidator : AbstractValidator<ApplyPaymentCommand>
{
    public ApplyPaymentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Loan ID must be greater than zero.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than zero.");
    }
}