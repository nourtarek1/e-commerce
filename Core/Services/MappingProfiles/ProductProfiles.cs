using AutoMapper;
using Domain.Models;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
  public class ProductProfiles : Profile
  {
    public ProductProfiles()
    {
      // عند إرجاع المنتج
      CreateMap<Product, ProductResultDto>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
        .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
        .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
        .ForMember(d => d.ImageUrl , o => o.MapFrom<PictureUrlResolver>()); // ✅ دي الإضافة

      CreateMap<ProductVariant, ProductVariantResultDto>()
                 .ForMember(d => d.Color,
                     o => o.MapFrom(s => s.Color))
                 .ForMember(d => d.Size,
                     o => o.MapFrom(s => s.Size))
                 .ForMember(d => d.StockQuantity,
                     o => o.MapFrom(s => s.StockQuantity));

      // عند الإنشاء
      CreateMap<ProductVariantDto, ProductVariant>();
      CreateMap<ProductCreateDto, Product>();
      CreateMap<ProductUpdateDto, Product>();

    }
  }

}
