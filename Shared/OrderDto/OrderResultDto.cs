using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderDto
{
  public class OrderResultDto
  {
    public Guid Id { get; set; }

    // User Email
    public string UserEmail { get; set; }

    // Shipping Address
    public AddressDto ShippingAddress { get; set; }


    //OrderItems
    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();


    // DeliveryMethod
    public string DeliveryMethod { get; set; }

    //Payment Stauts

    public string PaymentStatus { get; set; } 

    // Sub Total

    public decimal SubTotal { get; set; }

    // Order Date
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    // Payment
    public string PaymentIntentId { get; set; } = string.Empty;

    public decimal Total { get; set; }
  }
}
