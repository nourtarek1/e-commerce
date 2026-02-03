using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentaion
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class OrderController(IServiceManager serviceManager) : ControllerBase
  {
    [HttpPost("AddOrder")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequestDto orderRequest)
    {
      var email = User.FindFirstValue(ClaimTypes.NameIdentifier);
       var result = await serviceManager.orderService.CreateOrderAsync(orderRequest, email);
      return Ok(result);
    }

    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetOrders()
    {
      var email = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var result = await serviceManager.orderService.GetAllOrdersByUserEmailAsync( email);
      return Ok(result);
    }

    [HttpGet("GetOrderBy{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
      var result = await serviceManager.orderService.GetOrderByIdAsync(id);
      return Ok(result);
    }


    [HttpGet("DeliveryMethods")]
    public async Task<IActionResult> GetDeliveryMethod()
    {
      var result = await serviceManager.orderService.GetAllDeliveryMethods();
      return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteOrderAsync(Guid id)
    {
      var result = await serviceManager.orderService.DeleteOrderAsync(id);
      return Ok(result);
    }

    [HttpPut("mark-paid/{orderId}")]
    public async Task<IActionResult> MarkOrderAsPaid(Guid orderId)
    {
      await serviceManager.orderService.MarkOrderAsPiadAsync(orderId);
      return Ok();
    }


  }
}
