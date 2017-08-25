using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IYoutubeRepository
    {
        IQueryable<YoutubeAccount> GetAllYoutubeAccounts();
        IQueryable<YoutubeVideo> GetVideos(int dealerId);
        YoutubeVideo GetVideo(int id);
        YoutubeVideo GetVideoByVideoId(string videoId);
        void AddNewVideo(YoutubeVideo entity);
    }
}
