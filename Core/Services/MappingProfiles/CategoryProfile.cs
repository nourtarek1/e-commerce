using AutoMapper;
using Domain.Models;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping_Profiles
{
  public class CategoryProfile : Profile

  {
    public CategoryProfile()
    {
        CreateMap<Category,CategoryResultDto>().ReverseMap();

      // لما تعمل إضافة
      CreateMap<CategoryCreateDto, Category>();

      // لما تعمل تعديل
      CreateMap<CategoryUpdateDto, Category>();
    }
  }
}
