using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IOrderService
  {

    // Get Order By Id
    Task<OrderResultDto> GetOrderByIdAsync(Guid id);


    // Get Order

    Task<IEnumerable<OrderResultDto>> GetAllOrdersByUserEmailAsync(string userEmail);


    // Create Order
    Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userEmail);

    // Get All Delivery Methods

    Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods();
    // ✅ Delete Order By Id
    Task<OrderResultDto> DeleteOrderAsync(Guid id);
    Task MarkOrderAsPiadAsync(Guid orderId);
  }
}
