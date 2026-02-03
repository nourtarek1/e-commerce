using Domain.Models;
using Domin.Contercts;
using Domin.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
  public class BasketRepository : IBasketRepository
  {
    private readonly StoreDbContext _context;
    private readonly AuthDbContext _authDbContext;

    public BasketRepository(StoreDbContext context,AuthDbContext authDbContext)
    {
      _context = context;
      _authDbContext = authDbContext;
    }


    public async Task<Basket?> GetBasketWithItemsAsync(string userId)
    {
      return await _context.Baskets
          .Include(b => b.Items)
          .FirstOrDefaultAsync(b => b.AppUserId == userId);
    }

    public async Task<Basket?> GetBasketWithItemsByIdAsync(int basketId)
    {
      return await _context.Baskets
          .Include(b => b.Items)
          .FirstOrDefaultAsync(b => b.Id == basketId);
    }

    public async Task<Basket?> UpdateBasketAsync(Basket basket)
    {
      basket.Items ??= new List<BasketItem>();

      var existingBasket = await _context.Baskets
          .Include(b => b.Items)
          .FirstOrDefaultAsync(b => b.AppUserId == basket.AppUserId);

      if (existingBasket == null)
      {
        var userExists = await _authDbContext.Users
            .AnyAsync(u => u.Id == basket.AppUserId);

        if (!userExists)
          throw new Exception("Invalid AppUserId. User does not exist.");

        _context.Baskets.Add(basket);
      }
      else
      {
        _context.Entry(existingBasket).CurrentValues.SetValues(basket);

        // حذف العناصر اللي اتشالت
        foreach (var existingItem in existingBasket.Items.ToList())
        {
          if (!basket.Items.Any(i => i.Id == existingItem.Id))
            _context.BasketItems.Remove(existingItem);
        }

        // إضافة / تحديث العناصر
        foreach (var item in basket.Items)
        {
          var existingItem = existingBasket.Items
              .FirstOrDefault(i => i.Id == item.Id);

          if (existingItem == null)
          {
            existingBasket.Items.Add(item);
          }
          else
          {
            _context.Entry(existingItem).CurrentValues.SetValues(item);
          }
        }
      }

      await _context.SaveChangesAsync();

      // ✅ السطر اللي كان ناقص
      return existingBasket ?? basket;
    }


    public async Task<Basket?> GetBasketByIdAsync(int basketId, string userId)
{
    return await _context.Baskets
        .AsNoTracking() // 🔥 مهم
        .Include(b => b.Items)
        .FirstOrDefaultAsync(b =>
            b.Id == basketId &&
            b.AppUserId == userId
        );
}


    public async Task DeleteBasketAsync(Basket basket)
    {
      // نعمل Attach لأن AsNoTracking مستخدمة
      _context.Baskets.Attach(basket);

      _context.Baskets.Remove(basket);
      await _context.SaveChangesAsync();
    }





    public async Task<Product?> GetProductByIdAsync(int id)
    {
      // جلب المنتج بالـ ID من قاعدة البيانات
      return await _context.Products
                           .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<ProductVariant?> GetProductVariantByIdAsync(int id)
    {
      return await _context.ProductVariants
                             .FirstOrDefaultAsync(v => v.Id == id);
    }

  
  }
}
