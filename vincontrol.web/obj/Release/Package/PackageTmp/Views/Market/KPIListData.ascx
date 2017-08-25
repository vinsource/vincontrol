<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.InventoryFormViewModel>" %>
<style type="text/css">
    .disable
    {
        display: none;
    }
</style>
<script type="text/javascript">

          $(document).ready(function () {

            $("#inventory_used_tab_number").html(<%=Model.SubSetList.Count() %>);
        });
</script>
<% var index = 0; %>
<% foreach (var tmp in Model.SubSetList)
   {
       if (index % 2 == 0)
       {
%>
<div class="kpi_list_items">
    <% }
       else
       { %>
    <div class="kpi_list_items kpi_list_items_odd">
        <% } %>
        <a href="<%= Url.Action("ViewIProfile", "Inventory", new {ListingID = tmp.ListingId}) %>">
            <div class="kpi_list_collum kpi_list_img">
                <img src="<%= tmp.SinglePhoto %>" width="65px;" height="45px;">
            </div>
        </a>
        <div class="kpi_list_collum">
            <a href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
                <% if (tmp.IsUsed && tmp.Type < 4) %>
                <%
                   { %>
                <% if (tmp.MarketRange == 3) %>
                <%
                   { %>
                <div class="age_text orange_color">
                    <%= tmp.DaysInInvenotry %>
                </div>
                <% } %>
                <%
                   else if (tmp.MarketRange == 2)
                   { %>
                <div class="age_text green_color">
                    <%= tmp.DaysInInvenotry %>
                </div>
                <% } %>
                <%
                   else if (tmp.MarketRange == 1)
                   { %>
                <div class="age_text blue_color">
                    <%= tmp.DaysInInvenotry %>
                </div>
                <% } %>
                <%
                   else
                   { %>
                <div class="age_text black_color">
                    <%= tmp.DaysInInvenotry %>
                </div>
                <% } %>
                <% } %>
                <%
                   else
                   { %>
                <div class="age_text_border border_color">
                    <%= tmp.DaysInInvenotry %>
                </div>
                <% } %>
            </a>
        </div>
        <div class="kpi_list_collum kpi_list_marketData">
            <% =tmp.CarRanking + "/" + tmp.NumberOfCar%>
        </div>
        <a href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
            <div class="kpi_list_collum kpi_list_vin">
                <%=tmp.Vin.Length < 8 ? tmp.Vin : tmp.Vin.Substring(tmp.Vin.Length - 8, 8)%>
            </div>
        </a><a href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
            <div class="kpi_list_collum">
                <%=tmp.Stock %>
            </div>
        </a><a href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
            <div class="kpi_list_collum">
                <%=tmp.ModelYear %>
            </div>
        </a><a href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
            <div class="kpi_list_collum_medium" title="Lexus">
                <%=tmp.Make %>
            </div>
        </a><a href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
            <div class="kpi_list_collum_long" title="IS 250">
                <%=tmp.Model %>
            </div>
        </a><a href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
            <div class="kpi_list_collum_medium" title="">
                <%=tmp.Trim %>
            </div>
        </a>
        <div class="kpi_list_collum_long" title="Nebula">
            <%=tmp.ExteriorColor %>
        </div>
        <div class="kpi_list_collum kpi_list_miles">
            <% =tmp.Mileage.ToString("#0,0")%>
        </div>
        <div class="kpi_list_collum kpi_list_price">
            <% =tmp.SalePrice.ToString("#0,0")%>
        </div>
        <a class="vin_viewprofile" href="<%=Url.Action("ViewIProfile","Inventory",new {ListingID=tmp.ListingId}) %>">
            <div class="right_content_items">
                <% if (tmp.IsUsed && tmp.Type < 4) %>
                <%
                   { %>
                    <% if (tmp.MarketRange == 3) %>
                    <%
                       { %>
                    <img src="/Content/images/market_higher.gif">
                    <% } %>
                    <%
                       else if (tmp.MarketRange == 2)
                       { %>
                    <img src="/Content/images/market_equal.gif">
                    <% } %>
                    <%
                       else if (tmp.MarketRange == 1)
                       { %>
                    <img src="/Content/images/market_lower.gif">
                    <% } %>
                    <%
                       else
                       { %>
                    <img src="/Content/images/market_question.gif">
                    <% } %>
                <% } %>
            </div>
        </a>
        <div class="kpi_list_collum kpi_list_carfaxowner">
            <%=tmp.CarFaxOwner %>
        </div>
    </div>
    <%
                   index++;
   } %>
