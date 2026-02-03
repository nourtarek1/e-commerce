using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderDto
{
  public class OrderRequestDto
  {

    public AddressDto ShipToAddress { get; set; }
    public int DeliveryMethodId { get; set; }
  }
}
