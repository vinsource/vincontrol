<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.VehicleViewModel>" %>
<%@ Import Namespace="vincontrol.VinSell.Handlers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	VinSell | <%= Model.Year %> <%= Model.Make %> <%= Model.Model %> <%= Model.Trim %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/plot.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%=Url.Content("~/Css/validationEngine.jquery.css")%>" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        #ddcl-trim-filter
        {
            z-index: 100;
        }

        .ui-dropdownchecklist-text
        {
            text-overflow: ellipsis;
        }

        div.ui-dropdownchecklist
        {
            height: 250px !important;
            width: 176px !important;
            left: 956px !important;
            top: 500px !important;
            color: gray !important;
            margin-left: 2px;
            padding-top: 4px;
        }

        div.ui-dropdownchecklist-dropcontainer
        {
            height: 168px;
            overflow-y: auto;
            overflow-x: hidden;
            background-color: white !important;
            border: 1px solid #cdcdcd !important;
        }

        span.ui-dropdownchecklist-selector
        {
            width: 170px !important;
        }

        .ptl_charts_right
        {
            text-align: left !important;
        }

        .content_new_bg
        {
            display: none;
            z-index: 100;
            right: 70px;
            top: 20px;
            position: absolute;
            width: 400px;
            border: 2px solid #808080;
            background-color: #CDCDCD;
        }

        .content_view_title
        {
            padding-top: 5px;
            text-align: center;
            font-weight: bold;
            border-bottom: 1px solid #808080;
            height: 25px;
        }

        .bg_edit_content .bg_add_content
        {
            background-color: #FFF;
            height: 50px;
            margin: 0px;
            padding: 8px;
            font-size: 13px;
        }

        .bg_edit_btns
        {
            height: 25px;
            padding: 10px;
        }

            .bg_edit_btns > div
            {
                padding: 5px 15px;
                cursor: pointer;
                color: #FFF;
                background-color: #5C5C5C;
                float: right;
                font-size: 13px;
                margin-right: 5px;
            }

        .formError .formErrorContent
        {
            width: 100%;
            font-style: italic;
        }

        .grid .tick
        {
            stroke: rgba(0,0,0,.3);
        }

        .axis path, .axis line
        {
            fill: none;
            stroke: black;
            shape-rendering: crispEdges;
        }

        .axis text
        {
            font-family: sans-serif;
            font-size: 10px;
        }
    </style>
    
    <script src="<%=Url.Content("~/js/jquery.alerts.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/jquery.alerts.css")%>" rel="stylesheet" type="text/css" />
      
    <input type="hidden" id="isSold" value="<%=Model.IsSold %>"/>
    <input type="hidden" id="Id" name="Id" value="<%= Model.Id %>" />
    <input type="hidden" id="Vin" name="Vin" value="<%= Model.Vin %>" />
    <input type="hidden" id="Note" name="Note" value="<%= Model.Note %>" />
    <input type="hidden" id="IsFavorite" name="IsFavorite" value="<%= Model.IsFavorite %>" />
    <input type="hidden" id="PreviousId" name="PreviousId" value="<%= Model.PreviousId %>" />
    <input type="hidden" id="NextId" name="NextId" value="<%= Model.NextId %>" />
    <input type="hidden" id="hdnState" name="hdnState" />
    <input type="hidden" id="hdnRegionCode" name="hdnRegionCode" />
    <input type="hidden" id="hdnRegionName" name="hdnRegionName" />
    <input type="hidden" id="NeedToReloadPage" name="NeedToReloadPage" value="False" />
    <input type="hidden" id="IsCarsCom" name="IsCarsCom" value="false" />
    <input type="hidden" id="Options" name="Options" value="false" />
    <input type="hidden" id="Trims" name="Trims" value="" />
    <input type="hidden" id="IsCertified" name="IsCertified" value="false" />
    <input type="hidden" id="IsAll" name="IsAll" value="true" />
    <input type="hidden" id="IsFranchise" name="IsFranchise" value="false" />
    <input type="hidden" id="IsIndependant" name="IsIndependant" value="false" />
    <div class="inner-wrap">
     <script type="text/javascript">
         function popupRegion(state) {

             if (state == '') {
             } else {

                 $.fancybox({
                     href: '/Auction/GetAuctions?state=' + state,
                     'type': 'iframe',
                     'width': 600,
                     'height': 250,
                     'scrolling': 'no',
                     'hideOnOverlayClick': true,
                     //'centerOnScroll': true,
                     'onCleanup': function () {
                     },
                     onClosed: function () {
                         if ($("#hdnRegionCode").val() != "" && $("#hdnState").val() != "") {
                             blockUI(waitingImage);
                             window.location.href = "/Auction/GetVehicles?auctionCode=" + $("#hdnRegionCode").val() + "&auctionName=" + $("#hdnRegionName").val() + "&state=" + $("#hdnState").val();
                         }
                     }
                 });
             }
         }

     </script>
        <div class="page-info">
            <span>Auctions >  <a href="javascript:popupRegion('<%= Model.State%>');"> <%=  Model.State%></a><br />
            </span><span><b> <%= Html.ActionLink(Model.RegionName, "GetVehicles", "Auction", new { auctionCode = Model.Region, auctionName = Model.RegionName, state = Model.State }, null)%></b>
            </span>
            <h3><%= Model.Seller %> <small style="font-size: .65em">[  <%= Html.ActionLink("Lane " + Model.Lane, "GetVehiclesByLane", "Auction", new { auctionCode = Model.Region, auctionName = Model.RegionName, state = Model.State,lane = Model.Lane }, null)%>/ <%= Html.ActionLink("Run " + Model.Run, "GetVehiclesByRun", "Auction", new { auctionCode = Model.Region, auctionName = Model.RegionName, state = Model.State,Lane=Model.Lane,Run = Model.Run }, null)%> ]</small></h3>
        </div>
        <div class="car-controls">
            <% if (Model.IsSold) {%>
            <div style="display:inline-block;float:left;background-color:White;padding:3px;border:2px solid Red;font-weight:bold;color:Red;font-size:1.4em;">SOLD</div>
            <%}%>
            <% if (Model.PreviousId > 0) {%>
            <div class="prev-car">
                <img src='<%= Url.Content("~/Content/Images/car-control-left.jpg") %>'></div>
            <%}%>
            <% if (Model.NextId > 0) {%>
            <div class="next-car">
                <img src='<%= Url.Content("~/Content/Images/car-control-right.jpg") %>'></div>
            <%}%>
            <div class="favorite-vehicle">
                <input type="checkbox" name="favorite" id="favorite-checkbox" />
                <img src='<%= Url.Content("~/Content/Images/favorite-ico.png") %>' title="+ Add Favorite">
            </div>
            <h3><%= Model.Year %> <%= Model.Make %> <%= Model.Model %> <%= Model.Trim.Equals("Not Specified") ? "" : Model.Trim %></h3>
            <div style="float:right;margin-right:30px;"><img id="imgPrint" src='<%= Url.Content("~/Content/Images/print.png") %>' title="Print" style="cursor:pointer;border:0;" /></div>
        </div>
        <div id="divDetailVehicle" class="content" style="height: 750px !important;">
            <div class="vehicle" style="width:950px;">
                <div class="information">
                    <div class="images">
                        <div class="main-image">                            
                            <a href="<%=Url.Content("~/Manheim/OpenManaheimLoginWindow?url=")%><%=Model.Url%>" target="_blank">
                            <img src='<%= Model.Images.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray()[0] %>' />
                            </a>
                        </div>
                       
                        <div class="image-list">
                            <%
                                var imageArray = Model.Images.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray().Distinct();
                                foreach (var image in imageArray)
                            {
                            %>
                                <a class="single_image" rel="group1" href="<%= image %>">
                                <img src="<%= image %>" />
                                </a>
                            <%
                            }
                            %>                            
                        </div>
                    </div>
                    <div class="carInfo" style="height:auto;">
                        <div class="car-details">
                            <h3>Vehicle Information  <small style="font-size: .8em;font-weight: normal">(<%= Model.Vin %>)</small></h3>
                            <ul>
                               <%-- <li >VIN :</li>--%>
                                <li><b>Year: </b><%= Model.Year %></li>
                                <li><b>Make: </b><%= Model.Make %></li>
                                <li><b>Model: </b><%= Model.Model %></li>
                                <li><b>Trim: </b><%= Model.Trim %></li>
                                <li><b>Mileage: </b><%= Model.Mileage.ToString("0,0") %> mi</li>
                                <li><b>Body Style: </b><%= Model.BodyStyle %></li>
                                <li><b>Exterior. Color: </b><%= Model.ExteriorColor %></li>
                                <li><b>Interior Color: </b><%= Model.InteriorColor %></li>
                                 <li><b>Engine: </b><%= Model.Engine %></li>
                            </ul>
                            <ul>
                               
                                <li><b>Displacement: </b><%= Model.Litters %></li>
                                <li><b>Fuel Type: </b><%= Model.FuelType %></li>
                                <li><b>Transmission: </b><%= Model.Transmission %></li>
                                <li><b>Drive Train: </b><%= Model.DriveTrain %></li>
                                <li><b>Doors: </b><%= Model.Doors.Equals(0) ? "Not Specified" : Model.Doors.ToString() %></li>
                                <li><b>Stereo: </b><%= Model.Stereo %></li>
                                <li><b>Airbags: </b><%= Model.Airbags %></li>
                                <li><b>Interior Type: </b><%= Model.InteriorType %></li>
                            </ul>
                        </div>
                        
                        <div class="equipement">
                        <% if (Model.Equipments.Count > 0) {%>
                            <h3>Equipment</h3>
                            <ul>
                                <% foreach (var option in Model.Equipments.Skip(0).Take(9)){%>
                                    <li><%= option %></li>
                                <%}%>                                
                            </ul>
                            <ul>
                                <% foreach (var option in Model.Equipments.Skip(9).Take(9)){%>
                                    <li><%= option %></li>
                                <%}%>
                            </ul>
                        <%}%>
                            <div class="cr-large">
                                <div class="cr">CR</div>
                                <br /><%= Model.Cr %>
                            </div>
                            <div class="cr-data triangle-border right hidden">
                                <div class="cr-data-wrap">
                                    <b>Seller</b><br />
                                    <%= Model.Seller %><br />
                                    <b>Location</b><br />
                                    <%= Model.Region.Substring(0, 2) %> - <%= Model.RegionName %><br />
                                    <b>Sale Date</b><br />
                                    <%= Model.AuctionDate.ToString("MM/dd/yyy hh:mm:ss tt") %><br />
                                    <b>
                                    <%= Html.ActionLink("Lane " + Model.Lane, "GetVehiclesByLane", "Auction", new { auctionCode = Model.Region, auctionName = Model.RegionName, state = Model.State, lane = Model.Lane }, null)%>/ <%= Html.ActionLink("Run " + Model.Run, "GetVehiclesByRun", "Auction", new { auctionCode = Model.Region, auctionName = Model.RegionName, state = Model.State,Lane=Model.Lane,Run = Model.Run }, null)%> 
                                    </b><br />
                                </div>
                            </div>
                        </div>
                        
                        <% if (!String.IsNullOrEmpty(Model.Comment)) {%>
                        <div class="comment" style="display:inline-block;width:90%;">
                            <h3>Comments</h3>
                            <div style="font-size:0.7em;height:40px;display:inline-block;overflow:auto;"><%= Model.Comment %></div>
                        </div>
                        <%}%>
                        <div class="clear">
                        </div>
                    </div>
                     
                     <div class="pt_left_charts_holder">
                        <div class="ptl_charts_title">
                         

                            <a id="A1" href="<%= Url.Action("ViewFullChart", "Chart", new { ListingId = Model.Id}) %>">
                                <div class="btns_shadow ptl_charts_btn expend_market">
                                    <input type="button" style="margin-left: 0; border: none; background: none; color: white; padding-top: 2px; font-weight: bold; cursor: pointer"
                                        id="graphButton" name="toggleGraph"
                                        value="Expand" />
                                </div>
                            </a>
                          
                            <div class="ptl_charts_text">
                                Within (100 miles) from your location
                            </div>
                        </div>
                        <div class="ptl_chart_holder">
                         
                            <div class="ptl_charts_left">
                                <div class="ptl_charts" id="graphWrap" style="width: 400px; height: 183px; background-image: none;">
                                    <div id="placeholder" style="height: 100%; width: 100%; padding-left: 8px;" align="center">
                                        <img src="/content/images/ajaxloadingindicator.gif" />
                                    </div>
                                </div>
                                <div style="clear: both; border-top: solid 2px gray; height: 60px;">
                                    <div class="ptl_charts_ranking">
                                        <div class="ptl_chart_rangking_text">
                                            <div class="market_ranking_key">
                                                Market Ranking
                                            </div>
                                        </div>
                                        <div class="ptl_chart_avg">
                                            <nobr style="color: #458C00"><label id="txtAvgDays">
                                            0</label></nobr>
                                            Avg Days
                                        </div>
                                    </div>
                                    <div style="float: left; width: 100%; padding-left: 30px;">
                                        <div class="ptl_charts_right_items" style="margin-top: 0px;">
                                            <div class="ptl_charts_right_icons kpi_market_above_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #D12C00">
                                                <label id="txtMaxPrice">
                                                    0</label>
                                            </div>
                                        </div>
                                        <div class="ptl_charts_right_items">
                                            <div class="ptl_charts_right_icons kpi_market_equal_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #458C00">
                                                <label id="txtAveragePrice">
                                                    0</label>
                                            </div>
                                        </div>
                                        <div class="ptl_charts_right_items">
                                            <div class="ptl_charts_right_icons kpi_market_below_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #0062A1">
                                                <label id="txtMinPrice">
                                                    0</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="ptl_charts_right">
                                <div class="market_ranking_value">
                                    <label id="txtRanking">
                                        0
                                    </label>
                                    out of
                                    <label id="txtCarsOnMarket">
                                        0</label>
                                </div>

                                <div>
                                    <div id="divTrims" style="display: inline-block; text-overflow: ellipsis; white-space: nowrap; overflow: hidden; width:140px; background-color: #222; color: white; padding-left: 17px; font-size: 13px;">
                                        &nbsp;
                                    </div>
                                    <div id="trim-filter" style="padding: 5px; height: 155px; overflow-y: auto; font-size: 13px;">
                                    </div>
                                  
                                    <div class="btns_shadow profile_top_items_btn" id="SaveSelectionSmallChart" style="margin: 10px 5px;">
                                        Save
                                    </div>
                                   
                                </div>
                            </div>
                          
                        </div>
                    </div>
                
                </div>
                 <div class="data">
                   
                <div class="data-box" id="carfax">
                        <div class="header">
                            <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=&vin=5XYZU3LB5EG149127')">
                            <img src="/Content/Images/carfax.jpg">
                            </a>
                        </div>
                        <div class="subheader-nav">
                                <div class="ptr_cafax_owners">
                                    <nobr class="ptr_cafax_number" id="carfaxOwner">
									</nobr>
                                    Owner(s)
                                </div>
                          
                             <div class="ptr_cafax_service_repords">
                                    <nobr class="ptr_cafax_number" id="carfaxRecord">
									    
								    </nobr>
                                    Service Reports
                                </div>
                        </div>
                           <div id="history-report" class="data-content" style="height: 90px;">
                                <img  src="/content/images/ajaxloadingindicator.gif" />
                                
                                
                            </div>
                    </div>
                   
                   <div class="data-box" id="manheim">
                          <div class="ptr_items_top" id="controlappraisals_manheim">
                            <a title="MMR Auction Pricing" href="JavaScript:newPopup('<%=Url.Content("~/Manheim/OpenManaheimLoginWindow?Vin=")%><%=Model.Vin%>')">
                            <div class="ptr_items_top_title_holder">
                                <div class="ptr_items_top_title_logo">
                                    
                                        <img src="/content/images/logo_manheim.gif" />
                                </div>
                                <div class="ptr_items_top_title_text">
                                    Manheim
                                </div>
                            </div>
                                </a>
                            <div class="ptr_item_top_circle circleBase" id="ManheimCount">
                                0
                            </div>
                        </div>
                        <div class="ptr_items_contentKBB">
                            <div class="ptr_items_content_list partialContents" id="ManheimContent">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                            </div>
                        </div>
                    </div>
                     <div class="data-box" id="kbb">
                          <div class="ptr_items_top" id="controlappraisals_kbb">
                            <div class="ptr_items_top_title_holder">
                                <div class="ptr_items_top_title_logo">
                                    <img src="/content/images/kbb-logo-alpha.png" />
                                </div>
                                <div class="ptr_items_top_title_text">
                                    KBB
                                </div>
                            </div>
                            <div class="ptr_item_top_circle circleBase" id="KBBCount">
                                0
                            </div>
                        </div>
                        <a style="float: right; font-weight: normal; font-size: .8em; display: block; margin-top: -18px;"
                            href="<%= Url.Action("ResetKbbTrim", "Market", new { listingId = Model.Id }) %>">Not a correct trim? Click here</a>
                        <div class="ptr_items_contentKBB">
                            <div id="divPartialKbb" class="ptr_items_content_list partialContents">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                            </div>
                        </div>

                    </div>
                     
                        <div class="data-box" id="auction">
                            <div class="ptr_items_top" id="controlappraisals_auction">
                            <div class="ptr_items_top_title_holder">
                                <div class="ptr_items_top_title_logo"><img src="/content/images/logo_manheim.gif" /></div>
                                <div class="ptr_items_top_title_text">Auction</div>
                            </div>
                          
                        </div>
                        <div class="ptr_items_contentKBB">
                            <div id="divPartialAuction" class="">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                              
                            </div>
                        </div>
                    </div>
                          <div class="data-box" id="note">
                            <div class="ptr_items_top" id="controls_note">
                            <div class="ptr_items_top_title_holder">
                               <div class="ptr_items_top_title_text">Notes</div>
                            </div>
                          
                        </div>
                        <div class="ptr_items_contentKBB">
                              <textarea class="notes" id="vehicleNote" ><%= Model.Note %></textarea>
                        </div>
                    </div>
               
                </div>
            </div>
        </div>
        <!-- end of inner wrap div-->
        

    </div>
     <%=Html.Partial("_TemplateCarFaxData")  %>
     <%=Html.Partial("_TemplateManheimData")  %>
    <%=Html.Partial("_TemplateKarPowerData")  %>
    <%=Html.Partial("_TemplateAuctionData")  %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
<link href="<%=Url.Content("~/Content/vehicle.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/VinControl/common.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/extension.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.dragsort.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/jquery.flot.functions.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.validationEngine-en.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.validationEngine.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jsrender.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ui.dropdownchecklist-1.4-min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/windowsticker.js")%>" type="text/javascript"></script>
        
    <script src="<%=Url.Content("~/js/Chart/MarketMapping.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/google_map_graph_plotter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/ChartSmall.js")%>" type="text/javascript"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/d3/d3.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.d3.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/return-large.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/func.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/chart.js")%>"></script>
<script type="text/javascript">
    function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
    var baseUrl = '/Content/Images';
    var webAPI = '<%=System.Web.Configuration.WebConfigurationManager.AppSettings["WebAPIServerURL"]%>';
    var carfaxUrl = webAPI + '/Vinsell/CarFaxData?listingId=_listingId&dealerId=_dealerId';
    carfaxUrl = carfaxUrl.replace('_listingId', '<%=Model.Id%>').replace('_dealerId', '<%=Model.DealerId%>');

    var manheimUrl = webAPI + '/Vinsell/ManheimData?listingId=_listingID&dealerId=_dealerId&inventoryStatus=_inventoryStatus';
    manheimUrl = manheimUrl.replace('_listingID', '<%=Model.Id%>').replace('_inventoryStatus', 0).replace('_dealerId', '<%=Model.DealerId%>');

    var karPowerUrl = webAPI + '/Vinsell/KarPowerData?listingId=_listingID&dealerId=_dealerId&zipCode=1';
    karPowerUrl = karPowerUrl.replace('_listingID', '<%=Model.Id%>').replace('_dealerId', '<%=Model.DealerId%>');

    var auctionUrl = webAPI + '/Auction/AuctionDataOnVinsell?listingId=_listingID&dealerId=_dealerId';
    auctionUrl = auctionUrl.replace('_listingID', '<%=Model.Id%>').replace('_dealerId', '<%=Model.DealerId%>');
            
    // grab graph div element and click element
    var window_url = window.location.search;

    // default graph to unexpanded draw size
    var expand = expanded(window_url);
    var chart_dimensions = ["400px", "183px"];
    // set chart dimensions
    var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange: { min: 0, max: 100 }, isSoldView: false, isSmallChart: false };
    ChartInfo.isSmallChart = true;
    var $currentFilterredList = [];
    var default_trim;
    var gwrap = $('#graphWrap');
    gwrap.css('width', chart_dimensions[0]);
    gwrap.css('height', chart_dimensions[1]);

    var UrlPaths = UrlPaths || { requestNationwideUrl: "" };

    UrlPaths.requestNationwideUrl = webAPI + '/Vinsell/GetMarketDataByListingNationwideWithHttpPost?listingId=_listingID&dealerId=_dealerId&chartScreen=_chartScreen';
    UrlPaths.requestNationwideUrl = UrlPaths.requestNationwideUrl.replace("_listingID", '<%=Model.Id%>').replace("_dealerId", '<%=Model.DealerId%>').replace("_chartScreen", "Inventory");

    $(document).ready(function () {
        //$("a.single_image").fancybox();
       
        $.ajax({
            type: "POST",
            url: carfaxUrl,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var carfaxId = 0;
                var vin = '<%=Model.Vin%>';
                var obj = {
                    "ListItems": [],
                    "CarfaxdealerId": carfaxId,
                    "Vin": vin,
                };
                $.each(result.ReportList, function (i, item) {
                    obj.ListItems[i] = item;
                });

                if (result.NumberofOwners == 0)
                    $("#carfaxOwner").html("-");
                else
                    $("#carfaxOwner").html(result.NumberofOwners);
                $("#carfaxRecord").html(result.ServiceRecords);

                $("#history-report").attr("align", "");
                $("#history-report").html($("#CarFaxTemplate").render(obj));

            },
            error: function (err) {
                console.log('Error');
                console.log(err.status + " - " + err.statusText);
            }
        });

        $.ajax({
            type: "POST",
            url: manheimUrl,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
              
                var obj = {
                    "ListItems": []
                };
                $.each(result, function (i, item) {
                    obj.ListItems[i] = item;
                });
               
                $("#ManheimContent").html($("#ManHeimTemplate").render(obj));
                if (result.length > 0) {
                    $('#ManheimCount').html(result.length);
                    $("a.iframeManHeim").fancybox({ 'width': 790, 'height': 400, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                   
                    showDetailMainHeim(result[0].HighestPrice, result[0].AveragePrice, result[0].LowestPrice, result[0].Year, result[0].MakeServiceId, result[0].ModelServiceId, result[0].TrimServiceId);
                    $('#DDLManHeim').bind('change', function (e) {
                        var value = e.target.options[e.target.selectedIndex].value;
                        var arr = value.split('|');
                        showDetailMainHeim(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6]);
                    });
                } else {
                    $('#ManheimCount').html('0');
                }
            },
            error: function (err) {
                console.log('Error');
                console.log(err.status + " - " + err.statusText);
            }
        });

        $.ajax({
            type: "POST",
            url: karPowerUrl,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var obj = {
                    "ListItems": []
                };
                $.each(result, function (i, item) {
                    obj.ListItems[i] = item;
                });
                $("#divPartialKbb").html($("#KBBTemplate").render(obj));
                if (result.length > 0) {
                    $('#KBBCount').html(result[0].TotalCount == 1 && result[0].SelectedTrimId == 0 ? '' : result[0].TotalCount);
                    $("a.iframeKBB").fancybox({ 'width': 790, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                   
                    showDetailKBB(result[0].Wholesale, result[0].MileageAdjustment, result[0].BaseWholesale, result[0].AddsDeducts, result[0].SelectedTrimId, result[0].SelectedModelId);
                    $('#DDLKBB').bind('change', function (e) {
                        console.log("CHANGE");
                        var value = e.target.options[e.target.selectedIndex].value;

                        var arr = value.split('|');
                        showDetailKBB(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]);
                    });
                } else {
                    $('#KBBCount').html('0');
                }
            },
            error: function (err) {
                console.log('Error');
                console.log(err.status + " - " + err.statusText);
            }
        });
        $.ajax({
            type: "POST",
            url: auctionUrl,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var obj = {
                    "ListItems": []
                };
                $.each(result, function (i, item) {
                    obj.ListItems[i] = item;
                });
                $("#divPartialAuction").html($("#AuctionTemplate").render(obj));
                if (result.length > 0) {

                    $("a.iframeManHeim").fancybox({ 'width': 790, 'height': 400, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                    $('#NationPrice').html(money_format(obj.ListItems[0].AvgAuctionPrice));
                    $('#RegionPrice').html(money_format(obj.ListItems[1].AvgAuctionPrice));
                    $('#NationOdometer').html(miles_format(obj.ListItems[0].AvgOdometer));
                    $('#RegionOdometer').html(miles_format(obj.ListItems[1].AvgOdometer));


                }
            },
            error: function (err) {
                console.log('Error');
                console.log(err.status + " - " + err.statusText);
            }
        });
      
        if ($('#IsFavorite').val() == 'True') {

            $('.favorite-vehicle img').prop('src', baseUrl + '/favorite-ico-on.png');
            $('.favorite-vehicle img').prop('title', '- Remove Favorite');

            $('#favorite-checkbox').prop('checked', true);
        } else {
            $('.favorite-vehicle img').prop('src', baseUrl + '/favorite-ico.png');
            $('.favorite-vehicle img').prop('title', '+ Add Favorite');
            $('#favorite-checkbox').prop('checked', false);
        }

        // CR Detail Page Hover
        $('.cr-large').mouseenter(function () {
            $('.cr-data').removeClass('hidden');
        }).mouseleave(function () {
            $('.cr-data').addClass('hidden');
        });

        $('.cr-data').mouseenter(function () {
            $(this).removeClass('hidden');
        }).mouseleave(function () {
            $(this).addClass('hidden');
        });

        $('.cr').click(function () {
            var url = '<%= Model.CrUrl %>';
            if (url != '') {
                newPopup(url, 700, 800);
            } else {
                jAlert('The condition report is not available', 'Warning!');
            }
        });

    
        // favorite vehicle event
        $('.favorite-vehicle img').click(function () {
            var img = $(this);
            var favorite = $('#favorite-checkbox');

            if (favorite.prop('checked') === true) {
                img.prop('src', baseUrl + '/favorite-ico.png');
                img.prop('title', '+ Add Favorite');
                favorite.prop('checked', false);

                // can update database here, or wait till
                // all page data is saved

            } else if (favorite.prop('checked') === false) {
                img.prop('src', baseUrl + '/favorite-ico-on.png');
                img.prop('title', '- Remove Favorite');
                favorite.prop('checked', true);

                // can update database here, or wait till
                // all page data is saved
            }

            $.ajax({
                type: "GET",
                dataType: "html",
                url: '/Auction/MarkFavorite?vehicleId=' + $('#Id').val(),
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {

                },
                error: function (err) {
                    //jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');

                }
            });
        });

        $('#vehicleNote').mouseenter(function () {

        }).mouseleave(function () {
            console.log("mouse enter");
            if ($('#vehicleNote').val() != $('#Note').val()) {
                $.ajax({
                    type: "GET",
                    dataType: "html",
                    url: '/Auction/MarkNote?vehicleId=' + $('#Id').val() + '&note=' + $('#vehicleNote').val(),
                    data: {},
                    cache: false,
                    traditional: true,
                    success: function (result) {
                        $('#Note').val($('#vehicleNote').val());
                    },
                    error: function (err) {
                        //jAlert('System Error: ' + err.status + " - " + err.statusText, 'Warning!');
                    }
                });
            }
        });

        // next car controls at top left
        $('.next-car').click(function () {
            window.location.href = '/Auction/DetailVehicle/' + $('#NextId').val();
        });

        $('.prev-car').click(function () {
            window.location.href = '/Auction/DetailVehicle/' + $('#PreviousId').val();
        });

        $('img#imgPrint').click(function () {
            printContent();
        });

        $("#chartgraphlink").live('click', function () {
            //console.log($("#isSold").val().toLowerCase());
            if ($("#isSold").val().toLowerCase() == 'false') {
                $.fancybox({
                    href: '/Chart/ViewFullChart?ListingId=' + '<%= Model.Id%>',
                    'type': 'iframe',
                    autoDimensions: false,
                    'width': 1010,
                    'height': 700,
                    'scrolling': 'yes',
                    'hideOnOverlayClick': true,
                    //'centerOnScroll': true,
                    'onCleanup': function () {
                    },
                    onClosed: function () {

                    }
                });
            }
        });

    });

    function showDetailMainHeim(HighestPrice, AveragePrice, LowestPrice, year, make, model, trim) {
        try {
            $('#divHighestPriceManHeim').html(money_format(HighestPrice));
            $('#divAveragePriceManHeim').html(money_format(AveragePrice));
            $('#divLowestPriceManHeim').html(money_format(LowestPrice));

            var urldetail = '<%=Url.Content("~/Manheim/ManheimTransactionDetail?year=_Year&make=_Make&model=_Model&trim=_Trim")%>';
            urldetail = urldetail.replace('_Year', year).replace('_Make', make).replace('_Model', model).replace('_Trim', trim);
            $('#lnkDetailManHeim').attr('href', urldetail);
        }
        catch (e)
        { }
    }

    function showDetailKBB(Wholesale, mileageAdjustment, baseWholesale, addsdeducts, trimID, modelID) {
        try {

            $('#divWholeSale').html(money_format(Wholesale));
            $('#divMileageAdjustment').html(money_format(mileageAdjustment));
            $('#divBaseWholesale').html(money_format(baseWholesale));
            $('#divAddDeducts').html(money_format(addsdeducts));



            var urldetail = '<%= Url.Action("GetSingleKarPowerSummary", "KarPower", new { listingId = "_listingID", trimId = "_trimID", modelId = "_modelID" }) %>';
            urldetail = urldetail.replace('_trimID', trimID);
            urldetail = urldetail.replace('_modelID', modelID);
            urldetail = urldetail.replace('_listingID', $('#Id').val());
            $('#lnkDetail').attr('href', urldetail);

        } catch (e) {
        }
    }

    function printContent() {
        var mywindow = window.open('/Auction/PrintVehicle/' + $('#Id').val(), 'Profile', 'height=800,width=990');

        return true;
    }

</script>
</asp:Content>
