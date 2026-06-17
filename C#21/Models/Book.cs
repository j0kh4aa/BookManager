namespace BookManager.Models;

public class Book
{
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Year { get; set; }
    public int Pages { get; set; }
    public double Price { get; set; }
    public string Cigar { get; set; } = "";

    public Book() { }

    public Book(string title, string author, int year, int pages, double price, string cigar)
    {
        Title = title;
        Author = author;
        Year = year;
        Pages = pages;
        Price = price;
        Cigar = cigar;
    }

    /// <summary>
    /// Returns a formatted string with full book details.
    /// </summary>
    public virtual string GetDetails()
    {
        return $"Title: {Title} | Author: {Author} | Year: {Year} | Pages: {Pages} | Price: ${Price:F2} {Cigar}  ";
    }

    /// <summary>
    /// Returns a short summary of the book (title and author only).
    /// </summary>
    public override string ToString()
    {
        return $"\"{Title}\" by {Author}";
    }
}