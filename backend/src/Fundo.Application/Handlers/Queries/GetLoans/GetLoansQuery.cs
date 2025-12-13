using Fundo.Application.Handlers.Results;
using Fundo.Application.Handlers.Shared;
using MediatR;

namespace Fundo.Application.Handlers.Queries.GetLoans;

public record class GetLoansQuery : IRequest<Result<IEnumerable<LoanResponse>>>;