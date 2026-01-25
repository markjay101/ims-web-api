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

            if (result != Guid.Empty)
                return StatusCode(StatusCodes.Status201Created, ApiResponse<Guid>.Success(result, "Payment method successfully created."));

            return BadRequest(ApiResponse<object>.Failure(["Empty Guid" ], "Failed to create payment method."));
        }

        [HttpPost("update")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] UpdatePaymentMethodCommand command)
        {
            var result = await Mediator.Send(command);

            if (result)
                return Ok(ApiResponse<object>.Success(message: "Payment method successfully updated."));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to update payment method."));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<PaymentMethodDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<PaymentMethodDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentMethods([FromQuery] GetPaymentMethodsQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Items.Count > 0)
                return Ok(ApiResponse<PaginatedList<PaymentMethodDto>>.Success(result));

            return StatusCode(StatusCodes.Status204NoContent, ApiResponse<PaginatedList<PaymentMethodDto>>.Success(result, "No payment methods found."));
        }
    }
}
