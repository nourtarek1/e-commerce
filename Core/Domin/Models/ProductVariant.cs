using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
  public class ProductVariant :BaseEntity<int>
  {
    public int ProductId { get; set; }
    public Product Product { get; set; }

    //public int ColorId { get; set; }
    //public Color Color { get; set; }

    //public int SizeId { get; set; }
    //public Size Size { get; set; }
    public string Color { get; set; } = null!;
    public string Size { get; set; } = null!;    // S, XL
    public int StockQuantity { get; set; }
  }
}
