<%@ Page Title="Appraisal Profile" MasterPageFile="~/Views/Shared/Site.Master" Language="C#"
    Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <%=Model.ModelYear %>
        <%=Model.Make %>
        <%=Model.AppraisalModel %></title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Css/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet"
        type="text/css" media="screen" />

    <%--<script src="http://code.jquery.com/jquery-latest.js" type="text/javascript"></script>--%>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.dragsort.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.flot.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.flot.symbols.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/jquery.flot.functions.js")%>" type="text/javascript"></script>

    <link href="<%=Url.Content("~/js/plot.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet"
        type="text/css" />

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>

    <style type="text/css">
        .scroll-pane
        {
            height: 100%;
            overflow: auto;
            overflow-x: hidden;
        }
        #notes
        {
            height: 200px;
            margin-bottom: 6em;
        }
        #carInfo
        {
            background: #333;
            padding: .6em;
            margin-top: 2em;
        }
        #carInfo h3
        {
            margin-top: 0;
        }
        #figures
        {
            margin-bottom: -75px;
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
        #references
        {
            margin-top: 1em !important;
        }
        #listImages
        {
            overflow-x: hidden !important;
        }
        .sortMiles:hover
        {
            cursor: pointer;
            color: red;
        }
        .sortMiles
        {
            color: white;
            font-weight: bold;
        }
        #t1
        {
            position: relative;
            top: -16px;
        }
        #sorting
        {
            width: 500px;
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
        input[name="acv"]
        {
            background: green;
            color: white;
        }
        input[name="notes"]
        {
            background: black !important;
        }
        .topbtns
        {
            margin-top: 0 !important;
        }
        input[name="edit"]
        {
            position: relative;
            margin-top: 0;
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
        #kbb div, #kbb h3, #kbb span, #bb div, #bb h3, #bb span,#manheim div, #manheim h3, #manheim span
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
        #placeholder
        {
            background: url('../images/loading.jpg') center no-repeat !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />
    <input type="hidden" id="NeedToReloadPage" name="NeedToReloadPage" value="false" />
    <div id="dropdownWrap">
        <% Html.BeginForm("Action", "Appraisal", FormMethod.Post); %>
        <div class="clear">
        </div>
    </div>
    <div id="divReportButton" class="clear">
        <%--<a class="pad_tab" title="Edit" href="<%= Url.Content("~/Appraisal/EditAppraisal?AppraisalId=")%>">Add to Appraisal</a>--%>
        <%--<a class="iframe" href="<%=Url.Content("~/TradeIn/VinInput")%>"  title="Edit" href="<%= Url.Content("~/Appraisal/EditAppraisal?AppraisalId=")%>">Add to Appraisal</a>--%>
        <% if (String.IsNullOrEmpty(Model.VinNumber))
           { %>
        <a class="iframe pad_tab" href="<%=Url.Content("~/TradeIn/InputVIN/"+Model.AppraisalID)%>"
            title="Edit">Add to Appraisal</a>
        <%}
           else
           {%>
        <a id="btnVIN" class="pad_tab" title="Edit" href="javascript:;">Add to Appraisal</a>
        <%} %>
    </div>
    <div id="detailWrap">
        <div id="images" class="column" style="margin: 0 !important;">
            <img style="width: 160px; height: 160px" src="<%= Url.Content("~/content/images/vin-trade-icon.gif") %>" />
            <%=Html.HiddenFor(x => x.AppraisalID)%>
        </div>
        <div id="profileInfo">
            <h3>
                <%=  Html.DynamicHtmlLabel("txtTitle", "Title")%></h3>
            <ul class="column">
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelVin", "Vin")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelAppraisal", "AppraisalID")%>
                </li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelStock", "Stock")%>
                </li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelYear", "Year")%>
                </li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelMake", "Make")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelModel", "Model")%>
                </li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelTrim", "Trim")%>
                </li>
                <li style="width:140px;">
                    <%=  Html.DynamicHtmlLabel("txtLabelExteriorColor", "ExteriorColor")%>
                </li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelInteriorColor", "InteriorColor")%></li>
            </ul>
            <ul class="column">
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelTranmission", "Tranmission")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelOdometer", "Odometer")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelCylinders", "Cylinders")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelLitters", "Litters")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelDoors", "Doors")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelFuelType", "FuelType")%>
                </li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelMSRP", "MSRP")%></li>
                <li>
                    <%=  Html.DynamicHtmlLabel("txtLabelDriveType", "DriveType")%></li>
            </ul>
            <div id="description" class="clear">
                <p class="bot">
                    <%=  Html.DynamicHtmlLabel("txtDescription", "Description")%>
                </p>
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
                <%=Html.ExpandChartButton()%>
            </div>
            <div id="age" class="column">
                <h3>
                    <span id="totalCars" class="green">
                        <label id="txtCarsOnMarket">
                            0</label></span> Cars on Market</h3>
                <p class="top">
                </p>
                <p class="bot">
                </p>
                <h3 style="margin-bottom: 0;">
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
                <h4>
                    <span class="green">
                        <label id="txtAvgDays">
                            0</label></span> Avg Days in Inventory</h4>
            </div>
        </div>
        <div id="references">
            <p class="top">
            </p>
            <p class="bot">
            </p>
            <div id="carfax">
                <div id="carfax-header">
                        <a class="iframe" href="http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.VinNumber%>"
                        target="_blank">
                        <img style="display: inline-block; float: left;" src='<%=Url.Content("~/Images/carfax-large.jpg")%>'
                            alt="carfax logo" /></a>
                    <h3 style="">
                        Summary</h3>
                </div>
                <%if (Model.CarFax != null && Model.CarFax.Success)
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
                            Service Report(s)</div>
                    </div>
                </div>
                <%} %>
                <div id="report-wrapper">
                    <div id="history-report">
                        <%if (Model.CarFax != null)
                          { %>
                        <ul>
                            <%foreach (WhitmanEnterpriseMVC.Models.CarFaxWindowSticker tmp in Model.CarFax.ReportList)
                              {  %>
                            <li>
                                <%=tmp.Text%>
                                <img class="c-fax-img" src="<%=tmp.Image %>" height="10px" width="10px"/>
                            </li>
                            <%} %>
                        </ul>
                             <a class="iframe" href="http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.VinNumber%>"
                        target="_blank">View
                            Full Report</a>
                        <%} %>
                    </div>
                </div>
                <!--<textarea  class="z-index" id="Textarea3" name="description" cols="55" rows="15" ><%=ViewData["CarFaxResponse"]%></textarea>-->
            </div>
            <%if ((bool)Session["IsEmployee"] == false)
              {%>
            <p class="bot">
            </p>
            <div id="manheim">
                <h3 class="mr-on">
                    <a target="_blank" style="padding-right:15px;" target="_blank" title="MMR Auction Pricing" href="<%=Url.Content("~/Market/OpenManaheimLoginWindow?Vin=")%><%=Model.VinNumber%>">
                        <img height="20" style="position: relative; top: 2px;" src="http://www.microsoft.com/southafrica/microsoftservices/images/logo/logo_manheim.gif" alt="MMR Auction Pricing"/>
                        Manheim </a>
                </h3>
                
                    <% if (Model.ManheimWholesales != null && Model.ManheimWholesales.Count > 0) {%>
                    <% foreach (var manheimWholesale in Model.ManheimWholesales){%>
                    <div id="manheim-row" class="mr-row">
                    <div class="range-item label">
                        <a class="iframe" style="font-size: .7em" title="<%= manheimWholesale.TrimName %>" href="<%=Url.Content("~/Report/ManheimTransactionDetail?year=")%><%=manheimWholesale.Year%>&make=<%=manheimWholesale.MakeServiceId%>&model=<%=manheimWholesale.ModelServiceId%>&trim=<%=manheimWholesale.TrimServiceId%>">
                            <%= manheimWholesale.TrimName %>
                        </a>
                    </div>
                    <div class="low-wrap range-item low">
                        <span class="bb-price"><%= manheimWholesale.LowestPrice %></span>
                    </div>
                    <div class="mid-wrap range-item mid">
                        <span class="bb-price"><%= manheimWholesale.AveragePrice %></span>
                    </div>
                    <div class="high-wrap range-item high">
                        <span class="bb-price"><%= manheimWholesale.HighestPrice %></span>
                    </div>
                    </div>
                    <%} %>
                    
                    <%} else {%>
                    <div id="manheim-row" class="mr-row">
                    There is no Manheim value associated with this vehicle
                    </div>
                    <%} %>
                
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
                    <%if (Model.KBB!=null && Model.KBB.Success)
                      {
                          foreach (WhitmanEnterpriseMVC.Models.KellyBlueBookTrimReport trimDetail in Model.KBB.TrimReportList)
                          { %>
                    <div id="kbb-row" class="mr-row">
                        <div class="range-item label">
                            <a class="iframe" style="font-size: .7em" title="<%=trimDetail.TrimName%>" href="<%=Url.Content("~/Market/GetKellyBlueBookSummaryAppraisalByTrim?AppraisalId=")%><%=Model.AppraisalGenerateId %>&TrimId=<%=trimDetail.TrimId %> ">
                                <%=trimDetail.TrimName%></a></div>
                        <div class="low-wrap range-item low">
                            <span class="bb-price">
                                <%=trimDetail.BaseWholesale%></span>
                        </div>
                        <div class="mid-wrap range-item mid">
                            <%if (trimDetail.MileageAdjustment >= 0)
                              {%>
                            <span class="bb-price">+
                                <%= trimDetail.MileageAdjustment%></span>
                            <% }
                              else
                              {%>
                            <span class="bb-price">
                                <%= trimDetail.MileageAdjustment %></span>
                            <% } %>
                        </div>
                        <div class="high-wrap range-item high">
                            <span class="bb-price">=
                                <%=trimDetail.WholeSale%></span>
                        </div>
                    </div>
                    <%
                        }%>
                    <%}
                      else
                      {  %>
                    <div id="kbb-row" class="mr-row">
                        There is no KBB value associated with this vehicle
                    </div>
                    <%} %>
                    <%--<%=Html.ReportKellyBlueBookButtonInAppraisal()%>--%>
                </div>
                <%--   <div id="kbb-row" class="mr-row">
                                <strong>Getting KBB and Black Book Values...</strong>
                                </div>--%>
                <div id="bb" class="hider">
                    <h3>
                        <img height="20" style="position: relative; top: 2px;" src="http://cdn.autofusion.com/apps/mm4/4.1/images/extras/kbb-logo-alpha.png" />
                        Kelly Blue Book
                    </h3>
                    <h3 style="padding: 4px;" class="mr-on">
                        <img height="25" style="position: relative; top: 4px;" src='<%=Url.Content("~/images/bblogo.png")%>' />
                        Black Book
                    </h3>                    
                    <%if (Model.BB!=null&& Model.BB.Success)
                      {
                          foreach (WhitmanEnterpriseMVC.Models.BlackBookTrimReport trimDetail in Model.BB.TrimReportList)
                          {
                    %>
                    <div id="bb-row" class="mr-row">
                        <div class="range-item label">
                            <%=trimDetail.TrimName%>
                        </div>
                        <div class="low-wrap range-item low">
                            <span class="bb-title">Low</span> <span class="bb-price">
                                <%=trimDetail.TradeInRough%></span>
                        </div>
                        <div class="mid-wrap range-item mid">
                            <span class="bb-title">Mid</span> <span class="bb-price">
                                <%=trimDetail.TradeInAvg%></span>
                        </div>
                        <div class="high-wrap range-item high">
                            <span class="bb-title">High</span> <span class="bb-price">
                                <%=trimDetail.TradeInClean%></span>
                        </div>
                    </div>
                    <%
                        }%>
                    <%}
                      else
                      {  %>
                    <div id="bb-row" class="mr-row">
                        There is no Black Book value associated with this vehicle
                    </div>
                    <%} %>
                    <%=Html.ReportBlackBookButton()%>
                </div>
            </div>
            <%}%>
        </div>
    </div>
    </div>
    <div id="c3" class="column">
        <div id="listImages">
            <h3>
                <b style="color: white; font-size: 1em;">Customer Information</b></h3>
            <div id="space" class="label">
                First Name:
            </div>
            <%=Html.TextBoxFor(x => x.CustomerFirstName, new { @class = "z-index" })%>
            <div class="label">
                Last Name:
            </div>
            <%=Html.TextBoxFor(x => x.CustomerLastName, new {@class="z-index" })%>
            <div class="label">
                Address:
            </div>
            <%=Html.TextBoxFor(x => x.CustomerAddress, new { @class = "z-index" })%>
            <div class="label">
                City:
            </div>
            <%=Html.TextBoxFor(x => x.CustomerCity, new { @class = "z-index" })%>
            <div class="label">
                State:
            </div>
            <%=Html.TextBoxFor(x => x.CustomerState, new { @class = "z-index" })%>
            <div class="label">
                ZIP:
            </div>
            <%=Html.TextBoxFor(x => x.CustomerZipCode, new { @class = "z-index" })%>
            <br />
            <br />
            <div class="label">
                <b style="color: white; font-size: 1.1em;">Trade in Value</b>:
            </div>
            <%=Html.TextBoxFor(x => x.ACV, new { @class = "z-index" })%>
            <%--   <h3>
                Appraisal Options</h3>
            <input type="submit" class="pad" id="AddToInventory" name="AddToInventory" value="Add to Inventory" />
            <input type="submit" class="pad" id="AddToWholeSale" name="AddToWholeSale" value="Add to WholeSale" />
            <input type="button" class="pad" name="print" onclick="window.print()" value="Print Appraisal" />--%>
        </div>
        <div class="clear">
        </div>
    </div>
    </div>
    <% Html.EndForm(); %>

    <script src="<%=Url.Content("~/js/graph_plotter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/ViewAppraisal.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
//        function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
//        // ############################### //
//        // Chart Data Set and Draw Section //
//        // ############################### //
//        // check url for GET
//        var window_url = window.location.search;

//        // default graph to unexpanded draw size
//        var expand = expanded(window_url);

//        // set chart dimensions
//        if (expand) {
//            chart_dimensions = ["700px", "500px"];
//            $('#graphButton').css('display', 'none');
//        } else {
//            chart_dimensions = ["300px", "143px"];
//        }

//        // grab graph div element and click element
//        var gwrap = $('#graphWrap');
//        gwrap.css('width', chart_dimensions[0]);
//        gwrap.css('height', chart_dimensions[1]);
//        $(document).ready(function(e) {

//            // initialize data set array	
//            $data = [];
//            var AppraisalID = $('#AppraisalID').val();

//            var request_url = '<%= Url.Action("GetCustomerMarketDataWithin50MilesRadius", "chart", new { customerId = "PLACEHOLDER" } ) %>';

//            request_url = request_url.replace('PLACEHOLDER', AppraisalID);
//            //var request_url = "/chart/GetMarketDataWithin100MilesRadius?appraisalId=" + AppraisalID;
//            $.ajax({
//                url: request_url,
//                context: document.body
//            }).done(function(data) {
//                var carlist = data.carlist;
//                var marketinfo = data.market;
//                var target = data.target;
//                if (carlist == null) {
//                    $('#age').html('<em style="display: block; margin-top: 35px;">Market Data Not Available</em>');
//                    carlist = [];
//                }

//                if (marketinfo != null) {


//                    $("#txtCarsOnMarket").text(marketinfo.carsOnMarket);
//                    $("#txtMinPrice").text(marketinfo.minimumPrice);
//                    $("#txtAveragePrice").text(marketinfo.averagePrice);
//                    $("#txtMaxPrice").text(marketinfo.maximumPrice);
//                    $("#txtAvgDays").text(marketinfo.averageDays);
//                }

//                for (var i = 0; i < carlist.length; i++) {
//                    var arr = carlist;
//                    // build object
//                    $imgstring = "";

//                    $car = {
//                        title: // year make model
//                        {
//                        year: arr[i].title.year,
//                        make: ucfirst(arr[i].title.make),
//                        model: ucfirst(arr[i].title.model),
//                        trim: ucfirst(arr[i].title.trim)
//                    },
//                    vin: arr[i].vin, 			// vin number
//                    miles: arr[i].miles, 			// mileage
//                    price: arr[i].price, 			// price
//                    extcolor: arr[i].color.exterior, // exterior color
//                    distance: arr[i].distance, // distance from dealer
//                    certified: arr[i].certified, 		// certified 0/1 (false/true)
//                    option: arr[i].option, 		// options string, comma deliminated
//                    trim: arr[i].trim, 			// trim 
//                    uptime: arr[i].uptime, 		// time on cars.com, autotrader.com and ebay
//                    thumbnail: arr[i].thumbnail				// img string, comma deliminated
//                }; // after construction add object to array	
//                $data[i] = $car;
//            }

//            //set Dealer Vehicle
//            //set Dealer Vehicle
//            if (carlist.length > 0) {
//                var $dCar = {
//                    label: '',
//                    color: '#003cff',
//                    shadowSize: 5,
//                    points: {
//                        radius: 10,
//                        fillColor: 'white'
//                    },
//                    data: [[target.mileage /* miles */, target.salePrice /* price */]],
//                    clickable: false,
//                    draggabley: true,
//                    draggablex: false
//                };
//            } else {
//                var $dCar = {
//                    label: '',
//                    color: '#003cff',
//                    shadowSize: 5,
//                    points: {
//                        radius: 10,
//                        fillColor: 'white'
//                    },
//                    data: [[0 /* miles */, 0 /* price */]],
//                    clickable: false,
//                    draggabley: true,
//                    draggablex: false
//                };
//                $filter = {};
//            }


//            // default graph to unexpanded draw size
//            var expand = false;

//            // set ranges
//            var ranged = setRange($data);

//            // save original pricing
//            var originalPrice = $dCar.data[0][1];

//            // get chart range 
//            var fRange = 100; // default

//            // filter script
//            $('#certified').click(function() {
//                // check for selected range setting
//                var rangeSet = $('.selected').attr('id');
//                // check to see if certified is checked or unchecked, if checked apply filter
//                if (this.checked) { $filter.certified = 1; }
//                // draw chart
//                drawChart($data, fRange, $filter, $dCar, expand);
//                // unset filter
//                $filter = {};
//            });

//            // chart expand/contract script
//            $('#graphButton').click(function() {
//                if (expand) {
//                } else {
//                    var gwrap_fbox = $('#graph-fancybox');
//                    // open fancybox
//                    gwrap_fbox.click();
//                }

//            });

//            // range navigation script
//            $('#rangeNav span').click(function() {
//                // remove selected class from all range nav elements
//                $('.selected').removeClass('selected');
//                // apply selected class to clicked element
//                $(this).addClass('selected');
//                // apply clicked element id to fRange
//                fRange = this.id;
//                // check filter settings for certified checkbox
//                if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }
//                // draw chart
//                drawChart($data, fRange, $filter, $dCar, expand);
//                // unset filter
//                $filter = {};
//            });

//            // chart expand/contract script



//            // draw chart
//            drawChart($data, fRange, $filter, $dCar, expand);

//            // set y change check
//            var newY = false;

//            // set plotclick function
//            $("#placeholder").bind("plotclick", function(event, pos, item) {
//                if (item.datapoint[0] == $dCar.data[0][0] && item.datapoint[1] == $dCar.data[0][1]) {

//                } else {
//                    sidebar(item, $dCar, $data);
//                }

//            });

//            // set dragging
//            $("#placeholder").bind("plotFinalSeriesChange", function(event, seriesIndex, dataIndex, x, y) {
//                // flag movement
//                newY = true;
//                // make sure the plot clicked is the vehicle, then rest its values
//                if (x.toFixed(0) != $dCar.data[0][0] && y.toFixed(0) != $dCar.data[0][1]) { } else {
//                    // generate metrics for testing.
//                    var graphM = new gCon($data);
//                    var m = new metrics($data);

//                    // check if vehicle marker has left the chart boundaries
//                    if (y.toFixed(0) > graphM.priceRangeEnd) {
//                        // if vehicle has higher price that chart displays, set it to 100 below chart maximum price
//                        $dCar.data[0][1] = graphM.priceRangeEnd.toFixed(0) - 100;
//                    } else if (y.toFixed(0) < 0) {
//                        // if vehicle is set to below 0 or threshold of the chart, set its price to 0 or above threshold
//                        if (0 > graphM.startPriceRange) { $dcar.data[0][1] = 0; } else { $dCar.data[0][1] = graphM.startPriceRange + 100; }
//                    } else {
//                        // if none of the above are true, then place vehicle on the graph at its new price
//                        $dCar.data[0][1] = y.toFixed(0);
//                        sidebar(item, $dCar, $data);
//                    }
//                }

//                // check filter settings
//                if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }

//                // redraw chart to replot vehicle.
//                drawChart($data, fRange, $filter, $dCar, expand);

//            });


//            // check for changes on graph.
////            $('a').click(function() {
////                var link = $(this).attr('class');
////                if (link != 'iframe') {
////                    if (newY) {
////                        // if external link clicked and the graph has been changed, do stuff
////                        confirm('The pricing was changed from $' + addCommas(originalPrice) + ' to $' + addCommas($dCar.data[0][1]) + '. Save changes?');
////                    } else {
////                        // if external link is clicked and the graph has not been changed, do stuff
////                    }
////                } else {
////                    // if not an external link (fancybox/iframe class), do stuff
////                }
////            });

//            $('#graphButton.iframe').fancybox({
//                autoDimensions: true,
//                width: 940,
//                height: 615
//            });

//            $("#btnVIN").click(function() {
//                var vin = '<%=Model.VinNumber %>';

//                var theDate = new Date();

//                {

//                    $('#elementID').removeClass('hideLoader');


//                    $.post('<%= Url.Content("~/Decode/VinDecode") %>', { vin: vin }, function(result) {
//                        if (result.Status == "SoldOut") {

//                            var actionUrl = '<%= Url.Action("SoldOutAlert", "Inventory", new { ListingID = "PLACEHOLDER" } ) %>';

//                            actionUrl = actionUrl.replace('PLACEHOLDER', result.ListingId);

//                            $('#elementID').addClass('hideLoader');

//                            $("<a href=" + actionUrl + "></a>").fancybox({
//                                overlayShow: true,
//                                showCloseButton: true,
//                                enableEscapeButton: true,

//                                onClosed: function() {
//                                    window.location.reload(true);
//                                }


//                            }).click();




//                        }
//                        else if (result.Status == "Inventory") {


//                            var actionUrl = '<%= Url.Action("CreateAppraisal", "Appraisal", new { ID = "PLACEHOLDER" ,ActionName ="ViewIProfile",CustomerID= Model.AppraisalID } ) %>';

//                            actionUrl = actionUrl.replace('PLACEHOLDER', result.ListingId);

//                            window.location = actionUrl;
//                        }
//                        else if (result.Status == "Appraisal") {
//                            var actionUrl = '<%= Url.Action("CreateAppraisal", "Appraisal", new { ID = "PLACEHOLDER", ActionName ="ViewProfileForAppraisal",CustomerID= Model.AppraisalID } ) %>';

//                            actionUrl = actionUrl.replace('PLACEHOLDER', result.AppraisalId);

//                            window.location = actionUrl;


//                        }
//                        else if (result.Status == "VinProcessing") {
//                            var actionUrl = '<%= Url.Action("CreateAppraisal", "Appraisal", new { ID = "PLACEHOLDER", ActionName ="DecodeProcessingByVin",CustomerID= Model.AppraisalID } ) %>';

//                            actionUrl = actionUrl.replace('PLACEHOLDER', result.Vin);

//                            window.location = actionUrl;




//                        }
//                        else if (result.Status == "VinInvalid") {
//                            $('#elementID').addClass('hideLoader');
//                            var actionUrl = '<%= Url.Action("InvalidVinAlert", "Decode", new { Vin = "PLACEHOLDER" } ) %>';

//                            actionUrl = actionUrl.replace('PLACEHOLDER', result.Vin);

//                            $("<a href=" + actionUrl + "></a>").fancybox({
//                                overlayShow: true,
//                                showCloseButton: true,
//                                enableEscapeButton: true,

//                                onClosed: function() {
//                                    window.location.reload(true);
//                                }


//                            }).click();

//                        }


//                    });
//                }

//            });
//        });

//        if ($("#IsEmployee").val() == 'True') {
//            $("#divReportButton").hide();
//        }
        //    });
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

    <script language="javascript" type="text/javascript">
        //          $(document).ready(function(e) {
        //              var AppraisalID = $('#AppraisalGenerateId').val();

        //              var request_url = "/Vincontrol/Ajax/GetKBBWholeSale?appraisalId=" + AppraisalID;
        //              $.ajax({
        //                  url: request_url,
        //                  context: document.body
        //              }).done(function(data) {
        //                  if (data.Success) {
        //                      var sb = new StringBuilder();
        //                      for (var i = 0; i < data.TrimReportList.length; i++) {

        //                          var trimDetail = data.TrimReportList[i];

        //                          sb.appendLine("<div id='kbb-row' class='mr-row'>");
        //                          sb.appendLine("<div class='range-item label'>");
        //                          sb.appendLine("<a class='iframe' style='font-size: .7em' title='" + trimDetail.TrimName + "' href='/Market/GetKellyBlueBookSummaryAppraisalByTrim?AppraisalId=" + AppraisalID + "&TrimId=" + trimDetail.TrimId + "' >" + trimDetail.TrimName + "</a>");
        //                          //sb.appendLine(trimDetail.TrimName);
        //                          sb.appendLine("</div>");
        //                          sb.appendLine("  <div class=\"low-wrap range-item low\">");
        //                          sb.appendLine("<span class=\"bb-price\">" + trimDetail.BaseWholesale + "</span>");
        //                          sb.appendLine("</div>");
        //                          sb.appendLine("<div class=\"mid-wrap range-item mid\">");
        //                          if (trimDetail.MileageAdjustment > 0)
        //                              sb.appendLine("<span class=\"bb-price\">+" + trimDetail.MileageAdjustment + "</span>");
        //                          else
        //                              sb.appendLine("<span class=\"bb-price\">" + trimDetail.MileageAdjustment + "</span>");
        //                          sb.appendLine("</div>");
        //                          sb.appendLine(" <div class='high-wrap range-item high'>");
        //                          sb.appendLine("<span class='bb-price'>" + trimDetail.WholeSale + "</span>");
        //                          sb.appendLine("  </div>");
        //                          sb.appendLine("</div>");


        //                      }
        //                      sb.appendLine("<a class='iframe' target='blank' style='float:right; font-weight:normal; font-size: .9em;' href='/Market/GetKellyBlueBookSummaryAppraisal?AppraisalId=1485'>View KBB Summary Report</a>");
        //                      $("#kbb-row").remove();

        //                      $("#kbb").append(sb.toString());

        //                  } else {
        //                  
        //                  sb = new StringBuilder();
        //                      sb.appendLine("<div id='kbb-row' class='mr-row'>");
        //                      sb.appendLine("There is no KBB value associated with this vehicle");
        //                      sb.appendLine("</div>");

        //                      $("#kbb-row").remove();

        //                      $("#kbb").append(sb.toString());

        //                  }
        //              });

        //          });
        $("#AddToInventory").click(function() {

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

    </script>

</asp:Content>
