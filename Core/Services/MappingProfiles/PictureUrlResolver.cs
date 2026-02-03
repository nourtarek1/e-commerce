using AutoMapper;
using Domain.Models;
using Domin.Models;
using Domin.Models.OrderModels;
using Microsoft.Extensions.Configuration;
using Shared.Basket;
using Shared.OrderDto;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
  public class PictureUrlResolver : IValueResolver<Product, ProductResultDto, string>,
    IValueResolver<OrderItem, OrderItemDto, string>,
    IValueResolver<BasketItem, BasketItemDto, string>


  {
    private readonly IConfiguration _configuration;

    public PictureUrlResolver(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
    {
      if (string.IsNullOrEmpty(source.ImageUrl)) return string.Empty;
      return $"{_configuration["BaseUrl"]}/{source.ImageUrl}";
    }

    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
      if (string.IsNullOrEmpty(source.Product.PicturUrl))
        return null;

      return $"{_configuration["BaseUrl"]}/{source.Product.PicturUrl}";
    }

    public string Resolve(BasketItem source, BasketItemDto destination, string destMember, ResolutionContext context)
    {
      if (string.IsNullOrEmpty(source.ImageUrl)) return string.Empty;
      return $"{_configuration["BaseUrl"]}/{source.ImageUrl}";
    }


  }

}
