using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IReportService
    {
        List<Borrowing> GetCurrentlyBorrowedBooks();
        List<Borrowing> GetOverdueBooks();
        List<Member> GetMembersWithPendingFines();
        List<(Book Book, int BorrowCount)> GetMostBorrowedBooks(int topN = 10);
        List<Book> GetAvailableBooksByCategory(int categoryId);
        List<Borrowing> GetMemberBorrowingHistory(int memberId);
        (int ActiveBorrowings, int ReturnedBorrowings, decimal TotalUnpaidFine) GetMemberBorrowingSummary(int memberId);
    }
}
