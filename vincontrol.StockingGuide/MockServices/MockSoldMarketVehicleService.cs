using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;

namespace vincontrol.StockingGuide.MockServices
{
    public class MockSoldMarketVehicleService : ISoldMarketVehicleService
    {
        public int GetHistory(DateTime dateTime, int dealerId)
        {
            return 0;
        }

        public List<List<MakeHistory>> GetHistoryList(DateTime dateTime, int dealerId)
        {
            return new List<List<MakeHistory>>() { new List<MakeHistory>(), new List<MakeHistory>(), new List<MakeHistory>()};
        }

        public List<List<MakeHistory>> GetHistoryList(DateTime dateTime, double longitude, double lattitude)
        {
            return new List<List<MakeHistory>>() { new List<MakeHistory>(), new List<MakeHistory>(), new List<MakeHistory>() };

        }

        public List<MakeHistory> GetSoldVehicleHistoryListByMake(DateTime dateTime, double longitude, double lattitude)
        {
          return new List<MakeHistory>();
        }
    }
}
