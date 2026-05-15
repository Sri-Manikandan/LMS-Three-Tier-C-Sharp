using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Contexts;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;
using LMSModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSBLLLibrary.Services
{
    public class ReportService : IReportService
    {
        private readonly IBorrowingRepository _borrowingRepo;
        private readonly IBookCategoryRepository _categoryRepo;

        public ReportService()
        {
            _borrowingRepo = new BorrowingRepository();
            _categoryRepo = new BookCategoryRepository();
        }

        public List<Borrowing> GetCurrentlyBorrowedBooks()
        {
            using var context = new LibraryDbContext();
            return context.Borrowings
                .Where(b => b.BorrowingStatus == BorrowingStatusEnum.active)
                .Include(b => b.Member)
                .Include(b => b.BookCopy).ThenInclude(bc => bc.Book)
                .OrderBy(b => b.DueDate)
                .ToList();
        }

        public List<Borrowing> GetOverdueBooks() => _borrowingRepo.GetOverdueBorrowings();

        public List<Member> GetMembersWithPendingFines()
        {
            using var context = new LibraryDbContext();
            return context.Members
                .Where(m => context.Fines.Any(f =>
                    f.Borrowing.MemberId == m.MemberId &&
                    f.FinePaidStatus == FineStatusEnum.unpaid))
                .Include(m => m.MembershipType)
                .OrderBy(m => m.MemberName)
                .ToList();
        }

        public List<(Book Book, int BorrowCount)> GetMostBorrowedBooks(int topN = 10)
        {
            using var context = new LibraryDbContext();
            var grouped = context.Borrowings
                .GroupBy(b => b.BookCopy.BookId)
                .Select(g => new { BookId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(topN)
                .ToList();

            return grouped
                .Select(r => (Book: context.Books.Find(r.BookId)!, BorrowCount: r.Count))
                .Where(r => r.Book != null)
                .ToList();
        }

        public List<Book> GetAvailableBooksByCategory(int categoryId) =>
            _categoryRepo.GetAvailableBooksByCategory(categoryId);

        public List<Borrowing> GetMemberBorrowingHistory(int memberId)
        {
            using var context = new LibraryDbContext();
            return context.Borrowings
                .Where(b => b.MemberId == memberId)
                .Include(b => b.BookCopy).ThenInclude(bc => bc.Book)
                .Include(b => b.Fine)
                .OrderByDescending(b => b.BorrowDate)
                .ToList();
        }

        public (int ActiveBorrowings, int ReturnedBorrowings, decimal TotalUnpaidFine) GetMemberBorrowingSummary(int memberId)
        {
            return _borrowingRepo.GetMemberBorrowingSummary(memberId);
        }
    }
}
