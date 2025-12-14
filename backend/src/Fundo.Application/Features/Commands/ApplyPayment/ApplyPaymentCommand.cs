using Fundo.Application.Features.Results;
using Fundo.Application.Features.Shared;
using MediatR;

namespace Fundo.Application.Features.Commands.ApplyPayment;

public record ApplyPaymentCommand(Guid Id, decimal Amount) : IRequest<Result<LoanResponse>>;