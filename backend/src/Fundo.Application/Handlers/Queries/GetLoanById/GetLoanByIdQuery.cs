using Fundo.Application.Handlers.Results;
using Fundo.Application.Handlers.Shared;
using MediatR;

namespace Fundo.Application.Handlers.Queries.GetLoanById;

public record GetLoanByIdQuery(Guid Id) : IRequest<Result<LoanResponse>>;