namespace BookManager.Models;

public class Book
{
    // Properties
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public int Pages { get; set; }
    public double Price { get; set; }

    // Constructor
    public Book(string title, string author, int year, int pages, double price)
    {
        Title = title;
        Author = author;
        Year = year;
        Pages = pages;
        Price = price;
    }

    /// <summary>
    /// Returns a formatted string with full book details.
    /// </summary>
    public virtual string GetDetails()
    {
        return $"Title: {Title} | Author: {Author} | Year: {Year} | Pages: {Pages} | Price: ${Price:F2}";
    }

    /// <summary>
    /// Returns a short summary of the book (title and author only).
    /// </summary>
    public override string ToString()
    {
        return $"\"{Title}\" by {Author}";
    }
}
