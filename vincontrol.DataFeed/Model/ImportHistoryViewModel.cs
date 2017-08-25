using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Backend.Data;

namespace vincontrol.DataFeed.Model
{
    public class ImportHistoryViewModel
    {
        public int Id { get; set; }
        public DateTime RunningDate { get; set; }
        public string Status { get; set; }
        public string ArchiveFileName { get; set; }
        public int DealerId { get; set; }

        public ImportHistoryViewModel(){}

        public ImportHistoryViewModel(importservicehistory obj)
        {
            Id = obj.Id;
            RunningDate = obj.RunningDate.GetValueOrDefault();
            Status = obj.Status;
            ArchiveFileName = obj.ArchiveFileName;
            DealerId = obj.DealerId.GetValueOrDefault();
        }
    }
}
