using Domin.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.specifications
{
  public class OrderWithPaymentIntentSpecifctions : BaseSpecifications<Order, Guid>
  {
    public OrderWithPaymentIntentSpecifctions(string PaymentIntentId) : base(O => O.PaymentIntentId == PaymentIntentId)
    {

    }
  }
}
