using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;

namespace vincontrol.Data.Repository.Implementation
{
    public class FacebookRepository : IFacebookRepository
    {
        private VinReviewEntities _context;
        private VincontrolEntities _contextVincontrol;

        public FacebookRepository(VinReviewEntities context,VincontrolEntities contextVincontrol)
        {
            _context = context;
            _contextVincontrol = contextVincontrol;
        }

        #region IFacebookRepository Members

        public IQueryable<FBPost> GetAllPosts()
        {
            return _context.FBPosts;
        }

        public IQueryable<FBPost> GetAllPosts(int dealerId)
        {
            return _context.FBPosts.Where(i => i.DealerId == dealerId);
        }

        public IQueryable<FBPost> GetAllPosts(int dealerId, DateTime publishDate)
        {
            return _context.FBPosts.Where(i => i.DealerId == dealerId && EntityFunctions.TruncateTime(i.PublishDate) == publishDate);
        }

        public IQueryable<FBPost> GetAllPosts(int dealerId, int hour)
        {
            return _context.FBPosts.Where(i => i.DealerId == dealerId && i.PublishDate.Hour == hour);
        }

        public IQueryable<FBPost> GetAllPostsByHour(int hour)
        {
            return _context.FBPosts.Where(i => i.PublishDate.Hour == hour);
        }

        public IQueryable<FBSharedWith> GetAllSharedWith()
        {
            return _context.FBSharedWiths;
        }

        public FBPost GetById(int postId)
        {
            return _context.FBPosts.FirstOrDefault(i => i.FBPostId == postId);
        }

        public FBPost GetByRealPostId(long realPostId)
        {
            return _context.FBPosts.FirstOrDefault(i => i.RealPostId == realPostId);
        }

        public FBCredential GetCredential(int dealerId)
        {
            return _context.FBCredentials.FirstOrDefault(i => i.DealerId == dealerId);
        }

        public bool CheckCredentialExisting(int dealerId)
        {
            return _context.FBCredentials.Any(i => i.DealerId == dealerId);
        }

        public Dictionary<string, string> GetNumberOfPostsByDay(int dealerId)
        {
            return _context.FBPosts.Where(i => i.DealerId == dealerId).OrderByDescending(i => i.PublishDate).GroupBy(i => EntityFunctions.TruncateTime(i.PublishDate)).ToDictionary(i => i.Key.Value.ToString("MM/dd/yyyy"), i => i.Count().ToString(), StringComparer.OrdinalIgnoreCase);
        }

        public bool CheckPublishDateConflict(int dealerId, DateTime publishDate)
        {
            return _context.FBPosts.Any(i => i.DealerId == dealerId && (i.PublishDate) == publishDate);
        }

        public bool CheckPublishDateConflict(int postId, int dealerId, DateTime publishDate)
        {
            return _context.FBPosts.Any(i => i.FBPostId != postId && i.DealerId == dealerId && (i.PublishDate) == publishDate);
        }

        public void AddNewPost(FBPost post)
        {
            _context.AddToFBPosts(post);
        }

        public void AddNewCredential(FBCredential post)
        {
            _context.AddToFBCredentials(post);
        }

        public void DeletePost(int postId)
        {
            var existingPost = GetById(postId);
            if (existingPost != null)
                _context.DeleteObject(existingPost);
        }


        public void AddFbPostTracking(FBPostTracking post)
        {
            _contextVincontrol.AddToFBPostTrackings(post);
        }
        #endregion
    }
}
