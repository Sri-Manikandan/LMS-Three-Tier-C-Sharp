using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Contexts;
using LMSModelLibrary.Exceptions;
using LMSModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSBLLLibrary.Services
{
    public class BorrowingService : IBorrowingService
    {
        public Borrowing BorrowBook(int memberId, int bookCopyId)
        {
            using var context = new LibraryDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var member = context.Members
                    .Include(m => m.MembershipType)
                    .FirstOrDefault(m => m.MemberId == memberId)
                    ?? throw new MemberNotFoundException(memberId);

                if (!member.IsActive)
                    throw new MemberInactiveException(member.MemberName);

                var unpaidFine = context.Database
                    .SqlQuery<decimal>($"SELECT calculate_member_fine({memberId}) AS \"Value\"")
                    .FirstOrDefault();

                if (unpaidFine > 500)
                    throw new FineLimitExceededException(unpaidFine);

                var activeBorrowingCount = context.Borrowings
                    .Count(b => b.MemberId == memberId && b.BorrowingStatus == BorrowingStatusEnum.active);

                if (activeBorrowingCount >= member.MembershipType.MaxActiveBorrowings)
                    throw new BorrowingLimitExceededException(
                        member.MembershipType.MembershipTypeName.ToString(),
                        member.MembershipType.MaxActiveBorrowings);

                var copy = context.BookCopies
                    .Include(c => c.Book)
                    .FirstOrDefault(c => c.BookCopyId == bookCopyId)
                    ?? throw new BookCopyNotFoundException(bookCopyId);

                if (copy.CopyStatus != CopyStatusEnum.available)
                    throw new BookCopyNotAvailableException(bookCopyId, copy.CopyStatus.ToString());

                bool alreadyBorrowed = context.Borrowings
                    .Any(b => b.MemberId == memberId
                           && b.BorrowingStatus == BorrowingStatusEnum.active
                           && b.BookCopy.BookId == copy.BookId);

                if (alreadyBorrowed)
                    throw new DuplicateBorrowingException(copy.Book.BookTitle);

                var today = DateOnly.FromDateTime(DateTime.Today);
                var borrowing = new Borrowing
                {
                    MemberId = memberId,
                    BookCopyId = bookCopyId,
                    BorrowDate = today,
                    DueDate = today.AddDays(member.MembershipType.MaxBorrowDays),
                    BorrowingStatus = BorrowingStatusEnum.active
                };

                context.Borrowings.Add(borrowing);

                copy.CopyStatus = CopyStatusEnum.borrowed;

                context.SaveChanges();
                transaction.Commit();

                return borrowing;
            }
            catch (LibraryException)
            {
                transaction.Rollback();
                throw;
            }
            catch (DbUpdateException ex)
            {
                transaction.Rollback();
                throw new DatabaseException("Borrow transaction failed due to a database error.", ex);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new LibraryException($"An unexpected error occurred during borrowing: {ex.Message}", ex);
            }
        }
    }
}
