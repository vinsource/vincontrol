using System.Collections.Generic;
using System.Text;

namespace vincontrol.Helper
{
    public static class QueryBuilderHelper
    {
        public static string BuildAndContainClause(IEnumerable<string> inputList)
        {
            var result = new StringBuilder();

            foreach (var input in inputList)
            {
                result.Append(string.Format("CONTAINS(*, '{0}') AND ", input));
            }

            if (result.Length > 0)
            {
                result.Remove(result.Length - 5, 4);
            }

            return result.ToString();
        }

        public static object BuildDealerIdClause(IEnumerable<int> dealerIdList)
        {
            var result = new StringBuilder();

            foreach (var dealerId in dealerIdList)
            {
                result.Append(string.Format("DealerId = {0} OR ", dealerId));
            }

            if (result.Length > 0)
            {
                result.Remove(result.Length - 4, 3);
                result.Insert(0, "(");
                result.Append(")");
            }

            return result.ToString();
        }

        public static string GetFullTextTopVehicleQuery(string viewName, List<string> termList, IEnumerable<int> dealerIdList)
        {
            const string formatString = "SELECT * FROM (SELECT TOP 4 * FROM {0} WHERE {1} AND {2} ORDER BY DealerId, Year DESC, Make, Model, Trim) {3}Result";
            return string.Format(formatString, viewName, BuildAndContainClause(termList), BuildDealerIdClause(dealerIdList), viewName);
        }

        public static string GetFullTextVehicleQuery(string viewName, List<string> termList, IEnumerable<int> dealerIdList)
        {
            const string formatString = "SELECT * FROM {0} WHERE {1} AND {2}";
            return string.Format(formatString, viewName, BuildAndContainClause(termList), BuildDealerIdClause(dealerIdList), viewName);
        }

        public static string GetVehicleQuery(string viewName, List<string> termList, int dealerId, string additionCondition)
        {
            string formatString = termList.Count > 0 ? "SELECT * FROM {0} WHERE DealerId = {1} AND {2} {3}" : "SELECT * FROM {0} WHERE DealerId = {1} {2} {3}";

            return string.Format(formatString, viewName, dealerId, BuildAndContainClause(termList), additionCondition);
        }
    }
}