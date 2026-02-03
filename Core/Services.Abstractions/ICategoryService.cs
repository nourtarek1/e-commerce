using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface ICategoryService
  {
    Task<IEnumerable<CategoryResultDto>> GetAllCategoryAsync();
    Task<CategoryResultDto?> GetCategoryById(int id);
    Task<CategoryResultDto> AddCategoryAsync(CategoryCreateDto createDto);
    Task<CategoryResultDto> UppdateCategoryAsync(int id, CategoryUpdateDto updateDto);
    Task<bool> DeleteCategoryAsync(int id);
  }
}
