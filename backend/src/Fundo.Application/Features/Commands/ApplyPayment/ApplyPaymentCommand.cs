using Fundo.Application.Features.Shared;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Features.Commands.ApplyPayment;

public record ApplyPaymentCommand(Guid Id, decimal Amount) : IRequest<Result<LoanResponse>>;