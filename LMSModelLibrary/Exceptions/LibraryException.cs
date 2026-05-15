namespace LMSModelLibrary.Exceptions
{
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message) { }
        public LibraryException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidInputException : LibraryException
    {
        public InvalidInputException(string message) : base(message) { }
    }

    public class DatabaseException : LibraryException
    {
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner) : base(message, inner) { }
    }
}
