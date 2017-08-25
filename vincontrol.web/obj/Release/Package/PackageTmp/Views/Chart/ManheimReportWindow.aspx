<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.ManheimReport>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Manheim Report</title>
    
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/tablesorter_blue.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/tablesorter/addons/pager/jquery.tablesorter.pager.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html
        {
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
            background: #e5eaf0;
            color: #000;
            font-size: 13px;
        }
        body
        {
            margin: 0 auto;
        }
    </style>
</head>

<body>
    <div id="container">
        <form id="manheimReportForm" method="post" action="">

            <input type="hidden" id="listingId" name="listingId" value="<%= (int)ViewData["LISTINGID"] %>" />
            <input type="hidden" id="auctionRegion" name="auctionRegion" value="<%= (short)ViewData["REGION"] %>" />
            <input type="hidden" id="vehicleStatus" name="auctionRegion" value="<%= (short)ViewData["VEHICLESTATUS"] %>" />

            <div class="popup_title">

                <div style="display: inline-block; width: 100%;">
                    <div style="float: left; display: inline-block;">
                        <strong class="reportHeader">REPORTED PAST AUCTION SALES</strong>
                    </div>
                    <div style="float: right; font-size: 11px;">
                        <img id="imgPrintToExcel" src="../../Content/images/excel.png" alt="Export to Excel file" title="Export to Excel file" style="width: 22px; cursor: pointer;" />Excel&nbsp;
                        <img id="imgPrintToPDF" src="../../Content/images/pdf.png" alt="Export to PDF file" title="Export to PDF file" style="width: 22px; cursor: pointer;" />PDF
                    </div>
                </div>
            </div>

        </form>

        <div>

            <div style="display: inline-block;">
                <table id="manheimTransaction" class="tablesorter" style="display: inline-block; width: 755px;">
                    <thead style="cursor: pointer;">
                        <tr>

                            <th align="left" width="60px">Sale date
                            </th>
                            <th align="left" width="60px">Price
                            </th>
                            <th align="left" width="60px">Odo.
                            </th>
                            <th align="left" width="80px">Vin
                            </th>
                            <th align="left" width="70px">Engine
                            </th>
                            <th align="left" width="70px">Color
                            </th>
                            <th align="left" width="140px">Auction
                            </th>
                            <th align="left" width="100px">Region
                            </th>
                        </tr>
                    </thead>
                    <tbody id="result"><%= Html.Partial("ManheimPastTransactionDetail", Model.ManheimTransactions) %></tbody>
                </table>
                <%= Html.Partial("_PagerPartial") %>
            </div>
        </div>

    </div>
    
    <script src="<%=Url.Content("~/js/tablesorter/jquery-latest.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/addons/pager/jquery.tablesorter.pager.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var numberOfTransactions = '<%= Model.NumberOfTransactions %>';
        $(document).ready(function () {

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

            $("#imgPrintToPDF").click(function () {
                ExportFile(2);
            });

            $("#imgPrintToExcel").click(function () {
                ExportFile(1);
            });
        });

        function ExportFile(reportType) {
            blockUIPopUp();
            var listingId = $("#listingId").val();
            var auctionRegion = $("#auctionRegion").val();
            var vehicleStatus = $("#vehicleStatus").val();

            window.location.href = "/PDF/PrintManheimPastAuctionReport?listingId=" + listingId + "&vehicleStatus=" + vehicleStatus + "&auctionRegion=" + auctionRegion + "&reportType=" + reportType;

            var flag = true;
            var temp = window.setInterval(function () {
                if (flag) {
                    unblockUI();
                    flag = false;
                }
            }, 20 * numberOfTransactions);
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
</body>
</html>
