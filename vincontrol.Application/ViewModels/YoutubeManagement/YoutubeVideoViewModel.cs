using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Data.Model;

namespace vincontrol.Application.ViewModels.YoutubeManagement
{
    public class YoutubeVideoViewModel
    {
        #region Ctor
        public YoutubeVideoViewModel() { Categories = new List<string>(); Tags = new List<string>(); }

        public YoutubeVideoViewModel(YoutubeVideo obj)
        {
            DealerId = obj.DealerId;
            YoutubeVideoId = obj.YoutubeVideoId;
            VideoId = obj.VideoId;
            Title = obj.Title;
            Description = obj.Description;
            Thumbnail = obj.Thumbnail;
            if (!string.IsNullOrEmpty(obj.Categories))
                Categories = obj.Categories.Split(',').ToList();
            ViewCounts = obj.ViewCounts.GetValueOrDefault();
            CommentCounts = obj.CommentCounts.GetValueOrDefault();
            LikeCounts = obj.LikeCounts.GetValueOrDefault();
            DislikeCounts = obj.DislikeCounts.GetValueOrDefault();
            Rating = obj.Rating.GetValueOrDefault();
        }
        #endregion

        #region Basic Info
        public int DealerId { get; set; }

        public int InventoryId { get; set; }

        public int YoutubeVideoId { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public List<string> Categories { get; set; }

        public List<string> Tags { get; set; }

        public string Uploader { get; set; }

        public string VideoUrl 
        {
            //get { return string.Format("http://youtu.be/{0}", VideoId); }
            get; set;
        }

        public string EmbededUrl 
        {
            //get { return string.Format("www.youtube.com/embed/{0}?feature=player_detailpage", VideoId); }
            get;
            set;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        #endregion

        #region Statistics
        public List<YoutubeCommentVideModel> Comments { get; set; }

        public double ViewCounts { get; set; }

        public double CommentCounts { get; set; }

        public double LikeCounts { get; set; }

        public double DislikeCounts { get; set; }

        public double Rating { get; set; }
        #endregion

        #region For uploading
        public string LocalFilePath { get; set; }

        public string LocalFileType { get; set; }
        #endregion
    }

    public class YoutubeCommentVideModel
    {
        public string Author { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class YoutubeAccountViewModel
    {
        public YoutubeAccountViewModel() { }

        public YoutubeAccountViewModel(YoutubeAccount obj)
        {
            YoutubeAccountId = obj.YoutubeAccountId;
            DealerId = obj.DealerId;
            YoutubeUserId = obj.YoutubeUserId;
        }
        public int YoutubeAccountId { get; set; }
        public int DealerId { get; set; }
        public string YoutubeUserId { get; set; }
    }
}
