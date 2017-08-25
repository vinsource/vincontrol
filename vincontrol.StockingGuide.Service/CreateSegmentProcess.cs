using System;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using log4net;
using vincontrol.StockingGuide.Common;
using vincontrol.StockingGuide.Common.Helpers;
using vincontrol.StockingGuide.Interfaces;
using vincontrol.StockingGuide.Service.Contracts;

namespace vincontrol.StockingGuide.Service
{
    public class CreateSegmentProcess : ICreateSegmentProcess
    {
        private readonly ISegmentService _segmentService;
      

        public void Run()
        {
            _segmentService.AddSegments(GetSegmentList().Values);
        }

        public CreateSegmentProcess(ISegmentService segmentService)
        {
            _segmentService = segmentService;
          
        }

        private Hashtable GetSegmentList()
        {
            var hashtable = new Hashtable();
            string fileName = ConfigurationManager.AppSettings["SegmentFilePath"];
            ServiceLog.Info(fileName);
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName +
                                   ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\";";
            var dbSegmentList = _segmentService.GetAllSegments().Select(i=>i.Name).ToList();

            try
            {
                using (var connection = new OleDbConnection(strConn))
                {
                    connection.Open();
                    var command = new OleDbCommand("select * from [Full List From 2000$]", connection);
                    using (OleDbDataReader dr = command.ExecuteReader())
                    {
                      
                        while (dr != null && dr.Read())
                        {
                            if (dr[0].ToString() != "TrimId")
                            {
                                string segment = dr[5].ToString();
                                if (!hashtable.Contains(segment) && !dbSegmentList.Contains(segment))
                                {
                                    hashtable.Add(segment, segment);
                                }
                            }
                        }
                     
                    }
                }
            }
            catch (Exception e)
            {
                ServiceLog.Info(e.InnerException.Message);
                ServiceLog.Info(e.StackTrace);
                ServiceLog.Info(e.Message);
            }

            return hashtable;
        }
    }
}
