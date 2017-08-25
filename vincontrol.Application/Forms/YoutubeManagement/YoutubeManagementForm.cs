using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.YoutubeManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Forms.YoutubeManagement
{
    public class YoutubeManagementForm : BaseForm, IYoutubeManagementForm
    {
        #region Constructors
        public YoutubeManagementForm() : this(new SqlUnitOfWork()) { }

        public YoutubeManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region Youtube Interface Implementation
        public YoutubeVideoViewModel InitializeYoutubePostViewModel()
        {
            var viewModel = new YoutubeVideoViewModel();
            return viewModel;
        }

        public YoutubeVideoViewModel InitializeYoutubePostViewModel(CarShortViewModel car)
        {
            var viewModel = new YoutubeVideoViewModel
            {
                DealerId = car.DealerId,
                InventoryId = car.ListingId,
                Thumbnail = String.IsNullOrEmpty(car.CarImageUrl) ? car.DefaultImageUrl.Split(',')[0] : car.CarImageUrl.Split(',')[0],
                Title = String.Format("{0} {1} {2} {3}", car.ModelYear, car.Make, car.Model, car.Trim),
                Description = car.Description
            };
            viewModel.Categories.Add("Autos & Vehicles");
            viewModel.Tags.Add("Auto");
            viewModel.Tags.Add("Vehicle");
            viewModel.Tags.Add(car.Make);
            viewModel.Tags.Add(car.Model);

            var existingVideoTracking = UnitOfWork.VideoTracking.GetVideoByInventoryId(car.ListingId);
            if (existingVideoTracking != null)
            {
                viewModel.YoutubeVideoId = existingVideoTracking.VideoTrackingId;
                viewModel.VideoUrl = existingVideoTracking.Url;
                viewModel.EmbededUrl = string.IsNullOrEmpty(existingVideoTracking.Url)
                    ? string.Empty
                    : string.Format("https://www.youtube.com/embed/{0}?autohide=1&amp;et=OEgsToPDskLs-vUcrI_a_6mDYpxlh5GM&amp;rel=0", Path.GetFileName(existingVideoTracking.Url));
            }

            return viewModel;
        }
        
        public List<YoutubeAccountViewModel> GetAllYoutubeAccounts()
        {
            var list = UnitOfWork.Youtube.GetAllYoutubeAccounts();
            return list.Any() ? list.AsEnumerable().Select(i => new YoutubeAccountViewModel(i)).ToList() : new List<YoutubeAccountViewModel>();
        }

        public List<YoutubeVideoViewModel> GetVideos(int dealerId)
        {
            var list = UnitOfWork.Youtube.GetVideos(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new YoutubeVideoViewModel(i)).ToList() : new List<YoutubeVideoViewModel>();
        }

        public void SaveVideoStatistic(YoutubeVideoViewModel model)
        {
            var entity = UnitOfWork.Youtube.GetVideoByVideoId(model.VideoId);
            if (entity != null)
            {
                model.ToEntity(entity);
            }
            else
            {
                entity = model.ToEntity();
                UnitOfWork.Youtube.AddNewVideo(entity);
            }

            UnitOfWork.CommitVinreviewModel();
        }
        #endregion
    }
}
