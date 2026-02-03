using Domain.Contracts;
using Domain.Models;
using Domin.Contercts;
using Persistence.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly StoreDbContext _context;
    private readonly AuthDbContext _authDbContext;
    private readonly ConcurrentDictionary<string , object> _repositories;
    public IBasketRepository BasketRepository { get; }

    public UnitOfWork(StoreDbContext context)
    {
      _context = context;
      _repositories = new ConcurrentDictionary<string , object>();
      BasketRepository = new BasketRepository(_context , _authDbContext);
    }


    public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
        where TEntity : BaseEntity<TKey>
    {
      return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_context));
    }

    public async Task<int> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync();
    }

    
  }
}
