using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class VideoTrackingRepository : IVideoTrackingRepository
    {
        private VincontrolEntities _context;

        public VideoTrackingRepository(VincontrolEntities context)
        {
            _context = context;
        }

        public void AddNewVideo(VideoTracking obj)
        {
            _context.VideoTrackings.AddObject(obj);
        }

        public VideoTracking GetVideoById(int id)
        {
            return _context.VideoTrackings.FirstOrDefault(i => i.VideoTrackingId.Equals(id));
        }

        public VideoTracking GetVideoByInventoryId(int inventoryId)
        {
            return _context.VideoTrackings.FirstOrDefault(i => i.InventoryId.Equals(inventoryId));
        }

        public IQueryable<VideoTracking> GetUnPostedVideos()
        {
            return _context.VideoTrackings.Where(i => i.IsPosted == false || (i.IsPosted && i.IsSucceed == false));
        }
    }
}
