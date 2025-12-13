using Fundo.Application.Handlers.Commands.CreateLoan;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.WebApi.Controllers
{
    [Route("/loan")]
    [ApiController]
    public class LoanManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoanManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<ActionResult> Get()
        {
            return Task.FromResult<ActionResult>(Ok());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] CreateLoanCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(PostAsync), new { id = result.Value!.Id }, result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}