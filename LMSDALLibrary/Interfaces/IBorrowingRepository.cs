using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IBorrowingRepository : IRepository<Borrowing>
    {
        List<Borrowing> GetByMemberId(int memberId);
        List<Borrowing> GetActiveBorrowingsByMember(int memberId);
        List<Borrowing> GetOverdueBorrowings();
        Borrowing? GetActiveBorrowingByMemberAndBook(int memberId, int bookId);
        (int ActiveBorrowings, int ReturnedBorrowings, decimal TotalUnpaidFine) GetMemberBorrowingSummary(int memberId);
    }
}
