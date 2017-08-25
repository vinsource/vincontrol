using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GooglePlusLib.NET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vincontrol.Crawler.Expressions;
using vincontrol.Crawler.Helpers;
using vincontrol.Data;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Crawler.Entities
{
    public class ReviewPost : ObjectBase
    {
        public ReviewPost() : this(new SqlUnitOfWork())
        {
            _doc = new XmlDocument();
            _nsManager = new XmlNamespaceManager(new XmlTextReader("http://www.w3.org/1999/xhtml").NameTable);
        }

        public ReviewPost(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private XmlDocument _doc;
        private XmlNamespaceManager _nsManager;

        #region XmlElements

        [XmlElement("reviewNode")]
        public MatchType ReviewNode;

        [XmlElement("nextPageUrl")]
        public MatchType NextPageUrl;
        
        [XmlElement("reviewRating")]
        public MatchType ReviewRating;

        [XmlElement("reviewAuthor")]
        public MatchType ReviewAuthor;

        [XmlElement("reviewContent")]
        public MatchType ReviewContent;

        [XmlElement("reviewDate")]
        public MatchType ReviewDate;

        [XmlElement("reviewUrl")]
        public MatchType ReviewUrl;

        [XmlElement("reviewDetailContent")]
        public MatchType ReviewDetailContent;

        [XmlElement("additionalReview")]
        public AdditionalReviewPost AdditionalReview;

        #endregion

        #region Execution

        public void Execute(ReviewSite site, Page page, DealerReview currentDealerReview, UserReview latestUserReview)
        {
            try
            {
                var continueToGetNextPage = true;
                var pageUrl = page.Url;
                while (!String.IsNullOrEmpty(pageUrl))
                {
                    _doc = site.DownloadDocument(pageUrl);
                    _nsManager = site.GetXmlNamespaceManager(_doc);
                    if (!_nsManager.HasNamespace("ns")) _nsManager = null;

                    XmlNodeList reviews = _doc.SelectNodes(ReviewNode.Context, _nsManager);
                    foreach (XmlNode review in reviews)
                    {
                        try
                        {
                            DateTime convertedReviewDate;
                            DateTime.TryParse(this.ReviewDate.GetString(review, _nsManager), out convertedReviewDate);

                            // Wrong data
                            if (convertedReviewDate.Equals(DateTime.MinValue)) continue;

                            // Don't scrap pass data
                            if (latestUserReview != null && (convertedReviewDate) < (latestUserReview.ReviewDate)) { continueToGetNextPage = false; break; }

                            var author = this.ReviewAuthor.GetString(review, _nsManager);
                            var comment = this.ReviewContent.GetString(review, _nsManager);

                            decimal convertedRating;
                            decimal.TryParse(this.ReviewRating.GetString(review, _nsManager), out convertedRating);
                            if (page.HasDetailUserReview)
                            {
                                var url = this.ReviewUrl.GetString(review, _nsManager);
                                var doc = site.DownloadDocument(site.CombineUrl(site.Url, url));
                                var nsManager = site.GetXmlNamespaceManager(doc);
                                if (!nsManager.HasNamespace("ns")) nsManager = null;

                                comment = this.ReviewDetailContent.GetString(doc, nsManager);
                            }

                            Console.WriteLine("--- User Review: Processing review of {0} {1} {2}", convertedReviewDate.ToString("MM/dd/yyyy hh:mm"), author, convertedRating);
                            var existingReview = Save(currentDealerReview.DealerReviewId, author, (convertedReviewDate), convertedRating, comment, false, String.IsNullOrEmpty(page.Category) || page.Category.ToLower().Equals("sales") ? 1 : 2);

                            // Check to see if the page has additional user' reviews
                            if (page.HasAdditionalUserReview)
                            {
                                GettingAdditionalUserReview(existingReview.UserReviewId, review, _nsManager);
                            }
                        }
                        catch (Exception ex)
                        {
                            //UnitOfWork.Review.AddLog(site.Name, currentDealerReview.DealerReviewId, ex.Message, ex.StackTrace);
                            //Console.WriteLine("--- User Review: ERROR {0}", ex.Message);
                        }
                    }

                    var nextPage = this.NextPageUrl.GetString(_doc, _nsManager);
                    pageUrl = continueToGetNextPage && !String.IsNullOrEmpty(nextPage) ? site.CombineUrl(site.Url, nextPage) : null;
                    Console.WriteLine("--- User Review: Getting next page {0}", pageUrl);
                }

                if (site.IsAjaxRequestSite(site.Name))
                {
                    ExecuteAjaxRequestSite(site, currentDealerReview);
                }

            }
            catch (Exception ex)
            {
                UnitOfWork.VinsocialCommon.AddLog(site.Name, currentDealerReview.DealerReviewId, ex.Message, ex.StackTrace);
                Console.WriteLine("--- User Review: ERROR {0}", ex.Message);
            }
        }

        #endregion

        #region Private Methods

        private UserReview Save(int dealerReviewId, string author, DateTime reviewDate, decimal rating, string comment, bool isFilterred, int categoryId)
        {
            var existingReview = UnitOfWork.Review.GetUserReview(dealerReviewId, author, reviewDate, rating);
            if (existingReview == null)
            {
                var newReview = new UserReview()
                                    {
                                        DealerReviewId = dealerReviewId,
                                        Author = author,
                                        ReviewDate = reviewDate,
                                        Rating = rating,
                                        Comment = comment,
                                        DateStamp = DateTime.Now,
                                        IsFilterred = isFilterred,
                                        CategoryReviewId = categoryId
                                    };
                UnitOfWork.Review.AddUserReview(newReview);
                UnitOfWork.CommitVinreviewModel();
                existingReview = newReview;
            }

            return existingReview;
        }

        private void GettingAdditionalUserReview(int userReviewId, XmlNode element, XmlNamespaceManager nsManager)
        {
            decimal customerService;
            decimal.TryParse(this.AdditionalReview.CustomerService.GetString(element, nsManager), out customerService);

            decimal qualityOfWork;
            decimal.TryParse(this.AdditionalReview.QualityOfWork.GetString(element, nsManager), out qualityOfWork);

            decimal friendliness;
            decimal.TryParse(this.AdditionalReview.Friendliness.GetString(element, nsManager), out friendliness);

            decimal overallExperience;
            decimal.TryParse(this.AdditionalReview.OverallExperience.GetString(element, nsManager), out overallExperience);

            decimal pricing;
            decimal.TryParse(this.AdditionalReview.Pricing.GetString(element, nsManager), out pricing);

            decimal buyingProcess;
            decimal.TryParse(this.AdditionalReview.BuyingProcess.GetString(element, nsManager), out buyingProcess);

            decimal overallFacilities;
            decimal.TryParse(this.AdditionalReview.OverallFacilities.GetString(element, nsManager), out overallFacilities);

            var reasonForVisit = this.AdditionalReview.ReasonForVisit.GetString(element, nsManager);

            bool recommentThisDealer;
            bool.TryParse(this.AdditionalReview.RecommentThisDealer.GetString(element, nsManager), out recommentThisDealer);

            bool purchasedAVehicle;
            bool.TryParse(this.AdditionalReview.PurchasedAVehicle.GetString(element, nsManager), out purchasedAVehicle);

            UnitOfWork.Review.AddAdditionalUserReview(userReviewId, customerService, qualityOfWork, friendliness, overallExperience, pricing, buyingProcess, overallFacilities, reasonForVisit, recommentThisDealer, purchasedAVehicle);
            UnitOfWork.CommitVinreviewModel();
        }

        private void ExecuteAjaxRequestSite(ReviewSite site, DealerReview currentDealerReview)
        {
            switch (site.Name)
            {
                case ReviewSite.AjaxRequestSite.CitySearch:
                    ExecuteCitySearch();
                    break;
                case ReviewSite.AjaxRequestSite.LocalYahoo:
                    ExecuteLocalYahoo();
                    break;
                case ReviewSite.AjaxRequestSite.PlusGoogle:
                    ExecuteGooglePlus(site, currentDealerReview);
                    break;
                default: break;
            }
        }

        private void ExecuteCitySearch()
        {
            
        }

        private void ExecuteLocalYahoo()
        {

        }

        private void ExecuteGooglePlus(ReviewSite site, DealerReview currentDealerReview)
        {
            try
            {
                var dealer = UnitOfWork.User.GetDealer(currentDealerReview.DealerId);
                var referenceKey = GetGooglePlusReference(site, dealer);
                if (String.IsNullOrEmpty(referenceKey)) return;

                GetGooglePlusReviews(site, currentDealerReview, referenceKey);
                //var apiHelper = new GooglePlusAPIHelper(/*Path.GetFileName(currentDealerReview.Url)*/currentDealerReview.Url.Replace("https://plus.google.com/", "").Replace("/about", ""), "AIzaSyABfH03KWHX802fbmqWUXckORVoVQ9a0xA");
                //var activities = apiHelper.ListActivities();
                //while (!string.IsNullOrEmpty(activities.nextPageToken))
                //{
                //    foreach (GPlusActivity activity in activities.items)
                //    {
                //        //if (activity.plusObject.attachments == null) continue;
                //        Console.WriteLine("--- User Review: Processing review of {0} {1} {2}", activity.published.ToString("MM/dd/yyyy hh:mm"), string.Empty, 0);
                //        Save(currentDealerReview.DealerReviewId, string.Empty, activity.published, 0, string.Empty, false, 1);
                //    }

                //    Console.WriteLine("--- User Review: Getting next page {0}", activities.nextPageToken);
                //    activities = apiHelper.ListActivities(activities.nextPageToken);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- User Review: ERROR {0}", ex.Message);
            }
        }

        private string GetGooglePlusReference(ReviewSite site, Dealer dealer)
        {
            var basicUrl = "https://maps.googleapis.com/maps/api/place";
            var placeUrl = String.Format("{0}/nearbysearch/json?location={1},{2}&radius={3}&name={4}&sensor=true&key={5}", basicUrl, dealer.Lattitude, dealer.Longtitude, 5000000, dealer.Name, ConfigurationManagement.ConfigurationHandler.GoogleAPIKey);
            var content = site.DownloadContent(placeUrl);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
            try
            {
                if (jsonObj != null && jsonObj["results"] != null && jsonObj["results"].Children().Any()) 
                {
                    return jsonObj["results"][0]["reference"].ToString();
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void GetGooglePlusReviews(ReviewSite site, DealerReview currentDealerReview, string referenceKey)
        {
            var basicUrl = "https://maps.googleapis.com/maps/api/place";
            var detailUrl = String.Format("{0}/details/json?sensor=true&key={1}&reference={2}", basicUrl, ConfigurationManagement.ConfigurationHandler.GoogleAPIKey, referenceKey);
            var content = site.DownloadContent(detailUrl);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(content);
            try
            {
                if (jsonObj != null && jsonObj["result"] != null && jsonObj["result"].Children().Any())
                {
                    var overallRating = jsonObj["result"]["rating"] != null ? Convert.ToDecimal(jsonObj["result"]["rating"].ToString()) : 0M;
                    var existingDealerReview = UnitOfWork.Review.GetDealerReview(currentDealerReview.DealerId, site.Id, currentDealerReview.CategoryReviewId.GetValueOrDefault());
                    if (existingDealerReview != null)                    
                    {
                        existingDealerReview.OverallScore = overallRating;
                        UnitOfWork.CommitVinreviewModel();
                        Console.WriteLine("** Dealer Review: Update {0}", currentDealerReview.Url);
                    }

                    foreach (var item in jsonObj["result"]["reviews"].Children())
                    {
                        var date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((item.Value<int>("time")));
                        var comment = item.Value<string>("text");
                        var author = item.Value<string>("author_name");
                        var rating = item.Value<decimal>("rating");
                        Save(currentDealerReview.DealerReviewId, author, date, rating, comment, false, currentDealerReview.CategoryReviewId.GetValueOrDefault());
                    }
                }

            }
            catch (Exception)
            {
                
            }
        }

        #endregion
    }
}
