namespace LMSDALLibrary.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T? GetById(int id);
        List<T> GetAll();
        T Update(T entity);
        void Delete(int id);
    }
}
