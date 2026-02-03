using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.specifications
{
  public class ProductWithVariantsSpecification :BaseSpecifications<Product,int>
  {
    public ProductWithVariantsSpecification(int productId)
      : base(p => p.Id == productId)
    {
      AddInclude(p => p.Variants);
    }
  }
}
