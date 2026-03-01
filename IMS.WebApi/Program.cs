using IMS.Application;
using IMS.Infrastructure;
using IMS.WebApi;
using IMS.WebApi.Extensions;
using IMS.WebApi.Middlewares;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddWebApiServices(builder.Configuration);


var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = builder.Configuration["LogRequestTemplate"]!;

    // Filter out noisy requests like Swagger/Scalar UI or Health Checks
    options.GetLevel = (httpContext, elapsed, ex) =>
        (ex != null || httpContext.Response.StatusCode > 499)
            ? LogEventLevel.Error
            : (httpContext.Request.Path.StartsWithSegments("/scalar") || httpContext.Request.Path.StartsWithSegments("/openapi"))
                ? LogEventLevel.Verbose
                : LogEventLevel.Information;
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    await app.InitialiseDatabaseAsync();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("IMS API Reference")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseCors("AppClients");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();