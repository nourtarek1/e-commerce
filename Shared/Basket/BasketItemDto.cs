using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Basket
{
  public class BasketItemDto
  {
    public int Id { get; set; } // مهم في التحديث
    public int ProductId { get; set; }
    public int? ProductVariantId { get; set; }   // 🔥 لازم ده

    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

  }

}
