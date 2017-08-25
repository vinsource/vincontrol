<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.PriceChangeViewModel>" %>

<%@ Import Namespace="Vincontrol.Web.Controllers" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Price Changes Tracking</title>
    
    <link href="<%=Url.Content("~/Content/common.css")%>" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="profile_tab_holder profile_popup" id="profile_pricetracking_holder" style="display: block">
        <div class="bucketjump_popup_header">
            Price Tracking Change Chart
        </div>
        <div class="bucketjump_popup_content" style="padding: 15px;">
            <div class="pricetracking_time_select_holder">
                <label>
                    <input type="radio" checked="checked" name="selectedTime" value="<%=ChartTimeType.Last7Days%>"
                        onclick="LoadChartAndGridView()" />
                    Last 7 Days
                </label>
                <label>
                    <input type="radio" name="selectedTime" value="<%=ChartTimeType.ThisMonth%>" onclick="LoadChartAndGridView()" />
                    This Month
                </label>
                <label>
                    <input type="radio" name="selectedTime" value="<%=ChartTimeType.LastMonth%>" onclick="LoadChartAndGridView()" />
                    Last Month
                </label>
                <label>
                    <input type="radio" name="selectedTime" value="<%=ChartTimeType.FromBeginning%>"
                        onclick="LoadChartAndGridView()" />
                    From Beginning
                </label>
            </div>
            <div class="pricetracking_chart_holder">
                <div id="result">
                </div>
            </div>
            <div class="pricetracking_btns_holder">
                <div class="btns_shadow price_tracking_print" id="btnPrintTracking">
                    Print
                </div>
            </div>
        </div>
    </div>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            LoadChartAndGridView();
        }
        );
        $("#btnPrintTracking").click(function () {
            window.location = '/Inventory/PrintPriceTracking?itemId=<%=Model.Id%>&type=' + $("input[name=selectedTime]:checked").val() + '&inventoryStatus=<%=Model.InventoryStatus%>';
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
