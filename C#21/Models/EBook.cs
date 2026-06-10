namespace BookManager.Models;

/// <summary>
/// EBook inherits from Book and adds a download URL.
/// Demonstrates Inheritance and Polymorphism (overrides GetDetails).
/// </summary>
public class EBook : Book
{
    public string DownloadUrl { get; set; }

    public EBook(string title, string author, int year, int pages, double price, string downloadUrl)
        : base(title, author, year, pages, price)
    {
        DownloadUrl = downloadUrl;
    }

    /// <summary>
    /// Overrides base GetDetails to include the download URL (Polymorphism).
    /// </summary>
    public override string GetDetails()
    {
        return base.GetDetails() + $" | [E-Book] URL: {DownloadUrl}";
    }
}
