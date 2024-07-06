using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailsController(IEmailService emailService)
        {
            // Initialize the EmailController with IEmailService dependency
            this.emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail()
        {
            var htmlBody = new TextPart(TextFormat.Html)
            {
                Text = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Reservation Details</title>
        <style>
            body, html {{ margin: 0; padding: 0; font-family: Arial, sans-serif; }}
            .container {{ max-width: 600px; margin: 20px auto; padding: 20px; background-color: #ffffff; border-radius: 8px; }}
            .header {{ padding: 20px; text-align: center; background-color: #007bff; color: #ffffff; }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <div class=""header"">
                <h1>Reservation Details</h1>
            </div>
            <p>Dear Patron ,</p>
            <p>We are delighted to provide you with the details of your reservation. Your support is greatly appreciated, and we hope this reservation enhances your experience with our services. Should you have any questions or need further assistance, please feel free to reach out to us. Thank you for choosing us!</p>
            
            <a href=""https://easylibro.online"" style=""display: block; padding: 10px 0; text-align: center; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 4px; margin-top: 20px;"">Go to Easylibro</a>
        </div>
    </body>
    </html>"
            };

            // Call the SendEmail method of the EmailService to send the email
            var response = emailService.SendEmail(htmlBody,"ashencharanga99@gmail.com","Reminder success");
            
          // Return the response from the SendEmail method
           return Ok(response);
        }

    }
}
