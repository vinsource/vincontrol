using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.FacebookManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;
using vincontrol.EmailHelper;

namespace vincontrol.Application.Forms.FacebookManagement
{
    public class FacebookManagementForm : BaseForm, IFacebookManagementForm
    {
        private IEmail _emailHelper;

        #region Constructors
        public FacebookManagementForm() : this(new SqlUnitOfWork()) { }

        public FacebookManagementForm(IUnitOfWork unitOfWork)
        {
            _emailHelper = new Email();
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region IFacebookManagementForm Members

        public FacebookPostViewModel InitializeFacebookPostViewModel()
        {
            var viewModel = new FacebookPostViewModel();
            var list = UnitOfWork.Facebook.GetAllSharedWith();
            viewModel.SharedWiths = list.Any() ? list.AsEnumerable().Select(i => new SelectListItem(i.FBSharedWithId, i.Name, false)).ToList() : new List<SelectListItem>();

            return viewModel;
        }

        public FacebookPostViewModel InitializeFacebookPostViewModel(int postId)
        {
            var viewModel = GetById(postId);
            var list = UnitOfWork.Facebook.GetAllSharedWith();
            viewModel.SharedWiths = list.Any() ? list.AsEnumerable().Select(i => new SelectListItem(i.FBSharedWithId, i.Name, false)).ToList() : new List<SelectListItem>();

            return viewModel;
        }

        public IList<FacebookPostViewModel> GetAllPosts()
        {
            var list = UnitOfWork.Facebook.GetAllPosts();
            return list.Any() ? list.AsEnumerable().Select(i => new FacebookPostViewModel(i)).ToList() : new List<FacebookPostViewModel>();
        }

        public IList<FacebookPostViewModel> GetAllPosts(int dealerId)
        {
            var list = UnitOfWork.Facebook.GetAllPosts(dealerId);
            return list.Any() ? list.AsEnumerable().Select(i => new FacebookPostViewModel(i)).ToList() : new List<FacebookPostViewModel>();
        }

        public IList<FacebookPostViewModel> GetAllPosts(int dealerId, DateTime publishDate)
        {
            var list = UnitOfWork.Facebook.GetAllPosts(dealerId, publishDate);
            return list.Any() ? list.AsEnumerable().Select(i => new FacebookPostViewModel(i)).ToList() : new List<FacebookPostViewModel>();
        }

        public IList<FacebookPostViewModel> GetAllPosts(int dealerId, int hour)
        {
            var list = UnitOfWork.Facebook.GetAllPosts(dealerId, hour);
            return list.Any() ? list.AsEnumerable().Select(i => new FacebookPostViewModel(i)).ToList() : new List<FacebookPostViewModel>();
        }

        public IList<FacebookPostViewModel> GetAllPostsByHour(int hour)
        {
            var list = UnitOfWork.Facebook.GetAllPostsByHour(hour);
            return list.Any() ? list.AsEnumerable().Select(i => new FacebookPostViewModel(i)).ToList() : new List<FacebookPostViewModel>();
        }

        public FacebookPostViewModel GetById(int postId)
        {
            var existingPost = UnitOfWork.Facebook.GetById(postId);
            return existingPost != null ? new FacebookPostViewModel(existingPost) : new FacebookPostViewModel();
        }

        public FacebookCredentialViewModel GetCredential(int dealerId)
        {
            var existingCredential = UnitOfWork.Facebook.GetCredential(dealerId);
            return existingCredential != null
                ? new FacebookCredentialViewModel(existingCredential)
                : new FacebookCredentialViewModel();
        }

        public bool CheckCredentialExisting(int dealerId)
        {
            return UnitOfWork.Facebook.CheckCredentialExisting(dealerId);
        }

        public List<FacebookPostSummaryByDay> GetNumberOfPostsByDay(int dealerId)
        {
            var list = UnitOfWork.Facebook.GetNumberOfPostsByDay(dealerId);
            var order = 1;
            return list.Any() ? list.Select(i => new FacebookPostSummaryByDay() { Day = i.Key, NumberOfPosts = i.Value, Order = order ++ }).ToList() : new List<FacebookPostSummaryByDay>();
        }

        public string GetUserNameByRealPostId(long realPostId)
        {
            var post = UnitOfWork.Facebook.GetByRealPostId(realPostId);
            if (post == null) return string.Empty;
            var user = UnitOfWork.User.GetUser(post.UserId);

            return user == null ? string.Empty : user.Name;
        }

        public bool CheckPublishDateConflict(int dealerId, DateTime publishDate)
        {
            return UnitOfWork.Facebook.CheckPublishDateConflict(dealerId, publishDate);
        }

        public bool CheckPublishDateConflict(int postId, int dealerId, DateTime publishDate) 
        {
            return UnitOfWork.Facebook.CheckPublishDateConflict(postId, dealerId, publishDate);
        }

        public bool CheckTokenExpiration(int dealerId)
        {
            var existingCredential = UnitOfWork.Facebook.GetCredential(dealerId);
            return existingCredential == null || existingCredential.ExpiredDate < DateTime.Now ? true : false;
        }

        public string GetShortToken(int dealerId)
        {
            var existingCredential = UnitOfWork.Facebook.GetCredential(dealerId);
            return existingCredential == null || existingCredential.ExpiredDate < DateTime.Now ? string.Empty : existingCredential.AccessToken;
        }

        public long GetPageId(int dealerId)
        {
            var existingCredential = UnitOfWork.Facebook.GetCredential(dealerId);
            return existingCredential == null ? 0 : existingCredential.PageId.GetValueOrDefault();
        }

        public void AddNewPost(FacebookPostViewModel post)
        {
            UnitOfWork.Facebook.AddNewPost(MappingHandler.ConvertViewModelToFBPost(post));
            UnitOfWork.CommitVinreviewModel();
        }

        public void AddNewCredential(FacebookCredentialViewModel post)
        {
            UnitOfWork.Facebook.AddNewCredential(MappingHandler.ConvertViewModelToFBCredential(post));
            UnitOfWork.CommitVinreviewModel();
        }

        public void AddNewCredential(FacebokPersonalInfo post)
        {
            UnitOfWork.Facebook.AddNewCredential(MappingHandler.ConvertViewModelToFBCredential(post));
            UnitOfWork.CommitVinreviewModel();
        }

        public void UpdateCredential(FacebookCredentialViewModel post)
        {
            var existingCredential = UnitOfWork.Facebook.GetCredential(post.DealerId);
            if (existingCredential != null)
            {
                //existingCredential.Category = post.Category;
                //existingCredential.Name = post.Name;
                existingCredential.AccessToken = post.AccessToken;
                existingCredential.ExpiredDate = DateTime.Now.AddDays(30);
                UnitOfWork.CommitVinreviewModel();
            }
        }

        public void UpdateCredential(FacebokPersonalInfo info)
        {
            var existingCredential = UnitOfWork.Facebook.GetCredential(info.DealerId);
            if (existingCredential != null)
            {
                existingCredential.Category = info.Category;
                existingCredential.Name = info.Name;
                existingCredential.PageId = info.Id;
                existingCredential.PageUrl = info.Link;
                existingCredential.About = info.About;
                existingCredential.Website = info.Website;
                existingCredential.Phone = info.Phone;
            }
            else
            {
                AddNewCredential(info);
            }

            UnitOfWork.CommitVinreviewModel();
        }

        public void UpdateCredential(string email, string password, int dealerId)
        {
            var existingCredential = UnitOfWork.Facebook.GetCredential(dealerId);
            if (existingCredential != null)
            {
                existingCredential.Email = email;
                existingCredential.Password = password;
                
                UnitOfWork.CommitVinreviewModel();
            }
        }

        public void UpdatePostStatus(int postId, long realPostId, string status)
        {
            var existingPost = UnitOfWork.Facebook.GetById(postId);
            if (existingPost != null)
            {
                existingPost.RealPostId = realPostId;
                existingPost.Status = status;
                UnitOfWork.CommitVinreviewModel();

                if (status.Equals(Constant.Constanst.FBPostStatus.Done) && !existingPost.UserId.Equals(0))
                {
                    var currentUser = UnitOfWork.User.GetUser(existingPost.UserId);
                    if (currentUser == null) return;

                    var credential = UnitOfWork.Facebook.GetCredential(existingPost.DealerId);
                    var emailContent = EmailTemplateReader.GetFacebookPostNotificationEmailContent();                    
                    emailContent = emailContent.Replace(EmailTemplateReader.FacebookFanPage, credential.Name);
                    emailContent = emailContent.Replace(EmailTemplateReader.FacebookPostUrl, String.Format("https://www.facebook.com/{0}_{1}", credential.PageId, realPostId));//https://www.facebook.com/permalink.php?story_fbid={0}&id={1}
                    emailContent = emailContent.Replace(EmailTemplateReader.FacebookPost, existingPost.Content);
                    _emailHelper.SendEmail(new List<string> { currentUser.Email }, currentUser.Name + " posted on wall of " + credential.Name, emailContent);
                }
            }
        }

        public void UpdatePost(FacebookPostViewModel post)
        {
            var existingPost = UnitOfWork.Facebook.GetById(post.PostId);
            if (existingPost != null)
            {
                existingPost.Content = post.Content;
                existingPost.Link = post.Link;
                existingPost.Picture = post.Picture;
                existingPost.PublishDate = post.PublishDate;
                existingPost.FBSharedWithId = post.SharedWithId;
                existingPost.LocationId = post.LocationId;
                existingPost.LocationName = post.LocationName;
                UnitOfWork.CommitVinreviewModel();
            }
        }

        public void DeletePost(int postId)
        {
            UnitOfWork.Facebook.DeletePost(postId);
            UnitOfWork.CommitVinreviewModel();
        }

        public void AddFbPostTracking(FacebookPostViewModel post)
        {
            UnitOfWork.Facebook.AddFbPostTracking(MappingHandler.ConvertViewModelToFBPostTracking(post));
            UnitOfWork.CommitVincontrolModel();
        }

        #endregion
    }
}
