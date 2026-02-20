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

            if(result == Guid.Empty)
                return NoContent();

            return CreatedAtAction(nameof(GetApplicationById), new { id = result }, ApiResponse<Guid>.Success(result, "Application successfully submitted."));
        }

        [HttpPost("update-status")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<ApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusCommand command)
        {
            var result = await Mediator.Send(command);

            if(result == null)
                return NoContent();
                
            return Ok(ApiResponse<ApplicationDto>.Success(result, $"Application status successfully updated to {command.Status}"));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<ApplicationListWithStatusCounts>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
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

        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<ApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetApplicationById([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new GetApplicationByIdQuery(id));

            var response = ApiResponse<ApplicationDto>.Success(result);

            if (result == null)
                response.Message = $"Application with id {id} is not found.";

            return Ok(response);
        }
    }
}
