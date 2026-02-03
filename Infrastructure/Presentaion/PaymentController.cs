using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentaion
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class PaymentController(IServiceManager serviceManager) : ControllerBase
  {
    [HttpPost("{OrderId}")]
    public async Task<IActionResult> CreateOrUpdatePaymentAsync(Guid orderId)
    {
      var result = await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(orderId);
      return Ok(result);
    }
  }
}
