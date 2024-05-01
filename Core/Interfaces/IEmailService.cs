using backend_dotnet7.Core.Dtos.Email;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IEmailService
    {
        // Declaring a method to send an email, taking a RequestDTO as a parameter
        string SendEmail(RequestDTO request);
        object SendEmail(RequestDto request);
    }
}
