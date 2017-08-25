<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<vincontrol.Application.ViewModels.CommonManagement.ManheimTransactionViewModel>>" %>

<%if (Model.Any(i => i.Price != "0" && i.Odometer != "0")) {%>
<% foreach (var item in Model.Where(i => i.Price != "0" && i.Odometer != "0")){%>
<tr>
   
    <td><%= item.SaleDate %>
    </td>
      <td align="left">
        <%= item.Price %>
    </td>
     <td align="left">
        <%= item.Odometer %>
    </td>
  
     <td>
        <%= item.Vin %>
    </td>
    <td align="left">
        <%= item.Engine %>
    </td>
    <td>
        <%= item.Color %>
    </td>
    <td>
        <%= item.Auction %>
    </td>
       <td>
        <%= item.Region %>
    </td>
</tr>
<% } %>        
<%} else {%><%} %>
