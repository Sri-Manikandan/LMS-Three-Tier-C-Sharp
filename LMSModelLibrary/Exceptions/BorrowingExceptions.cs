namespace LMSModelLibrary.Exceptions
{
    public class BorrowingException : LibraryException
    {
        public BorrowingException(string message) : base(message) { }
    }

    public class BorrowingNotFoundException : BorrowingException
    {
        public BorrowingNotFoundException(int borrowingId)
            : base($"Borrowing record with ID {borrowingId} was not found.") { }
    }

    public class BorrowingLimitExceededException : BorrowingException
    {
        public BorrowingLimitExceededException(string membershipType, int maxLimit)
            : base($"Borrowing limit reached. '{membershipType}' membership allows a maximum of {maxLimit} active book(s).") { }
    }

    public class DuplicateBorrowingException : BorrowingException
    {
        public DuplicateBorrowingException(string bookTitle)
            : base($"Member already has an active borrowing for '{bookTitle}'. Return it before borrowing again.") { }
    }

    public class BorrowingAlreadyReturnedException : BorrowingException
    {
        public BorrowingAlreadyReturnedException(int borrowingId)
            : base($"Borrowing record {borrowingId} has already been returned.") { }
    }
}
