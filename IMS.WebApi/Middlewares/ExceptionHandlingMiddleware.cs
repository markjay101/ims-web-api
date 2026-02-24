using FluentValidation;
using IMS.WebApi.Common;

namespace IMS.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex) //fluent validation exception
            {
                logger.LogError(ex, "Validation error occurred");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<object>.Failure(errors, "Validation failed");

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (UnauthorizedAccessException uaex)
            {
                logger.LogError(uaex, "Unauthorized access error occurred");

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure([uaex.Message], "Unauthorized"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure([ex.Message], "Server Error"));
            }
        }
    }
}
