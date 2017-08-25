using System;
using System.Linq;
using System.Linq.Expressions;

namespace vincontrol.StockingGuide.Repository.Interfaces
{
    public interface IRepository<T> 
                    where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T newEntity);
        void Remove(T entity);
    }
}