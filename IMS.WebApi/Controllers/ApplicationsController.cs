using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using IMS.Application.Features.Applications.Commands.CreateApplication;
using IMS.Application.Features.Applications.Commands.UpdateApplicationStatus;
using IMS.Application.Features.Applications.Queries;
using IMS.Application.Features.Applications.Queries.GetApplicationById;
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
        [ProducesResponseType(typeof(ApiResponse<ApplicationListWithStatusCounts>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplications([FromQuery] GetApplicationsQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<ApplicationListWithStatusCounts>.Success(result);
            if (result.Items.Count == 0)
                response.Message = "No application found.";

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<ApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ApplicationDto>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplications([FromRoute] string id)
        {
            var query = new GetApplicationByIdQuery(id);
            var result = await Mediator.Send(query);

            if (result != null)
                return Ok(ApiResponse<ApplicationDto>.Success(result));

            return StatusCode(StatusCodes.Status204NoContent, ApiResponse<ApplicationDto>.Success(result, $"Application with id {id} is not found."));
        }
    }
}
