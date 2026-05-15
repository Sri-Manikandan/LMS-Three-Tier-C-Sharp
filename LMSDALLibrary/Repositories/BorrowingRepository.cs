using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Repositories
{
    public class BorrowingRepository : AbstractRepository<Borrowing>, IBorrowingRepository
    {
        public List<Borrowing> GetByMemberId(int memberId)
        {
            return context.Borrowings
                .Where(b => b.MemberId == memberId)
                .ToList();
        }

        public List<Borrowing> GetActiveBorrowingsByMember(int memberId)
        {
            return context.Borrowings
                .Where(b => b.MemberId == memberId && b.BorrowingStatus == BorrowingStatusEnum.active)
                .ToList();
        }

        public List<Borrowing> GetOverdueBorrowings()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return context.Borrowings
                .Where(b => b.BorrowingStatus == BorrowingStatusEnum.active && b.DueDate < today)
                .Include(b => b.Member)
                .Include(b => b.BookCopy)
                .ToList();
        }

        public Borrowing? GetActiveBorrowingByMemberAndBook(int memberId, int bookId)
        {
            return context.Borrowings
                .Where(b => b.MemberId == memberId
                         && b.BorrowingStatus == BorrowingStatusEnum.active
                         && b.BookCopy.BookId == bookId)
                .FirstOrDefault();
        }

        public (int ActiveBorrowings, int ReturnedBorrowings, decimal TotalUnpaidFine) GetMemberBorrowingSummary(int memberId)
        {
            var active = context.Borrowings
                .Count(b => b.MemberId == memberId && b.BorrowingStatus == BorrowingStatusEnum.active);

            var returned = context.Borrowings
                .Count(b => b.MemberId == memberId && b.BorrowingStatus == BorrowingStatusEnum.returned);

            var unpaidFine = context.Database
                .SqlQuery<decimal>($"SELECT calculate_member_fine({memberId})")
                .FirstOrDefault();

            return (active, returned, unpaidFine);
        }
    }
}
