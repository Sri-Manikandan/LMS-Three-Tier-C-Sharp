using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IBookCopyRepository : IRepository<BookCopy>
    {
        List<BookCopy> GetAvailableCopies();
        List<BookCopy> GetAvailableCopiesByBookId(int bookId);
        void UpdateCopyStatus(int copyId, CopyStatusEnum status);
    }
}
