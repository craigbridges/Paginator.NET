namespace Paginator;

public static class PagedResultExtensions
{
    /// <summary>
    /// Converts a paged result to the new type specified
    /// </summary>
    /// <typeparam name="TFrom">The collection type to convert from</typeparam>
    /// <typeparam name="TTo">The collection type to convert to</typeparam>
    /// <param name="originalResult">The original result</param>
    /// <param name="convertedItems">The converted collection items</param>
    /// <returns>A paged result with the new items</returns>
    public static PagedResult<TTo> ConvertTo<TFrom, TTo>(this PagedResult<TFrom> originalResult, IEnumerable<TTo> convertedItems)
    {
        if (originalResult == null)
        {
            throw new ArgumentNullException(nameof(originalResult));
        }

        return new PagedResult<TTo>
        (
            originalResult.CurrentPageNumber,
            originalResult.PageCount,
            originalResult.PageSize,
            originalResult.TotalItemCount,
            convertedItems
        );
    }
}
