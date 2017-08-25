using Google.GData.YouTube;
using Google.YouTube;

namespace Vincontrol.Youtube
{
    public static class YoutubeExtension
    {
        private const string _embededUrl = "//www.youtube.com/embed/{0}?feature=player_detailpage";

        public static YoutubeVideo ToYoutubeVideo(this YouTubeEntry entry)
        {
            YoutubeVideo video = new YoutubeVideo();

            video.VideoId = entry.VideoId;
            video.Title = entry.Title.Text;
            foreach (var category in entry.Media.Categories)
                if (category.Attributes["label"] != null)
                {
                    video.Categories.Add(category.Attributes["label"].ToString());
                }
            if (entry.Statistics != null)
                video.ViewCounts = long.Parse(entry.Statistics.ViewCount);
            if (entry.Comments != null)
                video.CommentCounts = long.Parse(entry.Comments.FeedLink.CountHint.ToString());
            if (entry.Rating != null)
                video.Rating = entry.Rating.Average;
            if (entry.YtRating != null)
            {
                video.LikeCounts = long.Parse(entry.YtRating.NumLikes);
                video.DislikeCounts = long.Parse(entry.YtRating.NumDislikes);
            }
            video.VideoUrl = entry.AlternateUri.ToString();
            video.EmbededUrl = string.Format(_embededUrl, entry.VideoId);
            video.Uploader = entry.Authors[0].Name;

            return video;
        }

        public static YoutubeVideo ToYoutubeVideo(this Video video)
        {
            var youtubeVideo = video.YouTubeEntry.ToYoutubeVideo();
            youtubeVideo.Description = video.Description;
            youtubeVideo.Thumbnail = video.Thumbnails[0].Attributes["url"].ToString();

            return youtubeVideo;
        }
    }
}
