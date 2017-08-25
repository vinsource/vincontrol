using System.Collections.Generic;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.YoutubeManagement;

namespace vincontrol.Application.Forms.YoutubeManagement
{
    public interface IYoutubeManagementForm
    {
        YoutubeVideoViewModel InitializeYoutubePostViewModel();
        YoutubeVideoViewModel InitializeYoutubePostViewModel(CarShortViewModel car);
        List<YoutubeAccountViewModel> GetAllYoutubeAccounts();
        List<YoutubeVideoViewModel> GetVideos(int dealerId);
        void SaveVideoStatistic(YoutubeVideoViewModel model);
    }
}
