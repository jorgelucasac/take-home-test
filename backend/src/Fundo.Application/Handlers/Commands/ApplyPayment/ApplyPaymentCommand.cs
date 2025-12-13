using Fundo.Application.Handlers.Results;
using Fundo.Application.Handlers.Shared;
using MediatR;

namespace Fundo.Application.Handlers.Commands.ApplyPayment;

public record ApplyPaymentCommand(Guid Id, decimal Amount) : IRequest<Result<LoanResponse>>;