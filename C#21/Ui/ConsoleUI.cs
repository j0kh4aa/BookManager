namespace BookManager.UI;

using BookManager.Interfaces;
using BookManager.Models;

public class ConsoleUI
{
    private readonly IBookRepository _repository;

    public ConsoleUI(IBookRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Starts the main application loop.</summary>
    public void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        bool running = true;
        while (running)
        {
            ShowMenu();
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1": AddBookUI(); break;
                case "2": ListAllBooksUI(); break;
                case "3": FindBookUI(); break;
                case "4": running = false; break;
                default:
                    Console.WriteLine("\n  [!] Invalid choice. Press any key...");
                    Console.ReadKey();
                    break;
            }
        }

        Console.Clear();
        Console.WriteLine("\n  Goodbye!\n");
    }

    /// <summary>Clears screen and draws the main menu.</summary>
    private void ShowMenu()
    {
        Console.Clear();
        PrintHeader("BOOK MANAGER");
        Console.WriteLine("  1.  Add a new book");
        Console.WriteLine("  2.  List all books");
        Console.WriteLine("  3.  Find a book by title");
        Console.WriteLine("  4.  Exit");
        PrintDivider();
        Console.Write("  Choose: ");
    }

    /// <summary>Clears screen and handles adding a new book with validation.</summary>
    private void AddBookUI()
    {
        Console.Clear();
        PrintHeader("ADD NEW BOOK");

        string title = PromptNonEmpty("  Title        : ");
        string author = PromptNonEmpty("  Author       : ");
        int year = PromptInt("  Year         : ", 1000, DateTime.Now.Year);
        int pages = PromptInt("  Pages        : ", 1, 99999);
        double price = PromptDouble("  Price ($)    : ", 0, 9999);

        Console.Write("  E-Book? (y/n): ");
        string isEbook = Console.ReadLine()?.Trim().ToLower() ?? "n";

        if (isEbook == "y")
        {
            string url = PromptNonEmpty("  Download URL : ");
            _repository.AddBook(new EBook(title, author, year, pages, price, url));
        }
        else
        {
            _repository.AddBook(new Book(title, author, year, pages, price));
        }

        Console.WriteLine("\n  [✓] Book added! Press any key to return...");
        Console.ReadKey();
    }

    /// <summary>Clears screen and lists all books with full details.</summary>
    private void ListAllBooksUI()
    {
        Console.Clear();
        PrintHeader("ALL BOOKS");

        var books = _repository.GetAllBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("  No books found.");
        }
        else
        {
            // Separate physical books and ebooks
            var physical = books.Where(b => b is not EBook).ToList();
            var ebooks = books.Where(b => b is EBook).ToList();

            if (physical.Count > 0)
            {
                Console.WriteLine("  📚 Physical Books:\n");
                for (int i = 0; i < physical.Count; i++)
                    PrintBookRow(i + 1, physical[i]);
            }

            if (ebooks.Count > 0)
            {
                Console.WriteLine("\n  💻 E-Books:\n");
                for (int i = 0; i < ebooks.Count; i++)
                    PrintBookRow(i + 1, ebooks[i]);
            }

            PrintDivider();
            Console.WriteLine($"  Total: {books.Count} books  |  Physical: {physical.Count}  |  E-Books: {ebooks.Count}");
        }

        Console.WriteLine("\n  Press any key to return...");
        Console.ReadKey();
    }

    /// <summary>Clears screen and searches a book by title.</summary>
    private void FindBookUI()
    {
        Console.Clear();
        PrintHeader("FIND BOOK BY TITLE");

        Console.Write("  Enter title: ");
        string title = Console.ReadLine()?.Trim() ?? "";

        Console.Clear();
        PrintHeader("SEARCH RESULT");

        var book = _repository.FindByTitle(title);

        if (book == null)
        {
            Console.WriteLine($"  [!] No book found with title \"{title}\".");
        }
        else
        {
            string type = book is EBook ? "💻 E-Book" : "📚 Physical";
            Console.WriteLine($"  {type}\n");
            Console.WriteLine($"  Title   : {book.Title}");
            Console.WriteLine($"  Author  : {book.Author}");
            Console.WriteLine($"  Year    : {book.Year}");
            Console.WriteLine($"  Pages   : {book.Pages}");
            Console.WriteLine($"  Price   : ${book.Price:F2}");
            if (book is EBook eb)
                Console.WriteLine($"  URL     : {eb.DownloadUrl}");
        }

        Console.WriteLine("\n  Press any key to return...");
        Console.ReadKey();
    }

    // ── UI Helpers ───────────────────────────────────────────────

    /// <summary>Prints a styled header with the given title.</summary>
    private void PrintHeader(string title)
    {
        Console.WriteLine("  ╔══════════════════════════════════════╗");
        Console.WriteLine($"  ║{title,-38}║");
        Console.WriteLine("  ╚══════════════════════════════════════╝\n");
    }

    /// <summary>Prints a horizontal divider line.</summary>
    private void PrintDivider()
    {
        Console.WriteLine("  ──────────────────────────────────────────");
    }

    /// <summary>Prints a single book row with index and details.</summary>
    private void PrintBookRow(int index, Book book)
    {
        Console.WriteLine($"  {index,2}. {book.Title,-35} {book.Author,-20} {book.Year}  {book.Pages}pg  ${book.Price:F2}");
    }

    // ── Validation helpers ───────────────────────────────────────

    /// <summary>Prompts until the user enters a non-empty string.</summary>
    private string PromptNonEmpty(string prompt)
    {
        string value;
        do
        {
            Console.Write(prompt);
            value = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(value))
                Console.WriteLine("  [!] This field cannot be empty.");
        }
        while (string.IsNullOrEmpty(value));
        return value;
    }

    /// <summary>Prompts until the user enters a valid integer within range.</summary>
    private int PromptInt(string prompt, int min, int max)
    {
        int result;
        do
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine()?.Trim(), out result) && result >= min && result <= max)
                break;
            Console.WriteLine($"  [!] Enter a number between {min} and {max}.");
        }
        while (true);
        return result;
    }

    /// <summary>Prompts until the user enters a valid decimal number within range.</summary>
    private double PromptDouble(string prompt, double min, double max)
    {
        double result;
        do
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine()?.Trim(), out result) && result >= min && result <= max)
                break;
            Console.WriteLine($"  [!] Enter a number between {min} and {max}.");
        }
        while (true);
        return result;
    }
}
