using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Shared.Basket;
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

  public class BasketController(IServiceManager servicesManager) : ControllerBase
  {

    [HttpGet("GetBasket")]
    public async Task<IActionResult> GetBasket()
    {
      var result = await servicesManager.BasketService.GetBasketAsync();
      return Ok(result);
    }
    //[HttpPost("AddItem")]
    //public async Task<IActionResult> UpdateBasket(BasketDto basket)
    //{
    //  var result = await servicesManager.BasketService.UpdateBasketAsync(basket);
    //  return Ok(result);
    //}
    [HttpPost("AddItem")]
    public async Task<ActionResult<BasketDto>> AddItem(AddBasketItemDto dto)
  => Ok(await servicesManager.BasketService.AddItemAsync(dto));

    [HttpPut("UpdateItem/{Id}")]
    public async Task<ActionResult<BasketDto>> UpdateItem(UpdateBasketItemDto dto)
      => Ok(await servicesManager.BasketService.UpdateItemAsync(dto));

    //[HttpDelete("RemoveItem/{variantId}")]
    //public async Task<ActionResult<BasketDto>> RemoveItem(int variantId)
    //  => Ok(await servicesManager.BasketService.RemoveItemAsync(variantId));
    [HttpDelete("{basketItemId}")]
    public async Task<ActionResult> DeleteBasket([FromRoute] int basketItemId)
    {
      var result = await servicesManager.BasketService.DeleteBasketAsync(basketItemId);

      if (result == null)  // 🔹 بدل if (!result)
        return NotFound();

      return NoContent();
    }



  }
}
