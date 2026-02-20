using IMS.Application.Common.Models;
using IMS.Application.Features.InternetPlans.Queries;
using IMS.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;
using IMS.Application.Features.PaymentMethods.Commands.UpdatePaymentMethod;
using IMS.Application.Features.PaymentMethods.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    [Route("api/payment-methods")]
    public class PaymentMethodsController : ApiControllerBase
    {
        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == Guid.Empty) return NoContent();

            return CreatedAtAction(null, ApiResponse<Guid>.Success(result, "Payment method successfully created."));
        }

        [HttpPost("update")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaymentMethodDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] UpdatePaymentMethodCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == null) return NoContent();

            return Ok(ApiResponse<PaymentMethodDto>.Success(result, "Payment method successfully updated."));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<PaymentMethodDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentMethods([FromQuery] GetPaymentMethodsQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<PaginatedList<PaymentMethodDto>>.Success(result);

            if (result.Items.Count == 0)
                response.Message = "No payment methods found.";

            return Ok(response);
        }
    }
}
