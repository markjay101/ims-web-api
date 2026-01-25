using IMS.Application.Features.Invoices.Commands.UpdateInvoiceStatus;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class InvoicesController : ApiControllerBase
    {
        [HttpPost("update-status")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInvoiceStatus([FromBody] UpdateInvoiceStatusCommand command)
        {
            var result = await Mediator.Send(command);

            if (result)
                return Ok(ApiResponse<object>.Success(message: $"Invoice status successfully update to {command.Status}"));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to update invoice status"));
        }
    }
}
