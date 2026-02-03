using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Sherd.productSpecificationsParamters;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentaion
{
  [ApiController]
  [Route("api/[Controller]")]
  public class ProductController(IServiceManager serviceManager) : ControllerBase
  {
    [HttpGet("Get-All-Product")]
    public async Task<IActionResult> GetAllProducts([FromQuery] productSpecificationsParamters specparams)
    {
      var result = await serviceManager.ProductService.GetAllProductsAsync(specparams);
      return Ok(result);
    }


    [HttpGet("Get-ProductBy-Id{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {

      var result = await serviceManager.ProductService.GetProductById(id);
      return Ok(result);
    }


    [HttpPost("Add-Product")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
    {
      var result = await serviceManager.ProductService.AddProductAsync(dto);
      if (result == null) return BadRequest("فشل في إنشاء المنتج");
      return Ok(result);
    }

  }
}
