using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        Member? GetByEmail(string email);
        Member? GetByPhone(string phone);
        void UpdateActiveStatus(int memberId, bool isActive);
        void UpdateMembershipType(int memberId, int membershipTypeId);
    }
}
