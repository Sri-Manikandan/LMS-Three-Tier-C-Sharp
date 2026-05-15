using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSFEApplication.Helpers;
using LMSModelLibrary.Models;
using LMSModelLibrary.Exceptions;

namespace LMSFEApplication.Menus
{
    public class MemberMenu
    {
        private readonly IMemberService _memberService;
        private readonly IMembershipTypeRepository _typeRepo;

        public MemberMenu(IMemberService memberService, IMembershipTypeRepository typeRepo)
        {
            _memberService = memberService;
            _typeRepo = typeRepo;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("MEMBER MANAGEMENT");
                Console.WriteLine("1. Add New Member");
                Console.WriteLine("2. View All Members");
                Console.WriteLine("3. Search by Email");
                Console.WriteLine("4. Search by Phone");
                Console.WriteLine("5. Update Membership Status");
                Console.WriteLine("6. Deactivate Member");
                Console.WriteLine("7. Update Membership Type");
                Console.WriteLine("0. Back to Main Menu");

                var choice = ConsoleHelper.ReadMenuChoice(0, 7);
                if (choice == 0) return;

                try
                {
                    switch (choice)
                    {
                        case 1: AddMember(); break;
                        case 2: ViewAllMembers(); break;
                        case 3: SearchByEmail(); break;
                        case 4: SearchByPhone(); break;
                        case 5: UpdateStatus(); break;
                        case 6: DeactivateMember(); break;
                        case 7: UpdateMembershipType(); break;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }

                ConsoleHelper.Pause();
            }
        }

        private void AddMember()
        {
            Console.WriteLine();
            Console.WriteLine("Add New Member");
            var types = _typeRepo.GetAll();
            Console.WriteLine("Available Membership Types:");
            foreach (var t in types)
                Console.WriteLine($"{t.MembershipTypeId}. {t.MembershipTypeName} | Max {t.MaxActiveBorrowings} books | {t.MaxBorrowDays} days borrow period");

            var name = ConsoleHelper.ReadString("Member Name");
            var email = ConsoleHelper.ReadString("Email");
            var phone = ConsoleHelper.ReadString("Phone");
            var typeId = ConsoleHelper.ReadInt("Membership Type ID");

            var member = _memberService.AddMember(name, email, phone, typeId);
            Console.WriteLine($"[OK] Member added! Member ID: {member.MemberId}");
        }

        private void ViewAllMembers()
        {
            Console.WriteLine();
            Console.WriteLine("All Members");
            var members = _memberService.GetAllMembers();
            var types = _typeRepo.GetAll().ToDictionary(t => t.MembershipTypeId);
            if (!members.Any()) { Console.WriteLine("[!] No members registered yet."); return; }
            PrintMemberTable(members, types);
        }

        private void SearchByEmail()
        {
            Console.WriteLine();
            Console.WriteLine("Search by Email");
            var email = ConsoleHelper.ReadString("Email");
            var member = _memberService.SearchByEmail(email);
            if (member == null) { Console.WriteLine("[!] No member found with that email."); return; }
            var types = _typeRepo.GetAll().ToDictionary(t => t.MembershipTypeId);
            PrintMemberTable(new List<Member> { member }, types);
        }

        private void SearchByPhone()
        {
            Console.WriteLine();
            Console.WriteLine("Search by Phone");
            var phone = ConsoleHelper.ReadString("Phone");
            var member = _memberService.SearchByPhone(phone);
            if (member == null) { Console.WriteLine("[!] No member found with that phone."); return; }
            var types = _typeRepo.GetAll().ToDictionary(t => t.MembershipTypeId);
            PrintMemberTable(new List<Member> { member }, types);
        }

        private void UpdateStatus()
        {
            Console.WriteLine();
            Console.WriteLine("Update Membership Status");
            var id = ConsoleHelper.ReadInt("Member ID");
            var member = _memberService.GetById(id);
            if (member == null) { Console.WriteLine("[ERROR] Member not found."); return; }

            Console.WriteLine($"Member: {member.MemberName}");
            Console.WriteLine($"Current Status: {(member.IsActive ? "Active" : "Inactive")}");

            var active = ConsoleHelper.ReadBool("Set as Active?");
            _memberService.UpdateMembershipStatus(id, active);
            Console.WriteLine($"[OK] Status updated to {(active ? "Active" : "Inactive")}.");
        }

        private void DeactivateMember()
        {
            Console.WriteLine();
            Console.WriteLine("Deactivate Member");
            var id = ConsoleHelper.ReadInt("Member ID");
            var member = _memberService.GetById(id);
            if (member == null) { Console.WriteLine("[ERROR] Member not found."); return; }
            if (!member.IsActive) { Console.WriteLine($"[!] '{member.MemberName}' is already inactive."); return; }

            if (!ConsoleHelper.ReadBool($"Deactivate '{member.MemberName}'?")) { Console.WriteLine("Cancelled."); return; }
            _memberService.DeactivateMember(id);
            Console.WriteLine("[OK] Member deactivated successfully.");
        }

        private void UpdateMembershipType()
        {
            Console.WriteLine();
            Console.WriteLine("Update Membership Type");
            var id = ConsoleHelper.ReadInt("Member ID");
            var member = _memberService.GetById(id);
            if (member == null) { Console.WriteLine("[ERROR] Member not found."); return; }

            Console.WriteLine($"Member: {member.MemberName}");

            var types = _typeRepo.GetAll();
            Console.WriteLine("Available Membership Types:");
            foreach (var t in types)
                Console.WriteLine($"{t.MembershipTypeId}. {t.MembershipTypeName} | Max {t.MaxActiveBorrowings} books | {t.MaxBorrowDays} days");

            var typeId = ConsoleHelper.ReadInt("New Membership Type ID");
            _memberService.UpdateMembershipType(id, typeId);
            Console.WriteLine("[OK] Membership type updated successfully.");
        }

        private static void PrintMemberTable(List<Member> members, Dictionary<int, MembershipType> types)
        {
            Console.WriteLine("ID | Name | Email | Phone | Type | Status");
            foreach (var m in members)
            {
                var typeName = types.TryGetValue(m.MembershipTypeId, out var t) ? t.MembershipTypeName.ToString() : $"ID:{m.MembershipTypeId}";
                Console.WriteLine($"{m.MemberId} | {m.MemberName} | {m.MemberEmail} | {m.MemberPhone} | {typeName} | {(m.IsActive ? "Active" : "Inactive")}");
            }
            Console.WriteLine($"Total: {members.Count} member(s)");
        }
    }
}
