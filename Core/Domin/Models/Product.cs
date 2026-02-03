using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
  public class Product : BaseEntity<int>
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }
    public int TypeId { get; set; }
   

    public Category Category { get; set; }
    public ProductType Type { get; set; }

    public ICollection<ProductVariant> Variants { get; set; }

  }

}
