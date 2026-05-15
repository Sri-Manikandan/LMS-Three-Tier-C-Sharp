using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Contexts;
using LMSModelLibrary.Exceptions;
using LMSModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSBLLLibrary.Services
{
    public class ReturnService : IReturnService
    {
        private const decimal FinePerDay = 10m;

        public Borrowing ReturnBook(int borrowingId)
        {
            using var context = new LibraryDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var borrowing = context.Borrowings
                    .Include(b => b.BookCopy)
                    .Include(b => b.Fine)
                    .FirstOrDefault(b => b.BorrowingId == borrowingId)
                    ?? throw new BorrowingNotFoundException(borrowingId);

                if (borrowing.BorrowingStatus == BorrowingStatusEnum.returned)
                    throw new BorrowingAlreadyReturnedException(borrowingId);

                var today = DateOnly.FromDateTime(DateTime.Today);
                borrowing.ReturnDate = today;
                borrowing.BorrowingStatus = BorrowingStatusEnum.returned;

                if (today > borrowing.DueDate)
                {
                    int delayedDays = today.DayNumber - borrowing.DueDate.DayNumber;
                    decimal fineAmount = delayedDays * FinePerDay;

                    if (borrowing.Fine == null)
                    {
                        context.Fines.Add(new Fine
                        {
                            BorrowingId = borrowingId,
                            FineAmount = fineAmount,
                            FinePaidStatus = FineStatusEnum.unpaid
                        });
                    }
                    else
                    {
                        borrowing.Fine.FineAmount = fineAmount;
                        borrowing.Fine.FinePaidStatus = FineStatusEnum.unpaid;
                    }
                }

                borrowing.BookCopy.CopyStatus = CopyStatusEnum.available;

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
                throw new DatabaseException("Return transaction failed due to a database error.", ex);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new LibraryException($"An unexpected error occurred during return: {ex.Message}", ex);
            }
        }
    }
}
