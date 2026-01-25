using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using IMS.Application.Features.Applications.Commands.CreateApplication;
using IMS.Application.Features.Applications.Commands.UpdateApplicationStatus;
using IMS.Application.Features.Applications.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class ApplicationsController : ApiControllerBase
    {
        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationCommand command)
        {
            var result = await Mediator.Send(command);

            if (result != Guid.Empty)
                return StatusCode(StatusCodes.Status201Created, ApiResponse<Guid>.Success(result, "Application successfully submitted."));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to submit application."));
        }

        [HttpPost("update-status")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusCommand command)
        {
            var result = await Mediator.Send(command);

            if (result)
                return Ok(ApiResponse<object>.Success(message: $"Application status successfully update to {command.Status}"));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to update application status"));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<ApplicationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<ApplicationDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplications([FromQuery] GetApplicationsQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Items.Count > 0)
                return Ok(ApiResponse<PaginatedList<ApplicationDto>>.Success(result));

            return StatusCode(StatusCodes.Status204NoContent, ApiResponse<PaginatedList<ApplicationDto>>.Success(result, "No applications found."));
        }
    }
}
