using Serilog;
using Serilog.Events;

namespace IMS.WebApi.Extensions
{
    public static class LoggingConfiguration
    {
        public static void UseSerilog(this WebApplicationBuilder builder)
        {
            var logTemplate = builder.Configuration["LogTemplate"]!;

            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(builder.Configuration)
                                .WriteTo.Logger(lc => lc
                                    .Filter.ByExcluding(logEvent =>
                                        logEvent.Level < LogEventLevel.Warning
                                        && logEvent.Properties.ContainsKey("SourceContext")
                                        && logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore")
                                        || logEvent.MessageTemplate.Text.Contains("Application started. Press Ctrl+C to shut down"))
                                    .WriteTo.Console(outputTemplate: logTemplate)
                                    .WriteTo.File(builder.Configuration["LogPath"]!, 
                                                    rollingInterval: RollingInterval.Day, 
                                                    outputTemplate: logTemplate))
                                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
