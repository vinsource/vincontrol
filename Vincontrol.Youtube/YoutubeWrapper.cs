using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google.GData.Client;
using Google.GData.Extensions.Location;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;
using Google.YouTube;
using vincontrol.ConfigurationManagement;

namespace Vincontrol.Youtube
{
    public class YoutubeWrapper
    {
        private string _applicationName = "VINSOCIAL";
        private string _developerKey, _userId;
        private string _userName = "vinsocvid@gmail.com";
        private string _password = "social1451";

        private const string _videoUrl = "https://gdata.youtube.com/feeds/api/videos";
        private const string _videoByUserUrl = "https://gdata.youtube.com/feeds/api/users/";
        private const string _embededUrl = "//www.youtube.com/embed/{0}?feature=player_detailpage";

        public YoutubeWrapper()
        {
            _developerKey = ConfigurationHandler.YoutubeDeveloperKey;
            _userId = ConfigurationHandler.YoutubeUserId;
        }

        public List<YoutubeVideo> SearchVideo(string query, int numberToRetrieve, string orderBy)
        {
            YouTubeService service = new YouTubeService(_applicationName, _developerKey);
            const String searchFields = "entry(id,title,link[@rel='alternate'],author(name,yt:userId),media:group(media:category(@label),media:credit,yt:videoid,yt:uploaderId),yt:statistics,yt:rating,gd:rating(@average),gd:comments/gd:feedLink(@countHint))";
            String url = String.Format("{0}?v=2&fields={1}", _videoUrl, searchFields);

            YouTubeQuery searchQuery = new YouTubeQuery(url);
            searchQuery.Query = query;
            searchQuery.NumberToRetrieve = numberToRetrieve;
            searchQuery.OrderBy = orderBy;

            YouTubeFeed searchResults = service.Query(searchQuery);

            List<YoutubeVideo> videos = new List<YoutubeVideo>();
            foreach (YouTubeEntry entry in searchResults.Entries)
                videos.Add(entry.ToYoutubeVideo());

            return videos;
        }

        public List<Video> GetVideoByUserId(string userId)
        {
            var youTubeRequestSettings = new YouTubeRequestSettings(_applicationName, _developerKey);
            var request = new YouTubeRequest(youTubeRequestSettings);

            youTubeRequestSettings.PageSize = 50;
            youTubeRequestSettings.AutoPaging = true;

            var feed = request.GetVideoFeed(userId);
            var videos = new List<Video>();
            foreach (Video video in feed.Entries)
                videos.Add(video);

            return videos;
        }

        public List<YoutubeVideo> GetVideoByUserId(int numberToRetrieve, string userId)
        {
            var youTubeRequestSettings = new YouTubeRequestSettings(_applicationName, _developerKey);
            var request = new YouTubeRequest(youTubeRequestSettings);

            youTubeRequestSettings.Maximum = 50;

            var feed = request.GetVideoFeed(_userId);

            var videos = new List<YoutubeVideo>();

            foreach (var item in feed.Entries.ToList())
                videos.Add(item.ToYoutubeVideo());

            return videos;
        }

        public YoutubeVideo GetVideoById(string videoId)
        {
            YouTubeService service = new YouTubeService(_applicationName, _developerKey);
            string url = String.Format("{0}/{1}?v=2&", _videoUrl, videoId);

            YouTubeQuery searchQuery = new YouTubeQuery(url);

            var result = service.Query(searchQuery);
            YouTubeEntry entry = result.Entries.FirstOrDefault() as YouTubeEntry;

            var video = entry.ToYoutubeVideo();

            string commentUrl = String.Format("http://gdata.youtube.com/feeds/api/videos/{0}/comments?max-results={1}&start-index={2}", videoId, 50, 1);

            var youTubeRequestSettings = new YouTubeRequestSettings(_applicationName, _developerKey);
            var request = new YouTubeRequest(youTubeRequestSettings);
            Feed<Comment> comments = request.Get<Comment>(new Uri(commentUrl));

            foreach (var item in comments.Entries)
            {
                video.Comments.Add(new YoutubeComment
                {
                    Author = item.Author,
                    UpdatedOn = item.Updated,
                    Title = item.Title,
                    Content = item.Content,
                });
            }

            return video;
        }

        public bool UploadVideoToYouTube(YoutubeVideo video, out string videoId, string userName = "", string password = "")
        {
            //to upload video onto youtube, the video must have atleast 1 category
            videoId = string.Empty;
            if (video.Categories == null || video.Categories.Count == 0)
                return false;

            var settings = new YouTubeRequestSettings(_applicationName, _developerKey,
                string.IsNullOrEmpty(userName) ? _userName : userName,
                string.IsNullOrEmpty(password) ? _password : password)
            {
                Timeout = 100000000
            };

            var request = new YouTubeRequest(settings);

            var newVideo = new Video { Title = video.Title, Description = video.Description };

            foreach (var category in video.Categories)
            {
                newVideo.Tags.Add(new MediaCategory(category, YouTubeNameTable.CategorySchema));
            }

            foreach (var tag in video.Tags)
            {
                newVideo.Tags.Add(new MediaCategory(tag, YouTubeNameTable.DeveloperTagSchema));
            }

            newVideo.Keywords = video.Tags.Any() ? String.Join(",", video.Tags.ToArray()) : string.Empty;
            newVideo.Private = true;
            newVideo.YouTubeEntry.Private = false;
            newVideo.YouTubeEntry.Location = new GeoRssWhere(video.Latitude, video.Longitude);
            newVideo.YouTubeEntry.MediaSource = new MediaFileSource(video.LocalFilePath, "video/wmv");

            try
            {
                Video createdVideo = request.Upload(newVideo);
                videoId = createdVideo.Id;
                return true;
            }
            catch (Exception ex)
            {
                
            }

            return false;
        }
    }
}
