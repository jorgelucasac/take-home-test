using Fundo.Application.Features.Commands.CreateLoan;

namespace Fundo.WebApi.Transport.Rerquest;

public record CreateLoanRequest(decimal Amount, decimal CurrentBalance, string ApplicantName)
{
    public CreateLoanCommand ToCommand()
        => new(Amount, CurrentBalance, ApplicantName);
};