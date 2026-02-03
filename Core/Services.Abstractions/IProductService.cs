using Sherd;
using Sherd.Pagination;
using Sherd.productSpecificationsParamters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IProductService
  {
    Task<PaginationRespons<ProductResultDto>> GetAllProductsAsync(productSpecificationsParamters specparams);
    Task<ProductResultDto?> GetProductById(int id);

    Task<ProductResultDto> AddProductAsync(ProductCreateDto dto);



  }
}
