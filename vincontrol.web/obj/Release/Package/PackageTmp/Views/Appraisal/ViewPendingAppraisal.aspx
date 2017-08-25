<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AppraisalListViewModel>" %>

<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Pending Appraisals
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    <div id="inventory_top_btns_holder">
        <% AppraisalsUserRight userRight = SessionHandler.UserRight.Appraisals; %>

        <% if (userRight.Recent == true) %>
        <% { %>
        <div class="admin_top_btns" id="appraisal_recent_tab">
            <a href="<%=Url.Action("ViewAppraisal","Appraisal") %>">Recent</a>
        </div>
        <% } %>

        <% if (userRight.Pending == true) %>
        <% { %>
        <div class="admin_top_btns hasNum admin_top_btns_active active_padding" id="appraisal_pending_tab">
            <a href="<%=Url.Action("ViewPendingAppraisal","Appraisal") %>">Pending</a>
            <div class="appraisals_number_below">
                0
            </div>
        </div>
        <% } %>

        <% if (userRight.Advisor == true) %>
        <% { %>
        <div class="admin_top_btns" id="appraisal_tradein_tab">
            <a href="<%=Url.Content("~/Tradein/Report")%>" style="font-size: 0px;">
                <img src="/content/images/VINAdvisor-Short.png" />
            </a>
        </div>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="appraisal_fixed_title">
                Appraisals List
            </div>
        </div>
    </div>
    <div>
        <div id="right_content_nav">
            <div class="right_content_nav_items nav_middle"></div>
            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_vin_expended right_content_items_vin nav_long">
                    VIN
                    <img class="imgSort" id="imgSort_1" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_middle" id="appraisalsort_3">
                    Year
                    <img class="imgSort" id="imgSort_3" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_long" id="appraisalsort_4">
                    Make
                    <img class="imgSort" id="imgSort_4" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_long" id="appraisalsort_5">
                    Model
                    <img class="imgSort" id="imgSort_5" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_long" id="appraisalsort_6">
                    Trim
                    <img class="imgSort" id="imgSort_6" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_middle" id="appraisalsort_7">
                    Color
                    <img class="imgSort" id="imgSort_7" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <a href="javascript:void(0);">
                <div class="right_content_nav_items right_content_items_owners nav_short" id="appraisalsort_8" style="width: 71px!important;">
                    Owners
                    <img class="imgSort" id="imgSort_8" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <div class="right_content_nav_items right_content_items_client">
                Client
            </div>

            <div class="right_content_nav_items right_content_items_appraiser">
                Appraiser
            </div>


            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_middle_MilesAppraisal" id="appraisalsort_11">
                    Odometer
                    <img class="imgSort" id="imgSort_11" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>

            <a href="javascript:void(0);">
                <div class="right_content_nav_items nav_long" id="appraisalsort_12">
                    ACV
                    <img class="imgSort" id="imgSort_12" src="../Content/images/vincontrol/dot.png" border="0" width="12px" />
                </div>
            </a>
            <div class="right_content_nav_filter_item" onclick="toggleFilterPanel();" style="position: absolute; right: 0px; font-size: 12px; padding: 2px 0px;">
                Filter
                <span style="display: inline-block; float: none;" class="right_content_nav_filter_item_arrow_down"></span>
            </div>
        </div>
        <div id="filterPanel" class="filterPanel">
            <div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">Year</div>
                    <select id="selectYear"></select>
                </div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">Make</div>
                    <select id="selectMake"></select>
                </div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">Model</div>
                    <select id="selectModel"></select>
                </div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">Trim</div>
                    <select id="selectTrim"></select>
                </div>
            </div>
            <div>
                <div class="filter_item_holder" style="display: none">
                    <div class="filter_item_label">Price</div>
                    <select id="selectPrice"></select>
                </div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">Miles</div>
                    <input type="text" id="mileFrom" onkeypress="validateInput(event)" onkeyup="formatMile(event, this)" />
                </div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">to</div>
                    <input type="text" id="mileTo" onkeypress="validateInput(event)" onkeyup="formatMile(event, this)" />
                </div>
                <div class="filter_item_holder" style="float: right;">
                    <input class="submitFilter" id="submitFilter" type="button" value="Submit" />
                </div>
            </div>
        </div>
        <div id="divNoResult" style="display: none; text-align: center; padding-top: 10px;">
            There are no results which match your filter criteria.
        </div>
        <div id="recentAppraisals">
            <div class="data-content" align="center">
                <img src="/content/images/ajaxloadingindicator.gif" />
            </div>
        </div>
    </div>
    <input type="hidden" id="InventoryStatusCodeId" name="InventoryStatusCodeId" />
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript" src="/js/VinControl/appraisal.js"></script>
    <script type="text/javascript">
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';
        var isUp = false;
        var idValue = 0;
        var pageIndex = 1;
        var pageSize = 50;
        var scrollHeight = 400;
        var lastCondition = 0;

        function ChangeStatus(vin, appraisalID, inventoryStatus) {
            if (inventoryStatus == 1) {
                $.post('<%= Url.Content("~/Appraisal/CheckAppaisalInSoldOutInventory") %>', { vin: vin, appraisalId: appraisalID }, function (data) {
                    if (data.sussess == true) {
                        if (data.isExist == true) {
                            window.parent.showPopUpViewDetail(data.url, 'Sold+Out');
                        } else {
                            window.parent.openMarkSoldAppraisalIframe(appraisalID);
                        }
                    }
                });
            } else {
                $.post('<%= Url.Content("~/Appraisal/CheckAppaisalInInventory") %>', { vin: vin, appraisalId: appraisalID }, function (data) {
                    if (data.sussess == true) {
                        if (data.isExist == true) {
                            window.parent.showPopUpViewDetail(data.url, 'Inventory');
                        } else {
                            window.parent.showPopUpStock(inventoryStatus, appraisalID);
                        }
                    }
                });
            }
        }

        function UnCheckStatus(className) {
            $('.' + className).each(function () {
                $(this).attr('checked', false);
            });
        }

        function SubmitAddInventory(appraisalID) {
            if ($('#InventoryStatusCodeId').val() == 1) {

            }
            else if ($('#InventoryStatusCodeId').val() == 3) {
                $.post('<%= Url.Content("~/Appraisal/AddToWholeSaleNew") %>', { appraisalID: appraisalID, stock: '' }, function (data) {
                    var listingid = data.listingid;
                    var actionUrl = '<%= Url.Action("ViewIProfile", "Inventory",new { ListingID = "PLACEHOLDER" } ) %>';
                    actionUrl = actionUrl.replace('PLACEHOLDER', listingid);
                    window.location.href = actionUrl;

                });
            }
            else {
                $.post('<%= Url.Content("~/Appraisal/AddToInventoryNew") %>', { appraisalID: appraisalID, stock: '', inventoryStatusCodeId: $('#InventoryStatusCodeId').val() }, function (data) {
                    var listingid = data.listingid;
                    var actionUrl = '<%= Url.Action("ViewIProfile", "Inventory",new { ListingID = "PLACEHOLDER" } ) %>';
                    actionUrl = actionUrl.replace('PLACEHOLDER', listingid);
                    window.location.href = actionUrl;
                });
            }
    }

    function SubmitStockNumber(stock, appraisalID) {
        if ($('#InventoryStatusCodeId').val() == 1) {

        }
        else if ($('#InventoryStatusCodeId').val() == 3) {
            $.post('<%= Url.Content("~/Appraisal/AddToWholeSaleNew") %>', { appraisalID: appraisalID, stock: stock }, function (data) {
                var listingid = data.listingid;
                var actionUrl = '<%= Url.Action("ViewIProfile", "Inventory",new { ListingID = "PLACEHOLDER" } ) %>';
                actionUrl = actionUrl.replace('PLACEHOLDER', listingid);
                window.location.href = actionUrl;
            });
        }
        else {
            $.post('<%= Url.Content("~/Appraisal/AddToInventoryNew") %>', { appraisalID: appraisalID, stock: stock, inventoryStatusCodeId: $('#InventoryStatusCodeId').val() }, function (data) {
                var listingid = data.listingid;
                var actionUrl = '<%= Url.Action("ViewIProfile", "Inventory",new { ListingID = "PLACEHOLDER" } ) %>';
                actionUrl = actionUrl.replace('PLACEHOLDER', listingid);
                window.location.href = actionUrl;
            });
        }
}

function showPopUpStock(inventoryStatus, appraisalID) {
    $('#InventoryStatusCodeId').val(inventoryStatus);
    var markSoldUrl = '/Appraisal/PopUpStockList?appraisalID=' + appraisalID;
    $("<a href=" + markSoldUrl + "></a>").fancybox({
        height: 115,
        width: 455,
        padding: 0,
        margin: 0,
        overlayShow: true,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe',
        onClosed: function () {
            UnCheckStatus('chk');
        }
    }).click();
}

function showPopUpViewDetail(url, message) {
    var popUpUrl = '/Appraisal/PopUpViewDetail/?url=' + url + '&message=' + message;
    $("<a href=" + popUpUrl + "></a>").fancybox({
        height: 150,
        width: 455,
        padding: 0,
        margin: 0,
        overlayShow: true,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe',
        onClosed: function () {
            UnCheckStatus('chk');
        }
    }).click();
}

function openMarkSoldAppraisalIframe(appraisalId) {
    var markSoldUrl = '/Appraisal/ViewMarkSold?appraisalID=' + appraisalId;
    $("<a href=" + markSoldUrl + "></a>").fancybox({
        height: 306,
        width: 455,
        overlayShow: true,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe',
        onClosed: function () {
            UnCheckStatus('chk');
        }
    }).click();
}

function resetScrollValues() {
    pageIndex = 1;
    pageSize = 50;
    scrollHeight = 400;
}

//$(".sForm").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });

$("a.iframe").fancybox({ 'width': 1000, 'height': 700 });

var tempACV = 0;
$('input[name=Acv]').live("focus", function () {
    tempACV = $(this).val();
}).live("focusout", function () {
    var tempCurrentACVValue = $(this).val();
    if (tempCurrentACVValue != tempACV) {
        if ($(this).val() == "" || parseInt($(this).val().replace(/,/g, "")) <= 100000000) {
            var img = $(this).parent().find('.imgLoadingPendingAppraisal');
            img.show();

            $.post('<%= Url.Content("~/Appraisal/UpdateACV") %>', { AppraisalId: $(this).attr('id'), ACV: $(this).val() }, function (data) {
                img.hide();
            });
        }
    }
});

var tempMiles = 0;
$('input[name=odometer]').live("focus", function () {
    tempMiles = $(this).val();
}).live("focusout", function () {
    if ($(this).val() == "") {
        $(this).val(0);
    }

    var tempCurrentMileValue = $(this).val();
    if (tempCurrentMileValue != tempMiles) {
        if (parseInt($(this).val().replace(/,/g, "")) <= 2000000) {
            var img = $(this).parent().find('.imgLoadingPendingAppraisal');
            img.show();

            $.post('<%= Url.Content("~/Appraisal/UpdateMileage") %>', { AppraisalId: $(this).attr('id'), mileage: $(this).val() }, function (data) {
                    img.hide();
                });
            }
        }
    });

    $(document).ready(function () {

    });

    function setHeaderVisibility() {

        if ($("#recentAppraisals div div").length == 0) {
            $("#right_content_nav").hide();
            $("#divNoResult").show();
        }
        else {
            $("#right_content_nav").show();
            $("#divNoResult").hide();
        }
    }
    var appraisalStatus = 1;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/appraisals.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .nav_middle
        {
            width: 55px!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
</asp:Content>
