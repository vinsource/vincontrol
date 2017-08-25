using System;
using System.Collections.Generic;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface ISoldMarketVehicleService
    {
        int GetHistory(DateTime dateTime, int dealerId);
        List<List<MakeHistory>> GetHistoryList(DateTime dateTime, int dealerId);
        List<List<MakeHistory>> GetHistoryList(DateTime dateTime, double longitude, double lattitude);

        List<MakeHistory> GetSoldVehicleHistoryListByMake(DateTime dateTime, double longitude, double lattitude);

    }
}