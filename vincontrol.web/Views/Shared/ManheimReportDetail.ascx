<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.ManheimTransactionViewModel>>" %>

<%if (Model.Any(i => i.Price != "0" && i.Odometer != "0")) {%>
<% foreach (var item in Model.Where(i => i.Price != "0" && i.Odometer != "0")){%>
<tr>
   
    <td><%= item.SaleDate %>
    </td>
    <td>
        <%= item.Auction %>
    </td>
    <td>
        <%= item.Odometer %>
    </td>
    <td>
        <%= item.Price %>
    </td>
   
    <td>
        <%= item.Engine %>
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
<%} else {%><%} %>
