using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Entity.Custom;

namespace vincontrol.StockingGuide.Interfaces
{
    public interface IChromeService   
    {
        List<string> GetModels(string make);
        List<Model> GetModels(int startIndex, int pageSize);
        IQueryable<Model> GetModelByYearMakeModel(string year, string make, string model);
        void SaveChanges();
        //short? GetSegmentId(int year, string make, string model);
        short? GetSegmentId(string make, string model);
        List<MakeModelSegmentId> GetMakeModelSegmentList();
        IQueryable<Model> GetModelByMakeModel(string make, string model);
        List<MakeModelTrim> GetMakeModelTrimList();
    }
}