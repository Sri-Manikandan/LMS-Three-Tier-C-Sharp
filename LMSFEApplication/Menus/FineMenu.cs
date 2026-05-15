using LMSBLLLibrary.Interfaces;
using LMSFEApplication.Helpers;
using LMSModelLibrary.Models;

namespace LMSFEApplication.Menus
{
    public class FineMenu
    {
        private readonly IFineService _fineService;
        private readonly IMemberService _memberService;

        public FineMenu(IFineService fineService, IMemberService memberService)
        {
            _fineService = fineService;
            _memberService = memberService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("FINE MANAGEMENT");
                Console.WriteLine("1. View Pending Fines");
                Console.WriteLine("2. Pay Fine");
                Console.WriteLine("3. View Fine History");
                Console.WriteLine("4. Get Total Unpaid Fine");
                Console.WriteLine("0. Back to Main Menu");

                var choice = ConsoleHelper.ReadMenuChoice(0, 4);
                if (choice == 0) return;

                try
                {
                    switch (choice)
                    {
                        case 1: ViewPendingFines(); break;
                        case 2: PayFine(); break;
                        case 3: ViewFineHistory(); break;
                        case 4: GetTotalUnpaidFine(); break;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }

                ConsoleHelper.Pause();
            }
        }

        private void ViewPendingFines()
        {
            Console.WriteLine();
            Console.WriteLine("Pending Fines");
            var memberId = ConsoleHelper.ReadInt("Member ID");
            var member = _memberService.GetById(memberId);
            if (member == null) { Console.WriteLine("[ERROR] Member not found."); return; }
            Console.WriteLine($"Member: {member.MemberName}");

            var fines = _fineService.GetPendingFines(memberId);
            if (!fines.Any()) { Console.WriteLine("[OK] No pending fines for this member."); return; }

            PrintFineTable(fines);

            var total = fines.Sum(f => f.FineAmount);
            Console.WriteLine($"[!] Total Pending: Rs.{total:F2}");
            if (total > 500) Console.WriteLine("[!] Borrowing is BLOCKED — unpaid fine exceeds Rs.500.");
        }

        private void PayFine()
        {
            Console.WriteLine();
            Console.WriteLine("Pay Fine");
            var memberId = ConsoleHelper.ReadInt("Member ID");
            var member = _memberService.GetById(memberId);
            if (member == null) { Console.WriteLine("[ERROR] Member not found."); return; }
            Console.WriteLine($"Member: {member.MemberName}");

            var fines = _fineService.GetPendingFines(memberId);
            if (!fines.Any()) { Console.WriteLine("[OK] No pending fines for this member."); return; }

            PrintFineTable(fines);

            var fineId = ConsoleHelper.ReadInt("Fine ID to pay");
            var selectedFine = fines.FirstOrDefault(f => f.FineId == fineId);
            if (selectedFine != null)
                Console.WriteLine($"Fine amount: Rs.{selectedFine.FineAmount:F2}");

            var amount = ConsoleHelper.ReadDecimal("Amount to pay (Rs.)");
            var payment = _fineService.PayFine(fineId, amount, memberId);

            Console.WriteLine("[OK] Payment recorded!");
            Console.WriteLine($"Payment ID: {payment.PaymentId}");
            Console.WriteLine($"Amount Paid: Rs.{payment.FinePaidAmount:F2}");
            Console.WriteLine($"Paid Date: {payment.PaidDate:dd MMM yyyy HH:mm}");
        }

        private void ViewFineHistory()
        {
            Console.WriteLine();
            Console.WriteLine("Fine History");
            var memberId = ConsoleHelper.ReadInt("Member ID");
            var member = _memberService.GetById(memberId);
            if (member == null) { Console.WriteLine("[ERROR] Member not found."); return; }
            Console.WriteLine($"Member: {member.MemberName}");

            var fines = _fineService.GetFineHistory(memberId);
            if (!fines.Any()) { Console.WriteLine("No fine records found for this member."); return; }

            PrintFineTable(fines);
        }

        private void GetTotalUnpaidFine()
        {
            Console.WriteLine();
            Console.WriteLine("Total Unpaid Fine");
            var memberId = ConsoleHelper.ReadInt("Member ID");
            var member = _memberService.GetById(memberId);
            if (member == null) { Console.WriteLine("[ERROR] Member not found."); return; }
            Console.WriteLine($"Member: {member.MemberName}");

            var total = _fineService.GetTotalUnpaidFine(memberId);
            if (total > 0)
            {
                Console.WriteLine($"[!] Total unpaid fine: Rs.{total:F2}");
                if (total > 500) Console.WriteLine("[!] Borrowing BLOCKED — unpaid fine exceeds Rs.500.");
            }
            else
            {
                Console.WriteLine("[OK] No unpaid fines. Member is clear to borrow.");
            }
        }

        private static void PrintFineTable(List<Fine> fines)
        {
            Console.WriteLine("Fine ID | Borrowing ID | Amount | Status");
            foreach (var f in fines)
                Console.WriteLine($"{f.FineId} | {f.BorrowingId} | Rs.{f.FineAmount:F2} | {f.FinePaidStatus}");
        }
    }
}
