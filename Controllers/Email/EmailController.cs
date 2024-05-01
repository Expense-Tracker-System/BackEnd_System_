using backend_dotnet7.Core.Dtos.Email;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        // Declaring a private readonly field to hold an instance of the email service
        private readonly IEmailService emailService;

        // Constructor for the EmailController, injecting an instance of IEmailService
        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        // HTTP POST endpoint for sending emails
        [HttpPost("SendEmails")]
        public ActionResult SendEmail(RequestDto request)
        {
            // Calling the SendEmail method of the injected email service
            var result = emailService.SendEmail(request);

            // Returning an HTTP response indicating success with a message
            return Ok("Mail sent!");
        }
    }
