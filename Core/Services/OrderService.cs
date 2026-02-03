using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Domin.Contercts;
using Domin.Models.OrderModels;
using Services.Abstractions;
using Services.specifications;
using Shared.OrderDto;

namespace Services
{
  public class OrderService(
      IMapper mapper,
      IBasketRepository basketRepository,
      IUnitOfWork unitOfWork
  ) : IOrderService
  {
    public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequest, string userId)
    {
      if (string.IsNullOrWhiteSpace(userId))
        throw new Exception("UserEmail is required");

      // 1️⃣ تحويل العنوان من DTO → Entity
      var address = mapper.Map<Address>(orderRequest.ShipToAddress);

      // 2️⃣ جلب Basket الحالي للمستخدم
      var basket = await basketRepository.GetBasketWithItemsAsync(userId);

      if (basket == null || !basket.Items.Any())
        throw new Exception("Basket is empty");

      var orderItems = new List<OrderItem>();

      foreach (var item in basket.Items)
      {
        if (!item.ProductVariantId.HasValue)
          throw new Exception("ProductVariantId is required");

        // 3️⃣ جلب المنتج مع الفاريانت
        var spec = new ProductWithVariantsSpecification(item.ProductId);
        var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
        if (product == null)
          throw new Exception($"Product with ID {item.ProductId} not found");

        var variant = product.Variants.FirstOrDefault(v => v.Id == item.ProductVariantId.Value);
        if (variant == null)
          throw new Exception($"Variant not found for product {product.Name}");

        if (variant.StockQuantity < item.Quantity)
          throw new Exception($"Not enough stock for {product.Name} ({variant.Color}-{variant.Size})");

        // 4️⃣ خصم من Stock
        variant.StockQuantity -= item.Quantity;

        // 5️⃣ إنشاء OrderItem
        var orderItem = new OrderItem(
            new ProductInOrderItem(product.Id, product.Name, product.ImageUrl),
            item.Quantity,
            product.Price
        );

        orderItems.Add(orderItem);
      }

      // 6️⃣ Delivery Method
      var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
          .GetAsync(orderRequest.DeliveryMethodId);

      if (deliveryMethod == null)
        throw new Exception("Delivery Method not found");

      var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

      // 7️⃣ إنشاء الأوردر بدون PaymentIntentId
      var order = new Order(
          userId,
          address,
          orderItems,
          deliveryMethod,
          OrderPaymentStatus.pending,
          subTotal,
          paymentIntentId: "" // Empty for now
      );

      await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);

      var resultCount = await unitOfWork.SaveChangesAsync();
      if (resultCount == 0)
        throw new Exception("Failed to save order");

      // 8️⃣ تحويل الأوردر → DTO للعرض
      return mapper.Map<OrderResultDto>(order);
    }


    public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods()
    {
      var deliveryMethods = await unitOfWork
          .GetRepository<DeliveryMethod, int>()
          .GetAllAsync();

      return mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
    }

    public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
    {
      var spec = new OrderSpecifications(id);

      var order = await unitOfWork
          .GetRepository<Order, Guid>()
          .GetAsync(spec);

      if (order is null)
        throw new Exception("Order not found");

      return mapper.Map<OrderResultDto>(order);
    }

    public async Task<IEnumerable<OrderResultDto>> GetAllOrdersByUserEmailAsync(string userId)
    {
      var spec = new OrderSpecifications(userId);

      var orders = await unitOfWork
          .GetRepository<Order, Guid>()
          .GetAllAsync(spec);

      return mapper.Map<IEnumerable<OrderResultDto>>(orders);
    }

    public async Task<OrderResultDto> DeleteOrderAsync(Guid id)
    {
      // 1️⃣ جلب الأوردر
      var order = await unitOfWork.GetRepository<Order, Guid>()
          .GetAsync(id);

      if (order == null)
        throw new Exception("Order not found");

      // 2️⃣ حذف الأوردر
      unitOfWork.GetRepository<Order, Guid>().Delete(order);

      // 3️⃣ حفظ التغييرات
      var result = await unitOfWork.SaveChangesAsync();
      if (result == 0)
        throw new Exception("Failed to delete order");

      // 4️⃣ تحويل للـ DTO
      return mapper.Map<OrderResultDto>(order);
    }

    public async Task MarkOrderAsPiadAsync(Guid orderId)
    {
      var order = await unitOfWork
                        .GetRepository<Order, Guid>()
                        .GetAsync(orderId);
      if(order == null)
        throw new Exception("Order not found");

      order.PaymentStatus = OrderPaymentStatus.PaymentReceived;

      await unitOfWork.SaveChangesAsync();
    }
  }
}
