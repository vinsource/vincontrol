<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.ManheimReport>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Manheim Report</title>
    <script src="<%=Url.Content("~/js/tablesorter/jquery-latest.js")%>" type="text/javascript"></script>
    <%--<script src="<%=Url.Content("~/js/jquery-1.6.1.min.js")%>" type="text/javascript"></script>--%>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/addons/pager/jquery.tablesorter.pager.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var numberOfTransactions = '<%= Model.NumberOfTransactions %>';
        var pageIndex = '<%= (int)ViewData["ManheimPageIndex"] %>';
        var pageSize = '<%= (int)ViewData["ManheimPageSize"] %>';
        $(document).ready(function () {
            EnableViewMore(numberOfTransactions, pageIndex, pageSize);
            // add parser through the tablesorter addParser method 
            $.tablesorter.addParser({
                // set a unique id 
                id: 'price',
                is: function (s) {
                    // return false so this parser is not auto detected 
                    return false;
                },
                format: function (s) {
                    // format your data for normalization 
                    return s.replace('$', '').replace(/,/g, '');
                },
                // set type, either numeric or text 
                type: 'numeric'
            });

            $("table#manheimTransaction")
                .tablesorter({
                    sortList: [[0, 1]]
                    , headers: {
                        //0: { sorter: true, sortInitialOrder: 'desc' },
                        2: { sorter: 'price' }, // miles
                        3: { sorter: 'price' }// prices
                    }
                })
                .tablesorterPager({ container: $("#pager") });

            $("#ddlRegion").change(function () {
                blockUIPopUp();
                var year = $("#Year").val();
                var make = $("#Make").val();
                var model = $("#Model").val();
                var trim = $("#Trim").val();
                var region = $("#ddlRegion").val();
                window.location.href = "/Report/ManheimTransactionDetail?year=" + year + "&make=" + make + "&model=" + model + "&trim=" + trim + "&region=" + region /*+ "&pageIndex=" + pageIndex + "&pageSize=" + pageSize*/;
            });

            $("#viewMore").click(function () {
                blockUIPopUp("table#manheimTransaction");
                var year = $("#Year").val();
                var make = $("#Make").val();
                var model = $("#Model").val();
                var trim = $("#Trim").val();
                var region = $("#ddlRegion").val();
                $.ajax({
                    type: "POST",
                    url: "/Report/ManheimReportDetail?year=" + year + "&make=" + make + "&model=" + model + "&trim=" + trim + "&region=" + region + "&pageIndex=" + (++pageIndex) + "&pageSize=" + pageSize,
                    data: {},
                    success: function (results) {
                        //pageIndex++;
                        EnableViewMore(numberOfTransactions, pageIndex, pageSize);
                        $("#result").append(results);
                        unblockUI("table#manheimTransaction");

                        TriggerSorting();
                    }
                });
            });
            unblockUI();

            //Paging(1);
            $("a[id^=nextPage]").click(function () {
                blockUIPopUp("table#manheimTransaction");
                var i = parseInt(this.id.split('_')[1]);
                $("a[id^=nextPage]").removeClass("active");
                $(this).addClass("active");

                Paging(i);
            });

            $("#imgPrintToPDF").click(function () {
                ExportFile(2);
            });

            $("#imgPrintToExcel").click(function () {
                ExportFile(1);
            });
        });

        function Paging(i) {
            var year = $("#Year").val();
            var make = $("#Make").val();
            var model = $("#Model").val();
            var trim = $("#Trim").val();
            var region = $("#ddlRegion").val();
            $.ajax({
                type: "POST",
                url: "/Report/ManheimReportDetail?year=" + year + "&make=" + make + "&model=" + model + "&trim=" + trim + "&region=" + region + "&pageIndex=" + (i) + "&pageSize=" + pageSize,
                data: {},
                success: function (results) {
                    //$("table#manheimTransaction tbody").empty();
                    //$("table#manheimTransaction > tbody > tr").remove();
                    //$("table#manheimTransaction > tbody").innerHTML = results;
                    $("#result").html(results);
                    //EnableViewMore(numberOfTransactions, pageIndex, pageSize);
                    //$("#result").html(results);
                    unblockUI("table#manheimTransaction");
                    //$("table#manheimTransaction").trigger("update");
                    TriggerSorting();
                }
            });
        }

        function ExportFile(reportType) {
            blockUIPopUp();
            var year = $("#Year").val();
            var make = $("#Make").val();
            var model = $("#Model").val();
            var trim = $("#Trim").val();
            var region = $("#ddlRegion").val();
            var regionName = $("#ddlRegion option:selected").text();

            window.location.href = "/PDF/PrintManheimReportDetail?year=" + year + "&make=" + make + "&model=" + model + "&trim=" + trim + "&regionValue=" + region + "&regionName=" + regionName + "&reportType=" + reportType;

            var flag = true;
            var temp = window.setInterval(function () {
                if (flag) {
                    unblockUI();
                    flag = false;
                }
            }, 20 * numberOfTransactions);
        }

        function EnableViewMore(numberOfTransactions, pageIndex, pageSize) {
            if (numberOfTransactions > (pageIndex * pageSize))
                $('#viewMore').show();
            else
                $('#viewMore').hide();
        }

        function TriggerSorting() {
            var sortUp = ($("table#manheimTransaction thead tr th").index($(".headerSortUp")));
            var sortDown = ($("table#manheimTransaction thead tr th").index($(".headerSortDown")));
            var sorting = [[sortUp == -1 ? sortDown : sortUp, sortUp == -1 ? 0 : 1]];
            $("table#manheimTransaction").trigger("update");
            setTimeout(function () {
                $("table#manheimTransaction").trigger("sorton", [sorting]);
            }, 100);

        }
    </script>

    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/tablesorter_blue.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/tablesorter/addons/pager/jquery.tablesorter.pager.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html {
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
            background: #e5eaf0;
            color: #000;  
            font-size: 13px;          
        }
        body {
            
            margin: 0 auto;
        }        
    </style>
</head>

<body>
    <div id="container">
        <form id="manheimReportForm" method="post" action="">
            <% if (!Model.IsAuction)
               {%>
            <input type="hidden" id="Year" name="Year" value="<%= (string)ViewData["ManheimYear"] %>" />
            <input type="hidden" id="Make" name="Make" value="<%= (string)ViewData["ManheimMake"] %>" />
            <input type="hidden" id="Model" name="Model" value="<%= (string)ViewData["ManheimModel"] %>" />
            <input type="hidden" id="Trim" name="Trim" value="<%= (string)ViewData["ManheimTrim"] %>" />
            <%} %>
            
            <div class="popup_title">

                <div style="display:inline-block; width: 100%;">
                    <% if (!Model.IsAuction)
                       {%>
                    <div style="float: left; display: inline-block;">
                        <select id="ddlRegion" name="ddlRegion">
                            <option value="NA" <%= Model.Region.Equals("NA") ? "selected" : "" %>>National</option>
                            <option value="SE" <%= Model.Region.Equals("SE") ? "selected" : "" %>>South East</option>
                            <option value="NE" <%= Model.Region.Equals("NE") ? "selected" : "" %>>North East</option>
                            <option value="MW" <%= Model.Region.Equals("MW") ? "selected" : "" %>>Mid West</option>
                            <option value="SW" <%= Model.Region.Equals("SW") ? "selected" : "" %>>South West</option>
                            <option value="WC" <%= Model.Region.Equals("WC") ? "selected" : "" %>>West Coast</option>
                        </select>

                        <strong class="reportHeader">REPORTED WHOLESALE AUCTION SALES - With Exact Matches</strong>
                    </div>
                    <%} %>
                    <div style="float: right; font-size: 11px;">
                        <img id="imgPrintToExcel" src="../../Content/images/excel.png" alt="Export to Excel file" title="Export to Excel file" style="width: 22px; cursor: pointer;" />Excel&nbsp;
                    <img id="imgPrintToPDF" src="../../Content/images/pdf.png" alt="Export to PDF file" title="Export to PDF file" style="width: 22px; cursor: pointer;" />PDF
                    </div>
                </div>
            </div>
            
        </form>

        <div>
            <% if (!Model.IsAuction)
               {%>
            <div style="float: left; width: 130px; display: inline-block; border: 1px solid black; padding: 4px;">
                <div>
                    High Price:	<b><%= Model.HighestPrice %></b><br />
                    Low Price: <b><%= Model.LowestPrice %></b><br />
                    Avg Price: <b><%= Model.AveragePrice %></b><br />
                    Avg Odo: <b><%= Model.AverageOdometer %></b><br />
                    # of Vehicles: <b><%= Model.NumberOfTransactions %></b>
                </div>

            </div>
            <%} %>
            <div style="display: inline-block;">
                <table id="manheimTransaction" class="tablesorter" style="display: inline-block; width: 605px;">
                    <thead style="cursor: pointer;">
                        <tr>
                            <th align="left" width="80px">Sale date
                            </th>
                            <th align="left" width="100px">Auction
                            </th>
                            <th align="left" width="75px">Odometer
                            </th>
                            <th align="left" width="75px">Price
                            </th>
                            <th align="left" width="50px">Engine
                            </th>
                            <th align="left" width="50px">Cond
                            </th>
                            <th align="left" width="75px">Color
                            </th>
                            <th align="left" width="75px">In Sample
                            </th>
                        </tr>
                    </thead>
                    <tbody id="result"><%= Html.Partial("ManheimReportDetail", Model.ManheimTransactions) %></tbody>
                </table>
                <%= Html.Partial("_PagerPartial") %>
            </div>
        </div>

    </div>

</body>
</html>
