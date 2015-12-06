using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Common.Data.Core.Contracts
{
    public interface IDbContext
    {
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet Set(Type entityType);
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
