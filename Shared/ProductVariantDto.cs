using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherd
{
  public class ProductVariantDto
  {


    //public List<int> ColorId { get; set; } = new();
    //public List<int> SizeId { get; set; } = new();
    //public int StockQuantity { get; set; }
    public string Color { get; set; }   // ✅ جديد
    public string Size { get; set; }    // ✅ جديد
    public int StockQuantity { get; set; }

  }
}
