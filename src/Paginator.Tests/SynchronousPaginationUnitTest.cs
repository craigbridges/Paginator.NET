namespace Paginator.Tests;

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
        var pages = new PagedCollection<string>(Array.Empty<string>(), PageSize);

        // Act
        var page = pages[1];

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), Array.Empty<string>());
    }

    [Test]
    public void Paginate_First_Page_Using_PagedCollection()
    {
        // Arrange
        var words = Words;
        var pages = new PagedCollection<string>(words, PageSize);

        // Act
        var page = pages[1];

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), FirstPageWords);
    }

    [Test]
    public void Paginate_Second_Page_Using_PagedCollection()
    {
        // Arrange
        var words = Words;
        var pages = new PagedCollection<string>(words, PageSize);

        // Act
        var page = pages[2];

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), SecondPageWords);
    }

    [Test]
    public void Paginate_Third_Page_Using_PagedCollection()
    {
        // Arrange
        var words = Words;
        var pages = new PagedCollection<string>(words, PageSize);

        // Act
        var page = pages[3];

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), ThirdPageWords);
    }

    [Test]
    public void Paginate_First_Page_Using_Extension_Method()
    {
        // Arrange
        var words = Words;

        // Act
        var page = words.Page(1, PageSize);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), FirstPageWords);
    }

    [Test]
    public void Paginate_Second_Page_Using_Extension_Method()
    {
        // Arrange
        var words = Words;

        // Act
        var page = words.Page(2, PageSize);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), SecondPageWords);
    }

    [Test]
    public void Paginate_Third_Page_Using_Extension_Method()
    {
        // Arrange
        var words = Words;

        // Act
        var page = words.Page(3, PageSize);

        // Assert
        CollectionAssert.AreEqual(page.Results.ToArray(), ThirdPageWords);
    }
}
