using System;

namespace Owleye.Shared.Data
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; }

        public DateTimeOffset Created { get; protected set; }
        public DateTimeOffset? Modified { get; protected set; }

        public int CreatedById { get; protected set; }
        public int? ModifiedById { get; protected set; }
    }
}
