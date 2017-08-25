using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data.Interface;
using vincontrol.Backend.Model;

namespace vincontrol.Backend.Data.Repository.Interface
{
    public interface IImportProfileRepository
    {
        IEnumerable<ImportProfile> GetAll();
    }
}
