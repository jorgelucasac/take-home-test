using Fundo.Application.Features.Shared;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Features.Queries.GetLoanById;

public record GetLoanByIdQuery(int Id) : IRequest<Result<LoanResponse>>;