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
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var result = await Mediator.Send(new GetDashboardSummaryQuery());

            var response = ApiResponse<object>.Success(result);

            if (result == null)
                response.Message = "No dashboard summary to be shown.";

            return Ok(response);
        }

        [HttpGet("monthly-earnings")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<List<MonthlyEarningDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMonthlyEarnings()
        {
            var result = await Mediator.Send(new GetMonthlyEarningsQuery());

            var response = ApiResponse<List<MonthlyEarningDto>>.Success([.. result]);

            if (result == null || !result.Any())
                response.Message = "No monthly earnings to be shown.";

            return Ok(response);
        }
    }
}
