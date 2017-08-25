<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.Chart.ChartSelectionViewModel>" %>

<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Graph V2</title>

<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
<link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" media="screen" />
<link href="<%=Url.Content("~/Content/VinControl/chart.css")%>" rel="stylesheet" type="text/css" />
<link href="<%=Url.Content("~/Content/VinControl/MarketChartNew.css")%>" rel="stylesheet" type="text/css" />
<style type="text/css">
    /*######################################################################*/
    /*PRINT CSS ##############################################################*/
    /*######################################################################*/
    span.ui-dropdownchecklist-selector
    {
        width: 106px !important;
        background-image: url('/Content/vincontrol/110.png') !important;
        padding: 3px 2px 2px 2px !important;
    }

    .centerImg
    {
        height: 25px;
    }

    .centerImgHolder
    {
        height: 25px;
        display: table;
        margin: 0px auto;
    }

    span.ui-dropdownchecklist-text
    {
        width: 90px !important;
    }

    #vehicle-list table
    {
        width: 785px;
        overflow: hidden;
        font-size: 1.1em;
    }

    #vehicle-list td
    {
        padding: .3em .7em .3em .7em;
        border-bottom: 1px #bbbbbb solid;
    }

    #vehicle-list tr:nth-child(2n + 2) td
    {
        background: #cccccc;
    }

    #vehicle-list tr.highlight td
    {
        background: green;
        color: #fff;
    }

    #vehicle-list table tbody tr td
    {
        text-align: center;
    }

    #vehicle-list table thead tr th
    {
        text-align: center;
    }
    /*#vehicle-list tr:nth-child(1) td
        {
            font-weight: bold;
            color: white;
            background: #222222;
        }*/
    #graph-title-bar
    {
        position: relative;
        height: 50px !important;
        max-height: 50px !important;
        overflow: hidden;
        width: 99%;
        display: block;
    }

        #graph-title-bar h2
        {
            display: inline-block;
            padding-bottom: 0;
            margin-bottom: 0;
        }

        #graph-title-bar a
        {
            margin-left: 20px;
            padding: .2em .5em .2em .5em;
            background: #3366cc;
            color: white;
            position: relative;
            top: -3px;
            font-size: .9em;
            cursor: pointer;
        }

            #graph-title-bar a:hover
            {
                background: #3366cc;
            }

    @media print
    {
        #result
        {
            display: none;
        }

        #printable-list
        {
            display: block;
        }
        /*#vehicle-list tr:nth-child(1) td
            {
                font-weight: bold;
                color: black;
                border-bottom: #C80000 4px solid;
            }*/
        #graph-title-bar a
        {
            display: none;
        }
    }

    .blue
    {
        color: blue;
    }

    .red
    {
        color: red;
    }

    .green
    {
        color: green;
    }

    #placeholder
    {
    }

    .market-info
    {
        background: #dddddd;
        width: 200px;
        padding-left: 20px;
        display: inline-block;
        padding-bottom: 10px;
    }

    .highlightselected td
    {
        background: #3366cc !important;
        color: white !important;
        position: relative;
        left: 2px;
        box-shadow: 2px 2px 2px #333333;
        border: none !important;
        -moz-box-shadow: 2px 2px 2px #000000;
        border: none !important;
    }

    a.pad_tab
    {
        display: inline-block;
        background: #111111;
        font-size: 0.9em /*1.1em*/;
        color: white;
        font-weight: bold;
    }

        a.pad_tab:hover
        {
            color: red;
            font-weight: bold;
            cursor: pointer;
        }

    a.pad_tab
    {
        color: white;
        text-decoration: none;
        font-weight: bold;
    }

    .ui-dropdownchecklist-selector
    {
        width: 110px !important;
    }

    .ui-dropdownchecklist
    {
        padding-top: 3px;
    }

    .ui-dropdownchecklist-item
    {
        text-align: left;
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
        font-size: 11px;
    }
</style>

<% using (Html.BeginForm("PrintGraphInfo", "PDF", null, FormMethod.Post, new { id = "myform" }))
   { %>
<input type="hidden" id="content" name="content" value="" />
<input type="hidden" id="PdfContent" name="PdfContent" value="<%= Model.PdfContent %>" />
<input type="hidden" id="IsCarsCom" name="IsCarsCom" value="<%= Model.IsCarsCom %>" />
<input type="hidden" id="Options" name="Options" value="<%= Model.Options %>" />
<input type="hidden" id="Trims" name="Trims" value="<%= Model.Trims %>" />
<input type="hidden" id="IsCertified" name="IsCertified" value="<%= Model.IsCertified %>" />
<input type="hidden" id="IsAll" name="IsAll" value="<%= Model.IsAll %>" />
<input type="hidden" id="IsFranchise" name="IsFranchise" value="<%= Model.IsFranchise %>" />
<input type="hidden" id="IsIndependant" name="IsIndependant" value="<%= Model.IsIndependant %>" />
<input type="hidden" id="CarsCom_IsCarsCom" name="CarsCom_IsCarsCom" value="<%= Model.CarsCom.IsCarsCom %>" />
<input type="hidden" id="CarsCom_Options" name="CarsCom_Options" value="<%= Model.CarsCom.Options %>" />
<input type="hidden" id="CarsCom_Trims" name="CarsCom_Trims" value="<%= Model.CarsCom.Trims %>" />
<input type="hidden" id="CarsCom_IsCertified" name="CarsCom_IsCertified" value="<%= Model.CarsCom.IsCertified %>" />
<input type="hidden" id="CarsCom_IsAll" name="CarsCom_IsAll" value="<%= Model.CarsCom.IsAll %>" />
<input type="hidden" id="CarsCom_IsFranchise" name="CarsCom_IsFranchise" value="<%= Model.CarsCom.IsFranchise %>" />
<input type="hidden" id="CarsCom_IsIndependant" name="CarsCom_IsIndependant" value="<%= Model.CarsCom.IsIndependant %>" />
<input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= Session["IsEmployee"] %>" />
<input type="hidden" id="CarListingId" name="listingId" />
<input type="hidden" id="FilterOptions" name="filterOptions" value="<%=Model.FilterOptions %>" />
<% } %>

<div id="result" class="market_popup_holder_v2">

    <div class="mp_v2_header">
        <div class="mp_v2_header_left">
            <div class="mp_v2_hl_nummeric" id="rangeNav">
                <span class="mp_v2_hl_items selected" id="100">100 </span><span class="mp_v2_hl_items"
                    id="250">250 </span><span class="mp_v2_hl_items" id="500">500 </span><span class="mp_v2_hl_items"
                        id="nation">Nation </span><span class="mp_v2_hl_items" id="viewGoogleMapLink" onclick="ViewMap();">Map</span> <span class="mp_v2_hl_items" id="viewLinkChart" style="display: none"
                            onclick="ViewChart();">Chart</span>
                <div class="mp_v2_hl_items" id="divMore">
                    <a class="iframe iframeMore" href="<%=Url.Action("MoreDistance","Chart") %>" id="moredistance" style="position: relative">...</a>
                </div>

            </div>
            <div class="mp_v2_hl_carinfo" title="<%=ViewData[Constanst.CarTitle]%>">
                <%=ViewData[Constanst.CarTitle]%>
            </div>
        </div>
        <div class="mp_v2_header_right">
            <div class="mp_v2_hr_info">
                <%=ViewData[Constanst.CarMileAndPrice]%>
            </div>
        </div>
    </div>
</div>
<div class="mp_v2_content_holder">
    <div class="mp_v2_content_left">
        <div class="mp_v2_cl_header">
            <div class="mp_v2_cl_items mp_v2_cl_trim">
                <label>
                    Trims</label>
                <select id="trim-filter" multiple="multiple" style="height: 20px;">
                    <option>Any Trim</option>
                </select>
                <%--<input type="text" />--%>
            </div>
            <div class="mp_v2_cl_items mp_v2_cl_fi">
                <div class="mp_v2_cl_fi_all_new">
                    <input type="radio" id="all" checked="checked" name="dealertype" key="all" class="tab_radio_All" /><label
                        for="all">All</label>
                </div>
                <div class="mp_v2_cl_itemsmp_v2_cl_fi_franchise mp_v2_cl_btns">
                    <input type="radio" id="franchise" name="dealertype" key="franchise" class="tab_radio" /><label
                        for="franchise">Franchise</label>
                </div>
                <div class="mp_v2_cl_fi_independant mp_v2_cl_btns">
                    <input type="radio" id="independant" name="dealertype" key="independant" class="tab_radio" /><label
                        for="independant">independent</label>
                </div>
            </div>
            <div class="mp_v2_cl_items mp_v2_cl_fi">
                <div class="mp_v2_cl_fi_all_new" style="background-color: red!important; color: white!important">
                    <input type="radio" id="radioAll" checked="checked" name="webSource" key="all" class="tab_radio_All_car" /><label
                        for="radioAll">All</label>
                </div>
                <div class="mp_v2_cl_fi_carscom mp_v2_cl_btns" style="background-color: blue!important; color: white!important">
                    <input type="radio" id="radioCarsCom" name="webSource" key="carscom" class="tab_radio_car" /><label
                        for="radioCarsCom">CarsCom</label>
                </div>
                <div class="mp_v2_cl_fi_at mp_v2_cl_btns" style="background-color: black!important; color: white!important">
                    <input type="radio" id="radioAutoTrader" name="webSource" key="autotrader" class="tab_radio_car" /><label
                        for="radioAutoTrader">AutoTrader</label>
                </div>
            </div>
            <div class="mp_v2_cl_save_setting" style="margin-left: 10px;">
                <% if (Model.Type == Constanst.CarInfoType.Appraisal) %>
                <%
                   { %>
                <a href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {appraisalID = Model.Id}) %>">
                    <input type="button" id="Button1" name="btnCancelChart" class="btnSaveSelection"
                        value="Cancel" />
                </a>
                <% }
                   else if (Model.Type == Constanst.CarInfoType.Used)
                   { %>
                <a href="<%= Url.Action("ViewIProfile", "Inventory", new {ListingID = Model.Id}) %>">
                    <input type="button" id="btnCancelChart" name="btnCancelChart" class="btnSaveSelection"
                        value="Cancel" />
                </a>
                <% } %>
            </div>
            <div class="mp_v2_cl_save_setting" style="margin-left: 1px;">
                <input type="button" id="btnSave" name="btnSave" class="btnSaveSelection" value="Save Selections" />
            </div>
            <% if (Model.Type == Constanst.CarInfoType.Appraisal) %>
            <%
               { %>
            <a href="<%= Url.Action("ViewProfileForAppraisal", "Appraisal", new {appraisalID = Model.Id}) %>">
                <div style="background: black; float: left; color: white; padding: 5px 10px; border: 2px solid white; font-weight: bold; position: relative; top: 55px; right: 3px; z-index: 100; cursor: pointer;">
                    X
                </div>
            </a>
            <% }
               else if (Model.Type == Constanst.CarInfoType.Used)
               { %>
            <a href="<%= Url.Action("ViewIProfile", "Inventory", new {ListingID = Model.Id}) %>">
                <div style="background: black; float: left; color: white; padding: 5px 10px; border: 2px solid white; font-weight: bold; position: relative; top: 55px; right: 3px; z-index: 100; cursor: pointer;">
                    X
                </div>
            </a>
            <% } %>
        </div>
        <div class="mp_v2_scroll_holder">
            <div id="divMap" style="display: none;">
                <div id="google-maps" style="height: 450px; width: 99%">
                </div>
            </div>
            <div class="mp_v2_chart_holder" id="divChart" style="width: 100%;">
                <div id="graphWrap">
                    <div id="placeholder" style="height: 100%; width: 100%;">
                    </div>
                </div>
            </div>
            <div class="mp_v2_list_holder" style="clear: both">
                <div id="graph-title-bar" style="padding-top: 50px;">
                    <h2 id="NumberofCarsOnTheChart">List of Charted Vehicles
                    </h2>
                    <a class="btn-print">Print List</a>
                    <%if (Session["CanViewBucketJumpReport"] != null && (bool)Session["CanViewBucketJumpReport"])
                      {%>
                    <a id="btnPrintBucketJump">Print Bucket Jump</a><%}%>
                </div>
                <div id="printable-list">
                    <div id="vehicle-list" style="font-size: .6em">
                        <table id="tblVehicles" cellspacing="0" style="display: none;">
                            <thead style="background-color: gray; color: #fff; height: 20px; cursor: pointer;">
                                <tr>
                                    <th>#
                                    </th>
                                    <th align="center">Year
                                    </th>
                                    <th align="center">Make
                                    </th>
                                    <th align="left">Model
                                    </th>
                                    <th align="left" style="width: 80px">Trim
                                    </th>
                                    <th align="left">Dist
                                    </th>
                                    <th style="width: 150px">Seller
                                    </th>
                                    <th align="center">Miles
                                    </th>
                                    <th align="center">Price
                                    </th>
                                    <th align="center">Age
                                    </th>
                                    <th>
                                        <img src="/Content/images/carscom.png" height="18px" />
                                    </th>
                                    <th>
                                        <img src="/Content/images/autotrader.png" height="18px" />
                                    </th>
                                    <th>
                                        <img src="/Content/images/carfax.png" height="18px" />
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="mp_v2_content_right">
        <div class="mp_v2_right_header">
            Comparison
                <%=Html.Hidden("ListingId",ViewData["ListingId"]  ) %>
        </div>
        <div class="mp_v2_right_carinfo" id="divNoCarInfo" style="padding: 15px; text-align: center">
            No vehicle selected to compare.
        </div>
        <div class="mp_v2_right_carinfo" id="carInfo">
            <div class="mp_v2_rc_ymm">
                <span id="car"></span>
                <br />
                <span>Age:
                        <label id="lblAge"></label>
                </span>
            </div>
            <div class="mp_v2_rc_img">
                <span id="car-thumb"></span>
                <br />
                <div class="centerImg">
                    <div class="centerImgHolder">
                        <div id="CarsComLink" style="float: left;"></div>
                        <div id="AutoTraderLink"
                            style="float: left; padding-left: 10px">
                        </div>
                    </div>

                </div>
            </div>
            <div class="mp_v2_rc_comparison_holder" style="clear: both">
                <div class="mp_v2_cp_items">
                    <div class="mp_v2_cp_items_left">
                        <label class="mpv2_cp_items_title">
                            Miles</label>
                        <label class="mpv2_cp_items_value">
                            <span id="miles">0</span></label>
                    </div>
                    <div class="mp_v2_cp_items_left">
                        <label class="mpv2_cp_items_title">
                            Difference</label>
                        <label class="mpv2_cp_items_value">
                            &nbsp;<span id="diffM"></span>
                        </label>
                    </div>
                </div>
                <div class="mp_v2_cp_items">
                    <div class="mp_v2_cp_items_left">
                        <label class="mpv2_cp_items_title">
                            Price</label>
                        <label class="mpv2_cp_items_value">
                            $<span id="price">0</span></label>
                    </div>
                    <div class="mp_v2_cp_items_left">
                        <label class="mpv2_cp_items_title">
                            Difference</label>
                        <label class="mpv2_cp_items_value">
                            &nbsp;<span id="diffP"></span></label>
                    </div>
                </div>
            </div>
            <div class="mpv2_rc_dealer_info">
                <label class="mpv2_rc_dealer_name">
                    <span id="seller"></span>
                </label>
                <div class="mpv2_rc_dealer_address">
                    <span id="address"></span>
                </div>
            </div>
            <div class="mpv2_rc_distance">
                Distance - <span id="distance"></span>
            </div>
        </div>
        <div class="mp_v2_right_header">
            Market Ranges
        </div>
        <div class="mpv2_rc_market_info">
            <div class="mpv2_rc_mi_items">
                <div class="mpv2_rcmi_items_left">
                    <label>
                        <div class="mpv2_rcmi_items_legent mpv2_rcmi_items_legent_hight">
                        </div>
                        High
                    </label>
                </div>
                <div class="mpv2_rcmi_items_right mpv2_rcmi_items_height">
                    <span class="red" id="high"></span>
                </div>
            </div>
            <div class="mpv2_rc_mi_items">
                <div class="mpv2_rcmi_items_left">
                    <label>
                        <div class="mpv2_rcmi_items_legent mpv2_rcmi_items_legent_equals">
                        </div>
                        Average
                    </label>
                </div>
                <div class="mpv2_rcmi_items_right mpv2_rcmi_items_equals">
                    <span class="green" id="middle"></span>
                </div>
            </div>
            <div class="mpv2_rc_mi_items">
                <div class="mpv2_rcmi_items_left">
                    <label>
                        <div class="mpv2_rcmi_items_legent mpv2_rcmi_items_legent_low">
                        </div>
                        Low
                    </label>
                </div>
                <div class="mpv2_rcmi_items_right mpv2_rcmi_items_low">
                    <span class="blue" id="low"></span>
                </div>
            </div>
        </div>
        <div class="mpv2_rc_market_info" style="display: none">
            <div class="mpv2_rc_mi_items">
                <div class="mpv2_rcmi_items_left">
                    <label style="padding-left: 14px;">
                        <img src="/Content/images/vincontrol/red.png" />
                        <label style="padding-left: 9px;">
                            All</label>
                    </label>
                </div>
            </div>
            <div class="mpv2_rc_mi_items">
                <div class="mpv2_rcmi_items_left">
                    <label style="padding-left: 14px;">
                        <img src="/Content/images/vincontrol/black.png" />
                        <label style="padding-left: 9px;">
                            Auto Trader</label>
                    </label>
                </div>
            </div>
            <div class="mpv2_rc_mi_items">
                <div class="mpv2_rcmi_items_left">
                    <label style="padding-left: 14px;">
                        <img src="/Content/images/vincontrol/blue.png" />
                        <label style="padding-left: 9px;">
                            CarsCom</label>
                    </label>
                </div>
            </div>
        </div>
    </div>
</div>
<br />

<script src="<%=Url.Content("~/js/Google/jsapi.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/extension.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/SideBar.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/GridView.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/jquery.flot.functions.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/ui.dropdownchecklist-1.4-min.js")%>" type="text/javascript"></script>
<script type="text/javascript">
    var dealerLongtitude = <%=SessionHandler.Dealer.Longtitude%>;
    var dealerLatitude = <%=SessionHandler.Dealer.Latitude%>;
</script>
<script src="<%=Url.Content("~/js/jsrender.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/VINGoogleMap.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/MarketChartInit.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/google_map_graph_plotter.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Chart/LoadFilterAndEvents.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/js/Google/markerclusterer_compiled.js")%>" type="text/javascript"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/d3/d3.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.d3.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/return-large.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/func.js")%>"></script>
<script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/chart.js")%>"></script>
<script type="text/javascript">
    function ViewMap() {
        if(typeof(VINControl) != "undefined"){
            $('#google-maps').show();
            if ($('#google-maps').html().trim() == '') {
                var zoomLevel = VINControl.GoogleMap.GetZoomLevel(dealerLongtitude, dealerLatitude);

                if (typeof google !== 'undefined' && typeof google.maps !== 'undefined') {
                    var map = new google.maps.Map(document.getElementById('google-maps'), {
                        zoom: zoomLevel.zoom,
                        center: new google.maps.LatLng(zoomLevel.latitude, zoomLevel.longtitude),
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    });

                    var objconvertHelper = new VINControl.GoogleMap.ConvertHelper(map);
                    var markers = objconvertHelper.convertToMarkerPoints($currentFilterredList);
                    //if (dealerCar != null) {
                    //    dealerCar.longtitude = dealerLongtitude;
                    //    dealerCar.latitude = dealerLatitude;
                    //    var marker = objconvertHelper.convertToMarkerPoint(dealerCar, map);
                    //    marker.setIcon('https://maps.google.com/mapfiles/kml/shapes/schools_maps.png');
                    //    marker.setMap(map);
                    //}
                    //            markers.push(marker);
                    //            console.log(dealerCar);
                    var markerCluster = new MarkerClusterer(map, markers);
                    //            google.maps.event.trigger(map, 'resize');
                    google.maps.event.addListener(markerCluster, 'clusterclick', function(cluster) {
                        var testMarker = cluster.getMarkers();
                        // your code here
                        pop_smalllist(testMarker);
                    });

                    //marker.setMap(map);
                }
            }
        }
        $('#divMap').show();
        $('#divChart').hide();

        $('#viewLinkChart').show();
        $('#viewGoogleMapLink').hide();
            
        $('#carInfo').hide();
        $('#divNoCarInfo').show();
    }

    function ViewChart() {
        $('#divChart').show();
        $('#divMap').hide();

        $('#viewGoogleMapLink').show();
        $('#viewLinkChart').hide();
            
        $('#carInfo').hide();
        $('#divNoCarInfo').show();
    }
        
    function ShowAdvancedDistanceSearch() {
            
    }

    function RedrawChartNew(fRangeFrom, fRangeTo)
    {
        $.blockUI({ message: '<div><div style="display:inline-block;width:auto;text-align:center;"><img src="' + '/images/ajaxloadingindicator.gif' + '" style="display:inline-block;width:100%;text-align:center;"/></div></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        obj = drawChart($data,  {min:fRangeFrom, max:fRangeTo }, ChartInfo.$filter, $dCar, expand, default_trim, dataTrims);
        $('#divMore a').text(fRangeFrom + ' - ' + fRangeTo);
        $.unblockUI();
    }

    function MoreClick()
    {
        $('#rangeNav span').each(function(){
            $(this).removeClass('selected');
        });
        $('#divMore').addClass('mp_v2_hl_items_active');
        $("#moreImage").attr("src","/Content/images/vincontrol/Chart/dot_on.png");
    }

    var isEmployee = ($("#IsEmployee").val());
    // ############################### //
    // Chart Data Set and Draw Section //
    // ############################### //
    function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
    // check url for GET
    // check url for GET
    var window_url = window.location.search;

    // default graph to unexpanded draw size
    var expand = true;

    // set chart dimensions
    if (expand) {
        chart_dimensions = ["785px", "500px"];
        $('#expand').css('display', 'none');
    } else {
        chart_dimensions = ["300px", "143px"];
    }

    // grab graph div element and click element
    var gwrap = $('#graphWrap');
    gwrap.css('width', chart_dimensions[0]);
    gwrap.css('height', chart_dimensions[1]);

    // load default options & trims
    var default_option = ($('#Options').val() == '' || $('#Options').val() == '0') ? [0] : $('#Options').val().split(',');
    var default_trim = ($('#Trims').val() == '' || $('#Trims').val() == '0') ? [0] : $('#Trims').val().split(',');

    var ListingId = $('#ListingId').val();
    var vinCurrentScreen = <%=String.IsNullOrEmpty(Request.QueryString["currentScreen"])?Constanst.VehicleStatus.Appraisal : Constanst.VehicleStatus.Inventory %>;
    // create ajax post url
    var UrlPaths = UrlPaths || { requestNationwideUrl: "" };
    UrlPaths.requestNationwideUrl = '<%= Url.Action("GetMarketDataByListingNationwideWithHttpPost", "chart", new { ListingId = "PLACEHOLDER" , screen = String.IsNullOrEmpty(Request.QueryString["currentScreen"])?Constanst.VehicleStatus.Appraisal : Constanst.VehicleStatus.Inventory } ) %>';
        var updateSalePrice = '<%= Url.Content("~/Inventory/UpdateSalePrice") %>';
    var logOff = '<%= Url.Action("LogOff", "Account" ) %>';
    var detailUrl = '<%= Url.Action("ViewIProfile", "Inventory", new { ListingID = "PLACEHOLDER" } ) %>';
    var waitingImage = '<%= Url.Content("~/images/ajaxloadingindicator.gif") %>';
    var chartType = <%=Constanst.VehicleStatus.Inventory %>;

    // create filter
    var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange:{min:0,max:100}, isSoldView:false, isSmallChart:false };
      
    var dataTrims = null;
     
    // get chart range
    //var fRange = {"from":0,"to":100}; // default
   

    var $dCar = {};
    var $selectedCar = {};
    // set y change check
    var newY = false;

    // create current filterred list of car
    var $currentFilterredList = [];
    var isSoldView = false;
        
    $(document).ready(function(e) {
            
        $('#carInfo').hide();
        $('#divNoCarInfo').show();
            
        $("a.iframeCustomerInfo").fancybox({ 'width': 330, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        $("a.iframeStatus").fancybox({ 'margin': 0, 'padding': 0, 'width': 500, 'height': 260, 'hideOnOverlayClick': false, 'centerOnScroll': true });
            
        $("a.iframeCommon").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeTransfer").fancybox({ 'width': 350, 'height': 257, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeMarkSold").fancybox({ 'width': 455, 'height': 306, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeWholeSale").fancybox({ 'width': 360, 'height': 127, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeTrackingPrice").fancybox({ 'width': 870, 'height': 509, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeBucketJump").fancybox({ 'width': 929, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeCustomerInfo").fancybox({ 'width': 455, 'height': 230, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeMore").fancybox({ 'width': 370, 'height': 100, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        $("#btnSoldView").click(function() {
            UrlPaths.requestNationwideUrl= '<%= Url.Action("GetMarketDataByListingNationwideWithHttpPostBySold", "chart", new { ListingId = "PLACEHOLDER", screen = String.IsNullOrEmpty(Request.QueryString["currentScreen"])?Constanst.VehicleStatus.Appraisal : Constanst.VehicleStatus.Inventory } ) %>';
            $("#btnCurrentView").removeClass("selected_view");
            $("#btnSoldView").addClass("selected_view");
            isSoldView = true;
            InitializeChart();
              
        });
        $("#btnCurrentView").click(function() {
            UrlPaths.requestNationwideUrl= '<%= Url.Action("GetMarketDataByListingNationwideWithHttpPost", "chart", new { ListingId = "PLACEHOLDER", screen = String.IsNullOrEmpty(Request.QueryString["currentScreen"])?Constanst.VehicleStatus.Appraisal : Constanst.VehicleStatus.Inventory   } ) %>';
                $("#btnSoldView").removeClass("selected_view");
                $("#btnCurrentView").addClass("selected_view");
                isSoldView = false;
                InitializeChart();
            });

    });
</script>

<%=Html.Partial("_TemplateMarketChart")  %>