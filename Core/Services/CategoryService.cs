using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
  public class CategoryService : ICategoryService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

  

    public async Task<IEnumerable<CategoryResultDto>> GetAllCategoryAsync()
    {
      var category = await _unitOfWork.GetRepository<Category, int>().GetAllAsync();
      return _mapper.Map<IEnumerable<CategoryResultDto>>(category);
    }

    public async Task<CategoryResultDto?> GetCategoryById(int id)
    {
      var category = await _unitOfWork.GetRepository<Category, int>().GetAsync(id);
      return _mapper.Map<CategoryResultDto?>(category);
    }

    public async Task<CategoryResultDto> AddCategoryAsync(CategoryCreateDto createDto)
    {
      var category = _mapper.Map<Category>(createDto);
      await _unitOfWork.GetRepository<Category, int>().AddAsync(category);
      await _unitOfWork.SaveChangesAsync();

      return new CategoryResultDto
      {
        Id = category.Id,
        Name = category.Name
      };
    }

   
    public async Task<CategoryResultDto> UppdateCategoryAsync(int id, CategoryUpdateDto updateDto)
    {
      var categoryRepo = _unitOfWork.GetRepository<Category, int>();
      var category = await categoryRepo.GetAsync(id);
      if (category is null)  throw new Exception("not valid");
      _mapper.Map(updateDto, category);
      categoryRepo.Update(category);
      await _unitOfWork.SaveChangesAsync();
      return new CategoryResultDto
      {
        Name = category.Name
      };

    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
      var categoryRepo = _unitOfWork.GetRepository<Category, int>();
      var category = await categoryRepo.GetAsync(id);
      if (category is null) throw new Exception("Not Succeded");
      categoryRepo.Delete(category);
      await _unitOfWork.SaveChangesAsync();

      return true;
    }
  }
}

