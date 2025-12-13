using Fundo.Application.Handlers.Results;
using MediatR;

namespace Fundo.Application.Handlers.Commands.CreateLoan;

public record CreateLoanCommand(decimal Amount, decimal CurrentBalance, string ApplicantName) : IRequest<Result<CreateLoanResponse>>;