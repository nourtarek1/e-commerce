using Domain.Models;
using Domin.Contercts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
  public interface IUnitOfWork
  {
    Task<int> SaveChangesAsync();
    IBasketRepository BasketRepository { get; }
    IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity :BaseEntity<Tkey>;
  }
}
