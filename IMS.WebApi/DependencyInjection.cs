using IMS.Application.Common.Interfaces;
using IMS.WebApi.Common;
using IMS.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    var response = ApiResponse<object>.Failure(errors, "Validation failed.");

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
