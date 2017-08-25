<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.InventoryFormViewModel>" %>

<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Inventory List
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    <div id="inventory_top_btns_holder">
        <% var userRight = SessionHandler.UserRight.Inventory; %>

        <% if (userRight.Used) %>
        <% { %>
        <div id="inventory_used_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.Inventory.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_used_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.Inventory.ToString()  %>');grid.setActive(this);">Used</a>
        </div>
        <% } %>

        <% if (userRight.New) %>
        <% { %>
        <div id="inventory_new_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.NewInventory.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_new_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.NewInventory.ToString()  %>');grid.setActive(this);">New</a>
        </div>
        <% } %>

        <% if (userRight.Loaner) %>
        <% { %>
        <div id="inventory_loaner_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.LoanerInventory.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_loaner_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.LoanerInventory.ToString()  %>');grid.setActive(this);">Loaner</a>
        </div>
        <% } %>

        <% if (userRight.Auction) %>
        <% { %>
        <div id="inventory_auction_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.AuctionInventory.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_auction_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.AuctionInventory.ToString()  %>');grid.setActive(this);">Auction</a>
        </div>
        <% } %>

        <% if (userRight.Recon) %>
        <% { %>
        <div id="inventory_recon_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.ReconInventory.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_recon_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.ReconInventory.ToString()  %>');grid.setActive(this);">Recon</a>
        </div>
        <% } %>

        <% if (userRight.WholeSale) %>
        <% { %>
        <div id="inventory_wholesale_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.WholesaleInventory.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_wholesale_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.WholesaleInventory.ToString()  %>');grid.setActive(this);">Wholesale</a>
        </div>
        <% } %>

        <% if (userRight.TradeNotClear) %>
        <% { %>
        <div id="inventory_tradenotclear_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.TradeNotClear.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_tradenotclear_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.TradeNotClear.ToString()  %>');grid.setActive(this);">Trade Not Clear</a>
        </div>
        <% } %>

        <% if (userRight.SoldCar) %>
        <% { %>
        <div id="inventory_soldcars_tab" class="admin_top_btns" onclick="grid.changeView('<%=CurrentViewEnum.SoldInventory.ToString()  %>');grid.setChildActive(this);">
            <div class="number_below" id="inventory_soldcars_tab_number">
                Loading..
            </div>
            <a onclick="grid.changeView('<%=CurrentViewEnum.SoldInventory.ToString()  %>');grid.setActive(this);">Sold Car</a>
        </div>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="hdCountCars" value="0" />
    <input type="hidden" id="hdPhotoUrl" value="" />
    <input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />
    <input type="hidden" id="NumberOfWSTemplate" name="NumberOfWSTemplate" value="<%= (int) SessionHandler.NumberOfWSTemplate %>" />
    <input type="hidden" id="hdVehicleStatusCodeId" value="<%=ViewData["VehicleStatusCodeId"] ?? 2%>" />
    <% var userRight = SessionHandler.UserRight.Inventory; %>
    
    <div id="recent">
        <%if (!Model.IsCompactView)
          { %>
        <div id="container_right_btn_holder">
            <div id="container_right_btns">
                <div class="inventory_fixed_title" id="divTitle">
                    Inventory List
                </div>
                <div id="divSoldCarBtns" class="sold_car_btns_holder" style="display: none">
                    <select class="cbSoldNewUsed" id="cbSoldNewUsed">
                        <option value="SoldInventory">All</option>
                        <option value="NewSold">New</option>
                        <option value="UsedSold">Used</option>
                    </select>
                    <div class="calendar_logo_To">
                        <div class="ac_date_holder">
                            <div class="calendarImg">
                                <img alt="Calendar" src="/Content/images/vincontrol/icon-calendar.png">
                            </div>
                            <div class="calendarInput">
                                To
                                <input disabled="disabled" type="text" id="sold_car_date_To" />
                            </div>
                        </div>
                    </div>
                    <div class="calendar_logo_From">
                        <div class="ac_date_holder">
                            <div class="calendarImg">
                                <img alt="Calendar" src="/Content/images/vincontrol/icon-calendar.png">
                            </div>
                            <div class="calendarInput">
                                From
                                <input disabled="disabled" type="text" id="sold_car_date_From" />
                            </div>
                        </div>
                    </div>
                </div>

                <div id="brochureMenuDiv" class="right_header_btns invent_used_new" style="display: none"
                    onclick="grid.showBrochureContent();">
                    <a onclick="grid.showBrochureContent();">Brochure</a>
                </div>
                <div id="brochureMenuDivCancel" class="right_header_btns invent_used_new" style="display: none"
                    onclick="grid.hideBrochureContent();">
                    <a onclick="grid.hideBrochureContent();">Cancel</a>
                </div>

                <div id="NoContentDiv" class="right_header_btns invent_no_content" onclick="grid.changeView(noContentScreen);">
                    <a onclick="grid.changeView(noContentScreen);">No Content</a>
                </div>
                <div id="MissingContentDiv" class="right_header_btns invent_missiing_content" onclick="grid.changeView(missingContentScreen);">
                    <a onclick="grid.changeView(missingContentScreen);">Missing Content</a>
                </div>
                <div id="ACarDiv" class="right_header_btns invent_acars" onclick="grid.changeView(aCarScreen);">
                    <a onclick="grid.changeView(aCarScreen);">A Cars</a>
                </div>
                <div id="TodayBucketJump" class="right_header_btns invent_today_bucket_jump" onclick="grid.changeView(todayBucketJumpScreen);">
                    <a onclick="grid.changeView(todayBucketJumpScreen);">Today Bucket Jump</a> &nbsp;&nbsp;
                </div>
            </div>
        </div>
        <%} %>
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
                    <div class="filter_item_label">Miles</div>
                    <input type="text" id="mileFrom" onkeypress="validateInput(event)" onkeyup="formatMile(event, this)" />
                </div>
                <div class="filter_item_holder">
                    <div class="filter_item_label">to</div>
                    <input type="text" id="mileTo" onkeypress="validateInput(event)" onkeyup="formatMile(event, this)" />
                </div>
                <div class="filter_item_holder">
                    <input class="submitFilter" id="submitFilter" type="button" value="Submit" />
                </div>
            </div>
        </div>
        <div id="DivContent">
            <div id="divHasContent" class="vin_listVehicle_holder">
            </div>
            <div id="divNoContent" style="display: none; padding-left: 400px; margin-top: 10px;">There are no vehicles in this category.</div>
        </div>
        <div id="brochureContent" style="display: none">
            <div id="vincontrol_brochure_container">
                <div class="vinbrochure_carinfo_holder">
                    <div class="vinbrochure_carimg_holder">
                        <div class="vinbrochure_cartitle" id="titleBrochure">2013 Honda Civic</div>
                        <div class="vinbrochure_carimg">
                            <img id="imgCar" src="/Content/images/vincontrol/car_default.png" width="100%" />
                        </div>
                    </div>
                    <div class="vinbrochure_ymm_holder">
                        <div class="vinbrochure_ymm_select">
                            <div class="vinbrochure_select_items">
                                <select id="vinbrochure_select_year">
                                    <%if (ViewData["Year"] != null) %>
                                    <%{ %>
                                    <% foreach (var item in (List<vincontrol.DomainObject.ExtendedSelectListItem>)ViewData["Year"])
                                       {%>
                                    <option value="<%=item.Value %>"><%=item.Text %></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </div>
                            <div class="vinbrochure_select_items" id="divMake">
                                <select id="vinbrochure_select_make">
                                    <option>Make</option>
                                </select>
                            </div>
                            <div class="vinbrochure_select_items" id="divModel">
                                <select id="vinbrochure_select_model">
                                    <option>Model</option>
                                </select>
                            </div>
                        </div>
                        <div id="divCustomerInfo" class="vinbrochure_customer_info" style="display: none">
                            <div class="vinbrochure_customer_info_items">
                                <div class="vinbrochure_customer_title">Customer Name:</div>
                                <input type="text" id="vinbrochure_customer_name" maxlength="50" name="vinbrochure_customer_name" />
                                <div class="errorBrochure" style="display: none" id="divErrorName">Customer Name is required.</div>
                            </div>
                            <div class="vinbrochure_customer_info_items">
                                <div class="vinbrochure_customer_title">Customer E-Mail:</div>
                                <input type="text" id="vinbrochure_customer_email" maxlength="50" name="vinbrochure_customer_email" />
                                <div class="errorBrochure" style="display: none" id="divErrorEmail">Customer E-Mail is required.</div>
                                <div class="errorBrochure" style="display: none" id="divErrorEmailValid">Customer E-Mail is invalid.</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divSendBrochure" class="vinbrochure_btns_holder" style="display: none">
                    <div class="vinbrochure_btns_items" id="brochure">
                        <div class="vinbrochure_btns_items_holder">
                            <div class="vinbrochure_btns_style" onclick="ViewBrochure(0);">View</div>
                            <div class="vinbrochure_btns_style">
                                <input type="button" class="btnSendBrochure" id="btnSend" value="Send" />
                            </div>
                            <div class="vinbrochure_btns_text">Brochure Name 1</div>
                        </div>
                    </div>
                    <div class="vinbrochure_btns_items" id="brochure1" style="display: none">
                        <div class="vinbrochure_btns_items_holder">
                            <div class="vinbrochure_btns_style" onclick="ViewBrochure(1);">View</div>
                            <div class="vinbrochure_btns_style">
                                <input type="button" class="btnSendBrochure" id="btnSend1" value="Send" />
                            </div>
                            <div class="vinbrochure_btns_text">Brochure Name 2</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/VinControl/inventory.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/windowsticker.js")%>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var inventoryObj = [];
        var auctionTab = '<%=Constanst.InventoryTab.Auction%>';
        var usedTab = '<%=Constanst.InventoryTab.Used%>';
        var newTab = '<%=Constanst.InventoryTab.New%>';
        var loanerTab = '<%=Constanst.InventoryTab.Loaner%>';
        var reconTab = '<%=Constanst.InventoryTab.Recon%>';
        var wholesaleTab = '<%=Constanst.InventoryTab.Wholesale%>';
        var soldTab = '<%=Constanst.InventoryTab.SoldCar%>';
        var tradeNotClearTab = '<%=Constanst.InventoryTab.TradeNotClear%>';

        var auctionScreen = '<%=CurrentViewEnum.AuctionInventory.ToString()%>';
        var newScreen = '<%=CurrentViewEnum.NewInventory.ToString()%>';
        var usedScreen = '<%=CurrentViewEnum.Inventory.ToString()%>';
        var loanerScreen = '<%=CurrentViewEnum.LoanerInventory.ToString()%>';
        var reconScreen = '<%=CurrentViewEnum.ReconInventory.ToString()%>';
        var wholesaleScreen = '<%=CurrentViewEnum.WholesaleInventory.ToString()%>';
        var soldScreen = '<%=CurrentViewEnum.SoldInventory.ToString()%>';
        var tradeNotClearScreen = '<%=CurrentViewEnum.TradeNotClear.ToString()%>';
        var todayBucketJumpScreen = '<%=CurrentViewEnum.TodayBucketJump.ToString()%>';
        var expressBucketJumpScreen = '<%=CurrentViewEnum.ExpressBucketJump.ToString()%>';
        var aCarScreen = '<%=CurrentViewEnum.ACar.ToString()%>';
        var missingContentScreen = '<%=CurrentViewEnum.MissingContent.ToString()%>';
        var noContentScreen = '<%=CurrentViewEnum.NoContent.ToString()%>';
        var usedSoldScreen ='<%=CurrentViewEnum.UsedSold.ToString()%>';
        var newSoldScreen = '<%=CurrentViewEnum.NewSold.ToString()%>';

        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';

        var updateIsFeaturedUrl = '<%= Url.Content("~/Inventory/UpdateIsFeatured") %>';
      
      
        var updateStatusFromInventoryPageUrl = '<%= Url.Content("~/Inventory/UpdateStatusFromInventoryPage") %>';
        var updateStatusUpdateStatusForSoldTab = '<%= Url.Content("~/Inventory/UpdateStatusForSoldTab") %>';
        var updateStatusFromSoldPageUrl = '<%= Url.Content("~/Inventory/UpdateStatusChange") %>';
        var updateViewInfoStatus = '<%= Url.Content("~/Inventory/UpdateViewInfoStatus") %>';

        var viewBrochureUrl = '<%= Url.Action("ViewBrochure","Inventory",new{ year = "_year", make = "_make", model = "_model", index = "_index"})%>';
        var sendBrochureUrl = '<%= Url.Action("SendBrochure","Inventory")%>';
        var checkBrochureUrl = '<%= Url.Action("CheckBrochure","Inventory",new{ year = "_year", make = "_make", model = "_model", index = "_index"})%>';
        var expandMode = 1;
        var normalMode = 0;
        var viewInfo = { sortFieldName: '<%=SessionHandler.InventoryViewInfo.SortFieldName %>', isUp: <%=SessionHandler.InventoryViewInfo.IsUp.ToString().ToLower() %>, currentView: '<%=SessionHandler.InventoryViewInfo.CurrentView %>', currentState: '<%=SessionHandler.InventoryViewInfo.CurrentState %>' };
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    <% if (SessionHandler.AllStore)
       { %>
        <%= Html.Partial("_TemplateGroupInventory") %>
    <% }
       else
       { %>
      <%= Html.Partial("_TemplateInventory") %>
        <% } %>
       

  
    <%=Html.Partial("_TemplateFullInventory")%>
</asp:Content>
