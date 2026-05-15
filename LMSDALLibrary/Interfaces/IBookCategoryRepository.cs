using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IBookCategoryRepository : IRepository<BookCategory>
    {
        BookCategory? GetByName(string name);
        List<Book> GetAvailableBooksByCategory(int categoryId);
    }
}
