<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.PriceChangeViewModel>" %>

<%@ Import Namespace="WhitmanEnterpriseMVC.Controllers" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Price Changes Tracking</title>
    <script src="<%=Url.Content("~/js/jquery.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            background: #222;
            color: #ddd;
        }
        body
        {
            width: 980px;
            margin: 0 auto;
        }
        #container
        {
            background: #333;
            padding: 1em;
        }
        h3, ul
        {
            margin: 0;
        }
        input[type="text"]
        {
            width: 30px;
        }
        span.label
        {
            display: block;
            width: 150px;
            float: left;
            clear: right;
        }
        input[type="submit"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 1.1em;
            font-weight: bold;
            padding: .5em;
            float: right;
            margin-top: -2em;
        }
        .short
        {
            width: 50px !important;
        }
        .submit
        {
            background: none repeat scroll 0 0 #860000;
            border: medium none #000000;
            color: #FFFFFF;
            cursor: pointer;
            display: inline-block;
            font-size: 14px;
            font-weight: normal;
            padding: 2px 8px;
            width: 100px;
            text-align: center;
            padding: 4px 2px;
        }
    </style>
</head>
<body>
    <div>
        <input type="radio" checked="checked" name="selectedTime" value="<%=ChartTimeType.Last7Days%>" onclick="LoadChartAndGridView()" />
        Last 7 days
        <input type="radio" name="selectedTime" value="<%=ChartTimeType.ThisMonth%>" onclick="LoadChartAndGridView()" />
        This month
        <input type="radio" name="selectedTime" value="<%=ChartTimeType.LastMonth%>" onclick="LoadChartAndGridView()" />
        Last month
          <input type="radio" name="selectedTime" value="<%=ChartTimeType.FromBeginning%>" onclick="LoadChartAndGridView()" />
        From Beginning
    </div>
    <div>
        <%--<input type="button" name="Show Tracking" value="Show" id="btnShowTracking" />--%>
        <input type="button" name="Print Tracking" value="Print" id="btnPrintTracking" />
    </div>
    <div id="result">
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            LoadChartAndGridView();
        }
        );
        $("#btnPrintTracking").click(function () {
            window.location = '/Inventory/PrintPriceTracking?itemId=<%=Model.Id%>&type=' + $("input[name=selectedTime]:checked" + '&inventoryStatus=<%=Model.InventoryStatus%>&').val();
        });

        $("#btnShowTracking").click(function () {
            LoadChartAndGridView();
        });

        function LoadChartAndGridView() {
            $.ajax({
                type: "POST",
                url: "/Inventory/PriceTrackingChart",
                data: { type: $("input[name=selectedTime]:checked").val(), itemID: '<%=Model.Id %>', inventoryStatus: '<%=Model.InventoryStatus %>' },
                success: function (results) {
                    $("#result").html(results);
                    $.unblockUI();
                }
            });
        }
        
    </script>
</body>
</html>
