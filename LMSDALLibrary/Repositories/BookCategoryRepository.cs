using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;

namespace LMSDALLibrary.Repositories
{
    public class BookCategoryRepository : AbstractRepository<BookCategory>, IBookCategoryRepository
    {
        public BookCategory? GetByName(string name)
        {
            return context.BookCategories
                .FirstOrDefault(c => c.BookCategoryName == name);
        }

        public List<Book> GetAvailableBooksByCategory(int categoryId)
        {
            return context.Books
                .Where(b => b.BookCategoryId == categoryId
                         && b.BookCopies.Any(c => c.CopyStatus == CopyStatusEnum.available))
                .ToList();
        }
    }
}
