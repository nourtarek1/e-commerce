using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Services.specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Persistence
{
  static class SpecificationEvaluator
  {

    //Generate Query
    public static IQueryable<TEntity> GetQuery<TEntity , Tkey>(
      IQueryable<TEntity> inputQuery ,
      ISpecifications<TEntity,Tkey> spec
      )
      where TEntity : BaseEntity<Tkey>
    {
      var query = inputQuery;
      if (spec.Criteria is not null)
        query = query.Where(spec.Criteria);


      if (spec.OrderBy is not null)
        query = query.OrderBy(spec.OrderBy);
      else if (spec.OrderByDescending is not null)
        query = query.OrderByDescending(spec.OrderByDescending);


      if (spec.IsPagination)
        query = query.Skip(spec.Skip).Take(spec.Take);


         query =    spec.IncludeExpression.Aggregate(query, (currentQuery, includexpressin) => currentQuery.Include(includexpressin));


   

      return query;
    }
  }
}
