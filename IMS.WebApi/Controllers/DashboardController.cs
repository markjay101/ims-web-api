using IMS.Application.Features.Dashboard.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class DashboardController : ApiControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var result = await Mediator.Send(new GetDashboardSummaryQuery());

            if (result is not null)
                return Ok(ApiResponse<object>.Success(result));

            return StatusCode(StatusCodes.Status204NoContent, ApiResponse<object>.Success(result, "No dashboard summary to be shown."));
        }
    }
}
