using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Dtos.Email;
using Microsoft.Extensions.Options;

namespace backend_dotnet7.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            // Initialize the EmailService with EmailSetting Inject values of in appsettings
            emailSettings = options.Value;
        }

        // Method to send an email
        public async Task<EmailResponseDto> SendEmail(TextPart text, string To, string Subject)
        {

            var htmlBody = text;

            // Create a new MimeMessage for composing the email
            var email = new MimeMessage();

            // Set the sender's email address
            email.From.Add(MailboxAddress.Parse(emailSettings.Email));

            // Set the recipient's email address
            email.To.Add(MailboxAddress.Parse(To));

            // Set the email subject
            email.Subject = Subject;

            // Set the email body as HTML text
            email.Body = htmlBody;

            // Create an instance of SmtpClient for sending the email
            using var smtp = new SmtpClient();

            // Connect to the SMTP server with the specified host and port using StartTLS for security
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);

            // Authenticate with the SMTP server using the provided username and password
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);

            // Send the composed email
            bool isSuccess;
            string message;
            try
            {
                smtp.Send(email);
                isSuccess = true;
                message = "Mail Sent Successfully";
            }
            catch (Exception ex)
            {
                // Return an error message if the email could not be sent
                isSuccess = false;
                message = "Error: " + ex.Message;
            }
            finally
            {
                // Disconnect from the SMTP server after sending the email
                smtp.Disconnect(true);
            }

            // Return a success message
            return new EmailResponseDto
            {
                IsSuccess = isSuccess,
                Message = message,
            };

        }
    }
}
