using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;
using LMSModelLibrary.Exceptions;
using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepo;

        public MemberService()
        {
            _memberRepo = new MemberRepository();
        }

        public Member AddMember(string name, string email, string phone, int membershipTypeId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidInputException("Member name is required.");
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidInputException("Email address is required.");
            if (string.IsNullOrWhiteSpace(phone))
                throw new InvalidInputException("Phone number is required.");
            if (membershipTypeId <= 0)
                throw new InvalidInputException("A valid membership type must be selected.");

            if (_memberRepo.GetByEmail(email) != null)
                throw new DuplicateMemberException("email", email);

            if (_memberRepo.GetByPhone(phone) != null)
                throw new DuplicateMemberException("phone", phone);

            var member = new Member
            {
                MemberName = name,
                MemberEmail = email,
                MemberPhone = phone,
                MembershipTypeId = membershipTypeId,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            return _memberRepo.Add(member);
        }

        public List<Member> GetAllMembers() => _memberRepo.GetAll();

        public Member? GetById(int memberId) => _memberRepo.GetById(memberId);

        public Member? SearchByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidInputException("Email address is required for search.");
            return _memberRepo.GetByEmail(email);
        }

        public Member? SearchByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new InvalidInputException("Phone number is required for search.");
            return _memberRepo.GetByPhone(phone);
        }

        public void UpdateMembershipStatus(int memberId, bool isActive)
        {
            if (_memberRepo.GetById(memberId) == null)
                throw new MemberNotFoundException(memberId);
            _memberRepo.UpdateActiveStatus(memberId, isActive);
        }

        public void DeactivateMember(int memberId) => UpdateMembershipStatus(memberId, false);

        public void UpdateMembershipType(int memberId, int membershipTypeId)
        {
            if (membershipTypeId <= 0)
                throw new InvalidInputException("A valid membership type ID is required.");

            if (_memberRepo.GetById(memberId) == null)
                throw new MemberNotFoundException(memberId);
            _memberRepo.UpdateMembershipType(memberId, membershipTypeId);
        }
    }
}
