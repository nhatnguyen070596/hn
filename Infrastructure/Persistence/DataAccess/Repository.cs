using System;
using ApplicationCore.Aggregates;
using ApplicationCore.Interfaces.DataAccess;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataAccess
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : AggregateRoot
    {
        protected readonly HomeNursingContext DbContext;

        protected Repository(HomeNursingContext dbContext) => DbContext = dbContext;

        public async Task<IReadOnlyList<TEntity>> GetALl()
            => await DbContext.Set<TEntity>().ToListAsync();

        public void Add(TEntity entity)
            => DbContext.Set<TEntity>().Add(entity);

        public void Update(TEntity entity)
            => DbContext.Set<TEntity>().Update(entity);

        public void Remove(TEntity entity)
            => DbContext.Set<TEntity>().Remove(entity);
    }
}

