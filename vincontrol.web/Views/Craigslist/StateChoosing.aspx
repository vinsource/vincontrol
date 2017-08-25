<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<VINControl.Craigslist.StateChoosing>>" %>

<!DOCTYPE html>

<html>
<head>
    <title>Craigslist</title>
    <script src="~/js/jquery-1.7.2.min.js"></script>
    <script src="~/js/fancybox/jquery.fancybox-1.3.4.js" type="text/javascript"></script>
    <script src="~/js/fancybox/jquery.easing-1.3.pack.js" type="text/javascript"></script>
    <script src="~/js/fancybox/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="~/js/jquery.blockUI.js" type="text/javascript"></script>
</head>
<body>
    <input type="hidden" id="listingId" value="<%= ((int)ViewData["LISTINGID"])%>"/>
    <div id="content">
        <% foreach (var state in Model)
        {%>
            <div style="float: left; padding: 5px 10px;">
                <ul style="padding: 0;"><%=state.Name %></ul>
                <%foreach (var location in state.Locations)
                {%>
                <li><a href="<%=location.Value %>"><%=location.Name%></a></li>
                <%}%>
            </div>
        <%}%>
    </div>
</body>
</html>
