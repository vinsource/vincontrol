using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vincontrol.Application.Forms.AccountManagement;
using vincontrol.Application.Forms.InventoryManagement;
using vincontrol.Application.Forms.VideoTrackingManagement;
using VINControl.Video;

namespace VINControl.VideoService
{
    public class Program
    {
        //private IAccountManagementForm _accountForm;
        //private IInventoryManagementForm _inventoryForm;

        public static void Main(string[] args)
        {
            var accountForm = new AccountManagementForm();
            var inventoryForm = new InventoryManagementForm();
            var videoTrackingForm = new VideoTrackingManagementForm();
            var videoGenerator = new VideoGenerator();
            var unpostedVideos = videoTrackingForm.GetUnPostedVideos();
            foreach (var video in unpostedVideos)
            {
                var dealer = accountForm.GetDealer(video.DealerId);
                if (String.IsNullOrEmpty(dealer.Setting.YoutubeUsername) || String.IsNullOrEmpty(dealer.Setting.YoutubePassword)) continue;

                var videoId = videoGenerator.Generate(dealer, inventoryForm.GetCarInfo(video.InventoryId));
                if (!String.IsNullOrEmpty(videoId))
                    videoTrackingForm.UpdateVideoTracking(video.InventoryId, string.Format("http://youtu.be/{0}", videoId), true, true);
                else videoTrackingForm.UpdateVideoTracking(video.InventoryId, string.Empty, false, false);
            }
        }
    }
}
