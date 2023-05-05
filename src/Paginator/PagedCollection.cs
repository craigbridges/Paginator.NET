namespace Paginator
{
    /// <summary>
    /// Represents the default implementation for IPagedCollection
    /// </summary>
    /// <typeparam name="T">The type of objects to paginate</typeparam>
    public class PagedCollection<T> : IPagedCollection<T>
    {
        private readonly IQueryable<T> _source;
        
        public PagedCollection(IEnumerable<T> source, int pageSize)
            : this(source.AsQueryable(), pageSize)
        { }

        public PagedCollection(IQueryable<T> source, int pageSize)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            var totalCount = source.Count();
            var pageCount = CalculatePageCount(pageSize, totalCount);

            PageSize = pageSize;
            PageCount = pageCount;
            ItemCount = totalCount;
        }

        protected int CalculatePageCount(int pageSize, int totalCount)
        {
            if (totalCount == 0)
            {
                return 0;
            }

            var remainder = totalCount % pageSize;
            var pageCount = ((totalCount / pageSize) + (remainder == 0 ? 0 : 1));

            return pageCount;
        }

        public int PageSize { get; }
        public int PageCount { get; }
        public int ItemCount { get; }

        public PagedResult<T> GetPage(int pageNumber)
        {
            return new PagedResult<T>(this, pageNumber);
        }

        public IEnumerable<PagedResult<T>> GetAllPages()
        {
            for (var number = 1; number <= PageCount; number++)
            {
                yield return GetPage(number);
            }
        }

        public IEnumerable<T> GetItems(int pageNumber)
        {
            var pageCount = PageCount;

            if (pageNumber == 1 && pageCount == 0)
            {
                return Array.Empty<T>();
            }

            if (pageNumber < 1 || pageNumber > pageCount)
            {
                throw new ArgumentOutOfRangeException
                (
                    nameof(pageNumber),
                    $"The number {pageNumber} is outside the available page range."
                );
            }

            if (ItemCount == 0)
            {
                return _source;
            }

            var pageSize = PageSize;
            var skipCount = ((pageNumber * pageSize) - pageSize);

            return _source.Skip(skipCount).Take(pageSize);
        }

        public IEnumerable<T> GetAllItems() => _source;

        public PagedResult<T> this[int page] => GetPage(page);

        public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _source.GetEnumerator();
    }
}
