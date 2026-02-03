using Domain.Models;
using Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Contercts
{
  public interface IBasketRepository
  {
    Task<Basket?> GetBasketWithItemsByIdAsync(int basketId);
    Task<Basket?> GetBasketWithItemsAsync(string userId);
    Task<ProductVariant?> GetProductVariantByIdAsync(int id);
    Task<Product?> GetProductByIdAsync(int id);

    Task<Basket?> UpdateBasketAsync(Basket basket);
    Task<Basket?> GetBasketByIdAsync(int basketId, string userId);
    Task DeleteBasketAsync(Basket basket);
  }
}
