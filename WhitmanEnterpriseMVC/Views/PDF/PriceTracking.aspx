<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.PriceChangeViewModel>" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Price Changes Tracking</title>
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
            margin-top: 380px;
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
     <div id="printable-list" style="text-align: center;">
        <div id="vehicle-list" style="font-size: 12pt">
             <% if (Model != null && Model.PriceChangeHistory != null && Model.PriceChangeHistory.Count() > 0)
           {%>
            <table align="center">
                <thead align="left" style="display: table-header-group">
                    <tr>
                        <td style="width: 200px; text-align: left">
                            User
                        </td>
                        <td style="width: 200px; text-align: right">
                            Date
                        </td>
                        <td style="text-align: right">
                            Sale Price
                        </td>
                    </tr>
                </thead>
                <% foreach (var item in Model.PriceChangeHistory)
                   { %>
                <tr>
                    <td style="text-align: left">
                        <%= item.UserStamp%>
                    </td>
                    <td style="text-align: right">
                        <%= item.DateStamp.ToString("MM/dd/yyy hh:mm:ss")%>
                    </td>
                    <td style="text-align: right">
                        <%= item.NewSalePrice.Equals(0) ? "" : item.NewSalePrice.ToString("c0")%>
                    </td>
                </tr>
                <% } %>
            </table>
            <%}
           else
           {%>
        There is no change.
        <%}%>
        </div>
    </div>

 
   
</body>
</html>
