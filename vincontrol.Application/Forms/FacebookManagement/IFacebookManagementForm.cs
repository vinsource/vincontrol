using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.FacebookManagement;

namespace vincontrol.Application.Forms.FacebookManagement
{
    public interface IFacebookManagementForm
    {
        FacebookPostViewModel InitializeFacebookPostViewModel();
        FacebookPostViewModel InitializeFacebookPostViewModel(int postId);
        IList<FacebookPostViewModel> GetAllPosts(int dealerId, DateTime publishDate);
        IList<FacebookPostViewModel> GetAllPosts(int dealerId, int hour);
        IList<FacebookPostViewModel> GetAllPostsByHour(int hour);
        IList<FacebookPostViewModel> GetAllPosts();
        IList<FacebookPostViewModel> GetAllPosts(int dealerId);
        FacebookPostViewModel GetById(int postId);
        FacebookCredentialViewModel GetCredential(int dealerId);
        bool CheckCredentialExisting(int dealerId);
        List<FacebookPostSummaryByDay> GetNumberOfPostsByDay(int dealerId);
        string GetShortToken(int dealerId);
        long GetPageId(int dealerId);
        string GetUserNameByRealPostId(long realPostId);
        bool CheckPublishDateConflict(int dealerId, DateTime publishDate);
        bool CheckPublishDateConflict(int postId, int dealerId, DateTime publishDate);
        bool CheckTokenExpiration(int dealerId);
        void AddNewPost(FacebookPostViewModel post);
        void AddNewCredential(FacebookCredentialViewModel post);
        void UpdateCredential(FacebookCredentialViewModel post);
        void UpdateCredential(FacebokPersonalInfo info);
        void UpdateCredential(string email, string password, int dealerId);
        void UpdatePost(FacebookPostViewModel post);
        void UpdatePostStatus(int postId, long realPostId, string status);
        void DeletePost(int postId);
        void AddFbPostTracking(FacebookPostViewModel post);
    }
}
