<%@ Page Title="Appraisal Profile"  MasterPageFile="~/Views/Shared/Site.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalViewFormModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title><%=Model.ModelYear %> <%=Model.Make %> <%=Model.AppraisalModel %></title>

       
<link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />

<link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />

<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />

<%--<script src="http://code.jquery.com/jquery-latest.js" type="text/javascript"></script>--%>
<script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

<script src="<%=Url.Content("~/js/jquery.dragsort.js")%>" type="text/javascript"></script>

<script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery.flot.js")%>" type="text/javascript"></script>

<script src="<%=Url.Content("~/js/jquery.flot.symbols.js")%>" type="text/javascript"></script>


<script src="<%=Url.Content("~/js/jquery.flot.functions.js")%>" type="text/javascript"></script>

<link href="<%=Url.Content("~/js/plot.css")%>" rel="stylesheet" type="text/css" />

<link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" />
<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>

<style type="text/css">

.scroll-pane {height: 100%; overflow:auto; overflow-x: hidden;}

#notes {height: 200px; margin-bottom: 6em;}

#carInfo {background: #333; padding: .6em; margin-top: 2em;}

#carInfo h3{ margin-top: 0;}

#figures {margin-bottom: -35px;}

#carInfo {background: #980000; padding: .6em; margin-top: 1.1em;}

#carInfo h3{ margin-top: 0;}

#references{margin-top:1em !important;}

#listImages{overflow-x: hidden !important;}

.sortMiles:hover{cursor: pointer;color:red;

}.sortMiles{color: white;font-weight:bold;

}

#t1 {position: relative; top: -16px;} 

#sorting { width: 500px;}

#filter {width: 20px; height: 20px; background: url('../images/filter2.gif') top left no-repeat; float: left; clear: right; margin-right: .3em;}

#filter span {display: none;}

input[name="acv"] {background: green; color: white;}

input[name="notes"] {background: black !important;}

.topbtns {margin-top: 0 !important;}

input[name="edit"] {
	position: relative;
	margin-top: 0;}

#carfax .number,
#carfax .text {
	font-size: .95em !important;
	display: inline-block;
	overflow:hidden;
	padding: .1em .4em .1em .4em;
	height:100%;
	border: 4px #111 solid;}

#carfax .oneowner,
#carfax .text {
	font-size: .95em !important;
	display: inline-block;
	overflow:hidden;
	padding: .1em .4em .1em .4em;
	height:100%;
	border: 4px #111 solid;}

#carfax .text {
	margin-left: 0;
	background: #111;}
	
#carfax .number {
	margin-right: 0;
	background: #c80000;
	overflow: hidden;
	color: white;
	font-weight: bold;}
#carfax .oneowner
{
	margin-right: 0;
	background: green;
	overflow: hidden;
	color: white;
	font-weight: bold;
}
	
#carfax .carfax-wrapper {
	margin-top: 10px;
	display: inline;
	width: 225px;}

#carfax-header,
#report-wrapper,
#summary-wrapper {
	display: block;
	}

#summary-wrapper {
	position: absolute;
	right: 0;
	top: 0;}

#carfax-header img {
	margin-right: 5px;}

#report-wrapper img {
	float: right;
	margin-right: 10px;
	margin-top: 3px;}

#carfax-header h3 {
	margin-top: 3px;}

#report-wrapper ul,
#report-wrapper li {
	margin: 0; 
	padding: 0;}

#report-wrapper ul {
	margin-top: 5px;}

#report-wrapper li {
	width: 100%;
	background: #111;
	margin-top: 4px;
	padding: .2em .5em .2em .5em;}

#carfax {
	position: relative;
	margin-bottom: 8px !important;}

#kbb div, #kbb h3, #kbb span,
#bb div, #bb h3, #bb span,
#manheim div, #manheim h3, #manheim span  
{
	display: inline;}

#kbb, #bb, #manheim {
	position:relative;
	overflow:hidden;
	width: 541px !important;
	margin-top: 8px !important;}

#kbb #kbb-row,
#bb #bb-row, #manheim #manheim-row {
	background: #860000;
	border: 4px solid #860000;
	white-space:nowrap;
	display: inline-block;
	width: 560px !important;
	min-height: 25px;}


	
#kbb .range-item,
#bb .range-item, #manheim .range-item {
display:inline-block;
width: 122px;
padding: 3px 4px 3px 4px;
background: #555;
margin-right: 0px;}

.range-item.label {
width: 120px !important;
}


#kbb h3,
#bb h3, #manheim h3 {
	background: #111;
	padding: 4px 5px 4px 5px !important;
	overflow: hidden;
	border-bottom: 3px #111 solid;
	cursor: pointer;}

.mr-on {
	background: #860000 !important;
	border-bottom: 3px #860000 solid;}

.high,
.mid,
.high,
.low-wrap,
.mid-wrap,
.high-wrap {
	padding-left: 10px;
	font-weight: bold;}
	
.high {
	background: #d02c00 !important;}
	
.mid {
	background: #008000 !important;}
	
.low {
	background: #0062A0 !important;}
	.hideLoader {display: none;}

#placeholder {background: url('../images/loading.jpg') center no-repeat !important;}
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
 				<div id="dropdownWrap">
 			        <% Html.BeginForm("Action", "Appraisal", FormMethod.Post); %>		
              
                    <div class="clear"></div>
                          
                        </div>     
                         <div id="divReportButton" class="clear">
                        <%=Html.ReportButtonGroupForAppraisal() %>
           		        </div>
                <div id="detailWrap">                        
                <div id="images" class="column" style="margin: 0 !important;">
                
                	<%=Html.DynamicHtmlLabel("txtCarImage", "CarImage")%>
                    <%=Html.HiddenFor(x=>x.AppraisalGenerateId) %>
                    <%=Html.HiddenFor(x=>x.VinNumber) %>
                	<input type="hidden" id="NeedToReloadPage" name="NeedToReloadPage" value="false" />
                    <div class="clear"></div>
               	</div>
               <div id="profileInfo">
                    <h3><%=  Html.DynamicHtmlLabel("txtTitle", "Title")%></h3>
                    <ul class="column">
                    	<li><%=  Html.DynamicHtmlLabel("txtLabelVin", "Vin")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelAppraisal", "AppraisalID")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelStock", "Stock")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelYear", "Year")%> </li>
                        <li> <%=  Html.DynamicHtmlLabel("txtLabelMake", "Make")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelModel", "Model")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelTrim", "Trim")%> </li>
                        <li style="width:140px;"><%=  Html.DynamicHtmlLabel("txtLabelExteriorColor", "ExteriorColor")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelInteriorColor", "InteriorColor")%></li>
                    </ul>
                    <ul class="column">
                    	<li> <%=  Html.DynamicHtmlLabel("txtLabelTranmission", "Tranmission")%></li>
                    	<li><%=  Html.DynamicHtmlLabel("txtLabelOdometer", "Odometer")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelCylinders", "Cylinders")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelLitters", "Litters")%></li>
                          <li><%=  Html.DynamicHtmlLabel("txtLabelDoors", "Doors")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelFuelType", "FuelType")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelMSRP", "MSRP")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelDriveType", "DriveType")%></li>
                    </ul>
                    <div id="description" class="clear">
                		
                        <p class="bot"><%=  Html.DynamicHtmlLabel("txtDescription", "Description")%>
                    
                        </p>
                    </div>
                </div>
                
                
                
              
                <div id="figures" style="overflow: hidden; margin-bottom: 0">
                    	
                        <div id="graph" class="column" style="font-size: .8em;
">
                           
                        	<div id="rangeNav"><input type="button" style="margin-left: 0;" id="graphButton" name="toggleGraph" value="Expand" />  Within (100 miles) from your location
                            
                              </div>
                              <div id="graphWrap">
                                    <div id="placeholder" style="height: 100%; width: 100%;"></div> 
                               </div>
                            <div id="sorting">
                                <br/>
                       
                           
                           </div>
                           	<%=Html.ExpandChartButton()%>
                           
                        </div>
             <div id="age" class="column" style="margin-top: 20px !important">
                
                <h4 style="margin-top: 0;margin-bottom: 0">
                    Ranks 
                    <span id="totalCars" class="green" style="font-size: .8em">
                        <label id="txtRanking">
                            0
                        </label>
                        out of
                        <label id="txtCarsOnMarket">
                            0</label>
                    </span> on Market
                </h4>
                <p class="top">
                </p>
                <p class="bot">
                </p>
                <h3 style="margin-top: 0;margin-bottom: 0">
                    Market Price Ranges</h3>
                <h4 style="margin-top: 0; margin-bottom: .5em;">
                    <span class="blue">
                        <label id="txtMinPrice">
                            0</label></span>|<span class="green"><label id="txtAveragePrice">0</label></span>|<span
                                class="red"><label id="txtMaxPrice">0</label></span></h4>
                <p class="top">
                </p>
                <p class="bot">
                </p>
                <h4 style="width: 220px; margin-top: 0;margin-bottom: 0">
                    <span class="green">
                        <label id="txtAvgDays">
                            0</label></span> Avg Days in Inventory</h4>
            </div>
                    </div>
                    
              
                   <div id="references">
                    <p class="top"></p>
                    <p class="bot"></p>
                    
                    <%if (!Model.IsAppraisedByYear)
                      { %>
                          <div id="carfax" style="
    overflow: hidden;
">
                                <div id="carfax-header" style="display: block;width: 100%;">
                 <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%= Model.CarFaxDealerId %>&vin=<%= Model.VinNumber %>')">
                        <img style="display: inline-flexbox; float: left;" src='<%= Url.Content("~/Images/carfax-large.jpg") %>'
                            alt="carfax logo" /></a>
                    <h3 style="">
                        Summary</h3>
                </div>
                <% if (Model.CarFax!=null && Model.CarFax.Success)
                   {%>
                <div id="summary-wrapper">
                    <div id="owners" class="carfax-wrapper">
                        <% if (Model.CarFax.NumberofOwners.Equals("0"))
                           { %>
                        <div class="number">
                            -</div>
                        <% }
                           else if (Model.CarFax.NumberofOwners.Equals("1"))
                           { %>
                        <div class="oneowner">
                            <%= Model.CarFax.NumberofOwners %></div>
                        <% }
                           else
                           {%>
                        <div class="number">
                            <%= Model.CarFax.NumberofOwners %></div>
                        <% } %>
                        <div class="text">
                            Owner(s)</div>
                    </div>
                    <div id="reports" class="carfax-wrapper">
                        <div class="number">
                            <%= Model.CarFax.ServiceRecords %></div>
                        <div class="text">
                            Service Reports</div>
                    </div>
                </div>
                <% } %>
              <div id="report-wrapper" style="
    width: 100%;
">
                      <div id="history-report" style="
    clear:  both;
    float: left;
    width: 100%;
">
                        <ul>
                            <% foreach (WhitmanEnterpriseMVC.Models.CarFaxWindowSticker tmp in Model.CarFax.ReportList)
                               { %>
                            <li>
                                <%= tmp.Text %>
                                <img class="c-fax-img" src="<%= tmp.Image %>" />
                            </li>
                            <% } %>
                        </ul>
                             <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%= Model.CarFaxDealerId %>&vin=<%= Model.VinNumber %>')">
               View
                            Full Report</a>
                    </div>
                </div>
                <%--<textarea  class="z-index" id="Textarea3" name="description" cols="55" rows="15" ><%=ViewData["CarFaxResponse"]%></textarea>--%>
            </div>
            <% } %>
                            <%if ((bool)Session["IsEmployee"] == false) {%>
                      <div id="manheim">
                             <h3 class="mr-on">
                               <%if (!String.IsNullOrEmpty(Model.VinNumber))
                                 {%>

                               <a target="_blank" style="padding-right: 15px;" target="_blank" title="MMR Auction Pricing"
                                   href="<%= Url.Content("~/Market/OpenManaheimLoginWindow?Vin=") %><%= Model.VinNumber %>">
                                   <% }
                                 else
                                 {
                                     if (!String.IsNullOrEmpty(Model.SampleVin))
                                     { %>
  
       <a target="_blank" style="padding-right: 15px;" target="_blank" title="MMR Auction Pricing"
                                   href="<%= Url.Content("~/Market/OpenManaheimLoginWindow?Vin=") %><%= Model.SampleVin %>">
           <% }
                                     else
                                     { %>
                                        <a target="_blank" style="padding-right: 15px;" target="_blank" title="MMR Auction Pricing"
                                   href="<%= Url.Content("~/Market/OpenManaheimLoginWindowNoVin") %>
                                    <%
                                      }
                                 }%>

                                   <img height="20" style="position: relative; top: 2px;" src="http://www.microsoft.com/southafrica/microsoftservices/images/logo/logo_manheim.gif"
                                       alt="MMR Auction Pricing" />
                                   Manheim 
                               </a>
                           </h3>
                           
                          <div class="partialContents" data-url="<%= Url.Action("ManheimData", "Appraisal", new { listingId = Model.AppraisalGenerateId }) %>">
                              <div id="manheim-row" class="mr-row">
                                  Loading...
                              </div>
                          </div>
                           
                       </div> 
                                       <div id="mkWrap">
                            
                            <div id="kbb">
                            
                                	<h3 class="mr-on">
                                    	<img height="20" style="position:relative; top: 2px;" src="http://cdn.autofusion.com/apps/mm4/4.1/images/extras/kbb-logo-alpha.png" /> 
                                        Kelly Blue Book
                                    </h3> 
                                    
                                    <h3 style="padding:4px;">
                                    	<img height="25" style="position:relative; top: 4px;"  src=<%=Url.Content("~/images/bblogo.png")%> />
                                        Black Book
                                   </h3>                                 
                             <a style=" display: inline-block; cursor: pointer;height: 17px; font-size: .7em" title="DMV Desk" onclick="window.open('https://secure.dmvdesk.com/dmvdesk/', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')" >
                                  
                            		<span style="position: relative; top: -5px;">DMV Desk</span>
                               </a>
                               
                               	 <a style=" display: inline-block; cursor: pointer;  height: 17px; font-size: .7em" title="KSR" onclick="window.open('https://www.dmvlink.com/online/default.asp', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')">
                                | 
                            		<span style="position: relative; top: -5px;">KSR</span>
                               </a>                           
                                <div class="partialContents" data-url="<%= Url.Action("KarPowerData", "Appraisal", new { listingId = Model.AppraisalGenerateId }) %>">
                                    <div id="kbb-row" class="mr-row">
                                        Loading...
                                    </div>
                                </div>
                            
                            <%--<div id="bb" class="hider">
                            
                            	    <h3>
                                    	<img height="20" style="position:relative; top: 2px;" src="http://cdn.autofusion.com/apps/mm4/4.1/images/extras/kbb-logo-alpha.png" /> 
                                        Kelly Blue Book
                                    </h3> 
                                    
                                    <h3 style="padding:4px;" class="mr-on">
                                    	<img height="25" style="position:relative; top: 4px;" src=<%=Url.Content("~/images/bblogo.png")%> />
                                        Black Book
                                   </h3>                                 
                                
                                <a class="iframe" style="display: inline-block; cursor: pointer; margin-top: 8px;
                                    margin-left: 10px; height: 17px; position: relative; top: -2px; font-size: .7em"
                                    title="MMR Auction Pricing" href="<%=Url.Content("~/Report/ViewManheimReportForAppraisal?AppraisalId=")%><%=Model.AppraisalGenerateId%>">
                                    <span style="position: relative; top: -5px;">Manheim</span>
                                    <img style="position: relative; display: inline-block" alt="manheim market research"
                                        src="http://www.microsoft.com/southafrica/microsoftservices/images/logo/logo_manheim.gif"
                                        height="20" />
                                </a>
                             <%
                                               if (Model.BB.Success)
                                               {
                                                   foreach (
                                                       WhitmanEnterpriseMVC.Models.BlackBookTrimReport trimDetail in
                                                           Model.BB.TrimReportList)
                                                   {
%>
                                	<div id="bb-row" class="mr-row">
                                 <div class="range-item label">
                            	  		<font size='2' ><%=trimDetail.TrimName%></font>
                            	  </div>
                                   <div class="low-wrap range-item low">
                                 		<span class="bb-title">Low</span> <span class="bb-price"><%=trimDetail.TradeInRough%></span>
                                    </div>
                                    <div class="mid-wrap range-item mid">
                                 		<span class="bb-title">Mid</span> <span class="bb-price"><%=trimDetail.TradeInAvg%></span>
                                    </div>
                                    <div class="high-wrap range-item high">
                                 		<span class="bb-title">High</span> <span class="bb-price"><%=trimDetail.TradeInClean%></span>
                                    </div>
                                   </div>
                                        <%
                                                   }%>
                                   
                               <%
                                               }
                                               else
                                               {%>
                                <div id="bb-row" class="mr-row">
                                        There is no Black Book value associated with this vehicle
                                        </div>
                                       
                                       
                               <%
                                               }%>
                                <%=Html.ReportBlackBookButton()%>
                                </div>--%>
                           
                              
                            </div>
                            <%}%>
                    	</div>
                    	</div>
                    	</div>
            	
            </div>
         
            <div id="c3" class="column">
            <div id="listImages">
            	<h3><b style="color: white; font-size: 1em;">Customer Information</b></h3>
            
                	<div id="space" class="label">First Name: </div><%=Html.TextBoxFor(x => x.CustomerFirstName, new { @class = "z-index" })%>
                    <div class="label">Last Name: </div><%=Html.TextBoxFor(x => x.CustomerLastName, new {@class="z-index" })%>				
                    <div class="label">Address: </div><%=Html.TextBoxFor(x => x.CustomerAddress, new { @class = "z-index" })%>		
                    <div class="label">City: </div><%=Html.TextBoxFor(x => x.CustomerCity, new { @class = "z-index" })%>		
                    <div class="label">State: </div><%=Html.TextBoxFor(x => x.CustomerState, new { @class = "z-index" })%>		
                    <div class="label">ZIP: </div><%=Html.TextBoxFor(x => x.CustomerZipCode, new { @class = "z-index" })%>		
                    <br /><br />
                    <div class="label"><b style="color: white; font-size: 1.1em;">ACV</b>: </div><%=Html.TextBoxFor(x => x.ACV, new { @class = "z-index" })%>	
                          <% if ((bool) Session["IsEmployee"] == false)
                             { %>
                    <h3>Appraisal Options</h3>
                    <input type="submit" class="pad" id="AddToInventory" name="AddToInventory" value="Add to Inventory" />
                    <input type="submit" class="pad" id="AddToWholeSale" name="AddToWholeSale" value="Add to WholeSale" />
                    <input type="button" class="pad" name="print" onclick="window.print()" value="Print Appraisal"  />
                    <% } %>
                    
           
                </div>
               
            </div>
        </div>
        <% Html.EndForm(); %>
        
        
 <script src="<%=Url.Content("~/js/graph_plotter.js")%>" type="text/javascript"></script>
 <script src="<%=Url.Content("~/js/Chart/ViewAppraisal.js")%>" type="text/javascript"></script> 
 <script language="javascript" type="text/javascript">
     function newPopup(url) {
         var popupWindow = window.open(
                url, 'popUpWindow', 'height=900,width=1000,left=500,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
     }

     $("#AddToInventory").click(function() {

         $('#elementID').removeClass('hideLoader');
     });
     $("#SaveAppraisal").click(function() {

         $('#elementID').removeClass('hideLoader');
     });


     $("#AddToWholeSale").click(function() {

         $('#elementID').removeClass('hideLoader');
     });


     $("a.iframe").fancybox({
         'width': 1000,
         'height': 700,
         'hideOnOverlayClick': false,
         'centerOnScroll': true,
         'onCleanup': function() {
             // reload page when closing Chart screen
             if (openChart && $("#NeedToReloadPage").val() == 'true') {
                 $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                 openChart == false;
                 window.location.href = '/Appraisal/ViewProfileForAppraisal?AppraisalId=' + ListingId;
             }
         }
     });

     // fire click event to open Manheim Transaction fancybox popup
     $("a[id^='manheimRow']").live('mouseover focus', function (e) {
         $(this).fancybox({
             'width': 1000,
             'height': 700,
             'hideOnOverlayClick': false,
             'centerOnScroll': true,
             'onCleanup': function () {
             }
         });
     });

     // fire click event to open KBB Summary fancybox popup
     $("a[id^='kbbRow']").live('mouseover focus', function (e) {
         $(this).fancybox({
             'width': 1000,
             'height': 700,
             'hideOnOverlayClick': false,
             'centerOnScroll': true,
             'onCleanup': function () {
             }
         });
     });

     function StringBuilder() {
         var strings = [];

         this.append = function(string) {
             string = verify(string);
             if (string.length > 0) strings[strings.length] = string;
         };

         this.appendLine = function(string) {
             string = verify(string);
             if (this.isEmpty()) {
                 if (string.length > 0) strings[strings.length] = string;
                 else return;
             } else strings[strings.length] = string.length > 0 ? "\r\n" + string : "\r\n";
         };

         this.clear = function() { strings = []; };

         this.isEmpty = function() { return strings.length == 0; };

         this.toString = function() { return strings.join(""); };

         var verify = function(string) {
             if (!defined(string)) return "";
             if (getType(string) != getType(new String())) return String(string);
             return string;
         };

         var defined = function(el) {
             // Changed per Ryan O'Hara's comment:
             return el != null && typeof (el) != "undefined";
         };

         var getType = function(instance) {
             if (!defined(instance.constructor)) throw Error("Unexpected object type");
             var type = String(instance.constructor).match(/function\s+(\w+)/);

             return defined(type) ? type[1] : "undefined";
         };
     }
     ;

        
 </script>
<script type="text/javascript">
    function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
    // ############################### //
    // Chart Data Set and Draw Section //
    // ############################### //
    // check url for GET
    var window_url = window.location.search;

    // default graph to unexpanded draw size
    var expand = expanded(window_url);

    // set chart dimensions
    if (expand) {
        chart_dimensions = ["700px", "500px"];
        $('#graphButton').css('display', 'none');
    } else {
        chart_dimensions = ["300px", "143px"];
    }

    // grab graph div element and click element
    var gwrap = $('#graphWrap');
    gwrap.css('width', chart_dimensions[0]);
    gwrap.css('height', chart_dimensions[1]);

    // load default options & trims
    // we already filtered the results in code behind, so we don't need to do on the js code
    //var default_option = ($('#Options').val() == '' || $('#Options').val() == '0') ? [0] : $('#Options').val().split(',');
    //var default_trim = ($('#Trims').val() == '' || $('#Trims').val() == '0') ? [0] : $('#Trims').val().split(',');
    var default_option = [0];
    var default_trim = [0];

    var ListingId = $('#AppraisalGenerateId').val();

    // create ajax post url
    var requestUrl = '<%= Url.Action("GetMarketDataWithin100MilesRadius", "chart", new { appraisalId = "PLACEHOLDER" } ) %>';
    var waitingImage = '<%= Url.Content("~/images/ajax-loader1.gif") %>';

    // create filter
    var $filter = {};

    // get chart range
    var fRange = 100; // default

    var $dCar = {};
    var $selectedCar = {};
    // set y change check
    var newY = false;

    // create current filterred list of car
    var $currentFilterredList = [];

    var openChart = false;
    
</script>
  
 

</asp:Content>
