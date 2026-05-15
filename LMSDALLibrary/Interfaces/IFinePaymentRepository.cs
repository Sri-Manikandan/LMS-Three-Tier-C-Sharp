using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IFinePaymentRepository : IRepository<FinePayment>
    {
        List<FinePayment> GetByMemberId(int memberId);
    }
}
