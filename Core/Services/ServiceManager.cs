using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Domin.Contercts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Services
{
  public class ServiceManager(UserManager<AppUser> userManager ,
    IOptions<JwtOptions> options,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IBasketRepository basketRepository ,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration
    ) :IServiceManager
    {
    public IAuthService AuthService { get; } = new AuthService(userManager , options);

    public ICategoryService CategoryService { get; } = new CategoryService(unitOfWork, mapper);

    public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);

    public IBasketService BasketService { get; } = new BasketService(basketRepository, mapper , httpContextAccessor);

    public IOrderService orderService { get; } = new OrderService(mapper, basketRepository, unitOfWork);

    public IPaymentService PaymentService =>  new PaymentService(basketRepository,unitOfWork,mapper,configuration);
  }
}
