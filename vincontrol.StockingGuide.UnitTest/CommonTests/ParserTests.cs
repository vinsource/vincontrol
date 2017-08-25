using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vincontrol.StockingGuide.Common.Helpers;

namespace vincontrol.StockingGuide.UnitTest.CommonTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void GetListBySeparatingCommas_not_return_empty_value()
        {
            const string value = "Danny,Test,Other,";
            var result = Parser.GetListBySeparatingCommas(value);
            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod]
        public void GetListBySeparatingCommas_return_correct_value()
        {
            const string value = "Danny,Test,Other,";
            var result = Parser.GetListBySeparatingCommas(value);
            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Danny");
            Assert.AreEqual(result[1], "Test");
            Assert.AreEqual(result[2], "Other");
        }
    }
}
