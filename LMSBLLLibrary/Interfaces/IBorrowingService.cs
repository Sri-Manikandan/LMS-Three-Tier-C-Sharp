using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IBorrowingService
    {
        Borrowing BorrowBook(int memberId, int bookCopyId);
    }
}
