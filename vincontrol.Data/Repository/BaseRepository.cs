using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using vincontrol.Data.Interface;

namespace vincontrol.Data.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class//, IBaseEntity
    {
        protected ObjectSet<T> ObjectSet;

        public BaseRepository(ObjectContext context)
        {
            ObjectSet = context.CreateObjectSet<T>();
        }

        #region IBaseRepository<T> Members

        //public T GetById(int id)
        //{
        //    return ObjectSet.Single(i => i.Id == id);
        //}

        public IList<T> GetAll()
        {
            return ObjectSet.ToList();
        }

        public IList<T> Query(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            return ObjectSet.Where(filter).ToList();
        }

        public void Add(T entity)
        {
            ObjectSet.AddObject(entity);
        }

        public void Remove(T entity)
        {
            ObjectSet.DeleteObject(entity);
        }

        #endregion
    }
}
