﻿namespace Paginator.Async;

/// <summary>
/// A LINQ based implementation of IAsyncPagedCollection
/// </summary>
/// <typeparam name="T">The type of objects to paginate</typeparam>
public class AsyncPagedCollection<T> : IAsyncPagedCollection<T>
{
    private readonly IAsyncEnumerable<T> _source;

    private int? _cachedPageCount;
    private readonly Dictionary<int, IEnumerable<T>> _cachedItems = new();

    private int? _cachedTotalCount;
    private IEnumerable<T>? _cachedAllItems;

    public AsyncPagedCollection(IAsyncEnumerable<T> source, int pageSize)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }

        PageSize = pageSize;
    }

    public int PageSize { get; }

    public async Task<int> GetPageCountAsync(CancellationToken cancellationToken = default)
    {
        if (_cachedPageCount == null)
        {
            var pageSize = PageSize;
            var totalCount = await GetItemCountAsync(cancellationToken).ConfigureAwait(false);

            _cachedPageCount = CalculatePageCount(pageSize, totalCount);
        }

        return _cachedPageCount.Value;
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

    public async Task<PagedResult<T>> GetPageAsync(int pageNumber, CancellationToken cancellationToken = default)
    {
        var items = await GetItemsAsync(pageNumber, cancellationToken).ConfigureAwait(false);
        var pageCount = await GetPageCountAsync(cancellationToken).ConfigureAwait(false);
        var itemCount = await GetItemCountAsync(cancellationToken).ConfigureAwait(false);

        return new PagedResult<T>(pageNumber, pageCount, PageSize, itemCount, items);
    }

    public Task<PagedResult<T>> this[int page, CancellationToken cancellationToken = default]
    {
        get
        {
            return GetPageAsync(page, cancellationToken);
        }
    }

    public async Task<IEnumerable<PagedResult<T>>> GetAllPagesAsync(CancellationToken cancellationToken = default)
    {
        var pageSize = PageSize;
        var pages = new List<PagedResult<T>>();

        var allItems = await GetAllItemsAsync(cancellationToken).ConfigureAwait(false);
        var pageCount = await GetPageCountAsync(cancellationToken).ConfigureAwait(false);
        var itemCount = allItems.Count();

        if (itemCount == 0)
        {
            return new PagedResult<T>[]
            {
                new PagedResult<T>(1, 0, pageSize, 0, Array.Empty<T>())
            };
        }

        for (var pageNumber = 1; pageNumber <= pageCount; pageNumber++)
        {
            var skipCount = (pageNumber * pageSize);
            var pageItems = allItems.Skip(skipCount).Take(pageSize);

            var page = new PagedResult<T>(pageNumber, pageCount, pageSize, itemCount, pageItems);

            pages.Add(page);
        }

        return pages;
    }

    public async Task<int> GetItemCountAsync(CancellationToken cancellationToken = default)
    {
        _cachedTotalCount ??= await _source.CountAsync(cancellationToken).ConfigureAwait(false);

        return _cachedTotalCount.Value;
    }

    public async Task<IEnumerable<T>> GetItemsAsync(int pageNumber, CancellationToken cancellationToken = default)
    {
        var pageCount = await GetPageCountAsync(cancellationToken).ConfigureAwait(false);

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

        if (_cachedItems.TryGetValue(pageNumber, out IEnumerable<T>? value))
        {
            return value;
        }
        else
        {
            var totalCount = await GetItemCountAsync(cancellationToken).ConfigureAwait(false);

            if (totalCount == 0)
            {
                return await _source.ToListAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var pageSize = PageSize;
                var skipCount = ((pageNumber * pageSize) - pageSize);

                var items = await _source
                    .Skip(skipCount)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                _cachedItems[pageNumber] = items;

                return items;
            }
        }
    }

    public async Task<IEnumerable<T>> GetAllItemsAsync(CancellationToken cancellationToken = default)
    {
        _cachedAllItems ??= await _source.ToListAsync(cancellationToken).ConfigureAwait(false);

        return _cachedAllItems;
    }
}
