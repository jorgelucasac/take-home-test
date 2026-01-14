using Fundo.Application.Features.Queries.GetHistoryByLoanId;
using Fundo.Application.Features.Queries.GetLoanById;
using Fundo.Application.Features.Queries.GetLoans;
using Fundo.Application.Features.Shared;
using Fundo.Application.Pagination;
using Fundo.Application.Results;
using Fundo.WebApi.Transport.Rerquest;
using Fundo.WebApi.Transport.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.WebApi.Controllers;

[Route("/loans")]
[ApiController]
[ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
public class LoanManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoanManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<LoanResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetLoansQuery();
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return HandlerErrorResponse(result.Error!);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<LoanResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var query = new GetLoanByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return HandlerErrorResponse(result.Error!);
    }

    [HttpPost]
    [ProducesResponseType<LoanResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateLoanRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(request.ToCommand(), cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction("GetById", new { id = result.Value!.Id }, result.Value);
        }

        return HandlerErrorResponse(result.Error!);
    }

    [HttpPost("{id:int}/payment")]
    [ProducesResponseType<LoanResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> PaymentAsync(int id, [FromBody] PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(request.ToCommand(id), cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return HandlerErrorResponse(result.Error!);
    }

    [HttpGet("{id:int}/histories")]
    [ProducesResponseType<PaginatedResponse<LoanHistoryResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistoryAsync(int id, [FromQuery] PaginationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetHistoryByLoanIdQuery(id, request.PageNumber, request.PageSize), cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return HandlerErrorResponse(result.Error!);
    }

    protected IActionResult HandlerErrorResponse(Error error)
    {
        return error.Type switch
        {
            ErrorType.Failure => BadRequest(ErrorResponse.From(error)),
            ErrorType.Validation => BadRequest(ErrorResponse.From(error)),
            ErrorType.NotFound => NotFound(ErrorResponse.From(error)),
            _ => BadRequest(ErrorResponse.From(error))
        };
    }
}