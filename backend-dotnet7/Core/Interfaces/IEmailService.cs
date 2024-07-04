using MimeKit;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmail(TextPart text, String To, String Subject);
    }
}
