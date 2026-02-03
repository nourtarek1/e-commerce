using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Domin.Contercts;
using Domin.Models.OrderModels;
using Microsoft.Extensions.Configuration;
using Services.Abstractions;
using Shared.OrderDto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProduct = Domain.Models.Product;

namespace Services
{
  public class PaymentService
    (IBasketRepository basketRepository,
    IUnitOfWork unitOfWork ,
    IMapper mapper,
    IConfiguration configuration
    ) : IPaymentService
  {
    public async Task<PaymentOrderDto> CreateOrUpdatePaymentIntentAsync(Guid orderId)
    {
      // 1️⃣ جلب الأوردر مباشرة بالـ ID مع تحميل الـ OrderItems والـ DeliveryMethod
      var order = await unitOfWork
          .GetRepository<Order, Guid>()
          .GetByIdAsync(orderId, includeRelated: true)
          ?? throw new Exception("Order not found");

      // 2️⃣ التحقق من حالة الأوردر والعناصر
      if (order.PaymentStatus != OrderPaymentStatus.pending)
        throw new Exception("Order is not in pending state");

      if (order.DeliveryMethod == null)
        throw new Exception("Delivery method is required");

      if (order.OrderItems == null || !order.OrderItems.Any())
        throw new Exception("Order has no items");

      // 3️⃣ تحديث أسعار الـ OrderItems من المنتجات الحالية
      foreach (var item in order.OrderItems)
      {
        var product = await unitOfWork.GetRepository<OrderProduct, int>()
            .GetAsync(item.Product.ProductId)
            ?? throw new Exception($"Product {item.Product.ProductName} not found");

        item.Price = product.Price;
      }

      // 4️⃣ حساب المبلغ النهائي
      order.SubTotal = order.OrderItems.Sum(i => i.Price * i.Quantity);
      var shippingPrice = order.DeliveryMethod.Cost;
      var amount = (long)((order.SubTotal + shippingPrice) * 100);

      // 5️⃣ إعداد Stripe
      StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
      var stripeService = new PaymentIntentService();

      if (string.IsNullOrEmpty(order.PaymentIntentId))
      {
        var createOptions = new PaymentIntentCreateOptions
        {
          Amount = amount,
          Currency = "usd",
          PaymentMethodTypes = new List<string> { "card" }
        };

        var paymentIntent = await stripeService.CreateAsync(createOptions);
        order.PaymentIntentId = paymentIntent.Id;
        order.ClientSecret = paymentIntent.ClientSecret;
      }
      else
      {
        var updateOptions = new PaymentIntentUpdateOptions
        {
          Amount = amount
        };

        await stripeService.UpdateAsync(order.PaymentIntentId, updateOptions);
      }

      await unitOfWork.SaveChangesAsync();

      // 6️⃣ تحويل الأوردر → DTO
      return mapper.Map<PaymentOrderDto>(order);
    }

  }
}






  

