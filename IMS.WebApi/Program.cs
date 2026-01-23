using IMS.Application;
using IMS.Infrastructure;
using IMS.Infrastructure.Persistence;
using IMS.WebApi;
using IMS.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddWebApiServices();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initialiser.InitializeAsync();
        await initialiser.SeedAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "IMS API V1");

    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
