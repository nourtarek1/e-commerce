using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models.OrderModels
{
  public class Order :BaseEntity<Guid>
  {
    public Order()
    {

    }
    public Order(string userEmail,
      Address shippingAddress,
      ICollection<OrderItem> orderItems,
      DeliveryMethod deliveryMethod,
      OrderPaymentStatus paymentStatus,
      decimal subTotal,
      string paymentIntentId)
    {
      Id =Guid.NewGuid();
      UserEmail = userEmail;
      ShippingAddress = shippingAddress;
      OrderItems = orderItems;
      DeliveryMethod = deliveryMethod;
      DeliveryMethodId = deliveryMethod.Id;
      PaymentStatus = paymentStatus;
      SubTotal = subTotal;
      ShippingPrice = deliveryMethod.Cost;
      PaymentStatus = OrderPaymentStatus.pending;
      OrderDate = DateTimeOffset.Now;
    }


    // User Email
    public string UserEmail { get; set; }

    // Shipping Address
    public Address ShippingAddress { get; set; }


    //OrderItems
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    // DeliveryMethod
    public DeliveryMethod DeliveryMethod { get; set; }//Navigatinol proprty

    //Payment Stauts

    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.pending;

    // Sub Total

    public decimal SubTotal { get; set; }

    // Order Date
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    // Payment
    public string? PaymentIntentId { get; set; }   // ✅ Stripe
    public string? ClientSecret { get; set; }
    public int? DeliveryMethodId { get; set; } //FK

    public decimal? ShippingPrice { get; set; }


  }
}
