using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;
using LMSModelLibrary.Exceptions;
using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Services
{
    public class BookCopyService : IBookCopyService
    {
        private readonly IBookCopyRepository _copyRepo;
        private readonly IBookRepository _bookRepo;

        public BookCopyService()
        {
            _copyRepo = new BookCopyRepository();
            _bookRepo = new BookRepository();
        }

        public BookCopy AddBookCopy(int bookId)
        {
            if (bookId <= 0)
            {
                throw new InvalidInputException("A valid book ID is required.");
            }

            var book = _bookRepo.GetById(bookId);
            if (book == null)
            {
                throw new BookNotFoundException(bookId);
            }

            var copy = new BookCopy{
                BookId = bookId,
                CopyStatus = CopyStatusEnum.available
            };

            return _copyRepo.Add(copy);
        }

        public void MarkAsDamaged(int copyId)
        {
            var copy = _copyRepo.GetById(copyId);
            if (copy == null)
            {
                throw new BookCopyNotFoundException(copyId);
            }

            if (copy.CopyStatus == CopyStatusEnum.borrowed)
            {
                throw new BookCopyNotAvailableException(copyId, copy.CopyStatus.ToString());
            }

            _copyRepo.UpdateCopyStatus(copyId, CopyStatusEnum.damaged);
        }

        public void MarkAsUnavailable(int copyId)
        {
            var copy = _copyRepo.GetById(copyId);
            if (copy == null)
            {
                throw new BookCopyNotFoundException(copyId);
            }

            if (copy.CopyStatus == CopyStatusEnum.borrowed)
            {
                throw new BookCopyNotAvailableException(copyId, copy.CopyStatus.ToString());
            }

            _copyRepo.UpdateCopyStatus(copyId, CopyStatusEnum.unavailable);
        }

        public List<BookCopy> GetAvailableCopiesByBook(int bookId)
        {
            if (bookId <= 0)
            {
                throw new InvalidInputException("A valid book ID is required.");
            }
            return _copyRepo.GetAvailableCopiesByBookId(bookId);
        }

        public List<BookCopy> GetAllAvailableCopies()
        {
            return _copyRepo.GetAvailableCopies();
        }
    }
}
