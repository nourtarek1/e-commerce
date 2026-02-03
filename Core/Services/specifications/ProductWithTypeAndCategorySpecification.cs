using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Sherd.productSpecificationsParamters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.specifications
{
  public class ProductWithTypeAndCategorySpecification : BaseSpecifications<Product , int>
  {
    public ProductWithTypeAndCategorySpecification(int id) :base(p => p.Id ==id)
    {
      ApllyIncludes();

    }
    public ProductWithTypeAndCategorySpecification(productSpecificationsParamters specparams)
      :base(p =>
      (string.IsNullOrEmpty(specparams.Search) || p.Name.ToLower().Contains(specparams.Search.ToLower())) &&
      (!specparams.CategoryId.HasValue || p.CategoryId == specparams.CategoryId) &&
      (!specparams.TypeId.HasValue || p.TypeId == specparams.TypeId)

      )
    {
      ApllyIncludes();
      ApllySorting(specparams.sort);
      ApplyPagination(specparams.pageIndex, specparams.pageSize);



    }

    private void ApllyIncludes()
    {
      AddInclude(p => p.Category);
      AddInclude(p => p.Type);
    }


    private void ApllySorting(string? sort)
    {
      if (!string.IsNullOrEmpty(sort))
      {
        switch (sort.ToLower())
        {
          case "namedesc":
            AddOrderByDescending(p => p.Name);
            break;
          case "priceasc":
            AddOrderBy(p => p.Price);
            break;
          case "pricedesc":
            AddOrderByDescending(p => p.Price);
            break;
          default:
            AddOrderBy(p => p.Name);
            break;
        }
      }
      else
      {
        AddOrderBy(p => p.Name);

      }
    }
  }
}
