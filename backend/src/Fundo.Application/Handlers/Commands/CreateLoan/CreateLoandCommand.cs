using Fundo.Application.Handlers.Results;
using Fundo.Application.Handlers.Shared;
using MediatR;

namespace Fundo.Application.Handlers.Commands.CreateLoan;

public record CreateLoanCommand(decimal Amount, decimal CurrentBalance, string ApplicantName) : IRequest<Result<LoanResponse>>;