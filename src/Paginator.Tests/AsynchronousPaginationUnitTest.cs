namespace Paginator.Tests;

using Paginator.Async;

public class AsynchronousPaginationUnitTest
{
    const int PageSize = 3;

    static async IAsyncEnumerable<string> GetWordsAsync()
    {
        yield return "dotnet";
        yield return "standard";
        yield return "pagination";
        yield return "collection";
        yield return "synchronous";
        yield return "asynchronous";
        yield return "nuget";

        await Task.CompletedTask;
    }

    static readonly string[] FirstPageWords = { "dotnet", "standard", "pagination" };
    static readonly string[] SecondPageWords = { "collection", "synchronous", "asynchronous" };
    static readonly string[] ThirdPageWords = { "nuget" };

    [Test]
    public async Task PaginateAsync_First_Page_Using_EmptyCollection()
    {
        // Arrange
        var pages = new AsyncPagedCollection<string>(AsyncEnumerable.Empty<string>(), PageSize);

        // Act
        var page = await pages[1].ConfigureAwait(false);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), Array.Empty<string>());
    }

    [Test]
    public async Task PaginateAsync_First_Page_Using_PagedCollection()
    {
        // Arrange
        var words = GetWordsAsync();
        var pages = new AsyncPagedCollection<string>(words, PageSize);

        // Act
        var page = await pages[1].ConfigureAwait(false);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), FirstPageWords);
    }

    [Test]
    public async Task PaginateAsync_Second_Page_Using_PagedCollection()
    {
        // Arrange
        var words = GetWordsAsync();
        var pages = new AsyncPagedCollection<string>(words, PageSize);

        // Act
        var page = await pages[2].ConfigureAwait(false);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), SecondPageWords);
    }

    [Test]
    public async Task PaginateAsync_Third_Page_Using_PagedCollection()
    {
        // Arrange
        var words = GetWordsAsync();
        var pages = new AsyncPagedCollection<string>(words, PageSize);

        // Act
        var page = await pages[3].ConfigureAwait(false);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), ThirdPageWords);
    }

    [Test]
    public async Task PaginateAsync_First_Page_Using_Extension_Method()
    {
        // Arrange
        var words = GetWordsAsync();

        // Act
        var page = await words.PageAsync(1, PageSize).ConfigureAwait(false);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), FirstPageWords);
    }

    [Test]
    public async Task PaginateAsync_Second_Page_Using_Extension_Method()
    {
        // Arrange
        var words = GetWordsAsync();

        // Act
        var page = await words.PageAsync(2, PageSize).ConfigureAwait(false);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), SecondPageWords);
    }

    [Test]
    public async Task PaginateAsync_Third_Page_Using_Extension_Method()
    {
        // Arrange
        var words = GetWordsAsync();

        // Act
        var page = await words.PageAsync(3, PageSize).ConfigureAwait(false);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), ThirdPageWords);
    }
}
