using System;
using System.Collections.Generic;
using Vincontrol.Youtube.Common;

namespace Vincontrol.Youtube
{
    public class YoutubeVideo
    {
        #region Ctor
        public YoutubeVideo()
        {
            Categories = new List<string>();
            Tags = new List<string>();
            Comments = new List<YoutubeComment>();
        }
        #endregion

        #region Basic Info
        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public List<string> Categories { get; set; }

        public List<string> Tags { get; set; }

        public string Uploader { get; set; }

        public string VideoUrl { get; set; }

        public string EmbededUrl { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        #endregion

        #region Statistics
        public List<YoutubeComment> Comments { get; set; }

        public long ViewCounts { get; set; }

        public long CommentCounts { get; set; }

        public long LikeCounts { get; set; }

        public long DislikeCounts { get; set; }

        public double Rating { get; set; }
        #endregion

        #region For uploading
        public string LocalFilePath { get; set; }

        public string LocalFileType { get; set; }
        #endregion
    }

    public class YoutubeComment
    {
        public string Author { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
