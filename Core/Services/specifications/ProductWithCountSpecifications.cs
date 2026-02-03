using Domain.Models;
using Sherd.productSpecificationsParamters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.specifications
{
  public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
  {
    public ProductWithCountSpecifications(productSpecificationsParamters specparams)

      : base(p =>
          (string.IsNullOrEmpty(specparams.Search) || p.Name.ToLower().Contains(specparams.Search.ToLower())) &&
           (!specparams.CategoryId.HasValue || p.CategoryId == specparams.CategoryId) &&
           (!specparams.TypeId.HasValue || p.TypeId == specparams.TypeId)

      )
    {
    }
  }
}
