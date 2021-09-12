namespace Owleye.Shared.Model
{
    public class PagedQuery
    {
        const int maxPageSize = 50;
        public PagedQuery(int pageSize, int pageNumber)
        {
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
        }

        public PagedQuery()
        {

        }

        public int PageNumber
        {
            get
            {
                return _pageNmber;
            }
            set
            {
                _pageSize = (value <= 0) ? 1 : value;
            }
        }

        private int _pageSize = 10;
        private int _pageNmber = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
