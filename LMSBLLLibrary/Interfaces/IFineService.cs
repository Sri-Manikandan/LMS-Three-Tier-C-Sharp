using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IFineService
    {
        List<Fine> GetPendingFines(int memberId);
        FinePayment PayFine(int fineId, decimal amount, int memberId);
        List<Fine> GetFineHistory(int memberId);
        decimal GetTotalUnpaidFine(int memberId);
    }
}
