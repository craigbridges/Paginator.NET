namespace Paginator
{
    using System.Collections.Generic;
    
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Takes a page of items from a collection
        /// </summary>
        /// <typeparam name="T">The collection type</typeparam>
        /// <param name="source">The collection to get the page from</param>
        /// <param name="number">The page number</param>
        /// <param name="size">The page size (number of items per page)</param>
        /// <returns>A paged result</returns>
        public static PagedResult<T> Page<T>(this IEnumerable<T> source, int number, int size)
        {
            var pagedCollection = new PagedCollection<T>(source, size);

            return pagedCollection[number];
        }
    }
}
