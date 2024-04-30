using backend_dotnet7.Core.Dtos.Helper;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly dbfirstcontext dbfirstcontext;
        private readonly IEmailService emailService;

        public CustomerController(dbfirstcontext dbfirstcontext , IEmailService emailService)
        {
            this.dbfirstcontext = dbfirstcontext;
            this.emailService = Service;
        }
        [HttpGet("Getall")]
        public ActionResult Index()
        {
        }

        public async Task<IActionResult> SendMail()
        {
            try {

                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmasil = "menkasadruwan121@gmail.com";
                mailrequest.Subject = "thnks for me";
                mailrequest.body = "thnks";
                await emailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                throw
            }
          
        }
    }
}
