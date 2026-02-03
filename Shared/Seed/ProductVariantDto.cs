using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherd.Seed
{
  public class ProductVariantDto
  {
    public int ProductId { get; set; }

    public string Color { get; set; }
    public string Size { get; set; }
    public int StockQuantity { get; set; }
  }
}
