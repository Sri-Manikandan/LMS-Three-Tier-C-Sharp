using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IReturnService
    {
        Borrowing ReturnBook(int borrowingId);
    }
}
