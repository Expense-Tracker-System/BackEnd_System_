using backend_dotnet7.Core.Dtos.Email;
using MimeKit;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IEmailService
    {
        Task<EmailResponseDto> SendEmail(TextPart text, string To, string Subject);
    }
}
