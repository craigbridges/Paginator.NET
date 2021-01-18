namespace System.Collections.Generic
{
    using Paginator;
    using Paginator.Async;
    using System.Threading.Tasks;

    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// Asynchronously takes a page of items from an asynchronous collection
        /// </summary>
        /// <typeparam name="T">The collection type</typeparam>
        /// <param name="source">The collection to get the page from</param>
        /// <param name="number">The page number</param>
        /// <param name="size">The page size (number of items per page)</param>
        /// <returns>A paged result</returns>
        public static async Task<PagedResult<T>> PageAsync<T>(this IAsyncEnumerable<T> source, int number, int size)
        {
            var pagedCollection = new AsyncPagedCollection<T>(source, size);

            return await pagedCollection[number].ConfigureAwait(false);
        }
    }
}
