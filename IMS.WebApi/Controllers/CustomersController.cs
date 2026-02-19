using IMS.Application.Common.Models;
using IMS.Application.Features.Customers.Commands.AssignCustomerModem;
using IMS.Application.Features.Customers.Commands.UpdateCustomerStatus;
using IMS.Application.Features.Customers.Queries;
using IMS.Application.Features.Customers.Queries.GetCustomerById;
using IMS.Application.Features.Invoices.Queries;
using IMS.Application.Features.Invoices.Queries.GetCustomerInvoices;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        [HttpPost("update-status")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomerStatus([FromBody]UpdateCustomerStatusCommand command)
        {
            var result =  await Mediator.Send(command);

            if (result)
                return Ok(ApiResponse<object>.Success(message: $"Customer status successfully update to {command.Status}"));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to update customer status"));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<CustomerDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<CustomerDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers([FromQuery] GetCustomersQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<CustomersListWithStatusCounts>.Success(result);

            if (result.Items.Count == 0)
                response.Message = "No customer found.";

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerById([FromRoute] string id)
        {
            var query = new GetCustomerByIdQuery(id);

            var result = await Mediator.Send(query);

            if (result != null)
                return Ok(ApiResponse<CustomerDto>.Success(result));

            return StatusCode(204, ApiResponse<CustomerDto>.Success(result, $"Customer with id {id} is not found."));
        }

        [HttpGet("{customerId}/invoices")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<InvoiceDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<InvoiceDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerInvoices([FromRoute] string customerId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var query = new GetCustomerInvoicesQuery(customerId, pageNumber, pageSize);

            var result = await Mediator.Send(query);

            if (result.Items.Count > 0)
                return Ok(ApiResponse<PaginatedList<InvoiceDto>>.Success(result));

            return StatusCode(204, ApiResponse<PaginatedList<InvoiceDto>>.Success(result, "No Customer's Invoices found."));
        }

        [HttpPost("assign-modem")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto?>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignCustomerModem([FromBody] AssignCustomerModemCommand command)
        {
            var result = await Mediator.Send(command);

            if (result is not null)
                return Ok(ApiResponse<CustomerDto?>.Success(result));

            return StatusCode(204, ApiResponse<CustomerDto?>.Success(result, "Failed to assign modem."));
        }
    }
}
