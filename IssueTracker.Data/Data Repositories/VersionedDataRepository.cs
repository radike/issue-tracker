using Common.Data.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.Data.Data_Repositories
{
    public abstract class VersionedDataRepository<TEntity> : DataRepositoryBase<TEntity>
        where TEntity : class, IIdentifiableEntity, IVersionableEntity, new()
    {
        protected VersionedDataRepository(IDbContext context)
            :base(context)
        {

        }

        public override TEntity Get(Guid id)
        {
            return FindSingleBy(i => i.Id == id);
        }

        public override ICollection<TEntity> GetAll()
        {
            return Fetch().ToList();
        }

        public override IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Fetch().Where(predicate);
        }

        public override TEntity FindSingleBy(Expression<Func<TEntity, bool>> predicate)
        {
            return base.FindBy(predicate).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        }

        public override IQueryable<TEntity> Fetch()
        {
            return base.FindBy(i => i.Active).GroupBy(i => i.Id).Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault());
        }

        public IQueryable<TEntity> GetAllVersions(Guid id)
        {
            return base.FindBy(i => i.Id == id);
        }

        public override void Remove(Guid id)
        {
            var entities = base.FindBy(p => p.Id == id);
            // todo: remove sets Active flag only for the last version
            foreach (var entity in entities)
            {
                entity.Active = false;
            }
            base.Save();
        }
    }
}
