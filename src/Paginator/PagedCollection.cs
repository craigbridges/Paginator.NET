namespace Paginator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the default implementation for IPagedCollection
    /// </summary>
    /// <typeparam name="T">The type of objects to paginate</typeparam>
    public class PagedCollection<T> : IPagedCollection<T>
    {
        private readonly IQueryable<T> _source;
        
        public PagedCollection(IEnumerable<T> source, int pageSize)
            : this(source?.AsQueryable(), pageSize)
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

            this.PageSize = pageSize;
            this.PageCount = pageCount;
            this.ItemCount = totalCount;
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
            for (var number = 0; number <= this.PageCount-1; number++)
            {
                yield return GetPage(number);
            }
        }

        public IEnumerable<T> GetItems(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > this.PageCount)
            {
                throw new ArgumentOutOfRangeException
                (
                    nameof(pageNumber),
                    $"The number {pageNumber} is outside the available page range."
                );
            }

            if (this.ItemCount == 0)
            {
                return _source;
            }

            var pageSize = this.PageSize;
            var skipCount = ((pageNumber * pageSize) - pageSize);

            var items = _source.Skip(skipCount).Take(pageSize);

            return items;
        }

        public IEnumerable<T> GetAllItems() => _source;

        public PagedResult<T> this[int page] => GetPage(page);

        public IEnumerator<T> GetEnumerator() => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _source.GetEnumerator();
    }
}
