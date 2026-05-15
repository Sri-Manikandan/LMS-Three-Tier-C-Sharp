using LMSBLLLibrary.Interfaces;
using LMSFEApplication.Helpers;
using LMSModelLibrary.Models;
using LMSModelLibrary.Exceptions;

namespace LMSFEApplication.Menus
{
    public class ReturnMenu
    {
        private readonly IReturnService _returnService;
        private readonly IReportService _reportService;
        private readonly IMemberService _memberService;

        public ReturnMenu(IReturnService returnService, IReportService reportService, IMemberService memberService)
        {
            _returnService = returnService;
            _reportService = reportService;
            _memberService = memberService;
        }

        public void Run()
        {
            Console.WriteLine();
            Console.WriteLine("RETURN BOOK");

            try
            {
                var memberId = ConsoleHelper.ReadInt("Member ID");
                var member = _memberService.GetById(memberId);
                if (member == null) { Console.WriteLine("[ERROR] Member not found."); ConsoleHelper.Pause(); return; }

                Console.WriteLine($"Member: {member.MemberName}");

                var history = _reportService.GetMemberBorrowingHistory(memberId);
                var active = history.Where(b => b.BorrowingStatus == BorrowingStatusEnum.active).ToList();

                if (!active.Any())
                {
                    Console.WriteLine("[!] This member has no active borrowings.");
                    ConsoleHelper.Pause();
                    return;
                }

                var today = DateOnly.FromDateTime(DateTime.Today);

                Console.WriteLine("Active Borrowings:");
                Console.WriteLine("Borrow ID | Book Title | Copy ID | Borrow Date | Due Date | Overdue?");
                foreach (var b in active)
                {
                    var bookTitle = b.BookCopy?.Book?.BookTitle ?? $"Copy:{b.BookCopyId}";
                    var isOverdue = today > b.DueDate;
                    var overdueTxt = isOverdue ? $"YES (+{today.DayNumber - b.DueDate.DayNumber}d)" : "No";
                    Console.WriteLine($"{b.BorrowingId} | {bookTitle} | {b.BookCopyId} | {b.BorrowDate} | {b.DueDate} | {overdueTxt}");
                }

                var borrowingId = ConsoleHelper.ReadInt("Borrowing ID to return");

                if (!ConsoleHelper.ReadBool($"Confirm return of borrowing {borrowingId}?"))
                {
                    Console.WriteLine("[!] Return cancelled.");
                    ConsoleHelper.Pause();
                    return;
                }

                var returned = _returnService.ReturnBook(borrowingId);

                Console.WriteLine("[OK] Book returned successfully!");
                Console.WriteLine($"Return Date: {returned.ReturnDate:dd MMM yyyy}");

                if (today > returned.DueDate)
                {
                    int days = today.DayNumber - returned.DueDate.DayNumber;
                    decimal fine = returned.Fine?.FineAmount ?? 0m;
                    Console.WriteLine($"[!] Book returned {days} day(s) late. Fine charged: Rs.{fine:F2}");
                    Console.WriteLine("[!] Please visit Fine Management to pay the outstanding fine.");
                }
                else
                {
                    Console.WriteLine("No fine. Book returned on time.");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            ConsoleHelper.Pause();
        }
    }
}
