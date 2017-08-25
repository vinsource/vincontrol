using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhitmanEnterpriseMVC.Models;
using System.Text;
using System.Configuration;
using System.Data;
using WhitmanEnterpriseMVC.DatabaseModel;

namespace WhitmanEnterpriseMVC.HelperClass
{
    public sealed class EbayHelper
    {
        public static string BuildEbayHtmlSource(EbayFormViewModel ebay, DealershipViewModel dealer)
        {
            var builder = new StringBuilder();

            builder.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            builder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            builder.AppendLine("   <head>");

            builder.AppendLine("  <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            builder.AppendLine("    <title>Ebay Title Goes Here</title>");
            builder.AppendLine(" <style type=\"text/css\">");
            builder.AppendLine("body {");
            builder.AppendLine("width: 100%; ");
            builder.AppendLine("font-family:");
            builder.AppendLine("\"Trebuchet MS\", ");
            builder.AppendLine("Arial, ");
            builder.AppendLine("Helvetica, ");
            builder.AppendLine("sans-serif;}");
            builder.AppendLine("ul.unstyled {");
            builder.AppendLine("list-style-type: none; ");
            builder.AppendLine("	margin: 0; ");
            builder.AppendLine("padding: 0;}	");
            builder.AppendLine("p {");
            builder.AppendLine("padding: 0 15px 0 15px;}");
            builder.AppendLine("h3.header{");
            builder.AppendLine("padding-bottom: 8px;}");
            builder.AppendLine("div {");
            builder.AppendLine("display: block;}");
            builder.AppendLine(".header {");
            builder.AppendLine("background: #900508; ");
            builder.AppendLine("color: white; ");
            builder.AppendLine("padding: ");
            builder.AppendLine("	.5em ");
            builder.AppendLine(".2em ");
            builder.AppendLine(".5em; ");
            builder.AppendLine("margin-bottom: 0;");
            builder.AppendLine("margin-top: 0;}");
            builder.AppendLine(".pad {");
            builder.AppendLine("	margin: 10px;");
            builder.AppendLine("overflow:hidden;");
            builder.AppendLine("background: #ddd;");
            builder.AppendLine("margin-bottom: 20px;");
            builder.AppendLine("	position: relative;");
            builder.AppendLine("padding-bottom: 35px;}");
            builder.AppendLine("	#description .pad,");
            builder.AppendLine("	#summary .pad {");
            builder.AppendLine("padding: 0;}");
            builder.AppendLine(".pad .return {");
            builder.AppendLine("width: 150px !important;");
            builder.AppendLine("	position: absolute;");
            builder.AppendLine("bottom: 5px;");
            builder.AppendLine("right: 5px;}");
            builder.AppendLine("	#title .header {");
            builder.AppendLine("color: white;");
            builder.AppendLine("font-size: 1.5em;}");
            builder.AppendLine(".header.bg {");
            builder.AppendLine("background: url('http://vincontrol.com/alpha/ebayimages/header-repeat.jpg') bottom left repeat-x;}");
            builder.AppendLine("	#outerwrap {");
            builder.AppendLine("width: 770px; ");
            builder.AppendLine("margin: 0 auto; ");
            builder.AppendLine("background: #000;");
            builder.AppendLine("overflow:hidden;");
            builder.AppendLine("margin-top: 50px;");
            builder.AppendLine("margin-bottom: 50px;}");
            builder.AppendLine("	#innerwrap {");
            builder.AppendLine("overflow: hidden;");
            builder.AppendLine("	margin: 15px; ");
            builder.AppendLine("background: #444;");
            builder.AppendLine("padding-bottom: 10px;}");
            builder.AppendLine("#title {");
            builder.AppendLine("border-bottom: ");
            builder.AppendLine("2px ");
            builder.AppendLine("#000000 ");
            builder.AppendLine("solid;}");
            builder.AppendLine("#dealer-banner {");
            builder.AppendLine("color: white;");
            builder.AppendLine("width: 100%; ");
            builder.AppendLine("	background: #000;");
            builder.AppendLine("overflow: hidden;}");
            builder.AppendLine("	#dealer-banner img {");
            builder.AppendLine("max-width: 350px;");
            builder.AppendLine("}");
            builder.AppendLine("				#dealer-banner-phone {				text-align:right;				float: right;				padding-right: 20px;}");
            builder.AppendLine("#dealer-banner-phone h5 {				margin-bottom: 0;}							#dealer-banner-phone h1 {				margin-top: 0;}				#topnav {				height: 30px !important;}				");

            builder.AppendLine("#topnav, 			#topnav-innerwrap {				height: 100%;				overflow:hidden;				margin: 0; 				padding: 0; 				width: 100% !important;				background:			url('http://vincontrol.com/alpha/ebayimages/navbg.jpg') 					bottom 					left 					repeat-x;}							#topnav ul {				height: 100%;				margin: 0; 				padding: 0;}");

            builder.AppendLine("#topnav li {				height: 22px; 				margin: 0; 				padding: 0; 				padding-top: 1px;				list-style-type:none; 				float: left; 				clear: right;				border-right: 					1px 					#333 					solid;				border-left: 					1px 					#000 					solid;}						#topnav li.end {				border-right: none;}							#topnav li.start {				border-left: none;}							#topnav li a {				height: 100%;				color: #ffffff;				text-decoration:none; 				margin: 					.2em 					.5em 					.2em 					.5em;}							#topnav li a:hover {			color: #900508;}						#topnav #facebook-ico img{				height: 25px;		position: relative;				left: 15px;}							#summary {			margin-top: 20px;}");


            builder.AppendLine("#summary #img {				width: 450px;				height: 300px;				overflow:hidden;				float: left;				display: inline-block;}						#summary #img img {				width: 100%;}							#information {				font-size: .9em;				display: inline-block;				width: 270px;}							#information ul {				margin: 15px;}			");


            builder.AppendLine("	a.btn {				font-size:1.1em;				text-align: center;				text-decoration:none;				color: white;				background: 					url('http://vincontrol.com/alpha/ebayimages/navbg.jpg') 					top 					left 					repeat-x;				border-radius: 4px;				padding: 					.2em 					.5em 					.2em 					.5em;				margin: 5px;				display: block;				color: white;				}");

            builder.AppendLine("					a.btn:hover {				background: 					url('http://vincontrol.com/alpha/ebayimages/header-repeat.jpg') 					top 					left 					repeat-x;}						#packages {			font-size: .9em;}							#packages-innerwrap h3 {				margin-left: 15px; 				margin-bottom: 0;}							#packages ul {		margin-top: 0; 				width: 250px;}					#packages #option-list,			#packages #package-list {				display: inline-block;}				#packages #option-list {				margin-left: 10px; 				float: left;}");




            builder.AppendLine("			#images-innerwrap {padding-bottom: 50px;}			#images .imagelisting-wrap {				width: 720px;				height: 500px !important;}							#warranty p,				#terms p{				font-size: .8em;}							#brand {				color: #888;				text-align:center;				padding: 2em;}			#brand a {				font-size: 1.5em;			font-weight: bold;				text-decoration:none;}							#carfaxWrap {				width: 400px;				margin: 0 auto;				margin-bottom: 30px;}			#outerwrap table, 			#outerwrap td, 			#outerwrap tr {				table-layout:fixed; 				overflow: hidden; 				padding: 0 !important; 				margin: 0 !important;}			            #outerwrap table.image, 			#outerwrap table.image td {		overflow: hidden; 				width:100% !important; 				height:100% !important; 				background: #ddd ;}			            #outerwrap table.image img {				display: block; 				margin: 0 auto; 				max-width: 98% !important;				max-height: 98% !important; 				margin-left: auto;				margin-right: auto;}​			a img {border: none;}			");


            builder.AppendLine(".image-small-wrap {				width: 120px;				height: 90px;				display: inline-block;				float: left;			}			.image-small-wrap {				text-align: center;				margin: 0;				padding: 0;				margin-bottom: 40px;			}			.image-small-wrap a {				text-decoration: none;				color: #960000;				font-weight: bold;				font-size: .8em;				margin: 2px;				margin-top: 0;			}			table.image {				margin-right: 5px;			} 			a.bx-next {				background: #111;				padding: 1px 4px 3px 4px;				text-decoration: none;				color: white;				font-weight: bold;				position: absolute;				top: 30px;				right: -45px;			}			a.bx-next:hover,			a.bx-prev:hover {				color: red;			}			a.bx-prev {				background: #111;				padding: 1px 4px 3px 4px;				text-decoration: none;				color: white;				font-weight: bold;				position: absolute;				top: 30px;				left: -45px;			}			.bx-wrapper {				width: 600px !important;				margin: 0 auto;			}");

            builder.AppendLine("</style>        </head>");



            builder.AppendLine("	     <body>            <div id=\"outerwrap\" >                <div id=\"innerwrap\">                	<div id=\"top\" name=\"top\"></div>");

            builder.AppendLine("    <!-- DEALER BANNER -->                    <div id=\"dealer-banner\">                        <img alt=\"dealer-info-goes-here\" src=\"" + dealer.LogoUrl + "\" />                        <div id=\"dealer-banner-phone\">                      	<h5> " + dealer.ContactPerson + " </h5>                            <h1> " + dealer.Phone + "</h1>                        </div>                    </div>                    ");

            
            

            builder.AppendLine(" <!-- TITLE --><div id=\"title\"> <h1 class=\"header\">" +CommonHelper.TrimString(ebay.SellerProvidedTitle,100)+ "</h1>                    </div>                                        <!-- TOPNAV -->                    <div id=\"topnav\">                        <div id=\"topnav-innerwrap\">                            <ul id=\"nav\">                                <li class=\"start\"><a href=\"#description\">Description</a></li>                               <li><a href=\"#packages\">Packages &amp; Options</a></li>                              <li><a href=\"#carfax\">Carfax</a></li>                               <li><a href=\"#images\">Images</a></li>                               <li><a href=\"#dealership\">Dealer</a></li>                                <li><a href=\"#warranty\">Warranty</a></li>                                <li><a href=\"#terms\">Terms &amp; Conditions</a></li>                                <li id=\"facebook-ico\" class=\"end\" style=\"margin:0 !important;\"><a style=\"margin:0 !important;\" href=\"" + dealer.FacebookUrl + "\"><img border=\"0\" alt=\"dealer-info-goes-here-facebook\" src=\"http://vincontrol.com/alpha/cListThemes/dealerImages/facbookIcon.png\" /></a></li>                            </ul>                        </div>                    </div>");

            string carTitle = ebay.VehicleInfo.ModelYear + " " + ebay.VehicleInfo.Make + " " + ebay.VehicleInfo.Model;
            builder.AppendLine("    <!-- CAR SUMMARY -->         <div id=\"summary\">                    	<h3 class=\"header bg\"> " + carTitle + " - " + dealer.ContactPerson + " " + dealer.Phone + "</h3>                    	<div id=\"summary-innerwrap\" class=\"pad\">                        	<div id=\"img\">                            	<table class=\"image\" style=\"vertical-align: middle;\">                                <tr><td style=\"vertical-align: middle;\"><img alt=\"dealer-info-plus-car-info-here\" src=\"" + ebay.VehicleInfo.SinglePhoto + "\" /></td></tr>                            </table>                            </div>                            <div id=\"information\">                            	<ul class=\"unstyled\">                                	<li><b>VIN:</b> " + ebay.VehicleInfo.Vin + "</li>                                	<li><b>Stock:</b> " + ebay.VehicleInfo.StockNumber + "</li>                                	<li><b>Mileage:</b> " + ebay.VehicleInfo.Mileage + "</li>                                	<li><b>Ext. Color:</b>" + ebay.VehicleInfo.ExteriorColor + "</li>                                	<li><b>Int. Color:</b>" + ebay.VehicleInfo.InteriorColor + "</li>                                	<li><b>Trans:</b>" + ebay.VehicleInfo.Tranmission + "</li>                                	<li><b>Engine:</b> " + ebay.VehicleInfo.Engine + "</li>                                </ul>                                <a class=\"btn\" href=\"" + dealer.EbayInventoryUrl + "\">View Our Inventory on eBay!</a>                                <a class=\"btn\" href=\"" + dealer.CreditUrl + "\">Financing</a>                                <a class=\"btn\" href=\"" + dealer.WebSiteUrl + "\">Visit Us Online!</a>                                <a class=\"btn\" href=\"" + dealer.ContactUsUrl + "\">Contact Us</a>                            </div>                        </div>                    </div>                    ");


            builder.AppendLine("                        <!-- DESCRIPTION -->                                        <div id=\"description\" name=\"description\">                    	<h3 class=\"header bg\">Description</h3>                    	<div id=\"description-innerwrap\" class=\"pad\">                        	<p>" + ebay.VehicleInfo.Description + "</p>                        </div>                    </div>                    ");


                int count = ebay.VehicleInfo.StandardOptions.Count;
                if (count > 0)
                {
                    builder.AppendLine("   <!-- PACKAGES & OPTIONS -->            <div id=\"packages\" name=\"packages\">                    	<h3 class=\"header bg\">Standard Options</h3>                    	                        <div id=\"packages-innerwrap\" class=\"pad\">                        	<div id=\"option-list\">                                ");


                    builder.AppendLine(" <ul>");

                    int index = count / 2;



                    foreach (string tmp in ebay.VehicleInfo.StandardOptions.Take(index))
                    {
                        builder.AppendLine("   <li>" + tmp + "</li>");
                    }
                    builder.AppendLine("</ul>");
                    builder.AppendLine("      </div>");
                    builder.AppendLine("");
                    builder.AppendLine("");
                    builder.AppendLine("");
                    builder.AppendLine("");
                    builder.AppendLine("");
                    builder.AppendLine("");
                    builder.AppendLine("");
                    builder.AppendLine("  <div id=\"package-list\">");
                    builder.AppendLine("                               <ul>");
                    foreach (string tmp in ebay.VehicleInfo.StandardOptions.Skip(index).Take(Math.Min(count / 2, count - index)))
                    {
                        builder.AppendLine("   <li>" + tmp + "</li>");
                    }
                    builder.AppendLine(" </ul>");
                    builder.AppendLine(" </div>");
                    builder.AppendLine("                             <a href=\"#top\" class=\"btn return\">Back to top</a>                        </div>                                            </div>");
                }

                if (ebay.VehicleInfo.ExistOptions.Count != 0 || ebay.VehicleInfo.ExistPackages.Count != 0)
                {

                    builder.AppendLine("  <div id=\"packages\" name=\"packages\">                    	<h3 class=\"header bg\">Additional Options &amp; Packages</h3>");
                    builder.AppendLine("   <div id=\"packages-innerwrap\" class=\"pad\">                        	<div id=\"option-list\">                                <h3>Options</h3>");

                    builder.AppendLine("  <ul>");
                    foreach (string tmp in ebay.VehicleInfo.ExistOptions)
                    {
                        builder.AppendLine("   <li>" + tmp + "</li>");
                    }
                    builder.AppendLine(" </ul>");
                    builder.AppendLine("   </div>");
                    builder.AppendLine("   <div id=\"package-list\">");
                    builder.AppendLine("  <h3>Packages</h3>");
                    builder.AppendLine("  <ul>");
                    foreach (string tmp in ebay.VehicleInfo.ExistPackages)
                    {
                        builder.AppendLine("   <li>" + tmp + "</li>");
                    }
                    builder.AppendLine(" </ul>");
                    builder.AppendLine("   </div>                            <a href=\"#top\" class=\"btn return\">Back to top</a>                        </div>                                            </div>                    ");
                }


            builder.AppendLine("     <!-- CARFAX -->                <div id=\"carfax\" name=\"carfax\">                    	<h3 class=\"header bg\">Carfax Vehicle History Report</h3>                    	<div id=\"carfax-innerwrap\" class=\"pad\">                            <a href=\"#top\" class=\"btn return\">Back to top</a>                        	<div id=\"carfaxWrap\" style=\"position: relative; \">                                                            <a target=\"_blank\" href=\"http://www.carfax.com/VehicleHistory/p/Report.cfx?vin=" + ebay.VehicleInfo.Vin + "\">                                	<img alt=\"carfax logo\" title=\"Click for full report\" style=\"display: inline-block; float: left; margin-right: 5px;\" src=\"http://www.carfaxonline.com/phoenix/img/cfx_logo_bw.jpg\" />                                </a>                                                                <h2> Vehicle History Report</h2>                                                    <a class=\"btn\" target=\"_blank\" id=\"carfaxBTN\" href=\"http://www.carfax.com/VehicleHistory/p/Report.cfx?vin=" + ebay.VehicleInfo.Vin + "\">Click Here For Full Report</a>                                                            <div style=\" width: 100%; font-size: .8em; opacity: .7; text-align:center !important;\">                                    Not all accidents or other issues are reported to CARFAX. The number of owners is estimated. See the full CARFAX Report for additional information and glossary of terms. 23.Feb.2012 14:43:00                                </div>                            </div>    	                        </div>                    </div>                    ");


            builder.AppendLine("   <!-- IMAGES -->                                        <div id=\"images\" name=\"images\">                    	<h3 class=\"header bg\">Images - " + carTitle + "</h3>                    	                        <div id=\"images-innerwrap\" class=\"pad\">                            <a href=\"#top\" class=\"btn return\">Back to top</a>");

            List<string> ebayImageList = String.IsNullOrEmpty(ebay.VehicleInfo.CarImageUrl) ? null : (from data in ebay.VehicleInfo.CarImageUrl.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList();

            if (ebayImageList != null)
            {
                foreach (string imgSrc in ebayImageList)
                {
                    builder.AppendLine(" <div class=\"imagelisting-wrap\">                            	<table class=\"image\" style=\"vertical-align: middle;\">                                	<tr><td style=\"vertical-align: middle;\"><img alt=\"dealer-info-plus-car-info-here\" src=\"" + imgSrc + "\" /></td></tr>                           	</table>                            </div>");


                }
            }






            builder.AppendLine("                             </div>                                            </div>");


            builder.AppendLine("<!-- OTHER EBAY VEHICLES --> <div id=\"otherebay\">                    	<h3 class=\"header bg\">Other Cars On eBay</h3>        	<div id=\"otherebay-innerwrap\" class=\"pad\">                    		<a href=\"#top\" class=\"btn return\">Back to Top</a>                    		");

            builder.AppendLine("            <div id=\"slider1\">	                    		<div>       ");

            foreach (PostEbayAds ebayPost in ebay.PostEbayList)
            {

                builder.AppendLine("  <div class=\"image-small-wrap\">		                    			<table class=\"image\" style=\"vertical-align: middle;\">		");
                builder.AppendLine("<tr><td style=\"vertical-align: middle;\"><a href=\"" + ebayPost.ebayAdURL + "\" target=\"_blank\"><img alt=\"dealer-info-plus-car-info-here\" src=\"" + ebayPost.ebayThumbNailPic + "\" /></a>");
                builder.AppendLine("</td></tr>		                            	</table>	");

                builder.AppendLine(" <a href=\"" + ebayPost.ebayAdURL + "\"  target=\"_blank\">" + ebayPost.Title + "</a>");

                builder.AppendLine(" </div>");



            }

            builder.AppendLine(" </div>");
            builder.AppendLine(" </div>");
            builder.AppendLine(" </div>");
            builder.AppendLine(" </div>");





            builder.AppendLine("       <!-- DEALER INFO -->                                        <div id=\"dealership\" name=\"dealership\">                    	<h3 class=\"header bg\">About Our Dealership</h3>                    	<div id=\"dealership-innerwrap\" class=\"pad\">                            <a href=\"#top\" class=\"btn return\">Back to top</a>                        	<p>" + dealer.DealerInfo + "</p>                        </div>                    </div>                                        ");



            builder.AppendLine("                   <!-- WARRANTY INFO -->                                        <div id=\"warranty\" name=\"warranty\">                    	<h3 class=\"header bg\">Warranty Information</h3>                    	<div id=\"warranty-innerwrap\" class=\"pad\">                            <a href=\"#top\" class=\"btn return\">Back to top</a>                        	<p>" + dealer.DealerWarranty + "</p>                        </div>                    </div>");



            builder.AppendLine(" <!-- TERMS & CONDITIONS -->                                        <div id=\"terms\" name=\"terms\">                    	<h3 class=\"header bg\">Terms &amp; Conditions</h3>                    	<div id=\"terms-innerwrap\" class=\"pad\">                            <a href=\"#top\" class=\"btn return\">Back to top</a>                        	<p>" + dealer.TermConditon + "</p>");


            builder.AppendLine("  </div>                        <div id=\"dealer-address\" style=\"text-align:center; color:white;\">" + dealer.DealershipAddress + "</div>                    </div>                                    </div>                <div id=\"brand\">					Powered by <a href=\"\" style=\"color: white;\">V<span style=\"color: red;\">I</span>N</a> | Vehicle Inventory Network, LLC.				</div>            </div>                </body>        </html>		");



            return builder.ToString();
        }


        public static string BuildEbayItem(EbayFormViewModel ebay, string EbayHTMLSource)
        {
            var builder = new StringBuilder();

            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            builder.AppendLine("<AddItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">");

            builder.AppendLine("<RequesterCredentials>");

            //PLUGIN REAL EBAY TOKEN 

            //builder.AppendLine("<eBayAuthToken>" + ConfigurationManager.AppSettings["UserToken"] + "</eBayAuthToken>");

            builder.AppendLine("<eBayAuthToken>" + ebay.Dealer.EbayToken + "</eBayAuthToken>");

            builder.AppendLine("</RequesterCredentials>");

            builder.AppendLine("<Version>" + ConfigurationManager.AppSettings["Version"] + "</Version>");

            builder.AppendLine("<WarningLevel>High</WarningLevel>");

            builder.AppendLine("<Item>");

            builder.AppendLine("<Country>US</Country>");

            builder.AppendLine("<Currency>USD</Currency>");

            if (ebay.SelectedAuctionType.Equals("BuyItNowBestOffer"))
                builder.AppendLine("<ListingType>" + "FixedPriceItem" + "</ListingType>");
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
                builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</StartPrice>");

                builder.AppendLine("<ListingDetails> ");
                builder.AppendLine("<MinimumBestOfferPrice currencyID=\"USD\"> " + ebay.ReservePrice + "</MinimumBestOfferPrice>");
                builder.AppendLine("</ListingDetails>");

                //builder.AppendLine("<MinimumBestOfferPrice currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMSRP(MinimumPrice) + "</MinimumBestOfferPrice>");

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
                builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</StartPrice>");

                builder.AppendLine("<PaymentDetails>");

                builder.AppendLine("<HoursToDeposit>" + ebay.SelectedHoursToDeposit + "</HoursToDeposit>");

                builder.AppendLine("<DaysToFullPayment>3</DaysToFullPayment>");

                builder.AppendLine("</PaymentDetails>");

                builder.AppendLine("<PaymentMethods>MOCC</PaymentMethods>");

            }
            else if (ebay.SelectedAuctionType.Equals("Chinese"))
            {
                if (!String.IsNullOrEmpty(ebay.StartingPrice))
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.StartingPrice) + "</StartPrice>");
                else
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">0</StartPrice>");
                builder.AppendLine("<BuyItNowPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</BuyItNowPrice>");

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
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.StartingPrice) + "</StartPrice>");
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



            string sellTitle = ebay.VehicleInfo.ModelYear + " " + ebay.VehicleInfo.Make + " " + ebay.VehicleInfo.Model + " " + ebay.VehicleInfo.Trim + " " + ebay.VehicleInfo.Description;

            builder.AppendLine("<SellerProvidedTitle>" + CommonHelper.TrimString(ebay.SellerProvidedTitle, 100) + "</SellerProvidedTitle>");

            builder.AppendLine("<VIN>" + ebay.VehicleInfo.Vin + "</VIN>");

            if (ebay.HitCounter)

                builder.AppendLine("<HitCounter>BasicStyle</HitCounter>");

            builder.AppendLine("<PictureDetails><PictureURL>" + ebay.VehicleInfo.SinglePhoto + "</PictureURL>    </PictureDetails>");

            if (ebay.VehicleInfo.Condition.Equals("Used"))

                builder.AppendLine("<ConditionID>3000</ConditionID>");

            else if (ebay.VehicleInfo.Condition.Equals("PreOwned"))

                builder.AppendLine("<ConditionID>2500</ConditionID>");

            else

                builder.AppendLine("<ConditionID>1000</ConditionID>");


            builder.AppendLine("<Description>      <![CDATA[  " + EbayHTMLSource + "  ]]>    </Description>");

            ////3 months later
            //builder.AppendLine("<ItemSpecifics>");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>Year</Name> ");

            //builder.AppendLine(" <Value>" + ebay.VehicleInfo.ModelYear + "</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>Mileage</Name> ");

            //builder.AppendLine(" <Value>" + ebay.VehicleInfo.Mileage + "</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>Vehicle Title</Name> ");

            //builder.AppendLine(" <Value>Clear</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>For sale by</Name> ");

            //builder.AppendLine(" <Value>Dealer</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>Body Type</Name> ");

            //builder.AppendLine(" <Value>" + ebay.VehicleInfo.BodyType + "</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>Tranmission</Name> ");

            //builder.AppendLine("<Value>" + ebay.VehicleInfo.Tranmission + "</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>Drive Train</Name> ");

            //builder.AppendLine("<Value>" + ebay.VehicleInfo.WheelDrive + "</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");

            //builder.AppendLine("<Name>Engine</Name> ");

            //builder.AppendLine("<Value>" + ebay.VehicleInfo.Engine + "</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");


            //builder.AppendLine("<Name>Fuel Type</Name> ");

            //builder.AppendLine("<Value>" + ebay.VehicleInfo.Fuel + "</Value> ");

            //builder.AppendLine("</NameValueList> ");


            //builder.AppendLine("<NameValueList> ");


            //builder.AppendLine("<Name>Exterior Color</Name> ");

            //builder.AppendLine("<Value>" + ebay.VehicleInfo.ExteriorColor + "</Value> ");

            //builder.AppendLine("</NameValueList> ");


            //builder.AppendLine("<NameValueList> ");


            //builder.AppendLine("<Name>Interior Color</Name> ");

            //builder.AppendLine("<Value>" + ebay.VehicleInfo.InteriorColor + "</Value> ");

            //builder.AppendLine("</NameValueList> ");

            //builder.AppendLine("<NameValueList> ");


            //builder.AppendLine("<Name>Certification</Name> ");

            //builder.AppendLine("<Value>----</Value> ");

            //builder.AppendLine("</NameValueList> ");


            //////ADD MORE ATTRIBUTEs TO VEHICLE
            //ADD MORE ATTRIBUTEs TO VEHICLE
            builder.AppendLine("<AttributeSetArray>");
            builder.AppendLine("<AttributeSet attributeSetID=\"1137\">");

            builder.AppendLine("<Attribute attributeID=\"38\">");
            builder.AppendLine("<Value>");

            builder.AppendLine("<ValueID>0</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.ModelYear + "0000" + "</ValueLiteral>");

            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10243\">");
            builder.AppendLine("<Value>");

            builder.AppendLine("<ValueID>10448</ValueID>");
            builder.AppendLine("<ValueLiteral>Clear</ValueLiteral>");
            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"10244\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"85\">");

            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-3</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Mileage + "</ValueLiteral>");

            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"10242\">");

            builder.AppendLine("<Value>");

            if (ebay.LimitedWarranty)
            {

                builder.AppendLine("<ValueID>10721</ValueID>");
                builder.AppendLine("<ValueLiteral>Vehicle has an existing warranty</ValueLiteral>");
            }
            else
            {
                builder.AppendLine("<ValueID>10722</ValueID>");
                builder.AppendLine("<ValueLiteral>Vehicle does NOT have an existing warranty</ValueLiteral>");
            }

            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10241\">");

            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-3</ValueID>");
            builder.AppendLine("<ValueLiteral>" + CommonHelper.TrimString(ebay.VehicleInfo.Trim + " " + ebay.VehicleInfo.BodyType, 11) + "</ValueLiteral>");

            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"26738\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-13</ValueID>");
            builder.AppendLine("<ValueLiteral>Yes</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10240\">");

            builder.AppendLine("<Value>");
            if (ebay.VehicleInfo.Door == 2)
                builder.AppendLine("<ValueID>1026</ValueID>");
            else if (ebay.VehicleInfo.Door == 3)
                builder.AppendLine("<ValueID>1027</ValueID>");
            else if (ebay.VehicleInfo.Door == 4)
                builder.AppendLine("<ValueID>1028</ValueID>");
            else if (ebay.VehicleInfo.Door == 5)
                builder.AppendLine("<ValueID>1028</ValueID>");
            else
                builder.AppendLine("<ValueID>-10</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Door + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"25846\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-3</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Engine + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            if (ebay.VehicleInfo.IsElectric == false)
            {
                builder.AppendLine("<Attribute attributeID=\"10718\">");
                builder.AppendLine("<Value>");
                if (ebay.VehicleInfo.Cylinder.Equals("2"))
                    builder.AppendLine("<ValueID>1026</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("3"))
                    builder.AppendLine("<ValueID>1027</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("4"))
                    builder.AppendLine("<ValueID>1028</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("5"))
                    builder.AppendLine("<ValueID>1029</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("6"))
                    builder.AppendLine("<ValueID>1030</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("8"))
                    builder.AppendLine("<ValueID>1032</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("10"))
                    builder.AppendLine("<ValueID>1034</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("12"))
                    builder.AppendLine("<ValueID>1173</ValueID>");

                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Cylinder + "</ValueLiteral>");
                builder.AppendLine("</Value>");
                builder.AppendLine("</Attribute>");

            }

            builder.AppendLine("<Attribute attributeID=\"10239\">");
            builder.AppendLine("<Value>");
            if (ebay.VehicleInfo.Tranmission.ToLowerInvariant().Contains("automatic"))
            {
                builder.AppendLine("<ValueID>10427</ValueID>");
                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Tranmission + "</ValueLiteral>");
            }
            else
            {
                builder.AppendLine("<ValueID>10428</ValueID>");
                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Tranmission + "</ValueLiteral>");
            }
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10719\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>" + ebay.SelectedExteriorColor + "</ValueID>");

            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.ExteriorColor + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"10720\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>" + ebay.SelectedInteriorColor + "</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.InteriorColor + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");
            if (ebay.VehicleInfo.IsElectric == false)
            {
                builder.AppendLine("<Attribute attributeID=\"39705\">");
                builder.AppendLine("<Value>");
                builder.AppendLine("<ValueID>-3</ValueID>");
                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Fuel + "</ValueLiteral>");
                builder.AppendLine("</Value>");
                builder.AppendLine("</Attribute>");
            }

            //builder.AppendLine("<Attribute attributeID=\"25919\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Air conditioning</ValueLiteral>");
            //builder.AppendLine("<ValueID>10430</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cruise Control</ValueLiteral>");
            //builder.AppendLine("<ValueID>10434</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Locks</ValueLiteral>");
            //builder.AppendLine("<ValueID>10439</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Windows</ValueLiteral>");
            //builder.AppendLine("<ValueID>10441</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10440</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");
            //builder.AppendLine("<Attribute attributeID=\"25918\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>AntiLock Brakes</ValueLiteral>");
            //builder.AppendLine("<ValueID>10432</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Driver Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10435</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Passenger Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10438</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");
            //builder.AppendLine("<Attribute attributeID=\"10446\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Leather Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10437</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cassette Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10433</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>CD Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10431</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");
            //builder.AppendLine("<Attribute attributeID=\"26591\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Leather Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10437</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cassette Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10433</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>CD Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10431</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Air Conditioning</ValueLiteral>");
            //builder.AppendLine("<ValueID>10430</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cruise Control</ValueLiteral>");
            //builder.AppendLine("<ValueID>10434</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Locks</ValueLiteral>");
            //builder.AppendLine("<ValueID>10439</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Windows</ValueLiteral>");
            //builder.AppendLine("<ValueID>10441</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10440</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Anti-Lock Brakes</ValueLiteral>");
            //builder.AppendLine("<ValueID>10432</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Driver Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10435</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Passenger Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10438</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");



            builder.AppendLine("</AttributeSet>");
            builder.AppendLine("</AttributeSetArray>");
            //builder.AppendLine("</ItemSpecifics>");
            builder.AppendLine("</Item>");
            builder.AppendLine("</AddItemRequest>");

            return builder.ToString();



        }

        public static string BuildEbayItemToVerify(EbayFormViewModel ebay, DealershipViewModel dealer)
        {
            var builder = new StringBuilder();

            builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            builder.AppendLine("<VerifyAddItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">");

            builder.AppendLine("<RequesterCredentials>");

            //PLUGIN REAL EBAY TOKEN 

            //builder.AppendLine("<eBayAuthToken>" + ConfigurationManager.AppSettings["UserToken"] + "</eBayAuthToken>");

            builder.AppendLine("<eBayAuthToken>" + dealer.EbayToken + "</eBayAuthToken>");

            builder.AppendLine("</RequesterCredentials>");

            builder.AppendLine("<Version>" + ConfigurationManager.AppSettings["Version"] + "</Version>");

            builder.AppendLine("<WarningLevel>High</WarningLevel>");

            builder.AppendLine("<Item>");

            builder.AppendLine("<Country>US</Country>");

            builder.AppendLine("<Currency>USD</Currency>");

            if (ebay.SelectedAuctionType.Equals("BuyItNowBestOffer"))
                builder.AppendLine("<ListingType>" + "FixedPriceItem" + "</ListingType>");
            else
                builder.AppendLine("<ListingType>" + ebay.SelectedAuctionType + "</ListingType>");


            builder.AppendLine(ebay.XMLListingEnhancement);

            builder.AppendLine("<Quantity>1</Quantity>");

            builder.AppendLine("<PostalCode>" + dealer.ZipCode + "</PostalCode>");

            builder.AppendLine("<ListingDuration>" + ebay.SelectedAuctionLength + "</ListingDuration>");

            builder.AppendLine("<PrimaryCategory>");

            builder.AppendLine("<CategoryID>" + ebay.EbayCategoryID + "</CategoryID>");

            builder.AppendLine("</PrimaryCategory>");




            if (ebay.SelectedAuctionType.Equals("BuyItNowBestOffer"))
            {
                builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</StartPrice>");

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
                builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</StartPrice>");

                builder.AppendLine("<PaymentDetails>");

                builder.AppendLine("<HoursToDeposit>" + ebay.SelectedHoursToDeposit + "</HoursToDeposit>");

                builder.AppendLine("<DaysToFullPayment>3</DaysToFullPayment>");

                builder.AppendLine("</PaymentDetails>");

                builder.AppendLine("<PaymentMethods>MOCC</PaymentMethods>");

            }
            else if (ebay.SelectedAuctionType.Equals("Chinese"))
            {
                if (!String.IsNullOrEmpty(ebay.StartingPrice))
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.StartingPrice) + "</StartPrice>");
                else
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">0</StartPrice>");

                builder.AppendLine("<BuyItNowPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.BuyItNowPrice) + "</BuyItNowPrice>");

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
                    builder.AppendLine("<StartPrice  currencyID=\"USD\">" + CommonHelper.RemoveSpecialCharactersForMsrp(ebay.StartingPrice) + "</StartPrice>");
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



            builder.AppendLine("<SellerProvidedTitle>" + CommonHelper.TrimString(ebay.SellerProvidedTitle, 100) + "</SellerProvidedTitle>");

            builder.AppendLine("<VIN>" + ebay.VehicleInfo.Vin + "</VIN>");

            if (ebay.HitCounter)
                builder.AppendLine("<HitCounter>BasicStyle</HitCounter>");
            else
                builder.AppendLine("<HitCounter>NoHitCounter</HitCounter>");


            builder.AppendLine("<PictureDetails><PictureURL>" + ebay.VehicleInfo.SinglePhoto + "</PictureURL></PictureDetails>");

            if (ebay.VehicleInfo.Condition.Equals("Used"))

                builder.AppendLine("<ConditionID>3000</ConditionID>");

            else if (ebay.VehicleInfo.Condition.Equals("PreOwned"))

                builder.AppendLine("<ConditionID>2500</ConditionID>");

            else

                builder.AppendLine("<ConditionID>1000</ConditionID>");

            builder.AppendLine("<Description>      <![CDATA[  HTML CODE HERE  ]]>    </Description>");


            builder.AppendLine("<ItemSpecifics>");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>Year</Name> ");

            builder.AppendLine(" <Value>" + ebay.VehicleInfo.ModelYear + "</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>Mileage</Name> ");

            builder.AppendLine(" <Value>" + ebay.VehicleInfo.Mileage + "</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>Vehicle Title</Name> ");

            builder.AppendLine(" <Value>Clear</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>For sale by</Name> ");

            builder.AppendLine(" <Value>Dealer</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>Body Type</Name> ");

            builder.AppendLine(" <Value>" + ebay.VehicleInfo.BodyType + "</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>Tranmission</Name> ");

            builder.AppendLine("<Value>" + ebay.VehicleInfo.Tranmission + "</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>Drive Train</Name> ");

            builder.AppendLine("<Value>" + ebay.VehicleInfo.WheelDrive + "</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");

            builder.AppendLine("<Name>Engine</Name> ");

            builder.AppendLine("<Value>" + ebay.VehicleInfo.Engine + "</Value> ");

            builder.AppendLine("</NameValueList> ");

            builder.AppendLine("<NameValueList> ");


            builder.AppendLine("<Name>Fuel Type</Name> ");

            builder.AppendLine("<Value>" + ebay.VehicleInfo.Fuel + "</Value> ");

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


            builder.AppendLine("<Name>Certification</Name> ");

            builder.AppendLine("<Value>----</Value> ");

            builder.AppendLine("</NameValueList> ");



            builder.AppendLine("</ItemSpecifics>");

            //ADD MORE ATTRIBUTEs TO VEHICLE
            builder.AppendLine("<AttributeSetArray>");
            builder.AppendLine("<AttributeSet attributeSetID=\"1137\">");

            builder.AppendLine("<Attribute attributeID=\"38\">");
            builder.AppendLine("<Value>");

            builder.AppendLine("<ValueID>0</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.ModelYear + "0000" + "</ValueLiteral>");

            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10243\">");
            builder.AppendLine("<Value>");

            builder.AppendLine("<ValueID>10448</ValueID>");
            builder.AppendLine("<ValueLiteral>Clear</ValueLiteral>");
            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"10244\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"85\">");

            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-3</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Mileage + "</ValueLiteral>");

            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"10242\">");

            builder.AppendLine("<Value>");

            if (ebay.LimitedWarranty)
            {

                builder.AppendLine("<ValueID>10721</ValueID>");
                builder.AppendLine("<ValueLiteral>Vehicle has an existing warranty</ValueLiteral>");
            }
            else
            {
                builder.AppendLine("<ValueID>10722</ValueID>");
                builder.AppendLine("<ValueLiteral>Vehicle does NOT have an existing warranty</ValueLiteral>");
            }

            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10241\">");

            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-3</ValueID>");
            builder.AppendLine("<ValueLiteral>" + CommonHelper.TrimString(ebay.VehicleInfo.Trim + " " + ebay.VehicleInfo.BodyType, 11) + "</ValueLiteral>");

            builder.AppendLine("</Value>");

            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"26738\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-13</ValueID>");
            builder.AppendLine("<ValueLiteral>Yes</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10240\">");

            builder.AppendLine("<Value>");
            if (ebay.VehicleInfo.Door == 2)
                builder.AppendLine("<ValueID>1026</ValueID>");
            else if (ebay.VehicleInfo.Door == 3)
                builder.AppendLine("<ValueID>1027</ValueID>");
            else if (ebay.VehicleInfo.Door == 4)
                builder.AppendLine("<ValueID>1028</ValueID>");
            else if (ebay.VehicleInfo.Door == 5)
                builder.AppendLine("<ValueID>1028</ValueID>");
            else
                builder.AppendLine("<ValueID>-10</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Door + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"25846\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>-3</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Engine + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            if (ebay.VehicleInfo.IsElectric == false)
            {
                builder.AppendLine("<Attribute attributeID=\"10718\">");
                builder.AppendLine("<Value>");
                if (ebay.VehicleInfo.Cylinder.Equals("2"))
                    builder.AppendLine("<ValueID>1026</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("3"))
                    builder.AppendLine("<ValueID>1027</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("4"))
                    builder.AppendLine("<ValueID>1028</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("5"))
                    builder.AppendLine("<ValueID>1029</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("6"))
                    builder.AppendLine("<ValueID>1030</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("8"))
                    builder.AppendLine("<ValueID>1032</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("10"))
                    builder.AppendLine("<ValueID>1034</ValueID>");
                else if (ebay.VehicleInfo.Cylinder.Equals("12"))
                    builder.AppendLine("<ValueID>1173</ValueID>");

                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Cylinder + "</ValueLiteral>");
                builder.AppendLine("</Value>");
                builder.AppendLine("</Attribute>");

            }

            builder.AppendLine("<Attribute attributeID=\"10239\">");
            builder.AppendLine("<Value>");
            if (ebay.VehicleInfo.Tranmission.ToLowerInvariant().Contains("automatic"))
            {
                builder.AppendLine("<ValueID>10427</ValueID>");
                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Tranmission + "</ValueLiteral>");
            }
            else
            {
                builder.AppendLine("<ValueID>10428</ValueID>");
                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Tranmission + "</ValueLiteral>");
            }
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            builder.AppendLine("<Attribute attributeID=\"10719\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>" + ebay.SelectedExteriorColor + "</ValueID>");

            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.ExteriorColor + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");
            builder.AppendLine("<Attribute attributeID=\"10720\">");
            builder.AppendLine("<Value>");
            builder.AppendLine("<ValueID>" + ebay.SelectedInteriorColor + "</ValueID>");
            builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.InteriorColor + "</ValueLiteral>");
            builder.AppendLine("</Value>");
            builder.AppendLine("</Attribute>");

            if (ebay.VehicleInfo.IsElectric == false)
            {
                builder.AppendLine("<Attribute attributeID=\"39705\">");
                builder.AppendLine("<Value>");
                builder.AppendLine("<ValueID>-3</ValueID>");
                builder.AppendLine("<ValueLiteral>" + ebay.VehicleInfo.Fuel + "</ValueLiteral>");
                builder.AppendLine("</Value>");
                builder.AppendLine("</Attribute>");
            }

            //builder.AppendLine("<Attribute attributeID=\"25919\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Air conditioning</ValueLiteral>");
            //builder.AppendLine("<ValueID>10430</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cruise Control</ValueLiteral>");
            //builder.AppendLine("<ValueID>10434</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Locks</ValueLiteral>");
            //builder.AppendLine("<ValueID>10439</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Windows</ValueLiteral>");
            //builder.AppendLine("<ValueID>10441</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10440</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");
            //builder.AppendLine("<Attribute attributeID=\"25918\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>AntiLock Brakes</ValueLiteral>");
            //builder.AppendLine("<ValueID>10432</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Driver Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10435</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Passenger Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10438</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");
            //builder.AppendLine("<Attribute attributeID=\"10446\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Leather Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10437</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cassette Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10433</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>CD Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10431</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");
            //builder.AppendLine("<Attribute attributeID=\"26591\">");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Leather Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10437</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cassette Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10433</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>CD Player</ValueLiteral>");
            //builder.AppendLine("<ValueID>10431</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Air Conditioning</ValueLiteral>");
            //builder.AppendLine("<ValueID>10430</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Cruise Control</ValueLiteral>");
            //builder.AppendLine("<ValueID>10434</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Locks</ValueLiteral>");
            //builder.AppendLine("<ValueID>10439</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Windows</ValueLiteral>");
            //builder.AppendLine("<ValueID>10441</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Power Seats</ValueLiteral>");
            //builder.AppendLine("<ValueID>10440</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Anti-Lock Brakes</ValueLiteral>");
            //builder.AppendLine("<ValueID>10432</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Driver Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10435</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("<Value>");
            //builder.AppendLine("<ValueLiteral>Passenger Airbag</ValueLiteral>");
            //builder.AppendLine("<ValueID>10438</ValueID>");
            //builder.AppendLine("</Value>");
            //builder.AppendLine("</Attribute>");



            builder.AppendLine("</AttributeSet>");
            builder.AppendLine("</AttributeSetArray>");
            builder.AppendLine("</Item>");
            builder.AppendLine("</VerifyAddItemRequest>");

            return builder.ToString();



        }

        public static List<PostEbayAds> GetPostEbayList(int dealershipID, string ListingId)
        {
            var list = new List<PostEbayAds>();

            int lid = Convert.ToInt32(ListingId);

            var context = new whitmanenterprisewarehouseEntities();

            var queryContents = from a in context.whitmanenterprisebayads
                                where a.ListingId != lid
                                join b in context.whitmanenterprisedealershipinventories
                                on a.ListingId equals b.ListingID
                                where a.EbayAdEndTime > DateTime.Now && a.DealerId==dealershipID
                                orderby a.EbayAdEndTime
                                select new
                                {
                                    a.ListingId, a.EbayAdURL, a.EbayAdEndTime, b.ThumbnailImageURL, b.ModelYear, b.Make, b.Model, b.Trim

                                };

           

            var ebayListNonExpired = queryContents.ToList();

            if (ebayListNonExpired.Count > 0)
            {
                foreach (var tmp in ebayListNonExpired)
                {
                    var ads = new PostEbayAds()
                    {
                        ListingId = tmp.ListingId,
                        ebayAdURL = tmp.EbayAdURL,
                        ModelYear = tmp.ModelYear.Value,
                        Make = tmp.Make,
                        Model = tmp.Model,
                        Title = tmp.ModelYear.Value + " " + tmp.Make + " " + tmp.Model

                    };

                    if (!String.IsNullOrEmpty(tmp.ThumbnailImageURL))
                    {
                        string thumailUrl = (from data in tmp.ThumbnailImageURL.Split(new string[] { ",", "|" }, StringSplitOptions.RemoveEmptyEntries) select data).ToList().First();
                        ads.ebayThumbNailPic = thumailUrl;

                    }

                    list.Add(ads);
                }


            }

            return list;
        }
   
    



    }
}
