using BicycleCompany.DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll() => repositoryContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => 
            repositoryContext.Set<T>().Where(expression).AsNoTracking();

        public Task CreateAsync(T entity)
        {
            repositoryContext.Set<T>().Add(entity);
            return repositoryContext.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            repositoryContext.Set<T>().Update(entity);
            return repositoryContext.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity) {
            repositoryContext.Set<T>().Remove(entity);
            return repositoryContext.SaveChangesAsync();
        }
    }
}
