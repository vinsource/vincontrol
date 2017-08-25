using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using VINCONTROL.Services.Model;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace VINCONTROL.Services.Business
{
    public class MarketValue
    {
        public List<VinsellChartVehicle> _list { get; set; }

        private double _sumX;
        private double _sumY;
        private double _sumXY;
        private double _sumXSquare;
        private double _count;
        private double? _slope;
        private double? _intercept;

        public MarketValue(List<VinsellChartVehicle> list)
        {
            if (list == null)
            {
                list = new List<VinsellChartVehicle>();
            }

            if (list.Count > 0)
            {
                _list = list;
                _sumY = list.Sum(e => e.Price);
                _sumX = list.Sum(e => e.Mileage);
                _count = list.Count();
                _sumXY = list.Sum(e => e.Price * e.Mileage);
                _sumXSquare = list.Sum(e => e.Mileage * e.Mileage);
                _slope = GetSlope();
                _intercept = GetIntercept();
            }
        }

        private double? GetIntercept()
        {
            if (_count == 1)
            {
                return (_sumX + _sumY);
            }
            else
            {
                if (!_slope.HasValue)
                {
                    return null;
                }
                else
                {
                    return (_sumY - _slope.Value * _sumX) / _count;
                }
            }
        }

        private double? GetSlope()
        {
            if (_count == 1)
            {
                return -1;
            }
            else
            {
                if (_count * _sumXSquare - _sumX * _sumX == 0)
                {
                    return null;
                }
                else
                {
                    return (_count * _sumXY - _sumX * _sumY) / (_count * _sumXSquare - _sumX * _sumX);
                }
            }
        }

        private double? GetPrice(int mileage, double number)
        {
            if (_intercept != null)
            {
                double aOver = _intercept.Value * number;
                return aOver + _slope * mileage;
            }

            return null;
        }

        private double? GetMileage(double price, double number)
        {
            if (_intercept != null)
            {
                double aOver = _intercept.Value * number;
                return (price - aOver) / _slope;
            }

            return null;
        }


        public double? GetOverGoodPrice(int mileage)
        {
            return GetPrice(mileage, 1.025);
        }

        public double? GetOverGoodMileage(double price)
        {
           return GetMileage(price, 1.025);
        }

        public double? GetOverBadPrice(int mileage)
        {
             return GetPrice(mileage, 1.2);
        }
        
        public double? GetOverBadMileage(double price)
        {
            return GetMileage(price, 1.2);
        }

        public double? GetUnderGoodPrice(int mileage)
        {
            return GetPrice(mileage, 0.975);
        }

        public double? GetUnderGoodMileage(double price)
        {
            return GetMileage(price, 0.975);
        }

        public double? GetUnderBadPrice(int mileage)
        {
            return GetPrice(mileage, 0.8);
        }
        
        public double? GetGoodPrice(int mileage)
        {
            return GetPrice(mileage, 1);
        }

        public double? GetUnderBadMileage(int mileage)
        {
            return GetMileage(mileage, 0.8);
        }

        public double? GetGoodMileage(int mileage)
        {
            return GetMileage(mileage, 1);
        }
    }

    public enum CarValueRange
    {
        NoValue, BelowMarket, InMarket, OverMarket, NA
    }
}
