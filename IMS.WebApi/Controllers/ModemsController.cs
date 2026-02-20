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

            if (result == Guid.Empty) return NoContent();

            return CreatedAtAction(null, ApiResponse<Guid>.Success(result, "Modem successfully created."));
        }

        [HttpPost("update")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<ModemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UdpateModem([FromBody] UpdateModemCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == null) return NoContent();
                
            return Ok(ApiResponse<ModemDto>.Success(result, "Modem successfully updated."));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<ModemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetModems([FromQuery] GetModemsQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<PaginatedList<ModemDto>>.Success(result);

            if (result.Items.Count == 0)
                response.Message = "No modems found.";

            return Ok(response);
        }

        [HttpGet("available")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<List<ModemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailableModems([FromQuery] GetAvailableModemsQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<List<ModemDto>>.Success(result);

            if (result.Count == 0)
                response.Message = "No available modems found.";

            return Ok(response);
        }
    }
}
