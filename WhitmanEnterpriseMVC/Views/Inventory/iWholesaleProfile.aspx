<%@ Page Title="" MasterPageFile="~/Views/Shared/Site.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.CarInfoFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <%=Model.ModelYear %>
        <%=Model.Make %>
        <%=Model.Model %></title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Css/iprofile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/extension.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.dragsort.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.symbols.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.functions.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/plot.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.easing-1.3.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.mousewheel-3.0.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <style type="text/css">
        /* Slider CSS */
        #dropdownWrap, #print
        {
            margin-left: -15px;
            margin-bottom: 0;
            width: 585px;
            background: black;
            float: left;
            clear: right;
        }
        #print
        {
            padding: 1em;
        }
        #dropdownWrap div
        {
            background: none repeat scroll 0 0 black;
        }
        #dropdownWrap table td
        {
            width: 100px;
            font-size: .8em;
        }
        #dropdownWrap table input[type="text"], #dropdownWrap table select
        {
            width: 100px;
        }
        #t1
        {
            float: left;
            clear: right;
        }
        #optionals
        {
            background: #333 !important;
            padding-left: 1em;
            padding: 1em;
            margin: 0;
        }
        #packages
        {
            background: #333 !important;
            padding-left: 1em;
            padding: 1em;
            margin: 0;
        }
        #descParagraph
        {
            height: 79px;
            overflow: hidden;
        }
        #descriptionWrapper
        {
            background: #333 !important;
            padding: 1em;
            padding: 1em;
            margin: 0;
        }
        #notes
        {
            background: #333 !important;
            padding: .3em;
            padding-left: 1em;
            margin: 1em;
            margin-top: 0;
            width: 500px;
            position: relative;
            top: -16px;
        }
        #warranty
        {
            background: #333 !important;
            padding: .3em;
            padding-left: 1em;
            margin: 1em;
            margin-top: 0;
            width: 500px;
            position: relative;
            top: -16px;
        }
        #internetprice
        {
            background: #333 !important;
            padding: .3em;
            padding-left: 1em;
            margin: 1em;
            margin-top: 0;
            width: 500px;
            position: relative;
            top: -16px;
        }
        #optionals
        {
            margin-top: 0;
            margin-bottom: 0;
            overflow: hidden;
            padding-left: 1em;
            padding-top: .7em;
        }
        #c2
        {
            padding-top: 0;
        }
        .hider
        {
            display: none;
        }
        #optionals ul
        {
            padding: 1em;
        }
        #Dc2
        {
            width: 585px;
        }
        #noteList
        {
            padding: 1em;
        }
        #noteBox
        {
            width: 300px;
            margin-bottom: 1em;
        }
        input[name="edit"]
        {
            position: relative;
            margin-top: 0;
        }
        input[name="options"].pad_tab, input[name="notes"].pad_tab, input[name="packages"].pad_tab, input[name="description"].pad_tab, input[name="warranty"].pad_tab, input[name="internetprice"].pad_tab
        {
            background: #333;
            margin-bottom: 0;
            margin-left: 0;
            margin-top: .3em;
        }
        #print
        {
            margin-bottom: 0;
        }
        p.top, p.bot
        {
            margin: 0;
            padding: 0;
        }
        p.bot
        {
            padding: .3em;
        }
        #c3 li
        {
            float: left;
            clear: right;
        }
        #placeholder
        {
            background-color: #eee !important;
        }
        #mInfoList h1, #mInfoList h2
        {
            margin-bottom: 0;
            margin-top: 0;
        }
        #mInfoList h2
        {
            font-size: 1.4em;
        }
        span.gidInfo
        {
            font-size: .8em;
            color: white;
        }
        #carInfo
        {
            background: #980000;
            padding: .6em;
            margin-top: 1.1em;
        }
        #carInfo h3
        {
            margin-top: 0;
        }
        #figures
        {
            margin-bottom: -35px;
        }
        #mInfo input[name="acvBTN"], #mInfo input[name="costBTN"], #mInfo input[name="saleBTN"]
        {
            margin-top: 2px;
        }
        #listImages
        {
            overflow: auto;
            width: 100%;
            max-height: 800px;
        }
        #references
        {
            margin-top: 1em !important;
        }
        .sortMiles
        {
            color: white;
            font-weight: bold;
        }
        .sortMiles:hover
        {
            cursor: pointer;
            color: red;
        }
        .checked
        {
            border: 2px red solid;
            width: 36px;
            height: 36px;
        }
        .centerImage input[type="checkbox"]
        {
            display: none;
        }
        #filter
        {
            width: 20px;
            height: 20px;
            background: url('../images/filter2.gif') top left no-repeat;
            float: left;
            clear: right;
            margin-right: .3em;
        }
        #filter span
        {
            display: none;
        }
        #carfax .number, #carfax .text
        {
            font-size: .95em !important;
            display: inline-block;
            overflow: hidden;
            padding: .1em .4em .1em .4em;
            height: 100%;
            border: 4px #111 solid;
        }
        #carfax .oneowner, #carfax .text
        {
            font-size: .95em !important;
            display: inline-block;
            overflow: hidden;
            padding: .1em .4em .1em .4em;
            height: 100%;
            border: 4px #111 solid;
        }
        #carfax .text
        {
            margin-left: 0;
            background: #111;
        }
        #carfax .number
        {
            margin-right: 0;
            background: #c80000;
            overflow: hidden;
            color: white;
            font-weight: bold;
        }
        #carfax .oneowner
        {
            margin-right: 0;
            background: green;
            overflow: hidden;
            color: white;
            font-weight: bold;
        }
        #carfax .carfax-wrapper
        {
            overflow: hidden;
            margin-top: 10px;
            display: inline;
            width: 225px;
        }
        #carfax-header, #report-wrapper, #summary-wrapper
        {
            display: block;
        }
        #carfax-header
        {
            position: relative;
            top: -8px;
        }
        #summary-wrapper
        {
            position: absolute;
            right: 0;
            top: 0;
        }
        #carfax-header img
        {
            margin-right: 5px;
        }
        #report-wrapper img
        {
            float: right;
            margin-right: 10px;
            margin-top: 3px;
        }
        #carfax-header h3
        {
            margin-top: 3px;
        }
        #report-wrapper ul, #report-wrapper li
        {
            margin: 0;
            padding: 0;
        }
        #report-wrapper ul
        {
            margin-top: 5px;
        }
        #report-wrapper li
        {
            width: 100%;
            background: #111;
            margin-top: 4px;
            padding: .2em .5em .2em .5em;
        }
        #carfax
        {
            position: relative;
            margin-bottom: 8px !important;
        }
        #kbb div, #kbb h3, #kbb span, #bb div, #bb h3, #bb span, #manheim div, #manheim h3, #manheim span
        {
            display: inline;
        }
        #kbb, #bb, #manheim
        {
            position: relative;
            overflow: hidden;
            width: 541px !important;
            margin-top: 8px !important;
        }
        #kbb #kbb-row, #bb #bb-row, #manheim #manheim-row
        {
            background: #860000;
            border: 4px solid #860000;
            white-space: nowrap;
            display: inline-block;
            width: 560px !important;
            min-height: 25px;
        }
        #kbb .range-item, #bb .range-item, #manheim .range-item
        {
            display: inline-block;
            width: 122px;
            padding: 3px 4px 3px 4px;
            background: #555;
            margin-right: 0px;
        }
        .range-item.label
        {
            width: 120px !important;
        }
        #kbb h3, #bb h3, #manheim h3
        {
            background: #111;
            padding: 4px 5px 4px 5px !important;
            overflow: hidden;
            border-bottom: 3px #111 solid;
            cursor: pointer;
        }
        .mr-on
        {
            background: #860000 !important;
            border-bottom: 3px #860000 solid;
        }
        .high, .mid, .high, .low-wrap, .mid-wrap, .high-wrap
        {
            padding-left: 10px;
            font-weight: bold;
        }
        .high
        {
            background: #d02c00 !important;
        }
        .mid
        {
            background: #008000 !important;
        }
        .low
        {
            background: #0062A0 !important;
        }
        .hideLoader
        {
            display: none;
        }
        .hidden
        {
            display: none;
        }
        .selected
        {
            font-weight: bold;
            color: red;
        }
        #graphWrap
        {
            width: 300px;
            height: 143px;
            display: inline-block;
            float: left;
        }
        #carInfo
        {
            display: inline-block;
        }
        #placeholder
        {
            background: url('../images/loading.jpg') center no-repeat !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 
                                       <%=  Html.HiddenFor(x=>x.ListingId)%>  
                                         
                                         <%= Html.DynamicHtmlControlForIprofile("txtHiddenPhotos", "HiddenPhotos")%>
                                           <input class="z-index" type="hidden" id="hiddenPhotoOrder" name="hiddenPhotoOrder" value="" />
                          <div id="topNavBTN" class="clear" style="width: 600px;overflow: hidden;">
                                <%=Html.ReportButtonGroup() %>
                <script type="text/javascript">
                    $('input[name="edit"]').click(function () {
                        $("#dropdownWrap").slideToggle("slow");





                    });


                    $('input[name="edit"]').click(function () {
                        $("#detailWrap").slideToggle("slow");

                    });

                            </script></div>
                <div id="detailWrap">                
                      <div id="images" class="column" >
                	<%=  Html.DynamicHtmlLabel("txtCarImage", "CarImage")%>
                
                    <div class="clear"></div>
               	</div>
               	<div id="profileInfo">
                
                    <h3><%=  Html.DynamicHtmlLabel("txtTitle", "Title")%></h3>
                    <ul class="column">
                    	<li><%=  Html.DynamicHtmlLabel("txtLabelVin", "Vin")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelStock", "Stock")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelYear", "Year")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelMake", "Make")%></li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelModel", "Model")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelTrim", "Trim")%> </li>
                        <li><%=  Html.DynamicHtmlLabel("txtLabelExteriorColor", "ExteriorColor")%> </li>
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
              
                        <div id="description" class="clear"><h4>Description</h4>
                            <p class="top"><%=  Html.DynamicHtmlLabel("txtLabelDescription", "Description")%></p>
                            <p class="bot"></p>
                            <div id="mInfo">
                             
                                <ul id="mInfoList">
                                    <li>
                                        <h2 class="subtext acv">ACV</h2>
                                        <h1 title="Click to change this info." id="acvTitle" class="green acv"></h1>
                                          <% Html.BeginForm("UpdateACV", "Inventory", FormMethod.Post, new { id = "SaveACVForm" });%>
                                        <input  class="green" style="
                                            	width: 110px !important;
                                                font-size: 2.0em; 
                                                overflow:hidden; 
                                                display:block;
                                                color: #66FE00 !important;
                                                background: #222 !important;" type="text" name="acv" id="txtACV"  value=<%=Model.ACV %> >
                                        <%= Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId")%>
                                        <input class="acv" style="width: 110px !important;" type="submit" name="acvBTN" value=" Save " />
                                         <% Html.EndForm(); %>
                                    </li>
                                    <li>
                                        <h2 class="subtext">Dealer Cost</h2>
                                        <h1 title="Click to change this info." id="costTitle" class="green cost"></h1>
                                          <% Html.BeginForm("UpdateDealerCost", "Inventory", FormMethod.Post, new { id = "SaveDealerCostForm" });%>
                                        <input type="text" class="green" style="
                                            	width: 110px !important;
                                                font-size: 2.0em; 
                                                overflow:hidden; 
                                                display:block;
                                                color: #66FE00 !important;
                                                background: #222 !important;" name="dealerCost" id="txtDealerCost" value=<%=Model.DealerCost %>>
                                        <%= Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId")%>
                                        <input class="cost" style="width: 110px !important;" type="submit" name="costBTN" value=" Save " />
                                         <% Html.EndForm(); %>
                                    </li>
                                    <li>
                                        <h2 class="subtext">Sale Price</h2>
                                        
                                        <h1 title="Click to change this info." id="saleTitle" name="saleTitle" class="green sale"></h1>
                                         <% Html.BeginForm("UpdateSalePrice", "Inventory", FormMethod.Post, new { id = "SaveSalePriceForm" });%>
                                        <input type="text" class="green" style="
                                            	width: 110px !important;
                                                font-size: 2.0em; 
                                                overflow:hidden; 
                                                display:block;
                                                color: #66FE00 !important;
                                                background: #222 !important;" name="salePrice" id="txtSalePrice" value=<%=Model.SalePrice %>>
                                        <%= Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId")%>
                                        <input class="sale" style="width: 110px !important;" type="submit" name="saleBTN" value=" Save " />
                                         <% Html.EndForm(); %>
                                    </li>
                                    <div class="clear"></div>
                                </ul>
                               
                            </div>
                            
                            <p class="top"></p>
                            <p class="bot"></p>
                    	</div>
                	</div>
                
                
                    <div id="figures">
                    	
                        <div id="graph" class="column" style="font-size: .8em;">
                <div id="rangeNav">
                    <input type="button" style="margin-left: 0;" id="graphButton" name="toggleGraph"
                        value="Expand" />
                    <span id="txtDistance">Within (100 miles) from your location</span>
                </div>
                <div id="graphWrap">
                    <div id="placeholder" style="height: 100%; width: 100%;">
                    </div>
                </div>
                <div id="sorting">
                    <br />
                </div>
                <%=Html.ExpandChartButton(Model.ListingId)%>
            </div>
                        <div id="age" class="column" style="margin-top: 20px !important">
                <h4 style="margin-top: 0; margin-bottom: 0">
                    Ranks <span id="totalCars" class="green" style="font-size: .8em">
                        <label id="txtRanking">
                            0
                        </label>
                        out of
                        <label id="txtCarsOnMarket">
                            0</label>
                    </span>on Market
                </h4>
                <p class="top">
                </p>
                <p class="bot">
                </p>
                <h3 style="margin-top: 0; margin-bottom: 0">
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
                <h4 style="width: 220px; margin-top: 0; margin-bottom: 0">
                    <span class="green">
                        <label id="txtAvgDays">
                            0</label></span> Avg Days in Inventory</h4>
            </div>
                    </div>
                    
                    
                   
                
                   <div id="references">
                    <p class="top"></p>
                    <p class="bot"></p>
                      <div id="carfax" style="overflow: hidden;">
                <div id="carfax-header" style="display: block; width: 100%;">
                    <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.Vin%>')">
                        <img style="display: inline-flexbox; float: left;" src='<%=Url.Content("~/Images/carfax-large.jpg")%>'
                            alt="carfax logo" /></a>
                    <h3 style="">
                        Summary</h3>
                </div>
                <%if (Model.CarFax.Success)
                  {%>
                <div id="summary-wrapper">
                    <div id="owners" class="carfax-wrapper">
                        <%if (Model.CarFax.NumberofOwners.Equals("0"))
                          { %>
                        <div class="number">
                            -</div>
                        <%}
                          else if (Model.CarFax.NumberofOwners.Equals("1"))
                          { %>
                        <div class="oneowner">
                            <%=Model.CarFax.NumberofOwners%></div>
                        <%}
                          else
                          {%>
                        <div class="number">
                            <%=Model.CarFax.NumberofOwners%></div>
                        <%} %>
                        <div class="text">
                            Owner(s)</div>
                    </div>
                    <div id="reports" class="carfax-wrapper">
                        <div class="number">
                            <%=Model.CarFax.ServiceRecords %></div>
                        <div class="text">
                            Service Reports</div>
                    </div>
                </div>
                <%} %>
                <div id="report-wrapper" style="width: 100%;">
                    <div id="history-report" style="clear: both; float: left; width: 100%;">
                        <ul>
                            <%foreach (WhitmanEnterpriseMVC.Models.CarFaxWindowSticker tmp in Model.CarFax.ReportList)
                              {  %>
                            <%if (tmp.Text.Contains("Prior Rental") || tmp.Text.Contains("Accident(s) / Damage Reported to CARFAX"))
                              { %>
                            <li style="background-color: red">
                                <% }
                              else
                              {
                                %>
                                <li>
                                    <%
                                } %>
                                    <%=tmp.Text %>
                                    <img class="c-fax-img" src="<%=tmp.Image %>" height="10px" width="10px" />
                                </li>
                                <%} %>
                        </ul>
                        <a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.Vin%>')">
                            View Full Report</a>
                    </div>
                </div>
                <%--<textarea  class="z-index" id="Textarea3" name="description" cols="55" rows="15" ><%=ViewData["CarFaxResponse"]%></textarea>--%>
            </div>
                                            <div id="manheim">
                <h3 class="mr-on">
                    <a target="_blank" style="padding-right: 15px;" target="_blank" title="MMR Auction Pricing"
                        href="<%=Url.Content("~/Market/OpenManaheimLoginWindow?Vin=")%><%=Model.Vin%>">
                        <img height="20" style="position: relative; top: 2px;" src="http://www.microsoft.com/southafrica/microsoftservices/images/logo/logo_manheim.gif"
                            alt="MMR Auction Pricing" />
                        Manheim </a>
                </h3>
                <div class="partialContents" data-url="<%= Url.Action("ManheimData", "Inventory", new { Model.ListingId,Model.InventoryStatus }) %>">
                    <div id="manheim-row" class="mr-row">
                        Loading...
                    </div>
                </div>
            </div>
                                          <div id="mkWrap">
                <div id="kbb">
                    <h3 class="mr-on">
                        <img height="20" style="position: relative; top: 2px;" src="http://cdn.autofusion.com/apps/mm4/4.1/images/extras/kbb-logo-alpha.png" />
                        Kelly Blue Book
                    </h3>
                    <h3 style="padding: 4px;">
                        <img height="25" style="position: relative; top: 4px;" src='<%=Url.Content("~/images/bblogo.png")%>' />
                        Black Book
                    </h3>
                    <a style="display: inline-block; cursor: pointer; height: 17px; font-size: .7em"
                        title="DMV Desk" onclick="window.open('https://secure.dmvdesk.com/dmvdesk/', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')">
                        <span style="position: relative; top: -5px;">DMV Desk</span> </a><a style="display: inline-block;
                            cursor: pointer; height: 17px; font-size: .7em" title="KSR" onclick="window.open('https://www.dmvlink.com/online/default.asp', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')">
                            | <span style="position: relative; top: -5px;">KSR</span> </a>
                    <tr>
                        <div id="divPartialKbb" class="partialContents" data-url="<%= Url.Action("KarPowerData", "Inventory", new { Model.ListingId,Model.InventoryStatus }) %>">
                            <div id="kbb-row" class="mr-row">
                                Loading...
                            </div>
                        </div>
                </div>
               
            </div>

                	</div>
            	
            	</div>
            </div>
            <div id="c3" class="column">
            <%=  Html.ImageButton("Upload")%>
                        <input id="delete" class="pad" type="submit" name="submit" value="Delete"  />
                        <input id="save" class="pad" type="submit" name="submit" value="Save"  />
             
                
                     <br />
                  
                 <div id="photos"  >
                <ul id="listImages">
                    <%=Html.DynamicHtmlLabel("txtUploadedPhotos", "UploadCarImage")%>
                  
                 
                  
                 </ul>
                 
              
                   </div>
                <p class="top clear"></p>
                <p class="bot"></p>
            
                <div class="clear"></div>
                </div>
           
                
            </div>
           
    <input name="list1SortOrder" type="hidden" />
    
 <script src="<%=Url.Content("~/js/graph_plotter.js")%>" type="text/javascript"></script>   
    <script src="<%=Url.Content("~/js/Chart/ViewProfile.js")%>" type="text/javascript"></script>
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

        var ListingId = $('#ListingId').val();

        // create ajax post url
        var requestUrl = '<%= Url.Action("GetMarketDataByListingWholesaleWithin100MilesRadius", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
        var requestNationwideUrl = '<%= Url.Action("GetMarketDataByListingNationwideFromAutoTraderWithHttpPost", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
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
<script type="text/javascript" >

    $(document).ready(function () {

        $("#txtACV").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        $("#txtDealerCost").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        $("#txtSalePrice").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        $("#txtDealerDiscount").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        $("#txtRetailPrice").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        //Setup the AJAX call

        $(".partialContents").each(function (index, item) {
            var url = $(item).data("url");
            if (url && url.length > 0) {
                $(item).load(url);
            }
        });

        $("#aLoadFullKBBTrims").live('click', function () {
            $('.mr-row').each(function (index) {
                //$(this).removeClass('disable');
                //$(this).addClass('mr-row');
                $(this).show();
            });
        });

        $("#SaveDealerCostForm").submit(function (event) {

            event.preventDefault();

            SaveDealerCost(this);


        });



        $("#SaveACVForm").submit(function (event) {

            event.preventDefault();

            SaveACV(this);


        });
        $("#SaveSalePriceForm").submit(function (event) {

            event.preventDefault();

            SaveSalePrice(this);


        });


    });

    function SaveDealerCost(form) {



        $.ajax({

            url: form.action,

            type: form.method,

            dataType: "json",

            data: $(form).serialize(),

            success: DealerCostSave

        });


    }


    function SaveACV(form) {



        $.ajax({

            url: form.action,

            type: form.method,

            dataType: "json",

            data: $(form).serialize(),

            success: ACVSave

        });


    }


    function SaveSalePrice(form) {



        $.ajax({

            url: form.action,

            type: form.method,

            dataType: "json",

            data: $(form).serialize(),

            success: SalePriceSave

        });


    }


    function SalePriceSave(result) {

        // Update the page with the result
        //        var item = "<label for=\"txtLabelSalePrice\">" + result + "</label>";
        //        $("#saleTitle").html(item);
        //$("#txtSalePrice").val("");

    }

    function DealerCostSave(result) {

        // Update the page with the result
        //        var item = "<label for=\"txtLabelCostPrice\">" + result + "</label>";
        //        $("#costTitle").html(item);
        //$("#txtDealerCost").val("");

    }


    function ACVSave(result) {

        // Update the page with the result
        //        var item = "<label for=\"txtLabelAcv\">" + result + "</label>";
        //        $("#acvTitle").html(item);
        //        $("#txtACV").val("");

    }

    $('#txtACV').blur(function () {
        var ListingId = $("#ListingId").val();
        var acv = $("#txtACV").val();

        $.post('<%= Url.Content("~/Inventory/UpdateACV") %>', { ListingId: ListingId, acv: acv }, function (data) {
            console.log(data);
        });

    });

    $('#txtDealerCost').blur(function () {
        var ListingId = $("#ListingId").val();
        var dealerCost = $("#txtDealerCost").val();

        $.post('<%= Url.Content("~/Inventory/UpdateDealerCost") %>', { ListingId: ListingId, dealerCost: dealerCost }, function (data) {

        });


    });


    $('#txtSalePrice').blur(function () {
        var ListingId = $("#ListingId").val();
        var salePrice = $("#txtSalePrice").val();

        $.post('<%= Url.Content("~/Inventory/UpdateSalePrice") %>', { ListingId: ListingId, SalePrice: salePrice }, function (data) {
        });
    });




    function markVehicleSold(ListingId) {
        builder.Append("<a class=\"iframe\" href=\"" + urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\"  type=\"button\" name=\"ebayCraigslist\" value=\"Ebay\" /></a>" + Environment.NewLine);
        var answer = confirm("Are you sure you want to mark this vehicle sold?");
        //console.log(ListingId);
        if (answer) {
            $.post('<%= Url.Content("~/Inventory/MarkSold") %>', { ListingId: ListingId }, function (data) {
                if (data == "SessionTimeOut") {
                    parent.$.fancybox.close();
                    alert("Your session is timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                } else {

                    $("#Marksold").val("Removed");


                    $("#Marksold").attr("disabled", "disabled");
                }


            });
        }
    }


    function get_check_value(param) {
        var c_value = "";
        var tmp = "";
        if (param == "options") {
            for (var i = 0; i < document.getElementsByName("selectedOptions").length; i++) {
                if (document.getElementsByName("selectedOptions")[i].checked) {
                    tmp = document.getElementsByName("selectedOptions")[i].value;
                    c_value = c_value + tmp.substring(0, tmp.indexOf("$") - 1) + ",";
                }
            }
        }
        else {
            for (var i = 0; i < document.getElementsByName("selectedPackages").length; i++) {
                if (document.getElementsByName("selectedPackages")[i].checked) {
                    tmp = document.getElementsByName("selectedPackages")[i].value;
                    c_value = c_value + tmp.substring(0, tmp.indexOf("$") - 1) + ",";
                }
            }
        }
        c_value = c_value.substring(c_value, c_value.length - 1);

        return c_value;
    }

    //    function openWindowSticker(ListingId) {

    //        var selectedoptions = get_check_value("options");

    //        var selectedPackages = get_check_value("packages");

    //        var MSRP = $("#txtMSRP").val();

    //        //var actionUrl = '<%= Url.Action("ViewSticker", "Report", new { ListingId = "PLACEHOLDER" , Options ="PLACEOPTIONS",Packages="PLACEPACKAGES",MSRP="PLACEMSRP"   } ) %>';

    //        var actionUrl = '<%= Url.Action("ViewSticker", "Report", new { ListingId = "PLACEHOLDER"  } ) %>';

    //        actionUrl = actionUrl.replace('PLACEHOLDER', ListingId);

    ////        actionUrl = actionUrl.replace('PLACEOPTIONS', selectedoptions);

    ////        actionUrl = actionUrl.replace('PLACEPACKAGES', selectedPackages);

    ////        actionUrl = actionUrl.replace('PLACEMSRP', MSRP);

    //        window.open(actionUrl);

    //    }

    function openBuyerGuide(ListingId) {


        var actionUrl = '<%= Url.Action("ViewBuyerGuide", "Report", new { ListingId = "PLACEHOLDER"  } ) %>';

        actionUrl = actionUrl.replace('PLACEHOLDER', ListingId);

        //        actionUrl = actionUrl.replace('PLACEOPTIONS', selectedoptions);

        //        actionUrl = actionUrl.replace('PLACEPACKAGES', selectedPackages);

        //        actionUrl = actionUrl.replace('PLACEMSRP', MSRP);

        window.open(actionUrl);

    }

    function formatDollar(num) {
        var p = num.toFixed(2).split(".");
        return "$" + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
            return num + (i && !(i % 3) ? "," : "") + acc;
        }, "") + "." + p[1];
    }



    var pressed = 0;
    var flag = false;

    $('#txtDealerDiscount').blur(function () {
        var retailPrice = $("#txtRetailPrice").val();
        var discountPrice = $("#txtDealerDiscount").val();
        var number1 = Number(retailPrice.replace(/[^0-9\.]+/g, ""));
        var number2 = Number(discountPrice.replace(/[^0-9\.]+/g, ""));
        var total = Number(number1) - Number(number2);
        $("#txtWSSalePrice").val(formatDollar(total));
        flag = true;

    });

    $("a.image").mousedown(function () {
        pressed++;
        var t = setTimeout("pressed=0", 500);
        if (pressed == 2) {
            pressed = 0;
            $(this).fancybox();
            return false;
        }
    });


    //$("a.iframe").live('click', function () {
    $("a.iframe").fancybox({
        'width': 1000,
        'height': 700,
        'hideOnOverlayClick': false,
        'centerOnScroll': true,
        'onCleanup': function () {
            // reload page when closing Chart screen
            if (openChart && $("#NeedToReloadPage").val() == 'true') {
                $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });
                openChart == false;
                window.location.href = '/Inventory/ViewIProfile?ListingID=' + ListingId;
            }
        }
    });
    //});

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
                // reload KBB section
                if ($("#NeedToReloadPage").val() == 'true') {
                    $("#divPartialKbb").html('<div id=\"kbb-row\" class=\"mr-row\">Loading...</div>');
                    $.ajax({
                        type: "GET",
                        url: "/Inventory/KarPowerData?listingId=" + '<%= Model.ListingId %>' + '&InventoryStatus=' + '<%= Model.InventoryStatus %>',
                        data: {},
                        dataType: 'html',
                        success: function (data) {
                            $("#divPartialKbb").html(data);
                            $("#NeedToReloadPage").val('false');
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                             $.unblockUI();
                            alert(xhr.status + ' ' + thrownError);
                        }
                    });
                }
            }
        });
    });

    $("a.smalliframe").fancybox();

    $('input[name="print"]').click(function () { $("#print").slideToggle("slow"); $('input[name="ebayCraigslist"]').slideToggle("slow"); $('input[name="edit"]').slideToggle("slow"); $('input[name="sold"]').slideToggle("slow"); });

    $('input[name="edit"]').click(function () {

        var dropdownstyle = $("#dropdownWrap").attr("style");
        if (dropdownstyle.substring(0, dropdownstyle.indexOf(":")) == "overflow") {
            $('input[name="edit"]').val("Save");
        }

        else {
            $('input[name="edit"]').val("Edit");
        }

        $("#iProfileForm").change(function () {
            flag = true;
        });

        $("#txtDescription").change(function () {
            flag = true;
        });
        $("#packages").change(function () {
            flag = true;
        });
        $("#optionals").change(function () {
            flag = true;
        });
        $("#warranty").change(function () {

            flag = true;
        });

        flag = true;

        if (flag) {


            var trim = $("#txtTrim").val();

            $("#txtLabelTrim").text(trim);

            var newTitle = $("#txtYear").val() + " " + $("#txtMake").val() + " " + $("#txtModel").val();

            var finalExteriorColor = "";

            var finalInteriorColor = "";

            if (trim != "NA")
                newTitle += " " + trim;



            $("#txtTitle").text(newTitle);
            $("#txtLabelModel").text($("#txtModel").val());
            $("#txtLabelTranmission").text($("#txtTranmission").val());
            $("#txtLabelOdometer").text($("#txtOdometer").val());
            $("#txtLabelCylinders").text($("#txtCylinders").val());
            $("#txtLabelLitters").text($("#txtLitters").val());
            $("#txtLabelDoors").text($("#txtDoors").val());
            $("#txtLabelFuelType").text($("#txtFuelType").val());
            $("#txtLabelMSRP").text($("#txtMSRP").val());
            $("#txtLabelStock").text($("#txtStock").val());
            $("#txtLabelVin").text($("#txtEditVin").val());
            $("#txtLabelYear").text($("#txtYear").val());
            $("#txtLabelMake").text($("#txtMake").val());



            if ($("#txtDescription").val().length > 150)
                $("#txtLabelDescription").text($("#txtDescription").val().substring(0, 150) + "...");
            else
                $("#txtLabelDescription").text($("#txtDescription").val());



            if ($("#txtDriveType").val() == "Front Wheel Drive")
                $("#txtLabelDriveType").text("FWD");
            else if ($("#txtDriveType").val() == "Rear Wheel Drive")
                $("#txtLabelDriveType").text("RWD");
            else if ($("#txtDriveType").val() == "Four Wheel Drive")
                $("#txtLabelDriveType").text("4WD");
            else if ($("#txtDriveType").val() == "All Wheel Drive")
                $("#txtLabelDriveType").text("AWD");
            else if ($("#txtDriveType").val() == "Dual Rear Wheels")
                $("#txtLabelDriveType").text("Dual-RWD");



            if ($("#txtExteriorColor").val() != "Other Colors") {

                $("#txtLabelExteriorColor").text($("#txtExteriorColor").val().substring(0, Math.min(15, $("#txtExteriorColor").val().length)));

                finalExteriorColor = $("#txtExteriorColor").val();
            }

            else {
                if ($("#txtCusExteriorColor").val() != "") {
                    $("#txtLabelExteriorColor").text($("#txtCusExteriorColor").val().substring(0, Math.min($("#txtCusExteriorColor").val().length, 15)));
                    finalExteriorColor = $("#txtCusExteriorColor").val();
                }
            }

            if ($("#txtInteriorColor").val() != "Other Colors") {
                $("#txtLabelInteriorColor").text($("#txtInteriorColor").val().substring(0, Math.min(15, $("#txtInteriorColor").val().length)));
                finalInteriorColor = $("#txtInteriorColor").val();
            }
            else {
                if ($("#txtCusInteriorColor").val() != "") {
                    $("#txtLabelInteriorColor").text($("#txtCusInteriorColor").val().substring(0, Math.min($("#txtCusInteriorColor").val().length, 15)));
                    finalInteriorColor = $("#txtCusInteriorColor").val();
                }
            }

            var packages = getSelectedPackages();

            var options = getSelectedOptions();

            var finalOptions = "";

            if (packages != "" && options == "") {
                finalOptions = packages;
            } else if (packages == "" && options != "") {
                finalOptions = options;
            } else if (packages != "" && options != "") {
                finalOptions = packages + "," + options;
            }

            var mileage = $("#txtOdometer").val().replace(",", "");

            var CertifiedCheck = false;
            var PriorRentalCheck = false;

            if ($('#txtCertified').is(':checked')) {
                CertifiedCheck = true;
            }

            if ($('#txtPriorRental').is(':checked')) {
                PriorRentalCheck = true;
            }

            $.post('<%= Url.Content("~/Inventory/UpdateIProfile") %>', { ListingId: $("#ListingID").val(), Vin: $("#txtEditVin").val(), StockNumber: $("#txtStock").val(), ModelYear: $("#txtYear").val(), Make: $("#txtMake").val(), Model: $("#txtModel").val(), ExteriorColor: finalExteriorColor, InteriorColor: finalInteriorColor, Trim: trim, Mileage: mileage, Tranmission: $("#txtTranmission").val(), Cylinders: $("#txtCylinders").val(), Liters: $("#txtLitters").val(), Doors: $("#txtDoors").val(), Style: $("#txtBodyType").val(), Fuel: $("#txtFuelType").val(), Drive: $("#txtDriveType").val(), Description: $("#txtDescription").val(), Options: finalOptions, MSRP: $("#txtMSRP").val(), Certified: CertifiedCheck, PriorRental: PriorRentalCheck, RetailPrice: $("#txtRetailPrice").val(), DiscountPrice: $("#txtDealerDiscount").val(), ManufacturerRebate: $("#txtManufacturerRebate").val() }, function (data) {

                flag = false;
                console.log(PriorRentalCheck);
            });


        }



    });

    function getSelectedPackages() {
        var x = document.forms["iProfileForm"];

        var result = "";

        var totalPackages = document.getElementsByName("selectedPackages");

        for (var i = 0; i < totalPackages.length; i++) {
            if (totalPackages[i].checked) {

                result += totalPackages[i].value.substring(0, totalPackages[i].value.indexOf("$") - 1) + ",";
            }
        }

        result = result.substring(0, result.length - 1);
        return result;

    }

    function getSelectedOptions() {
        var x = document.forms["iProfileForm"];

        var result = "";

        var totalOptions = document.getElementsByName("selectedOptions");

        for (var i = 0; i < totalOptions.length; i++) {
            if (totalOptions[i].checked)
                result += totalOptions[i].value.substring(0, totalOptions[i].value.indexOf("$") - 1) + ",";
        }

        result = result.substring(0, result.length - 1);
        return result;

    }
    $('input[name="options"]').click(function () { $("#optionals").slideToggle("slow"); });
    $('input[name="options"]').click(function () { $("#descriptionWrapper").hide("slow"); });
    $('input[name="options"]').click(function () { $("#notes").hide("slow"); });
    $('input[name="options"]').click(function () { $("#packages").hide("slow"); });
    $('input[name="options"]').click(function () { $("#warranty").hide("slow"); });
    $('input[name="options"]').click(function () { $("#internetprice").hide("slow"); });

    $('input[name="description"]').click(function () { $("#descriptionWrapper").slideToggle("slow"); });
    $('input[name="description"]').click(function () { $("#optionals").hide("slow"); });
    $('input[name="description"]').click(function () { $("#packages").hide("slow"); });
    $('input[name="description"]').click(function () { $("#notes").hide("slow"); });
    $('input[name="description"]').click(function () { $("#warranty").hide("slow"); });
    $('input[name="description"]').click(function () { $("#internetprice").hide("slow"); });

    $('input[name="packages"]').click(function () { $("#packages").slideToggle("slow"); });
    $('input[name="packages"]').click(function () { $("#descriptionWrapper").hide("slow"); });
    $('input[name="packages"]').click(function () { $("#optionals").hide("slow"); });
    $('input[name="packages"]').click(function () { $("#notes").hide("slow"); });
    $('input[name="packages"]').click(function () { $("#warranty").hide("slow"); });
    $('input[name="packages"]').click(function () { $("#internetprice").hide("slow"); });

    $('input[name="notes"]').click(function () { $("#notes").slideToggle("slow"); });
    $('input[name="notes"]').click(function () { $("#descriptionWrapper").hide("slow"); });
    $('input[name="notes"]').click(function () { $("#optionals").hide("slow"); });
    $('input[name="notes"]').click(function () { $("#packages").hide("slow"); });
    $('input[name="notes"]').click(function () { $("#warranty").hide("slow"); });
    $('input[name="notes"]').click(function () { $("#internetprice").hide("slow"); });

    $('input[name="warranty"]').click(function () { $("#warranty").slideToggle("slow"); });
    $('input[name="warranty"]').click(function () { $("#descriptionWrapper").hide("slow"); });
    $('input[name="warranty"]').click(function () { $("#notes").hide("slow"); });
    $('input[name="warranty"]').click(function () { $("#packages").hide("slow"); });
    $('input[name="warranty"]').click(function () { $("#internetprice").hide("slow"); });
    $('input[name="warranty"]').click(function () { $("#optionals").hide("slow"); });

    $('input[name="internetprice"]').click(function () { $("#internetprice").slideToggle("slow"); });
    $('input[name="internetprice"]').click(function () { $("#descriptionWrapper").hide("slow"); });
    $('input[name="internetprice"]').click(function () { $("#optionals").hide("slow"); });
    $('input[name="internetprice"]').click(function () { $("#packages").hide("slow"); });
    $('input[name="internetprice"]').click(function () { $("#notes").hide("slow"); });
    $('input[name="internetprice"]').click(function () { $("#warranty").hide("slow"); });




    var currentheight = 0;

    var count = document.getElementById('listImages').getElementsByTagName('li').length;
    window.onload = function () {
        currentheight = 55 * (count / 3);
        document.getElementById('listImages').style.height = currentheight + 'px';
        document.getElementById('age').style.marginTop = '.8em';
    }
    var shifted = 0;
    $('input[name="toggleGraph"]').click(function () {

        //	         var minheight = 287;
        //	         if (currentheight > minheight) {
        //	             while (currentheight >= minheight) {
        //	                 currentheight = currentheight - 1;

        //	                 document.getElementById('listImages').style.height = currentheight + 'px';
        //	             }
        //	             document.getElementById('listImages').style.overflow = 'auto';
        //	         } else {
        //	             currentheight = 55 * (count / 3);
        //	             document.getElementById('listImages').style.height = currentheight + 'px';
        //	             document.getElementById('listImages').style.overflow = 'visible';
        //	         }

        //	         $("#references").slideToggle("slow");
        //	         $("#carInfo").slideToggle("fast");

        //	         if (shifted == 0) {
        //	             document.getElementById('age').style.marginTop = '-1em';
        //	             shifted = 1;
        //	         } else {
        //	             document.getElementById('age').style.marginTop = '.8em';
        //	             shifted = false;
        //	         }
    });

    $('#placeholder').dblclick(function () {
        //	         var minheight = 287;
        //	         if (currentheight > minheight) {
        //	             while (currentheight >= minheight) {
        //	                 currentheight = currentheight - 1;

        //	                 document.getElementById('listImages').style.height = currentheight + 'px';
        //	             }
        //	             document.getElementById('listImages').style.overflow = 'auto';
        //	         } else {
        //	             currentheight = 55 * (count / 3);
        //	             document.getElementById('listImages').style.height = currentheight + 'px';
        //	             document.getElementById('listImages').style.overflow = 'visible';
        //	         }

        //	         $("#references").slideToggle("slow");
        //	         $("#carInfo").slideToggle("fast");

        //	         if (shifted == 0) {
        //	             document.getElementById('age').style.marginTop = '-1em';
        //	             shifted = 1;
        //	         } else {
        //	             document.getElementById('age').style.marginTop = '.8em';
        //	             shifted = false;
        //	         }
    });


    var sX;
    var sY;
    var trackXY = {};
    $("#listImages li img").each(function (index) {
        $(this).attr("id", 'image' + (index + 1));

        $(this).mousedown(function (e) {
            trackXY.y = e.pageY;
            trackXY.x = e.pageX;
            console.log("mousedown");
            //START YOUR DRAG STUFF
            if ($(this).hasClass('checked')) {
                $(this).next().attr("checked", true);
                if ($(this).attr("value") == "Upload") {
                    console.log('checked');

                    $(this).addClass('checked');
                }

            } else {

                $(this).next().attr("checked", false);

                if ($(this).attr("value") == "Upload") {
                    console.log('unchecked');
                    $(this).removeClass('checked');
                }


            }

        });
        $(this).mouseup(function (e) {

            console.log("mouseup");
            if (trackXY.x === e.pageX && trackXY.y === trackXY.y) {

                if ($(this).hasClass('checked')) {
                    $(this).next().attr("checked", false);


                    if ($(this).attr("value") == "Upload") {
                        console.log('unchecked');
                        $(this).removeClass('checked');
                    }


                } else {
                    $(this).next().attr("checked", true);
                    if ($(this).attr("value") == "Upload") {
                        console.log('checked');
                        $(this).addClass('checked');
                    }

                }
            }
        });



    });

    $("#listImages, #listImages2").dragsort({
        dragSelector: "div",
        dragBetween: true,
        dragEnd: saveOrder,
        placeHolderTemplate: "<li class='placeHolder'><div></div></li>"
    });

    function saveOrder() {
        var imageorder = "";
        var data = $("#listImages li ").map(function () {
            return $(this).children().html();
        }).get();
        $("input[name=list1SortOrder]").val(data.join(","));

        $('#listImages li img').each(function (index) {
            $("#pIdImage").attr("src", $(this).attr("src"));
            return false;


        });
        $('#listImages li img').each(function (index) {
            if ($(this).attr("value") == "Upload") {
                imageorder += $(this).attr("src") + ",";
            }


        });
        imageorder = imageorder.substring(0, imageorder.length - 1);
        //console.log(imageorder);
        $('#hiddenPhotoOrder').val(imageorder);


    };
    function addCommas(nStr) {
        nStr += ''; x = nStr.split('.'); x1 = x[0]; x2 = x.length > 1 ? '.' + x[1] : ''; var rgx = /(\d+)(\d{3})/; while (rgx.test(x1)) { x1 = x1.replace(rgx, '$1' + ',' + '$2'); }
        return x1 + x2;
    }


    $('#delete').click(function () {
        //console.log('You are deleteing pic');

        var flag = false;
        $('#listImages li img').each(function (index) {
            if ($(this).hasClass('checked')) {
                flag = true;
            }

        });

        var answer;
        if (flag) {

            answer = confirm("Are you sure you want to delete selected images?");
        }
        else {
            alert("You must select at least one image to delete");
        }


        var arrayPic = "";

        if (answer) {
            $('#listImages li img').each(function (index) {
                if (!$(this).hasClass('checked')) {
                    //console.log(index);
                    if ($(this).attr("value") == "Upload") {

                        arrayPic += $(this).attr("src") + ",";
                    }
                    //console.log(arrayPic);


                } else {
                    if ($(this).attr("value") == "Upload") {
                        $(this).attr("value", "Default");
                        $(this).attr("src", '<%=Url.Content("~/images/40x40grey1.jpg")%>');
                        $(this).removeClass('checked');
                    }
                }



            });

            var count = 0;
            var topPics = new Array();
            if (arrayPic != "") {


                arrayPic = arrayPic.substring(0, arrayPic.length - 1);

                topPics = arrayPic.split(",");

                count = topPics.length;
            }


            $('#listImages li img').each(function (index) {


                if (count == 0) {

                    $(this).attr("value", "Default");
                    $(this).attr("src", '<%=Url.Content("~/Images/40x40grey1.jpg")%>');
                    $(this).removeClass('checked');
                }
                else {
                    count--;
                    $(this).attr("value", "Upload");
                    $(this).attr("src", topPics[index]);
                    $(this).removeClass('checked');

                }

            });

            if (topPics.length == 0)
                $("#pIdImage").attr("src", $("#txtHiddenPhotos").val());
            else
                $("#pIdImage").attr("src", topPics[0]);


            $.post('<%= Url.Content("~/Inventory/DeleteCarImageURL") %>', { ListingId: $("#ListingId").val(), CarThubmnailImageURL: arrayPic }, function (data) {
                $("#txtHiddenPhotos").val(arrayPic);
            });
        }

    });


    $('#save').click(function () {
        $('#elementID').removeClass('hideLoader');

        var ListingId = $("#ListingId").val();

        if ($("#hiddenPhotoOrder").val() != null && $("#hiddenPhotoOrder").val() != "") {

            $.post('<%= Url.Content("~/Inventory/UpdateCarImageUrl") %>', { ListingId: ListingId, CarThubmnailImageURL: $("#hiddenPhotoOrder").val() }, function (data) {
                var actionUrl = '<%= Url.Action("ViewIProfile", "Inventory", new { ListingID = "PLACEHOLDER" } ) %>';

                actionUrl = actionUrl.replace('PLACEHOLDER', ListingId);

                window.location = actionUrl;
            });

        }

        $('#elementID').addClass('hideLoader');


    });



    //	     $('#mkWrap h3').click(function() {

    //	         $this = $(this);
    //	         $rowId = $this.parent('div').children('.mr-row').attr('id');
    //	         $parentId = $this.parent('div').attr('id');

    //	         $this.parent('div').addClass('hider');

    //	         $('.mr-row').each(function(index, element) {
    //	             if (this.id != $rowId) {
    //	                 $(this).parent('div').removeClass('hider');
    //	             }

    //	         });

    //	     });
    //	     $('input[name="toggleGraph"]').click(function() {
    ////	         $('#save').toggleClass('hider');
    ////	         $('#delete').toggleClass('hider');
    //	     });
    //	     $('#placeholder').dblclick(function() {
    ////	         $('#save').toggleClass('hider');
    ////	         $('#delete').toggleClass('hider');
    //	     });
    //	     $('#description').dblclick(function() {
    //	         //console.log($("#dropdownWrap").attr("style"));
    //	     var dropdownstyle = $("#dropdownWrap").attr("style");
    //	     if (dropdownstyle.substring(0, dropdownstyle.indexOf(":")) == "overflow") {
    //	         $('input[name="edit"]').val("Save");
    //	     }

    //	     else {
    //	         $('input[name="edit"]').val("Edit");
    //	     }
    //	         //	         $('input[name="ebayCraigslist"]').toggleClass("hider");ed
    //	         //	         $('input[name="print"]').addClass("hider");
    //	         //	         $('input[name="bg"]').addClass("hider");
    //	         //	         $('input[name="ws"]').addClass("hider");
    //	         //	         $('input[name="sold"]').addClass("hider");
    //	         //	         $('input[name="wholesale"]').addClass("hider");
    //	         $("#dropdownWrap").slideDown("fast");
    //	         $("#descriptionWrapper").slideDown("slow");
    //	         $("#detailWrap").slideUp("slow");
    //	     });

    function warrantyInfoUpdate(checkbox) {

        $.post('<%= Url.Content("~/Inventory/UpdateWarrantyInfo") %>', { WarrantyInfo: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

            if (data.SessionTimeOut == "TimeOut") {
                alert("Your session has timed out. Please login back again");
                var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                window.parent.location = actionUrl;
            }


        });
    }
                        
    </script>
     
</asp:Content>



