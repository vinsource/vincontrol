using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using vincontrol.Crawler.Expressions;
using vincontrol.Crawler.Helpers;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;

namespace vincontrol.Crawler.Entities
{
    public class ReviewPage : Page
    {
        public ReviewPage() : this(new SqlUnitOfWork())
        {
            _doc = new XmlDocument();
            _nsManager = new XmlNamespaceManager(new XmlTextReader("http://www.w3.org/1999/xhtml").NameTable);
        }

        public ReviewPage(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private XmlDocument _doc;
        private XmlNamespaceManager _nsManager;

        #region XmlElements

        [XmlElement("about")]
        public MatchType About;

        [XmlElement("overallScore")]
        public MatchType OverallScore;

        [XmlElement("additionalReview")]
        public AdditionalReviewPage AdditionalReview;

        #endregion

        public override void Execute(ReviewSite site)
        {
            try
            {
                if (String.IsNullOrEmpty(this.Url)) return;

                _doc = site.DownloadDocument(this.Url);
                _nsManager = site.GetXmlNamespaceManager(_doc);
                if (!_nsManager.HasNamespace("ns")) _nsManager = null;

                var dealerId = this.DealerId;
                var url = this.Url;
                var about = this.About.GetString(_doc, _nsManager);
                decimal convertedOverallScore;
                decimal.TryParse(this.OverallScore.GetString(_doc, _nsManager), out convertedOverallScore);

                // Storing dealer' review
                var existingDealerReview = Save(dealerId, site.Id, url, about, convertedOverallScore, this.CategoryId);
                var latestUserReview = UnitOfWork.Review.GetLatestUserReview(existingDealerReview.DealerReviewId);

                // Check to see if the page has additional dealer' reviews
                if (this.HasAdditionalDealerReview)
                {
                    GettingAdditionalDealerReview(existingDealerReview.DealerReviewId, _doc, _nsManager);
                }

                // Getting user' reviews
                site.ReviewPosts.Execute(site, this, existingDealerReview, latestUserReview);
            }
            catch (Exception ex)
            {
                UnitOfWork.VinsocialCommon.AddLog(site.Name, 0, ex.Message, ex.StackTrace);
                Console.WriteLine("** Dealer Review: ERROR {0}", ex.Message);
            }
        }

        private void GettingAdditionalDealerReview(int dealerReviewId, XmlDocument doc, XmlNamespaceManager nsManager)
        {
            decimal customerService;
            decimal.TryParse(this.AdditionalReview.CustomerService.GetString(doc, nsManager), out customerService);

            decimal qualityOfWork;
            decimal.TryParse(this.AdditionalReview.QualityOfWork.GetString(doc, nsManager), out qualityOfWork);

            decimal friendliness;
            decimal.TryParse(this.AdditionalReview.Friendliness.GetString(doc, nsManager), out friendliness);

            decimal overallExperience;
            decimal.TryParse(this.AdditionalReview.OverallExperience.GetString(doc, nsManager), out overallExperience);

            decimal pricing;
            decimal.TryParse(this.AdditionalReview.Pricing.GetString(doc, nsManager), out pricing);

            decimal buyingProcess;
            decimal.TryParse(this.AdditionalReview.BuyingProcess.GetString(doc, nsManager), out buyingProcess);

            decimal overallFacilities;
            decimal.TryParse(this.AdditionalReview.OverallFacilities.GetString(doc, nsManager), out overallFacilities);

            SaveAdditionalReview(dealerReviewId, customerService, qualityOfWork, friendliness, overallExperience, pricing, buyingProcess, overallFacilities);
        }

        private DealerReview Save(int dealerId, int siteId, string url, string about, decimal overallScore, int categoryId)
        {
            var existingDealerReview = UnitOfWork.Review.GetDealerReview(dealerId, siteId, categoryId);
            if (existingDealerReview == null)
            {
                var newDealerReview = new DealerReview()
                                          {
                                              DealerId = dealerId,
                                              SiteId = siteId,
                                              Url = url,
                                              OverallScore = overallScore,
                                              About = about,
                                              DateStamp = Service.GetUSTime(DateTime.Now),
                                              CreatedDate = Service.GetUSTime(DateTime.Now),
                                              //CategoryReviewId = categoryId
                                          };
                UnitOfWork.Review.AddDealerReview(newDealerReview);
                UnitOfWork.CommitVinreviewModel();
                existingDealerReview = newDealerReview;

                Console.WriteLine("** Dealer Review: Add new {0}", url);
            }
            else
            {
                existingDealerReview.DealerId = dealerId;
                //existingDealerReview.CategoryReviewId = categoryId;
                existingDealerReview.SiteId = siteId;
                existingDealerReview.Url = url;
                existingDealerReview.OverallScore = overallScore;
                existingDealerReview.About = about;
                existingDealerReview.DateStamp = Service.GetUSTime(DateTime.Now);
                UnitOfWork.CommitVinreviewModel();

                Console.WriteLine("** Dealer Review: Update {0}", url);
            }

            return existingDealerReview;
        }

        private void SaveAdditionalReview(int dealerReviewId, decimal customerService, decimal qualityOfWork, decimal friendliness, decimal overallExperience, decimal pricing, decimal buyingProcess, decimal overallFacilities)
        {
            var existingAdditionalDealerReview = UnitOfWork.Review.GetAdditionalDealerReview(dealerReviewId);
            if (existingAdditionalDealerReview == null)
            {
                var newAdditionalDealerReview = new AdditionalDealerReview()
                                                    {
                                                        DealerReviewId = dealerReviewId,
                                                        CustomerService = customerService,
                                                        QualityOfWork = qualityOfWork,
                                                        Friendliness = friendliness,
                                                        OverallExperience = overallExperience,
                                                        Pricing = pricing,
                                                        BuyingProcess = buyingProcess,
                                                        OverallFacilities = overallFacilities
                                                    };
                UnitOfWork.Review.AddAdditionalDealerReview(newAdditionalDealerReview);
                UnitOfWork.CommitVinreviewModel();
                
                //existingAdditionalDealerReview = newAdditionalDealerReview;
            }
            else
            {
                existingAdditionalDealerReview.DealerReviewId = dealerReviewId;
                existingAdditionalDealerReview.CustomerService = customerService;
                existingAdditionalDealerReview.QualityOfWork = qualityOfWork;
                existingAdditionalDealerReview.Friendliness = friendliness;
                existingAdditionalDealerReview.OverallExperience = overallExperience;
                existingAdditionalDealerReview.Pricing = pricing;
                existingAdditionalDealerReview.BuyingProcess = buyingProcess;
                existingAdditionalDealerReview.OverallFacilities = overallFacilities;
                UnitOfWork.CommitVinreviewModel();
            }
        }
    }
}
