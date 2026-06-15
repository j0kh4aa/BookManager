namespace BookManager.Services;

using BookManager.Interfaces;
using BookManager.Models;
using System.Linq;

/// <summary>
/// Manages the book collection. Implements IBookRepository.
/// Demonstrates Encapsulation (private list) and Interface implementation.
/// </summary>
public class BookManagerService : IBookRepository
{
    // Private list — encapsulated, not accessible directly from outside
    private readonly List<Book> books = new();

    /// <summary>
    /// Adds a new book to the collection.
    /// Validates that no duplicate title exists.
    /// </summary>
    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        // Validation: no duplicate titles
        bool exists = books.Any(b => b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase));
        if (exists)
        {
            Console.WriteLine($"  [!] A book with the title \"{book.Title}\" already exists.");
            return;
        }

        books.Add(book);
        Console.WriteLine($"  [+] Added: {book}");
    }

    /// <summary>
    /// Returns a copy of all books in the collection.
    /// </summary>
    public List<Book> GetAllBooks()
    {
        return books;
    }

    /// <summary>
    /// Finds and returns a book by its title (case-insensitive).
    /// Returns null if not found.
    /// </summary>
    public Book? FindByTitle(string title)
    {
        return books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public Book? FindByAuthor(string author)
    {
        return books.FirstOrDefault(a => a.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
    }

    public List<Book> SortByPages()
    {
        return books.OrderBy(b => b.Pages).ToList();
    }

    public List<Book> SortByPrice()
    {
        return books.OrderBy(b => b.Price).ToList();
    }

    public List<Book> SortByYear()
    {
        return books.OrderBy(b => b.Year).ToList();
    }
    /// <summary>
    /// Seeds the collection with 20 sample books for demonstration.
    /// </summary>
    public void SeedData()
    {
        var books = new List<Book>
    {
        new Book("Clean Code",                    "Robert C. Martin",  2008, 431,  35.99,""),
        new Book("The Pragmatic Programmer",      "David Thomas",      1999, 352,  42.00,""),
        new Book("Design Patterns",               "Gang of Four",      1994, 395,  54.99, ""),
        new Book("Refactoring",                   "Martin Fowler",     1999, 448,  45.00,""),
        new Book("The Mythical Man-Month",        "Frederick Brooks",  1975, 322,  28.50,""),
        new Book("Code Complete",                 "Steve McConnell",   2004, 960,  49.99,""),
        new Book("Introduction to Algorithms",    "Thomas Cormen",     2009, 1292, 89.99,""),
        new Book("Structure and Interpretation",  "Harold Abelson",    1996, 657,  55.00,""),
        new Book("The Art of Computer Programming","Donald Knuth",     1968, 672,  120.00,""),
        new Book("Working Effectively with Legacy Code","Michael Feathers",2004,456,38.00,""),
         new Book("me bebia iliko da ilarioni","nodar dumbadze",2016,232,40.00,""),
        new EBook("C# in Depth",                 "Jon Skeet",         2019, 528,  29.99, "https://books.example.com/csharp-in-depth",""),
        new EBook("Pro ASP.NET Core",            "Adam Freeman",      2022, 1248, 39.99, "https://books.example.com/pro-aspnet",""),
        new EBook("JavaScript: The Good Parts",  "Douglas Crockford", 2008, 176,  19.99, "https://books.example.com/js-good-parts",""),
        new EBook("You Don't Know JS",           "Kyle Simpson",      2015, 278,  0.00,  "https://books.example.com/ydkjs",""),
        new EBook("Eloquent JavaScript",         "Marijn Haverbeke",  2018, 472,  25.00, "https://eloquentjavascript.net",""),
        new Book("Domain-Driven Design",          "Eric Evans",        2003, 560,  52.99,""),
        new Book("Patterns of Enterprise App",    "Martin Fowler",     2002, 533,  59.99,""),
        new Book("The Clean Coder",               "Robert C. Martin",  2011, 256,  32.00,""),
        new Book("Continuous Delivery",           "Jez Humble",        2010, 512,  44.99,""),
        new Book("Soft Skills",                   "John Sonmez",       2014, 504,  22.50,""),
         new Book("how i made a billionar dollar bussines with my frineds jz(tuff)",
         "Zura joxiani",       2026, 100000, 1, "packha parliament night blue"),
         new Book("Belal as champion",           "WelterWeight",        2026, 1,  0,""),
    }; 

        foreach (var book in books)
            this.books.Add(book);
    }
}
