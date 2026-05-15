namespace LMSModelLibrary.Exceptions
{
    public class BookException : LibraryException
    {
        public BookException(string message) : base(message) { }
    }

    public class BookNotFoundException : BookException
    {
        public BookNotFoundException(int bookId)
            : base($"Book with ID {bookId} was not found.") { }
    }

    public class BookCopyNotFoundException : BookException
    {
        public BookCopyNotFoundException(int copyId)
            : base($"Book copy with ID {copyId} was not found.") { }
    }

    public class BookCopyNotAvailableException : BookException
    {
        public BookCopyNotAvailableException(int copyId, string status)
            : base($"Book copy {copyId} is not available for borrowing (current status: {status}).") { }
    }

    public class CategoryAlreadyExistsException : BookException
    {
        public CategoryAlreadyExistsException(string name)
            : base($"A book category named '{name}' already exists.") { }
    }
}
