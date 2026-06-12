namespace BookManager.Interfaces;

using BookManager.Models;

/// <summary>
/// Defines the contract for any book storage/management class.
/// Demonstrates Interface usage.
/// </summary>
public interface IBookRepository
{
    /// <summary>Adds a new book to the list.</summary>
    void AddBook(Book book);

    /// <summary>Returns all books in the list.</summary>
    List<Book> GetAllBooks();

    /// <summary>Finds a book by its title (case-insensitive).</summary>
    Book? FindByTitle(string title);

    Book? FindByAuthor(string author);


    List<Book> SortByPages();

    List<Book> SortByPrice();

    List<Book> SortByYear();
}
