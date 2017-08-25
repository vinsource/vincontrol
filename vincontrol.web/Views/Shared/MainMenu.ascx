<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<div id="container_left_btns_holder">
    <% UserRightSetting userRight = SessionHandler.UserRight; %>

    <% if (userRight.InventoryTab) %>
    <% { %>
    <a href="<%=Url.Action("ViewInventory","Inventory") %>">
        <div id="main_inventory_tab" class="container_left_btns">
            Inventory
        </div>
    </a>
    <% } %>

    <% if (userRight.AppraisalsTab) %>
    <% { %>
    <a href="<%=Url.Action("ViewAppraisal","Appraisal") %>">
        <div id="main_appraisals_tab" class="container_left_btns">
            <nobr class="appraisals_num"></nobr>
            Appraisals
        </div>
    </a>
    <% } %>

    <% if (userRight.KPITab) %>
    <% { %>
    <a href="<%=Url.Action("viewKPI","Market") %>">
        <div id="main_kpi_tab" class="container_left_btns">
            KPI
        </div>
    </a>
    <% } %>
    
    <% if (SessionHandler.Dealer.IsPendragon && SessionHandler.AllStore) %>
    <% { %>
    <a href="<%=Url.Action("Index","MassBucketJump") %>">
        <div id="main_bucketjump_tab" class="container_left_btns">
            Bucket Jump
        </div>
    </a>
    <% } %>

     <% if (userRight.MarketSearchTab) %>
    <% { %>
    <a href="<%=Url.Action("Index","StockingGuide") %>">
        <div id="main_marketsearch_tab" class="container_left_btns">
            Smart Stocking
        </div>
    </a>
    <% } %>


     <% if (userRight.MarketSearchTab && !SessionHandler.AllStore) %>
        <% { %>
        <a href="http://vinsell.com/Account/LogOnFromVincontrol?userid=<%=SessionHandler.UserId%>">
            <div id="vinsell_tab" class="container_left_btns">
                Vinsell
            </div>
        </a>
        <% } %>


    <% if (userRight.AdminTab || userRight.ReportsTab) %>
    <% { %>

    <div class="menu_gear_holder">
        <% if (userRight.AdminTab && !SessionHandler.AllStore) %>
        <% { %>
        <a href="<%=Url.Action("AdminSecurity","Admin") %>">
            <div id="main_admin_tab" class="container_left_btns">
                Admin
            </div>
        </a>
        <% } %>

        <% if (userRight.ReportsTab) %>
        <% { %>
        <a href="<%=Url.Action("viewReport","Report") %>">
            <div id="main_report_tab" class="container_left_btns">
                Reports
            </div>
        </a>
        <% } %>
        
        <% if (!SessionHandler.AllStore) {%>
        <div style="font-size: 13px; padding-top: 5px;">
            There are <a href="<%= Url.Content("~/Appraisal/ViewPendingAppraisal") %>"><span id="PendingSpan"></span> pending</a> appraisal(s)
        </div>
        <%}%>
        
        <% if ((SessionHandler.Dealer.IsPendragon && SessionHandler.AllStore) || SessionHandler.Dealer.IsPendragonWholesale) {%>
        <div style="font-size: 13px; padding-top: 5px; float: right; margin-right: 10px;">
            <div style="display: inline-block; float: right;">
                <img id="massCertified" src="../../Content/images/certified_icon.png" style="width: 42px; display: none;" alt="Certified" title="Certified"/>
                <img id="massACar" src="../../Content/images/a_car_icon.png" style="width: 42px; display: none;" alt="A Car" title="A Car"/>
            </div>
            <div style="display: inline-block; float: right;">
                <div class="massExpandedValue" style="display: none;">
                    <label style="width: 100px; display: inline-block; font-size: 12px;">
                        Certified 
                        <img src="../Content/images/ajaxloadingindicator.gif" style="height: 20px; display: none;"/>
                    </label>
                    <input id="saveCertifiedAmount" type="text" style="width: 85px;" maxlength="10"/>
                    
                </div>
                <div class="massExpandedValue" style="display: none;">
                    <label style="width: 100px; display: inline-block; font-size: 12px;">
                        Miscellaneous 
                        <img src="../Content/images/ajaxloadingindicator.gif" style="height: 20px; display: none;"/>
                    </label>
                    <input id="saveMileageAdjustment" type="text" style="width: 85px;" maxlength="10"/>                    
                </div>
                <div class="massExpandedValue" style="display: none;">
                    <label style="width: 99px; display: inline-block; vertical-align: top; font-size: 12px;">
                        Note 
                        <img src="../Content/images/ajaxloadingindicator.gif" style="height: 20px; display: none;"/>
                    </label>
                    <textarea id="saveNote" rows="10" cols="20"></textarea>
                    
                </div>
            </div>
        </div>
        <%}%>
    </div>
    <% } %>
</div>


