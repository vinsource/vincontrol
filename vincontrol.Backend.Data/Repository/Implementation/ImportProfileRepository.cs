using System;
using System.Collections.Generic;
using vincontrol.Backend.Data.Repository.Interface;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.Data.Repository.Implementation
{
    class ImportProfileRepository : BaseRepository, IImportProfileRepository
    {
        public ImportProfileRepository() : this(new vincontrolwarehouseEntities()) { }

        public ImportProfileRepository(vincontrolwarehouseEntities context)
        {
            Context = context;
        }

        #region Implementation of IImportProfileRepository

        public IEnumerable<ImportProfile> GetAll()
        {
            return new List<ImportProfile>{new ImportProfile(){Name = "Duy Vo"}};
        }

        #endregion
    }
}
