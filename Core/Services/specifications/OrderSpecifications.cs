using Domin.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.specifications
{
  public class OrderSpecifications :BaseSpecifications<Order , Guid>
  {
    public OrderSpecifications(Guid id ) : base(o => o.Id==id)
    {
      AddInclude(o => o.DeliveryMethod);
      AddInclude(o => o.OrderItems);
    }
    public OrderSpecifications(string userId) : base(o => o.UserEmail == userId)
    {
      AddInclude(o => o.DeliveryMethod);
      AddInclude(o => o.OrderItems);
      AddOrderBy(o => o.OrderDate);
    }
  }
}
