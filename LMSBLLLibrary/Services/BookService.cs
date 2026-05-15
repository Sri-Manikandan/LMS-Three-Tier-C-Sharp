using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;
using LMSModelLibrary.Exceptions;
using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;

        public BookService()
        {
            _bookRepo = new BookRepository();
        }

        public Book AddBook(string title, string author, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidInputException("Book title is required.");
            }
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new InvalidInputException("Author name is required.");
            }
            if (categoryId <= 0)
            {
                throw new InvalidInputException("A valid category ID is required.");
            }

            var book = new Book
            {
                BookTitle = title,
                BookAuthor = author,
                BookCategoryId = categoryId
            };

            return _bookRepo.Add(book);
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepo.GetAll();
        }

        public Book? GetById(int bookId)
        {
            if (bookId <= 0)
            {
                throw new InvalidInputException("A valid book ID is required.");
            }
            return _bookRepo.GetById(bookId);
        }

        public List<Book> SearchByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidInputException("Title search term is required.");
            }
            return _bookRepo.GetByTitle(title);
        }

        public List<Book> SearchByAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new InvalidInputException("Author search term is required.");
            }
            return _bookRepo.GetByAuthor(author);
        }

        public List<Book> SearchByCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new InvalidInputException("Category name is required.");
            }
            return _bookRepo.GetByCategoryName(categoryName);
        }
    }
}
