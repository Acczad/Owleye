using System;

namespace Owleye.Shared.Data
{
    public interface IBaseEntity
    {
        public int Id { get;  }

        public DateTimeOffset Created { get;}
        public DateTimeOffset? Modified { get; }

        public int CreatedById { get;}
        public int? ModifiedById { get; }
    }
}