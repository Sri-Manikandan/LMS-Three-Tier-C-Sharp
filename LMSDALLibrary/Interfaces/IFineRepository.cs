using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IFineRepository : IRepository<Fine>
    {
        Fine? GetByBorrowingId(int borrowingId);
        List<Fine> GetPendingFinesByMember(int memberId);
        decimal GetTotalUnpaidFine(int memberId);
        List<Fine> GetFineHistoryByMember(int memberId);
    }
}
