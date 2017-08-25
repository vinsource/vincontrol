<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.MarketFilterSource>" %>

<!DOCTYPE html>

<html>
    
<head runat="server">
    <title>Stocking Guide Market Search</title>
</head>
<body>
    <div>
        <input type="hidden" id ="hdnModel" value="<%=ViewData["Model"]%>"/>
        <input type="hidden" id ="hdnMake" value="<%=ViewData["Make"]%>"/>
    <%Html.RenderPartial("MarketSearch", Model); %>    
    </div>
    <script src="<%=Url.Content("~/js/VinControl/MarketStockingGuide.js")%>" type="text/javascript"></script>
    
    <%=Html.Partial("_TemplateMarketChart")  %>
</body>
</html>
