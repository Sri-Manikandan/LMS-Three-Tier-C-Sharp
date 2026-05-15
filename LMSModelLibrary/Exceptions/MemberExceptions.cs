namespace LMSModelLibrary.Exceptions
{
    public class MemberException : LibraryException
    {
        public MemberException(string message) : base(message) { }
    }

    public class MemberNotFoundException : MemberException
    {
        public MemberNotFoundException(int memberId)
            : base($"Member with ID {memberId} was not found.") { }

        public MemberNotFoundException(string identifier)
            : base($"No member found with identifier '{identifier}'.") { }
    }

    public class MemberInactiveException : MemberException
    {
        public MemberInactiveException(string memberName)
            : base($"Member '{memberName}' is inactive and cannot perform this action.") { }
    }

    public class DuplicateMemberException : MemberException
    {
        public DuplicateMemberException(string field, string value)
            : base($"A member with {field} '{value}' already exists.") { }
    }
}
