namespace IMS.Application.Features.Modems.Queries
{
    public class ModemDto
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
    }
}
