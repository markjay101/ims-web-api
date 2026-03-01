namespace IMS.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync(string to, string from, string message, CancellationToken cancellation = default);
    }
}
