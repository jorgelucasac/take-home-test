using Fundo.Application.Features.Shared;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Features.Commands.CreateLoan;

public record CreateLoanCommand(decimal Amount, decimal CurrentBalance, string ApplicantName) : IRequest<Result<LoanResponse>>;