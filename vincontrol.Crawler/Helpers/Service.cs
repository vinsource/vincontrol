using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using vincontrol.Crawler.Entities;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Crawler.Helpers
{
    public class Service
    {
        private IUnitOfWork _unitOfWork;
        public XmlSerializer Serializer { get; set; }

        public Service()
        {
            _unitOfWork = new SqlUnitOfWork();
        }

        public void Execute()
        {
            foreach (var site in _unitOfWork.VinsocialCommon.GetAllSites())
            {
                Execute(site.Name, site.SiteId);
            }
        }

        public void Execute(string siteName, int siteId)
        {
            var filePath = GetXmlConfig(siteName);
            var siteConfig = new ReviewSite();
            if (File.Exists(filePath))
            {
                siteConfig = (ReviewSite)Serializer.Deserialize(new XmlTextReader(filePath));
                siteConfig.DealerReviews = _unitOfWork.Review.GetDealerReviewsBySite(siteId).ToList();
                if (siteConfig.ReviewPages.Count <= 0 || siteConfig.DealerReviews.Count <= 0)
                    return;
            }

            siteConfig.Id = siteId;
            siteConfig.Execute();
        }

        #region Static Methods

        public static DateTime GetUSTime(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(-9);
        }

        #endregion

        #region Private Methods

        private string GetXmlConfig(string siteName)
        {
            Serializer = BaseObjectType.GetXmlSerializer("ReviewSite");
            var xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Config\" + siteName + ".xml";
            // because Console App will automatically generate Config folder in to bin\Debug folder,
            // so replace the file path to refer to Config folder outsite bin\Debug.
            xmlFilePath = xmlFilePath.Replace(@"\bin\Debug", "");
            return xmlFilePath;
        }

        #endregion
    }
}
