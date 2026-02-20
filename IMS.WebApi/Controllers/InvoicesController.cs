using IMS.Application.Features.Invoices.Commands.UpdateInvoiceStatus;
using IMS.Application.Features.Invoices.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class InvoicesController : ApiControllerBase
    {
        [HttpPost("update-status")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<InvoiceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInvoiceStatus([FromBody] UpdateInvoiceStatusCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == null)
                return NoContent();

            return Ok(ApiResponse<InvoiceDto>.Success(result, $"Invoice status successfully update to {command.Status}"));
        }
    }
}
