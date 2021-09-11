using System.Collections.Generic;

namespace Owleye.Shared.Model
{
    public class QueryListResult<T>
    {
        public QueryListResult(IEnumerable<T> data)
        {
            Data = data;
        }
        public IEnumerable<T> Data { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
    }
}
