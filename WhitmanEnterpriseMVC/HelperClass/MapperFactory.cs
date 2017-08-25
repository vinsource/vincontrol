using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhitmanEnterpriseMVC.DatabaseModelScrapping;
using WhitmanEnterpriseMVC.Interfaces;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public class MapperFactory
    {
        public static IQueryable<MarketCarInfo> GetAutoTraderMarketCarQuery(vincontrolscrappingEntities context, int? year)
        {
            if (!year.HasValue)
            {
                return null;
            }

            if (year.Value <= 2000)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_lessorequal_year2000).Where(x=>x.Year==year);
            }
            else if (year.Value >= 2001 && year.Value <= 2003)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_from_year2001_to_year2003).Where(x => x.Year == year);
            }
            else if (year.Value >= 2004 && year.Value <= 2005)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_from_year2004_to_year2005).Where(x => x.Year == year);
            }
            else if (year.Value == 2006)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2006);
            }
            else if (year.Value == 2007)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2007);
            }
            else if (year.Value == 2008)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2008);
            }
            else if (year.Value == 2009)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2009);

            }
            else if (year.Value == 2010)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2010);
            }
            else if (year.Value == 2011)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2011);
            }
            else if (year.Value == 2012)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2012);
            }
            else if (year.Value == 2013)
            {
                return AutotraderMapper.GetMarketCarInfoQuery(context.autotrader_year2013);
            }

            return null;
        }

        public static IQueryable<MarketCarInfo> GetCarsComMarketCarQuery(vincontrolscrappingEntities context, int? year)
        {
            if (!year.HasValue)
            {
                return null;
            }

            if (year.Value <= 2000)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_lessorequal_year2000).Where(x => x.Year == year);
            }
            else if (year.Value >= 2001 && year.Value <= 2003)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_from_year2001_to_year2003).Where(x => x.Year == year);
            }
            else if (year.Value >= 2004 && year.Value <= 2005)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_from_year2004_to_year2005).Where(x => x.Year == year);
            }
            else if (year.Value == 2006)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2006);
            }
            else if (year.Value == 2007)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2007);
            }
            else if (year.Value == 2008)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2008);
            }
            else if (year.Value == 2009)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2009);

            }
            else if (year.Value == 2010)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2010);
            }
            else if (year.Value == 2011)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2011);
            }
            else if (year.Value == 2012)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2012);
            }
            else if (year.Value == 2013)
            {
                return CarsComMapper.GetMarketCarInfoQuery(context.carscom_year2013);
            }

            return null;
        }     
    }
}
