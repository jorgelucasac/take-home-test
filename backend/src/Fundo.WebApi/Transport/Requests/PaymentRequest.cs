using Fundo.Application.Features.Commands.ApplyPayment;

namespace Fundo.WebApi.Transport.Requests;

public record PaymentRequest(decimal Amount)
{
    public ApplyPaymentCommand ToCommand(int loanId)
        => new(loanId, Amount);
}