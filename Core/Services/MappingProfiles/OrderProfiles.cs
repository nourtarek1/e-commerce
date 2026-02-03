using AutoMapper;
using Domin.Models.OrderModels;
using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
  public class OrderProfiles :Profile
  {
    public OrderProfiles()
    {

      CreateMap<Address, AddressDto>().ReverseMap();

      CreateMap<OrderItem, OrderItemDto>()
        .ForMember(d => d.ProductId , o => o.MapFrom(s => s.Product.ProductId))
        .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
         .ForMember(d => d.ImageUrl, o => o.MapFrom<PictureUrlResolver>());

      CreateMap<Order, OrderResultDto>()
        .ForMember(d => d.PaymentStatus, o => o.MapFrom(s => s.PaymentStatus.ToString()))
        .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
        .ForMember(d => d.Total, o => o.MapFrom(s => s.SubTotal + s.DeliveryMethod.Cost));

      CreateMap<Order, PaymentOrderDto>()
         .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems))
         .ForMember(dest => dest.DeliveryMethodId, opt => opt.MapFrom(src => src.DeliveryMethodId))
         .ForMember(dest => dest.ShippingPrice, opt => opt.MapFrom(src => src.DeliveryMethod.Cost))
         .ForMember(dest => dest.ClientSecret, opt => opt.MapFrom(src => src.ClientSecret))
         .ForMember(dest => dest.PaymentIntentId, opt => opt.MapFrom(src => src.PaymentIntentId))
         .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SubTotal + src.DeliveryMethod.Cost))
         ;
    
    CreateMap<DeliveryMethod, DeliveryMethodDto>();

    }


  }
}
