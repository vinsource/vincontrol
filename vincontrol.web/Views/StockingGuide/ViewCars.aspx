<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>ViewAllInventory</title>
    
    <style type="text/css">
        .listCars
        {
            background-color: #3366cc;
            width: 190px;
            height: 15px;
            padding: 10px;
            margin: 5px;
            color: white;
            font-weight: bold;
            font-size: 16px;
            cursor: pointer;
        }

        .carLeft
        {
            float: left;
            width: 165px;
        }

        .carRight
        {
            float: right;
            width: 25px;
            text-align: right;
        }
    </style>
</head>
<body>
    <div style="width: 220px; margin: 0 auto; overflow: auto">
        <div id="divMarket" class="listCars" onclick="openListMarketIframe();">
            <div class="carLeft">Market:
                <img id="imgMarketLoading" src="../Content/images/ajaxloadingindicator.gif" height="15px"><span id="marketCountHolder"></span> car(s)</div>
            <div class="carRight">
                <img src="../Content/images/vincontrol/arrowRight.png" /></div>
        </div>
        <div id="divAuction" class="listCars" onclick="openListAuctionIframe();">
            <div class="carLeft">Auction:
                <img id="imgAuctionLoading" src="../Content/images/ajaxloadingindicator.gif" height="15px"><span id="auctionCountHolder"></span> car(s)</div>
            <div class="carRight">
                <img src="../Content/images/vincontrol/arrowRight.png" /></div>
        </div>
        <div id="divAppraisal" onclick="openListCarIframe();" class="listCars">
            <div class="carLeft">Appraisal:
                <img id="imgAppraisalLoading" src="../Content/images/ajaxloadingindicator.gif" height="15px"><span id="appraisalCountHolder"></span> car(s)</div>
            <div class="carRight">
                <img src="../Content/images/vincontrol/arrowRight.png" /></div>
        </div>
    </div>
    <input type="hidden" id="hdMake" value="<%=ViewData["Make"] %>" />
    <input type="hidden" id="hdModel" value="<%=ViewData["Model"] %>" />
    <script src="<%=Url.Content("~/js/jquery-1.7.2.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function openListCarIframe() {
            if ($("#appraisalCountHolder").text() > 0) {
                var make = $('#hdMake').val();
                var model = $('#hdModel').val();

                window.parent.openListCarIframe(make, model);
            }
        }
        function openListMarketIframe() {

            if ($("#marketCountHolder").text() > 0) {
                var make = $('#hdMake').val();
                var model = $('#hdModel').val();
                //console.log(make); console.log(model);
                window.parent.openListMarketIframe(make, model);
            }
        }
        function openListAuctionIframe() {
            if ($("#auctionCountHolder").text() > 0) {
                var make = $('#hdMake').val();
                var model = $('#hdModel').val();
                window.parent.openListAuctionIframe(make, model);
            }
        }

        $(function () {
            $.get("/StockingGuide/GetMarketCount", { make: $("#hdMake").val(), model: $("#hdModel").val() }, function (data) { $("#imgMarketLoading").hide(); $("#marketCountHolder").html(data); });
            $.get("/StockingGuide/GetAuctionCount", { make: $("#hdMake").val(), model: $("#hdModel").val() }, function (data) { $("#imgAuctionLoading").hide(); $("#auctionCountHolder").html(data); });
            $.get("/StockingGuide/GetAppraisalCount", { make: $("#hdMake").val(), model: $("#hdModel").val() }, function (data) { $("#imgAppraisalLoading").hide(); $("#appraisalCountHolder").html(data); });
        });
    </script>
</body>
</html>
