namespace Paginator
{
    /// <summary>
    /// Represents the result of a single page from a paged collection
    /// </summary>
    /// <typeparam name="T">The paged object type</typeparam>
    public class PagedResult<T>
    {
        public PagedResult(int pageNumber, int pageCount, int pageSize, int itemCount, IEnumerable<T> results)
        {
            CurrentPageNumber = pageNumber;
            PageCount = pageCount;
            PageSize = pageSize;
            TotalItemCount = itemCount;
            Results = results ?? throw new ArgumentNullException(nameof(results));
        }

        internal protected PagedResult(IPagedCollection<T> collection, int pageNumber)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            CurrentPageNumber = pageNumber;
            PageCount = collection.PageCount;
            PageSize = collection.PageSize;
            TotalItemCount = collection.ItemCount;
            Results = collection.GetItems(pageNumber);
        }

        /// <summary>
        /// Gets the current page number
        /// </summary>
        public int CurrentPageNumber { get; }

        /// <summary>
        /// Gets the page count (for the page size)
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        /// Gets the page size (number of objects per page)
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total number of items in all pages
        /// </summary>
        public int TotalItemCount { get; }

        /// <summary>
        /// Gets the objects in the page
        /// </summary>
        public IEnumerable<T> Results { get; }

        /// <summary>
        /// Gets a description of the paged result
        /// </summary>
        /// <returns>A page indicator description</returns>
        public override string ToString() => $"Page {CurrentPageNumber} of {PageCount}";
    }
}
