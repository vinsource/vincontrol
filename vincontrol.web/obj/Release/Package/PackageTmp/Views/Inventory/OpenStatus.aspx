<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.StatusInfoModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mark As Sold</title>
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/VinControl/popup.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />
    
</head>
<body>
    <% Html.BeginForm("MarkSold", "Inventory", FormMethod.Post, new { id = "markSoldForm", name = "markSoldForm" }); %>
    <div class="kpi_print_title">
        Status Change
    </div>
    <div class="kpi_print_options" id="status_popup_holder">
        <label class="status_vh">
            <%=Model.Title %></label>
        <label class="status_current">
            Current Status:
            <nobr><%=Model.CurrentStatus %></nobr>
        </label>
        <label class="status_change">
            Change Status to:
        </label>
        <label class="status_change_select">
            <select id="DDLStatus">
                <% foreach (var item in Model.ListStatus)
                   {%>
                <option value="<%=item.Value %>">
                    <%=item.Text %></option>
                <% } %>
            </select>
        </label>
        <div class="status_btn_holder">
            <div class="btns_shadow" id="status_cancel" onclick="Cancel();">
                Cancel
            </div>
            <div class="btns_shadow" id="status_transfer" onclick="Transfer();">
                Transfer
            </div>
        </div>
    </div>
    <input type="hidden" id="hdListingID" value="<%=Model.ListingID %>" />
    <input type="hidden" id="hdCurrentStatusID" value="<%=Model.CurrentStatusID %>" />
    <% Html.EndForm(); %>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type="text/javascript">

        function Cancel() {
            parent.$.fancybox.close();
        }

        function Transfer() {
            var listingId = $('#hdListingID').val();
            var inventoryStatus = $("#DDLStatus option:selected").val();
            var currentStatus = $('#hdCurrentStatusID').val();


            updateStatus(currentStatus, inventoryStatus, listingId);
        }

        function updateStatus(currentStatus, inventoryStatus, listingId) {
            var updateStatusFromInventoryPageUrl = '<%= Url.Content("~/Inventory/UpdateStatusChange") %>';
            var urlInventory = '<%= Url.Content("~/Inventory/ViewInventory") %>';
            if (inventoryStatus == 0) {
                alert('please choose status');
            } else {
                if (inventoryStatus != 1) {
                    $.post(updateStatusFromInventoryPageUrl, { currentStatus: currentStatus, status: inventoryStatus, ListingId: listingId }, function (data) {
                        parent.$.fancybox.close();
                        window.parent.location = urlInventory;
                    });
                } else {
                    window.parent.openMarkSoldIframe(listingId);
                }
            }
        }
    </script>
</body>
</html>

