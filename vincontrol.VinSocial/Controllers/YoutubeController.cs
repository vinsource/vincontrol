using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;

using DotNetOpenAuth.OAuth2;

using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util;
using Google.Apis.Youtube.v3;
using Google.Apis.Youtube.v3.Data;
using vincontrol.VinSocial.YoutubeHelper;

namespace vincontrol.VinSocial.Controllers
{
    public class YoutubeConstant
    {
        public static class Category
        {
            public const int AutosAndVehicles = 2;
            public const int Comedy = 23;
            public const int Education = 27;
            public const int Entertainment = 24;
            public const int FilmAndAnimation = 1;
            public const int Gaming = 20;
            public const int HowtoAndStyle = 26;
            public const int Music = 10;
            public const int NewsAndPolitics = 25;
            public const int NonprofitsAndActivism = 29;
            public const int PeopleAndBlogs = 22;
            public const int PetsAndAnimals = 15;
            public const int ScienceAndTechnology = 28;
            public const int Sports = 17;
            public const int TravelAndEvents = 19;
        }

        public static class PrivacyStatus
        {
            public const string Public = "public";
            public const string Unlisted = "unlisted";
            public const string Private = "private";
        }

        public static class ContentType
        {
            public const string Video = "video/*";
            public const string Text = "text/plain";
        }

        public static class PartType
        {
            public const string Snippet = "snippet, status";
        }
    }

    public class YoutubeController : Controller
    {
        private static string _clientId = "844241159218.apps.googleusercontent.com";
        private static string _clientSecret = "cWuidQBssTs2OCLpWepnr5bD";
        private static string _token = "AIzaSyABfH03KWHX802fbmqWUXckORVoVQ9a0xA";

        private YouTubeService _youtube;
        private IAuthorizationState _refreshToken;

        public YoutubeController()
        {
            //_youtube = BuildService();
            //_refreshToken = new AuthorizationState()
            //{
            //    RefreshToken = _token
            //};
        }

        public ActionResult Index()
        {
            //UploadVideo();
            return View();
        }
        
        #region Private Methods

        private void GetTopVideos()
        {
            var channelsListRequest = _youtube.Channels.List("contentDetails");
            channelsListRequest.Mine = true;
        }

        private void UploadVideo()
        {
            var video = new Video
                            {
                                Snippet = new VideoSnippet
                                              {
                                                  Title = "Title",
                                                  Description = "Description",
                                                  Tags = new string[] {"", ""},
                                                  CategoryId = YoutubeConstant.Category.PeopleAndBlogs.ToString()
                                              },
                                Status = new VideoStatus { PrivacyStatus = YoutubeConstant.PrivacyStatus.Private }
                            };

            var fileStream = new FileStream(@"C:\Users\vincontrol\Downloads\Video\Car 02.mp4", FileMode.Open);
            VideosResource.InsertMediaUpload insertRequest = _youtube.Videos.Insert(video, YoutubeConstant.PartType.Snippet, fileStream, YoutubeConstant.ContentType.Video);
            insertRequest.ProgressChanged += InsertRequest_ProgressChanged;
            insertRequest.ResponseReceived += InsertRequest_ResponseReceived;

            insertRequest.Upload();
        }

        private static void InsertRequest_ResponseReceived(Video obj)
        {
            // obj.ID gives you the ID of the Youtube video.
            // you can access the video from
            // http://www.youtube.com/watch?v={obj.ID}
        }

        private static void InsertRequest_ProgressChanged(IUploadProgress obj)
        {
            // You can handle several status messages here.
            switch (obj.Status)
            {
                case UploadStatus.Failed:
                    break;
                case UploadStatus.Completed:
                    break;
                default:
                    break;
            }
        }

        private YouTubeService BuildService()
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description)
            {
                ClientIdentifier = _clientId,
                ClientSecret = _clientSecret
            };

            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);

            var service = new YouTubeService((new BaseClientService.Initializer()
            {
                Authenticator = auth, 
                //ApiKey = _token
            }));

            service.HttpClient.Timeout = TimeSpan.FromSeconds(360);
            return service;
        }

        private IAuthorizationState GetAuthorization(NativeApplicationClient client)
        {
            client.RefreshToken(_refreshToken);
            return _refreshToken;
        }

        //private IAuthorizationState GetAuthorization(NativeApplicationClient client)
        //{
        //    var storage = MethodBase.GetCurrentMethod().DeclaringType.ToString();

        //    IAuthorizationState state = AuthorizationMgr.GetCachedRefreshToken(storage, _token);
        //    if (state != null)
        //    {
        //        client.RefreshToken(state);
        //    }
        //    else
        //    {
        //        state = AuthorizationMgr.RequestNativeAuthorization(client, YouTubeService.Scopes.YoutubeUpload.GetStringValue());
        //        AuthorizationMgr.SetCachedRefreshToken(storage, _token, state);
        //    }

        //    return state;
        //}

        #endregion
    }
}
