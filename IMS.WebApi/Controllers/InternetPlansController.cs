using IMS.Application.Common.Models;
using IMS.Application.Features.InternetPlans.Commands.CreateInternetPlan;
using IMS.Application.Features.InternetPlans.Commands.UpdateInternetPlan;
using IMS.Application.Features.InternetPlans.Queries;
using IMS.Application.Features.InternetPlans.Queries.GetInternetPlanById;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    [Route("api/internet-plans")]
    [ApiController]
    public class InternetPlansController : ApiControllerBase
    {
        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInternetPlan([FromBody] CreateInternetPlanCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == Guid.Empty) return NoContent();

            return CreatedAtAction(nameof(GetInternetPlanById), new { id = result }, ApiResponse<Guid>.Success(result, "Internet Plan successfully created."));
        }

        [HttpPost("update")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<InternetPlanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInternetPlan([FromBody] UpdateInternetPlanCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == null)
                return NoContent();

            return Ok(ApiResponse<InternetPlanDto>.Success(result, "Internet Plan successfully updated."));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<InternetPlanDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInternetPlans([FromQuery] GetInternetPlansQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<PaginatedList<InternetPlanDto>>.Success(result);

            if (result.Items.Count == 0)
                response.Message = "No internet plans found.";

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<InternetPlanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInternetPlanById([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new GetInternetPlanByIdQuery(id));

            var response = ApiResponse<InternetPlanDto>.Success(result);

            if (result == null)
                response.Message = $"Internet Plan with id {id} is not found.";

            return Ok(response);
        }
    }
}
