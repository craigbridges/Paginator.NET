# Paginator.NET
A lightweight library written in C# .NET Standard for handling paginating collections of objects in .NET code.

## Getting Started
Paginator can be installed using the NuGet package manager:

```
Install-Package Paginator.NET
```

 Or the dotnet CLI:

 ```
 dotnet add package Paginator.NET
 ```

## Synchronous and Asynchronous Pagination
Paginator can handle synchronous and asynchronous pagination. Synchronous pagination is handled in the `Paginator.NET` NuGet package, but asynchronous pagination is handled in the `Paginator.NET.Async NuGet` package.

## Synchronous Example
```c#
using Paginator;

const int PageSize = 3;

var wordList = new List<string>()
{
    "dotnet",
    "standard",
    "pagination",
    "collection",
    "synchronous",
    "asynchronous",
    "nuget"
};

// EITHER:

// Break the word list into pages of 3 items and take the first page.
// Note, any collection that implements IEnumerable is supported.
var firstPage = wordList.Page(1, PageSize);

// OR:

// This will create a paged collection with 3 pages, the first two pages 
// will contain 3 items, while the last page will contain 1 item.
var pages = new PagedCollection<string>(wordList, PageSize);
var firstPage = pages[1]; // Alternatively use pages.GetPage(1);

// THEN:

foreach (var word in firstPage.Results)
{
    Console.WriteLine($"'{word}'\n");
}

// OUTPUT:

/*
'dotnet'
'standard'
'pagination'
*/
```

## Asynchronous Example
```c#
using Paginator.Async;

const int PageSize = 3;

async IAsyncEnumerable<string> GetWordsAsync()
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

var words = GetWordsAsync();

// EITHER:

// Break the word list into pages of 3 items and take the first page.
// Note, any collection that implements IAsyncEnumerable is supported.
var firstPage = await words.PageAsync(1, PageSize).ConfigureAwait(false);

// OR:

// This will create an asynchronous paged collection with 3 pages, the first
// two pages will contain 3 items, while the last page will contain 1 item.
var pages = new AsyncPagedCollection<string>(words, PageSize);
var firstPage = await pages[1].ConfigureAwait(false);
// Alternatively use pages.GetPageAsync(1).ConfigureAwait(false);

// THEN:

foreach (var word in firstPage.Results)
{
    Console.WriteLine($"'{word}'\n");
}

// OUTPUT:

/*
'dotnet'
'standard'
'pagination'
*/
```

## Entity Framework Core Integration
If you are using Entity Framework Core asynchronously, there is a NuGet package that allows IQueryable to be used with IAsyncPagedCollection, the package can be installed using the NuGet package manager:

```
Install-Package Paginator.NET.EntityFrameworkCore
```

 Or the dotnet CLI:

 ```
 dotnet add package Paginator.NET.EntityFrameworkCore
 ```