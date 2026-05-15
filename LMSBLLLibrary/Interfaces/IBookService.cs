using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Interfaces
{
    public interface IBookService
    {
        Book AddBook(string title, string author, int categoryId);
        List<Book> GetAllBooks();
        Book? GetById(int bookId);
        List<Book> SearchByTitle(string title);
        List<Book> SearchByAuthor(string author);
        List<Book> SearchByCategory(string categoryName);
    }
}
