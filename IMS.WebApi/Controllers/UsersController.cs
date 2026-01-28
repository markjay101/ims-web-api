using IMS.Application.Common.Models;
using IMS.Application.Features.Users.Commands.CreateUser;
using IMS.Application.Features.Users.Commands.SignIn;
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
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            var userToken = await Mediator.Send(command);

            if (!string.IsNullOrEmpty(userToken?.Token))
            {
                return Ok(ApiResponse<UserTokenDto>.Success(userToken, "Login successful."));
            }

            return Unauthorized(ApiResponse<object>.Failure(["The email or password provided is incorrect."], "Authentication failed."));
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

            if(result)
                return StatusCode(201, ApiResponse<object>.Success(message: $"User {command.Role} successfully created."));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to create user."));
        }

        [HttpGet("admins")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<UserDto>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdmins([FromQuery]GetAdminsQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Items.Count != 0)
                return Ok(ApiResponse<PaginatedList<UserDto>>.Success(result));

            return StatusCode(204, ApiResponse<PaginatedList<UserDto>>.Success(result));
        }
    }
}
