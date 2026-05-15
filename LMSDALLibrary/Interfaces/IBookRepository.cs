using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        List<Book> GetByTitle(string title);
        List<Book> GetByAuthor(string author);
        List<Book> GetByCategoryName(string categoryName);
    }
}
