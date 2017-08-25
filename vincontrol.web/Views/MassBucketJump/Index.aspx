<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<%@ Import Namespace="vincontrol.Constant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mass Bucket Jump
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="recent">
        <div id="container_right_btn_holder">
            <div id="container_right_btns">
                <div class="inventory_fixed_title" id="divTitle">
                    Bucket Jump
                </div>
            </div>
        </div>
        <div id="right_content_nav" class="right_content_nav">
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
                <div class="filter_item_holder">
                    <div class="filter_item_label">Price</div>
                    <select id="selectPrice"></select>
                </div>
                   <div class="filter_item_holder">
                    <div class="filter_item_label">Store</div>
                    <select id="selectStore"></select>
                </div>
                <%--<div class="filter_item_holder">
                    <div class="filter_item_label">Miles</div>
                    <input type="text" id="mileFrom" onkeypress="validateInput(event)" onkeyup="formatMile(event, this)" />
                </div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">to</div>
                    <input type="text" id="mileTo" onkeypress="validateInput(event)" onkeyup="formatMile(event, this)" />
                </div>--%>
                <div class="filter_item_holder" style="float: right">
                    <input class="submitFilter" id="submitFilter" type="button" value="Submit" />
                </div>
            </div>
        </div>
        <div id="DivContent">
            <div id="divHasContent" class="vin_listVehicle_holder">
            </div>
            <div id="divNoContent" style="display: none; padding-left: 400px; margin-top: 10px;">There are no vehicles.</div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SubMenu" runat="server">
    <div id="inventory_top_btns_holder">
        <div id="bucketjump_landrover_tab" class="admin_top_btns" onclick="grid.changeView('<%=BucketJumpView.LandRover.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below number_belowActive" id="bucketjump_landrover_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=BucketJumpView.LandRover.ToString()  %>');grid.setActive(this);">Land Rover</a>
        </div>
        <div id="bucketjump_jaguar_tab" class="admin_top_btns" onclick="grid.changeView('<%=BucketJumpView.Jaguar.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="bucketjump_jaguar_tab_number">
                Loading..
            </div>
            <a  onclick="grid.changeView('<%=BucketJumpView.Jaguar.ToString()  %>');grid.setChildActive(this);">Jaguar</a>
        </div>
        <div id="bucketjump_al_tab" class="admin_top_btns" onclick="grid.changeView('<%=BucketJumpView.AL.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="bucketjump_al_tab_number">
                Loading..
            </div>
            <a  onclick="grid.changeView('<%=BucketJumpView.AL.ToString()  %>');grid.setChildActive(this);">Off Makes A-L</a>
        </div>

        <div id="bucketjump_mz_tab" class="admin_top_btns" onclick="grid.changeView('<%=BucketJumpView.MZ.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="bucketjump_mz_tab_number">
                Loading..
            </div>
            <a  onclick="grid.changeView('<%=BucketJumpView.MZ.ToString()  %>');grid.setChildActive(this);">Off Makes M-Z</a>
        </div>

         <div id="bucketjump_today" class="admin_top_btns" onclick="grid.changeView('<%=BucketJumpView.GroupTodayBucketJump.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="bucketjump_today_tab_number">
                Loading..
            </div>
            <a  onclick="grid.changeView('<%=BucketJumpView.GroupTodayBucketJump.ToString()  %>');grid.setChildActive(this);">Today Bucket Jump</a>
        </div>
        
        <div id="bucketjump_saved_tab" class="admin_top_btns" onclick="grid.changeView('<%=BucketJumpView.Saved.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="bucketjump_saved_tab_number">
                Loading..
            </div>
            <a  onclick="grid.changeView('<%=BucketJumpView.Saved.ToString()  %>');grid.setChildActive(this);">Past Bucket Jump</a>
        </div>
        <div id="bucketjump_report_tab" class="admin_top_btns" style="padding: 2px 8px;">
            <a class="iframeReport" href="javascript:;" style="color: black">
                Report
            </a>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ClientScripts" runat="server">
    <%--<script src="<%=Url.Content("~/js/Utility.js")%>" type="text/javascript"></script>--%>
    <script src="<%=Url.Content("~/js/VinControl/bucketjump.js")%>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        
        var landRoverTab = '<%=Constanst.BucketJumpTab.LandRover%>';
        var jaguarTab = '<%=Constanst.BucketJumpTab.Jaguar%>';
        var alTab = '<%=Constanst.BucketJumpTab.AL%>';
        var mzTab = '<%=Constanst.BucketJumpTab.MZ%>';
        var todayTab = '<%=Constanst.BucketJumpTab.Today%>';
        var savedTab = '<%=Constanst.BucketJumpTab.Saved%>';
        
        var landRoverScreen = '<%=BucketJumpView.LandRover.ToString()%>';
        var jaguarScreen = '<%=BucketJumpView.Jaguar.ToString()%>';
        var alScreen = '<%=BucketJumpView.AL.ToString()%>';
        var mzScreen = '<%=BucketJumpView.MZ.ToString()%>';
        var todayScreen = '<%=BucketJumpView.GroupTodayBucketJump.ToString()%>';
        var savedScreen = '<%=BucketJumpView.Saved.ToString()%>';
        
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';

        var updateIsFeaturedUrl = '<%= Url.Action("UpdateIsFeatured","Inventory")%>';
        var updateStatusFromInventoryPageUrl = '<%= Url.Action("UpdateStatusFromInventoryPage","Inventory")%>';
        var updateStatusUpdateStatusForSoldTab = '<%= Url.Action("UpdateStatusForSoldTab","Inventory")%>';
        var updateStatusFromSoldPageUrl = '<%= Url.Action("UpdateStatusChange","Inventory")%>';
        var updateViewInfoStatus = '<%= Url.Action("UpdateViewInfoStatus","Inventory")%>';

        var viewBrochureUrl = '<%= Url.Action("ViewBrochure","Inventory", new{ year = "_year", make = "_make", model = "_model", index = "_index"})%>';
        var sendBrochureUrl = '<%= Url.Action("SendBrochure","Inventory")%>';
        var checkBrochureUrl = '<%= Url.Action("CheckBrochure","Inventory", new{ year = "_year", make = "_make", model = "_model", index = "_index"})%>';
        var expandMode = 1;
        var normalMode = 0;
        var viewInfo = { sortFieldName: '<%=SessionHandler.MassBucketJumpViewInfo.SortFieldName %>', isUp: <%=SessionHandler.MassBucketJumpViewInfo.IsUp.ToString().ToLower() %>, currentView: '<%=SessionHandler.MassBucketJumpViewInfo.CurrentView %>', currentState: '<%=SessionHandler.MassBucketJumpViewInfo.CurrentState %>' };
        
        function checkSalePrice(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
                return "Price should <= $100,000,000";
            }
        }
        
        function checkMiles(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 2000000) {
                return "Miles should <= 2,000,000";
            }
        }

        $(document).ready(function () {
            $('img[id^="btnCraigslist_"]').live('click', function(){
                var id = this.id.split('_')[1];
                //blockUI();
                $.ajax({
                    type: "GET",
                    dataType: "html",
                    url: "/Craigslist/AuthenticationChecking",
                    data: {},
                    cache: false,
                    traditional: true,
                    success: function (result) {
                        if (result == 302) {
                            $.ajax({
                                type: "GET",
                                dataType: "html",
                                url: "/Craigslist/LocationChecking",
                                data: {},
                                cache: false,
                                traditional: true,
                                success: function (result) {
                                    if (result == false)
                                        ShowWarningMessage("Please choose State/City/Location in Admin section.");
                                    else
                                        var url = '/Craigslist/GoToPostingPreviewPage?listingId=' + id;
                                    $.fancybox({
                                        href: url,
                                        'width': 600,
                                        'height': 600,
                                        'hideOnOverlayClick': false,
                                        'centerOnScroll': true,
                                        'padding': 0,
                                        'scrolling': 'no',
                                        'onCleanup': function () {
                                        },
                                        isLoaded: function () {
                                            //unblockUI();
                                        },
                                        onClosed: function () {
                                            //unblockUI();
                                        }
                                    });
                                }
                            });
                            
                        } else {
                            unblockUI();
                            ShowWarningMessage("Please enter valid Email/Password in Admin section.");
                        }                        
                    },
                    error: function (err) {
                        unblockUI();
                    }
                });
            });
            
            //$("a.iframe").fancybox({ 'width': 430, 'height': 283, 'hideOnOverlayClick': false, 'centerOnScroll': true, "padding": 0, "margin":0 });
            $("a.iframeReport").click(function() {
                $.fancybox({
                    href: '<%= Url.Action("DailyBucketJumpReport", "MassBucketJump") %>',
                    'width': 350, 
                    'height': 190, 
                    'hideOnOverlayClick': false, 
                    'centerOnScroll': true, 
                    padding: 0, 
                    margin: 0,
                    type: 'iframe'
                });
                
            });
                
        });
    </script>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="ClientTemplates" runat="server">
    <%=Html.Partial("../Inventory/_TemplateBucketJumpInventory")%>
    
</asp:Content>
