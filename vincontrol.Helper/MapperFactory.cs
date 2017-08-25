using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.DomainObject;
using vincontrol.Data.Model;

namespace vincontrol.Helper
{
    public class MapperFactory
    {
        public static IQueryable<MarketCarInfo> GetMarketCarQueryForCurrent(VinMarketEntities context, int? year)
        {
                return AutotraderMapper.GetMarketCarInfoQuery(context.years).Where(x => x.Year.Value == year);
        }

        public static IQueryable<MarketCarInfo> GetManheimTransactionQuery(VinsellEntities context, int? year)
        {
            var currendate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var past30Days = currendate.AddDays(-30);

            return ManheimMapper.GetMarketCarInfoQuery(context.manheim_vehicles_sold).Where(x => x.Year.Value == year && x.CurrentPrice > 0 && x.Mileage > 0 && x.DateAdded >= past30Days);
        }

   

        public static IQueryable<MarketCarInfo> GetMarketCarQueryForSold(VinMarketEntities context, int? year, int days)
        {
            var lastDays = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays((-1) * days);
            return AutotraderMapper.GetMarketCarInfoQuery(context.yearsolds.Where(x => x.LastUpdated >= lastDays)).Where(x => x.Year.Value == year);
        }

        public static IQueryable<MarketCarInfo> GetMarketCarQuery(VinMarketEntities context, int? year, bool isSold)
        {
            return isSold ? GetMarketCarQueryForSold(context, year, 30) : GetMarketCarQueryForCurrent(context, year);
        }

        public static IQueryable<MarketCarInfo> GetMarketCarQueryByYearRange(VinMarketEntities context, int yearFrom, int yearTo)
        {
            return AutotraderMapper.GetMarketCarInfoQuery(context.years).Where(x => x.Year.Value <= yearTo && x.Year.Value >= yearFrom);
        }

        public static IQueryable<MarketCarInfo> GetMarketCarQuery(VinsellEntities context, int? year)
        {
            return GetManheimTransactionQuery(context, year);
        }

       
        [Obsolete]
        public static IQueryable<MarketCarInfo> GetAutoTraderMarketCarQuery(VinMarketEntities context, int? year)
        {
            return AutotraderMapper.GetMarketCarInfoQuery(context.years).Where(x => x.Year.Value == year);
        }

        [Obsolete]
        public static IQueryable<MarketCarInfo> GetCarsComMarketCarQuery(VinMarketEntities context, int? year)
        {
            return CarsComMapper.GetMarketCarInfoQuery(context.years).Where(x => x.Year.Value == year);
        }
    }
}
