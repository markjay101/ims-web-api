using IMS.Application.Common.Models;
using IMS.Application.Features.Modems.Commands.CreateModem;
using IMS.Application.Features.Modems.Commands.UpdateModem;
using IMS.Application.Features.Modems.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class ModemsController : ApiControllerBase
    {
        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateModem([FromBody] CreateModemCommand command)
        {
            var result = await Mediator.Send(command);

            if (result != Guid.Empty)
                return StatusCode(201, ApiResponse<Guid>.Success(result, "Modem successfully created."));

            return BadRequest(ApiResponse<object>.Failure(["Empty Guid"], "Failed to create modem."));
        }

        [HttpPost("update")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UdpateModem([FromBody] UpdateModemCommand command)
        {
            var result = await Mediator.Send(command);

            if (result)
                return Ok(ApiResponse<Guid>.Success(message: "Modem successfully updated."));

            return BadRequest(ApiResponse<object>.Failure(["Empty Guid"], "Failed to update modem."));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<ModemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<ModemDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetModems([FromQuery] GetModemsQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Items.Count > 0)
                return Ok(ApiResponse<PaginatedList<ModemDto>>.Success(result));

            return StatusCode(StatusCodes.Status204NoContent, ApiResponse<PaginatedList<ModemDto>>.Success(result, "No modems found."));
        }

        [HttpGet("available")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<List<ModemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<ModemDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailableModems([FromQuery] GetAvailableModemsQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Count > 0)
                return Ok(ApiResponse<List<ModemDto>>.Success(result));

            return StatusCode(StatusCodes.Status204NoContent, ApiResponse<List<ModemDto>>.Success(result, "No available modems."));
        }
    }
}
