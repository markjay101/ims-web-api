using IMS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace IMS.Infrastructure.Email
{
    internal class EmailTemplateService(IWebHostEnvironment env) : IEmailTemplateService
    {
        public async Task<string> GetRawTemplateAsync(string templateName, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(env.ContentRootPath, "EmailTemplates", $"{templateName}.html");
            return await File.ReadAllTextAsync(filePath, cancellationToken);
        }

        public string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            foreach (var item in placeholders)
            {
                template = template.Replace($"{{{item.Key}}}", item.Value);
            }
            return template;
        }
    }
}
