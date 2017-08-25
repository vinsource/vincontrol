<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.ReportDetailedViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ReportDetail</title>
    <style type="text/css">
        table.vehicle-list
        {
            font-size: .7em;
        }
        .vehicle-list td
        {
            padding: .3em .7em .3em .7em;
            border-bottom: .1em #bbbbbb solid;
        }
        .printable-list
        {
            display: block;
        }
        .vehicle-list thead tr td
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
    <div>
        <div class="graph-title-bar">
            <h1 style="font-size: 40px; text-align: center;">
                Appraisal
            </h1>
        </div>
        <div class="printable-list" style="text-align: center;">
            <div class="vehicle-list" style="font-size: 12pt;">
                <table align="center">
                    <thead align="left" style="display: table-header-group">
                        <tr>
                            <td style="width: 200px; text-align: left">
                                Name
                            </td>
                            <td style="text-align: right">
                                Year
                            </td>
                            <td style="text-align: left">
                                Make
                            </td>
                            <td style="text-align: left">
                                Model
                            </td>
                            <td style="text-align: right">
                                Date
                            </td>
                        </tr>
                    </thead>
                    <% foreach (var item in Model.Appraisals)
                       { %>
                    <tr>
                        <td style="text-align: left">
                            <%= Html.Encode(item.Name) %>
                        </td>
                        <td style="text-align: right">
                            <%= Html.Encode(item.Year) %>
                        </td>
                        <td style="text-align: left">
                            <%= Html.Encode(item.Make) %>
                        </td>
                        <td style="text-align: left">
                            <%= Html.Encode(item.Model) %>
                        </td>
                        <td style="text-align: right">
                            <%= Html.Encode(String.Format("{0:g}", item.Date)) %>
                        </td>
                    </tr>
                    <% } %>
                </table>
            </div>
        </div>
        <div class="graph-title-bar">
            <h1 style="font-size: 40px; text-align: center;">
                Details
            </h1>
        </div>
        <div class="printable-list" style="text-align: center;">
            <div class="vehicle-list" style="font-size: 12pt;">
                <table align="center">
                    <thead align="left" style="display: table-header-group">
                        <tr>
                            <td style="width: 200px;text-align: left">
                                Name
                            </td>
                            <td style="text-align: right">
                                Year
                            </td>
                            <td style="text-align: left">
                                Make
                            </td>
                            <td style="text-align: left">
                                Model
                            </td>
                        </tr>
                    </thead>
                    <% foreach (var item in Model.Inventories)
                       { %>
                    <tr>
                        <td style="text-align: left">
                            <%= Html.Encode(item.Name) %>
                        </td>
                        <td style="text-align: right">
                            <%= Html.Encode(item.Year) %>
                        </td>
                        <td style="text-align: left">
                            <%= Html.Encode(item.Make) %>
                        </td>
                        <td style="text-align: left">
                            <%= Html.Encode(item.Model) %>
                        </td>
                    </tr>
                    <% } %>
                </table>
            </div>
        </div>
    </div>
</body>
</html>
