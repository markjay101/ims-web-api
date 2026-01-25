using IMS.Application.Common.Models;
using IMS.Application.Features.Applications.Queries;
using IMS.Application.Features.InternetPlans.Commands.CreateInternetPlan;
using IMS.Application.Features.InternetPlans.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            if (result != Guid.Empty)
                return StatusCode(201, ApiResponse<Guid>.Success(result, "Internet Plan successfully created."));

            return BadRequest(ApiResponse<object>.Failure(["Empty Guid"], "Failed to create internet plan."));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<InternetPlanDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<InternetPlanDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInternetPlans([FromQuery] GetInternetPlansQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Items.Count > 0)
                return Ok(ApiResponse<PaginatedList<InternetPlanDto>>.Success(result));

            return StatusCode(StatusCodes.Status204NoContent, ApiResponse<PaginatedList<InternetPlanDto>>.Success(result, "No internet plans found."));
        }
    }
}
