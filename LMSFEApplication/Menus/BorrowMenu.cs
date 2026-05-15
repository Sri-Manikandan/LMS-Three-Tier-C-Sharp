using LMSBLLLibrary.Interfaces;
using LMSFEApplication.Helpers;
using LMSModelLibrary.Exceptions;

namespace LMSFEApplication.Menus
{
    public class BorrowMenu
    {
        private readonly IBorrowingService _borrowingService;
        private readonly IMemberService _memberService;
        private readonly IBookService _bookService;
        private readonly IBookCopyService _copyService;

        public BorrowMenu(IBorrowingService borrowingService, IMemberService memberService,
            IBookService bookService, IBookCopyService copyService)
        {
            _borrowingService = borrowingService;
            _memberService = memberService;
            _bookService = bookService;
            _copyService = copyService;
        }

        public void Run()
        {
            Console.WriteLine();
            Console.WriteLine("BORROW BOOK");

            try
            {
                var memberId = ConsoleHelper.ReadInt("Member ID");
                var member = _memberService.GetById(memberId);
                if (member == null) { Console.WriteLine("[ERROR] Member not found."); ConsoleHelper.Pause(); return; }
                if (!member.IsActive) { Console.WriteLine($"[ERROR] Member '{member.MemberName}' is inactive."); ConsoleHelper.Pause(); return; }

                Console.WriteLine($"Member: {member.MemberName} | Type ID: {member.MembershipTypeId} | Status: Active");

                int bookCopyId;
                var searchByTitle = ConsoleHelper.ReadBool("Search book by title first?");

                if (searchByTitle)
                {
                    var title = ConsoleHelper.ReadString("Book Title (partial match)");
                    var books = _bookService.SearchByTitle(title);
                    if (!books.Any()) { Console.WriteLine("[!] No books matched that title."); ConsoleHelper.Pause(); return; }

                    Console.WriteLine("Book ID | Title | Author");
                    foreach (var b in books)
                        Console.WriteLine($"{b.BookId} | {b.BookTitle} | {b.BookAuthor}");

                    var bookId = ConsoleHelper.ReadInt("Book ID");
                    var copies = _copyService.GetAvailableCopiesByBook(bookId);

                    if (!copies.Any())
                    {
                        Console.WriteLine("[!] No available copies for this book right now.");
                        ConsoleHelper.Pause();
                        return;
                    }

                    Console.WriteLine("Available Copies:");
                    Console.WriteLine("Copy ID | Status");
                    foreach (var c in copies)
                        Console.WriteLine($"{c.BookCopyId} | {c.CopyStatus}");

                    bookCopyId = ConsoleHelper.ReadInt("Book Copy ID to borrow");
                }
                else
                {
                    bookCopyId = ConsoleHelper.ReadInt("Book Copy ID");
                }

                if (!ConsoleHelper.ReadBool($"Confirm borrow — Member ID {memberId}, Copy ID {bookCopyId}?"))
                {
                    Console.WriteLine("[!] Borrow cancelled.");
                    ConsoleHelper.Pause();
                    return;
                }

                var borrowing = _borrowingService.BorrowBook(memberId, bookCopyId);

                Console.WriteLine("[OK] Book borrowed successfully!");
                Console.WriteLine($"Borrowing ID: {borrowing.BorrowingId}");
                Console.WriteLine($"Borrow Date: {borrowing.BorrowDate:dd MMM yyyy}");
                Console.WriteLine($"Due Date: {borrowing.DueDate:dd MMM yyyy}");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            ConsoleHelper.Pause();
        }
    }
}
