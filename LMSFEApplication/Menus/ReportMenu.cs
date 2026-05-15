using LMSBLLLibrary.Interfaces;
using LMSFEApplication.Helpers;
using LMSModelLibrary.Models;

namespace LMSFEApplication.Menus
{
    public class ReportMenu
    {
        private readonly IReportService _reportService;
        private readonly IBookCategoryService _categoryService;

        public ReportMenu(IReportService reportService, IBookCategoryService categoryService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("REPORTS");
                Console.WriteLine("1. Currently Borrowed Books");
                Console.WriteLine("2. Overdue Books");
                Console.WriteLine("3. Members with Pending Fines");
                Console.WriteLine("4. Most Borrowed Books");
                Console.WriteLine("5. Available Books by Category");
                Console.WriteLine("6. Member Borrowing History");
                Console.WriteLine("7. Member Borrowing Summary");
                Console.WriteLine("0. Back to Main Menu");

                var choice = ConsoleHelper.ReadMenuChoice(0, 7);
                if (choice == 0) return;

                try
                {
                    switch (choice)
                    {
                        case 1: CurrentlyBorrowedBooks(); break;
                        case 2: OverdueBooks(); break;
                        case 3: MembersWithPendingFines(); break;
                        case 4: MostBorrowedBooks(); break;
                        case 5: AvailableBooksByCategory(); break;
                        case 6: MemberBorrowingHistory(); break;
                        case 7: MemberBorrowingSummary(); break;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }

                ConsoleHelper.Pause();
            }
        }

        private void CurrentlyBorrowedBooks()
        {
            Console.WriteLine();
            Console.WriteLine("Currently Borrowed Books");
            var borrowings = _reportService.GetCurrentlyBorrowedBooks();
            if (!borrowings.Any()) { Console.WriteLine("No books are currently borrowed."); return; }
            PrintBorrowingTable(borrowings);
        }

        private void OverdueBooks()
        {
            Console.WriteLine();
            Console.WriteLine("Overdue Books");
            var overdue = _reportService.GetOverdueBooks();
            if (!overdue.Any()) { Console.WriteLine("[OK] No overdue books."); return; }

            Console.WriteLine($"[!] {overdue.Count} overdue borrowing(s) found:");
            PrintBorrowingTable(overdue);
        }

        private void MembersWithPendingFines()
        {
            Console.WriteLine();
            Console.WriteLine("Members with Pending Fines");
            var members = _reportService.GetMembersWithPendingFines();
            if (!members.Any()) { Console.WriteLine("[OK] No members have pending fines."); return; }

            Console.WriteLine("ID | Name | Email | Membership Type");
            foreach (var m in members)
            {
                var typeName = m.MembershipType?.MembershipTypeName.ToString() ?? $"ID:{m.MembershipTypeId}";
                Console.WriteLine($"{m.MemberId} | {m.MemberName} | {m.MemberEmail} | {typeName}");
            }
            Console.WriteLine($"[!] Total: {members.Count} member(s) with pending fines.");
        }

        private void MostBorrowedBooks()
        {
            Console.WriteLine();
            Console.WriteLine("Most Borrowed Books");
            var topN = ConsoleHelper.ReadInt("Show top N books (e.g. 5 or 10)");
            var results = _reportService.GetMostBorrowedBooks(topN);
            if (!results.Any()) { Console.WriteLine("No borrowing data found."); return; }

            Console.WriteLine("Rank | Book ID | Title | Author | Times Borrowed");
            int rank = 1;
            foreach (var (book, count) in results)
                Console.WriteLine($"{rank++} | {book.BookId} | {book.BookTitle} | {book.BookAuthor} | {count}");
        }

        private void AvailableBooksByCategory()
        {
            Console.WriteLine();
            Console.WriteLine("Available Books by Category");
            var categories = _categoryService.GetAllCategories();
            if (!categories.Any()) { Console.WriteLine("[!] No categories found."); return; }

            Console.WriteLine("Categories:");
            foreach (var c in categories)
                Console.WriteLine($"{c.BookCategoryId}. {c.BookCategoryName}");

            var categoryId = ConsoleHelper.ReadInt("Category ID");
            var books = _reportService.GetAvailableBooksByCategory(categoryId);

            if (!books.Any()) { Console.WriteLine("[!] No available books in this category."); return; }

            Console.WriteLine("Book ID | Title | Author");
            foreach (var b in books)
                Console.WriteLine($"{b.BookId} | {b.BookTitle} | {b.BookAuthor}");
            Console.WriteLine($"Total: {books.Count} available book(s) in this category.");
        }

        private void MemberBorrowingHistory()
        {
            Console.WriteLine();
            Console.WriteLine("Member Borrowing History");
            var memberId = ConsoleHelper.ReadInt("Member ID");
            var history = _reportService.GetMemberBorrowingHistory(memberId);
            if (!history.Any()) { Console.WriteLine("No borrowing history found for this member."); return; }
            PrintBorrowingTable(history);
        }

        private void MemberBorrowingSummary()
        {
            Console.WriteLine();
            Console.WriteLine("Member Borrowing Summary");
            var memberId = ConsoleHelper.ReadInt("Member ID");
            var (active, returned, unpaidFine) = _reportService.GetMemberBorrowingSummary(memberId);

            Console.WriteLine($"Active Borrowings: {active}");
            Console.WriteLine($"Returned Books: {returned}");
            Console.WriteLine($"Total Books Borrowed: {active + returned}");

            if (unpaidFine > 0)
            {
                Console.WriteLine($"[!] Total Unpaid Fine: Rs.{unpaidFine:F2}");
                if (unpaidFine > 500) Console.WriteLine("[!] Borrowing BLOCKED — unpaid fine exceeds Rs.500.");
            }
            else
            {
                Console.WriteLine("[OK] Total Unpaid Fine: Rs.0.00 — No outstanding fines.");
            }
        }

        private static void PrintBorrowingTable(List<Borrowing> borrowings)
        {
            Console.WriteLine("Borrow ID | Member | Book Title | Borrow Date | Due Date | Status | Return Date");
            foreach (var b in borrowings)
            {
                var memberName = b.Member?.MemberName ?? $"ID:{b.MemberId}";
                var bookTitle = b.BookCopy?.Book?.BookTitle ?? $"Copy:{b.BookCopyId}";
                var returnDate = b.ReturnDate.HasValue ? b.ReturnDate.Value.ToString() : "-";
                Console.WriteLine($"{b.BorrowingId} | {memberName} | {bookTitle} | {b.BorrowDate} | {b.DueDate} | {b.BorrowingStatus} | {returnDate}");
            }
            Console.WriteLine($"Total: {borrowings.Count} record(s)");
        }
    }
}
