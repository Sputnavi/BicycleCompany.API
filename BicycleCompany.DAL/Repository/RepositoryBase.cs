using BicycleCompany.DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BicycleCompany.DAL.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }


        public IQueryable<T> FindAll(bool trackChanges) => 
            trackChanges ?
                repositoryContext.Set<T>() :
                repositoryContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => 
            trackChanges ?
                repositoryContext.Set<T>().Where(expression) :
                repositoryContext.Set<T>().Where(expression).AsNoTracking();

        public void Create(T entity)
        {
            repositoryContext.Set<T>().Add(entity);
            repositoryContext.SaveChanges();
        }

        public void Update(T entity)
        {
            repositoryContext.Set<T>().Update(entity);
            repositoryContext.SaveChanges();
        }

        public void Delete(T entity) {
            repositoryContext.Set<T>().Remove(entity);
            repositoryContext.SaveChanges();
        }
    }
}
