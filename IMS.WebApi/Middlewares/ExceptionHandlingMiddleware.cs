using FluentValidation;
using IMS.WebApi.Common;

namespace IMS.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex) //fluent validation exception
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

                var response = ApiResponse<object>.Failure(errors, "Validation failed.");

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (UnauthorizedAccessException uaex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure([uaex.Message], "Unauthorized."));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(ApiResponse<object>.Failure([ex.Message], "Server Error"));
            }
        }
    }
}
