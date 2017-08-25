<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Vincontrol.Web.Models.ManheimRegionVehicle>>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>View Auctions</title>

    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
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
        /*th, td
        {
            border: solid 1px #cdcdcd;
            font-weight: normal!important;
        }*/
    </style>
</head>
<body>
    <input type="hidden" id="make" name="make" value="<%= (string)ViewData["Make"] %>" />
    <input type="hidden" id="model" name="model" value="<%= (string)ViewData["Model"] %>" />
    <div class="popup_title" style="padding: 5px;">
        <div style="display: inline-block; width: 100%;">
            <div style="float: left; display: inline-block;">
                <%=ViewData["Make"] %> <%=ViewData["Model"] %>
            </div>
            <div style="float: right; font-size: 11px;">
                <img id="imgPrintToExcel" src="../../Content/images/excel.png" alt="Export to Excel file" title="Export to Excel file" style="width: 22px; cursor: pointer;" />Excel&nbsp;
            <img id="imgPrintToPDF" src="../../Content/images/pdf.png" alt="Export to PDF file" title="Export to PDF file" style="width: 22px; cursor: pointer;" />PDF
            </div>
        </div>
    </div>
    <div class="kpi_print_options" id="status_popup_holder">
        <div style="width: auto; margin: 0 auto; overflow: auto">
            <table id="tblVehicles" class="tablesorter" style="">
                <thead style="cursor: pointer">
                    <tr>
                        <th style="width: 50px; text-align: center">Year
                        </th>
                        <th style="width: 60px; text-align: center">Make
                        </th>
                        <th style="width: 70px; text-align: center">Model
                        </th>
                        <th style="width: 90px; text-align: center">Trim
                        </th>
                        <th style="width: 90px; text-align: center">Body
                        </th>
                        <th style="width: 90px; text-align: center">Vin
                        </th>
                        <th style="width: 90px; text-align: center">Region
                        </th>
                        <th style="width: 90px; text-align: center">Seller
                        </th>
                        <th style="width: 60px; text-align: center">Miles
                        </th>
                        <th style="width: 60px; text-align: center">MMR
                        </th>
                        <th style="width: 40px; text-align: center">Age
                        </th>
                        <th style="width: 90px; text-align: center">Auction Date
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <% for (int i = 0; i < Model.Count; i++)
                       {%>
                    <tr style="">
                        <td style="width: 50px; text-align: center"><a href="<%=Model[i].VinsellUrl %>" target="_blank"><%=Model[i].ManheimVehicle.Year %></a></td>
                        <td style="width: 60px; text-align: center"><a href="<%=Model[i].VinsellUrl %>" target="_blank"><%=Model[i].ManheimVehicle.Make %></a></td>
                        <td style="width: 70px; text-align: center"><a href="<%=Model[i].VinsellUrl %>" target="_blank"><%=Model[i].ManheimVehicle.Model %></a></td>
                        <td style="width: 90px; text-align: center"><%=Model[i].ManheimVehicle.Trim %></td>
                        <td style="width: 90px; text-align: center"><%=Model[i].ManheimVehicle.BodyStyle %></td>
                        <td style="width: 90px; text-align: center"><%=Model[i].ManheimVehicle.Vin %></td>
                        <td style="width: 90px; text-align: center"><%=Model[i].Region %></td>
                        <td style="width: 90px; text-align: center"><%=Model[i].ManheimVehicle.Seller %></td>
                        <td style="width: 60px; text-align: center"><%=Model[i].ManheimVehicle.Mileage.HasValue?Model[i].ManheimVehicle.Mileage.Value.ToString("N0"):"0"%></td>
                        <td style="width: 60px; text-align: center"><%=Model[i].ManheimVehicle.Mmr.ToString("C0") %></td>
                        <td style="width: 40px; text-align: center"><%=Model[i].ManheimVehicle.DateStamp==null?"0":DateTime.Now.Subtract(Model[i].ManheimVehicle.DateStamp.Value).Days.ToString("N0") %></td>
                        <td style="width: 90px; text-align: center"><%=Model[i].ManheimVehicle.SaleDate==null?String.Empty:Model[i].ManheimVehicle.SaleDate.Value.ToShortDateString() %>
                    </tr>
                    <% } %>
                </tbody>
            </table>
            <%= Html.Partial("_PagerPartial") %>
        </div>
    </div>

    <script src="<%=Url.Content("~/js/tablesorter/jquery-latest.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/tablesorter/addons/pager/jquery.tablesorter.pager.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/StockingGuide/ViewListAuctions.js")%>"></script>
    <script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var numberOfTransactions = parseInt("<%= Model.Count %>");
        $(document).ready(function () {
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

            $("table#tblVehicles")
                .tablesorter({
                    sortList: [[11, 1]]
                    , headers: {
                        //0: { sorter: true, sortInitialOrder: 'desc' },
                        8: { sorter: 'price' }, // miles
                        9: { sorter: 'price' }// prices
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
            var make = $("#Make").val();
            var model = $("#Model").val();

            window.location.href = "/PDF/PrintStockingGuideAuctionsReport?make=" + make + "&model=" + model + "&reportType=" + reportType;

            var flag = true;
            var temp = window.setInterval(function () {
                if (flag) {
                    unblockUI();
                    flag = false;
                }
            }, 20 * numberOfTransactions);
        }
    </script>
</body>
</html>
