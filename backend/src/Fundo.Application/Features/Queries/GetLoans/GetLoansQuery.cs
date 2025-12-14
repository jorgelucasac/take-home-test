using Fundo.Application.Features.Shared;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Features.Queries.GetLoans;

public record class GetLoansQuery : IRequest<Result<IEnumerable<LoanResponse>>>;