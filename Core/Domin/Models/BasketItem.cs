using Domain.Models;

namespace Domin.Models
{
  public class BasketItem :BaseEntity<int>
  {
    public string ProductName { get; set; }
    public string ImageUrl { get; set; } 
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int BasketId { get; set; }
    public Basket Basket { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int? ProductVariantId { get; set; }  // Nullable لو ممكن يكون المنتج بدون Variant
    public ProductVariant? ProductVariant { get; set; }
  }
}
