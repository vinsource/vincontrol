using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;

namespace vincontrol.Data.Repository.Interface
{
    public interface IFacebookRepository
    {
        IQueryable<FBPost> GetAllPosts();
        IQueryable<FBPost> GetAllPosts(int dealerId);
        IQueryable<FBPost> GetAllPosts(int dealerId, DateTime publishDate);
        IQueryable<FBPost> GetAllPosts(int dealerId, int hour);
        IQueryable<FBPost> GetAllPostsByHour(int hour);
        IQueryable<FBSharedWith> GetAllSharedWith();
        FBPost GetById(int postId);
        FBPost GetByRealPostId(long realPostId);
        FBCredential GetCredential(int dealerId);
        bool CheckCredentialExisting(int dealerId);
        Dictionary<string, string> GetNumberOfPostsByDay(int dealerId);
        bool CheckPublishDateConflict(int dealerId, DateTime publishDate);
        bool CheckPublishDateConflict(int postId, int dealerId, DateTime publishDate);
        void AddNewPost(FBPost post);
        void AddNewCredential(FBCredential post);
        void DeletePost(int postId);
        void AddFbPostTracking(FBPostTracking post);
    }
}
