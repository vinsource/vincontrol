using System.Linq;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class YoutubeRepository : IYoutubeRepository
    {
        #region Members

        private VinReviewEntities _context;

        #endregion

        #region Ctor

        public YoutubeRepository(VinReviewEntities context)
        {
            _context = context;
        }

        #endregion

        #region Youtube Interface Implementation

        public IQueryable<YoutubeAccount> GetAllYoutubeAccounts()
        {
            return _context.YoutubeAccounts;
        }

        public IQueryable<YoutubeVideo> GetVideos(int dealerId)
        {
            return _context.YoutubeVideos.Where(x => x.DealerId == dealerId);
        }

        public YoutubeVideo GetVideo(int id)
        {
            return _context.YoutubeVideos.FirstOrDefault(x => x.YoutubeVideoId == id);
        }

        public YoutubeVideo GetVideoByVideoId(string videoId)
        {
            return _context.YoutubeVideos.FirstOrDefault(x => x.VideoId == videoId);
        }

        public void AddNewVideo(YoutubeVideo entity)
        {
            _context.AddToYoutubeVideos(entity);
        }

        #endregion
    }
}
