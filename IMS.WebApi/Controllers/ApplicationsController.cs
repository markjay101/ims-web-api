using IMS.Application.Common.Models;
using IMS.Application.Features.Applications.Commands.CreateApplication;
using IMS.Application.Features.Applications.Commands.UpdateApplicationStatus;
using IMS.Application.Features.Applications.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class ApplicationsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationCommand command)
        {
            var result = await Mediator.Send(command);

            if (result)
                return StatusCode(201, ApiResponse<object>.Success(message: "Application successfully submitted."));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to submit application."));
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications([FromQuery] GetApplicationsQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Items.Count > 0)
                return Ok(ApiResponse<PaginatedList<ApplicationDto>>.Success(result));

            return StatusCode(204, ApiResponse<PaginatedList<ApplicationDto>>.Success(result, "No applications found."));
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateApplicationStatus([FromBody] UpdateApplicationStatusCommand command)
        {
            var result = await Mediator.Send(command);

            if (result)
                return Ok(ApiResponse<object>.Success(message: $"Application status successfully update to {command.Status}"));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to update application status"));
        }
    }
}
