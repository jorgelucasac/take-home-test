using Fundo.Application.Features.Results;
using Fundo.Application.Features.Shared;
using MediatR;

namespace Fundo.Application.Features.Queries.GetLoans;

public record class GetLoansQuery : IRequest<Result<IEnumerable<LoanResponse>>>;