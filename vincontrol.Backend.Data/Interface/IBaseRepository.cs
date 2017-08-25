using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace vincontrol.Backend.Data.Interface
{
    public interface IBaseRepository<T> where T : class//, IBaseEntity
    {
        //T GetById(int id);
        IList<T> GetAll();
        IList<T> Query(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
    }
}
