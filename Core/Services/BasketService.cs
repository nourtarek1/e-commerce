using AutoMapper;
using Domain.Contracts;
using Domin.Contercts;
using Domin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;
using Shared.Basket;
using System.Security.Claims;

namespace Services
{
  public class BasketService : IBasketService
  {
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BasketService(IBasketRepository basketRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
      _basketRepository = basketRepository;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    private string GetUserId()
    {
      var user = _httpContextAccessor.HttpContext?.User;

      if (user == null || !user.Identity!.IsAuthenticated)
        throw new UnauthorizedAccessException("User not authenticated");

      return user.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }


    public async Task<BasketDto?> GetBasketAsync()
    {
      var userId = GetUserId();

      var basket = await _basketRepository.GetBasketWithItemsAsync(userId);
      if (basket == null)
        return null;

      return _mapper.Map<BasketDto>(basket);
    }

    public async Task<BasketDto> AddItemAsync(AddBasketItemDto dto)
    {
      var userId = GetUserId();

      var basket = await _basketRepository.GetBasketWithItemsAsync(userId)
                   ?? new Basket { AppUserId = userId, Items = new List<BasketItem>() };

      var variant = await _basketRepository.GetProductVariantByIdAsync(dto.ProductVariantId)
                    ?? throw new Exception("Product variant not found");

      var product = await _basketRepository.GetProductByIdAsync(dto.ProductId)
                    ?? throw new Exception("Product not found");

      // ✅ نشوف هل العنصر موجود قبل كده
      var existingItem = basket.Items.FirstOrDefault(i =>
        i.ProductId == dto.ProductId &&
        i.ProductVariantId == dto.ProductVariantId
      );

      if (existingItem != null)
      {
        // 🔥 نزود الكمية بس
        existingItem.Quantity += dto.Quantity;
      }
      else
      {
        basket.Items.Add(new BasketItem
        {
          ProductId = dto.ProductId,
          ProductVariantId = dto.ProductVariantId,
          ProductName = product.Name,
          ImageUrl = product.ImageUrl,
          Price = product.Price,
          Quantity = dto.Quantity,
          Basket = basket // 🔥 أهم سطر
        });
      }

      await _basketRepository.UpdateBasketAsync(basket);
      return _mapper.Map<BasketDto>(basket);
    }

    public async Task<BasketDto> UpdateItemAsync(UpdateBasketItemDto dto)
    {
      var basket = await _basketRepository.GetBasketWithItemsAsync(GetUserId())
          ?? throw new Exception("Basket not found");

      var item = basket.Items.FirstOrDefault(i => i.Id == dto.Id)
          ?? throw new Exception("Item not found");

      if (dto.Quantity <= 0)
      {
        basket.Items.Remove(item);
      }
      else
      {
        item.Quantity = dto.Quantity;

        // ✅ نغير الـ Variant لو اتبعت
        if (dto.ProductVariantId != item.ProductVariantId)
          item.ProductVariantId = dto.ProductVariantId;
      }

      await _basketRepository.UpdateBasketAsync(basket);
      return _mapper.Map<BasketDto>(basket);
    }



    public async Task<BasketDto?> DeleteBasketAsync(int basketItemId)
    {
      var userId = GetUserId();

      var basket = await _basketRepository.GetBasketWithItemsAsync(userId)
          ?? throw new Exception("Basket not found");

      var item = basket.Items.FirstOrDefault(i => i.Id == basketItemId)
          ?? throw new Exception("Item not found");

      basket.Items.Remove(item);

      await _basketRepository.UpdateBasketAsync(basket);

      return _mapper.Map<BasketDto?>(basket); // ملاحظة هنا
    }



  }
}
