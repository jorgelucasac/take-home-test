using Fundo.Application.Features.Results;
using Fundo.Application.Features.Shared;
using MediatR;

namespace Fundo.Application.Features.Queries.GetLoanById;

public record GetLoanByIdQuery(Guid Id) : IRequest<Result<LoanResponse>>;