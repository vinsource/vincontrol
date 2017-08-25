using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace EmployeeData.Custom {
    public class SqlRepository<T> : IRepository<T>
                                    where T : class {

        public SqlRepository(ObjectContext context) {
            _objectSet = context.CreateObjectSet<T>();

        }

       public IQueryable<T> Find(Expression<Func<T, bool>> predicate) {
            return _objectSet.Where(predicate);
        }

        public void Add(T newEntity) {
            _objectSet.AddObject(newEntity);
        }

        public void Remove(T entity) {
            _objectSet.DeleteObject(entity);
        }
    
        public IQueryable<T> FindAll()
        {
            return _objectSet;
        }

        protected ObjectSet<T> _objectSet;
    }
}