using System;

namespace Emaily.Core.Abstraction
{
    public abstract class Entity : IEntity, ISoftDelete
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }

        protected Entity()
        {
            this.Created = DateTime.UtcNow;
        }
    }
}