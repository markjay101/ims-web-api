namespace IMS.Infrastructure.Common.Options
{
    public class EmailOptions
    {
        public const string SectionName = "EmailSettings";

        public string Provider { get; init; } = string.Empty;
        public GmailOptions Gmail { get; init; } = new();
    }

    public class GmailOptions
    {
        public string Email { get; init; } = string.Empty;
        public string AppPassword { get; init; } = string.Empty;
    }
}
