namespace IMS.Application.Common.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<string> GetRawTemplateAsync(string templateName, CancellationToken cancellationToken = default);
        string ReplacePlaceholders(string template, Dictionary<string, string> placeholders);
    }
}
