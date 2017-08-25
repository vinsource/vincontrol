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
                Cancel</div>
            <div class="btns_shadow" id="status_transfer" onclick="Transfer();">
                Transfer</div>
        </div>
    </div>
    <input type="hidden" id="hdListingID" value="<%=Model.ListingID %>" />
    <input type="hidden" id="hdCurrentStatusID" value="<%=Model.CurrentStatusID %>" />
    <% Html.EndForm(); %>
    <div class="loFancybox" style="display: none">
        <div id="loPopupExistInInventory" style="width: 450px;">
            <div id="loExistInInventoryTitle" style="color: white;background-color: #36C;font-size: 21px;font-weight: bold;margin-bottom: 10px;padding: 5px 10px;text-align: left !important;">Car already exists in Inventory</div>
            <div id="loExistInInventoryContent" style="padding: 0px 5px;text-align: center;font-size: 17px;">
                This car already exists in Inventory. Would you like to view the profile page OR continue to Mark Sold ?
            </div>
            <div id="loExistInInventoryBtn">
                <div class="loExistButton btns_shadow" id="loMarkSole">Mark Sold</div>
                <div class="loExistButton btns_shadow" id="loViewInventory">View in Inventory</div>
            </div>
        </div>
    </div>
    
    <script src="<%=Url.Content("~/js/jquery-1.7.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Js/fancybox/jquery.fancybox-1.3.4.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script type="text/javascript">

        function Cancel() {
            parent.$.fancybox.close();
        }

        function Transfer() {
            var listingId = $('#hdListingID').val();
            var inventoryUrl = "<%=Url.Action("ViewIProfile", "Inventory", new {ListingId = Model.ListingID})%>";
            var inventoryStatus = $("#DDLStatus option:selected").val();
            var currentStatus = $('#hdCurrentStatusID').val();
            var appraisalID = <%=Model.AppraisalID %>;
        
        if(inventoryStatus==1) {
            $.post('<%= Url.Content("~/Appraisal/CheckAppaisalInSoldOutInventory") %>', { vin: '<%=Model.Vin %>', appraisalId: appraisalID }, function(data) {
                if (data.sussess == true) {
                    if (data.isExist == true) {
                        window.parent.showPopUpViewDetail(data.url,'Sold+Out');
                    } else {
                        $.post('<%= Url.Content("~/Appraisal/CheckAppaisalInInventory") %>', { vin: '<%=Model.Vin %>', appraisalId: appraisalID }, function(data) {
                
                            if (data.sussess == true) {
                                if (data.isExist == true) {
                                       
                                    $.fancybox({
                                        "href": "#loPopupExistInInventory",
                                        width : 300,
                                        height : 500,
                                        padding:0,
                                        margin : 0,
                                        wrapCSS : "leoCustom",
                                        onStart : function() {
                                            $("#fancybox-wrap").addClass("leoCustom");        
                                        },
                                    });

                                    $("#loMarkSole").off("click");
                                    $("#loMarkSole").click(function() {
                                        window.parent.openMarkSoldAppraisalIframe(appraisalID);
                                    });

                                    $("#loViewInventory").off("click");
                                    $("#loViewInventory").click(function() {
                                        window.parent.location = inventoryUrl;
                                    });
                                    // window.parent.showPopUpViewDetail(data.url,'Inventory');
                                } else {
                                    window.parent.openMarkSoldAppraisalIframe(appraisalID);
                                }
                            }
                        });
                        
                    }
                }
            });
        } else {
            $.post('<%= Url.Content("~/Appraisal/CheckAppaisalInInventory") %>', { vin: '<%=Model.Vin %>', appraisalId: appraisalID }, function(data) {
                
                if (data.sussess == true) {
                    if (data.isExist == true) {
                        window.parent.showPopUpViewDetail(data.url,'Inventory');
                    } else {
                        window.parent.showPopUpStock(inventoryStatus);
                    }
                }
            });
        }
    }

    function updateStatus(currentStatus,inventoryStatus,listingId) {
        var updateStatusFromInventoryPageUrl = '<%= Url.Content("~/Inventory/UpdateStatusChange") %>';
        var urlInventory = '<%= Url.Content("~/Inventory/ViewInventory") %>';
        if(inventoryStatus ==0) {
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

