using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;

namespace vincontrol.StockingGuide.Service
{
    public class UpdateWeeklyTurnOverByMonthProcess : IUpdateWeeklyTurnOverByMonthProcess
    {
        readonly IWeeklyTurnOverService _weeklyTurnOverService;
        readonly ISoldMarketVehicleService _soldMarketVehicleService;
        readonly ISoldMarketTruckService _soldMarketTruckService;
        readonly IInventoryService _inventoryService;
        readonly IInventorySegmentDetailService _inventorySegmentDetailService;
        private readonly IDealerService _dealerService;
        readonly ILog ServiceLog = LogManager.GetLogger(typeof(UpdateWeeklyTurnOverByMonthProcess));

        public UpdateWeeklyTurnOverByMonthProcess(IWeeklyTurnOverService weeklyTurnOverService, ISoldMarketVehicleService soldMarketVehicleService, IInventoryService inventoryService, IInventorySegmentDetailService inventorySegmentDetailService, IDealerService dealerService, ISoldMarketTruckService soldMarketTruckService)
        {
            _weeklyTurnOverService = weeklyTurnOverService;
            _soldMarketVehicleService = soldMarketVehicleService;
            _inventoryService = inventoryService;
            _inventorySegmentDetailService = inventorySegmentDetailService;
            _dealerService = dealerService;
            _soldMarketTruckService = soldMarketTruckService;
            LogHelper.InitLogger(ServiceLog);
        }

        public void Run()
        {
            var runmode = GetRunModeFromConfig();
            var dealerIdList = _dealerService.GetDealerIdListFromInventory();
            
            foreach (var dealerId in dealerIdList)
            {
                ServiceLog.Info(string.Format("UpdateWeeklyTurnOverByMonthProcess: dealer {0}", dealerId));
                var stock = _inventoryService.GetCurrentMonthUsedStock(dealerId);
                ServiceLog.Info("UpdateWeeklyTurnOverByMonthProcess: Finish _inventoryService.GetCurrentMonthUsedStock(dealerId)");

                if (runmode == Runmode.CurrentMonth)
                {
                    ServiceLog.Info("UpdateWeeklyTurnOverByMonthProcess: Run for current month");

                    var currentWeek = DateTime.Now.GetEndOfWeekListToCurrentDate().FirstOrDefault(i => i.Value >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
                    InsertWeeklyTurnOverIfNotExisted(currentWeek.Key, currentWeek.Value, dealerId, stock);

                }
                else if (runmode == Runmode.AllTwelveMonths)
                {
                    var twelvemonthData = _weeklyTurnOverService.GetAllTurnOversForDealer(dealerId).ToList();
                    ServiceLog.Info("UpdateWeeklyTurnOverByMonthProcess: Run all twelve months");

                    var currentMonthlist = DateTime.Now.GetEndOfWeekListToCurrentDate();
                    var list = new List<SGWeeklyTurnOver>();
                    foreach (var dateTime in currentMonthlist)
                    {
                        InsertOnlyWeeklyTurnOverItemToList(dateTime, dealerId, stock, twelvemonthData, list);
                    }

                    var date = DateTime.Now;
                    for (int i = 0; i < 11; i++)
                    {
                        date = date.AddMonths(-1);
                        var endOfWeekList = date.GetEndOfWeekList();
                        foreach (var week in endOfWeekList)
                        {
                            InsertOnlyWeeklyTurnOverItemToList(week, dealerId, stock, twelvemonthData, list);
                        }
                    }

                    if (list.Any())
                    {
                        _weeklyTurnOverService.AddWeeklyTurnOvers(list);
                    }
                }
            }
        }

        private void InsertOnlyWeeklyTurnOverItemToList(KeyValuePair<short, DateTime> dateTime, int dealerId, int stock, List<SGWeeklyTurnOver> twelvemonthData, List<SGWeeklyTurnOver> list)
        {
            var tempWeeklyTurnOver = GetWeeklyTurnOverInfo(dateTime.Value, dealerId, stock);
            var weeklyTurnOver = twelvemonthData.FirstOrDefault(
                i =>
                    i.Year == (short) dateTime.Value.Year && i.Month == (short) dateTime.Value.Month &&
                    i.Week == dateTime.Key);
            if (weeklyTurnOver != null)
            {
                ServiceLog.Info(string.Format("weekly turnover detail existed with {0} {1} {2}", weeklyTurnOver.Week, weeklyTurnOver.Month, weeklyTurnOver.Year));
            }
            else
            {
                weeklyTurnOver = new SGWeeklyTurnOver
                {
                    Stock = tempWeeklyTurnOver.Stock,
                    Turnover = tempWeeklyTurnOver.Turnover,
                    Year = (short) dateTime.Value.Year,
                    Month = (short) dateTime.Value.Month,
                    Week = dateTime.Key,
                    DealerId = dealerId,
                    CreatedDate = DateTime.Now
                };
                list.Add(weeklyTurnOver);
                ServiceLog.Info(string.Format("Insert weekly turnover detail  with {0} {1} {2}", weeklyTurnOver.Week, weeklyTurnOver.Month, weeklyTurnOver.Year));
            }
        }


        private Runmode GetRunModeFromConfig()
        {
            Runmode runmode;
            Enum.TryParse(ConfigurationManager.AppSettings["RunMode"], out runmode);
            return runmode;
        }


        private void InsertWeeklyTurnOverIfNotExisted(short weekNumber, DateTime dateTime, int dealerId, int stock)
        {
            var tempWeeklyTurnOver = GetWeeklyTurnOverInfo(dateTime, dealerId, stock);
            var weeklyTurnOver = _weeklyTurnOverService.GetCurrentWeeklyTurnOver(dateTime, dealerId);

            if (weeklyTurnOver == null)
            {
                weeklyTurnOver = new SGWeeklyTurnOver
                {
                    Stock = tempWeeklyTurnOver.Stock,
                    Turnover = tempWeeklyTurnOver.Turnover,
                    Year = (short)dateTime.Year,
                    Month = (short)dateTime.Month,
                    Week = weekNumber,
                    DealerId = dealerId,
                    CreatedDate = DateTime.Now,
                    History = tempWeeklyTurnOver.History
                };
                _weeklyTurnOverService.AddWeeklyTurnOver(weeklyTurnOver);
            }
            else
            {
                weeklyTurnOver.Stock = tempWeeklyTurnOver.Stock;
                weeklyTurnOver.Turnover = tempWeeklyTurnOver.Turnover;
                weeklyTurnOver.UpdateDate = DateTime.Now;
                weeklyTurnOver.History = tempWeeklyTurnOver.History;
                _weeklyTurnOverService.SaveChanges();
            }
        }

    
        private SGWeeklyTurnOver GetWeeklyTurnOverInfo(DateTime dateTime, int dealerId, int stock)
        {
            var history = _soldMarketVehicleService.GetHistory(dateTime, dealerId);
            
            var turnover = StockingGuideBusinessHelper.GetTurnOverFromStockAndHistory(stock, history);
            
            var tempWeeklyTurnOver = new SGWeeklyTurnOver
            {
                Stock = stock,
                Turnover = turnover,
                Year = (short)dateTime.Year,
                Month = (short)dateTime.Month,
                Week = (short)dateTime.GetWeekNumber(),
                DealerId = dealerId,
                History = history
            };
            return tempWeeklyTurnOver;
        }
    }

    public enum Runmode
    {
        CurrentMonth, AllTwelveMonths
    }
}
