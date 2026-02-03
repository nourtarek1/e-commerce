using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherd
{
  public class ProductUpdateDto
  {
    public int Id { get; set; } // الفرق عن Create هنا
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }

    public int CategoryId { get; set; }
    public int ProductTypeId { get; set; }

    public List<ProductVariantDto> Variants { get; set; } = new();
  }
}
