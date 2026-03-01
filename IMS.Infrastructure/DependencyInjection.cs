using IMS.Application.Common.Interfaces;
using IMS.Domain.Entities;
using IMS.Infrastructure.Common.Options;
using IMS.Infrastructure.Email;
using IMS.Infrastructure.Identity;
using IMS.Infrastructure.Persistence;
using IMS.Infrastructure.Persistence.Interceptors;
using IMS.Infrastructure.Sms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataProtection();
            services.AddScoped<AuditableEntityInterceptor>();
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();

                options.UseSqlServer(connectionString)
                        .AddInterceptors(interceptor);
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ApplicationDbContextInitializer>();

            services.AddIdentityCore<User>(options =>
                    {
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = true;
                        options.Password.RequiredLength = 8;
                        options.Password.RequireNonAlphanumeric = true;
                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));
            services.ConfigureOptions<ConfigureJwtBearerOptions>();
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer();


            services.AddHttpContextAccessor();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddAuthorization();

            services.Configure<EmailOptions>(config.GetSection(EmailOptions.SectionName));
            services.AddTransient<IEmailTemplateService, EmailTemplateService>();
            services.AddTransient<GmailService>();
            services.AddSingleton<EmailServiceFactory>();
            services.AddTransient<IEmailService>(sp =>
                sp.GetRequiredService<EmailServiceFactory>().GetEmailService());

            services.Configure<SmsOptions>(config.GetSection(SmsOptions.SectionName));
            services.AddTransient<TwilioService>();
            services.AddTransient<InfobipService>();
            services.AddSingleton<SmsServiceFactory>();
            services.AddTransient<ISmsService>(sp =>
                sp.GetRequiredService<SmsServiceFactory>().GetSmsService());

            return services;
        }

        public static async Task InitialiseDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

            await initialiser.InitializeAsync();
            await initialiser.SeedAsync();
        }
    }
}
