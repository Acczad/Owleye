using System;

namespace Owleye.Shared.Model
{
    public abstract class PagedResultBase
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
