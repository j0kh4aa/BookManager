namespace BookManager.Services;

using BookManager.Interfaces;
using BookManager.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Manages the book collection. Implements IBookRepository.
/// Demonstrates Encapsulation (private list) and Interface implementation.
/// </summary>
public class BookManagerService : IBookRepository
{
    private const string FilePath = "Data/books.json";

    private static readonly JsonSerializerOptions _opts = new()
    {
        WriteIndented = true,
        Converters = { new BookConverter() }
    };

    private readonly List<Book> books;

    public BookManagerService()
    {
        Directory.CreateDirectory("Data");
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            books = JsonSerializer.Deserialize<List<Book>>(json, _opts) ?? new();
        }
        else
        {
            books = new();
        }
    }

    private void Save() =>
        File.WriteAllText(FilePath, JsonSerializer.Serialize(books, _opts));

    /// <summary>
    /// Adds a new book to the collection.
    /// Validates that no duplicate title exists.
    /// </summary>
    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        bool exists = books.Any(b => b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase));
        if (exists)
        {
            Console.WriteLine($"  [!] A book with the title \"{book.Title}\" already exists.");
            return;
        }

        books.Add(book);
        Save();
        Console.WriteLine($"  [+] Added: {book}");
    }

    /// <summary>
    /// Returns all books in the collection.
    /// </summary>
    public List<Book> GetAllBooks() => books;

    /// <summary>
    /// Finds and returns a book by its title (case-insensitive).
    /// Returns null if not found.
    /// </summary>
    public Book? FindByTitle(string title) =>
        books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

    public Book? FindByAuthor(string author) =>
        books.FirstOrDefault(a => a.Author.Equals(author, StringComparison.OrdinalIgnoreCase));

    public List<Book> SortByPages() => books.OrderBy(b => b.Pages).ToList();
    public List<Book> SortByPrice() => books.OrderBy(b => b.Price).ToList();
    public List<Book> SortByYear() => books.OrderBy(b => b.Year).ToList();

    /// <summary>
    /// Seeds the collection with sample books if the file is empty.
    /// </summary>
    public void SeedData()
    {
        if (books.Count > 0) return;

        var seed = new List<Book>
        {
            new Book("Clean Code",                     "Robert C. Martin",   2008, 431,   35.99, ""),
            new Book("The Pragmatic Programmer",       "David Thomas",       1999, 352,   42.00, ""),
            new Book("Design Patterns",                "Gang of Four",       1994, 395,   54.99, ""),
            new Book("Refactoring",                    "Martin Fowler",      1999, 448,   45.00, ""),
            new Book("The Mythical Man-Month",         "Frederick Brooks",   1975, 322,   28.50, ""),
            new Book("Code Complete",                  "Steve McConnell",    2004, 960,   49.99, ""),
            new Book("Introduction to Algorithms",     "Thomas Cormen",      2009, 1292,  89.99, ""),
            new Book("Structure and Interpretation",   "Harold Abelson",     1996, 657,   55.00, ""),
            new Book("The Art of Computer Programming","Donald Knuth",       1968, 672,  120.00, ""),
            new Book("Working Effectively with Legacy Code", "Michael Feathers", 2004, 456, 38.00, ""),
            new Book("me bebia iliko da ilarioni",     "nodar dumbadze",     2016, 232,   40.00, ""),
            new EBook("C# in Depth",                  "Jon Skeet",          2019, 528,   29.99, "https://books.example.com/csharp-in-depth", ""),
            new EBook("Pro ASP.NET Core",             "Adam Freeman",       2022, 1248,  39.99, "https://books.example.com/pro-aspnet", ""),
            new EBook("JavaScript: The Good Parts",   "Douglas Crockford",  2008, 176,   19.99, "https://books.example.com/js-good-parts", ""),
            new EBook("You Don't Know JS",            "Kyle Simpson",       2015, 278,    0.00, "https://books.example.com/ydkjs", ""),
            new EBook("Eloquent JavaScript",          "Marijn Haverbeke",   2018, 472,   25.00, "https://eloquentjavascript.net", ""),
            new Book("Domain-Driven Design",           "Eric Evans",         2003, 560,   52.99, ""),
            new Book("Patterns of Enterprise App",     "Martin Fowler",      2002, 533,   59.99, ""),
            new Book("The Clean Coder",                "Robert C. Martin",   2011, 256,   32.00, ""),
            new Book("Continuous Delivery",            "Jez Humble",         2010, 512,   44.99, ""),
            new Book("Soft Skills",                    "John Sonmez",        2014, 504,   22.50, ""),
            new Book("how i made a billionar dollar bussines with my frineds jz(tuff)", "Zura joxiani", 2026, 100000, 1, "packha parliament night blue"),
            new Book("Belal as champion",              "WelterWeight",       2026, 1,      0,    ""),
        };

        foreach (var book in seed)
            books.Add(book);

        Save();
    }
}

/// <summary>
/// Custom JSON converter to correctly deserialize Book vs EBook based on DownloadUrl field.
/// </summary>
public class BookConverter : JsonConverter<Book>
{
    public override Book? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (root.TryGetProperty("DownloadUrl", out var urlProp) && urlProp.GetString() != "")
            return JsonSerializer.Deserialize<EBook>(root.GetRawText());

        return JsonSerializer.Deserialize<Book>(root.GetRawText());
    }

    public override void Write(Utf8JsonWriter writer, Book value, JsonSerializerOptions options)
    {
        if (value is EBook ebook)
            JsonSerializer.Serialize(writer, ebook);
        else
            JsonSerializer.Serialize(writer, value);
    }
}