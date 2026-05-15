using LMSModelLibrary.Exceptions;

namespace LMSFEApplication.Helpers
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception ex)
        {
            switch (ex)
            {
                case MemberNotFoundException e:        Console.WriteLine($"[ERROR] Member Not Found: {e.Message}"); break;
                case MemberInactiveException e:        Console.WriteLine($"[!] Inactive Member: {e.Message}"); break;
                case DuplicateMemberException e:       Console.WriteLine($"[!] Duplicate Member: {e.Message}"); break;
                case BookNotFoundException e:          Console.WriteLine($"[ERROR] Book Not Found: {e.Message}"); break;
                case BookCopyNotFoundException e:      Console.WriteLine($"[ERROR] Copy Not Found: {e.Message}"); break;
                case BookCopyNotAvailableException e:  Console.WriteLine($"[!] Copy Not Available: {e.Message}"); break;
                case CategoryAlreadyExistsException e: Console.WriteLine($"[!] Duplicate Category: {e.Message}"); break;
                case BorrowingNotFoundException e:     Console.WriteLine($"[ERROR] Borrowing Not Found: {e.Message}"); break;
                case BorrowingLimitExceededException e:Console.WriteLine($"[!] Borrowing Limit Reached: {e.Message}"); break;
                case DuplicateBorrowingException e:    Console.WriteLine($"[!] Already Borrowed: {e.Message}"); break;
                case BorrowingAlreadyReturnedException e: Console.WriteLine($"[!] Already Returned: {e.Message}"); break;
                case FineLimitExceededException e:     Console.WriteLine($"[!] Fine Limit Exceeded: {e.Message}"); break;
                case FineNotFoundException e:          Console.WriteLine($"[ERROR] Fine Not Found: {e.Message}"); break;
                case FineAlreadyPaidException e:       Console.WriteLine($"[!] Fine Already Paid: {e.Message}"); break;
                case FineWaivedException e:            Console.WriteLine($"[!] Fine Waived: {e.Message}"); break;
                case InvalidInputException e:          Console.WriteLine($"[ERROR] Invalid Input: {e.Message}"); break;
                case DatabaseException e:              Console.WriteLine($"[ERROR] Database Error: {e.Message}"); break;
                case LibraryException e:               Console.WriteLine($"[ERROR] Library Error: {e.Message}"); break;
                default:                               Console.WriteLine($"[ERROR] Unexpected Error: {ex.Message}"); break;
            }
        }
    }
}
