using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Constant;
using vincontrol.Data.Model;
using Vincontrol.Web.Models;
using vincontrol.Helper;

namespace Vincontrol.Web.HelperClass
{
    public sealed class EbayHelper
    {
        public static string BuildEbayItem(EbayFormViewModel ebay, DealershipViewModel dealer)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.AppendLine("<AddItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">");
            AppendEbayItemBody(ebay, builder);
            builder.AppendLine("</AddItemRequest>");
            return builder.ToString();
        }

        private static void AppendEbayItemBody(EbayFormViewModel ebay, StringBuilder builder)
        {
            builder.AppendLine("<RequesterCredentials>");
            builder.AppendLine("<eBayAuthToken>" + ebay.Dealer.DealerSetting.EbayToken/*ConfigurationManager.AppSettings["UserToken"]*/ + "</eBayAuthToken>");
            builder.AppendLine("</RequesterCredentials>");
            builder.AppendLine("<Version>" + ConfigurationManager.AppSettings["Version"] + "</Version>");
            builder.AppendLine("<WarningLevel>High</WarningLevel>");
            builder.AppendLine("<Item>");
            builder.AppendLine("<Country>US</Country>");
            builder.AppendLine("<Currency>USD</Currency>");

            if (ebay.SelectedAuctionType.Equals("BuyItNowBestOffer"))
                builder.AppendLine("<ListingType>" + "FixedPriceItem" + "</ListingType>");
            else if (ebay.SelectedAuctionType.Equals("ChineseNoBuyItNow"))
                builder.AppendLine("<ListingType>" + "Chinese" + "</ListingType>");
            else
                builder.AppendLine("<ListingType>" + ebay.SelectedAuctionType + "</ListingType>");

            builder.AppendLine(ebay.XMLListingEnhancement);
            builder.AppendLine("<Quantity>1</Quantity>");
            builder.AppendLine("<PostalCode>" + ebay.Dealer.ZipCode + "</PostalCode>");
            builder.AppendLine("<ListingDuration>" + ebay.SelectedAuctionLength + "</ListingDuration>");
            builder.AppendLine("<PrimaryCategory>");
            builder.AppendLine("<CategoryID>" + ebay.EbayCategoryID + "</CategoryID>");
            builder.AppendLine("</PrimaryCategory>");

            if (ebay.SelectedAuctionType.Equals("BuyItNowBestOffer"))
            {
                builder.AppendLine("<StartPrice  currencyID=\"USD\">" +
                                   CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</StartPrice>");
                builder.AppendLine("<ListingDetails> ");
                builder.AppendLine("<MinimumBestOfferPrice currencyID=\"USD\"> " + ebay.MinimumPrice +
                                   "</MinimumBestOfferPrice>");
                builder.AppendLine("</ListingDetails>");
                builder.Append("<BestOfferDetails>");
                builder.Append("<BestOfferCount>0</BestOfferCount>");
                builder.Append("<BestOfferEnabled>true</BestOfferEnabled>");
                builder.Append("</BestOfferDetails>");
                builder.AppendLine("<PaymentDetails>");
                builder.AppendLine("<HoursToDeposit>" + ebay.SelectedHoursToDeposit + "</HoursToDeposit>");
                builder.AppendLine("<DaysToFullPayment>3</DaysToFullPayment>");
                builder.AppendLine("</PaymentDetails>");
                builder.AppendLine("<PaymentMethods>MOCC</PaymentMethods>");
            }
            else if (ebay.SelectedAuctionType.Equals("FixedPriceItem"))
            {
                builder.AppendLine("<StartPrice  currencyID=\"USD\">" +
                                   CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</StartPrice>");
                builder.AppendLine("<PaymentDetails>");
                builder.AppendLine("<HoursToDeposit>" + ebay.SelectedHoursToDeposit + "</HoursToDeposit>");
                builder.AppendLine("<DaysToFullPayment>3</DaysToFullPayment>");
                builder.AppendLine("</PaymentDetails>");
                builder.AppendLine("<PaymentMethods>MOCC</PaymentMethods>");
            }
            else if (ebay.SelectedAuctionType.Equals("Chinese"))
            {
                if (!String.IsNullOrEmpty(ebay.StartingPrice))
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">" +
                                       CommonHelper.RemoveSpecialCharactersForMsrp(ebay.StartingPrice) + "</StartPrice>");
                else
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">0</StartPrice>");
                builder.AppendLine("<BuyItNowPrice  currencyID=\"USD\">" +
                                   CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</BuyItNowPrice>");

                if (!String.IsNullOrEmpty(ebay.ReservePrice))
                {
                    builder.AppendLine("<ReservePrice  currencyID=\"USD\">" + ebay.ReservePrice + "</ReservePrice>");
                }

                builder.AppendLine("<PaymentDetails>");
                builder.AppendLine("<HoursToDeposit>" + ebay.SelectedHoursToDeposit + "</HoursToDeposit>");
                builder.AppendLine("<DaysToFullPayment>3</DaysToFullPayment>");
                builder.AppendLine("<DepositAmount>500</DepositAmount>");
                builder.AppendLine("</PaymentDetails>");
                builder.AppendLine("<PaymentMethods>MOCC</PaymentMethods>");
            }
            else if (ebay.SelectedAuctionType.Equals("ChineseNoBuyItNow"))
            {
                if (!String.IsNullOrEmpty(ebay.StartingPrice))
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">" +
                                       CommonHelper.RemoveSpecialCharactersForMsrp(ebay.StartingPrice) + "</StartPrice>");
                else
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">0</StartPrice>");

                if (!String.IsNullOrEmpty(ebay.ReservePrice))
                {
                    builder.AppendLine("<ReservePrice  currencyID=\"USD\">" + ebay.ReservePrice + "</ReservePrice>");
                }

                builder.AppendLine("<PaymentDetails>");
                builder.AppendLine("<HoursToDeposit>" + ebay.SelectedHoursToDeposit + "</HoursToDeposit>");
                builder.AppendLine("<DaysToFullPayment>3</DaysToFullPayment>");
                builder.AppendLine("<DepositAmount>500</DepositAmount>");
                builder.AppendLine("</PaymentDetails>");
                builder.AppendLine("<PaymentMethods>MOCC</PaymentMethods>");
            }

            builder.AppendLine("<SellerProvidedTitle>" + CommonHelper.TrimString(ebay.SellerProvidedTitle, 100) +
                               "</SellerProvidedTitle>");
            builder.AppendLine("<VIN>" + ebay.VehicleInfo.Vin + "</VIN>");

            builder.AppendLine(ebay.HitCounter? "<HitCounter>BasicStyle</HitCounter>": "<HitCounter>NoHitCounter</HitCounter>");

            
            if (ebay.VehicleInfo.UploadPhotosUrl!=null)
                builder.AppendLine("<PictureDetails><PictureURL>" + ebay.VehicleInfo.UploadPhotosUrl.First() +
                                   "</PictureURL></PictureDetails>");
            else
                builder.AppendLine("<PictureDetails><PictureURL>" + ebay.VehicleInfo.DefaultImageUrl +
                                   "</PictureURL></PictureDetails>");

            builder.AppendLine(ebay.VehicleInfo.Condition == Constanst.ConditionStatus.Used
                ? "<ConditionID>3000</ConditionID>"
                : "<ConditionID>1000</ConditionID>");
            builder.AppendLine("<Description>      <![CDATA[  " + ebay.HtmlSource + "  ]]>    </Description>");
            BuildItemSpecifics(ebay, builder);
            builder.AppendLine("</Item>");
        }

        public static string BuildEbayItemToVerify(EbayFormViewModel ebay, DealershipViewModel dealer)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.AppendLine("<VerifyAddItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">");
            AppendEbayItemBody(ebay, builder);
            builder.AppendLine("</VerifyAddItemRequest>");
            return builder.ToString();
        }

        private static void BuildItemSpecifics(EbayFormViewModel ebay, StringBuilder builder)
        {
            builder.AppendLine("<ItemSpecifics>");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Year</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.ModelYear + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Make</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Make + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Model</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Model + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Trim</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Trim + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Engine</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Engine + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Drive Type</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.WheelDrive + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Exterior Color</Name> ");
            builder.AppendLine("<Value>" + ebay.VehicleInfo.ExteriorColor + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Interior Color</Name> ");
            builder.AppendLine("<Value>" + ebay.VehicleInfo.InteriorColor + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Transmission</Name> ");
            builder.AppendLine("<Value>" + ebay.VehicleInfo.Tranmission + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Warranty</Name> ");
            builder.AppendLine("<Value>Vehicle does NOT have an existing warranty</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Mileage</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Mileage + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Vin</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Vin + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Vehicle Title</Name> ");
            builder.AppendLine(" <Value>Clear</Value> ");
            builder.AppendLine("</NameValueList> ");
            //builder.AppendLine("<NameValueList> ");
            //builder.AppendLine("<Name>Options</Name> ");
            //builder.AppendLine(" <Value>" + (String.IsNullOrEmpty(ebay.VehicleInfo.CarsOptions) ? "" : new System.Xml.Linq.XText(ebay.VehicleInfo.CarsOptions).ToString()) + "</Value> ");
            //builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>For Sale By</Name> ");
            builder.AppendLine(" <Value>Dealer</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Number Of Doors</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Door + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Body Type</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.BodyType + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Number of Cylinders</Name> ");
            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Cylinder + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("<NameValueList> ");
            builder.AppendLine("<Name>Contact Number</Name> ");
            builder.AppendLine(" <Value>" + ebay.Dealer.Phone + "</Value> ");
            builder.AppendLine("</NameValueList> ");
            builder.AppendLine("</ItemSpecifics>");
        }

        public static List<PostEbayAds> GetPostEbayList(int dealerId, string listingId)
        {
            var list = new List<PostEbayAds>();
            var lid = Convert.ToInt32(dealerId);
            var context = new VincontrolEntities();

            var queryContents = from a in context.Ebays
                                where a.InventoryId != lid
                                join b in context.Inventories
                                on a.InventoryId equals b.InventoryId
                                where a.Expiration > DateTime.Now
                                && a.DealerId == dealerId
                                orderby a.DateStamp
                                select new
                                {
                                    a.InventoryId,
                                    a.EbayAdURL,
                                    a.DateStamp,
                                    b.ThumbnailUrl,
                                    b.Vehicle.Year,
                                    b.Vehicle.Make,
                                    b.Vehicle.Model,
                                    b.Vehicle.Trim,
                                    b.SalePrice

                                };

            var ebayListNonExpired = queryContents.ToList();

            if (ebayListNonExpired.Count > 0)
            {
                foreach (var tmp in ebayListNonExpired)
                {
                    var ads = new PostEbayAds
                    {
                        ListingId = tmp.InventoryId,
                        EbayAdUrl = tmp.EbayAdURL,
                        ModelYear = tmp.Year.Value,
                        Make = tmp.Make,
                        Model = tmp.Model,
                        Title = tmp.Year.Value + " " + tmp.Make + " " + tmp.Model + " " + tmp.Trim,
                        SalePrice = tmp.SalePrice.GetValueOrDefault()

                    };

                    if (!String.IsNullOrEmpty(tmp.ThumbnailUrl))
                    {
                        string thumailUrl = (from data in tmp.ThumbnailUrl.Split(new[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList().First();
                        ads.EbayThumbNailPic = thumailUrl;
                    }
                    list.Add(ads);
                }
            }

            return list;
        }

  
    }
}
