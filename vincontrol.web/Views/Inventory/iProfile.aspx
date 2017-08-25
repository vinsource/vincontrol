<%@ Page Title="" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>
<%@ Import Namespace="Vincontrol.Web.Models" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.ModelYear %>
    <%=Model.Make %>
    <%=Model.Model %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    <% Html.RenderPartial("InventoryTopMenu", new CarStatusViewModel() { InventoryStatus = Model.InventoryStatus, ListingId = Model.ListingId, Type = Model.Type }); %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />
    <input type="hidden" id="IsCarsCom" name="IsCarsCom" value="<%= Model.SavedSelections.IsCarsCom %>" />
    <input type="hidden" id="Options" name="Options" value="<%= Model.SavedSelections.Options %>" />
    <input type="hidden" id="Trims" name="Trims" value="<%= Model.SavedSelections.Trims %>" />
    <input type="hidden" id="IsCertified" name="IsCertified" value="<%= Model.SavedSelections.IsCertified %>" />
    <input type="hidden" id="IsAll" name="IsAll" value="<%= Model.SavedSelections.IsAll %>" />
    <input type="hidden" id="IsFranchise" name="IsFranchise" value="<%= Model.SavedSelections.IsFranchise %>" />
    <input type="hidden" id="IsIndependant" name="IsIndependant" value="<%= Model.SavedSelections.IsIndependant %>" />
    <input type="hidden" id="MaketStatus" name="MaketStatus" value="-1" />
    <input type="hidden" id="NeedToReloadPage" name="NeedToReloadPage" value="false" />
    <input type="hidden" id="Action" name="Action" value="" />
    <input type="hidden" id="NumberOfWSTemplate" name="NumberOfWSTemplate" value="<%= (int) SessionHandler.NumberOfWSTemplate %>" />
    <%=  Html.HiddenFor(x=>x.ListingId)%>
    <%=  Html.HiddenFor(x=>x.InventoryStatus)%>
    <input name="list1SortOrder" type="hidden" />
    <div class="profile_tab_holder" id="profile_edit_holder" style="display: block">
        <div id="container_right_btn_holder">
            <div id="container_right_btns">
                <div class="profile_vh_info">
                    <div style="display: inline-block;float: left;"><%=  Html.DynamicHtmlLabel("txtTitle", "Title")%></div>
                    <div style="margin-top: -10px;">
                        <% if (Model.IsCertified)
                        { %>
                        <img src="../../Content/images/certified_icon_white.png" style="width: 32px;" alt="Certified" title="Certified"/>
                        <% }%>
                        <% if (Model.ACar)
                        { %>
                        <img src="../../Content/images/a_car_icon_white.png" style="width: 32px;" alt="A Car"  title="A Car"/>
                        <% }%>
                    </div>
                </div>
                <div class="pt_left_carsInfo_btn_holder">
                    <%if (Model.Type == Constanst.CarInfoType.New || Model.Type == Constanst.CarInfoType.Used || Model.Type == Constanst.CarInfoType.Sold) %>
                    <%
                      {
                    %>
                    <% if (Model.Condition == Constanst.ConditionStatus.Used && Model.NotDoneBucketJump)
                       { %>
                    <div class="btns_shadow pt_left_carsInfo_btn" id="btnDoneTodayBucketJump">
                        <a onclick="doneTodayBucketJumpOnProfilePage(<%=Model.ListingId %> ,<%=Model.DaysInInvenotry %>);"
                            style="text-align: right;">Done
                        </a>
                    </div>
                    <% }%>
                    <% if (Model.PreviousListingId != -1)
                       { %>
                    <div class="btns_shadow pt_left_carsInfo_btn">
                        <% if (Model.Type == Constanst.CarInfoType.Sold) %>
                        <% { %>
                        <a href="<%= Url.Action("ViewISoldProfile", "Inventory", new {ListingID = Model.PreviousListingId, Page = String.IsNullOrEmpty(Request["page"]) ? "Inventory" : Request["page"] }) %>">Previous </a>
                        <% } %>
                        <% else %>
                        <% { %>
                        <a href="<%= Url.Action("ViewIProfile", "Inventory", new {ListingID = Model.PreviousListingId, Page = String.IsNullOrEmpty(Request["page"]) ? "Inventory" : Request["page"] }) %>">Previous </a>
                        <% } %>
                    </div>
                    <% }%>
                    <%else
                       { %>
                    <div class="pt_left_carsInfo_btn_Empty">
                        &nbsp;
                    </div>
                    <% }%>
                    <% if (Model.NextListingId != -1)
                       { %>
                    <% if (Model.Type == Constanst.CarInfoType.Sold) %>
                    <% { %>
                    <a href="<%= Url.Action("ViewISoldProfile", "Inventory", new {ListingID = Model.NextListingId, Page = String.IsNullOrEmpty(Request["page"]) ? "Inventory" : Request["page"] }) %>"
                        style="text-align: right;">
                        <div class="btns_shadow pt_left_carsInfo_btn">
                            Next
                        </div>
                    </a>
                    <% } %>
                    <% else %>
                    <% { %>
                    <a href="<%= Url.Action("ViewIProfile", "Inventory", new {ListingID = Model.NextListingId, Page = String.IsNullOrEmpty(Request["page"]) ? "Inventory" : Request["page"] }) %>"
                        style="text-align: right;">
                        <div class="btns_shadow pt_left_carsInfo_btn">
                            Next
                        </div>
                    </a>
                    <% } %>
                    <% } %>
                    <% } %>
                </div>

            </div>
        </div>
        <div id="container_right_content">
            <div class="profile_top_container">
                <div class="profile_top_left_holder">
                    <div class="pt_left_carsInfo">
                        <div class="pt_left_carsInfo_left">
                            <div class="pt_left_carsInfo_img" style="padding-left: 10px;">
                                <a id="show_images" class="iframe" href="<%=Url.Action("GetImageLinks","Inventory", new {listingId= Model.ListingId,inventoryStatus=Model.InventoryStatus }) %>">
                                    <%= Html.DynamicHtmlLabel("txtCarImage", "CarImage") %></a>


                            </div>
                            <% if (Model.Type != Constanst.CarInfoType.Sold) %>
                            <% { %>
                            <div>
                                <img id="imgShare" src="../../Content/images/share_icon.png" alt="Share" style="width: 42px; position: absolute; top: 0; left: 130px; cursor: pointer;" />
                            </div>
                            <% } %>
                            <% if (Model.Type == Constanst.CarInfoType.New) %>
                            <%
                               { %>
                            <div class="content_new_bg">
                                <div class="content_view_title">
                                    Brochure Sharing
                                </div>
                                <div class="bg_edit_content">
                                    <div class="bg_add_content" style="height: 66px;">
                                        <div>
                                        </div>
                                        <div>
                                            <form id="formBrochureSharing" method="post">
                                                <div style="width: 120px; float: left">
                                                    Customer Name(s):
                                                </div>
                                                <input type="text" id="txtCustomerNameBrochure" name="txtCustomerName" data-validation-engine="validate[required]"
                                                    data-errormessage-value-missing="Customer Name is required!" style="width: 180px" maxlength="50" />
                                                <div style="width: 120px; float: left">
                                                    Email(s):
                                                </div>
                                                <input type="text" id="sharedEmailBrochure" name="sharedEmails" data-validation-engine="validate[required,custom[email]]"
                                                    data-errormessage-value-missing="Email is required!" style="width: 180px" maxlength="50" />
                                            </form>
                                        </div>
                                    </div>
                                </div>
                                <div class="bg_edit_btns">

                                    <div class="btns_shadow bg_btns_save_brochure">
                                        <a id="aDoneBuyerGuide" href="javascript:;" style="color: White; font-weight: bold;">Share</a>
                                    </div>
                                    <div class="btns_shadow bg_btns_cancel">
                                        <a id="aCancelBuyerGuide" href="javascript:;" style="color: White; font-weight: bold;">Cancel</a>
                                    </div>
                                </div>
                            </div>
                            <% } %>
                            <% else
                               { %>
                            <div class="content_new_bg">
                                <div class="content_view_title">
                                    Flyer Sharing
                                </div>
                                <div class="bg_edit_content">
                                    <div class="bg_add_content" style="height: 66px;">
                                        <div>
                                        </div>
                                        <div>
                                            <form id="formFlyerSharing" method="post">
                                                <div style="width: 120px; float: left">
                                                    Customer Name(s):
                                                </div>
                                                <input type="text" id="txtCustomerName" name="txtCustomerName" data-validation-engine="validate[required]"
                                                    data-errormessage-value-missing="Customer Name is required!" style="width: 180px" maxlength="250" />
                                                <div style="width: 120px; float: left">
                                                    Email(s):
                                                </div>
                                                <input type="text" id="sharedEmails" name="sharedEmails" data-validation-engine="validate[required,funcCall[checkEmailAndCustomerName]]"
                                                    data-errormessage-value-missing="Email is required!" style="width: 180px" maxlength="250" />
                                            </form>
                                        </div>
                                        <div>
                                            (Seperate by , )
                                        </div>
                                    </div>

                                </div>
                                <div class="bg_edit_btns">

                                    <div class="btns_shadow bg_btns_save">
                                        <a id="a2" href="javascript:;" style="color: White; font-weight: bold;">Share</a>
                                    </div>
                                    <div class="btns_shadow bg_btns_cancel">
                                        <a id="a3" href="javascript:;" style="color: White; font-weight: bold;">Cancel</a>
                                    </div>
                                </div>
                            </div>
                            <% } %>
                        </div>
                        <div class="pt_left_carsInfo_right">
                            <ul>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelVin", "Vin")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelExteriorColor", "ExteriorColor")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelStock", "Stock")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelInteriorColor", "InteriorColor")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelYear", "Year")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelDriveType", "DriveType")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelMake", "Make")%>
                                </li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelCylinders", "Cylinders")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelModel", "Model")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelLitters", "Litters")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelTrim", "Trim")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelDoors", "Doors")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelOdometer", "Odometer")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelFuelType", "FuelType")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelTranmission", "Tranmission")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelStyle", "Style")%></li>
                                <li>
                                    <%=  Html.DynamicHtmlLabel("txtLabelDaysInInventory", "DaysInInventory")%></li>

                                <% if (Model.IsCommercialTruck)
                                   { %>
                                <li>
                                    <img id="imgTruck" src="../../images/bt_truck.png" style="border: 0px; width: 120px; height: 20px; margin-top: 2px; margin-right: 5px;"></li>
                                <% } %>
                                
                            </ul>
                        </div>
                    </div>
                    <div id="container_right_price">
                        <div class="profile_top_items_holder">
                            <div class="profile_top_items">
                                <%if (HTMLControlExtension.CanSeeButton(Constanst.ProfileButton.ACV))
                                  {%>
                                <%Html.BeginForm("UpdateAcv", "Inventory", FormMethod.Post,
                                                 new { id = "SaveACVForm" });%>
                                <div class="profile_top_items_text">
                                    ACV
                                </div>

                                <div class="divValue">
                                    <% if (SessionHandler.CurrentUser.RoleId == Constanst.RoleType.Employee) %>
                                    <% { %>
                                    <input class="profile_top_items_input" type="text" name="acv" id="txtACV" disabled="disabled" value="<%=Model.Acv%>" />
                                    <% } %>
                                    <% else %>
                                    <% { %>
                                    <input class="profile_top_items_input" type="text" name="acv" id="txtACV" data-validation-engine="validate[funcCall[checkPriceACV]]" value="<%=Model.Acv%>" />
                                    <% } %>

                                    <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" height="30px" />
                                </div>

                                <%=Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId")%>
                                <%Html.EndForm();%>
                                <%
                                  }%>
                            </div>
                            <div class="profile_top_items">
                                <%if (HTMLControlExtension.CanSeeButton(Constanst.ProfileButton.DealerCost))
                                  {%>
                                <%
                                      Html.BeginForm("UpdateDealerCost", "Inventory", FormMethod.Post,
                                                     new { id = "SaveDealerCostForm" });%>
                                <div class="profile_top_items_text">
                                    Dealer Cost
                                </div>

                                <div class="divValue">
                                    <% if (SessionHandler.CurrentUser.RoleId == Constanst.RoleType.Employee) %>
                                    <% { %>
                                    <input type="text" class="profile_top_items_input" name="dealerCost" id="txtDealerCost" disabled="disabled" value="<%=Model.DealerCost%>" />
                                    <% } %>
                                    <% else %>
                                    <% { %>
                                    <input type="text" class="profile_top_items_input" name="dealerCost" id="txtDealerCost" value="<%=Model.DealerCost%>" data-validation-engine="validate[funcCall[checkPriceDealerCost]]" />
                                    <% } %>

                                    <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" height="30px" />
                                </div>

                                <%=Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId")%>
                                <%
                                      Html.EndForm();%>
                                <%
                                  }%>
                            </div>
                            <div class="profile_top_items">
                                <% Html.BeginForm("UpdateSalePrice", "Inventory", FormMethod.Post, new { id = "SaveSalePriceForm", vehicleStatusCodeId = Constanst.VehicleStatus.Inventory });%>
                                <div class="profile_top_items_text">
                                    <% if (Model.Condition == Constanst.ConditionStatus.New)
                                       { %>
                                    Internet Price
                                    <% }
                                       else
                                       { %>
                                      Sale Price
                                    <% } %>
                                </div>

                                <div class="divValue">
                                    <% if (SessionHandler.CurrentUser.RoleId == Constanst.RoleType.Employee) %>
                                    <% { %>
                                    <input type="text" class="profile_top_items_input" name="salePrice" id="txtSalePrice" disabled="disabled" value="<%=Model.SalePrice %>" />
                                    <% } %>
                                    <% else %>
                                    <% { %>
                                    <input type="text" class="profile_top_items_input" name="salePrice" id="txtSalePrice" value="<%=Model.SalePrice %>" data-validation-engine="validate[funcCall[checkSalePrice]]" />
                                    <% } %>

                                    <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" height="30px" />
                                </div>

                                <%= Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId")%>
                                <% Html.EndForm(); %>
                            </div>
                            <% if (Model.Condition == Constanst.ConditionStatus.New)
                               { %>
                            <div class="profile_top_items">
                                <% Html.BeginForm("UpdateMsrp", "Inventory", FormMethod.Post, new { id = "SaveMsrpPriceForm" }); %>
                                <div class="profile_top_items_text">
                                    <% if (Model.Condition == Constanst.ConditionStatus.New)
                                       { %>
                                                    MSRP
                                                <% }
                                       else
                                       { %>
                                                    MSRP
                                            <% } %>
                                </div>

                                <div class="divValue">
                                    <% if (SessionHandler.CurrentUser.RoleId == Constanst.RoleType.Employee) %>
                                    <%
                                       { %>
                                    <input type="text" class="profile_top_items_input" name="msrp" id="txtMsrp" disabled="disabled" value="<%= Model.Msrp %>" />
                                    <% } %>
                                    <%
                                       else %>
                                    <%
                                       { %>
                                    <input type="text" class="profile_top_items_input" name="msrp" id="txtMsrp" value="<%= Model.Msrp %>" data-validation-engine="validate[funcCall[checkSalePrice]]" />
                                    <% } %>

                                    <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" height="30px" />
                                </div>

                                <%= Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId") %>
                                <% Html.EndForm(); %>
                            </div>
                            <div class="profile_top_items">
                                <% Html.BeginForm("UpdateDealerDiscount", "Inventory", FormMethod.Post, new { id = "SaveDiscountForm" }); %>
                                <div class="profile_top_items_text">
                                    <% if (Model.Condition == Constanst.ConditionStatus.New)
                                       { %>
                                                    Dealer Discount
                                                <% }
                                       else
                                       { %>
                                                    Dealer Discount
                                            <% } %>
                                </div>

                                <div class="divValue">
                                    <% if (SessionHandler.CurrentUser.RoleId == Constanst.RoleType.Employee) %>
                                    <%
                                       { %>
                                    <input type="text" class="profile_top_items_input" name="dealerDiscount" id="txtDealerDiscount" disabled="disabled" value="<%= Model.DealerDiscount %>" />
                                    <% } %>
                                    <%
                                       else %>
                                    <%
                                       { %>
                                    <input type="text" class="profile_top_items_input" name="dealerDiscount" id="txtDealerDiscount" value="<%= Model.DealerDiscount %>" data-validation-engine="validate[funcCall[checkSalePrice]]" />
                                    <% } %>

                                    <img class="imgLoading" src="../Content/images/ajaxloadingindicator.gif" height="30px" />
                                </div>

                                <%= Html.DynamicHtmlControlForIprofile("ListingId", "hiddenListingId") %>
                                <% Html.EndForm(); %>
                            </div>
                            
                            <% } %>
                        </div>
                    </div>
                    <div class="pt_left_charts_holder">
                        <div class="ptl_charts_title">
                            <% if (Model.Condition == Constanst.ConditionStatus.New) %>
                            <%
                               { %>
                            <div class="btns_shadow ptl_charts_btn expend_market">
                                <input type="button" style="margin-left: 0; border: none; background: none; color: white; padding-top: 2px; font-weight: bold; cursor: pointer"
                                    id="btnExpand" name="btnExpand"
                                    value="Expand" />
                            </div>
                            <% }
                               else
                               { %>

                            <a id="A1" href="<%= Url.Action("ViewFullChart", "Chart", new { ListingId = Model.ListingId, currentScreen = Model.CurrentScreen }) %>">
                                <div class="btns_shadow ptl_charts_btn expend_market">
                                    <input type="button" style="margin-left: 0; border: none; background: none; color: white; padding-top: 2px; font-weight: bold; cursor: pointer"
                                        id="graphButton" name="toggleGraph"
                                        value="Expand" />
                                </div>
                            </a>
                            <% } %>
                            <div class="ptl_charts_text">
                                Within (100 miles) from your location
                            </div>
                        </div>
                        <div class="ptl_chart_holder">
                            <% if (Model.Condition == Constanst.ConditionStatus.New) %>
                            <%
                               { %>
                            <div class="ptl_charts_none">
                                Not Applicable
                            </div>
                            <% }
                               else
                               {

                            %>
                            <div class="ptl_charts_left">
                                <div class="ptl_charts" id="graphWrap">
                                    <div id="placeholder" style="height: 100%; width: 100%; padding-left: 8px;" align="center">
                                        <img src="/content/images/ajaxloadingindicator.gif" />
                                    </div>
                                </div>
                                <div style="clear: both; border-top: solid 2px gray; height: 60px;">
                                    <div class="ptl_charts_ranking">
                                        <div class="ptl_chart_rangking_text">
                                            <div class="market_ranking_key">
                                                Market Ranking
                                            </div>
                                        </div>
                                        <div class="ptl_chart_avg">
                                            <nobr style="color: #458C00"><label id="txtAvgDays">
                                            0</label></nobr>
                                            Avg Days
                                        </div>
                                    </div>
                                    <div style="float: left; width: 100%; padding-left: 30px;">
                                        <div class="ptl_charts_right_items" style="margin-top: 0px;">
                                            <div class="ptl_charts_right_icons kpi_market_above_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #D12C00">
                                                <label id="txtMaxPrice">
                                                    0</label>
                                            </div>
                                        </div>
                                        <div class="ptl_charts_right_items">
                                            <div class="ptl_charts_right_icons kpi_market_equal_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #458C00">
                                                <label id="txtAveragePrice">
                                                    0</label>
                                            </div>
                                        </div>
                                        <div class="ptl_charts_right_items">
                                            <div class="ptl_charts_right_icons kpi_market_below_icon">
                                            </div>
                                            <div class="ptl_charts_right_value" style="color: #0062A1">
                                                <label id="txtMinPrice">
                                                    0</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="ptl_charts_right">
                                <div class="market_ranking_value">
                                    <label id="txtRanking">
                                        0
                                    </label>
                                    out of
                                    <label id="txtCarsOnMarket">
                                        0</label>
                                </div>

                                <div>
                                    <div id="divTrims" style="display: inline-block; text-overflow: ellipsis; white-space: nowrap; overflow: hidden; width: 160px; background-color: #222; color: white; padding-left: 17px; font-size: 13px;">
                                        &nbsp;
                                    </div>
                                    <div id="trim-filter" style="padding: 5px; height: 155px; overflow-y: auto; font-size: 13px;">
                                    </div>
                                    <% if (SessionHandler.IsEmployee == false) %>
                                    <% { %>
                                    <div class="btns_shadow profile_top_items_btn" id="SaveSelectionSmallChart" style="margin: 10px 5px;">
                                        Save
                                    </div>
                                    <% } %>
                                </div>
                            </div>
                            <% } %>
                        </div>
                    </div>
                    <div class="ptl_carinfo_des">
                        <%--<%=  Html.DynamicHtmlLabel("txtLabelDescription", "Description")%>--%>
                        <a class="iframe printFourSquare" title="print pdf for 4square form" href="<%=Url.Action("PrintFourSquare","PDF",new {inventoryId = Model.ListingId, type=Model.Type}) %>">
                            <div id="fourSquareBtn"></div>
                        </a>
                        <a class="iframe autoLoanCalculator" title="autoloan calculator" href="<%=Url.Action("AutoLoanCalculator","Inventory",new {inventoryId = Model.ListingId, type=Model.Type}) %>">
                            <div id="autoCalculatorBtn"></div>
                        </a>
                        <a class="iframe vehicleLog" title="vehicle log" href="<%=Url.Action("VehicleLog","Inventory",new {inventoryId = Model.ListingId, type=Model.Type}) %>">
                            <div id="vehicleLogBtn"></div>
                        </a>
                        <a class="" title="facebook posting" href="javascript:;">
                            <div id="fbPostingBtn"></div>
                        </a>
                        <a class="" title="youtube posting" href="javascript:;">
                            <div id="youtubePostingBtn"></div>
                        </a>
                        <a class="" title="option sticker" href="javascript:;">
                            <div id="optionStickerBtn"></div>
                        </a>
                    </div>
                </div>
                <% if (Model.Condition == Constanst.ConditionStatus.New) %>
                <%
                   { %>
                <div class="profile_top_right_holder">
                    <div class="ptr_items">
                        <div class="ptr_items_top" style="position: relative;">
                            <a>
                                <img style="display: inline-flexbox; float: left;" src='<%=Url.Content("~/Images/carfax-large.jpg")%>'
                                    alt="carfax logo" /></a>
                            <% if (SessionHandler.IsEmployee == false) %>
                            <% { %>
                            <div style="position: absolute; top: 10px; left: 170px; width: 140px;">
                                <a style="display: inline-block; cursor: pointer; height: 17px; font-size: 1.0em; font-weight: bold;"
                                    title="DMV Desk"><span style="position: relative; top: 0px;">DMV
                                        Desk</span> </a><a style="display: inline-block; cursor: pointer; height: 17px; font-size: 1.0em; font-weight: bold; padding-left: 5px; border-left: 1px solid;"
                                            title="KSR"><span
                                                style="position: relative; top: 0px;">KSR</span> </a>
                            </div>
                            <% } %>
                        </div>
                        <div class="ptr_items_content carFax-data-no-content">
                            <br />
                            <br />
                            Not Applicable
                        </div>
                    </div>

                    <% if (SessionHandler.IsEmployee == false) %>
                    <% { %>
                    <div class="ptr_itemsKBB">
                        <div class="ptr_items_top" id="Div2">
                            <a title="MMR Auction Pricing" href="JavaScript:newPopup('<%=Url.Content("~/Market/OpenManaheimLoginWindow?Vin=")%><%=Model.Vin%>')">
                                <div class="ptr_items_top_title_holder">
                                    <div class="ptr_items_top_title_logo">

                                        <img src="/content/images/logo_manheim.gif" />
                                    </div>
                                    <div class="ptr_items_top_title_text">
                                        Manheim
                                    </div>
                                </div>
                            </a>
                            <div class="ptr_item_top_circle circleBase">
                                0
                            </div>
                        </div>
                        <div class="ptr_items_contentKBB">
                            <div class="ptr_items_content_list partialContents">
                                <div class="data-no-content" align="center">
                                    Not Applicable
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="ptr_itemsKBB">
                        <div class="ptr_items_top">
                            <div class="ptr_items_top_title_holder">
                                <div class="ptr_items_top_title_logo">
                                    <img src="/content/images/kbb-logo-alpha.png" />
                                </div>
                                <div class="ptr_items_top_title_text">
                                    KBB
                                </div>
                            </div>
                            <div class="ptr_item_top_circle circleBase">
                                0
                            </div>
                        </div>
                        <div class="ptr_items_contentKBB">
                            <div id="div5" class="ptr_items_content_list partialContents">
                                <div class="data-no-content" align="center">
                                    Not Applicable
                                </div>
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
                <% }
                   else
                   { %>
                <div class="profile_top_right_holder">
                    <div class="ptr_items">
                        <div class="ptr_items_top" style="position: relative;">
                            <%--<a href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=<%=Model.CarFaxDealerId%>&vin=<%=Model.Vin%>')">--%>
                            <a id="carfaxreportlive_<%=Model.ListingId %>">
                                <img style="display: inline-flexbox; float: left;" src='<%=Url.Content("~/Images/carfax-large.jpg")%>'
                                    alt="carfax logo" /></a>
                            <% if (SessionHandler.IsEmployee == false) %>
                            <% { %>
                            <div style="position: absolute; top: 10px; left: 170px; width: 140px;">
                                <a style="display: inline-block; cursor: pointer; height: 17px; font-size: 1.0em; font-weight: bold;"
                                    title="DMV Desk" onclick="window.open('https://secure.dmvdesk.com/dmvdesk/', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')">
                                    <span style="position: relative; top: 0px;">DMV Desk</span>
                                </a>
                                <a style="display: inline-block; cursor: pointer; height: 17px; font-size: 1.0em; font-weight: bold; padding-left: 5px; border-left: 1px solid;"
                                    title="KSR" onclick="window.open('https://www.dmvlink.com/online/default.asp', 'mywindow', 'location=1,status=1,scrollbars=1,  width=600,height=500')">
                                    <span style="position: relative; top: 0px;">KSR</span>
                                </a>
                            </div>
                            <% } %>
                        </div>
                        <div class="ptr_items_content">

                            <div class="ptr_items_content_header">
                                <div class="ptr_cafax_owners">
                                    <nobr class="ptr_cafax_number" id="carfaxOwner">
									   
								    </nobr>
                                    Owner(s)
                                </div>
                                <div class="ptr_cafax_service_repords">
                                    <nobr class="ptr_cafax_number" id="carfaxRecord">
									    
								    </nobr>
                                    Service Reports
                                </div>
                            </div>
                            <div id="history-report" align="center" style="clear: both; float: left; width: 100%;">
                                <img src="/content/images/ajaxloadingindicator.gif" />


                            </div>
                        </div>
                    </div>
                    <% if (SessionHandler.IsEmployee == false) %>
                    <% { %>
                    <div class="ptr_itemsKBB">
                        <div class="ptr_items_top" id="controlappraisals_manheim">
                            <a title="MMR Auction Pricing" href="JavaScript:newPopup('<%=Url.Content("~/Market/OpenManaheimLoginWindow?Vin=")%><%=Model.Vin%>')">
                                <div class="ptr_items_top_title_holder">
                                    <div class="ptr_items_top_title_logo">

                                        <img src="/content/images/logo_manheim.gif" />
                                    </div>
                                    <div class="ptr_items_top_title_text">
                                        Manheim
                                    </div>
                                </div>
                            </a>
                            <div class="ptr_item_top_circle circleBase" id="ManheimCount">
                                0
                            </div>
                        </div>
                        <a style="float: right; font-weight: normal; font-size: .9em; display: block; margin-top: -18px;"
                            href="<%= Url.Action("ResetManheimTrim", "Market", new { listingId = Model.ListingId }) %>">Not a correct trim? Click here</a>
                        <div class="ptr_items_contentKBB">
                            <div class="ptr_items_content_list partialContents" id="ManheimContent">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="ptr_itemsKBB">
                        <div class="ptr_items_top" id="controlappraisals_kbb">
                            <div class="ptr_items_top_title_holder">
                                <div class="ptr_items_top_title_logo">
                                    <img src="/content/images/kbb-logo-alpha.png" />
                                </div>
                                <div class="ptr_items_top_title_text">
                                    KBB
                                </div>
                            </div>
                            <div class="ptr_item_top_circle circleBase" id="KBBCount">
                                0
                            </div>
                        </div>
                        <a style="float: right; font-weight: normal; font-size: .9em; display: block; margin-top: -18px;"
                            href="<%= Url.Action("ResetKbbTrim", "Market", new { listingId = Model.ListingId }) %>">Not a correct trim? Click here</a>
                        <div class="ptr_items_contentKBB">
                            <div id="divPartialKbb" class="ptr_items_content_list partialContents">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="ptr_itemsKBB">
                        <div class="ptr_items_top" id="controlappraisals_auction">
                            <div class="ptr_items_top_title_holder">
                                <div class="ptr_items_top_title_logo">
                                    <img src="/content/images/logo_manheim.gif" />
                                </div>
                                <div class="ptr_items_top_title_text">Auction</div>
                            </div>

                        </div>
                        <div class="ptr_items_contentKBB">
                            <div id="divPartialAuction" class="">
                                <div class="data-content" align="center">
                                    <img src="/content/images/ajaxloadingindicator.gif" />
                                </div>

                            </div>
                        </div>
                    </div>
                    <% } %>
                    <% } %>
                </div>
                
            </div>
        </div>
        <div style="display: none">
            <input type="checkbox" id="certified" name="certified" key="certified" />
        </div>
        
        <input type="hidden" id="hdVehicleStatusCodeId" value="<%=Model.VehicleStatusCodeId%>" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script src="<%=Url.Content("~/js/excanvas.compiled.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.dragsort.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/jquery.flot.functions.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/ui.dropdownchecklist-1.4-min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/windowsticker.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/MarketMapping.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/google_map_graph_plotter.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/Chart/ChartSmall.js")%>" type="text/javascript"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/d3/d3.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/aight/aight.d3.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/return-large.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/libs/func.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/js/V3Chart/chart.js")%>"></script>
    <script type="text/javascript">
        function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }
        // ############################### //
        // Chart Data Set and Draw Section //
        // ############################### //
        // check url for GET
        var window_url = window.location.search;

        // default graph to unexpanded draw size
        var expand = expanded(window_url);

        // set chart dimensions
        if (expand) {
            chart_dimensions = ["700px", "500px"];
            $('#graphButton').css('display', 'none');
        } else {
            chart_dimensions = ["400px", "183px"];
        }

        // grab graph div element and click element
        var gwrap = $('#graphWrap');
        gwrap.css('width', chart_dimensions[0]);
        gwrap.css('height', chart_dimensions[1]);

        // load default options & trims
        // we already filtered the results in code behind, so we don't need to do on the js code
        var default_option = [0];
        var default_trim = [0];

        var ListingId = $('#ListingId').val();

        var myArray = [];
        var trimsData = [];
        var selectedtrimsData = [];
        var $data = [];
        var dataTrims = null;

        // create ajax post url

        var webAPI = '<%=System.Web.Configuration.WebConfigurationManager.AppSettings["WebAPIServerURL"]%>';

        var carfaxUrl = webAPI + '/Inventory/CarFaxData?listingId=_listingId&dealerId=_dealerId';
        carfaxUrl = carfaxUrl.replace('_listingId', '<%=Model.ListingId%>').replace('_dealerId', '<%=Model.DealerId%>');


        var karPowerUrl = webAPI + '/Inventory/KarPowerData?listingId=_listingID&inventoryStatus=_inventoryStatus&dealerId=_dealerId&zipCode=1';
        karPowerUrl = karPowerUrl.replace('_listingID', '<%=Model.ListingId%>').replace('_inventoryStatus', '<%=Model.CurrentScreen%>').replace('_dealerId', '<%=Model.DealerId%>');

        //var manheimUrl = '<%= Url.Action("ManheimData", "Inventory", new { Model.ListingId,InventoryStatus = Model.CurrentScreen }) %>';            
        var manheimUrl = webAPI + '/Inventory/ManheimData?listingId=_listingID&inventoryStatus=_inventoryStatus&dealerId=_dealerId';
        manheimUrl = manheimUrl.replace('_listingID', '<%=Model.ListingId%>').replace('_inventoryStatus', '<%=Model.CurrentScreen%>').replace('_dealerId', '<%=Model.DealerId%>');
        var waitingImage = '<%= Url.Content("~/images/ajaxloadingindicator.gif") %>';

        var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange: { min: 0, max: 100 }, isSoldView: false, isSmallChart: false };
        ChartInfo.isSmallChart = true;

        var $dCar = {};
        var $selectedCar = {};
        // set y change check
        var newY = false;

        // create current filterred list of car
        var $currentFilterredList = [];

        var openChart = false;
        var default_trim;

        var auctionUrl = webAPI + '/Auction/AuctionData?listingId=_listingID&dealerId=_dealerId&vehicleStatus=2';
        auctionUrl = auctionUrl.replace('_listingID', '<%=Model.ListingId%>').replace('_dealerId', '<%=Model.DealerId%>');

        var UrlPaths = UrlPaths || { requestNationwideUrl: "" };

        UrlPaths.requestNationwideUrl = webAPI + '/Inventory/GetMarketDataByListingNationwideWithHttpPost?listingId=_listingID&dealerId=_dealerId&chartScreen=_chartScreen';
        UrlPaths.requestNationwideUrl = UrlPaths.requestNationwideUrl.replace("_listingID", ListingId).replace("_dealerId", '<%=Model.DealerId%>').replace("_chartScreen", "Inventory");

    </script>
    <script type="text/javascript">
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';

        function checkEmailAndCustomerName(field, rules, i, options) {
            console.log($("#sharedEmails").val().split(',').length);
            if ($("#txtCustomerName").val().split(',').length != $("#sharedEmails").val().split(',').length) {
                return "The number of customer name should be equal to the number of email";
            }
            if ($("#sharedEmails").val().split(',').length > 5) {
                return "The number of customer should be <= 5";
            }
            if ($("#sharedEmails").val() != '') {
                var arr = $("#sharedEmails").val().split(',');
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        if (!isValidEmailAddress(arr[i].replace(/ /g, ""))) {
                            return "The email is invalid";
                        }
                    }
                }
            }
        }

        function checkPriceACV(field, rules, i, options) {
            if (parseInt($("#txtACV").val().replace(/,/g, "")) > 100000000) {
                return "The ACV should <= $100,000,000";
            }
        }

        function checkPriceDealerCost(field, rules, i, options) {
            if (parseInt($("#txtDealerCost").val().replace(/,/g, "")) > 100000000) {
                return "The Dealer cost should <= $100,000,000";
            }
        }

        function checkSalePrice(field, rules, i, options) {
            if (parseInt($("#txtSalePrice").val().replace(/,/g, "")) > 100000000) {
                return "The Dealer cost should <= $100,000,000";
            }
        }

        function isValidEmailAddress(emailAddress) {
            var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
            return pattern.test(emailAddress);
        };

        function doneTodayBucketJumpOnProfilePage(listingId, day) {
            blockUI();
            var urlString = "/Inventory/DoneTodayBucketJump?listingId=" + listingId + "&day=" + day;
            $.ajax({
                type: "GET",
                dataType: "html",
                url: urlString,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    if (result == -1) {
                        ShowWarningMessage("You must Print Bucket Jump before Done");
                    } else {
                        $('#btnDoneTodayBucketJump').fadeOut(500);
                    }
                    unblockUI();
                },
                error: function (err) {
                    unblockUI();
                }
            });
        }

        function ShowDetailKBB(Wholesale, mileageAdjustment, baseWholesale, addsdeducts, trimID, modelID) {
            try {

                $('#divWholeSale').html(money_format(Wholesale));
                $('#divMileageAdjustment').html(money_format(mileageAdjustment));
                $('#divBaseWholesale').html(money_format(baseWholesale));
                $('#divAddDeducts').html(money_format(addsdeducts));



                var urldetail = '<%= Url.Action("GetSingleKarPowerSummary", "Market", new { listingId = "_listingID", trimId = "_trimID", modelId = "_modelID" }) %>';
                urldetail = urldetail.replace('_trimID', trimID);
                urldetail = urldetail.replace('_modelID', modelID);
                urldetail = urldetail.replace('_listingID', $('#ListingId').val());
                $('#lnkDetail').attr('href', urldetail);

            } catch (e) {
            }
        }

        function ShowDetailMainHeim(HighestPrice, AveragePrice, LowestPrice, year, make, model, trim) {
            try {
                $('#divHighestPriceManHeim').html(money_format(HighestPrice));
                $('#divAveragePriceManHeim').html(money_format(AveragePrice));
                $('#divLowestPriceManHeim').html(money_format(LowestPrice));

                var urldetail = '<%=Url.Content("~/Report/ManheimTransactionDetail?year=_Year&make=_Make&model=_Model&trim=_Trim")%>';
                urldetail = urldetail.replace('_Year', year).replace('_Make', make).replace('_Model', model).replace('_Trim', trim);
                console.log(urldetail);
                $('#lnkDetailManHeim').attr('href', urldetail);
            }
            catch (e)
            { }
        }

        $(document).ready(function () {
            $("a.printFourSquare").fancybox({ 'width': 790, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });
            $("a.autoLoanCalculator").fancybox({ 'width': 400, 'height': 400, 'hideOnOverlayClick': false, 'centerOnScroll': true });
            $("a.vehicleLog").fancybox({ 'width': 720, 'height': 400, 'hideOnOverlayClick': false, 'centerOnScroll': true });

            jQuery("#formBrochureSharing").validationEngine();
            jQuery("#formFlyerSharing").validationEngine();

            jQuery("#SaveACVForm").validationEngine();
            jQuery("#SaveDealerCostForm").validationEngine();
            jQuery("#SaveSalePriceForm").validationEngine();
            console.log(karPowerUrl);
            $.ajax({
                type: "POST",
                url: carfaxUrl,
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var carfaxId = '<%=Model.CarFaxDealerId%>';
                    var vin = '<%=Model.Vin%>';


                    var obj = {
                        "ListItems": [],
                        "CarfaxdealerId": carfaxId,
                        "Vin": vin,
                    };
                    $.each(result.ReportList, function (i, item) {
                        obj.ListItems[i] = item;
                    });

                    if (result.NumberofOwners == 0)
                        $("#carfaxOwner").html("-");
                    else
                        $("#carfaxOwner").html(result.NumberofOwners);
                    $("#carfaxRecord").html(result.ServiceRecords);

                    $("#history-report").attr("align", "");
                    $("#history-report").html($("#CarFaxTemplate").render(obj));

                },
                error: function (err) {
                    console.log('Error');
                    console.log(err.status + " - " + err.statusText);
                }
            });

            $.ajax({
                type: "POST",
                url: karPowerUrl,
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = {
                        "ListItems": []
                    };
                    $.each(result, function (i, item) {
                        obj.ListItems[i] = item;
                    });
                    $("#divPartialKbb").html($("#KBBTemplate").render(obj));
                    if (result.length > 0) {
                        $('#KBBCount').html(result[0].TotalCount == 1 && result[0].SelectedTrimId == 0 ? '' : result[0].TotalCount);
                        $("a.iframeKBB").fancybox({ 'width': 790, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                        console.log(result[0]);
                        ShowDetailKBB(result[0].Wholesale, result[0].MileageAdjustment, result[0].BaseWholesale, result[0].AddsDeducts, result[0].SelectedTrimId, result[0].SelectedModelId);
                        $('#DDLKBB').bind('change', function (e) {
                            var value = e.target.options[e.target.selectedIndex].value;
                            var arr = value.split('|');
                            ShowDetailKBB(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5]);
                            var isSold = '<%=Model.Type%>';
                            if (isSold == 4) {
                                $.ajax({
                                    type: "POST",
                                    url: "/Market/SaveSimpleKarPowerOptions?inventoryid=" + ListingId + "&trimId=" + arr[4] + "&type=SoldOut",
                                    data: {},
                                    dataType: 'html',
                                    success: function () {
                                        unblockUI();
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                        unblockUI();
                                        //alert(xhr.status + ' ' + thrownError);
                                        console.log(xhr.status + ' ' + thrownError);
                                    }
                                });
                            } else {
                                $.ajax({
                                    type: "POST",
                                    url: "/Market/SaveSimpleKarPowerOptions?inventoryid=" + ListingId + "&trimId=" + arr[4] + "&type=Inventory",
                                    data: {},
                                    dataType: 'html',
                                    success: function () {
                                        unblockUI();
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                        unblockUI();
                                        //alert(xhr.status + ' ' + thrownError);
                                        console.log(xhr.status + ' ' + thrownError);
                                    }
                                });
                            }

                        });
                    } else {
                        $('#KBBCount').html('0');
                    }
                },
                error: function (err) {
                    console.log('Error');
                    console.log(err.status + " - " + err.statusText);
                }
            });

            $.ajax({
                type: "POST",
                url: manheimUrl,
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    console.log('dasd');
                    var obj = {
                        "ListItems": []
                    };
                    $.each(result, function (i, item) {
                        obj.ListItems[i] = item;
                    });
                    $("#ManheimContent").html($("#ManHeimTemplate").render(obj));
                    if (result.length > 0) {
                        $('#ManheimCount').html(result.length);
                        $("a.iframeManHeim").fancybox({ 'width': 790, 'height': 400, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                        console.log(result);
                        ShowDetailMainHeim(result[0].HighestPrice, result[0].AveragePrice, result[0].LowestPrice, result[0].Year, result[0].MakeServiceId, result[0].ModelServiceId, result[0].TrimServiceId);

                        $('#DDLManHeim').bind('change', function (e) {
                            var value = e.target.options[e.target.selectedIndex].value;
                            var arr = value.split('|');
                            ShowDetailMainHeim(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6]);
                            var isSold = '<%=Model.Type%>';
                            if (isSold == 4) {
                                $.ajax({
                                    type: "POST",
                                    url: "/Market/SaveSimpleManheimTrim?inventoryid=" + ListingId + "&trimId=" + arr[6] + "&type=SoldOut",
                                    data: {},
                                    dataType: 'html',
                                    success: function () {
                                        unblockUI();
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                        unblockUI();
                                        //alert(xhr.status + ' ' + thrownError);
                                        console.log(xhr.status + ' ' + thrownError);
                                    }
                                });
                            } else {
                                $.ajax({
                                    type: "POST",
                                    url: "/Market/SaveSimpleManheimTrim?inventoryid=" + ListingId + "&trimId=" + arr[6] + "&type=Inventory",
                                    data: {},
                                    dataType: 'html',
                                    success: function () {
                                        unblockUI();
                                    },
                                    error: function (xhr, ajaxOptions, thrownError) {
                                        unblockUI();
                                        //alert(xhr.status + ' ' + thrownError);
                                        console.log(xhr.status + ' ' + thrownError);
                                    }
                                });
                            }
                        });
                    } else {
                        $('#ManheimCount').html('0');
                    }
                },
                error: function (err) {
                    console.log('Error');
                    console.log(err.status + " - " + err.statusText);
                }
            });

            $.ajax({
                type: "POST",
                url: auctionUrl,
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = {
                        "ListItems": []
                    };
                    $.each(result, function (i, item) {
                        obj.ListItems[i] = item;
                    });
                    $("#divPartialAuction").html($("#AuctionTemplate").render(obj));
                    if (result.length > 0) {

                        $("a.iframeManHeim").fancybox({ 'width': 790, 'height': 400, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                        $('#NationPrice').html(money_format(obj.ListItems[0].AvgAuctionPrice));
                        $('#RegionPrice').html(money_format(obj.ListItems[1].AvgAuctionPrice));
                        $('#NationOdometer').html(miles_format(obj.ListItems[0].AvgOdometer));
                        $('#RegionOdometer').html(miles_format(obj.ListItems[1].AvgOdometer));


                    }
                },
                error: function (err) {
                    console.log('Error');
                    console.log(err.status + " - " + err.statusText);
                }
            });

            $("#txtACV").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
            $("#txtDealerCost").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
            $("#txtSalePrice").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });

            $("#txtMsrp").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
            $("#txtDealerDiscount").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });

            $("#txtDealerDiscount").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
            $("#txtRetailPrice").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });

            if ($("#txtACV").val() != undefined && $("#txtACV").val() != '') $("#txtACV").val(formatDollar(Number($("#txtACV").val().replace(/[^0-9\.]+/g, ""))));
            if ($("#txtDealerCost").val() != undefined && $("#txtDealerCost").val() != '') $("#txtDealerCost").val(formatDollar(Number($("#txtDealerCost").val().replace(/[^0-9\.]+/g, ""))));
            if ($("#txtSalePrice").val() != undefined && $("#txtSalePrice").val() != '') $("#txtSalePrice").val(formatDollar(Number($("#txtSalePrice").val().replace(/[^0-9\.]+/g, ""))));

            if ($("#txtMsrp").val() != undefined && $("#txtMsrp").val() != '') $("#txtMsrp").val(formatDollar(Number($("#txtMsrp").val().replace(/[^0-9\.]+/g, ""))));
            if ($("#txtDealerDiscount").val() != undefined && $("#txtDealerDiscount").val() != '') $("#txtDealerDiscount").val(formatDollar(Number($("#txtDealerDiscount").val().replace(/[^0-9\.]+/g, ""))));

            $("#aLoadFullKBBTrims").live('click', function () {
                $('.mr-row').each(function (index) {
                    //$(this).removeClass('disable');
                    //$(this).addClass('mr-row');
                    $(this).show();
                });
            });

            $("#SaveDealerCostForm").submit(function (event) {
                event.preventDefault();

                SaveDealerCost(this);
            });

            $("#SaveACVForm").submit(function (event) {
                event.preventDefault();

                SaveACV(this);
            });

            $("#SaveSalePriceForm").submit(function (event) {
                event.preventDefault();

                SaveSalePrice(this);
            });

            $("#imgShare").live('click', function (event) {

                $(".content_new_bg").fadeIn();
            });

            $(".bg_btns_cancel").live("click", function () {
                $(".content_new_bg").fadeOut();
            });

            $(".bg_btns_save").click(function (event) {
                var emails = $('#sharedEmails').val();
                var names = $('#txtCustomerName').val();
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;


                if (!$("#txtCustomerName").validationEngine('validate') && !$("#sharedEmails").validationEngine('validate')) {
                    //$.fancybox.close();
                    var isUsed = '<%= Model.Condition%>';
                    console.log(isUsed.trim());
                    $(".content_new_bg").fadeOut();
                    blockUI();
                    $.ajax({
                        type: "Post",
                        contentType: "text/hmtl; charset=utf-8",
                        dataType: "html",
                        url: '/PDF/SendFlyer?inventoryId=' + '<%= Model.ListingId %>' + '&emails=' + emails + '&names=' + names,
                        data: {},
                        cache: false,
                        success: function (result) {
                            unblockUI();
                            ShowWarningMessage('Flyer was sent to customer successfully.');
                            $('#txtCustomerName').val('');
                            $('#sharedEmails').val('');
                        },
                        error: function (err) {
                            console.log(err.status + " - " + err.statusText);
                            unblockUI();

                        }
                    });
                }
            });

            $(".bg_btns_save_brochure").click(function (event) {
                var emails = $('#sharedEmailBrochure').val();
                var names = $('#txtCustomerNameBrochure').val();
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!$("#txtCustomerNameBrochure").validationEngine('validate') && !$("#sharedEmailBrochure").validationEngine('validate')) {
                    //$.fancybox.close();
                    var isUsed = '<%= Model.Condition%>';
                    console.log(isUsed.trim());
                    $(".content_new_bg").fadeOut();
                    blockUI();
                    $.ajax({
                        type: "POST",
                        url: '/PDF/SendBrochure?inventoryId=' + '<%= Model.ListingId %>' + '&emails=' + emails + '&names=' + names,
                        success: function (results) {
                            unblockUI();
                            console.log(results);
                            if (results.isExisted == false) {
                                ShowWarningMessage(results.message);
                                $('#txtCustomerNameBrochure').val('');
                                $('#sharedEmailBrochure').val('');
                            } else {
                                ShowWarningMessage('Brochure was sent to customer successfully.');
                                $('#txtCustomerNameBrochure').val('');
                                $('#sharedEmailBrochure').val('');
                            }
                        },
                        error: function (err) {
                            console.log(err.status + " - " + err.statusText);
                            unblockUI();
                        }
                    });
                }
            });

            var condition = '<%= Model.Condition %>';
            if (condition == '<%= Constanst.ConditionStatus.New%>') {
                $("#container_right_price").find(".profile_top_items").addClass("loPriceSmallBox");
                $("#container_right_price").find(".imgLoading").addClass("loImgLoadingSmall");
            }
        });

        function SaveDealerCost(form) {

            $.ajax({

                url: form.action,

                type: form.method,

                dataType: "json",

                data: $(form).serialize(),

                success: DealerCostSave

            });
        }

        function SaveACV(form) {

            $.ajax({

                url: form.action,

                type: form.method,

                dataType: "json",

                data: $(form).serialize(),

                success: ACVSave

            });
        }

        function SaveSalePrice(form) {

            $.ajax({

                url: form.action,

                type: form.method,

                dataType: "json",

                data: $(form).serialize(),

                success: SalePriceSave

            });
        }


        function SalePriceSave(result) {

        }

        function DealerCostSave(result) {

            // Update the page with the result
            //        var item = "<label for=\"txtLabelCostPrice\">" + result + "</label>";
            //        $("#costTitle").html(item);
            //$("#txtDealerCost").val("");
        }

        function ACVSave(result) {

            // Update the page with the result
            //        var item = "<label for=\"txtLabelAcv\">" + result + "</label>";
            //        $("#acvTitle").html(item);
            //        $("#txtACV").val("");

        }

        var tempACV = 0;
        $('#txtACV').focus(function () {
            tempACV = $(this).val();
        }).focusout(function () {
            var tempCurrentACVValue = $(this).val();
            if (tempCurrentACVValue != tempACV) {
                var ListingId = $("#ListingId").val();
                var acv = $("#txtACV").val();
                if ($(this).val() == "" || parseInt(acv.replace(/,/g, "")) <= 100000000) {
                    var img = $('#txtACV').parent().find('.imgLoading');
                    img.show();

                    $.post('<%= Url.Content("~/Inventory/UpdateACV") %>', { ListingId: ListingId, acv: acv }, function (data) {
                        img.hide();
                    });
                }
            }
        });

        var tempDealerCost = 0;
        $('#txtDealerCost').focus(function () {
            tempDealerCost = $(this).val();
        }).focusout(function () {
            var tempCurrentDealerCostValue = $(this).val();
            if (tempCurrentDealerCostValue != tempDealerCost) {
                var ListingId = $("#ListingId").val();
                var dealerCost = $("#txtDealerCost").val();
                if ($(this).val() == "" || parseInt(dealerCost.replace(/,/g, "")) <= 100000000) {
                    var img = $('#txtDealerCost').parent().find('.imgLoading');
                    img.show();

                    $.post('<%= Url.Content("~/Inventory/UpdateDealerCost") %>', { ListingId: ListingId, dealerCost: dealerCost }, function (data) {
                        img.hide();
                    });
                }
            }
        });

        var tempSalePriceValue = 0;
        $('#txtSalePrice').focus(function () {
            tempSalePriceValue = $(this).val();
        }).focusout(function () {
            if ($(this).val() == "") {
                $(this).val(0);
            }

            var tempCurrentValue = $(this).val();
            if (tempCurrentValue != tempSalePriceValue) {
                var ListingId = $("#ListingId").val();
                var salePrice = $("#txtSalePrice").val();
                var vehicleStatusCodeId = $('#hdVehicleStatusCodeId').val();
                if (parseInt(salePrice.replace(/,/g, "")) <= 100000000) {
                    var img = $('#txtSalePrice').parent().find('.imgLoading');
                    img.show();

                    $.post('<%= Url.Content("~/Inventory/UpdateSalePrice") %>', { ListingId: ListingId, SalePrice: salePrice, vehicleStatusCodeId: vehicleStatusCodeId }, function (data) {
                        img.hide();
                    });
                }
            }
        });

        //// Leo update msrp Price /////
        var tempMsrpValue = 0;
        $('#txtMsrp').focus(function () {
            tempMsrpValue = $(this).val();
        }).focusout(function () {
            if ($(this).val() == "") {
                $(this).val(0);
            }

            var tempCurrentValue = $(this).val();
            if (tempCurrentValue != tempMsrpValue) {
                var ListingId = $("#ListingId").val();
                var msrp = $("#txtMsrp").val();

                if (parseInt(msrp.replace(/,/g, "")) <= 100000000) {
                    var img = $('#txtMsrp').parent().find('.imgLoading');
                    img.show();

                    $.post('<%= Url.Content("~/Inventory/UpdateMsrp") %>', { ListingId: ListingId, msrp: msrp }, function (data) {
                        img.hide();
                    });
                }
            }
        });

        /// end update msrp Price
        //// Leo update Dealer Discount Price /////
        var tempDealerDiscountValue = 0;
        $('#txtDealerDiscount').focus(function () {
            tempDealerDiscountValue = $(this).val();
        }).focusout(function () {
            if ($(this).val() == "") {
                $(this).val(0);
            }

            var tempCurrentValue = $(this).val();
            if (tempCurrentValue != tempDealerDiscountValue) {
                var ListingId = $("#ListingId").val();
                var discount = $("#txtDealerDiscount").val();

                if (parseInt(discount.replace(/,/g, "")) <= 100000000) {
                    var img = $('#txtDealerDiscount').parent().find('.imgLoading');
                    img.show();

                    $.post('<%= Url.Content("~/Inventory/UpdateDealerDiscount") %>', { ListingId: ListingId, discount: discount }, function (data) {
                        img.hide();
                    });
                }
            }
        });

        /// end update Dealer Discount Price
        function markVehicleSold(ListingId) {
            builder.Append("<a class=\"iframe\" href=\"" + urlHelper.Content("~/Report/ViewBuyerGuide?ListingId=" + model.ListingId) + "\"><input class=\"pad_tab\"  type=\"button\" name=\"ebayCraigslist\" value=\"Ebay\" /></a>" + Environment.NewLine);
            var answer = confirm("Are you sure you want to mark this vehicle sold?");
            //console.log(ListingId);
            if (answer) {
                $.post('<%= Url.Content("~/Inventory/MarkSold") %>', { ListingId: ListingId }, function (data) {
                    if (data == "SessionTimeOut") {
                        parent.$.fancybox.close();
                        ShowWarningMessage("Your session is timed out. Please login back again");
                        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                        window.parent.location = actionUrl;
                    } else {

                        $("#Marksold").val("Removed");


                        $("#Marksold").attr("disabled", "disabled");
                    }


                });
            }
        }

        function get_check_value(param) {
            var c_value = "";
            var tmp = "";
            if (param == "options") {
                for (var i = 0; i < document.getElementsByName("selectedOptions").length; i++) {
                    if (document.getElementsByName("selectedOptions")[i].checked) {
                        tmp = document.getElementsByName("selectedOptions")[i].value;
                        c_value = c_value + tmp.substring(0, tmp.indexOf("$") - 1) + ",";
                    }
                }
            }
            else {
                for (var i = 0; i < document.getElementsByName("selectedPackages").length; i++) {
                    if (document.getElementsByName("selectedPackages")[i].checked) {
                        tmp = document.getElementsByName("selectedPackages")[i].value;
                        c_value = c_value + tmp.substring(0, tmp.indexOf("$") - 1) + ",";
                    }
                }
            }
            c_value = c_value.substring(c_value, c_value.length - 1);

            return c_value;
        }

        function openBuyerGuide(ListingId) {

            var actionUrl = '<%= Url.Action("ViewBuyerGuide", "Report", new { ListingId = "PLACEHOLDER"  } ) %>';

            actionUrl = actionUrl.replace('PLACEHOLDER', ListingId);

            window.open(actionUrl);
        }


        var pressed = 0;
        var flag = false;
        $('#txtDealerDiscount').blur(function () {
            var retailPrice = $("#txtRetailPrice").val();
            var discountPrice = $("#txtDealerDiscount").val();
            var number1 = Number(retailPrice.replace(/[^0-9\.]+/g, ""));
            var number2 = Number(discountPrice.replace(/[^0-9\.]+/g, ""));
            var total = Number(number1) - Number(number2);
            $("#txtWSSalePrice").val(formatDollar(total));
            flag = true;

        });

        $("a.image").mousedown(function () {
            pressed++;
            var t = setTimeout("pressed=0", 500);
            if (pressed == 2) {
                pressed = 0;
                $(this).fancybox();
                return false;
            }
        });

        //$("a.iframe").live('click', function (e) {
        $("a.iframe").fancybox({
            'width': 1000,
            'height': 700,
            'hideOnOverlayClick': false,
            'centerOnScroll': true,
            'onCleanup': function () {
                // reload page when closing Chart screen
                if (openChart && $("#NeedToReloadPage").val() == 'true') {
                    blockUI();
                    openChart == false;
                    window.location.href = '/Inventory/ViewIProfile?ListingID=' + ListingId;
                }
            }
        });

        $("a.iframeCommon").fancybox({ 'width': 1000, 'height': 770, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeTransfer").fancybox({ 'width': 350, 'height': 257, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeMarkSold").fancybox({ 'width': 455, 'height': 306, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeWholeSale").fancybox({ 'width': 360, 'height': 127, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeTrackingPrice").fancybox({ 'width': 870, 'height': 509, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeBucketJump").fancybox({ 'width': 929, 'height': 483, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeCustomerInfo").fancybox({ 'width': 455, 'height': 230, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.iframeStatus").fancybox({ 'margin': 0, 'padding': 0, 'width': 500, 'height': 230, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        //});

        $("div[id='postCL_Tab']").click(function (e) {
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
                            success: function (subresult) {
                                if (subresult == false)
                                    ShowWarningMessage("Please choose State/City/Location in Admin section.");
                                else {
                                    var url = '/Craigslist/GoToPostingPreviewPage?listingId=' + $('#ListingId').val();
                                    $.fancybox({
                                        href: url,
                                        'width': 600,
                                        'height': 600,
                                        'hideOnOverlayClick': false,
                                        'centerOnScroll': true,
                                        'padding': 0,
                                        'scrolling': 'no',
                                        //type: 'iframe',
                                        isLoaded: function () {
                                            //unblockUI();
                                        },
                                        onClosed: function () {
                                            //unblockUI();
                                        }
                                    });
                                }
                            }
                        });

                    } else {
                        //unblockUI();
                        ShowWarningMessage("Please enter valid Email/Password in Admin section.");
                    }
                },
                error: function (err) {

                }
            });

        });

        // fire click event to open Manheim Transaction fancybox popup
        $("a[id^='manheimRow']").live('mouseover focus', function (e) {
            $(this).fancybox({
                'width': 1000,
                'height': 700,
                'hideOnOverlayClick': false,
                'centerOnScroll': true,
                'onCleanup': function () {
                }
            });
        });

        // fire click event to open KBB Summary fancybox popup
        $("a[id^='kbbRow']").live('mouseover focus', function (e) {
            $(this).fancybox({
                'width': 1000,
                'height': 700,
                'hideOnOverlayClick': false,
                'centerOnScroll': true,
                'onCleanup': function () {
                    // reload KBB section

                    if ($("#NeedToReloadPage").val() == 'true') {
                        $("#divPartialKbb").html('<div id=\"kbb-row\" class=\"mr-row\">Loading...</div>');
                        $.ajax({
                            type: "GET",
                            url: "/Inventory/KarPowerData?listingId=" + '<%= Model.ListingId %>' + '&InventoryStatus=' + '<%= Model.CurrentScreen %>',
                            data: {},
                            dataType: 'html',
                            success: function (data) {
                                $("#divPartialKbb").html(data);
                                $("#NeedToReloadPage").val('false');
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                unblockUI();
                                ShowWarningMessage(xhr.status + ' ' + thrownError);
                            }
                        });
                    }
                }
            });
        });

        $("#fbPostingBtn").click(function () {
            $.ajax({
                type: "GET",
                url: "/Facebook/CheckCredentialExisting",
                data: {},
                dataType: 'html',
                success: function (data) {
                    if (data == "False") {
                        ShowWarningMessage("Our system shows that your dealer doesn’t have Facebook authorization on our account. Please contact vincontrol by sending email to support@vincontrol.com for setting up Facebook authorization to post on Facebook!");
                    } else {
                        AddNewPost();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    ShowWarningMessage(xhr.status + ' ' + thrownError);
                }
            });

        });

        $("#youtubePostingBtn").click(function () {
            $.ajax({
                type: "GET",
                url: "/Youtube/CheckCredentialExisting",
                data: {},
                dataType: 'html',
                success: function (data) {
                    if (data == "False") {
                        ShowWarningMessage("Our system shows that your dealer doesn’t have Youtube authorization on our account. Please contact vincontrol by sending email to support@vincontrol.com for setting up Youtube authorization to upload on Youtube!");
                    } else {
                        var url = '/Youtube/AddNewVideo?listingId=' + $("#ListingId").val() + "&inventoryStatus=" + $("#InventoryStatus").val();
                        $.fancybox({
                            href: url,
                            'type': 'iframe',
                            'width': 840,
                            'height': 410,
                            margin: 0,
                            padding: 0,
                            'scrolling': 'no',
                            'hideOnOverlayClick': true,
                            //'centerOnScroll': true,
                            'onCleanup': function () {
                            },
                            afterClose: function () {
                                if ($('#Action').val() == 'Save') {

                                }

                            }
                        });
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    ShowWarningMessage(xhr.status + ' ' + thrownError);
                }
            });
        });

        $("#optionStickerBtn").click(function () {
            var url = '/Inventory/SilentSalesman?listingId=' + $("#ListingId").val();
            $.fancybox({
                href: url,
                'type': 'iframe',
                'width': 820,
                'height': 740,
                margin: 0,
                padding: 0,
                'scrolling': 'no',
                'hideOnOverlayClick': true,
                //'centerOnScroll': true,
                'onCleanup': function () {
                },
                afterClose: function () {
                    if ($('#Action').val() == 'Save') {

                    }

                }
            });
        });

        function AddNewPost(date) {
            var url = '/Facebook/AddNewPost?listingId=' + $("#ListingId").val() + "&inventoryStatus=" + $("#InventoryStatus").val();
            $.fancybox({
                href: url,
                'type': 'iframe',
                'width': 540,
                'height': 510,
                margin: 0,
                padding: 0,
                'scrolling': 'no',
                'hideOnOverlayClick': true,
                //'centerOnScroll': true,
                'onCleanup': function () {
                },
                afterClose: function () {
                    if ($('#Action').val() == 'Save') {

                    }

                }
            });
        }

        $("a.smalliframe").fancybox();

        $('input[name="print"]').click(function () { $("#print").slideToggle("slow"); $('input[name="ebayCraigslist"]').slideToggle("slow"); $('input[name="edit"]').slideToggle("slow"); $('input[name="sold"]').slideToggle("slow"); });


        function getSelectedPackages() {
            var x = document.forms["iProfileForm"];

            var result = "";

            var totalPackages = document.getElementsByName("selectedPackages");

            for (var i = 0; i < totalPackages.length; i++) {
                if (totalPackages[i].checked) {

                    result += totalPackages[i].value.substring(0, totalPackages[i].value.indexOf("$") - 1) + ",";
                }
            }

            result = result.substring(0, result.length - 1);
            return result;

        }

        function getSelectedOptions() {
            var x = document.forms["iProfileForm"];

            var result = "";

            var totalOptions = document.getElementsByName("selectedOptions");

            for (var i = 0; i < totalOptions.length; i++) {
                if (totalOptions[i].checked)
                    result += totalOptions[i].value.substring(0, totalOptions[i].value.indexOf("$") - 1) + ",";
            }

            result = result.substring(0, result.length - 1);
            return result;

        }
        $('input[name="options"]').click(function () { $("#optionals").slideToggle("slow"); });
        $('input[name="options"]').click(function () { $("#descriptionWrapper").hide("slow"); });
        $('input[name="options"]').click(function () { $("#notes").hide("slow"); });
        $('input[name="options"]').click(function () { $("#packages").hide("slow"); });
        $('input[name="options"]').click(function () { $("#warranty").hide("slow"); });
        $('input[name="options"]').click(function () { $("#internetprice").hide("slow"); });

        $('input[name="description"]').click(function () { $("#descriptionWrapper").slideToggle("slow"); });
        $('input[name="description"]').click(function () { $("#optionals").hide("slow"); });
        $('input[name="description"]').click(function () { $("#packages").hide("slow"); });
        $('input[name="description"]').click(function () { $("#notes").hide("slow"); });
        $('input[name="description"]').click(function () { $("#warranty").hide("slow"); });
        $('input[name="description"]').click(function () { $("#internetprice").hide("slow"); });

        $('input[name="packages"]').click(function () { $("#packages").slideToggle("slow"); });
        $('input[name="packages"]').click(function () { $("#descriptionWrapper").hide("slow"); });
        $('input[name="packages"]').click(function () { $("#optionals").hide("slow"); });
        $('input[name="packages"]').click(function () { $("#notes").hide("slow"); });
        $('input[name="packages"]').click(function () { $("#warranty").hide("slow"); });
        $('input[name="packages"]').click(function () { $("#internetprice").hide("slow"); });

        $('input[name="notes"]').click(function () { $("#notes").slideToggle("slow"); });
        $('input[name="notes"]').click(function () { $("#descriptionWrapper").hide("slow"); });
        $('input[name="notes"]').click(function () { $("#optionals").hide("slow"); });
        $('input[name="notes"]').click(function () { $("#packages").hide("slow"); });
        $('input[name="notes"]').click(function () { $("#warranty").hide("slow"); });
        $('input[name="notes"]').click(function () { $("#internetprice").hide("slow"); });

        $('input[name="warranty"]').click(function () { $("#warranty").slideToggle("slow"); });
        $('input[name="warranty"]').click(function () { $("#descriptionWrapper").hide("slow"); });
        $('input[name="warranty"]').click(function () { $("#notes").hide("slow"); });
        $('input[name="warranty"]').click(function () { $("#packages").hide("slow"); });
        $('input[name="warranty"]').click(function () { $("#internetprice").hide("slow"); });
        $('input[name="warranty"]').click(function () { $("#optionals").hide("slow"); });

        $('input[name="internetprice"]').click(function () { $("#internetprice").slideToggle("slow"); });
        $('input[name="internetprice"]').click(function () { $("#descriptionWrapper").hide("slow"); });
        $('input[name="internetprice"]').click(function () { $("#optionals").hide("slow"); });
        $('input[name="internetprice"]').click(function () { $("#packages").hide("slow"); });
        $('input[name="internetprice"]').click(function () { $("#notes").hide("slow"); });
        $('input[name="internetprice"]').click(function () { $("#warranty").hide("slow"); });

        var currentheight = 0;
        window.onload = function () {
                
        };
        var shifted = 0;

        var sX;
        var sY;
        var trackXY = {};
        $("#listImages li img").each(function (index) {
            //$(this).attr("id", 'image' + (index + 1));

            $(this).mousedown(function (e) {
                trackXY.y = e.pageY;
                trackXY.x = e.pageX;
                //console.log("mousedown");
                //START YOUR DRAG STUFF
                if ($(this).hasClass('checked')) {
                    $(this).next().attr("checked", true);
                    if ($(this).attr("value") == "Upload") {
                        console.log('checked');

                        $(this).addClass('checked');
                    }

                } else {

                    $(this).next().attr("checked", false);

                    if ($(this).attr("value") == "Upload") {
                        console.log('unchecked');
                        $(this).removeClass('checked');
                    }


                }

            });
            $(this).mouseup(function (e) {

                //console.log("mouseup");
                if (trackXY.x === e.pageX && trackXY.y === trackXY.y) {

                    if ($(this).hasClass('checked')) {
                        $(this).next().attr("checked", false);


                        if ($(this).attr("value") == "Upload") {
                            console.log('unchecked');
                            $(this).removeClass('checked');
                        }


                    } else {
                        $(this).next().attr("checked", true);
                        if ($(this).attr("value") == "Upload") {
                            console.log('checked');
                            $(this).addClass('checked');
                        }

                    }
                }
            });

        });

        $("#listImages, #listImages2").dragsort({
            dragSelector: "div",
            dragBetween: true,
            dragEnd: saveOrder,
            placeHolderTemplate: "<li class='placeHolder'><div></div></li>"
        });

        function saveOrder() {
            var imageorder = "";
            var data = $("#listImages li ").map(function () {
                return $(this).children().html();
            }).get();
            $("input[name=list1SortOrder]").val(data.join(","));

            $('#listImages li img').each(function (index) {
                $("#pIdImage").attr("src", $(this).attr("src"));
                return false;


            });
            $('#listImages li img').each(function (index) {
                if ($(this).attr("value") == "Upload") {
                    imageorder += $(this).attr("src") + ",";
                }
                    
            });

            imageorder = imageorder.substring(0, imageorder.length - 1);

            $('#hiddenPhotoOrder').val(imageorder);

        };

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }

        $('#delete').click(function() {
            //console.log('You are deleteing pic');

            var flag = false;
            $('#listImages li img').each(function(index) {
                if ($(this).hasClass('checked')) {
                    flag = true;
                }
            });

            var answer;
            if (flag) {

                answer = confirm("Are you sure you want to delete selected images?");
            } else {
                ShowWarningMessage("You must select at least one image to delete");
            }

            var arrayPic = "";

            if (answer) {
                $('#listImages li img').each(function(index) {
                    if (!$(this).hasClass('checked')) {
                        //console.log(index);
                        if ($(this).attr("value") == "Upload") {

                            arrayPic += $(this).attr("src") + ",";
                        }
                        //console.log(arrayPic);

                    } else {
                        if ($(this).attr("value") == "Upload") {
                            $(this).attr("value", "Default");
                            $(this).attr("src", '<%=Url.Content("~/images/40x40grey1.jpg")%>');
                            $(this).removeClass('checked');
                        }
                    }
                });

                var count = 0;
                var topPics = new Array();
                if (arrayPic != "") {

                    arrayPic = arrayPic.substring(0, arrayPic.length - 1);

                    topPics = arrayPic.split(",");

                    count = topPics.length;
                }
                
                $('#listImages li img').each(function(index) {
                    if (count == 0) {

                        $(this).attr("value", "Default");
                        $(this).attr("src", '<%=Url.Content("~/Images/40x40grey1.jpg")%>');
                        $(this).removeClass('checked');
                    } else {
                        count--;
                        $(this).attr("value", "Upload");
                        $(this).attr("src", topPics[index]);
                        $(this).removeClass('checked');

                    }

                });

                if (topPics.length == 0)
                    $("#pIdImage").attr("src", $("#txtHiddenPhotos").val());
                else
                    $("#pIdImage").attr("src", topPics[0]);

                var inventoryStatus = $("#InventoryStatus").val();

                if (inventoryStatus == 1) {

                    $.post('<%= Url.Content("~/Inventory/DeleteCarImageURL") %>', { ListingId: $("#ListingId").val(), CarThubmnailImageURL: arrayPic }, function(data) {
                        $("#txtHiddenPhotos").val(arrayPic);
                    });
                } else if (inventoryStatus == -1) {

                    $.post('<%= Url.Content("~/Inventory/DeleteCarImageSoldURL") %>', { ListingId: $("#ListingId").val(), CarThubmnailImageURL: arrayPic }, function(data) {
                        $("#txtHiddenPhotos").val(arrayPic);
                    });
                }
            }

        });

        $('#save').click(function () {
            $('#elementID').removeClass('hideLoader');

            var ListingId = $("#ListingId").val();

            if ($("#hiddenPhotoOrder").val() != null && $("#hiddenPhotoOrder").val() != "") {

                $.post('<%= Url.Content("~/Inventory/UpdateCarImageUrl") %>', { ListingId: ListingId, CarThubmnailImageURL: $("#hiddenPhotoOrder").val() }, function (data) {
                    var actionUrl = '<%= Url.Action("ViewIProfile", "Inventory", new { ListingID = "PLACEHOLDER" } ) %>';

                    actionUrl = actionUrl.replace('PLACEHOLDER', ListingId);

                    window.location = actionUrl;
                });

            }

            $('#elementID').addClass('hideLoader');
        });

        $('a[id^=carfaxreportlive_]').live('click', function () {


            var vin = '<%=Model.Vin%>';

            var url = "/Chart/CarFaxReportFromAutoTrader?vin=" + vin;

            $.ajax({
                type: "POST",
                url: url,
                data: {},
                success: function (results) {
                    if (results == 'Error') {
                        ShowWarningMessage("Please check your carfax credentials");
                    } else {
                        var carfaxReporturl = 'http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=' + results + '&vin=' + vin;
                        window.open(
                            carfaxReporturl, 'popUpWindow', 'height=900,width=1000,left=500,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');


                    }
                  
                },

                error: function(err) {
                    ShowWarningMessage("Please check your carfax credentials");
                }

            });


        });
        
        function warrantyInfoUpdate(checkbox) {

            $.post('<%= Url.Content("~/Inventory/UpdateWarrantyInfo") %>', { WarrantyInfo: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                }
            });
        }

        function newPopup(url) {
            var popupWindow = window.open(
                url, 'popUpWindow', 'height=900,width=1000,left=500,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/plot.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/js/ui.dropdownchecklist.standalone.css")%>" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        #ddcl-trim-filter
        {
            z-index: 100;
        }

        .ui-dropdownchecklist-text
        {
            text-overflow: ellipsis;
        }

        div.ui-dropdownchecklist
        {
            height: 250px !important;
            width: 176px !important;
            left: 956px !important;
            top: 500px !important;
            color: gray !important;
            margin-left: 2px;
            padding-top: 4px;
        }

        div.ui-dropdownchecklist-dropcontainer
        {
            height: 168px;
            overflow-y: auto;
            overflow-x: hidden;
            background-color: white !important;
            border: 1px solid #cdcdcd !important;
        }

        span.ui-dropdownchecklist-selector
        {
            width: 170px !important;
        }

        .ptl_charts_right
        {
            text-align: left !important;
        }

        .content_new_bg
        {
            display: none;
            z-index: 100;
            right: 70px;
            top: 20px;
            position: absolute;
            width: 400px;
            border: 2px solid #808080;
            background-color: #CDCDCD;
        }

        .content_view_title
        {
            padding-top: 5px;
            text-align: center;
            font-weight: bold;
            border-bottom: 1px solid #808080;
            height: 25px;
        }

        .bg_edit_content .bg_add_content
        {
            background-color: #FFF;
            height: 50px;
            margin: 0px;
            padding: 8px;
            font-size: 13px;
        }

        .bg_edit_btns
        {
            height: 25px;
            padding: 10px;
        }

            .bg_edit_btns > div
            {
                padding: 5px 15px;
                cursor: pointer;
                color: #FFF;
                background-color: #5C5C5C;
                float: right;
                font-size: 13px;
                margin-right: 5px;
            }

        .formError .formErrorContent
        {
            width: 100%;
            font-style: italic;
        }

        .grid .tick
        {
            stroke: rgba(0,0,0,.3);
        }

        .axis path, .axis line
        {
            fill: none;
            stroke: black;
            shape-rendering: crispEdges;
        }

        .axis text
        {
            font-family: sans-serif;
            font-size: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    <%=Html.Partial("_TemplateCarFaxData")  %>
    <%=Html.Partial("_TemplateKarPowerData")  %>
    <%=Html.Partial("_TemplateManheimData")  %>
    <%=Html.Partial("_TemplateAuctionData")  %>
</asp:Content>