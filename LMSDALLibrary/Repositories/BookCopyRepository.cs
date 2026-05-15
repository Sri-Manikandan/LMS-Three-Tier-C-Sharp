using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;

namespace LMSDALLibrary.Repositories
{
    public class BookCopyRepository : AbstractRepository<BookCopy>, IBookCopyRepository
    {
        public List<BookCopy> GetAvailableCopies()
        {
            return context.BookCopies
                .Where(b => b.CopyStatus == CopyStatusEnum.available)
                .ToList();
        }

        public List<BookCopy> GetAvailableCopiesByBookId(int bookId)
        {
            return context.BookCopies
                .Where(b => b.BookId == bookId && b.CopyStatus == CopyStatusEnum.available)
                .ToList();
        }

        public void UpdateCopyStatus(int copyId, CopyStatusEnum status)
        {
            var copy = context.BookCopies.Find(copyId);
            if (copy != null)
            {
                copy.CopyStatus = status;
                context.SaveChanges();
            }
        }
    }
}
