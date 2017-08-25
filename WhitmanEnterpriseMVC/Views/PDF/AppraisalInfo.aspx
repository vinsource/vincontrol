<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalInfoPrintViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ViewPage1</title>
    <style type="text/css">
        table#vehicle-list
        {
            font-size: .7em;
        }
        #vehicle-list td
        {
            padding: .3em .7em .3em .7em;
            border-bottom: .1em #bbbbbb solid;
        }
        #printable-list
        {
            display: block;
        }
        #vehicle-list thead tr td
        {
            font-weight: bold;
            color: black;
            border-bottom: #C80000 4px solid;
        }
        .graph-title-bar a
        {
            display: none;
        }
        .padding-right
        {
            text-align: right;
        }
    </style>
</head>
<body>
    <div class="graph-title-bar">
        <h1 style="font-size:40px; text-align:center;">
            Appraisal Report
        </h1>
    </div>
    <div class="graph-title-bar" style=" margin-bottom:30px;">
        <h2 style="text-align:center;">
            Last <%=Model.NoOfDays %> days
        </h2>
    </div>
    <div id="printable-list">
        <div id="vehicle-list" style="font-size: 12pt">
            <table>
                <thead align="left" style="display: table-header-group">
                    <tr>
                        <td style="width: 50px" class="padding-right">
                            Year
                        </td>
                        <td style="width: 100px">
                            Make
                        </td>
                        <td style="width: 180px">
                            Model
                        </td>
                        <td style="width: 100px" class="padding-right">
                            Stock#
                        </td>
                        <td style="width: 240px" class="padding-right">
                            Vin
                        </td>
                        <td style="width: 100px" class="padding-right">
                            Mileage
                        </td>
                        <td style="width: 150px">
                            Color
                        </td>
                        <td style="width: 100px" class="padding-right">
                            Price
                        </td>
                        <td style="width: 80px" class="padding-right">
                            Days
                        </td>
                        <td class="padding-right">
                            Pics
                        </td>
                    </tr>
                </thead>
                <% foreach (var item in Model.CarInfoList)
                   { %>
                <tr>
                    <td class="padding-right">
                        <%= Html.Encode(item.ModelYear) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Make) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Model) %>
                    </td>
                    <td class="padding-right">
                        <%= Html.Encode(item.StockNumber) %>
                    </td>
                    <td class="padding-right">
                        <%= Html.Encode(item.Vin) %>
                    </td>
                    <td class="padding-right">
                        <%= Html.Encode(item.Mileage) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.ExteriorColor) %>
                    </td>
                    <td class="padding-right">
                        <%= Html.Encode(item.SalePrice) %>
                    </td>
                    <td class="padding-right">
                        <%= Html.Encode(item.Days) %>
                    </td>
                    <td class="padding-right">
                        <%= Html.Encode(item.imagesNum) %>
                    </td>
                </tr>
                <% } %>
            </table>
        </div>
    </div>
</body>
</html>
