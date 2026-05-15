using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Repositories
{
    public class FineRepository : AbstractRepository<Fine>, IFineRepository
    {
        public Fine? GetByBorrowingId(int borrowingId)
        {
            return context.Fines
                .FirstOrDefault(f => f.BorrowingId == borrowingId);
        }

        public List<Fine> GetPendingFinesByMember(int memberId)
        {
            return context.Fines
                .Where(f => f.Borrowing.MemberId == memberId
                         && f.FinePaidStatus == FineStatusEnum.unpaid)
                .Include(f => f.Borrowing)
                .ToList();
        }

        public decimal GetTotalUnpaidFine(int memberId)
        {
            return context.Database
                .SqlQuery<decimal>($"SELECT calculate_member_fine({memberId})")
                .FirstOrDefault();
        }

        public List<Fine> GetFineHistoryByMember(int memberId)
        {
            return context.Fines
                .Where(f => f.Borrowing.MemberId == memberId)
                .Include(f => f.Borrowing)
                .Include(f => f.FinePayments)
                .ToList();
        }
    }
}
