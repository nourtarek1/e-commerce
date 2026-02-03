using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.specifications
{
  public class ProductWithAllIncludesSpecification :BaseSpecifications<Product , int>
  {
    public ProductWithAllIncludesSpecification(int id) :base(p => p.Id == id)
    {
      ApllyIncludes();
    }


    private void ApllyIncludes()
    {
      AddInclude(p => p.Category);
      AddInclude(p => p.Type);
      AddInclude(p => p.Variants);
      


    }
  }
}
