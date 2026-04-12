namespace IMS.Application.Common.Options
{
    internal class ClientsOption
    {
        public const string SectionName = "ClientsSettings";

        public string Admin { get; init; } = string.Empty;
        public string Customer { get; init; } = string.Empty;
    }
}
