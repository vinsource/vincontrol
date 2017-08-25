<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<WhitmanEnterpriseMVC.Models.ReportSummaryViewModel>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ReportSummary</title>
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
            text-align: center;
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
        <h1 style="font-size: 40px; text-align: center;">
            Appraisal Report - Summary
        </h1>
    </div>
    <div id="printable-list" style="text-align: center;">
        <div id="vehicle-list" style="font-size: 12pt">
            <table align="center">
                <thead align="left" style="display: table-header-group">
                    <tr>
                        <td style="width: 200px; text-align: left">
                            Name
                        </td>
                        <td style="text-align: right">
                            Number Of Appraisals
                        </td>
                        <td style="text-align: right">
                            Number Of Items In Inventory
                        </td>
                    </tr>
                </thead>
                <% foreach (var item in Model)
                   { %>
                <tr>
                    <td style="text-align: left">
                        <%= Html.Encode(item.Name) %>
                    </td>
                    <td style="text-align: right">
                        <%= Html.Encode(item.NumOfAppraisal) %>
                    </td>
                    <td style="text-align: right">
                        <%= Html.Encode(item.NumOfInventory) %>
                    </td>
                </tr>
                <% } %>
            </table>
        </div>
    </div>
</body>
</html>
