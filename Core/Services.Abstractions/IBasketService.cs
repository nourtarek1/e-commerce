using Shared.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IBasketService
  {
    Task<BasketDto> GetBasketAsync();

    Task<BasketDto> AddItemAsync(AddBasketItemDto dto);

    Task<BasketDto> UpdateItemAsync(UpdateBasketItemDto dto);

    Task<BasketDto?> DeleteBasketAsync(int basketItemId);
    // اختياري لو حابب تمسح السلة كلها
  }
}
