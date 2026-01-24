using IMS.Application.Common.Models;
using IMS.Application.Features.Customers.Commands.UpdateCustomerStatus;
using IMS.Application.Features.Customers.Queries;
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

            if (result.Items.Count > 0)
                return Ok(ApiResponse<PaginatedList<CustomerDto>>.Success(result));

            return StatusCode(204, ApiResponse<PaginatedList<CustomerDto>>.Success(result, "No Customers found."));
        }
    }
}
