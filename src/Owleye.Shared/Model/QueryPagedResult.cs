using System.Collections.Generic;

namespace Owleye.Shared.Model
{
    public class QueryPagedResult<T> : PagedResultBase
    {
        public QueryPagedResult(IEnumerable<T> data)
        {
            Data = data;
        }

        public QueryPagedResult()
        { 
        }

        public IEnumerable<T> Data { get; set; }

    }
}
