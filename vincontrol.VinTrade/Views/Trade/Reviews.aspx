<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<vincontrol.Application.ViewModels.CommonManagement.CarInfoViewModel>>" %>
<%@ Import Namespace="vincontrol.VinTrade.Handlers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Reviews</title>
</head>
<body>
    <div>
    <h3>Big Thanks!</h3>
    <div><%= SessionHandler.TradeInDealer.DealershipName %> would like to congratulate the following customers and thank them for their business. Thank you for your patronage!</div>
    <% if (Model.Any()) {%>
    <% foreach (var review in Model.Where(i => i.CustomerFirstName != string.Empty && i.CustomerLastName != string.Empty)){%>
    <div><%= SessionHandler.TradeInDealer.DealershipName %> would like to congratulate <b><%= review.CustomerFirstName %> <%= review.CustomerLastName %></b>. from <%= review.CustomerCity %>, <%= review.CustomerState %> on the purchase of a <%= review.ModelYear %> <%= review.Make %> <%= review.Model %> <%= review.Trim %></div>
    <%}%>
    <%}%>
    </div>
</body>
</html>
