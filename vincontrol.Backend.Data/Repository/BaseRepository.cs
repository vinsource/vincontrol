using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using vincontrol.Backend.Data.Interface;

namespace vincontrol.Backend.Data.Repository
{
    public abstract class BaseRepository//<T> where T : class
    {
        protected vincontrolwarehouseEntities Context;

        //protected ObjectSet<T> ObjectSet;

        //protected BaseRepository(whitmanenterprisewarehouseEntities context)
        //{
        //    Context = context;
        //}

        //protected BaseRepository(ObjectContext context)
        //{
        //    ObjectSet = context.CreateObjectSet<T>();
        //}

        #region IBaseRepository<T> Members

        //public T GetById(int id)
        //{
        //    return ObjectSet.Single(i => i.Id == id);
        //}

        //public IList<T> GetAll()
        //{
        //    return ObjectSet.ToList();
        //}

        //public IList<T> Query(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        //{
        //    return ObjectSet.Where(filter).ToList();
        //}

        //public void Add(T entity)
        //{
        //    ObjectSet.AddObject(entity);
        //}

        //public void Remove(T entity)
        //{
        //    ObjectSet.DeleteObject(entity);
        //}

        #endregion
    }
}
