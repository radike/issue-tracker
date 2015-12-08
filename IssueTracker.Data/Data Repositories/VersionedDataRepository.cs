using Common.Data.Core;
using Common.Data.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IssueTracker.Data.Data_Repositories
{
    public abstract class VersionedDataRepository<TEntity> : DataRepositoryBase<TEntity>
        where TEntity : class, IIdentifiableEntity, IVersionableEntity, new()
    {
        public VersionedDataRepository(IDbContext context)
            :base(context)
        {

        }

        public override TEntity Get(Guid id)
        {
            return base.FindSingleBy(i => i.Id == id);
        }

        public override ICollection<TEntity> GetAll()
        {
            return base.FindBy(i => i.Active).GroupBy(i => i.Id).Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault()).ToList();
        }

        public override TEntity FindSingleBy(Expression<Func<TEntity, bool>> predicate)
        {
            return base.FindBy(predicate).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        }

        public override IQueryable<TEntity> Fetch()
        {
            return base.FindBy(i => i.Active).GroupBy(i => i.Id).Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault());
        }
    }
}
