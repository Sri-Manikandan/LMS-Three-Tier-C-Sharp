using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IBookCategoryService
    {
        BookCategory AddCategory(string name);
        List<BookCategory> GetAllCategories();
        BookCategory? GetByName(string name);
        List<Book> GetAvailableBooksByCategory(int categoryId);
    }
}
