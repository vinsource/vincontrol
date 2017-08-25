using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IVideoTrackingRepository
    {
        void AddNewVideo(VideoTracking obj);
        VideoTracking GetVideoById(int id);
        VideoTracking GetVideoByInventoryId(int inventoryId);
        IQueryable<VideoTracking> GetUnPostedVideos();
    }
}
