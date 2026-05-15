using LMSBLLLibrary.Interfaces;
using LMSFEApplication.Helpers;
using LMSModelLibrary.Models;
using LMSModelLibrary.Exceptions;

namespace LMSFEApplication.Menus
{
    public class BookMenu
    {
        private readonly IBookService _bookService;
        private readonly IBookCopyService _copyService;
        private readonly IBookCategoryService _categoryService;

        public BookMenu(IBookService bookService, IBookCopyService copyService, IBookCategoryService categoryService)
        {
            _bookService = bookService;
            _copyService = copyService;
            _categoryService = categoryService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("BOOK MANAGEMENT");
                Console.WriteLine("1. Add New Book");
                Console.WriteLine("2. Add Book Copy");
                Console.WriteLine("3. View All Books");
                Console.WriteLine("4. Search by Title");
                Console.WriteLine("5. Search by Author");
                Console.WriteLine("6. Search by Category");
                Console.WriteLine("7. View Available Copies of a Book");
                Console.WriteLine("8. Mark Copy as Damaged");
                Console.WriteLine("9. Mark Copy as Unavailable");
                Console.WriteLine("10. Add Book Category");
                Console.WriteLine("11. View All Categories");
                Console.WriteLine("0. Back to Main Menu");

                var choice = ConsoleHelper.ReadMenuChoice(0, 11);
                if (choice == 0) return;

                try
                {
                    switch (choice)
                    {
                        case 1:  AddBook(); break;
                        case 2:  AddBookCopy(); break;
                        case 3:  ViewAllBooks(); break;
                        case 4:  SearchByTitle(); break;
                        case 5:  SearchByAuthor(); break;
                        case 6:  SearchByCategory(); break;
                        case 7:  ViewAvailableCopies(); break;
                        case 8:  MarkDamaged(); break;
                        case 9:  MarkUnavailable(); break;
                        case 10: AddCategory(); break;
                        case 11: ViewAllCategories(); break;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }

                ConsoleHelper.Pause();
            }
        }

        private void AddBook()
        {
            Console.WriteLine();
            Console.WriteLine("Add New Book");
            var categories = _categoryService.GetAllCategories();
            if (!categories.Any())
            {
                Console.WriteLine("[!] No categories available. Please add a category first (option 10).");
                return;
            }

            Console.WriteLine("Available Categories:");
            foreach (var c in categories)
                Console.WriteLine($"{c.BookCategoryId}. {c.BookCategoryName}");

            var title = ConsoleHelper.ReadString("Book Title");
            var author = ConsoleHelper.ReadString("Author Name");
            var categoryId = ConsoleHelper.ReadInt("Category ID");

            var book = _bookService.AddBook(title, author, categoryId);
            Console.WriteLine($"[OK] Book added! Book ID: {book.BookId}");
        }

        private void AddBookCopy()
        {
            Console.WriteLine();
            Console.WriteLine("Add Book Copy");
            var bookId = ConsoleHelper.ReadInt("Book ID");
            var book = _bookService.GetById(bookId);
            if (book == null) { Console.WriteLine($"[ERROR] Book {bookId} not found."); return; }

            Console.WriteLine($"Adding copy for: {book.BookTitle} by {book.BookAuthor}");
            var copy = _copyService.AddBookCopy(bookId);
            Console.WriteLine($"[OK] Book copy added. Copy ID: {copy.BookCopyId}");
        }

        private void ViewAllBooks()
        {
            Console.WriteLine();
            Console.WriteLine("All Books");
            var books = _bookService.GetAllBooks();
            if (!books.Any()) { Console.WriteLine("[!] No books found."); return; }
            var categories = _categoryService.GetAllCategories().ToDictionary(c => c.BookCategoryId);
            PrintBookTable(books, categories);
        }

        private void SearchByTitle()
        {
            Console.WriteLine();
            Console.WriteLine("Search by Title");
            var title = ConsoleHelper.ReadString("Title (partial match)");
            var books = _bookService.SearchByTitle(title);
            if (!books.Any()) { Console.WriteLine("[!] No books found."); return; }
            var categories = _categoryService.GetAllCategories().ToDictionary(c => c.BookCategoryId);
            PrintBookTable(books, categories);
        }

        private void SearchByAuthor()
        {
            Console.WriteLine();
            Console.WriteLine("Search by Author");
            var author = ConsoleHelper.ReadString("Author (partial match)");
            var books = _bookService.SearchByAuthor(author);
            if (!books.Any()) { Console.WriteLine("[!] No books found."); return; }
            var categories = _categoryService.GetAllCategories().ToDictionary(c => c.BookCategoryId);
            PrintBookTable(books, categories);
        }

        private void SearchByCategory()
        {
            Console.WriteLine();
            Console.WriteLine("Search by Category Name");
            var categories = _categoryService.GetAllCategories();
            Console.WriteLine("Available Categories:");
            foreach (var c in categories)
                Console.WriteLine($"{c.BookCategoryId}. {c.BookCategoryName}");

            var categoryName = ConsoleHelper.ReadString("Category Name");
            var books = _bookService.SearchByCategory(categoryName);
            if (!books.Any()) { Console.WriteLine($"[!] No books found in category '{categoryName}'."); return; }
            var catDict = categories.ToDictionary(c => c.BookCategoryId);
            PrintBookTable(books, catDict);
        }

        private void ViewAvailableCopies()
        {
            Console.WriteLine();
            Console.WriteLine("Available Copies");
            var bookId = ConsoleHelper.ReadInt("Book ID");
            var book = _bookService.GetById(bookId);
            if (book == null) { Console.WriteLine($"[ERROR] Book {bookId} not found."); return; }

            Console.WriteLine($"Book: {book.BookTitle} by {book.BookAuthor}");

            var copies = _copyService.GetAvailableCopiesByBook(bookId);
            if (!copies.Any()) { Console.WriteLine("[!] No available copies for this book."); return; }

            Console.WriteLine("Copy ID | Status");
            foreach (var c in copies)
                Console.WriteLine($"{c.BookCopyId} | {c.CopyStatus}");
            Console.WriteLine($"Available: {copies.Count} copy(ies)");
        }

        private void MarkDamaged()
        {
            Console.WriteLine();
            Console.WriteLine("Mark Copy as Damaged");
            var copyId = ConsoleHelper.ReadInt("Book Copy ID");
            if (!ConsoleHelper.ReadBool($"Mark copy {copyId} as damaged?")) { Console.WriteLine("Cancelled."); return; }
            _copyService.MarkAsDamaged(copyId);
            Console.WriteLine($"[OK] Copy {copyId} marked as damaged.");
        }

        private void MarkUnavailable()
        {
            Console.WriteLine();
            Console.WriteLine("Mark Copy as Unavailable");
            var copyId = ConsoleHelper.ReadInt("Book Copy ID");
            if (!ConsoleHelper.ReadBool($"Mark copy {copyId} as unavailable?")) { Console.WriteLine("Cancelled."); return; }
            _copyService.MarkAsUnavailable(copyId);
            Console.WriteLine($"[OK] Copy {copyId} marked as unavailable.");
        }

        private void AddCategory()
        {
            Console.WriteLine();
            Console.WriteLine("Add Book Category");
            var name = ConsoleHelper.ReadString("Category Name");
            var category = _categoryService.AddCategory(name);
            Console.WriteLine($"[OK] Category added. ID: {category.BookCategoryId}, Name: {category.BookCategoryName}");
        }

        private void ViewAllCategories()
        {
            Console.WriteLine();
            Console.WriteLine("All Categories");
            var categories = _categoryService.GetAllCategories();
            if (!categories.Any()) { Console.WriteLine("[!] No categories found."); return; }

            Console.WriteLine("ID | Category Name");
            foreach (var c in categories)
                Console.WriteLine($"{c.BookCategoryId} | {c.BookCategoryName}");
            Console.WriteLine($"Total: {categories.Count} category(ies)");
        }

        private static void PrintBookTable(List<Book> books, Dictionary<int, BookCategory> categories)
        {
            Console.WriteLine("ID | Title | Author | Category");
            foreach (var b in books)
            {
                var catName = categories.TryGetValue(b.BookCategoryId, out var c) ? c.BookCategoryName : $"ID:{b.BookCategoryId}";
                Console.WriteLine($"{b.BookId} | {b.BookTitle} | {b.BookAuthor} | {catName}");
            }
            Console.WriteLine($"Total: {books.Count} book(s)");
        }
    }
}
