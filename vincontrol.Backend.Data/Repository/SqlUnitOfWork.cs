using System;
using vincontrol.Backend.Data.Interface;
using vincontrol.Backend.Data.Repository.Implementation;
using vincontrol.Backend.Data.Repository.Interface;

namespace vincontrol.Backend.Data.Repository
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        private const string VinControlWareHouseConnectionString = "whitmanenterprisewarehouseEntities";

        #region Private Varaiables

        private readonly vincontrolwarehouseEntities _vinControlWareHouseContext;
        private IImportProfileRepository _importProfile;

        #endregion

        public SqlUnitOfWork()
        {
            _vinControlWareHouseContext = new vincontrolwarehouseEntities(); //new ObjectContext(ConfigurationManager.ConnectionStrings[VinControlWareHouseConnectionString].ConnectionString);
            _vinControlWareHouseContext.ContextOptions.LazyLoadingEnabled = true;
            _vinControlWareHouseContext.CommandTimeout = 3 * 60; // in seconds
        }

        #region IUnitOfWork Members

        public IImportProfileRepository ImportProfile
        {
            get { return _importProfile ?? (_importProfile = new ImportProfileRepository(_vinControlWareHouseContext)); }
        }

        public void Commit()
        {
            _vinControlWareHouseContext.SaveChanges();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_vinControlWareHouseContext != null)
            {
                _vinControlWareHouseContext.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
