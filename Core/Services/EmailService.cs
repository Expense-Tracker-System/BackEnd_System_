using System.Net.Mail;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos.Helper;
using backend_dotnet7.Core.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

namespace backend_dotnet7.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting emailSetting;

        public EmailService(IOptions<EmailSetting> emailSetting)
        {
            emailSetting = emailSetting.Value;
        }

        public async Task SendEmailAsync(Mailrequest mailrequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSetting.Email);
            email.To.Add(new MailboxAddress(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject;
            var builder =new BodyBuilder();
            builder.HtmlBody = mailrequest.body;
            email.Body =builder.ToMessageBody();


            using var smtp = new SmtpClient();
            smtp.Connect(emailSetting, Host, emailSetting.Port);
            smtp.Authenticate(emailSetting.Email, emailSetting,Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            throw new NotImplementedException();

       
        }
    }
}
