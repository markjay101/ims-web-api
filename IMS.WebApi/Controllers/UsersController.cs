using IMS.Application.Features.Users.Commands.CreateUser;
using IMS.Application.Features.Users.Commands.SignIn;
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
                return Ok(ApiResponse<object>.Success(message: $"User {command.Role} successfully created."));

            return BadRequest(ApiResponse<object>.Failure([], "Failed to create user."));
        }
    }
}
