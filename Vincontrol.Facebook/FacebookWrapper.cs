using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Application.Forms.FacebookManagement;
using vincontrol.Application.ViewModels.FacebookManagement;
using vincontrol.ConfigurationManagement;
using vincontrol.Constant;

namespace Vincontrol.Facebook
{
    public class FacebookWrapper : IFacebookWrapper
    {        
        public struct GrantType
        {
            public const string ClientCredentials = "client_credentials";
            public const string ExchangeToken = "fb_exchange_token";
        }
        
        private const string Scope = "manage_pages,offline_access,publish_stream,offline_access";
        private FacebookClient _fbClient;
        private IFacebookManagementForm _facebookManagement;

        public FacebookWrapper()
        {
            //if (_fbClient == null) _fbClient = InitializeFacebookClient();
            _facebookManagement = new FacebookManagementForm();
        }

        #region IFacebookWrapper' Members

        private void InitializeFacebookClient()
        {
            if (_fbClient == null) _fbClient = new FacebookClient(ConfigurationHandler.FacebookPageToken) { AppId = ConfigurationHandler.FacebookAppId, AppSecret = ConfigurationHandler.FacebookAppSecret };
        }

        private void InitializeFacebookClient(string accessToken)
        {
            InitializeFacebookClient();
            _fbClient.AccessToken = accessToken;
        }

        public object SearchPlaces(string query)
        {
            var searchParams = new Dictionary<string, object>();
            searchParams.Add("q", query);
            searchParams.Add("type", "place");
            
            InitializeFacebookClient();
            var places = _fbClient.Get("/search", searchParams);
            return places;
        }

        public long Post(string pageId, FacebookPostViewModel model)
        {
            dynamic parameters = new ExpandoObject();
            parameters.message = model.Content;
            parameters.link = model.Link ?? string.Empty;
            parameters.place = model.LocationId ?? string.Empty;

            try
            {
                InitializeFacebookClient(model.AccessToken);
                var result = _fbClient.Post(String.Format("/{0}/feed", pageId), parameters);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(result.ToString());
                return Convert.ToInt64(jsonResult.First.First.ToString().Split('_')[1]);
            }
            catch (FacebookOAuthException)
            {
                
            }
            catch (Exception)
            {
            }

            return 0;
        }

        public long PostPhoto(string pageId, FacebookPostViewModel model)
        {
            dynamic parameters = new ExpandoObject();
            parameters.link = model.Link ?? string.Empty;
            parameters.place = model.LocationId ?? string.Empty;
            parameters.caption = model.Content;
            
            try
            {
                foreach (var item in model.Picture.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList())
                {
                    byte[] photo = File.ReadAllBytes(item);
                    var mediaObject = new FacebookMediaObject
                    {
                        FileName = Path.GetFileNameWithoutExtension(item),
                        ContentType = "image/jpeg"
                    };
                    mediaObject.SetValue(photo);
                    parameters.source = mediaObject;
                }
                InitializeFacebookClient(model.AccessToken);
                var result = _fbClient.Post(String.Format("/{0}/photos", pageId), parameters);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(result.ToString());
                return Convert.ToInt64(jsonResult.First.First.ToString());
            }
            catch (FacebookOAuthException)
            {
                
            }
            catch (Exception)
            {
            }

            return 0;
        }

        public long PostVideo(string pageId, FacebookPostViewModel model)
        {
            dynamic parameters = new ExpandoObject();
            parameters.link = model.Link ?? string.Empty;
            parameters.place = model.LocationId ?? string.Empty;
            parameters.title = model.Content;
            parameters.desription = model.Content;
            
            try
            {
                byte[] video = File.ReadAllBytes(model.Picture);
                //Stream video = File.OpenRead(model.Picture);
                var extension = Path.GetExtension(model.Picture).Replace(".", "").Replace(",", "");
                var mediaObject = new FacebookMediaObject /*FacebookMediaStream*/
                {
                    FileName = Path.GetFileName(model.Picture) /*Path.GetFileNameWithoutExtension(model.Picture)*/,
                    ContentType = "video/" + extension.ToLower()
                };
                mediaObject.SetValue(video);
                parameters.source = mediaObject;

                InitializeFacebookClient(model.AccessToken);
                var result = _fbClient.Post(String.Format("/{0}/videos", pageId), parameters);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(result.ToString());
                return Convert.ToInt64(jsonResult.First.First.ToString());
            }
            catch (FacebookOAuthException)
            {

            }
            catch (Exception)
            {
            }

            return 0;
        }
        
        public void Delete(string pageId, string realPostId)
        {
            try
            {
                InitializeFacebookClient();
                _fbClient.Delete(String.Format("/{0}_{1}", pageId, realPostId));
            }
            catch (FacebookOAuthException)
            {
                
            }
            catch (Exception)
            {
            }
        }

        //NOTE:
        //Log into Facebook with alers@vincontrol.com/jeff1451
        //Go to Graph Explorer Tool https://developers.facebook.com/tools/explorer
        //Change Application to VINSocial
        //Then, get Access Token. It's Debug Token
        //Example URL to get long live token
        //https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id=1412310822338075&client_secret=8840773be7acdda32558472f5c15bd56&fb_exchange_token={3}
        public string GetLongLiveToken(string debugToken)
        {
            var url =
                String.Format(
                    "{0}/oauth/access_token?grant_type=fb_exchange_token&client_id={1}&client_secret={2}&fb_exchange_token={3}",
                    ConfigurationHandler.FacebookGraphAPI, ConfigurationHandler.FacebookAppId,
                    ConfigurationHandler.FacebookAppSecret, debugToken);
            var content = WebHandler.DownloadContent(url);

            return content.Split('&')[0].Split('=')[1];
        }

        public void GetShortTokens()
        {
            var url = String.Format("{0}/{1}/accounts?access_token={2}", ConfigurationHandler.FacebookGraphAPI,
                ConfigurationHandler.FacebookPersonalId, ConfigurationHandler.FacebookPageToken);
            var content = WebHandler.DownloadContent(url);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
            if (jsonObj != null && jsonObj["data"] != null && jsonObj["data"].Children().Any())
            {
                foreach (var subresult in jsonObj["data"].Children())
                {
                    var credential = new FacebookCredentialViewModel()
                    {
                        Category = subresult.Value<string>("category"),
                        Name = subresult.Value<string>("name"),
                        AccessToken = subresult.Value<string>("access_token"),
                        PageId = Convert.ToInt64(subresult.Value<string>("id"))
                    };
                }
            }
        }

        public string GetShortToken(int dealerId)
        {
            
            var newToken = string.Empty;
            var url = String.Format("{0}/{1}/accounts?access_token={2}", ConfigurationHandler.FacebookGraphAPI,
                ConfigurationHandler.FacebookPersonalId, ConfigurationHandler.FacebookPageToken);
            var content = WebHandler.DownloadContent(url);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
            if (jsonObj != null && jsonObj["data"] != null && jsonObj["data"].Children().Any())
            {
                foreach (var subresult in jsonObj["data"].Children())
                {
                    var newCredential = new FacebookCredentialViewModel()
                    {
                        Category = subresult.Value<string>("category"),
                        Name = subresult.Value<string>("name"),
                        AccessToken = subresult.Value<string>("access_token"),
                        PageId = Convert.ToInt64(subresult.Value<string>("id")),
                        DealerId = dealerId
                        
                    };
                        
                    newToken = newCredential.AccessToken;

                    break;
                }
            }

            return newToken;
        }

        public string GetShortToken(string pageId)
        {
            var newToken = string.Empty;
            var url = String.Format("{0}/{1}/accounts?access_token={2}", ConfigurationHandler.FacebookGraphAPI,
                ConfigurationHandler.FacebookPersonalId, ConfigurationHandler.FacebookPageToken);
            var content = WebHandler.DownloadContent(url);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
            if (jsonObj != null && jsonObj["data"] != null && jsonObj["data"].Children().Any())
            {
                foreach (var subresult in jsonObj["data"].Children())
                {
                    if (pageId.Equals((subresult.Value<string>("id"))))
                    {
                        var newCredential = new FacebookCredentialViewModel()
                        {
                            Category = subresult.Value<string>("category"),
                            Name = subresult.Value<string>("name"),
                            AccessToken = subresult.Value<string>("access_token"),
                            PageId = Convert.ToInt64(subresult.Value<string>("id"))
                        };

                        newToken = newCredential.AccessToken;
                    }
                }
            }

            return newToken;
        }

        public FacebokPersonalInfo GetPersonalInfo(string id)
        {
            var url = String.Format("{0}/{1}", ConfigurationHandler.FacebookGraphAPI, id);
            var content = WebHandler.DownloadContent(url);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
            return jsonObj != null ? new FacebokPersonalInfo() {
                Id = jsonObj["id"] != null ? Convert.ToInt64(jsonObj.Value<string>("id")) : 0,
                Name = jsonObj["name"] != null ? (jsonObj.Value<string>("name")) : string.Empty,
                Likes = jsonObj["likes"] != null ? Convert.ToInt32(jsonObj.Value<string>("likes")) : 0,
                TalkingAbout = jsonObj["talking_about_count"] != null ? Convert.ToInt32(jsonObj.Value<string>("talking_about_count")) : 0,
                Category = jsonObj["category"] != null ? (jsonObj.Value<string>("category")) : string.Empty,
                About = jsonObj["about"] != null ? (jsonObj.Value<string>("about")) : string.Empty,
                Phone = jsonObj["phone"] != null ? (jsonObj.Value<string>("phone")) : string.Empty,
                Website = jsonObj["website"] != null ? (jsonObj.Value<string>("website")) : string.Empty,
                Link = jsonObj["link"] != null ? (jsonObj.Value<string>("link")) : string.Empty,
            } : null;
        }

        public List<FacebookPostViewModel> GetTopPosts(string pageId, bool viewInsight = false, bool includeUserName = false)
        {
            //var accessToken = GetShortToken("118949304788934") /*GetShortToken(dealerId)*/;
            var url = String.Format("{0}/{1}/posts?access_token={2}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken);
            var content = WebHandler.DownloadContent(url);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
            var list = new List<FacebookPostViewModel>();
            foreach (var result in jsonObj["data"].Children())
            {
                var type = result["type"] == null ? "" : result.Value<string>("type");
                var message = result["message"] == null ? (result["story"] == null ? (result["name"] == null ? type : result.Value<string>("name")) : result.Value<string>("story")) 
                                                        : result.Value<string>("message");
                //if (((!type.Equals("video") && !type.Equals("photo") && !type.Equals("link")) || String.IsNullOrEmpty(message))) continue;

                var createdTime = result["created_time"] == null ? "" : result.Value<string>("created_time");
                var updatedTime = result["updated_time"] == null ? "" : result.Value<string>("updated_time");
                if (string.IsNullOrEmpty(createdTime)) createdTime = updatedTime;

                list.Add(new FacebookPostViewModel()
                {                    
                    RealPostId = Convert.ToInt64(result.Value<string>("id").Split('_')[1]),
                    //RealPostUrl = String.Format("https://www.facebook.com/{0}", (result.Value<string>("id"))),
                    RealPostUrl = String.Format("https://www.facebook.com/{0}/posts/{1}", Convert.ToInt64(result.Value<string>("id").Split('_')[0]), Convert.ToInt64(result.Value<string>("id").Split('_')[1])),
                    TotalReach = !viewInsight ? 0 : GetPostInsightValue(result.Value<string>("id").Split('_')[1], result.Value<string>("id").Split('_')[0], "post_impressions_unique"),
                    Like = !viewInsight ? 0 : GetTotalPostLikes(result.Value<string>("id").Split('_')[1], result.Value<string>("id").Split('_')[0]),
                    Content = message,
                    Icon = result["icon"] == null ? "" : result.Value<string>("icon"),
                    Picture = result["picture"] == null ? "" : result.Value<string>("picture"),
                    Type = type,
                    PublishDate = ParseDate(createdTime),
                    StrPublishDate = ParseDate(createdTime).ToString("MM/dd/yyyy hh:mm tt"),
                    UpdatedDate = ParseDate(updatedTime),
                    UserName = !includeUserName ? string.Empty : _facebookManagement.GetUserNameByRealPostId(Convert.ToInt64(result.Value<string>("id").Split('_')[1]))
                });
            }

            return list;
        }

        public FacebookInsightChart InitializeInsightChart(string pageId, DateTime since, DateTime until) 
        {
            var model = new FacebookInsightChart()
                {
                    PageLike = new FacebookPageLike()
                    {
                        TotalPageLikes = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageFan, since.AddDays(-1), until.AddDays(-1), string.Empty),
                        NewPageLikes = GetTotalPageLikesByWeek(pageId, since, until)
                    },
                    PostReach = new FacebookPostReach()
                    {
                        TotalReach = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageImpressionUnique, since, until),
                        PostReach = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PagePostImpressionUnique, since, until)
                    },
                    Engagement = new FacebookEngagement()
                    {
                        PeopleEngaged = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageEngagedUser, since, until),
                        PostClicks = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageConsumption, since, until)
                    }
                };
                        
            var lastUntil = since.AddDays(-1);
            var lastSince = lastUntil.AddDays(-6);
            var lastModel = new FacebookInsightChart()
            {
                PageLike = new FacebookPageLike()
                {
                    TotalPageLikes = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageFan, lastSince.AddDays(-2), lastUntil.AddDays(-2), string.Empty),
                    NewPageLikes = GetTotalPageLikesByWeek(pageId, since, until)
                },
                PostReach = new FacebookPostReach()
                {
                    TotalReach = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageImpressionUnique, lastSince, lastUntil),
                    PostReach = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PagePostImpressionUnique, lastSince, lastUntil)
                },
                Engagement = new FacebookEngagement()
                {
                    PeopleEngaged = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageEngagedUser, lastSince, lastUntil),
                    PostClicks = GetPageInsightValueByWeek(pageId, Constanst.FBInsightType.PageConsumption, lastSince, lastUntil)
                }
            };

            model.PageLike.PercentageOfTotalPageLikes = lastModel.PageLike.TotalPageLikes.Equals(0) ? Convert.ToDouble(model.PageLike.TotalPageLikes) * 100 : Math.Round((Convert.ToDouble(model.PageLike.TotalPageLikes - lastModel.PageLike.TotalPageLikes) / lastModel.PageLike.TotalPageLikes) * 100, 1);
            model.PageLike.PercentageOfNewPageLikes = lastModel.PageLike.NewPageLikes.Equals(0) ? Convert.ToDouble(model.PageLike.NewPageLikes) * 100 : Math.Round((Convert.ToDouble(model.PageLike.NewPageLikes - lastModel.PageLike.NewPageLikes) / lastModel.PageLike.NewPageLikes) * 100, 1);
            model.PostReach.PercentageOfTotalReach = lastModel.PostReach.TotalReach.Equals(0) ? Convert.ToDouble(model.PostReach.TotalReach) * 100 : Math.Round((Convert.ToDouble(model.PostReach.TotalReach - lastModel.PostReach.TotalReach) / lastModel.PostReach.TotalReach) * 100, 1);
            model.PostReach.PercentageOfPostReach = lastModel.PostReach.PostReach.Equals(0) ? Convert.ToDouble(model.PostReach.PostReach) * 100 : Math.Round((Convert.ToDouble(model.PostReach.PostReach - lastModel.PostReach.PostReach) / lastModel.PostReach.PostReach) * 100, 1);
            model.Engagement.PercentageOfPeopleEngaged = lastModel.Engagement.PeopleEngaged.Equals(0) ? Convert.ToDouble(model.Engagement.PeopleEngaged) * 100 : Math.Round((Convert.ToDouble(model.Engagement.PeopleEngaged - lastModel.Engagement.PeopleEngaged) / lastModel.Engagement.PeopleEngaged) * 100, 1);
            model.Engagement.Likes = model.PageLike.NewPageLikes + lastModel.PageLike.NewPageLikes;
            
            return model;
        }
        
        public int GetPageInsightValueByWeek(string pageId, string type, DateTime since, DateTime until, string duration = "week")
        {
            try
            {                
                var url = String.Format("{0}/{1}/insights/{2}?access_token={3}&since={4}&until={5}", ConfigurationHandler.FacebookGraphAPI, pageId, type, ConfigurationHandler.FacebookPageToken, ToUnixTimespan(since), ToUnixTimespan(until));
                switch (duration)
                {
                    case "week":
                        url += "&period=week";
                        break;
                    default:
                        break;
                }

                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["data"].Children().Last()["values"].Children().Last().Value<string>("value"));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int GetPostInsightValue(string postId, string pageId, string type)
        {
            try
            {
                var url = String.Format("{0}/{1}_{2}/insights/{3}?access_token={4}", ConfigurationHandler.FacebookGraphAPI, pageId, postId, type, ConfigurationHandler.FacebookPageToken);
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["data"].Children().Last()["values"].Children().Last().Value<string>("value"));
            }
            catch (Exception)
            {
                return 0;
            } 
        }

        #endregion

        #region Private Methods

        private int GetTotalPostLikes(string postId, string pageId)
        {
            try
            {
                var url = String.Format("{0}/{1}_{2}/likes?summary=1&access_token={3}", ConfigurationHandler.FacebookGraphAPI, pageId, postId, ConfigurationHandler.FacebookPageToken);
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["summary"].Children().First().First().Value<string>());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetTotalPageLikes(string pageId)
        {
            try
            {
                var url = String.Format("{0}/{1}/insights/page_fans?access_token={2}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken);
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["data"].Children().Last()["values"].Children().Last().Value<string>("value"));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int GetTotalPageLikesByWeek(string pageId, DateTime since, DateTime until)
        {
            try
            {
                var url = String.Format("{0}/{1}/insights/page_fan_adds_unique?period=day&access_token={2}&since={3}&until={4}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken, ToUnixTimespan(since), ToUnixTimespan(until));
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                
                int likes = 0;
                foreach (var item in jsonObj["data"].Children().Last()["values"].Children())
                {
                    likes += Convert.ToInt32(item.Value<string>("value"));
                }

                return likes;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetTotalReach(string pageId)
        {
            try
            {
                var url = String.Format("{0}/{1}/insights/page_impressions_unique?access_token={2}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken);
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["data"].Children().Last()["values"].Children().Last().Value<string>("value"));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetPostReach(string pageId)
        {
            try
            {
                var url = String.Format("{0}/{1}/insights/page_posts_impressions_unique?access_token={2}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken);
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["data"].Children().Last()["values"].Children().Last().Value<string>("value"));
            }
            catch (Exception)
            {
                return 0;
            }
        }
        
        private int GetPeopleEngaged(string pageId)
        {
            try
            {
                var url = String.Format("{0}/{1}/insights/page_engaged_users?access_token={2}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken);
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["data"].Children().Last()["values"].Children().Last().Value<string>("value"));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int GetPostClicks(string pageId)
        {
            try
            {
                var url = String.Format("{0}/{1}/insights/page_consumptions?access_token={2}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken);
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                return Convert.ToInt32(jsonObj["data"].Children().Last()["values"].Children().Last().Value<string>("value"));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int GetPostClicksByWeek(string pageId, DateTime since, DateTime until)
        {
            try
            {
                var url = String.Format("{0}/{1}/insights/page_consumptions?period=day&access_token={2}&since={3}&until={4}", ConfigurationHandler.FacebookGraphAPI, pageId, ConfigurationHandler.FacebookPageToken, ToUnixTimespan(since), ToUnixTimespan(until));
                var content = WebHandler.DownloadContent(url);
                var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
                int clicks = 0;
                foreach (var item in jsonObj["data"].Children().Last()["values"].Children())
                {
                    clicks += Convert.ToInt32(item.Value<string>("value"));
                }

                return clicks;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private string GetNextPage(JObject jsonObj)
        {
            string nextPage = string.Empty;
            if (jsonObj["paging"] != null && jsonObj["paging"].Any())
            {
                nextPage = (((JProperty)(jsonObj["paging"].Last))).Value.ToString();
                if (nextPage.Contains("\""))
                {
                    nextPage = nextPage.Replace("\"", string.Empty);
                }
            }

            return nextPage;
        }

        private DateTime ParseDate(string date)
        {
            try
            {
                //return DateTime.ParseExact(date.Substring(0, 10), "yyyy-MM-dd", null);
                return DateTime.Parse(date, null, DateTimeStyles.RoundtripKind);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        //private long ToUnixTimespan(DateTime date)
        //{
        //    TimeSpan tspan = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        //    return (long)Math.Truncate(tspan.TotalSeconds);
        //}
        public long ToUnixTimespan(DateTime _DateTime)
        {
            TimeSpan _UnixTimeSpan = (_DateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)_UnixTimeSpan.TotalSeconds;
        }

        #endregion
    }
}
