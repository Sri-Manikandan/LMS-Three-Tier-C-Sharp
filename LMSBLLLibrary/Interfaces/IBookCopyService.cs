using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IBookCopyService
    {
        BookCopy AddBookCopy(int bookId);
        void MarkAsDamaged(int copyId);
        void MarkAsUnavailable(int copyId);
        List<BookCopy> GetAvailableCopiesByBook(int bookId);
        List<BookCopy> GetAllAvailableCopies();
    }
}
