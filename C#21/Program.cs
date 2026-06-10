using BookManager.Services;
using BookManager.UI;

var service = new BookManagerService();

// ── Seed data: books ──────────────────────────────────────
service.SeedData();

var ui = new ConsoleUI(service);
ui.Run();