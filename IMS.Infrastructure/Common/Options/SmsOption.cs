namespace IMS.Infrastructure.Common.Options
{
    internal class SmsOptions
    {
        public const string SectionName = "SmsSettings";

        public string Provider { get; set; } = string.Empty;
        public TwilioOptions Twilio { get; set; } = new();
        public InfobipOptions Infobip { get; set; } = new();
    }

    internal class TwilioOptions
    {
        public string AccountSID { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string MessagingServiceSID { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
    }

    internal class InfobipOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
    }
}
