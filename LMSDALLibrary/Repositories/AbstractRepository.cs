using LMSDALLibrary.Contexts;
using LMSDALLibrary.Interfaces;

namespace LMSDALLibrary.Repositories
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : class
    {
        protected readonly LibraryDbContext context;

        protected AbstractRepository()
        {
            context = new LibraryDbContext();
        }

        public virtual T Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual T? GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public virtual T Update(T entity)
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
