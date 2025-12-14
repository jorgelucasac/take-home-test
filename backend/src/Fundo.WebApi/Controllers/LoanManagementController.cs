using Fundo.Application.Features.Commands.CreateLoan;
using Fundo.Application.Features.Results;
using Fundo.Application.Features.Shared;
using Fundo.Application.Handlers.Commands.ApplyPayment;
using Fundo.Application.Handlers.Queries.GetLoanById;
using Fundo.Application.Handlers.Queries.GetLoans;
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType<LoanResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
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
    public async Task<IActionResult> CreateAsync([FromBody] CreateLoanCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtAction("GetById", new { id = result.Value!.Id }, result.Value);
        }

        return HandlerErrorResponse(result.Error!);
    }

    [HttpPost("{id:guid}/payment")]
    [ProducesResponseType<LoanResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> PaymentAsync(Guid id, [FromBody] PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var command = new ApplyPaymentCommand(id, request.Amount);
        var result = await _mediator.Send(command, cancellationToken);
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