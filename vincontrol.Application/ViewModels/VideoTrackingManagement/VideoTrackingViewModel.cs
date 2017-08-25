using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.VideoTrackingManagement
{
    public class VideoTrackingViewModel
    {
        public VideoTrackingViewModel(){}

        public VideoTrackingViewModel(VideoTracking obj)
        {
            VideoTrackingId = obj.VideoTrackingId;
            DealerId = obj.DealerId;
            InventoryId = obj.InventoryId;
            Url = obj.Url;
            IsPosted = obj.IsPosted;
            IsSucceeded = obj.IsSucceed;
            CreatedDate = obj.CreatedDate;
            LastDate = obj.LastDate;
        }

        public int VideoTrackingId { get; set; }
        public int DealerId { get; set; }
        public int InventoryId { get; set; }
        public string Url { get; set; }
        public bool IsPosted { get; set; }
        public bool IsSucceeded { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastDate { get; set; }
    }
}
