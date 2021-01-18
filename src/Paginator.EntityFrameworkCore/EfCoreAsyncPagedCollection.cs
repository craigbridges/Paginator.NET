namespace Paginator.Async.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    /// <summary>
    /// An Entity Framework Core implementation of IAsyncPagedCollection
    /// </summary>
    /// <typeparam name="T">The type of objects to paginate</typeparam>
    public class EfCoreAsyncPagedCollection<T> : AsyncPagedCollection<T>
    {
        public EfCoreAsyncPagedCollection(IQueryable<T> source, int pageSize)
            : base(source.AsAsyncEnumerable(), pageSize)
        { }
    }
}
