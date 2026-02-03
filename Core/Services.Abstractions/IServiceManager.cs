using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IServiceManager
  {
    IAuthService AuthService { get; }
    ICategoryService CategoryService { get; }
    IProductService ProductService { get; }
    IBasketService BasketService { get; }
    IOrderService  orderService { get; }
    IPaymentService PaymentService { get; }
  }
}
