using Amazon.Pinpoint.Model;
using Microsoft.AspNetCore.Mvc;
using PinPoint;
using PinpointAPI.Contracts;

namespace PinpointAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendSMSController : ControllerBase
    {
       

        private readonly ILogger<SendSMSController> _logger;
        private readonly IConfiguration _configuration;
        public SendSMSController(ILogger<SendSMSController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

      
        [HttpPost(Name = "SendSMS")]
        public async void Post([FromBody] SMS sms)
        {
            await SendSMS.SendMessage(sms.phoneNumber, sms.message, _configuration);
        }


    }
}