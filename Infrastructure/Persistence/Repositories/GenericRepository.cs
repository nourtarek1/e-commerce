using Domain.Contracts;
using Domain.Models;
using Domin.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
  public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
  {
    private readonly StoreDbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    public GenericRepository(StoreDbContext context)
    {
      _context = context;
      _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool treackChanges = false)
    {
     return treackChanges ?
        await _dbSet.ToListAsync()
        :await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetAsync(Tkey id)
    {
      return await _dbSet.FindAsync(id);
    }

   
    public async Task AddAsync(TEntity entity)
    {
       await _dbSet.AddAsync(entity);
    }
    public void Update(TEntity entity)
    {
      _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
      _dbSet.Remove(entity);

    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> spec, bool treackChanges = false)
    {
      return await ApplySpecifications(spec).ToListAsync();
    }

    public async Task<TEntity?> GetAsync(ISpecifications<TEntity, Tkey> spec)
    {
      return await ApplySpecifications(spec).FirstOrDefaultAsync();
    }


    public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> spec)
    {
      return await ApplySpecifications(spec).CountAsync();
    }

    private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity , Tkey> spec)
    {
      return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec);
    }

    public async Task<TEntity?> GetByIdAsync(Tkey id, bool includeRelated = false)
    {
      IQueryable<TEntity> query = _dbSet;

      if (includeRelated && typeof(TEntity) == typeof(Order))
      {
        query = query
            .Include("OrderItems.Product")
            .Include("DeliveryMethod") as IQueryable<TEntity>;
      }

      return await query.FirstOrDefaultAsync(e => e.Id!.Equals(id));
    }
  }
}
