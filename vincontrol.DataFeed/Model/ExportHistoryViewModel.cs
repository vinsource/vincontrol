using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data;

namespace vincontrol.DataFeed.Model
{
    public class ExportHistoryViewModel
    {
        public int Id { get; set; }
        public DateTime RunningDate { get; set; }
        public string Status { get; set; }
        public string ArchiveFileName { get; set; }
        public int DatafeedProfileId { get; set; }

        public ExportHistoryViewModel() { }

        public ExportHistoryViewModel(exportservicehistory obj)
        {
            Id = obj.Id;
            RunningDate = obj.RunningDate.GetValueOrDefault();
            Status = obj.Status;
            ArchiveFileName = obj.ArchiveFileName;
            DatafeedProfileId = obj.DatafeedProfileId.GetValueOrDefault();
        }
    }
}
