using IMS.Application.Features.Auth.Commands;
using IMS.Application.Features.Auth.Commands.SignIn;
using IMS.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi.Controllers
{
    public class AuthController : ApiControllerBase
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
    }
}
