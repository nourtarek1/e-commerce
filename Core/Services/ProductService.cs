using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.specifications;
using Sherd;
using Sherd.Pagination;
using Sherd.productSpecificationsParamters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
  public class ProductService : IProductService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }
    public async Task<PaginationRespons<ProductResultDto>> GetAllProductsAsync(productSpecificationsParamters specparams)
    {
      var spec = new ProductWithTypeAndCategorySpecification(specparams);
      var Products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);

      var specCount = new ProductWithCountSpecifications(specparams);
      var count = await _unitOfWork.GetRepository<Product, int>().CountAsync(specCount);
        
      var result = _mapper.Map<IEnumerable<ProductResultDto>>(Products);

      return new PaginationRespons<ProductResultDto>(specparams.pageIndex, specparams.pageSize, count, result);
    }

    public async Task<ProductResultDto?> GetProductById(int id)
    {
      var spec = new ProductWithAllIncludesSpecification(id);

      var Product = await _unitOfWork.GetRepository<Product, int>().GetAsync(spec);

      return _mapper.Map<ProductResultDto>(Product);
    }




    public async Task<ProductResultDto> AddProductAsync(ProductCreateDto dto)
    {
      var product = new Product
      {
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price,
        ImageUrl = dto.ImageUrl,
        CategoryId = dto.CategoryId,
        TypeId = dto.ProductTypeId,
        CreatedAt = DateTime.UtcNow,
        Variants = new List<ProductVariant>()
      };

      if (dto.Variants != null && dto.Variants.Any())
      {
        foreach (var v in dto.Variants)
        {
          product.Variants.Add(new ProductVariant
          {
            Color = v.Color,              // ✅ string
            Size = v.Size,                // ✅ string
            StockQuantity = v.StockQuantity
          });
        }
      }

      await _unitOfWork.GetRepository<Product, int>().AddAsync(product);
      await _unitOfWork.SaveChangesAsync();

      var spec = new ProductWithAllIncludesSpecification(product.Id);
      var addedProduct = await _unitOfWork
          .GetRepository<Product, int>()
          .GetAsync(spec);

      return _mapper.Map<ProductResultDto>(addedProduct);
    }







  }
}
