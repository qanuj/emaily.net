using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emaily.Core.Abstraction.Events;

namespace Emaily.Core.Abstraction.Persistence
{
    public interface IRepository<TEntity, in TKey> where TEntity : IEntity
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();

        TEntity ById(TKey id);
        IQueryable<TEntity> ById(IEnumerable<TKey> ids);

        TEntity Create(TEntity entity); //returns Rows Affected
        ICollection<TEntity> Create(ICollection<TEntity> entity); //returns Rows Affected

        TEntity Update(TEntity entity); //returns Rows Affected
        TEntity Delete(TEntity entity); //returns Rows Affected
        TEntity Delete(TKey id); //returns Rows Affected
        TEntity Attach(TEntity entity); //returns Rows Affected
        ICollection<TEntity> Delete(ICollection<TEntity> entities); //returns Rows Affected
        TEntity Purge(TEntity entity);

        IQueryable<TEntity> All { get; }

        
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : Entity
    {

    }
}
