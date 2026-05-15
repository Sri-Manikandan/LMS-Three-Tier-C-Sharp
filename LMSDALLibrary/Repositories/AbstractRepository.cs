using LMSDALLibrary.Contexts;
using LMSDALLibrary.Interfaces;
using LMSModelLibrary.Exceptions;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(
                    $"Failed to save {typeof(T).Name}. {ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        public virtual T? GetById(int id)
        {
            try
            {
                return context.Set<T>().Find(id);
            }
            catch (Exception ex) when (ex is not LibraryException)
            {
                throw new DatabaseException($"Failed to retrieve {typeof(T).Name} with ID {id}.", ex);
            }
        }

        public virtual List<T> GetAll()
        {
            try
            {
                return context.Set<T>().ToList();
            }
            catch (Exception ex) when (ex is not LibraryException)
            {
                throw new DatabaseException($"Failed to retrieve {typeof(T).Name} list.", ex);
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                context.Set<T>().Update(entity);
                context.SaveChanges();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(
                    $"Failed to update {typeof(T).Name}. {ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        public virtual void Delete(int id)
        {
            try
            {
                var entity = GetById(id);
                if (entity != null)
                {
                    context.Set<T>().Remove(entity);
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException(
                    $"Failed to delete {typeof(T).Name} with ID {id}. {ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }
    }
}
