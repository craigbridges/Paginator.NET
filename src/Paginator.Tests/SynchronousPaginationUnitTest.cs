namespace Paginator.Tests
{
    using NUnit.Framework;
    using System.Linq;

    public class SynchronousPaginationUnitTest
    {
        const int PageSize = 3;

        static readonly string[] Words = 
        {
            "dotnet",
            "standard",
            "pagination",
            "collection",
            "synchronous",
            "asynchronous",
            "nuget"
        };

        static readonly string[] FirstPageWords = { "dotnet", "standard", "pagination" };
        static readonly string[] SecondPageWords = { "collection", "synchronous", "asynchronous" };
        static readonly string[] ThirdPageWords = { "nuget" };

        [Test]
        public void Paginate_First_Page_Using_EmptyCollection()
        {
            // Arrange
            var pages = new PagedCollection<string>(new string[] { }, SynchronousPaginationUnitTest.PageSize);

            // Act
            var page = pages[1];

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                new string[] { }
            );
        }

        [Test]
        public void Paginate_First_Page_Using_PagedCollection()
        {
            // Arrange
            var words = SynchronousPaginationUnitTest.Words;
            var pages = new PagedCollection<string>(words, SynchronousPaginationUnitTest.PageSize);

            // Act
            var page = pages[1];

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                SynchronousPaginationUnitTest.FirstPageWords
            );
        }

        [Test]
        public void Paginate_Second_Page_Using_PagedCollection()
        {
            // Arrange
            var words = SynchronousPaginationUnitTest.Words;
            var pages = new PagedCollection<string>(words, SynchronousPaginationUnitTest.PageSize);

            // Act
            var page = pages[2];

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                SynchronousPaginationUnitTest.SecondPageWords
            );
        }

        [Test]
        public void Paginate_Third_Page_Using_PagedCollection()
        {
            // Arrange
            var words = SynchronousPaginationUnitTest.Words;
            var pages = new PagedCollection<string>(words, SynchronousPaginationUnitTest.PageSize);

            // Act
            var page = pages[3];

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                SynchronousPaginationUnitTest.ThirdPageWords
            );
        }

        [Test]
        public void Paginate_First_Page_Using_Extension_Method()
        {
            // Arrange
            var words = SynchronousPaginationUnitTest.Words;

            // Act
            var page = words.Page(1, SynchronousPaginationUnitTest.PageSize);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                SynchronousPaginationUnitTest.FirstPageWords
            );
        }

        [Test]
        public void Paginate_Second_Page_Using_Extension_Method()
        {
            // Arrange
            var words = SynchronousPaginationUnitTest.Words;

            // Act
            var page = words.Page(2, SynchronousPaginationUnitTest.PageSize);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                SynchronousPaginationUnitTest.SecondPageWords
            );
        }

        [Test]
        public void Paginate_Third_Page_Using_Extension_Method()
        {
            // Arrange
            var words = SynchronousPaginationUnitTest.Words;

            // Act
            var page = words.Page(3, SynchronousPaginationUnitTest.PageSize);

            // Assert
            CollectionAssert.AreEqual
            (
                page.Results.ToArray(),
                SynchronousPaginationUnitTest.ThirdPageWords
            );
        }
    }
}
