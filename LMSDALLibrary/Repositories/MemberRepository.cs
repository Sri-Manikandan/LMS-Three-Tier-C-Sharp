using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;

namespace LMSDALLibrary.Repositories
{
    public class MemberRepository : AbstractRepository<Member>, IMemberRepository
    {
        public Member? GetByEmail(string email)
        {
            return context.Members.FirstOrDefault(m => m.MemberEmail == email);
        }

        public Member? GetByPhone(string phone)
        {
            return context.Members.FirstOrDefault(m => m.MemberPhone == phone);
        }

        public void UpdateActiveStatus(int memberId, bool isActive)
        {
            var member = context.Members.Find(memberId);
            if (member != null)
            {
                member.IsActive = isActive;
                context.SaveChanges();
            }
        }

        public void UpdateMembershipType(int memberId, int membershipTypeId)
        {
            var member = context.Members.Find(memberId);
            if (member != null)
            {
                member.MembershipTypeId = membershipTypeId;
                context.SaveChanges();
            }
        }
    }
}
