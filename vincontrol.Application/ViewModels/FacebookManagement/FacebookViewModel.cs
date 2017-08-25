using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Application.ViewModels.FacebookManagement
{
    [DataContract]
    public class FacebookPostViewModel
    {
        public FacebookPostViewModel() { SharedWiths = new List<SelectListItem>(); }

        public FacebookPostViewModel(FBPost obj) 
        {
            PostId = obj.FBPostId;
            RealPostId = obj.RealPostId.GetValueOrDefault();
            DealerId = obj.DealerId;
            UserId = obj.UserId;
            SharedWithId = obj.FBSharedWithId;
            SharedWithName = obj.FBSharedWith.Name;
            Content = obj.Content;
            Link = obj.Link ?? string.Empty;
            Picture = obj.Picture ?? string.Empty;
            PublishDate = obj.PublishDate;
            Status = obj.Status ?? string.Empty;
            LocationId = obj.LocationId;
            LocationName = obj.LocationName;
        }

        [DataMember]
        public int PostId { get; set; }
        [DataMember]
        public long RealPostId { get; set; }
        public string RealPostUrl { get; set; }
        public int InventoryId { get; set; }
        public int InventoryStatus { get; set; }
        public int DealerId { get; set; }
        public int UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        public int SharedWithId { get; set; }
        public string SharedWithName { get; set; }
        [DataMember]
        public string Content { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public string Icon { get; set; }
        public string Place { get; set; }
        public string Tags { get; set; }
        public string Type { get; set; }
        [DataMember]
        public int TotalReach { get; set; }
        public int PaidReach { get; set; }
        [DataMember]
        public int Like { get; set; }        
        [DataMember]
        public DateTime PublishDate { get; set; }
        [DataMember]
        public string StrPublishDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IList<SelectListItem> SharedWiths { get; set; }
        public string Status { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string AccessToken { get; set; }
    }

    public class FacebookPostCalendarViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public string url { get; set; }
        public string className { get; set; }
    }

    public class FacebookCredentialViewModel
    {
        public FacebookCredentialViewModel(){}

        public FacebookCredentialViewModel(FBCredential obj)
        {
            FBCredentialId = obj.FBCredentialId;
            DealerId = obj.DealerId;
            Category = obj.Category;
            Name = obj.Name;
            Email = obj.Email;
            Password = obj.Password;
            PageId = obj.PageId.GetValueOrDefault();
            PageUrl = obj.PageUrl;
            About = obj.About;
            Phone = obj.Phone;
            Website = obj.Website;
            AccessToken = obj.AccessToken;
            ExpiredDate = obj.ExpiredDate;
        }

        public int FBCredentialId { get; set; }
        public int DealerId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long PageId { get; set; }
        public string PageUrl { get; set; }
        public string AccessToken { get; set; }
        public string About { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public bool HasAdminRole { get; set; }
        public DateTime ExpiredDate { get; set; }
    }

    [DataContract]
    public class FacebokPersonalInfo
    {
        public int DealerId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        [DataMember]
        public int Likes { get; set; }
        [DataMember]
        public int Fans { get; set; }
        [DataMember]
        public int TotalClicks { get; set; }
        public int TalkingAbout { get; set; }
        public string About { get; set; }
        public string Category { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Link { get; set; }
        [DataMember]
        public string ValuationDate { get; set; }
    }

    public class FacebookInsightChart
    {
        public FacebookInsightChart()
        {
            PageLike = new FacebookPageLike();
            PostReach = new FacebookPostReach();
            Engagement = new FacebookEngagement();
        }

        public FacebookPageLike PageLike { get; set; }
        public FacebookPostReach PostReach { get; set; }
        public FacebookEngagement Engagement { get; set; }
    }

    public class FacebookPageLike
    {
        public int TotalPageLikes { get; set; }
        public double PercentageOfTotalPageLikes { get; set; }
        public int NewPageLikes { get; set; }
        public double PercentageOfNewPageLikes { get; set; }
    }

    public class FacebookPostReach
    {
        public int TotalReach { get; set; }
        public double PercentageOfTotalReach { get; set; }
        public int PostReach { get; set; }
        public double PercentageOfPostReach { get; set; }
    }

    public class FacebookEngagement
    {
        public int PeopleEngaged { get; set; }
        public double PercentageOfPeopleEngaged { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int Shares { get; set; }
        public int PostClicks { get; set; }
    }

    [DataContract]
    public class FacebookPostSummaryByDay 
    {
        [DataMember]
        public string Day { get; set; }
        [DataMember]
        public string NumberOfPosts { get; set; }
        [DataMember]
        public int Order { get; set; }
    }
}
