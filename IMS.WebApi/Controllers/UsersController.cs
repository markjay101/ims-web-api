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
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            var token = await Mediator.Send(command);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(ApiResponse<object>.Success(new { token }, "Login successful."));
            }

            return Unauthorized(ApiResponse<object>.Failure(["The email or password provided is incorrect."], "Authentication failed."));
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateUserAdminOrSuperAdmin(CreateUserAdminOrSuperAdminCommand command)
        {
            var result = await Mediator.Send(command);

            if(result)
                return StatusCode(201, ApiResponse<object>.Success(message: $"User {command.Role} successfully created."));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to create user."));
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins([FromQuery]GetAdminsQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Items.Count != 0)
                return Ok(ApiResponse<PaginatedList<UserDto>>.Success(result));

            return StatusCode(204, ApiResponse<PaginatedList<UserDto>>.Success(result));
        }
    }
}
