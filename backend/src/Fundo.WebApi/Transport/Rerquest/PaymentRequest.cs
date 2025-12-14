using Fundo.Application.Features.Commands.ApplyPayment;

namespace Fundo.WebApi.Transport.Rerquest;

public record PaymentRequest(decimal Amount)
{
    public ApplyPaymentCommand ToCommand(Guid loanId)
        => new(loanId, Amount);
}