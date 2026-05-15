namespace LMSModelLibrary.Exceptions
{
    public class FineException : LibraryException
    {
        public FineException(string message) : base(message) { }
    }

    public class FineLimitExceededException : FineException
    {
        public FineLimitExceededException(decimal unpaidAmount)
            : base($"Borrowing blocked. Member has Rs.{unpaidAmount:F2} in unpaid fines (limit: Rs.500.00). Please clear the outstanding fines first.") { }
    }

    public class FineNotFoundException : FineException
    {
        public FineNotFoundException(int fineId)
            : base($"Fine with ID {fineId} was not found.") { }
    }

    public class FineAlreadyPaidException : FineException
    {
        public FineAlreadyPaidException(int fineId)
            : base($"Fine {fineId} has already been fully paid.") { }
    }

    public class FineWaivedException : FineException
    {
        public FineWaivedException(int fineId)
            : base($"Fine {fineId} has been waived and requires no payment.") { }
    }
}
