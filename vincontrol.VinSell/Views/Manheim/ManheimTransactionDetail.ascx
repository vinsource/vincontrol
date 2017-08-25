<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.ManheimTransactionViewModel>>" %>

<%if (Model.Count > 0) {%>
<table id="manheimTransaction" width="100%" class="reportText">
    <thead style="cursor: pointer;">
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
    </thead>
    <tbody>
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
    </tbody>
</table>        
<%} %>
