namespace Forum.API.Pagination.Params
{
    public class PagingParams
    {
        private readonly int MaxPageSize = 50;
        private int _pageNumber = 1;
        public int PageNumber {
            get => _pageNumber;
            set
            {
                if(value <= 0)
                {
                    _pageNumber = 1;
                }
                else
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
                if(value <= MaxPageSize)
                {
                    _pageSize = value;
                }
                else
                {
                    _pageSize = MaxPageSize;
                }
            }
        }
    }
}
