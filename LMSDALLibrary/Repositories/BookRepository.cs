using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Repositories
{
    public class BookRepository : AbstractRepository<Book>, IBookRepository
    {
        public List<Book> GetByTitle(string title)
        {
            return context.Books
                .Where(b => b.BookTitle.Contains(title))
                .ToList();
        }

        public List<Book> GetByAuthor(string author)
        {
            return context.Books
                .Where(b => b.BookAuthor.Contains(author))
                .ToList();
        }

        public List<Book> GetByCategoryName(string categoryName)
        {
            return context.BookCategories
                .Where(c => c.BookCategoryName == categoryName)
                .Join(context.Books,
                    c => c.BookCategoryId,
                    b => b.BookCategoryId,
                    (c, b) => b)
                .ToList();
        }
    }
}
