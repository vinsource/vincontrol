using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vincontrol.Helper;
using vincontrol.StockingGuide.Repository.Interfaces;
using vincontrol.StockingGuide.Repository.UnitOfWorks;

namespace vincontrol.StockingGuide.Repository.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private IVinmarketUnitOfWork _vinmarketUnitOfWork;

        [TestInitialize]
        public void TestInit()
        {
            _vinmarketUnitOfWork = new VinmarketUnitOfWork();
        }
        [TestMethod]
        public void TestGetWithin100miles()
        {
            var result =_vinmarketUnitOfWork.SoldMarketVehicleRepository.GetSoldVehicleForMonthWithin100Miles(DateTime.Now,
                    -117.598903, 33.862585).Where(i => i.Make == "Chevrolet" && i.Model == "Silverado 1500")
                    .Select(i => new {i.Longitude, i.Latitude}).ToList();

            foreach (var item in result)
            {
                var distance = CommonHelper.DistanceBetweenPlaces(33.862585, -117.598903, item.Latitude ?? 0,
                    item.Longitude ?? 0);
                Assert.IsTrue(distance<100);
            }
        }
    }
}
