using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vincontrol.Helper;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Entity.Custom;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Services;

namespace vincontrol.StockingGuide.UnitTest
{
    [TestClass]
    public class SoldMarketVehicleServiceTests
    {
        private ISoldMarketVehicleService _soldMarketVehicleService;
        private IInventoryService _inventoryService;

        [TestInitialize]
        public void TestInit()
        {
            _soldMarketVehicleService = new SoldMarketVehicleService();
            _inventoryService = new InventoryService();
          
        }

        [TestMethod]
        public void soldmarketservice_get_history_have_data()
        {
            // var result =_soldMarketVehicleService.GetHistory(new DateTime(2013,12,2),3636);
            //Assert.IsTrue(result>0);
        }

         [TestMethod]
        public void soldmarketservice_get_historylist_have_data()
        {
            // var result =_soldMarketVehicleService.GetHistoryList(new DateTime(2013,12,2),3636);
            //Assert.IsTrue(result[0].Count>0);
        }

        [TestMethod]
         public void soldmarketservice_get_historylist_have_data_for_astonmartin_db9()
        {
        //    var result = _soldMarketVehicleService.GetHistoryList(DateTime.Now);
        //    Assert.IsTrue(result[0].FirstOrDefault(i => i.Make=="Aston Martin"&& i.Model=="DB9")!=null||
        //        result[1].FirstOrDefault(i => i.Make == "Aston Martin" && i.Model == "DB9") != null ||
        //        result[2].FirstOrDefault(i => i.Make == "Aston Martin" && i.Model == "DB9") != null);
        }

        [TestMethod]
        public void soldmarketservice_get_historylist_have_historynot0_for_astonmartin_db9()
        {
            //var result = _soldMarketVehicleService.GetHistoryList(DateTime.Now);
            //var item1 = result[0].FirstOrDefault(i => i.Make == "Aston Martin" && i.Model == "DB9");
            //var item2 = result[1].FirstOrDefault(i => i.Make == "Aston Martin" && i.Model == "DB9");
            //var item3 = result[2].FirstOrDefault(i => i.Make == "Aston Martin" && i.Model == "DB9");
            //Assert.IsTrue(((item1 == null ? 0 : item1.History) > 0) || ((item2 == null ? 0 : item2.History) > 0) || ((item3 == null ? 0 : item3.History) > 0));
            //var averageResult = (int)
            //    Math.Ceiling(GetMarketHistory(result[0], "Aston Martin", "DB9") +
            //                 GetMarketHistory(result[1], "Aston Martin", "DB9") +
            //                 GetMarketHistory(result[2], "Aston Martin", "DB9")/3);
            //Assert.IsTrue( averageResult> 0);    
        }

        [TestMethod]
        public void get_weeklyturnover_havevalue()
        {
            //var history = _soldMarketVehicleService.GetHistory(DateTime.Now, 1541);
            //var stock = _inventoryService.GetCurrentMonthUsedStock(1541);
            ////_logEngine.Info("UpdateWeeklyTurnOverByMonthProcess: end _soldMarketVehicleService.GetHistory(dateTime, dealerId) ");


            //var turnover = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock, history);
            //Assert.IsTrue(turnover > 0);
        }

        [TestMethod]
        public void get_generatedquery()
        {
            var content = _soldMarketVehicleService.GetSoldVehicleHistoryListByMake(DateTime.Now, -117.598903, 33.862585).Count;
        }

        public void testGetWithin100Miles()
        {
          
        }

        //private static double GetMarketHistory(List<MakeHistory> history, string make, string model)
        //{
        //    return history.Where(i => i.Make == make && i.Model == model).Select(i => i.History).FirstOrDefault();
        //}
       
    }
}
