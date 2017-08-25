<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.EbayFormViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ebay Template</title>
    <!-- jQuery library - I get it from Google API's -->
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <link href="<%=Url.Content("~/js/bxslider/jquery.bxslider.css")%>" rel="stylesheet"
        type="text/css" media="screen" />
    <link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/previewlisting.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/bxslider/jquery.bxslider.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/previewlisting.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
</head>
<body>
    <script type="text/javascript">
        function PopupBuyerGuideWindow(actionUrl) {

            $("<a href=" + actionUrl + "></a>").fancybox({
                height: 915,
                width: 1000,
                overlayShow: true,
                showCloseButton: true,
                enableEscapeButton: true,
                type: 'iframe'
            }).click();
        }

        function newPopup(url) {
            return window.open(url, "Ebay from Vincontrol", 'height=768,width=1366,left=10,top=10,titlebar=no,toolbar=no,menubar=no,location=no,directories=no,status=no');
        }

        $(document).ready(function () {


            $("#PostButton").live('click', function () {
                $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
                $.post('<%= Url.Content("~/Market/PostEbayAds") %>', function (data) {
                    if (data == "TimeOut") {
                        ShowWarningMessage("Your session is timed out. Please login back again");
                        var actionUrl = '<%= Url.Action("LogOff", "Account") %>';
                        window.parent.location = actionUrl;
                    }
                    else if (data == "Fail") {
                        parent.$.fancybox.close();
                        ShowWarningMessage("There is an problem with ebay posting ads. Please contact 1-855-VINCONTRL for support.");
                    }
                    else {
                        window.location = data;
                        ShowWarningMessage("Thanks for using our ebay posting intergration Vincontrol. Your ads is just posted on ebay successfully.");

                    }

                });


            });

            $("#CancelButton").live('click', function () {
                parent.$.fancybox.close();

            });


        });




    </script>
    <div class="previewListingHeader" id="previewListingHeader">
        <%--     <% Html.BeginForm("PostEbayAds", "Market", FormMethod.Post, new { id = "PostEbayForm", target = "_blank" }); %>--%>
        <div class="previewListingHeader_price">
            <label>
                Final eBay Posting Price</label>
            <label class="price_top"><%=Model.TotalListingFee %></label>
        </div>
        <div class="previewListingHeader_btns">
            <div class="previewListingHeader_btns_items" id="PostButton">Complete Ad</div>
            <div class="previewListingHeader_btns_items" id="CancelButton">Cancel Ad </div>

        </div>
        <div class="previewListingHeader_title">
            <label>
            <%=Model.SellerProvidedTitle %></label>
        </div>
        <%=Html.HiddenFor(x => x.ListingId)%>
        <%--<% Html.EndForm(); %>--%>
    </div>
    <div class="previewListingLine">
    </div>

    <!-- START OF TEMPLATE -->
    <div class="previewListingContainer" id="template">
        <div class="container_header">
            <div class="container_header_top">
                <div class="container_header_top_dealerInfo">
                    <div class="container_header_dealerInfo_logo">
                      <img src="<%=Model.Dealer.DealerSetting.LogoUrl %>" />
                    </div>
                    <div class="container_header_dealerInfo_text">
                        <div class="container_header_phone">
                            <% if (Model.Dealer.DealerSetting.EbayContactInfoName!=null && Model.Dealer.DealerSetting.EbayContactInfoPhone !=null)
                               { %>
                            <% if ((Model.Dealer.DealerSetting.EbayContactInfoName.Length + Model.Dealer.DealerSetting.EbayContactInfoPhone.Length) <= 31)
                               { %>
                            Call <%= Model.Dealer.DealerSetting.EbayContactInfoName %>:
                            <%= Model.Dealer.DealerSetting.EbayContactInfoPhone %>
                            <% }
                               else
                               { %>

                             Call <%= Model.Dealer.DealerSetting.EbayContactInfoName %>:<br />
                            <%= Model.Dealer.DealerSetting.EbayContactInfoPhone %>
                            <% } %>
                            <% } %>
                        </div>
                        <div class="container_header_fax_email">
                            <%=Model.Dealer.DealerSetting.EbayContactInfoEmail%>
                        </div>
                        <div class="container_header_address">
                            <%=Model.Dealer.DealershipAddress %>
                        </div>
                        <div class="container_header_address">
                            <%=Model.Dealer.City %>, <%=Model.Dealer.State %> <%=Model.Dealer.ZipCode %>
                        </div>
                    </div>
                </div>
                <div class="container_header_top_navi">
                    <a href="<%=Model.Dealer.EbayInventoryUrl %>">
                        <div class="top_navi_items">
                            View Our Inventory on eBay!
                        </div>
                    </a><a href="<%=Model.Dealer.DealerSetting.CreditUrl %>">
                        <div class="top_navi_items">
                            Financing
                        </div>
                    </a><a href="<%=Model.Dealer.DealerSetting.WebSiteUrl %>">
                        <div class="top_navi_items">
                            Visit Us Online!
                        </div>
                    </a><a href="mailto:<%=Model.Dealer.Email %>">
                        <div class="top_navi_items">
                            Email Us
                        </div>
                    </a><a href="<%=Model.Dealer.DealerSetting.ContactUsUrl %>">
                        <div style="border: none" class="top_navi_items">
                            Contact Us
                        </div>
                    </a>
                </div>
            </div>
            <div class="container_header_bottom">
            </div>
        </div>
        <div class="navi_area_holder">
            <div class="navi_area_items" id="tab_pictures" style="border-radius: 5px 0px 0px 5px;">
                Pictures
            </div>
            <div class="navi_area_items" id="tab_vehicle_information">
                Vehicle Information
            </div>
            <div class="navi_area_items" id="tab_descriptions">
                Description
            </div>
            <div class="navi_area_items" id="tab_packages_options">
                Package &amp; Options
            </div>
            <div class="navi_area_items" id="tab_warranty">
                Warranty
            </div>
            <div class="navi_area_items" id="tab_terms_conditions" style="border-radius: 0px 5px 5px 0px;">
                Terms &amp; Conditions
            </div>
        </div>
        <div id="pictures_area" class="img_slider_holder">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text"> Pictures </nobr>
                <nobr class="image_slider_vhInfo_ymm"><%=Model.VehicleInfo.ModelYear %> <%=Model.VehicleInfo.Make %> <%=Model.VehicleInfo.Model %> <%=Model.VehicleInfo.Trim %></nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="img_slider_content_holder">
                <div class="bx-wrapper" style="max-width: 100%;">
                    <div class="bx-viewport" style="overflow: hidden; position: relative; max-height: 500px; width: 100%;">
                        <ul class="bxslider" style="width: 515%; position: relative; transition-duration: 0s; transform: translate3d(-936px, 0px, 0px);">
                            <%foreach (string imgSrc in Model.VehicleInfo.UploadPhotosUrl)
                              { %>
                            <li class="bx-clone" style="float: left; list-style: none outside none; position: relative; width: 896px;">
                                <img src="<%=imgSrc %>">
                            </li>
                            <%} %>
                        </ul>
                    </div>
                </div>
                <div id="bx-pager">
                    <% int index = 0; %>
                    <%foreach (string imgSrc in Model.VehicleInfo.UploadPhotosUrl)
                      { %>
                    <a class="active" href="" data-slide-index="<%=index %>">
                        <div class="img-box">
                            <div class="img-row">
                                <div class="img-cell">
                                    <img src="<%=imgSrc %>">
                                </div>
                            </div>
                        </div>
                    </a>
                    <% index++; %>
                    <%} %>
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="vehicleInfo_area" id="vehicle_information">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Vehicle Information
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="vehicleInfo_area_content">
                <ul>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								VIN:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Vin %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Stock:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Stock %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Mileage:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Mileage %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Ext. Color:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.ExteriorColor %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Int. Color:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.InteriorColor %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Trans:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Tranmission %>
							</nobr>
                    </li>
                    <li>
                        <nobr class="vehicleInfo_area_key">
								Engine:
							</nobr>
                        <nobr class="vehicleInfo_area_value">
								<%=Model.VehicleInfo.Engine %>
							</nobr>
                    </li>
                </ul>
                <div class="vehicleInfo_area_cafax">
                    <div class="vehicleInfo_area_cafax_logo">
                        <a target="_blank" href="http://www.carfax.com/VehicleHistory/p/Report.cfx?vin=<%=Model.VehicleInfo.Vin%>">
                            <img src="/content/images/carfax-large.jpg" title="Click for full report" />
                        </a>
                    </div>
                    <div class="vehicleInfo_area_cafax_text">
                        *Not all accidents or other issues are reported to CARFAX. The number of owners
                        is estimated. See the full CARFAX Report for additional information and glossary
                        of terms. 23.Feb.2012 14:43:00
                    </div>
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="descriptions_area" id="descriptions_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Description
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="des_area_content">
                <%=Model.VehicleInfo.Description %>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <%   if (Model.VehicleInfo.StandardOptions != null )
             {%>
        <div class="standard_options_area" id="standard_options_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Standard Options
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="standard_option_area_content">
                <ul>
                    <%foreach (string tmp in Model.VehicleInfo.StandardOptions)
                      { %>
                    <li>
                        <%=tmp %></li>
                    <%} %>
                </ul>
                <div style="clear: both">
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
         <% } %>
        <%   if (Model.VehicleInfo.ExistOptions != null || Model.VehicleInfo.ExistPackages!=null)
             {%>
        <div class="standard_options_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
					Additional Options &amp; Packages
				</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="standard_option_area_content">
                <ul>
                    
                    <%
                 if (Model.VehicleInfo.ExistOptions != null) 
                 {
                     foreach (string tmp in Model.VehicleInfo.ExistOptions)
                      { %>
                    <li>
                        <%=tmp %></li>
                    <%}
                      } %>
                    <%
                 if (Model.VehicleInfo.ExistPackages != null)
                 {
                     foreach (string tmp in Model.VehicleInfo.ExistPackages)
                     { %>
                    <li>
                        <%= tmp %></li>
                    <% }
                 } %>
                </ul>
                <div style="clear: both">
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <% } %>
        <div class="dealerInfo_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Dealer Info
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="dealerInfo_area_content">
                <div class="dealerInfo_area_text">
                    <div class="dealerInfo_area_one">
                        <div class="dealerInfo_area_one_dealername">
                            <%=Model.Dealer.DealershipName %>
                        </div>
                        <div class="dealerInfo_area_one_address">
                            <%=Model.Dealer.DealershipAddress %>
                        </div>
                        <div class="dealerInfo_area_one_address">
                            <%=Model.Dealer.City %>, <%=Model.Dealer.State %>, <%=Model.Dealer.ZipCode %>
                        </div>
                        <div class="largemap">
                            <small><a target="_blank" href="https://maps.google.com/maps?q=<%=Model.Dealer.DealershipAddress %>&ie=UTF8&hq=&hnear=<%=Model.Dealer.DealershipAddress %>&gl=us&t=m&z=14&iwloc=near&vpsrc=0&output=embed"
                                style="color: #0000FF; text-align: left">Click for Driving Directions</a></small>
                        </div>
                    </div>
                    <div class="dealerInfo_area_two">
                        <div class="dealerInfo_area_two_name">
                            Ask For: <%=Model.Dealer.DealerSetting.EbayContactInfoName %>
                        </div>
                        <div class="dealerInfo_area_two_phone">
                            <%=Model.Dealer.DealerSetting.EbayContactInfoPhone %>
                        </div>
                        <div class="dealerInfo_area_two_email">
                            <%=Model.Dealer.DealerSetting.EbayContactInfoEmail %>
                        </div>
                    </div>
                    <div class="dealerInfo_area_btns">

                        <div class="dealerInfo_area_btns_tous">
                            <a href="<%=Model.Dealer.DealerSetting.WebSiteUrl %>">Send us an E-mail
                        
                            </a>
                        </div>

                        <div class="dealerInfo_area_btns_tofriend">
                            <a href="<%=Model.Dealer.DealerSetting.WebSiteUrl %>">Email this to a Friend
                            </a>
                        </div>

                    </div>
                </div>
                <div class="dealerInfo_area_map">
                   <%-- <iframe width="550" height="400" frameborder="0" scrolling="no" marginheight="0"
                        marginwidth="0" src="https://maps.google.com/maps?q=<%=Model.Dealer.DealershipAddress %>&ie=UTF8&hq=&hnear=<%=Model.Dealer.DealershipAddress %>&gl=us&t=m&z=14&iwloc=near&vpsrc=0&output=embed"></iframe>--%>
                    
                    <%=Model.Dealer.DealerSetting.DealerInfo %>

                    <br />
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="warranty_area" id="warranty_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Warranty Information
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="des_area_content">
                <%=Model.Dealer.DealerSetting.DealerWarranty %>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div class="term_conditons_area" id="term_conditions_area">
            <div class="image_slider_vhInfo">
                <nobr class="image_slider_vhInfo_text">
						Term &amp; Conditions
					</nobr>
                <div style="clear: both">
                </div>
            </div>
            <div class="des_area_content">
                <%=Model.Dealer.DealerSetting.TermConditon%>
            </div>
            <div class="backtotop">
                Back to Top
            </div>
        </div>
        <div id="brand">
            Powered by Vincontrol, LLC.
				        
				      
			
        </div>
    </div>
    <!-- END OF TEMPLATE -->
</body>
</html>
