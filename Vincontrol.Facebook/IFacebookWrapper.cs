using System;
using System.Collections.Generic;
using System.Dynamic;
using vincontrol.Application.ViewModels.FacebookManagement;

namespace Vincontrol.Facebook
{
    public interface IFacebookWrapper
    {        
        long Post(string pageId, FacebookPostViewModel model);
        long PostPhoto(string pageId, FacebookPostViewModel model);
        long PostVideo(string pageId, FacebookPostViewModel model);
        void Delete(string pageId, string realPostId);
        string GetLongLiveToken(string debugToken);
        void GetShortTokens();
        string GetShortToken(int dealerId);
        string GetShortToken(string pageId);
        FacebokPersonalInfo GetPersonalInfo(string id);
        List<FacebookPostViewModel> GetTopPosts(string pageId, bool viewInsight = false, bool includeUserName = false);
        FacebookInsightChart InitializeInsightChart(string pageId, DateTime since, DateTime until);
        object SearchPlaces(string query);        
        int GetPageInsightValueByWeek(string pageId, string type, DateTime since, DateTime until, string duration = "week");
        int GetPostInsightValue(string postId, string pageId, string type);
        int GetTotalPageLikesByWeek(string pageId, DateTime since, DateTime until);
        int GetPostClicksByWeek(string pageId, DateTime since, DateTime until);
    }
}
