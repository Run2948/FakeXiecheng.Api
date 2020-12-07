namespace FakeXiecheng.Api.Models.Requests
{
    public class PaginationRequest
    {
        private const int MaxPageSize = 50;

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value >= 1)
                {
                    _pageNumber = value;
                }
            }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value >= 1)
                {
                    _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
                }
            }
        }

        public PaginationRequest Decr(int page = 1)
        {
            PageNumber -= page;
            return this;
        }

        public PaginationRequest Incr(int page = 1)
        {
            PageNumber += page;
            return this;
        }
    }
}