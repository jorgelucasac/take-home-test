using FluentValidation;

namespace Fundo.Application.Handlers.Commands.CreateLoan;

public sealed class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be > 0.");

        RuleFor(x => x.CurrentBalance)
            .GreaterThanOrEqualTo(0).WithMessage("CurrentBalance must be >= 0.")
            .LessThanOrEqualTo(x => x.Amount).WithMessage("CurrentBalance cannot exceed Amount.");

        RuleFor(x => x.ApplicantName)
            .NotEmpty().WithMessage("ApplicantName is required.")
            .MaximumLength(200).WithMessage("ApplicantName max length is 200.");
    }
}