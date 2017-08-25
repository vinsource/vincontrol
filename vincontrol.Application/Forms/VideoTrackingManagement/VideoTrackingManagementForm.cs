using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.VideoTrackingManagement;
using vincontrol.Constant;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.VideoTrackingManagement
{
    public class VideoTrackingManagementForm : BaseForm, IVideoTrackingManagementForm
    {
        #region Constructors
        public VideoTrackingManagementForm() : this(new SqlUnitOfWork()) { }

        public VideoTrackingManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        public void AddNewVideo(VideoTrackingViewModel model)
        {
            var existingVideo = UnitOfWork.VideoTracking.GetVideoByInventoryId(model.InventoryId);
            if (existingVideo == null)
            {
                UnitOfWork.VideoTracking.AddNewVideo(MappingHandler.ToEntity(model));
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public void UpdateVideoTracking(int listingId, string url, bool isPosted, bool isSucceeded)
        {
            var existingVideoTracking = UnitOfWork.VideoTracking.GetVideoByInventoryId(listingId);
            if (existingVideoTracking != null)
            {
                existingVideoTracking.Url = url;
                existingVideoTracking.IsPosted = isPosted;
                existingVideoTracking.IsSucceed = isSucceeded;
                existingVideoTracking.LastDate = DateUtilities.Now();
                UnitOfWork.CommitVincontrolModel();
            }
        }

        public List<VideoTrackingViewModel> GetUnPostedVideos()
        {
            var list = UnitOfWork.VideoTracking.GetUnPostedVideos();
            return list.Any()
                ? list.AsEnumerable().Select(i => new VideoTrackingViewModel(i)).ToList()
                : new List<VideoTrackingViewModel>();
        }
    }
}
