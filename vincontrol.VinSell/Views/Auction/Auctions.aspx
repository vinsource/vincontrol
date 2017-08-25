<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.ManheimAuctionManagement.RegionActionSummarizeViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title>VinSell | Auction</title>
    <link href="<%=Url.Content("~/Scripts/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%=Url.Content("~/Content/cupertino/jquery-ui-1.8.14.custom.css")%>" rel="stylesheet" type="text/css" media="screen" />
    <link href="<%=Url.Content("~/Content/style.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .popup .scrollableContainer { background: White; }
        .scrollableContainer { background: White; }
        .popup div.scrollingArea { height: 200px; }
        .auction { width: 343px; padding-right: 14px; }
    </style>
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.alerts.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.easing-1.3.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/fancybox/jquery.mousewheel-3.0.4.pack.js")%>"
        type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/tablesorter/jquery.tablesorter.js")%>" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $(document).ajaxError(function (event, request, settings, error) {
                //alert(request.status + " " + request.statusText);
            });

            $("a[id^='viewAuctions_']").live('click', function () {
                
                window.parent.document.getElementById('hdnState').value = this.id.split("_")[2];
                window.parent.document.getElementById('hdnRegionCode').value = this.id.split("_")[1];
                window.parent.document.getElementById('hdnRegionName').value = this.innerText;
                parent.$.fancybox.close();
            });
      

        });

    </script>
</head>
<body>
    <div class="popup hidden" style="display:block;position:inherit;margin:0;height:250px;background:White;">
        <div class="auctions">
            <div class="scrollableContainer">
                <div class="scrollingArea">
                    <table class="auction-list" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="state">
                                    <%= Model.Name %>
                                </th>
                                <th class="num-auctions">
                                    <%= Model.Regions.Count() == 1 ? "1 Auction" : Model.Regions.Count() + " Auctions" %>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var region in Model.Regions) {%>
                            <tr>
                                <td class="auction">
                                    <a id="viewAuctions_<%= region.Code %>_<%= Model.Name %>" href="javascript:;"><%= region.Name %></a>
                                </td>
                                <td class="num-vehicles">
                                    <%= region.NumberofVehicles == 1 ? "(1 vehicle)" : "(" + region.NumberofVehicles + " vehicles)" %>
                                </td>
                            </tr>
                            <%}%>                            
                            
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
