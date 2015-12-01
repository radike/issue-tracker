using IssueTracker.Core.Contracts;
using IssueTracker.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Core.Data
{
    public abstract class DataRepositoryBase<TEntity, TContext> : IDataRepository<TEntity>
        where TEntity : class, IIdentifiableEntity, new()
        where TContext : DbContext, new()
    {
        protected abstract TEntity AddEntity(TContext entityContext, TEntity entity);

        protected abstract TEntity UpdateEntity(TContext entityContext, TEntity entity);

        protected abstract IEnumerable<TEntity> GetEntities(TContext entityContext);

        protected abstract TEntity GetEntity(TContext entityContext, Guid id);

        public TEntity Add(TEntity entity)
        {
            using (TContext entityContext = new TContext())
            {
                TEntity addedEntity = AddEntity(entityContext, entity);
                
                entityContext.SaveChanges();
                
                return addedEntity;
            }
        }

        public void Remove(TEntity entity)
        {
            using (TContext entityContext = new TContext())
            {
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (TContext entityContext = new TContext())
            {
                TEntity entity = GetEntity(entityContext, id);
                entityContext.Entry<TEntity>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (TContext entityContext = new TContext())
            {
                TEntity existingEntity = UpdateEntity(entityContext, entity);

                SimpleMapper.PropertyMap(entity, existingEntity);

                entityContext.SaveChanges();
                return existingEntity;
            }
        }

        public IEnumerable<TEntity> Get()
        {
            using (TContext entityContext = new TContext())
            {
                entityContext.Configuration.AutoDetectChangesEnabled = false;
                entityContext.Configuration.LazyLoadingEnabled = false;
                return (GetEntities(entityContext)).ToArray().ToList();
            }
        }

        public TEntity Get(Guid id)
        {
            using (TContext entityContext = new TContext())
                return GetEntity(entityContext, id);
        }
    }
}
