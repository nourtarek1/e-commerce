using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentaion
{
  [ApiController]
  [Route("api/[controller]")]
  public class CategoryController(IServiceManager serviceManager) : ControllerBase
  {
    [HttpGet("Get-All-Category")]
    public async Task<IActionResult> GetAllASync()
    {
      var result = await serviceManager.CategoryService.GetAllCategoryAsync();
      return Ok(result);

    }
    [HttpGet("Get-CategoryBy-ID")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
      var result = await serviceManager.CategoryService.GetCategoryById(id);
      if (result is null) return NotFound();
      return Ok(result);
    }

    [HttpPost("Add-Category")]
    public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryCreateDto createDto)
    {
      var result = await serviceManager.CategoryService.AddCategoryAsync(createDto);
      return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
    }

    [HttpPut("Update-Category{id}")]
    public async Task<IActionResult> UpdateAsync(int id, CategoryUpdateDto updateDto)
    {
      var result = await serviceManager.CategoryService.UppdateCategoryAsync(id, updateDto);
      return Ok(result);
    }


    [HttpDelete("Delete-Category{id}")]
    public async Task<IActionResult> DeleteCategoryAsync(int id)
    {
      var isDeleted = await serviceManager.CategoryService.DeleteCategoryAsync(id);

      if (!isDeleted)
        return NotFound($"Category with ID {id} was not found.");

      return NoContent(); // ✅ 204 No Content = تم الحذف بنجاح
    }

  }
}
