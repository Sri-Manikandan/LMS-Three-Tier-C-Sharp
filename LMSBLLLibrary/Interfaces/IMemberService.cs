using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IMemberService
    {
        Member AddMember(string name, string email, string phone, int membershipTypeId);
        List<Member> GetAllMembers();
        Member? GetById(int memberId);
        Member? SearchByEmail(string email);
        Member? SearchByPhone(string phone);
        void UpdateMembershipStatus(int memberId, bool isActive);
        void DeactivateMember(int memberId);
        void UpdateMembershipType(int memberId, int membershipTypeId);
    }
}
