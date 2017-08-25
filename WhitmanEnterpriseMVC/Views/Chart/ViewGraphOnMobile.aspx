<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.ChartSelectionViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Graph V2</title>

    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>--%>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/extension.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <script src="<%=Url.Content("~/js/jquery.flot.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.symbols.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.flot.functions.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ui.dropdownchecklist-1.4-min.js")%>" type="text/javascript"></script>

    <style type="text/css">
        body
        {
            background: #eeeeee;
            font-family: 'Trebuchet MS' , Arial, sans-serif !important;
        }
        #result
        {
            font-family: "Trebuchet MS" , Helvetica, sans-serif;
        }
        .hidden
        {
            display: none;
        }
        #side-nav-wrap
        {
            position: fixed;
            top: 175px;
            right: 20px;
            overflow: hidden;
            width: 220px;
        }
        #rangeNav span
        {
            display: inline-block;
            width: 50px;
            text-align: center;
            padding: .3em .7em .3em .7em;
            background: #222222;
            color: white;
        }
        #rangeNav span.selected
        {
            font-weight: bold;
            background-color: #C80000;
        }
        #graphWrap
        {
            width: 300px;
            height: 143px;
            display: inline-block;
            float: left;
            margin-bottom: 50px;
        }
        #carInfo
        {
            display: inline-block;
            padding: 10px;
            background-color: #c80000;
            color: #eeeeee;
            width: 200px;
            font-size: .8em;
        }
        #filter-wrap
        {
            background: #c80000;
            padding: .5em;
            color: white;
            width: 98%;
        }
        input[type="radio"]
        {
            background: transparent;
        }
        /*######################################################################*/
        /*NEW CSS ##############################################################*/
        /*######################################################################*/
      #vehicle-list table { width: 680px; font-size: 1.0em;}
#vehicle-list td { padding: .3em .7em .3em .7em; border-bottom: 1px #bbbbbb solid;}
#vehicle-list tr:nth-child(2n+2) td {background: #cccccc;}
#vehicle-list tr.highlight td {background: green; color: #fff; }
/*#vehicle-list tr:nth-child(1) td {font-weight: bold;color: white;background: #222222;}*/
 

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
            background: #c80000;
            color: white;
            position: relative;
            top: -3px;
            font-size: .9em;
            cursor: pointer;
        }
        #graph-title-bar a:hover
        {
            background: #880000;
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
            background: url('../images/loading.jpg') center no-repeat;
        }
        .market-info
        {
            background: #dddddd;
            width: 200px;
            padding-left: 20px;
            display: inline-block;
            padding-bottom: 10px;
        }
        
        .highlightselected td {background: #880000 !important; color: white !important ; position: relative; left: 2px; box-shadow: 2px 2px 2px #333333; border: none !important; -moz-box-shadow: 2px 2px 2px #000000; border: none !important;}
        /*######################################################################*/
        /*NEW CSS ##############################################################*/
        /*######################################################################*/
   </style>
</head>
<body>
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
    <%=Html.Hidden("ListingId",ViewData["ListingId"]  ) %>

<% } %>
    
    <a id="viewGoogleMapLink" class="pad_tab" href="javascript:;">View Map</a>
    <div id="result">
        <h2>
            <%=ViewData["CarTitle"]%></h2>         
        <div id="graphWrap">
            <div id="placeholder" style="height: 100%; width: 100%;">
            </div>
        </div>
        <br />
         <div id="side-nav-wrap">
            <div id="carInfo">
                <h3>
                    Vehicle Info</h3>
                <%=Html.Hidden("ListingId",ViewData["ListingId"]  ) %>
                <b><span id="car"></span></b>
                <br />
                <span id="car-thumb"></span>
                <br />
                <span id="AutoTraderLink"></span>
                <br />
                <span id="CarsComLink"></span>
                <br />
                Miles: <span id="miles">0</span> (<span id="diffM"></span>)<br />
                Price: $<span id="price">0</span> (<span id="diffP"></span>)<br />
                Seller: <span id="seller"></span>
                <br />
                Address: <span id="address"></span>
                <br />
                <%--Days on Market: <span id="daysOnMarket"></span><br />--%>
                Distance: <span id="distance"></span>
                <br />
                <%--  <br />
            Certified: <span id="certified-span"></span>
            <br />--%>
                <br />
            </div>
            <div class="market-info">
                <h4 style="margin-bottom: 0;">
                    Market Price Ranges</h4>
                <h5 style="margin-top: 0; margin-bottom: .5em;">
                    <span class="blue" id="low"></span>|<span class="green" id="middle"></span>| <span
                        class="red" id="high"></span>
                </h5>
            </div>
        </div>        
    </div>
    <div id="google-maps" style="height: 500px; width: 99%;">
    </div>
    <div id="graph-title-bar">
         <h2 id="NumberofCarsOnTheChart">
            List of Charted Vehicles 
        </h2>
       
        <%--<a class="btn-print">Print List</a>
        <%if (Session["CanViewBucketJumpReport"] != null && (bool)Session["CanViewBucketJumpReport"]){%>
        <a id="btnPrintBucketJump">Print Bucket Jump</a><%}%>--%>
    </div>    
    <div id="printable-list">
        <div id="vehicle-list" style="font-size: .6em">
            <table id="tblVehicles" cellspacing="0" style="display: none;">                
                <thead style="background-color:#000; color: #fff; height:20px; cursor:pointer;">
                    <tr>
                        <th>#</th>
                        <th align="center">Year</th>
                        <th align="center">Make</th>
                        <th align="left">Model</th>
                        <th align="left">Trim</th>
                        <th align="left">Distance</th>
                        <th align="center">Certified</th>
                        <th>Seller</th>
                        <th align="center">Miles</th>
                        <th align="center">Price</th>
                        <th>Carscom</th>
                        <th>Autotrader</th>
                    </tr>                    
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </div>       
    <script src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/markerclusterer/src/markerclusterer_compiled.js"
        type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/VINGoogleMap.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/graph_plotter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/LoadFilterAndEvents.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/ViewGraph.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var isEmployee = ($("#IsEmployee").val());        
        // ############################### //
        // Chart Data Set and Draw Section //
        // ############################### //
        function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
        
        // check url for GET
        var window_url = window.location.search;

        // default graph to unexpanded draw size
        var expand = true;
        // set chart dimensions
        if (expand) {
            chart_dimensions = ["700px", "500px"];
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

        // create ajax post url
        var requestCarsComUrl = '<%= Url.Action("GetMarketDataByListingFromCarsComWithHttpPost", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
        var requestAutoTraderUrl = '<%= Url.Action("GetMarketDataByListingFromAutoTraderWithHttpPost", "chart", new { ListingId = "PLACEHOLDER" } ) %>';        
        var requestHiddenCarsComUrl = '<%= Url.Action("LoadMarketDataByListingFromCarsComIntoSession", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
        var requestHiddenAutoTraderUrl = '<%= Url.Action("LoadMarketDataByListingFromAutoTraderIntoSession", "chart", new { ListingId = "PLACEHOLDER" } ) %>';
        var updateSalePrice = '<%= Url.Content("~/Inventory/UpdateSalePrice") %>';
        var logOff = '<%= Url.Action("LogOff", "Account" ) %>';
        var detailUrl = '<%= Url.Action("ViewIProfile", "Inventory", new { ListingID = "PLACEHOLDER" } ) %>';
        var waitingImage = '<%= Url.Content("~/images/ajax-loader1.gif") %>';
        var chartType = 'Inventory';

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

    </script>
    
</body>
</html>
