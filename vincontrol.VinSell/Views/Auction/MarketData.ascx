<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<vincontrol.DomainObject.ChartGraph>" %>
<% if (Model == null || Model.Market == null) {%> 
<div class="subheader-nav"></div>
<div class="data-content">No data</div>
<%} else {%>
<div class="subheader-nav">
    <div class="subnav-text" style="background-color:#6770f0;color:White;"><%= Model.Market.CarsOnMarket == 0 ? "" : Model.Market.CarsOnMarket + " Cars on market in nation" %> 
    </div>
</div>
<div class="data-content">
    <ul>
        <li><span class="label">Highest on Market</span><img src='<%= Url.Content("~/Content/Images/market-high.jpg") %>'>
            <%= Model.Market.MaximumPrice %></li>
        <li><span class="label">Average on Market</span><img src='<%= Url.Content("~/Content/Images/market-mid.jpg") %>'>
            <%= Model.Market.AveragePrice %></li>
        <li><span class="label">Lowest on Market</span><img src='<%= Url.Content("~/Content/Images/market-low.jpg") %>'>
            <%= Model.Market.MinimumPrice %></li>
    </ul>
</div>
<%}%>

