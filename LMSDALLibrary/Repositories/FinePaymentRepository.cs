using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;

namespace LMSDALLibrary.Repositories
{
    public class FinePaymentRepository : AbstractRepository<FinePayment>, IFinePaymentRepository
    {
        public List<FinePayment> GetByMemberId(int memberId)
        {
            return context.FinePayments
                .Where(fp => fp.MemberId == memberId)
                .ToList();
        }
    }
}
