using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using vincontrol.Data.Model;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;

namespace vincontrol.StockingGuide.Service
{
    public class UpdateSegmentChromeProcess : IUpdateSegmentChromeProcess
    {
        readonly IChromeService _chromeService;
        private readonly ISegmentService _segmentService;

        public UpdateSegmentChromeProcess(IChromeService chromeService, ISegmentService segmentService)
        {
            _chromeService = chromeService;
            _segmentService = segmentService;
        }

        public void Run()
        {
            var hashtable = new Hashtable();
            string fileName = ConfigurationManager.AppSettings["SegmentFilePath"]; 
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName +
                                   ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\";";
            List<SGSegment> segments = _segmentService.GetAllSegments();

            using (var connection = new OleDbConnection(strConn))
            {
                connection.Open();
                var command = new OleDbCommand("select * from [Full List From 2000$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    //log4netEngine.Error("Start reading excel");
                    while (dr != null && dr.Read())
                    {
                        if (dr[0].ToString() != "TrimId")
                        {
                            string segment = dr[5].ToString();
                            string year = dr[1].ToString();
                            string make = dr[2].ToString();
                            string model = dr[3].ToString();
                            string vehicle = year + make + model;
                            if (!hashtable.Contains(vehicle))
                            {
                                hashtable.Add(vehicle, vehicle);
                                Model chromeModel = _chromeService.GetModelByYearMakeModel(year, make, model).FirstOrDefault();
                                var firstOrDefault = segments.FirstOrDefault(i => i.Name == segment);
                                if (firstOrDefault != null&&chromeModel != null)
                                {
                                   chromeModel.SGSegmentId = firstOrDefault.SGSegmentId;
                                    _chromeService.SaveChanges();
                                }
                            }
                        }
                    }
                    //log4netEngine.Error("End reading excel");
                }
            }

            
        }
    }
}
