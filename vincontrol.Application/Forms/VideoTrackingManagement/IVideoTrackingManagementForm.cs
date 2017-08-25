using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.VideoTrackingManagement;

namespace vincontrol.Application.Forms.VideoTrackingManagement
{
    public interface IVideoTrackingManagementForm
    {
        void AddNewVideo(VideoTrackingViewModel model);
        void UpdateVideoTracking(int listingId, string url, bool isPosted, bool isSucceeded);
        List<VideoTrackingViewModel> GetUnPostedVideos();
    }
}
