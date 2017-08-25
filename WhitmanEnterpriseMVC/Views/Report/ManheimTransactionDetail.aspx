<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<WhitmanEnterpriseMVC.Models.ManheimTransactionViewModel>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Manheim Transactions</title>
</head>
<body>
    <div>
        <br />        
        <strong class="reportHeader">REPORTED WHOLESALE AUCTION SALES - With Exact Matches</strong>
        <hr />
        <%if (Model.Count > 0) {%>
        <table width="100%" class="reportText">
            <tr>
                <th align="left" width="150px">
                    Type
                </th>
                <th align="right" width="100px">
                    Odometer
                </th>
                <th align="right" width="100px">
                    Price
                </th>
                <th align="center" width="100px">
                    Sale date
                </th>
                <th align="left" width="150px">
                    Auction
                </th>
                <th align="left" width="80px">
                    Engine
                </th>
                <th align="left" width="80px">
                    T<br />
                    R
                </th>
                <th align="left" width="80px">
                    Cond
                </th>
                <th align="left" width="150px">
                    Color
                </th>
                <th align="left" width="50px">
                    Sample
                </th>
            </tr>
            <% foreach (var item in Model){%>
            <tr>
                <td>
                    <%= item.Type %>
                </td>
                <td align="right">
                    <%= item.Odometer %>
                </td>
                <td align="right">
                    <%= item.Price %>
                </td>
                <td>
                    <center>
                        <%= item.SaleDate %></center>
                </td>
                <td>
                    <%= item.Auction %>
                </td>
                <td>
                    <%= item.Engine %>
                </td>
                <td>
                    <%= item.TR %>
                </td>
                <td>
                    <%= item.Cond %>
                </td>
                <td>
                    <%= item.Color %>
                </td>
                <td>
                    <%= item.Sample %>
                </td>
            </tr>
            <% } %>
        </table>        
        <%} %>
    </div>
</body>
</html>
