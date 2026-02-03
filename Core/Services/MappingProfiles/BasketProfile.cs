using AutoMapper;
using Domin.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using Shared.Basket;
using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
  public class BasketProfile : Profile
  {
    public BasketProfile() {

      // Basket → BasketDto مع العناصر
      CreateMap<Basket, BasketDto>()
          .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

      // BasketDto → Basket مع دمج العناصر
      CreateMap<BasketDto, Basket>()
          .ForMember(dest => dest.AppUserId, opt => opt.Ignore())
          .ForMember(dest => dest.Items, opt => opt.Ignore());

      // نترك Items للـ Service عشان يضيف/يحدث العناصر بشكل يدوي ويمنع Null

      // BasketItem → BasketItemDto
      CreateMap<BasketItem, BasketItemDto>()
       .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PictureUrlResolver>());

      // BasketItemDto → BasketItem مع تجاهل الحقول اللي يديرها الـ DB
      CreateMap<BasketItemDto, BasketItem>()
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.BasketId, opt => opt.Ignore())
          .ForMember(dest => dest.Basket, opt => opt.Ignore());

      CreateMap<Basket, PaymentOrderDto>();

    }
  }
}
