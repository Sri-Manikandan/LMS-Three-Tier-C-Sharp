using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;
using LMSModelLibrary.Exceptions;
using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Services
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IBookCategoryRepository _categoryRepo;

        public BookCategoryService()
        {
            _categoryRepo = new BookCategoryRepository();
        }

        public BookCategory AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidInputException("Category name is required.");

            if (_categoryRepo.GetByName(name) != null)
                throw new CategoryAlreadyExistsException(name);

            return _categoryRepo.Add(new BookCategory { BookCategoryName = name });
        }

        public List<BookCategory> GetAllCategories()
        {
            return _categoryRepo.GetAll();
        }

        public BookCategory? GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidInputException("Category name is required.");
            }
            return _categoryRepo.GetByName(name);
        }

        public List<Book> GetAvailableBooksByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new InvalidInputException("A valid category ID is required.");
            }
            return _categoryRepo.GetAvailableBooksByCategory(categoryId);
        }
    }
}
