using IMS.Application.Common.Interfaces;
using IMS.WebApi.Common;
using IMS.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

namespace IMS.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IMS API",
                    Version = "v1",
                    Description = "Inventory Management System API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token: Bearer {your_token}"
                });
            });
            services.AddOpenApi();

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
