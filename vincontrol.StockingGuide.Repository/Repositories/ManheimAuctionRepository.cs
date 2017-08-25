using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using vincontrol.Data.Repository.Interface;
using EmployeeData.Custom;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Repository.Interfaces;

namespace vincontrol.StockingGuide.Repository.Repositories
{
    public class ManheimAuctionRepository: SqlRepository<manheim_auctions>, IManheimAuctionRepository
    {
        public ManheimAuctionRepository(ObjectContext context) : base(context)
        {
        }
    }
}
