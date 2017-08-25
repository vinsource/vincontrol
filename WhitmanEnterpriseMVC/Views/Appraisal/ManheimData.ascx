<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<WhitmanEnterpriseMVC.Models.ManheimWholesaleViewModel>>" %>

<% if (Model != null && Model.Count > 0)
   {%>
<% foreach (var manheimWholesale in Model)
   {%>
<div id="manheim-row" class="mr-row">
    <div class="range-item label">
        <a id="manheimRow_<%= manheimWholesale.TrimServiceId %>" class="iframe" target="_blank" style="font-size: .7em" title="<%= manheimWholesale.TrimName %>"
            href="<%=Url.Content("~/Report/ManheimTransactionDetail?year=")%><%=manheimWholesale.Year%>&make=<%=manheimWholesale.MakeServiceId%>&model=<%=manheimWholesale.ModelServiceId%>&trim=<%=manheimWholesale.TrimServiceId%>">
            <%= manheimWholesale.TrimName %>
        </a>
    </div>
    <div class="low-wrap range-item low">
        <span class="bb-price">
            <%= manheimWholesale.LowestPrice %></span>
    </div>
    <div class="mid-wrap range-item mid">
        <span class="bb-price">
            <%= manheimWholesale.AveragePrice %></span>
    </div>
    <div class="high-wrap range-item high">
        <span class="bb-price">
            <%= manheimWholesale.HighestPrice %></span>
    </div>
</div>
<%} %>
<%}
   else
   {%>
<div id="manheim-row" class="mr-row">
    There is no Manheim value associated with this vehicle
</div>
<%} %>