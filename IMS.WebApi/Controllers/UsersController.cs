using IMS.Application.Common.Models;
using IMS.Application.Features.Users.Commands;
using IMS.Application.Features.Users.Commands.CreateUserAdminOrSuperAdmin;
using IMS.Application.Features.Users.Commands.SignIn;
using IMS.Application.Features.Users.Commands.UpdateUserAdminOrSuperAdmin;
using IMS.Application.Features.Users.Queries;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpPost("sign-in")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<UserTokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            var userToken = await Mediator.Send(command);

            if (!string.IsNullOrEmpty(userToken?.Token))
                return Ok(ApiResponse<UserTokenDto>.Success(userToken, "Login successful."));

            return Unauthorized(ApiResponse<object>.Failure([], "The email or password provided is incorrect."));
        }

        [HttpPost("refresh-token")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken()
        {
            var accessToken = await Mediator.Send(new RefreshTokenCommand());

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(ApiResponse<object>.Failure([], "Session expired. Please login again."));

            return Ok(ApiResponse<string?>.Success(accessToken, "Token Refreshed."));
        }

        [HttpPost("create-admin")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAdminOrSuperAdmin(CreateUserAdminOrSuperAdminCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == Guid.Empty) return NoContent();

            return CreatedAtAction(null, ApiResponse<object>.Success(result, $"User {command.Role} successfully created."));
        }

        [HttpPost("update-admin")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserAdminOrSuperAdmin(UpdateUserAdminOrSuperAdminCommand command)
        {
            var result = await Mediator.Send(command);

            if (result == null) return NoContent();

            return Ok(ApiResponse<object>.Success(result, $"User admin successfully updated."));
        }

        [HttpGet("admins")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdmins([FromQuery]GetAdminsQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<PaginatedList<UserDto>>.Success(result);

            if (result.Items.Count == 0)
                response.Message = "No admins found.";

            return Ok(response);
        }

        [HttpGet("admin/stats")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<AdminStatDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdminStats([FromQuery] GetAdminStatQuery query)
        {
            var result = await Mediator.Send(query);

            var response = ApiResponse<AdminStatDto>.Success(result);

             if (result == null)
                response.Message = "No admin statistics available.";

            return Ok(response);
        }
    }
}
