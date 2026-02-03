using Shared.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderDto
{
  public class PaymentOrderDto
  {
    public List<OrderItemDto> Items { get; set; } = new();

    // بيانات الدفع
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }

    // الشحن
    public int DeliveryMethodId { get; set; }
    public decimal ShippingPrice { get; set; }

    // الإجمالي (اختياري بس مُفضل)
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
  }
}
