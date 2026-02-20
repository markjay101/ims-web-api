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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomerStatus([FromBody]UpdateCustomerStatusCommand command)
        {
            var result =  await Mediator.Send(command);

            if (result == null) return NoContent();

            return Ok(ApiResponse<object>.Success(result, $"Customer status successfully update to {command.Status}"));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CustomerListWithStatusCounts>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers([FromQuery] GetCustomersQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<CustomerListWithStatusCounts>.Success(result);

            if (result.Items.Count == 0)
                response.Message = "No customer found.";

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerById([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new GetCustomerByIdQuery(id));

            var response = ApiResponse<CustomerDto>.Success(result);

            if (result == null)
                response.Message = $"Customer with id {id} is not found.";

            return Ok(response);
        }

        [HttpGet("{customerId:guid}/invoices")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<InvoiceDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerInvoices([FromRoute] Guid customerId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var query = new GetCustomerInvoicesQuery(customerId, pageNumber, pageSize);

            var result = await Mediator.Send(query);

            var response = ApiResponse<PaginatedList<InvoiceDto>>.Success(result);

            if (result.Items.Count == 0)
                response.Message = $"No invoices found for customer with id {customerId}.";

            return Ok(response);
        }

        [HttpPost("assign-modem")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CustomerDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignCustomerModem([FromBody] AssignCustomerModemCommand command)
        {
            var result = await Mediator.Send(command);

            var response = ApiResponse<CustomerDto?>.Success(result);

            if (result == null)
                response.Message = $"Customer with id {command.CustomerId} is not found.";

            return Ok(response);;
        }
    }
}
