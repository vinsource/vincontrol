using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vincontrol.StockingGuide.Common.Helpers;

namespace vincontrol.StockingGuide.UnitTest.CommonTests
{
    [TestClass]
    public class StockingGuideBusinessHelperTests
    {
        [TestMethod]
        public void GetSupplyFromStockAndHistory()
        {
            Assert.AreEqual(StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(5, 0), 0);
            Assert.AreEqual(StockingGuideBusinessHelper.GetSupplyFromStockAndHistory(5, 30), 5);
        }

        [TestMethod]
        public void GetTurnOverFromStockAndHistory()
        {
            Assert.AreEqual(StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(0, 3), 0);
            Assert.AreEqual(StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(12, 6), 6);
        }

        [TestMethod]
        public void GetAge()
        {
            Assert.AreEqual(StockingGuideBusinessHelper.GetAge(new DateTime(1996, 12, 5), new DateTime(1996, 12, 5)), 0);
            Assert.AreEqual(StockingGuideBusinessHelper.GetAge(new DateTime(2013, 12, 31), new DateTime(2014, 1, 2)), 2);
            Assert.AreEqual(StockingGuideBusinessHelper.GetAge(new DateTime(2013, 12, 28), new DateTime(2013, 12, 29)), 1);
        }
    }
}
