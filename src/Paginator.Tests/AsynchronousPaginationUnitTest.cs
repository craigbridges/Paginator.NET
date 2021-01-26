namespace Paginator.Tests
{
    using NUnit.Framework;
    using Paginator.Async;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
            var pages = new AsyncPagedCollection<string>(AsyncEnumerable.Empty<string>(), AsynchronousPaginationUnitTest.PageSize);

            // Act
            var page = await pages[1].ConfigureAwait(false);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                new string[] { }
            );
        }

        [Test]
        public async Task PaginateAsync_First_Page_Using_PagedCollection()
        {
            // Arrange
            var words = GetWordsAsync();
            var pages = new AsyncPagedCollection<string>(words, AsynchronousPaginationUnitTest.PageSize);

            // Act
            var page = await pages[1].ConfigureAwait(false);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                AsynchronousPaginationUnitTest.FirstPageWords
            );
        }

        [Test]
        public async Task PaginateAsync_Second_Page_Using_PagedCollection()
        {
            // Arrange
            var words = GetWordsAsync();
            var pages = new AsyncPagedCollection<string>(words, AsynchronousPaginationUnitTest.PageSize);

            // Act
            var page = await pages[2].ConfigureAwait(false);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                AsynchronousPaginationUnitTest.SecondPageWords
            );
        }

        [Test]
        public async Task PaginateAsync_Third_Page_Using_PagedCollection()
        {
            // Arrange
            var words = GetWordsAsync();
            var pages = new AsyncPagedCollection<string>(words, AsynchronousPaginationUnitTest.PageSize);

            // Act
            var page = await pages[3].ConfigureAwait(false);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                AsynchronousPaginationUnitTest.ThirdPageWords
            );
        }

        [Test]
        public async Task PaginateAsync_First_Page_Using_Extension_Method()
        {
            // Arrange
            var words = GetWordsAsync();

            // Act
            var page = await words.PageAsync(1, AsynchronousPaginationUnitTest.PageSize).ConfigureAwait(false);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                AsynchronousPaginationUnitTest.FirstPageWords
            );
        }

        [Test]
        public async Task PaginateAsync_Second_Page_Using_Extension_Method()
        {
            // Arrange
            var words = GetWordsAsync();

            // Act
            var page = await words.PageAsync(2, AsynchronousPaginationUnitTest.PageSize).ConfigureAwait(false);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                AsynchronousPaginationUnitTest.SecondPageWords
            );
        }

        [Test]
        public async Task PaginateAsync_Third_Page_Using_Extension_Method()
        {
            // Arrange
            var words = GetWordsAsync();

            // Act
            var page = await words.PageAsync(3, AsynchronousPaginationUnitTest.PageSize).ConfigureAwait(false);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                AsynchronousPaginationUnitTest.ThirdPageWords
            );
        }
    }
}
