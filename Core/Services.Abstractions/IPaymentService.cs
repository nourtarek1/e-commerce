using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IPaymentService
  {

    Task<PaymentOrderDto> CreateOrUpdatePaymentIntentAsync(Guid orderId);
  }
}
