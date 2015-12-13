using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emaily.Core.Abstraction;
using Emaily.Core.Abstraction.Events;
using Emaily.Core.Abstraction.Persistence;

namespace Emaily.Data
{
    public abstract class EfRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, IEntity, ISoftDelete
    {
        public IEntityFunctions Funcs { get; set; }

        protected EfRepository(DbContext context, IEventManager eventManager)
        {
            Context = context;
            EventManager = eventManager;
        }

        protected DbContext Context { get; }

        public IQueryable<TEntity> ById(IEnumerable<int> ids)
        {
            return All.Where(x => ids.Contains(x.Id));
        }

        public virtual IQueryable<TEntity> All
        {
            get
            {
                return Set.Where(x => !x.Deleted.HasValue);
            }
        }

        private DbSet<TEntity> Set => Context.Set<TEntity>();

        protected void Attach(TEntity entity, EntityState state)
        {
            Set.Attach(entity);
            Context.Entry(entity).State = state;
        }


        protected int UpdateEntity(TEntity entity)
        {
            Set.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChanges();
        }

        protected int CreateEntity(TEntity entity)
        {
            Set.Attach(entity);
            Context.Entry(entity).State = EntityState.Added;
            return Context.SaveChanges();
        }

        public virtual TEntity Create(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            entity.Created = DateTime.UtcNow;
            entity.Modified = DateTime.UtcNow;
            return entity;
        }

        public virtual TEntity ById(int id)
        {
            var entity = Set.Find(id);
            return entity?.Deleted != null ? null : entity;
        }

        public virtual TEntity Purge(TEntity entity)
        {
            Attach(entity);
            Context.Set<TEntity>().Remove(entity);
            return entity;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            Attach(entity);
            entity.Deleted = DateTime.UtcNow;
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            entity.Modified=DateTime.UtcNow;
            return entity;
        }

        protected void AttachDetachedCollection(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Attach(entity);
                Context.Entry(entity).State = EntityState.Unchanged;
            }
        }

        public TEntity Attach(TEntity entity)
        {
            Set.Attach(entity);
            return entity;
        }

        internal void Attach(ICollection<TEntity> entities)
        {
            AttachDetachedCollection(entities);
        }

        internal delegate void EntityEvent(TEntity entity);

        public IEventManager EventManager { get; }


        public virtual ICollection<TEntity> Create(ICollection<TEntity> entities)
        {
            Attach(entities);
            return entities;
        }

        public virtual ICollection<TEntity> Delete(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
            return entities;
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public TEntity Delete(int id)
        {
            return Delete(ById(id));
        }
    }
}
