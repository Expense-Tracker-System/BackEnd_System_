using backend_dotnet7.Core.Dtos.Helper;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest)
    }
}
