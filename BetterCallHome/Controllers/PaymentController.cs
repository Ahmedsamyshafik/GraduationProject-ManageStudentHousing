using BetterCallHomeWeb.Base;
using Core.Features.Users.Commands.Models.PaymentRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : AppControllerBase
    {
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Payment(FormPaymentCommand command)
        {

            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
