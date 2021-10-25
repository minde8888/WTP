using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Data.Repositorys;
using WTP.Domain.Entities;

namespace WTP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private readonly IMailReposidory _mailRepository;
        public EmailController(IMailReposidory mailRepository)
        {
            _mailRepository = mailRepository;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await _mailRepository.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Problems with Send Email !!!" });
            }

        }

        [HttpPost("welcome")]
        public async Task<IActionResult> SendWelcomeMail([FromForm] WelcomeRequest request)
        {
            try
            {
                await _mailRepository.SendWelcomeEmailAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error to send email form !!!" });
            }
        }
    }
}
