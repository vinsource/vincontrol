using vincontrol.Backend.Data.Interface;

namespace vincontrol.Backend.Data.Repository
{
    public class Repository<TDataEntity> : IRepository<TDataEntity> where TDataEntity : class
    {
        //private whitmanenterprisewarehouseEntities _context;

        //public Repository()
        //{
        //    _context = new whitmanenterprisewarehouseEntities();
        //}

        //public Repository(whitmanenterprisewarehouseEntities context)
        //{
        //    _context = context;
        //}
        
        //#region IRepository<TDataEntity> Members

        //IEnumerable<TDataEntity> IRepository<TDataEntity>.GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerable<TDataEntity> IRepository<TDataEntity>.GetBy(System.Linq.Expressions.Expression<Func<TDataEntity, bool>> expression)
        //{
        //    throw new NotImplementedException();
        //}

        //void IRepository<TDataEntity>.InsertOnSubmit(TDataEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //void IRepository<TDataEntity>.DeleteOnSubmit(TDataEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //void IRepository<TDataEntity>.Submit()
        //{
        //    throw new NotImplementedException();
        //}

        //void IRepository<TDataEntity>.Merge<TKey>(Func<TDataEntity, bool> originalGetBy, IEnumerable<TDataEntity> current, Func<TDataEntity, TKey> keySelecter, bool update)
        //{
        //    throw new NotImplementedException();
        //}

        //void IRepository<TDataEntity>.Merge<TKey>(Func<TDataEntity, bool> originalGetBy, IEnumerable<TDataEntity> current, Func<TDataEntity, TKey> keySelecter)
        //{
        //    throw new NotImplementedException();
        //}

        //void IRepository<TDataEntity>.DeleteAllOnSubmit(IEnumerable<TDataEntity> entities)
        //{
        //    throw new NotImplementedException();
        //}

        //void IRepository<TDataEntity>.DeleteBy(System.Linq.Expressions.Expression<Func<TDataEntity, bool>> expression)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion
    }
}
