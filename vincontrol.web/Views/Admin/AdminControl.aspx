<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AdminViewModel>" %>

<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="vincontrol.DomainObject" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Administration Panel
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">

    <div id="admin_top_btns_holder">
        <% AdminUserRight userRight = SessionHandler.UserRight.Admin; %>

        <% if (userRight.Content) %>
        <% { %>
        <div id="admin_content_tab" class="admin_top_btns">
            Content
        </div>
        <% } %>

        <% if (userRight.Notifications) %>
        <% { %>
        <div id="admin_notifications_tab" class="admin_top_btns">
            Notifications
        </div>
        <% } %>

        <% if (userRight.UserRights) %>
        <% { %>
        <div id="admin_userRights_tab" class="admin_top_btns">
            User Rights
        </div>
        <% } %>

        <% if (userRight.Rebate) %>
        <% { %>
        <div id="admin_rebate_tab" class="admin_top_btns">
            Rebate
        </div>
        <% } %>

        <% if (userRight.Credentials) %>
        <% { %>
        <div id="admin_credentials_tab" class="admin_top_btns">
            Credentials
        </div>
        <% } %>

        <% if (userRight.StockingGuide) %>
        <% { %>
        <div id="admin_stockingguide_tab" class="admin_top_btns">
            Appraisals Settings
        </div>
        <% } %>
    </div>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <%
        Html.BeginForm("UpdateContentSetting", "Admin", FormMethod.Post,
                       new { id = "adminForm", name = "adminForm", enctype = "multipart/form-data" });
    %>
    <div class="add_user_holder profile_popup">
    </div>
    <input type="hidden" id="currentTab" />
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="admin_fixed_title">
                Administration Panel
            </div>
            <div id="SaveChanges_holder">
                <div style="border: none;" class="save btns_shadow admin_save_change" id="saveBtn"
                    name="UpdateSetting">
                    Save Changes
                </div>
            </div>
        </div>
    </div>
    <div id="container_right_content">
        <div class="admin_tab_holder" id="admin_stockingguide_holder" style="display: block;">
            <div class="admin_stockingguide_brand_average">
                <div style="margin-top: 0px; height: 10px;"></div>
                <%--<div class="admin_stockingguide_brand_holder">
                    <div class="admin_stockingguide_brand_title">
                        Brand
                    </div>
                    <div class="admin_stockingguide_brand_list">
                        <%= Html.DynamicHtmlLabelAdmin("StockingGuide_BrandList") %>
                    </div>
                </div>--%>
                <div class="admin_stockingguide_average_holder">
                    <form id="vehicleForm">
                        <input type="hidden" id="SelectedBrandName" name="SelectedBrandName" />
                        <div>
                            <span></span>
                            <span class="smallspan"></span>
                            <span class="span1s">Cost</span>
                            <span class="span1s">Retail</span>
                        </div>
                        <div>
                            <span>Tire</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="vehicleTireCost" name="vehicleTireCost" value="<%=Model.DealerSetting.VehicleTireCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[vehicleTireCost],funcCall[validateValueIfCostFilledIn[vehicleTireCost]]]" data-errormessage-value-missing="Tire is required!" id="vehicleTireRetail" value="<%=Model.DealerSetting.VehicleTireRetail %>" />
                        </div>

                        <div>
                            <span>Light/bulb</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="lightBulbCost" name="lightBulbCost" value="<%=Model.DealerSetting.LightBulbCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[lightBulbCost],funcCall[validateValueIfCostFilledIn[lightBulbCost]]]" data-errormessage-value-missing="Light Bulb Retail is required!" id="lightBulbRetail" value="<%=Model.DealerSetting.LightBulbRetail %>" />

                        </div>

                        <div>
                            <span>Front Wind Shield</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="frontWindShieldCost" name="frontWindShieldCost" value="<%=Model.DealerSetting.FrontWindShieldCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[frontWindShieldCost],funcCall[validateValueIfCostFilledIn[frontWindShieldCost]]]" data-errormessage-value-missing="Front Wind Shield is required!" id="frontWindShieldRetail" value="<%=Model.DealerSetting.FrontWindShieldRetail %>" />
                        </div>
                        <div>
                            <span>Rear Wind Shield</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="rearWindShieldCost" name="rearWindShieldCost" value="<%=Model.DealerSetting.RearWindShieldCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[rearWindShieldCost],funcCall[validateValueIfCostFilledIn[rearWindShieldCost]]]" data-errormessage-value-missing="Rear Wind Shield is required!" id="rearWindShieldRetail" value="<%=Model.DealerSetting.RearWindShieldRetail %>" />
                        </div>
                        <div>
                            <span>Driver Window</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="driverWindowCost" name="driverWindowCost" value="<%=Model.DealerSetting.DriverWindowCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[driverWindowCost],funcCall[validateValueIfCostFilledIn[driverWindowCost]]]" data-errormessage-value-missing="Driver Window is required!" id="driverWindowRetail" value="<%=Model.DealerSetting.DriverWindowRetail %>" />
                        </div>
                        <div>
                            <span>Passenger Window</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="passengerWindowCost" name="passengerWindowCost" value="<%=Model.DealerSetting.PassengerWindowCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[passengerWindowCost],funcCall[validateValueIfCostFilledIn[passengerWindowCost]]]" data-errormessage-value-missing="Passenger Window is required!" id="passengerWindowRetail" value="<%=Model.DealerSetting.PassengerWindowRetail %>" />

                        </div>
                        <div>
                            <span>Driver side Mirror</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="driverSideMirrorCost" name="driverSideMirrorCost" value="<%=Model.DealerSetting.DriverSideMirrorCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[driverSideMirrorCost],funcCall[validateValueIfCostFilledIn[driverSideMirrorCost]]]" data-errormessage-value-missing="Driver side Mirror is required!" id="driverSideMirrorRetail" value="<%=Model.DealerSetting.DriverSideMirrorRetail %>" />

                        </div>
                        <div>
                            <span>Passenger side Mirror</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="passengerSideMirrorCost" name="passengerSideMirrorCost" value="<%=Model.DealerSetting.PassengerSideMirrorCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[passengerSideMirrorCost],funcCall[validateValueIfCostFilledIn[passengerSideMirrorCost]]]" data-errormessage-value-missing="Passenger side Mirror is required!" id="passengerSideMirrorRetail" value="<%=Model.DealerSetting.PassengerSideMirrorRetail %>" />

                        </div>
                        <div>
                            <span>Scratch</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="scratchCost" name="scratchCost" value="<%=Model.DealerSetting.ScratchCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[scratchCost],funcCall[validateValueIfCostFilledIn[scratchCost]]]" data-errormessage-value-missing="Scratch is required!" id="scratchRetail" value="<%=Model.DealerSetting.ScratchRetail %>" />

                        </div>
                        <div>
                            <span>Major Scratch</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="majorScratchCost" name="majorScratchCost" value="<%=Model.DealerSetting.MajorScratchCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[majorScratchCost],funcCall[validateValueIfCostFilledIn[majorScratchCost]]]" data-errormessage-value-missing="Major Scratch is required!" id="majorScratchRetail" value="<%=Model.DealerSetting.MajorScratchRetail %>" />

                        </div>

                        <div>
                            <span>Dent</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="dentCost" name="dentCost" value="<%=Model.DealerSetting.DentCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[dentCost],funcCall[validateValueIfCostFilledIn[dentCost]]]" data-errormessage-value-missing="Dent is required!" id="dentRetail" value="<%=Model.DealerSetting.DentRetail %>" />

                        </div>

                        <div>
                            <span>Major Dent</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="majorDentCost" name="majorDentCost" value="<%=Model.DealerSetting.MajorDentCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[majorDentCost],funcCall[validateValueIfCostFilledIn[majorDentCost]]]" data-errormessage-value-missing="Major Dent is required!" id="majorDentRetail" value="<%=Model.DealerSetting.MajorDentRetail %>" />

                        </div>

                        <div>
                            <span>Paint Damage</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="paintDamageCost" name="paintDamageCost" value="<%=Model.DealerSetting.PaintDamageCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[paintDamageCost],funcCall[validateValueIfCostFilledIn[paintDamageCost]]]" data-errormessage-value-missing="Paint Damage is required!" id="paintDamageRetail" value="<%=Model.DealerSetting.PaintDamageRetail %>" />

                        </div>

                        <div>
                            <span>Repainted Panel</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="repaintedPanelCost" name="repaintedPanelCost" value="<%=Model.DealerSetting.RepaintedPanelCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[repaintedPanelCost],funcCall[validateValueIfCostFilledIn[repaintedPanelCost]]]" data-errormessage-value-missing="Repainted Panel is required!" id="repaintedPanelRetail" value="<%=Model.DealerSetting.RepaintedPanelRetail %>" />

                        </div>

                        <div>
                            <span>Rust</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="rustCost" name="rustCost" value="<%=Model.DealerSetting.RustCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[rustCost],funcCall[validateValueIfCostFilledIn[rustCost]]]" data-errormessage-value-missing="Rust is required!" id="rustRetail" value="<%=Model.DealerSetting.RustRetail %>" />

                        </div>

                        <div>
                            <span>Acident</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="acidentCost" name="acidentCost" value="<%=Model.DealerSetting.AcidentCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[acidentCost],funcCall[validateValueIfCostFilledIn[acidentCost]]]" data-errormessage-value-missing="Acident is required!" id="acidentRetail" value="<%=Model.DealerSetting.AcidentRetail %>" />

                        </div>

                        <div>
                            <span>Missing parts</span>
                            <span class="smallspan"></span>

                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                id="missingPartsCost" name="missingPartsCost" value="<%=Model.DealerSetting.MissingPartsCost %>" data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            <input type="text" onkeyup="formatCurrency(event, this)" maxlength="12" onkeypress="validateInput(event)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue],condRequired[missingPartsCost],funcCall[validateValueIfCostFilledIn[missingPartsCost]]]" data-errormessage-value-missing="Missing parts is required!" id="missingPartsRetail" value="<%=Model.DealerSetting.MissingPartsRetail %>" />

                        </div>

                        <div>
                            <span>Average Recon Cost</span>
                            <span class="smallspan"></span>

                            <input class="span2s" type="text" id="avgCost" maxlength="12" value="<%=Model.DealerSetting.AverageCost %>" onkeypress="validateInput(event)" onkeyup="formatCurrency(event, this)"
                                data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                        </div>
                        <div>
                            <span>Average Desired Profit</span>


                            <input class="smallspan" type="radio" id="rdAvgProfit" name="profitGroup" onclick="setProfitUsage(1)" />
                            <div onclick="setProfitUsage(1)" style="display: inline-block">
                                <input class="span2s" type="text" id="avgProfit" maxlength="12" value="<%=Model.DealerSetting.AverageProfit %>" onkeypress="validateInput(event)" onkeyup="formatCurrency(event, this)"
                                    data-validation-engine="validate[funcCall[checkCurrencyValue]]" />
                            </div>

                        </div>
                        <div>
                            <span></span>
                            <span style="text-align: center">-Or-
                            </span>

                        </div>
                        <div>
                            <span></span>
                            <%--<label> </label>--%>
                            <input class="smallspan" type="radio" id="rdAvgProfitPercent" name="profitGroup" onclick="setProfitUsage(2)" />
                            <div onclick="setProfitUsage(2)" style="display: inline-block">
                                <input type="text" class="span2s" id="avgProfitPercent" value="<%=Model.DealerSetting.AverageProfitPercentage %>%" />
                            </div>

                        </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="admin_tab_holder" id="admin_credentials_holder" style="display: block;">
            <div class="admin_3rdparty_header">
                Third Party Site Login Credentials
            </div>
            <div class="admin_3rdparty_title">
                This area gives you the ability to access 3rd party web resources from VIN Control.
                It also provides additional information for your appraisals and profile pages. All
                information stored here is secure and will not be shared.
            </div>
            <div class="admin_3rdparty_items_holder">
                <div class="admin_3rdparty_items">
                    <div class="admin_3rdparty_items_text">
                        CarFax
                    </div>
                    <div class="admin_3rdparty_items_name">
                        <%= Html.EditorFor(x => x.DealerSetting.CarFax) %>
                    </div>
                    <div class="admin_3rdparty_items_pass">
                        <%= Html.EditorFor(x => x.EncodeCarFaxPassword) %>
                        <%= Html.HiddenFor(x => x.CarFaxPasswordChanged) %>
                        <input type="hidden" id="hdnOldCarFax" />
                    </div>
                </div>
                <div class="admin_3rdparty_items">
                    <div class="admin_3rdparty_items_text">
                        Manheim
                    </div>
                    <div class="admin_3rdparty_items_name">
                        <%= Html.EditorFor(x => x.DealerSetting.Manheim) %>
                    </div>
                    <div class="admin_3rdparty_items_pass">
                        <%= Html.EditorFor(x => x.EncodeManheimPassword) %>
                        <%= Html.HiddenFor(x => x.ManheimPasswordChanged) %>
                        <input type="hidden" id="hdnOldManheim" />
                    </div>
                </div>
                <div class="admin_3rdparty_items">
                    <div class="admin_3rdparty_items_text">
                        Kelly Blue Book
                    </div>
                    <div class="admin_3rdparty_items_name">
                        <%= Html.EditorFor(x => x.DealerSetting.KellyBlueBook) %>
                    </div>
                    <div class="admin_3rdparty_items_pass">
                        <%= Html.EditorFor(x => x.EncodeKellyPassword) %>
                        <%= Html.HiddenFor(x => x.KellyPasswordChanged) %>
                        <input type="hidden" id="hdnOldKBB" />
                    </div>
                </div>
                <div class="admin_3rdparty_items">
                    <div class="admin_3rdparty_items_text">
                        Black Book Online
                    </div>
                    <div class="admin_3rdparty_items_name">
                        <%= Html.EditorFor(x => x.DealerSetting.BlackBook) %>
                    </div>
                    <div class="admin_3rdparty_items_pass">
                        <%= Html.EditorFor(x => x.EncodeBlackBookPassword) %>
                        <%= Html.HiddenFor(x => x.BlackBookPasswordChanged) %>
                        <input type="hidden" id="hdnOldBB" />
                    </div>
                </div>
                <div class="admin_3rdparty_items">
                    <div class="admin_3rdparty_items_text">
                        Craigslist
                    </div>
                    <div style="display: inline-block; width: 80%;">
                        <div style="display: inline-block; font-size: 12px; width: 100%;">
                            <div class="admin_3rdparty_items_name" style="float: left;">
                                <%= Html.EditorFor(x => x.DealerCraigslistSetting.Email) %>
                            </div>
                            <div class="admin_3rdparty_items_pass">
                                <input class="text-box single-line password" id="DealerCraigslistSetting_EncodePassword" name="DealerCraigslistSetting.EncodePassword" type="password" value="<%= Model.DealerCraigslistSetting.EncodePassword %>" />
                                <%= Html.HiddenFor(x => x.DealerCraigslistSetting.PasswordChanged) %>
                                <input type="hidden" id="hdnOldCraigslist" />
                            </div>
                        </div>
                        <div id="divCraiglistSetting" style="display: inline-block; font-size: 12px; width: 100%;">
                            <%--<div style="text-align: center;"><img src="../../images/ajaxloadingindicator.gif" /></div>--%>
                        </div>
                        <div style="display: inline-block; font-size: 12px; width: 100%;">
                            <textarea rows="10" cols="80" id="DealerCraigslistSetting_EndingSentence" name="DealerCraigslistSetting.EndingSentence"><%= (Model.DealerCraigslistSetting.EndingSentence)%></textarea>
                        </div>
                        <input type="hidden" id="DealerCraigslistSetting_State" name="DealerCraigslistSetting.State" value="<%= Model.DealerCraigslistSetting.State %>" />
                        <input type="hidden" id="DealerCraigslistSetting_City" name="DealerCraigslistSetting.City" value="<%= Model.DealerCraigslistSetting.City %>" />
                        <input type="hidden" id="DealerCraigslistSetting_CityUrl" name="DealerCraigslistSetting.CityUrl" value="<%= Model.DealerCraigslistSetting.CityUrl %>" />
                        <input type="hidden" id="DealerCraigslistSetting_Location" name="DealerCraigslistSetting.Location" value="<%= Model.DealerCraigslistSetting.Location %>" />
                        <input type="hidden" id="DealerCraigslistSetting_LocationId" name="DealerCraigslistSetting.LocationId" value="<%= Model.DealerCraigslistSetting.LocationId %>" />
                    </div>
                </div>
            </div>
        </div>
        <div class="admin_tab_holder" id="admin_content_holder" style="display: block;">
            <div id="Div3">
                <div style="margin-top: 0px; height: 10px;">
                </div>
                <div class="admin_content_dealerinfo">
                    <div class="admin_content_info ac_dealer_info">
                        <div class="admin_content_info_title">
                            Dealer Info
                        </div>
                        <div class="admin_content_info_text">
                            <label>
                                <%= (Model.DealershipName) %></label>
                            <label>
                                <%= (Model.Phone) %></label>
                            <label class="ac_dealerinfo_address">
                                <%= (Model.Address) %></label>
                            <label>
                                <%= (Model.City) %>,
                                <%= (Model.State) %>,
                                <%= (Model.ZipCode) %></label>
                            <label>
                                <%= (Model.Email) %></label>
                            <div style="display: block; width: 80%; margin: 0px auto; font-size: 14px; padding-top: 5px;">
                                <div style="float: left; padding-top: 5px;">Brand:</div>
                                <div style="float: left; padding-left: 5px; width: 180px;">
                                    <select id="DDLFilterModel" multiple="multiple" style="height: 22px; width: 180px;">
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="admin_content_info ac_preferences" style="width: 35%;">
                        <div class="admin_content_info_title">
                            Preferences
                            <img src="<%= Url.Content("~/Content/images/vincontrol/quote.jpg") %>" title="Choose how VINControl sorts your inventory by default, how to display your sold car if we manage your website, and a fixed price subtraction for your dealership if your website uses our Trade In Appraisal Calculator." />
                        </div>
                        <div class="admin_content_info_text">
                            <div class="admin_content_prefer_items">
                                <label>
                                    Default Inventory Setting</label>
                                <%= Html.DropDownListFor(x => x.DealerSetting.InventorySorting, Model.SortSetList,
                                             new {id = "ac_pre_defaulsetting"}) %>
                                <%--    <select id="">
                                    <option>Age</option>
                                </select>--%>
                            </div>
                            <div class="admin_content_prefer_items">
                                <label>
                                    Select Sold Action</label>
                                <%= Html.DropDownListFor(x => x.DealerSetting.SoldOut, Model.SoldActionList) %>
                            </div>
                            <div class="admin_content_prefer_items">
                                <label>
                                    Trade In Price Variance</label>
                                <%--<%= Html.TextBoxFor(x => x.DealerSetting.PriceVariance) %>
                                <%= Html.ValidationMessage("PriceVariance", "*") %>--%>
                                <input type="text" id="DealerSetting_PriceVariance" name="DealerSetting.PriceVariance" value="<%=Model.DealerSetting.PriceVariance %>" data-validation-engine="validate[funcCall[checkVariancePrice]]" />
                            </div>
                        </div>
                    </div>
                    <div class="admin_content_info" style="width: 35%;">
                        <div class="admin_content_info_title">
                            Override Stock Image
                            <img src="<%= Url.Content("~/Content/images/vincontrol/quote.jpg") %>" title="Upload your own personalized image to take the place the stock image we display for cars without photos. Note: we will send this image in the place of nothing at all in our outgoing feeds as well.">
                        </div>
                        <div class="admin_content_info_text">
                            <div class="ac_uploadimg_left">
                                <small>Uploaded files cannot exceed 500kb and can be up to 500px wide and 500px tall</small>
                                <div class="ac_uploadimg_btns_holder">
                                    <%--<%= Html.DynamicHtmlLabelAdmin("OverrideStockImage") %>--%>
                                    <input type="checkbox" name="on" onclick="javascript:OverideStockImage(this);">
                                    <a id="btnUploadImage" name="btnUploadImage" href="javascript:void(0);">
                                        <input disabled="disabled" type="button" value="Upload" style="padding: 4px 14px; background-color: #333; color: White; box-shadow: 3px 2px 3px #333" /></a>
                                    <div id="uploadedMessage" style="font-size: 12px;">
                                    </div>
                                </div>
                            </div>
                            <div class="ac_uploadimg_right">
                                <div class="ac_uploadimg_img">
                                    <% if (!String.IsNullOrEmpty(Model.DealerSetting.DefaultStockImageUrl))
                                       { %>
                                    <img id="DefaultStockImage" width="94" style="max-height: 90px;" src="<%= Model.DealerSetting.DefaultStockImageUrl %>" />
                                    <% }
                                       else
                                       { %>
                                    <img src="/images/image_null.png">
                                    <% } %>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="admin_content_ws_des">
                    <div class="admin_content_ws_holder">
                        <div class="admin_content_info_title">
                            Window Stickers
                            <img src="<%= Url.Content("~/Content/images/vincontrol/quote.jpg") %>" title="Choose custom titles to take the place of price fields printed on your window stickers. Uncheck the boxes to not display those fields at all when printed.">
                        </div>
                        <div class="admin_content_info_text">
                            <div class="ac_ws_title">
                                Display on Sticker
                            </div>
                            <div class="ac_ws_list">
                                <div class="ac_ws_items">
                                    <%= Html.DynamicHtmlLabelAdmin("RetailPriceWSNotification") %>
                                    <%= Html.TextBoxFor(x => x.DealerSetting.RetailPriceWSNotificationText, new { MaxLength = 30 }) %>
                                    <label>
                                        Retail Price</label>
                                </div>
                                <div class="ac_ws_items">
                                    <%= Html.DynamicHtmlLabelAdmin("DealerDiscountWSNotification") %>
                                    <%= Html.TextBoxFor(x => x.DealerSetting.DealerDiscountWSNotificationText, new { MaxLength = 30 }) %>
                                    <label>
                                        Dealer Discount</label>
                                </div>
                                <div class="ac_ws_items">
                                    <%= Html.DynamicHtmlLabelAdmin("ManufacturerReabateNotification") %>
                                    <%= Html.TextBoxFor(x => x.DealerSetting.ManufacturerReabteWsNotificationText, new { MaxLength = 30 }) %>
                                    <label>
                                        Manufacturer</label>
                                </div>
                                <div class="ac_ws_items">
                                    <%= Html.DynamicHtmlLabelAdmin("SalePriceNotification") %>
                                    <%= Html.TextBoxFor(x => x.DealerSetting.SalePriceWsNotificationText, new { MaxLength = 30 }) %>
                                    <label>
                                        Sale Price</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="admin_content_des_holder">
                        <div class="admin_content_info_title">
                            Description
                            <img src="<%= Url.Content("~/Content/images/vincontrol/quote.jpg") %>" title="Add sentences that will populate on all your descriptions, sentences that will only populate on Auction vehicles, or information to be populated on ads you post to eBay.">
                        </div>
                        <div class="admin_content_info_text">
                            <div class="ac_des_left">
                                <div class="ac_des_left_title">
                                    Ebay/Craigslist Misc Information
                                </div>
                                <%-- <div class="ac_ws_title">
                                    Dealer Contact Info
                                </div>--%>
                                <div class="ac_ws_list">
                                    <div class="ac_ws_items">
                                        <%=Html.Label("Name ") %><%= Html.TextBoxFor(x => x.DealerSetting.EbayContactInfoName, new { MaxLength = 50 }) %>
                                    </div>
                                    <div class="ac_ws_items">
                                        <%=Html.Label("Phone") %><%= Html.TextBoxFor(x => x.DealerSetting.EbayContactInfoPhone, new { MaxLength = 50 }) %>
                                    </div>
                                    <div class="ac_ws_items">
                                        <%=Html.Label("Email") %>
                                        <%--<%= Html.EditorFor(x => x.DealerSetting.EbayContactInfoEmail) %>--%>
                                        <input type="text" id="DealerSetting_EbayContactInfoEmail" name="DealerSetting.EbayContactInfoEmail" maxlength="50" class="text-box single-line" value="<%=Model.DealerSetting.EbayContactInfoEmail %>" data-validation-engine="validate[funcCall[CheckEmail]]" />
                                    </div>
                                </div>
                            </div>
                            <div class="ac_des_right">
                                <div class="ac_des_right_top">
                                    <div class="ac_des_right_items">
                                        <div class="acd_right_items_left">
                                            Dealer Warranty
                                        </div>
                                        <div class="acd_right_items_right">
                                            <div class="acd_right_btns acd_right_btns_view">
                                                View
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_edit">
                                                Edit
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_clear" id="CleardealerWarranty_<%= Constanst.DescriptionSettingCategory.DealerWarranty %>">
                                                Clear
                                            </div>
                                        </div>
                                        <div class="content_view">
                                            <div class="content_view_title">
                                                Dealer Warranty
                                            </div>
                                            <div class="content_view_content">
                                                <p>
                                                    <%= Html.DisplayTextFor(x => x.DealerSetting.DealerWarranty) %>
                                                </p>
                                            </div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content_edit">
                                            <div class="content_view_title">
                                                Dealer Warranty
                                            </div>
                                            <div class="content_edit_content">
                                                <%= Html.TextAreaFor(x => x.DealerSetting.DealerWarranty,new {maxlength = "1000"}) %>
                                            </div>
                                            <div style="padding-left: 200px; padding-top: 10px;">Max length is 1000 chars</div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_save" id="DealerWarranty_<%= Constanst.DescriptionSettingCategory.DealerWarranty %>">
                                                    Save
                                                </div>
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="ac_des_right_items">
                                        <div class="acd_right_items_left">
                                            Shipping Details
                                        </div>
                                        <div class="acd_right_items_right">
                                            <div class="acd_right_btns acd_right_btns_view">
                                                View
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_edit">
                                                Edit
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_clear" id="ClearShippingInfo_<%= Constanst.DescriptionSettingCategory.ShippingInfo %>">
                                                Clear
                                            </div>
                                        </div>
                                        <div class="content_view">
                                            <div class="content_view_title">
                                                Shipping Details
                                            </div>
                                            <div class="content_view_content">
                                                <p>
                                                    <%= Html.DisplayTextFor(x => x.DealerSetting.ShippingInfo) %>
                                                </p>
                                            </div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content_edit">
                                            <div class="content_view_title">
                                                Shipping Details
                                            </div>
                                            <div class="content_edit_content">
                                                <%= Html.TextAreaFor(x => x.DealerSetting.ShippingInfo,new {maxlength = "1000"}) %>
                                            </div>
                                            <div style="padding-left: 200px; padding-top: 10px;">Max length is 1000 chars</div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_save" id="ShippingInfo_<%= Constanst.DescriptionSettingCategory.ShippingInfo %>">
                                                    Save
                                                </div>
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="ac_des_right_items">
                                        <div class="acd_right_items_left">
                                            Terms and Conditions
                                        </div>
                                        <div class="acd_right_items_right">
                                            <div class="acd_right_btns acd_right_btns_view">
                                                View
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_edit">
                                                Edit
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_clear" id="ClearTermConditon_<%= Constanst.DescriptionSettingCategory.TermConditon %>">
                                                Clear
                                            </div>
                                        </div>
                                        <div class="content_view">
                                            <div class="content_view_title">
                                                Terms and Conditions
                                            </div>
                                            <div class="content_view_content">
                                                <p>
                                                    <%= Html.DisplayTextFor(x => x.DealerSetting.TermConditon) %>
                                                </p>
                                            </div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content_edit">
                                            <div class="content_view_title">
                                                Terms and Conditions
                                            </div>
                                            <div class="content_edit_content">
                                                <%= Html.TextAreaFor(x => x.DealerSetting.TermConditon,new {maxlength = "5000"}) %>
                                            </div>
                                            <div style="padding-left: 200px; padding-top: 10px;">Max length is 5000 chars</div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_save" id="TermConditon_<%= Constanst.DescriptionSettingCategory.TermConditon %>">
                                                    Save
                                                </div>
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="ac_des_right_bottom">
                                    <div class="ac_ws_title" style="font-weight: bold">
                                        Auto Description
                                    </div>
                                    <div class="ac_des_right_items">
                                        <div class="acd_right_items_left">
                                            Starting Sentence
                                        </div>
                                        <div class="acd_right_items_right">
                                            <div class="acd_right_btns acd_right_btns_view">
                                                View
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_edit">
                                                Edit
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_clear" id="ClearStartSentence_<%= Constanst.DescriptionSettingCategory.StartSentence %>">
                                                Clear
                                            </div>
                                        </div>
                                        <div class="content_view">
                                            <div class="content_view_title">
                                                Starting Sentence
                                            </div>
                                            <div class="content_view_content">
                                                <p>
                                                    <%= Html.DisplayTextFor(x => x.DealerSetting.StartSentence) %>
                                                </p>
                                            </div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content_edit">
                                            <div class="content_view_title">
                                                Starting Sentence
                                            </div>
                                            <div class="content_edit_content">
                                                <%= Html.TextAreaFor(x => x.DealerSetting.StartSentence,new {maxlength = "1000"}) %>
                                            </div>
                                            <div style="padding-left: 200px; padding-top: 10px;">Max length is 1000 chars</div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_save" id="StartSentence_<%= Constanst.DescriptionSettingCategory.StartSentence %>">
                                                    Save
                                                </div>
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="ac_des_right_items">
                                        <div class="acd_right_items_left">
                                            Ending Sentence
                                        </div>
                                        <div class="acd_right_items_right">
                                            <div class="acd_right_btns acd_right_btns_view">
                                                View
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_ending_edit">
                                                Edit
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_clear" id="ClearEndSentence_<%= Constanst.DescriptionSettingCategory.EndSentence %>">
                                                Clear
                                            </div>
                                        </div>
                                        <div class="content_view" style="width: 400px!important;">
                                            <div style="width: 190px; margin-left: 5px; float: left">
                                                <div class="content_view_title">
                                                    Used - Ending Sentence
                                                </div>
                                                <div class="content_view_content" id="endingUsed">
                                                    <p>
                                                        <%= Html.DisplayTextFor(x => x.DealerSetting.EndSentence) %>
                                                    </p>
                                                </div>
                                            </div>
                                            <div style="width: 195px; margin-left: 5px; float: left">
                                                <div class="content_view_title">
                                                    New - Ending Sentence
                                                </div>
                                                <div class="content_view_content" id="endingNew">
                                                    <p>
                                                        <%= Html.DisplayTextFor(x => x.DealerSetting.EndSentenceForNew) %>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_cancel" style="margin-bottom: 10px; margin-top: 10px;">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content_edit">
                                            <div style="width: 290px; margin-left: 5px; float: left">
                                                <div class="content_view_title">
                                                    Used - Ending Sentence
                                                </div>
                                                <div class="content_edit_content">
                                                    <%= Html.TextAreaFor(x => x.DealerSetting.EndSentence,new {maxlength = "1000"}) %>
                                                </div>
                                            </div>
                                            <div style="width: 295px; margin-left: 5px; float: left">
                                                <div class="content_view_title">
                                                    New - Ending Sentence
                                                </div>
                                                <div class="content_edit_content">
                                                    <%= Html.TextAreaFor(x => x.DealerSetting.EndSentenceForNew,new {maxlength = "1000"}) %>
                                                </div>
                                            </div>
                                            <div style="width: 373px; float: left">
                                                <div style="padding-left: 200px; padding-top: 10px;">Max length is 1000 chars</div>
                                                <div class="content_edit_btns">
                                                    <div class="btns_shadow content_btns_ending_save" id="EndSentence_<%= Constanst.DescriptionSettingCategory.EndSentence %>">
                                                        Save
                                                    </div>
                                                    <div class="btns_shadow content_btns_cancel">
                                                        Cancel
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="ac_des_right_items">
                                        <div class="acd_right_items_left">
                                            Auction Sentence
                                        </div>
                                        <div class="acd_right_items_right">
                                            <div class="acd_right_btns acd_right_btns_view">
                                                View
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_edit">
                                                Edit
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_clear" id="ClearAuctionSentence_<%= Constanst.DescriptionSettingCategory.AuctionSentence %>">
                                                Clear
                                            </div>
                                        </div>
                                        <div class="content_view">
                                            <div class="content_view_title">
                                                Auction Sentence
                                            </div>
                                            <div class="content_view_content">
                                                <p>
                                                    <%= Html.DisplayTextFor(x => x.DealerSetting.AuctionSentence) %>
                                                </p>
                                            </div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content_edit">
                                            <div class="content_view_title">
                                                Auction Sentence
                                            </div>
                                            <div class="content_edit_content">
                                                <%= Html.TextAreaFor(x => x.DealerSetting.AuctionSentence,new {maxlength = "1000"}) %>
                                            </div>
                                            <div style="padding-left: 200px; padding-top: 10px;">Max length is 1000 chars</div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_save" id="AuctionSentence_<%= Constanst.DescriptionSettingCategory.AuctionSentence %>">
                                                    Save
                                                </div>
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="ac_des_right_items">
                                        <div class="acd_right_items_left">
                                            Loaner Sentence
                                        </div>
                                        <div class="acd_right_items_right">
                                            <div class="acd_right_btns acd_right_btns_view">
                                                View
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_edit">
                                                Edit
                                            </div>
                                            <div class="acd_right_separated">
                                                |
                                            </div>
                                            <div class="acd_right_btns acd_right_btns_clear" id="ClearLoanerSentence_<%= Constanst.DescriptionSettingCategory.LoanerSentence %>">
                                                Clear
                                            </div>
                                        </div>
                                        <div class="content_view">
                                            <div class="content_view_title">
                                                Loaner Sentence
                                            </div>
                                            <div class="content_view_content">
                                                <p>
                                                    <%= Html.DisplayTextFor(x => x.DealerSetting.LoanerSentence) %>
                                                </p>
                                            </div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content_edit">
                                            <div class="content_view_title">
                                                Loaner Sentence
                                            </div>
                                            <div class="content_edit_content">
                                                <%= Html.TextAreaFor(x => x.DealerSetting.LoanerSentence,new {maxlength = "1000"})%>
                                            </div>
                                            <div style="padding-left: 200px; padding-top: 10px;">Max length is 1000 chars</div>
                                            <div class="content_edit_btns">
                                                <div class="btns_shadow content_btns_save" id="LoanerSentence_<%= Constanst.DescriptionSettingCategory.LoanerSentence %>">
                                                    Save
                                                </div>
                                                <div class="btns_shadow content_btns_cancel">
                                                    Cancel
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="admin_content_bg">
                    <div class="ptr_items_content_list partialContents" id="BuyerGuideContent">
                        <div class="data-content" align="center">
                            <img src="/content/images/ajaxloadingindicator.gif" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <% if (SessionHandler.CurrentUser.Role.Equals(RoleType.Master.ToString()) || SessionHandler.CurrentUser.Role.Equals(RoleType.Admin.ToString()))
           {%>
        <div id="admin_notifications_holder" class="admin_tab_holder">
            <div id="container_content">
                <div class="admin_notifications_title">
                    Notifications
                </div>
                <div class="au_notifications_table">
                    <div class="au_notifications_items au_notifications_header">
                        <div class="aun_type">
                            Notification Type
                        </div>
                        <div class="aun_select">
                            Select
                        </div>
                        <div class="aun_user">
                            Users
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Appraisals
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("AppraisalNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("ApprasialUsersNotificationList")%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Wholesale
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("WholeSaleNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("WholeSaleUsersNotificationList")%>
                            <%--<div id="wUserList" class="hider">
                               
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Add to Inventory
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("InventoryNotfication")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("InventoryUsersNotificationList")%>
                            <%--<div id="iUserList" class="hider">
                               
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            24H Appraisal Alert
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("TwentyFourHourNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("24HUsersNotificationList")%>
                            <%--<div id="c24hUserList" class="hider">
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Note Notifications
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("NoteNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("NoteUsersNotificationList")%>
                            <%-- <div id="nUserList" class="hider">
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Price Changes
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("PriceChangeNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("PriceUsersNotificationList")%>
                            <%--  <div id="pUserList" class="hider">
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Ageing/Bucket Jump
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("AgeingBucketJumpNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("AgeingBucketJumpUsersNotificationList")%>
                            <%--  <div id="agingUserList" class="hider">
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Bucket Jump Report
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("BucketJumpReportNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("BucketJumpReportUsersNotificationList")%>
                            <%-- <div id="bucketJumpUserList" class="hider">
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Market Price Range
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("MarketPriceRangeChangeNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("MarketPriceRangeNotificationList")%>
                            <%-- <div id="marketPriceRangeUserList" class="hider">
                            </div>--%>
                        </div>
                    </div>
                    <div class="au_notifications_items">
                        <div class="aun_type">
                            Image Upload
                        </div>
                        <div class="aun_select">
                            <%=Html.DynamicHtmlLabelAdmin("ImageUploadNotification")%>
                        </div>
                        <div class="aun_user">
                            Manage Users
                        </div>
                        <div class="admin_notifications_manage_holder">
                            <%=Html.DynamicHtmlLabelAdmin("ImageUploadNotificationList")%>
                            <%--  <div id="imageuploadUserList" class="hider">
                            </div>--%>
                        </div>
                    </div>
                </div>
                <div class="admin_notifications_title">
                    Aging/Bucket Jump
                </div>
                <div class="auageing_holder">
                    <div class="auageing_items">
                        <label>
                            First Time Range</label>
                        <%=Html.TextBoxFor(x => x.DealerSetting.FirstTimeRangeBucketJump, new {@maxlength = "3"})%>
                    </div>
                    <div class="auageing_items">
                        <label>
                            Second Time Range</label>
                        <%--<%= Html.TextBoxFor(x => x.DealerSetting.SecondTimeRangeBucketJump) %>--%>
                        <input type="text" name="DealerSetting.SecondTimeRangeBucketJump" id="DealerSetting_SecondTimeRangeBucketJump"
                            maxlength="3" value="<%=Model.DealerSetting.SecondTimeRangeBucketJump%>" data-validation-engine="validate[funcCall[checkBucketJumpRange]]" />
                    </div>
                    <div class="auageing_items">
                        <label>
                            Interval</label>
                        <%=Html.DropDownListFor(x => x.DealerSetting.IntervalBucketJump, Model.IntervalList)%>
                    </div>
                </div>
            </div>
        </div>
        <%}%>
        <% if (Model.LandingPage.Equals("UserRights") && (SessionHandler.CurrentUser.Role.Equals(RoleType.Master.ToString()) || SessionHandler.CurrentUser.Role.Equals(RoleType.Admin.ToString())))
           {%>
        <div id="userRights">
            <% }
           else
           {%>
            <div id="userRights" class="hider">
                <% }%>
                <%-- Permisson Button--%>
                <div id="admin_userrights_holder" class="admin_tab_holder">
                    <div class="admin_notifications_title">
                        Permissions
                    </div>
                    <div class="admin_ur_permissions_holder">
                        <div class="admin_ur_permissions_items">
                            <div style="text-align: center">
                                <img src="/content/images/ajaxloadingindicator.gif" />
                            </div>
                        </div>
                    </div>
                    <%-- End Permisson Button--%>
                    <div class="admin_notifications_title" style="margin-top: 20px;">
                        User Rights
                    </div>
                    <div class="admin_userrights_holder">
                        <div class="admin_userrights_headerAdd">
                            <div class="admin_userrights_items admin_userrights_header">
                                <div class="adu_collumnNew">
                                    Name
                                </div>
                                <div class="adu_collumnNew">
                                    Username
                                </div>
                                <div class="adu_collumnNew">
                                    Password
                                </div>
                                <div class="adu_collumnNew">
                                    Email
                                </div>
                                <div class="adu_collumnNew">
                                    Cell #
                                </div>
                                <div class="adu_collumnNew">
                                    User Type
                                </div>
                            </div>
                            <div class="admin_userrights_items">
                                <div class="adu_collumnNew">
                                    <input id="NewName" type="text" data-validation-engine="validate[required]" maxlength="50" data-errormessage-value-missing="Name is required!" />
                                </div>
                                <div class="adu_collumnNew">
                                    <input id="NewUsername" type="text"  maxlength="50" data-errormessage-value-missing="UserName is required!" />
                                </div>
                                <div class="adu_collumnNew">
                                    <input id="NewPassword" type="password" data-validation-engine="validate[required,funcCall[checkPass]]"
                                        data-errormessage-value-missing="Password is required!" />
                                </div>
                                <div class="adu_collumnNew">
                                    <input id="NewEmail" type="text" data-validation-engine="validate[required,funcCall[CheckEmail]]"
                                        data-errormessage-value-missing="Email is required!" maxlength="50" />
                                </div>
                                <div class="adu_collumnNew">
                                    <input id="NewPhone" type="text" maxlength="20" data-validation-engine="validate[required]" data-errormessage-value-missing="Cellphone is required!" />
                                </div>
                                <div class="adu_collumnNew">
                                    <select id="UserLevel">
                                        <option value="<%= Constanst.RoleType.Manager %>">Manager</option>
                                        <option value="<%= Constanst.RoleType.Employee %>">Employee</option>
                                        <option value="<%= Constanst.RoleType.Admin %>">Admin</option>
                                    </select>
                                </div>
                                <div>
                                    <%= Html.HiddenFor(x => x.MutipleDealer) %>
                                </div>
                                <div class="btns_shadow adu_collumn adu_addUser_btn" id="addUserBtn">
                                    Add User
                                </div>
                            </div>
                        </div>
                        <div class="admin_userrights_search_holder">
                            <div class="btns_shadow adu_search_btn" id="admin_userrights_search_btn">
                                Search
                            </div>
                            <input id="admin_userrights_search_input" type="text" />
                        </div>
                        <div class="admin_userrights_list" id="admin_userlist">
                            <div class="data-content" align="center">
                                <img src="/content/images/ajaxloadingindicator.gif" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="admin_rebates_holder" class="admin_tab_holder" style="overflow: hidden; overflow-y: auto">
                    <div class="admin_notifications_title">
                        Rebates
                        <input type="hidden" id="hdRebateIsUp" value="true" />
                    </div>
                    <div class="admin_rebates_table">
                        <div class="admin_rebates_items admin_rebates_header">
                            <div class="admin_rebates_collumn">
                                <a onclick="ShortRebate(1);" style="cursor: pointer">Year </a>
                            </div>
                            <div class="admin_rebates_collumn">
                                <a onclick="ShortRebate(2);" style="cursor: pointer">Make </a>
                            </div>
                            <div class="admin_rebates_collumn">
                                <a onclick="ShortRebate(3);" style="cursor: pointer">Model </a>
                            </div>
                            <div class="admin_rebates_collumn">
                                <a onclick="ShortRebate(4);" style="cursor: pointer">Trim </a>
                            </div>
                            <div class="admin_rebates_collumn admin_rebates_bodytype">
                                <a onclick="ShortRebate(5);" style="cursor: pointer">Body Type </a>
                            </div>
                            <div class="admin_rebates_collumn admin_rebates_manufact">
                                <a onclick="ShortRebate(6);" style="cursor: pointer">MB </a>
                            </div>
                            <div class="admin_rebates_collumn admin_rebates_create">
                                <a onclick="ShortRebate(8);" style="cursor: pointer">Create </a>
                            </div>
                            <div class="admin_rebates_collumn admin_rebates_expiration">
                                <a onclick="ShortRebate(7);" style="cursor: pointer">Expiration </a>
                            </div>
                            <div class="admin_rebates_collumn admin_rebates_disclaimer">
                                Disclaimer
                            </div>
                        </div>
                        <div class="admin_rebates_items" style="margin-left: 6px;">
                            <%= Html.DropDownListFor(x => x.SelectedYear, Model.YearsList, new {@class = "admin_rebate_select"}) %>
                            <%= Html.DropDownListFor(x => x.SelectedMake, Model.MakesList, new {@class = "admin_rebate_select"}) %>
                            <%= Html.DropDownListFor(x => x.SelectedModel, Model.ModelsList,
                                             new {@class = "admin_rebate_select"}) %>
                            <%= Html.DropDownListFor(x => x.SelectedTrim, Model.TrimsList, new {@class = "admin_rebate_select"}) %>
                            <%= Html.DropDownListFor(x => x.SelectedBodyType, Model.BodyTypeList,
                                             new {@class = "admin_rebate_select admin_rebates_bodytype"}) %>
                            <input type="text" class="admin_rebate_manufactory" id="rebateamount" data-validation-engine="validate[funcCall[checkMB]]" />
                            <input type="text" class="admin_rebate_create" id="txtCreate" />
                            <input type="text" class="admin_rebate_expiration" id="txtExpiration" />
                            <input type="text" class="admin_rebate_disclaimer" id="disclaimerrebate" />
                            <div class="btns_shadow admin_rebates_add_btn" id="btnAddManuRebate">
                                Add
                            </div>
                        </div>
                        <div class="rebate_list_holder" id="discounts">
                            <div class="data-content" align="center">
                                <img src="/content/images/ajaxloadingindicator.gif" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tradeIn" class="hider">
            </div>
            <div id="priceInfo" class="hider">
                <div class="discount-wrap">
                </div>
                <div class="hider">
                </div>
            </div>
            <div id="dealerInfo" class="hider">
            </div>
            <% if (Model.LandingPage.Equals("Default"))
               {%>
            <div id="contentBTN">
                <% }
               else
               {%>
                <div id="contentBTN" class="hider">
                    <% }%>
                </div>
                <div id="wholesale" class="hider">
                </div>
                <div id="notifications" class="hider">
                    <div style="width: 100%; display: block;">
                    </div>
                </div>
                <div id="buyerGuide_backup" class="hider">
                </div>
                <div id="add_user_holder">
                </div>
            </div>
        </div>

    </div>

    <% Html.EndForm(); %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    
    <script src="<%= Url.Content("~/Scripts/jquery.uploadify.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.uploadify.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/jquery.maskedinput-1.3.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/ckeditor/ckeditor.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/ckeditor/adapters/jquery.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/VinControl/admin.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/js/ajaxupload.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/jquery.multiple.select.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/VinControl/Admin_brand.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var basicsWarrantyTypes = <%= Html.ToJson(Model.BasicWarrantyTypes) %>;
        var logOffURL = '<%= Url.Action("LogOff", "Account") %>';
        
        var loadingImg = '<%= Url.Content("~/Content/images/ajaxloadingindicator.gif") %>';
        $("#DDLFilterModel").multipleSelect({
            multiple: true,
            multipleWidth: 55
        });
    </script>
    <script type="text/javascript">
        function checkBucketJumpRange(field, rules, i, options) {
            //if($("#rebateamount").val() !='')
            //{
            if (parseInt($("#DealerSetting_FirstTimeRangeBucketJump").val())>parseInt($("#DealerSetting_SecondTimeRangeBucketJump").val())) {
                return "second time range must be bigger than first time range";
            }
            //}
        }

        function validateCellphone(cellPhone) {
            var pattern = /^\(\d{3}\)\s*\d{3}(?:-|\s*)\d{4}$/
            return pattern.test(cellPhone);
        }

        function validateEmail(email) { 
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        } 
        function CheckEmail(field, rules, i, options) {
            if (!validateEmail(field.val())) {
                return "Email invalid!";
            }
        }
        
        function checkVariancePrice(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) >= 1000000) {
                return "Trade In Price Variance < $1,000,000";
            }
        }
        
        function checkMB(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
                return "MB should <= $100,000,000";
            }
        }
        
        function checkPass(field, rules, i, options) {
            var pass = field.val();
            var validPassword = ValidatePass(pass);
            console.log(pass.length);
            if (!validPassword) {
                return "Password invalid, Password should be 4-15 characters, should include at least one alphabet and at least one number.";
            }
        }
        
        function checkUser(field, rules, i, options) {
            var userName = field.val();
            var validUser = ValidateUser(userName);
            if (!validUser) {
                return "UserName invalid, UserName should be 4-15 characters, allow alpha and number only.";
            }
        }

        function ValidatePass(pass) {
            var re = /^(?=.*\d)(?=.*[a-zA-Z]).{4,15}$/;
            return re.test(pass);
        }
        
        function ValidateUser(userName) {
            var re = /^[a-zA-Z0-9]{4,10}$/;
            return re.test(userName);
        }


        $(window).load(
            function () {
                //Load Data Brand
                LoadBrandData();
                
                $("#EnableAutoDescription").click(function () {
                    if ('<%= Model.DealerSetting.AutoDescriptionSubscribe %>'.toLowerCase() == 'false') {
                        if ($(this).is(':checked')) {
                            ShowWarningMessage('In order to enable auto description, please contact 1.855.VIN.CTRL.');
                            $(this).attr('checked', false);
                        }
                    }
                });
                var defaultStockImage = "";

                $("#FileUpload").fileUpload({
                    'uploader': '<%= Url.Content("~/Scripts/uploader.swf") %>',
                    'cancelImg': '<%= Url.Content("~/content/images/cancel.png") %>',
                    'script': '<%= Url.Content("~/Handlers/StockImageUpload.ashx") %>',
                    'folder': '<%= Url.Content("~/UploadImages") %>',
                    'fileDesc': 'Image Files',
                    'fileExt': '*.jpg;*.jpeg;*.gif;*.png',
                    'queueSizeLimit': 40,
                    'sizeLimit': 10240000,
                    'buttonText': 'Upload',
                    'displayData': 'percentage',
                    'scriptData': { 'DealerId':<%= SessionHandler.Dealer.DealershipId %> },
                    'multi': true,
                    'auto': true,
                    'simUploadLimit': 1,
                    onQueueFull: function (event, queueSizeLimit) {

                        $('#dialog').html("Sorry You Are only allowed to upload a maximuim of (40) images at a time!");
                        return false;
                    },
                    onUploadProgress: function (event, ID, fileObj, data) {
                        $("#progressbar").progressbar("value", 50);
                    },

                    onError: function (a, b, c, d) {

                        if (d.status == 404)
                            ShowWarningMessage("Could not find upload script. Use a path relative to: " + "<?= getcwd() ?>");
                        else if (d.type === "HTTP")
                            ShowWarningMessage("error " + d.type + ": " + d.status);
                        else if (d.type === "File Size")
                            ShowWarningMessage(c.name + " " + d.type + " Limit: " + Math.round(d.sizeLimit / 1024) + "KB");
                        else
                            ShowWarningMessage("error " + d.type + ": " + d.text);
                    },

                    onComplete: function (event, ID, fileObj, response, data) {
                        defaultStockImage = response.toString();
                        $("#DefaultStockImage").attr("src", defaultStockImage);
                    },
                    onAllComplete: function (event, data) {
                        $.post('<%= Url.Content("~/Admin/UpdateDefaultStockImage") %>', { DefaultStockImageUrl: defaultStockImage }, function (data) {
                        });

                    }
                });

                $("#vehicleTireCost").blur(UpdateAverageCost);
                $("#frontWindShieldCost").blur(UpdateAverageCost);
                $("#rearWindShieldCost").blur(UpdateAverageCost);
                $("#driverWindowCost").blur(UpdateAverageCost);
                $("#passengerWindowCost").blur(UpdateAverageCost);
                $("#driverSideMirrorCost").blur(UpdateAverageCost);
                $("#passengerSideMirrorCost").blur(UpdateAverageCost);
                $("#scratchCost").blur(UpdateAverageCost);
                $("#dentCost").blur(UpdateAverageCost);
                $("#majorDentCost").blur(UpdateAverageCost);
                $("#paintDamageCost").blur(UpdateAverageCost);
                $("#repaintedPanelCost").blur(UpdateAverageCost);
                $("#rustCost").blur(UpdateAverageCost);
                $("#acidentCost").blur(UpdateAverageCost);
                $("#missingPartsCost").blur(UpdateAverageCost);
                $("#lightBulbCost").blur(UpdateAverageCost);
             
                jQuery("#vehicleForm").validationEngine();
            });

                function UpdateAverageCost() {
                    console.log("changed value");
                    if (getValueOr0($("#vehicleTireCost").val()) == 0
                        && getValueOr0($("#frontWindShieldCost").val()) == 0
                        && getValueOr0($("#rearWindShieldCost").val()) == 0
                        && getValueOr0($("#driverWindowCost").val()) == 0
                        && getValueOr0($("#passengerWindowCost").val()) == 0
                        && getValueOr0($("#driverSideMirrorCost").val()) == 0
                        && getValueOr0($("#passengerSideMirrorCost").val()) == 0
                        && getValueOr0($("#scratchCost").val()) == 0
                        && getValueOr0($("#majorScratchCost").val()) == 0
                        && getValueOr0($("#dentCost").val()) == 0
                        && getValueOr0($("#majorDentCost").val()) == 0
                        && getValueOr0($("#paintDamageCost").val()) == 0
                        && getValueOr0($("#repaintedPanelCost").val()) == 0
                        && getValueOr0($("#rustCost").val()) == 0
                        && getValueOr0($("#acidentCost").val()) == 0
                        && getValueOr0($("#missingPartsCost").val()) == 0
                    && getValueOr0($("#lightBulbCost").val()) == 0)
                    {
                        $("#avgCost").val('$500');
                    } else {
                        
                    }
                }
        
                function formatCurrencyValue(value) {
                    var stringContent = value.toString();
                    stringContent = stringContent.split("").reverse().join("");
                    var index = 0; 
                    var j = 0;
                    //console.log(value.length);
                    while (index < stringContent.length - 1) {
                        index++;
                        j++;

                        if (j % 3 == 0) {
                            stringContent = stringContent.substr(0, index) + "," + stringContent.substr(index, stringContent.length);
                            index++;
                        }
                    }

                    return  "$" + stringContent.split("").reverse().join("");
                }
        
                function getValueOr0(value) {
                    if (value == '') {
                        return 0;
                    }
                    return parseInt(getCurrencyValue(value));
                }
                var profitUsage = 0;

                function setProfitUsage(value) {
                    if (profitUsage == value) {
                        return;
                    }

                    profitUsage = value;

                    if (profitUsage == 1) {
                        $('#rdAvgProfit').prop('checked', true);
                        $('#avgProfit').prop('disabled', false);
                        $('#avgProfit').focus();
                        $('#avgProfitPercent').prop('disabled', true);
                    }
                    else {
                        $('#rdAvgProfitPercent').prop('checked', true);
                        $('#avgProfit').prop('disabled', true);
                        $('#avgProfitPercent').prop('disabled', false);
                        $('#avgProfitPercent').focus();
                    }

                }
                $(document).ready(function () {
            
                    uploadEvents();
            
                    $("#DealerSetting_FirstTimeRangeBucketJump").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });

                    $("#DealerSetting_SecondTimeRangeBucketJump").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
                    
                    $("#DealerSetting_PriceVariance").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
                    
                    if ($("#DealerSetting_PriceVariance").val() != undefined && $("#DealerSetting_PriceVariance").val() != '') $("#DealerSetting_PriceVariance").val(formatDollar(Number($("#DealerSetting_PriceVariance").val().replace(/[^0-9\.]+/g, ""))));

                    jQuery("#adminForm").validationEngine({ promptPosition: "centerRight", scroll: false });

                    <% if (Model.DealerSetting.AverageProfitUsage == 1) %>
                    <% { %>
                    setProfitUsage(1);
                    <% } %>
                    <% else %>
                    <% { %>
                    setProfitUsage(2);
                    <% } %>

                    $("#avgProfitPercent").mask("9?9%");

                    $("#avgProfitPercent").blur(function() {
                        var value = ($(this).val().length == 1) ? $(this).val() + '%' : $(this).val();
                        $(this).val(value);
                    });
                 
                    formatCurrency(null, $("#avgCost"));
                    formatCurrency(null, $("#avgProfit"));
                    formatCurrency(null, $("#vehicleTireCost"));
                    formatCurrency(null, $("#frontWindShieldCost"));
                    formatCurrency(null, $("#rearWindShieldCost"));
                    formatCurrency(null, $("#driverWindowCost"));
                    formatCurrency(null, $("#passengerWindowCost"));
                    formatCurrency(null, $("#driverSideMirrorCost"));
                    formatCurrency(null, $("#passengerSideMirrorCost"));
                    formatCurrency(null, $("#scratchCost"));
                    formatCurrency(null, $("#majorScratchCost"));
                    formatCurrency(null, $("#dentCost"));
                    formatCurrency(null, $("#majorDentCost"));
                    formatCurrency(null, $("#paintDamageCost"));
                    formatCurrency(null, $("#repaintedPanelCost"));
                    formatCurrency(null, $("#rustCost"));
                    formatCurrency(null, $("#acidentCost"));
                    formatCurrency(null, $("#missingPartsCost"));
                    formatCurrency(null, $("#lightBulbCost"));
                    
                    formatCurrency(null, $("#vehicleTireRetail"));
                    formatCurrency(null, $("#frontWindShieldRetail"));
                    formatCurrency(null, $("#rearWindShieldRetail"));
                    formatCurrency(null, $("#driverWindowRetail"));
                    formatCurrency(null, $("#passengerWindowRetail"));
                    formatCurrency(null, $("#driverSideMirrorRetail"));
                    formatCurrency(null, $("#passengerSideMirrorRetail"));
                    formatCurrency(null, $("#scratchRetail"));
                    formatCurrency(null, $("#majorScratchRetail"));
                    formatCurrency(null, $("#dentRetail"));
                    formatCurrency(null, $("#majorDentRetail"));
                    formatCurrency(null, $("#paintDamageRetail"));
                    formatCurrency(null, $("#repaintedPanelRetail"));
                    formatCurrency(null, $("#rustRetail"));
                    formatCurrency(null, $("#acidentRetail"));
                    formatCurrency(null, $("#missingPartsRetail"));
                    formatCurrency(null, $("#lightBulbRetail"));

                    UpdateAverageCost();

                    $("#saveBtn").click(function () {
                        var urlSubmit = "/Admin/UpdateContentSetting";
                        var currentTab = $('#currentTab').val();
                        
                        $("#DealerCraigslistSetting_EndingSentence").val(escape(CKEDITOR.instances.DealerCraigslistSetting_EndingSentence.getData()));

                        switch (currentTab) {
                            case 'admin_content_tab':
                                urlSubmit = "/Admin/UpdateContentSetting";
                                var resultEmail = jQuery('#DealerSetting_EbayContactInfoEmail').validationEngine('validate');
                                var resultPriceVariance = jQuery('#DealerSetting_PriceVariance').validationEngine('validate');
                                if(resultEmail==false && resultPriceVariance==false) {
                                    $('#SelectedBrandName').val($('#DDLFilterModel').multipleSelect('getSelects', 'text').toString());
                                    blockUI();
                                    $.ajax({
                                        type: "POST",
                                        url: urlSubmit,
                                        data: $('form').serialize(),
                                        success: function (results) {
                                            unblockUI();
                                        }
                                    });
                                }
                                break;
                            case 'admin_notifications_tab':
                                urlSubmit = "/Admin/UpdateNotificationSetting";
                                var result = jQuery('#DealerSetting_SecondTimeRangeBucketJump').validationEngine('validate');
                                if(result==false)
                                {
                                    blockUI();
                                    $.ajax({
                                        type: "POST",
                                        url: urlSubmit,
                                        data: $('form').serialize(),
                                        success: function (results) {
                                            unblockUI();
                                        }
                                    });
                                }
                                break;
                            case 'admin_credentials_tab':
                                urlSubmit = "/Admin/UpdatePasswordSetting";
                                $("#DealerCraigslistSetting_State").val($("#craigslistState option:selected").val());
                                $("#DealerCraigslistSetting_City").val($("#craigslistCity_" + $("#craigslistState").val() + " option:selected").text());
                                $("#DealerCraigslistSetting_CityUrl").val($("#craigslistCity_" + $("#craigslistState").val() + " option:selected").val());
                                $("#DealerCraigslistSetting_Location").val($("#craigslistLocation option:selected").text());
                                $("#DealerCraigslistSetting_LocationId").val($("#craigslistLocation option:selected").val());
                                
                                blockUI();
                                $.ajax({
                                    type: "POST",
                                    url: urlSubmit,
                                    data: $('form').serialize(),
                                    success: function (results) {
                                        unblockUI();
                  
                                    }, error: function(err) {
                                        unblockUI();
                                    }
                                });
                                break;
                            case 'admin_stockingguide_tab':
                                var flag = true;
                                if ($('#avgCost').validationEngine('validate') ||
                                    $('#vehicleTireCost').validationEngine('validate') ||
                                    $('#frontWindShieldCost').validationEngine('validate') ||
                                    $('#rearWindShieldCost').validationEngine('validate') ||
                                    $('#driverWindowCost').validationEngine('validate') || 
                                    $('#passengerWindowCost').validationEngine('validate') || 
                                    $('#driverSideMirrorCost').validationEngine('validate') ||
                                    $('#passengerSideMirrorCost').validationEngine('validate') ||
                                    $('#scratchCost').validationEngine('validate') ||
                                    $('#majorScratchCost').validationEngine('validate') ||
                                    $('#dentCost').validationEngine('validate') ||
                                    $('#majorDentCost').validationEngine('validate') ||
                                    $('#paintDamageCost').validationEngine('validate') ||
                                    $('#repaintedPanelCost').validationEngine('validate') ||
                                    $('#rustCost').validationEngine('validate') ||
                                    $('#acidentCost').validationEngine('validate') ||
                                    $('#missingPartsCost').validationEngine('validate') ||  
                                      $('#lightBulbCost').validationEngine('validate')||
                                   
                                    
                                    $('#vehicleTireRetail').validationEngine('validate') ||
                                    $('#frontWindShieldRetail').validationEngine('validate') ||
                                    $('#rearWindShieldRetail').validationEngine('validate') ||
                                    $('#driverWindowRetail').validationEngine('validate') ||
                                    $('#passengerWindowRetail').validationEngine('validate') ||
                                    $('#driverSideMirrorRetail').validationEngine('validate') ||
                                    $('#passengerSideMirrorRetail').validationEngine('validate') ||
                                    $('#scratchRetail').validationEngine('validate') ||
                                    $('#majorScratchRetail').validationEngine('validate') ||
                                    $('#dentRetail').validationEngine('validate') ||
                                    $('#majorDentRetail').validationEngine('validate') ||
                                    $('#paintDamageRetail').validationEngine('validate') ||
                                    $('#repaintedPanelRetail').validationEngine('validate') ||
                                    $('#rustRetail').validationEngine('validate') ||
                                    $('#acidentRetail').validationEngine('validate') ||
                                    $('#missingPartsRetail').validationEngine('validate')
                                ||  $('#lightBulbRetail').validationEngine('validate')
                                ) {
                                    flag = false;
                                }
                                if (profitUsage == 1 && $('#avgProfit').validationEngine('validate'))
                                    flag = false;

                                if (flag) {

                                    blockUI();
                                    var selectedBrandNames = getSelectedBrandNames();
                                    var profitValue = 0;

                                    if (profitUsage == 1) {
                                        profitValue = getCurrencyValue($("#avgProfit").val());
                                    }
                                    else {
                                        profitValue = $("#avgProfitPercent").val();

                                        if (profitValue.length > 0 && profitValue[profitValue.length - 1] == '%') {
                                            profitValue = profitValue.substr(0, profitValue.length - 1);
                                        }
                                    }

                                    var avgCost = getCurrencyValue($("#avgCost").val());

                                    urlSubmit = "/Admin/UpdateStockingGuideSetting";
                                
                                    $.ajax({
                                        type: "POST",
                                        url: urlSubmit,
                                        data: {brandName: selectedBrandNames,AverageCost:avgCost,AverageProfitUsage:profitUsage,AverageProfit:profitValue,AverageProfitPercentage:profitValue,
                                            
                                            tireCost: getCurrencyValue($("#vehicleTireCost").val()),
                                            lightBulbCost:getCurrencyValue($("#lightBulbCost").val()),
                                            frontWindShieldCost:getCurrencyValue($("#frontWindShieldCost").val()),
                                            rearWindShieldCost:getCurrencyValue($("#rearWindShieldCost").val()),
                                            driverWindowCost:getCurrencyValue( $("#driverWindowCost").val()),
                                            passengerWindowCost:getCurrencyValue($("#passengerWindowCost").val()),
                                            driverSideMirrorCost:getCurrencyValue($("#driverSideMirrorCost").val()),
                                            passengerSideMirrorCost:getCurrencyValue($("#passengerSideMirrorCost").val()),
                                            scratchCost:getCurrencyValue($("#scratchCost").val()),
                                            majorScratchCost:getCurrencyValue($("#majorScratchCost").val()),
                                            dentCost:getCurrencyValue($("#dentCost").val()),
                                            majorDentCost:getCurrencyValue($("#majorDentCost").val()),
                                            paintDamageCost:getCurrencyValue($("#paintDamageCost").val()),
                                            repaintedPanelCost:getCurrencyValue($("#repaintedPanelCost").val()),
                                            rustCost:getCurrencyValue($("#rustCost").val()),
                                            acidentCost:getCurrencyValue($("#acidentCost").val()),
                                            missingPartsCost:getCurrencyValue($("#missingPartsCost").val()),
                                            
                                            tireRetail:getCurrencyValue($("#vehicleTireRetail").val()),
                                            lightBulbRetail:getCurrencyValue($("#lightBulbRetail").val()),
                                            frontWindShieldRetail:getCurrencyValue($("#frontWindShieldRetail").val()),
                                            rearWindShieldRetail:getCurrencyValue($("#rearWindShieldRetail").val()),
                                            driverWindowRetail:getCurrencyValue($("#driverWindowRetail").val()),
                                            passengerWindowRetail:getCurrencyValue($("#passengerWindowRetail").val()),
                                            driverSideMirrorRetail:getCurrencyValue($("#driverSideMirrorRetail").val()),
                                            passengerSideMirrorRetail:getCurrencyValue($("#passengerSideMirrorRetail").val()),
                                            scratchRetail:getCurrencyValue($("#scratchRetail").val()),
                                            majorScratchRetail:getCurrencyValue($("#majorScratchRetail").val()),
                                            dentRetail:getCurrencyValue($("#dentRetail").val()),
                                            majorDentRetail:getCurrencyValue($("#majorDentRetail").val()),
                                            paintDamageRetail:getCurrencyValue($("#paintDamageRetail").val()),
                                            repaintedPanelRetail:getCurrencyValue($("#repaintedPanelRetail").val()),
                                            rustRetail:getCurrencyValue($("#rustRetail").val()),
                                            acidentRetail:getCurrencyValue($("#acidentRetail").val()),
                                            missingPartsRetail:getCurrencyValue($("#missingPartsRetail").val()),
                                        },
                                        success: function (results) {
                                            unblockUI();
                                        }
                                    });
                                }
                                break;
                        }
                    });

                    $("#btnsubmit").live('click', function () {

                        if (IsNumeric($("#Variance").val())) {
                            $("#lbVarianceValidate").hide();

                            var value = parseInt($("#Variance").val());

                            blockUI();
                            $.ajax({
                                type: "POST",
                                url: "/Admin/SaveVarianceCost?cost=" + value,
                                data: {},
                                success: function (results) {

                                    $("#cost_result").html(results);
                                    unblockUI();
                                }
                            });

                        }
                        else {
                            $("#lbVarianceValidate").show();
                        }
                    });

                    $("#aCancelBuyerGuide").click(function () {
                        if ($("#divNewBuyerGuide").is(":visible")) {
                            $("#txtNewBuyerGuide").val('');
                            $("#divNewBuyerGuide").slideUp("slow");
                        }
                    });           
     
                    $("#txtNewBuyerGuide").keypress(function (event) {
                        if (event.which == 13) {
                            event.preventDefault();
                            if ($("#SelectedWarrantyType").val() == "") {
                                ShowWarningMessage('Category is required.');
                            }
                            else if ($("#txtNewBuyerGuide").val() == "") {
                                ShowWarningMessage('Buyer Guide Name is required.');
                            } else {

                                $.ajax({
                                    type: "GET",
                                    url: '/Admin/AddNewBuyerGuide?name=' + $("#txtNewBuyerGuide").val() + '&category=' + $("#SelectedWarrantyType").val(),
                                    data: {},
                                    success: function (results) {
                                        if (results == 'Existing') {
                                            ShowWarningMessage('This name is already in the system');
                                        } else if (results == 'TimeOut') {
                                            window.location.href = logOffURL;
                                        } else if (results == 'Error') {
                                            ShowWarningMessage('System error!');
                                        } else {
                                            var str = "<div class=ac_buyGuide_items id=subBuyerGuide_" + results + ">";
                                            str += "<div class=\"ac_buyGuide_item_btn\"> </div>";
                                            str += "<div class=ac_buyGuide_item_name id=spanBuyerGuideName_" + results + ">" + $("#txtNewBuyerGuide").val() + "</div>";

                                            str += "<select id=SelectedWarrantyTypeForEdit_" + results + " style='width:90px;float: left;display:none;'>";
                                            str += "<option value=''>-- Select category --</option>";

                                            $.each(basicsWarrantyTypes, function (index, value) {
                                                str += value.Value == $("#SelectedWarrantyType").val()
                                                    ? "<option value=" + value.Value + " selected='true'>" + value.Text + "</option>"
                                                    : "<option value=" + value.Value + ">" + value.Text + "</option>";
                                            });

                                            str += "</select>";

                                            str += "<input class='ac_input' type=text id=txtEditBuyerGuide_" + results + " name=txtEditBuyerGuide_" + results + " style='float: left; display:none;' value='" + $("#txtNewBuyerGuide").val() + "' />";
                                            str += "<div class=ac_buyGuide_item_lang ac_buyGuide_item_langFist > English | ";
                                            str += "<a class=iframe href=/Report/CreateBuyerGuide?type=" + results + " id=English_" + results + " style=''>";
                                            str += " </a>";
                                            str += "</div>";
                                            str += "<div class=ac_buyGuide_item_lang > Spanish ";
                                            str += "<a class=iframe href=/Report/CreateBuyerGuideSpanish?type=" + results + " id=Spanish_" + results + " style=''>";
                                            str += " </a>";
                                            str += "</div>";
                                            str += '<div class="ac_buyguide_remove_edit">';
                                            str += "<a id=aEditBuyerGuide_" + results + " href=javascript:; > Edit |</a>";
                                            str += "<a id=aRemoveBuyerGuide_" + results + " href=javascript:; > Remove </a>";
                                            str += '</div>';
                                            str += '<div style="float: left;" class="ac_buyguide_canel_done">';
                                            str += "<a id=aDoneEditBuyerGuide_" + results + " href=javascript:; style='padding:0 5px;display:none;'>Done</a>";
                                            str += '</div>';
                                            str += '<div style="float: left;" class="ac_buyguide_canel_done">';
                                            str += "<a id=aCancelEditBuyerGuide_" + results + " href=javascript:; style='padding:0 5px;display:none;'>Cancel</a>";
                                            str += '</div>';
                                            str += "<br/>";
                                            str += "</div>";

                                            $("#txtNewBuyerGuide").val('');
                                            $('#SelectedWarrantyType option:selected').remove();
                                            $("#divNewBuyerGuide").slideUp("slow");

                                            $('#spanNewBuyerGuide').append(str);
                                        }
                                    }
                                });
                            };
                        }
                    });

                    $("input[id^='txtEditBuyerGuide_']").live('keypress', function (event) {
                        if (event.which == 13) {
                            event.preventDefault();
                            var id = this.id.split("_")[1];
                            if ($("#SelectedWarrantyTypeForEdit_" + id).val() == "") {
                                ShowWarningMessage('Category is required.');
                            }
                            else if ($("#txtEditBuyerGuide_" + id).val() == "") {
                                ShowWarningMessage('Buyer Guide Name is required.');
                            } else {
                                $.ajax({
                                    type: "GET",
                                    url: '/Admin/UpdateBuyerGuide?listingId=' + id + '&name=' + $("#txtEditBuyerGuide_" + id).val() + '&category=' + $("#SelectedWarrantyTypeForEdit_" + id).val(),
                                    data: {},
                                    success: function (results) {
                                        if (results == 'Existing') {
                                            ShowWarningMessage('This name is already in the system');
                                        } else if (results == 'TimeOut') {
                                            window.location.href = logOffURL;
                                        } else if (results == 'Error') {
                                            ShowWarningMessage('System error!');
                                        } else if (results == 'True') {
                                            $("#aDoneEditBuyerGuide_" + id).hide();
                                            $("#aCancelEditBuyerGuide_" + id).hide();
                                            $("#txtEditBuyerGuide_" + id).hide();
                                            $("#SelectedWarrantyTypeForEdit_" + id).hide();
                                            $("#aEditBuyerGuide_" + id).show();
                                            $("#aRemoveBuyerGuide_" + id).show();
                                            $("#spanBuyerGuideName_" + id).html($("#txtEditBuyerGuide_" + id).val());
                                            $("#spanBuyerGuideName_" + id).show();
                                        } else {
                                            ShowWarningMessage(results);
                                        }
                                    }
                                });
                            }
                        }
                    });
                });

                function getSelectedBrandNames() {
                    var selectedNames = $('#DDLFilterModel').multipleSelect('getSelects', 'text').toString();

                    return selectedNames;
                }

                function uploadEvents() {

                    $("#btnUploadImage").click(function (e) {
                        $(this).ajaxUpload({
                            url: '<%= Url.Action("UploadDealerPhoto", "Image") %>',
                            name: "file",
                            onSubmit: function () {
                            },
                            onComplete: function (result) {
                        
                                switch (result) {
                                    case 'Error':
                                        $('#uploadedMessage').html('System Error!').css("color", "red");
                                        break;
                                    case 'NoFile':
                                        $('#uploadedMessage').html('No photo is uploaded!').css("color", "red");
                                        break;
                                    case 'NoContent':
                                        $('#uploadedMessage').html('Your photo is empty!').css("color", "red");
                                        break;
                                    case 'NoSupport':
                                        $('#uploadedMessage').html('Your photo is not supported!').css("color", "red");
                                        break;
                                    case 'OverContent':
                                        $('#uploadedMessage').html('Your photo is larger then maximum!').css("color", "red");
                                        break;
                                        //case 'FileExists':
                                        //    $('#uploadedMessage').html('A duplicate photo name exists!').css("color", "red");
                                        //    break;
                                    default:
                                        $('#uploadedMessage').html('');
                                        $('.ac_uploadimg_img').html('<img id="DefaultStockImage" width="94" style="max-height:90px;" src="' + result + '" />');                                
                                        break;
                                }
                            }
                        });
                    });
            
                $("#btnUploadImage").trigger("click");
    
            }

            function disclaimerManageEvents() {

                $(".rdm_view").mouseenter(function () {
                    $(".rebate_edit_disclaimer").hide();
                    $(this).parent().parent().find(".rebate_view_disclaimer").show();
                }).mouseleave(function () {
                    $(".rebate_view_disclaimer").hide();
                });

                $(".rdm_clear").live("click", function () {
                    if (confirm("Do you want to clear this disclaimer?")) {
                        $.post('/Admin/SetDisclaimerContent',  {rebateId:$(this).parent().attr("rebateId"),content:''}, function(data) {
                        });
                        $(this).parent().parent().find(".rebate_view_disclaimer").find(".rebate_disclaimer_content").html("<p></p>");
                    }
                    $(".rebate_edit_disclaimer").hide();
                });

                $(".rdm_edit").live("click", function () {
                    $(".rebate_edit_disclaimer").hide();
                    // $("#opacity-layer").show();
                    var html = $(this).parent().parent().find(".rebate_view_disclaimer").find(".rebate_disclaimer_content").children("p").html();
                    $(this).parent().parent().find(".rebate_edit_disclaimer").find("textarea").val(html);
                    $(this).parent().parent().find(".rebate_edit_disclaimer").fadeIn();
                });
                $(".rde_btns_cancel").live("click", function () {
                    $(".rebate_edit_disclaimer").fadeOut();
                });

                $(".rde_btns_save").live("click", function () {
                    var value = $(this).parent().parent().find("textarea").val();
                    $.post('/Admin/SetDisclaimerContent',  {rebateId:$(this).parent().parent().parent().attr("rebateId"),content:value}, function(data) {
                    });
                    $(this).parent().parent().parent().find(".rebate_disclaimer_content").html("<p>" + value + "</p>");
                    $(".rebate_edit_disclaimer").fadeOut();
                });
            }
            function appraisalNotify(checkbox) {
                $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 0 }, function (data) {
                    if (data.SessionTimeOut == "TimeOut") {
                        ShowWarningMessage("Your session has timed out. Please login back again");
                        var actionUrl = logOffURL;
                        window.parent.location = actionUrl;
                    }
                });

                var checks = $('input[type="checkbox"]');
                if (checkbox.checked) {
                    for (i in checks) {
                        if (checks[i] != undefined && checks[i].id != undefined) {
                            if (checks[i].id.indexOf("AppraisalCheckbox") != -1) {
                                checks[i].disabled = false;
                            }
                        }
                    }
                } else {
                    for (i in checks) {
                        if (checks[i] != undefined && checks[i].id != undefined) {
                            if (checks[i].id.indexOf("AppraisalCheckbox") != -1) {
                                checks[i].checked = false;
                                checks[i].disabled = true;
                            }
                        }
                    }
                }
            }

            function appraisalNotifyPerUser(checkbox) {
                $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 0 }, function (data) {

                    if (data.SessionTimeOut == "TimeOut") {
                        ShowWarningMessage("Your session has timed out. Please login back again");
                        var actionUrl = logOffURL;
                        window.parent.location = actionUrl;
                    }
                });
            }
            function WholesaleNotify(checkbox) {

                $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 1 }, function (data) {
                        console.log(data);
                        if (data.SessionTimeOut == "TimeOut") {
                            ShowWarningMessage("Your session has timed out. Please login back again");
                            var actionUrl = logOffURL;
                            window.parent.location = actionUrl;
                        }
                    });

                    var checks = $('input[type="checkbox"]');
                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("WholeSaleCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }
                            }
                        }
                    } else {
                        for (i in checks) {
                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("WholeSaleCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }
                            }
                        }
                    }
                }
                function wholeSaleNotifyPerUser(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 1 }, function (data) {
                        if (data.SessionTimeOut == "TimeOut") {
                            ShowWarningMessage("Your session has timed out. Please login back again");
                            var actionUrl = logOffURL;
                            window.parent.location = actionUrl;
                        }
                    });
                }
                function InventoryNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 2 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            ShowWarningMessage("Your session has timed out. Please login back again");
                            var actionUrl = logOffURL;
                            window.parent.location = actionUrl;
                        }
                    });
                    var checks = $('input[type="checkbox"]');
                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("InventoryCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }
                            }
                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("InventoryCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }
                            }
                        }
                    }
                }
                function inventoryNotifyPerUser(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>',
                { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 2 },
                function (data) {
                    if (data.SessionTimeOut == "TimeOut") {
                        ShowWarningMessage("Your session has timed out. Please login back again");
                        var actionUrl = logOffURL;
                        window.parent.location = actionUrl;
                    }
                });
                }
                function TwentyFourHourNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>',
                { Notify: checkbox.checked, Notificationkind: 3 },
                function (data) {
                    if (data.SessionTimeOut == "TimeOut") {
                        ShowWarningMessage("Your session has timed out. Please login back again");
                        var actionUrl = logOffURL;
                        window.parent.location = actionUrl;
                    }
                });
                    var checks = $('input[type="checkbox"]');
                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("24HCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }
                            }
                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("24HCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }
                            }
                        }
                    }
                }
                function twentyfourhourNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 3 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            ShowWarningMessage("Your session has timed out. Please login back again");
                            var actionUrl = logOffURL;
                            window.parent.location = actionUrl;
                        }
                    });
                }
                function NoteNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 4 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            ShowWarningMessage("Your session has timed out. Please login back again");
                            var actionUrl = logOffURL;
                            window.parent.location = actionUrl;
                        }
                    });
                    var checks = $('input[type="checkbox"]');
                    if (checkbox.checked) {
                        for (i in checks) {
                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("NoteCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }
                            }
                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("NoteCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }
                            }
                        }
                    }
                }
                function noteNotifyPerUser(checkbox) {

                    $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 4 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            ShowWarningMessage("Your session has timed out. Please login back again");
                            var actionUrl = logOffURL;
                            window.parent.location = actionUrl;
                        }

                    });
                }
                function PriceNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 5 }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            ShowWarningMessage("Your session has timed out. Please login back again");
                            var actionUrl = logOffURL;
                            window.parent.location = actionUrl;
                        }


                    });

                    var checks = $('input[type="checkbox"]');

                    if (checkbox.checked) {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("PriceCheckbox") != -1) {
                                    checks[i].disabled = false;
                                }

                            }

                        }
                    } else {
                        for (i in checks) {

                            if (checks[i] != undefined && checks[i].id != undefined) {
                                if (checks[i].id.indexOf("PriceCheckbox") != -1) {
                                    checks[i].checked = false;
                                    checks[i].disabled = true;
                                }
                            }
                        }
                    }

                }
                function MarketPriceRangeNotify(checkbox) {
                    $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 7 }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }


            });
            var checks = $('input[type="checkbox"]');

            if (checkbox.checked) {
                for (i in checks) {

                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("MarketPriceRangeCheckbox") != -1) {
                            checks[i].disabled = false;
                        }

                    }


                }
            } else {
                for (i in checks) {
                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("MarketPriceRangeCheckbox") != -1) {
                            checks[i].checked = false;
                            checks[i].disabled = true;
                        }
                    }
                }
            }
        }
        function priceNotifyPerUser(checkbox) {

            $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 5 }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }


            });
        }
        function marketPriceRangeNotifyPerUser(checkbox) {

            $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 7 }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }


            });
        }
        function AgeNotify(checkbox) {
            $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 6 }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
            var checks = $('input[type="checkbox"]');
            if (checkbox.checked) {
                for (i in checks) {

                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("AgeCheckbox") != -1) {
                            checks[i].disabled = false;
                        }
                    }
                }
            } else {
                for (i in checks) {
                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("AgeCheckbox") != -1) {
                            checks[i].checked = false;
                            checks[i].disabled = true;
                        }
                    }
                }
            }
        }
        function BucketJumpReportNotify(checkbox) {
            $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 8 }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
            var checks = $('input[type="checkbox"]');
            if (checkbox.checked) {
                for (i in checks) {
                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("BucketJumpCheckbox") != -1) {
                            checks[i].disabled = false;
                        }
                    }
                }
            } else {
                for (i in checks) {

                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("BucketJumpCheckbox") != -1) {
                            checks[i].checked = false;
                            checks[i].disabled = true;
                        }
                    }

                }
            }
        }
        function ImageUploadNotify(checkbox) {
            $.post('<%= Url.Content("~/Admin/UpdateNotification") %>', { Notify: checkbox.checked, Notificationkind: 9 }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
            var checks = $('input[type="checkbox"]');

            if (checkbox.checked) {
                for (i in checks) {

                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("ImageUploadCheckbox") != -1) {
                            checks[i].disabled = false;
                        }
                    }
                }
            } else {
                for (i in checks) {
                    if (checks[i] != undefined && checks[i].id != undefined) {
                        if (checks[i].id.indexOf("ImageUploadCheckbox") != -1) {
                            checks[i].checked = false;
                            checks[i].disabled = true;
                        }
                    }
                }
            }
        }
        function imageUploadNotifyPerUser(checkbox) {

            $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 9 }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }

            });
        }
        function ageNotifyPerUser(checkbox) {

            $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 6 }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }
        function bucketJumpReportNotifyPerUser(checkbox) {
            $.post('<%= Url.Content("~/Admin/UpdateNotificationPerUser") %>', { Notify: checkbox.checked, UserId: checkbox.id, Notificationkind: 8 }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }
        function retailPriceWindowStickerNotify(checkbox) {
            $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: <%= Constanst.NotificationType.RetailPrice %> }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }
        function dealerDiscountWindowStickerNotify(checkbox) {
            $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: <%= Constanst.NotificationType.DealerDiscount %> }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }
        function manufacturerRebateWindowStickerNotify(checkbox) {
            $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: <%= Constanst.NotificationType.Manufacturer %> }, function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }
        function salePriceWindowStickerNotify(checkbox) {
            $.post('<%= Url.Content("~/Admin/WindowStickerNotify") %>', { Notify: checkbox.checked, Notificationkind: <%= Constanst.NotificationType.SalePrice %> }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
            });
        }
        function OverideStockImage(checkbox) {

            $.post('<%= Url.Content("~/Admin/UpdateOverideStockImage") %>', { OverideStockImage: checkbox.checked }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session has timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }

            });
        }
       
      
        function updateRebateAmount(txtBox) {
            $.post('<%= Url.Content("~/Admin/UpdateRebateAmount") %>',
                { RebateAmount: txtBox.value, RebateId: txtBox.id },
                function (data) {
                });
        }
        function deleteRebate(rebate) {
            var answer = confirm("Are you sure you want to delete this rebate?");
            if (answer) {
               
                $('#elementID').removeClass('hideLoader');
                $.post('<%= Url.Content("~/Admin/DeleteRebate") %>', { rebateId: $(rebate).attr('rebateId') }, function (data) {
                    $(rebate).parent().remove();
                });
                $('#elementID').addClass('hideLoader');
            }
        }
        function updateRebateDisclaimer(txtBox) {
            $.post('<%= Url.Content("~/Admin/UpdateRebateDisclaimer") %>',
                { RebateDisclaimer: txtBox.value, RebateId: txtBox.id },
                function (data) {
                });
        }

        $(document).ready(function() {
        
            var Make = $("#SelectedMake");
            var Year = $("#SelectedYear");
            var Model = $("#SelectedModel");
            var Trim = $("#SelectedTrim");
            var BodyType = $("#SelectedBodyType");

            Year.change(function() {

                if (Year.val() != "Year") {
                    Make.html("");
                    Model.html("");
                    Trim.html("");
                    BodyType.html("");
                    $.post('<%= Url.Content("~/Admin/YearAjaxPost") %>', { YearId: Year.val() }, function(data) {
                        var itemsMake = "<option value='" + 0 + "****" + "Make..." + "'>" + "Make..." + "</option>";
                        var makeList = data.MakeList;
                        
                        if (makeList != null) {
                            for (i = 0; i < makeList.length; i++) {
                                itemsMake += "<option value='" + makeList[i] + "'>" + makeList[i] + "</option>";
                            }
                        }
                        Make.html(itemsMake);
                        
                    });
                }
            });


            Make.change(function() {

                if (Make.val() != "0****Make...") {
                    Model.html("");
                    Trim.html("");
                    BodyType.html("");
                    $.post('<%= Url.Content("~/Admin/MakeAjaxPost") %>', { YearId: Year.val(),makeId:Make.val() }, function(data) {
                        var itemsModel = "<option value='" + 0 + "****" + "Model..." + "'>" + "Model..." + "</option>";
                        var modelList = data.ModelList;

                        if (modelList != null) {
                            for (i = 0; i < modelList.length; i++) {
                                itemsModel += "<option value='" + modelList[i] + "'>" + modelList[i] + "</option>";
                            }
                        }
                        Model.html(itemsModel);

                    });
                }
            });

            Model.change(function() {

                if (Model.val() != "0****Model...") {
                    Trim.html("");
                    BodyType.html("");
                    $.post('<%= Url.Content("~/Admin/ModelAjaxPost") %>', { YearId: Year.val(), MakeId: Make.val(), ModelId: Model.val() }, function(data) {
                        //var itemTrims = "<option value='" + 0 + "****" + "Trim..." + "'>" + "Trim..." + "</option>";
                        var itemTrims = "<option value='All Trims'>" + "All Trims" + "</option>";
                        var trimList = data.TrimList;
                        if (trimList != null) {
                            if (trimList.length == 1) {
                                for (var i = 0; i < trimList.length; i++) {
                                    itemTrims = "<option value='" + trimList[i] + "'>" + trimList[i] + "</option>";
                                }
                            } else {
                                for (i = 0; i < trimList.length; i++) {
                                    itemTrims += "<option value='" + trimList[i] + "'>" + trimList[i] + "</option>";
                                }
                            }
                        }
                        Trim.html(itemTrims);

                    });
                }

            });


            Trim.change(function() {

                if (Trim.val() != "0****Trim...") {
                    BodyType.html("");
                    $.post('<%= Url.Content("~/Admin/BodyTypeAjaxPost") %>', { YearId: Year.val(), MakeId: Make.val(), ModelId: Model.val(),TrimId :Trim.val()}, function(data) {
                        var itemBodyTypes = "<option value='" + 0 + "****" + "Body..." + "'>" + "Body..." + "</option>";
                        var bodyTypeList = data.BodyTypeList;
                        if (bodyTypeList != null) {

                            if (bodyTypeList.length == 1) {
                                for (var i = 0; i < bodyTypeList.length; i++) {
                                    itemBodyTypes = "<option value='" + bodyTypeList[i] + "'>" + bodyTypeList[i] + "</option>";
                                }
                            } else {
                                for (i = 0; i < bodyTypeList.length; i++) {
                                    itemBodyTypes += "<option value='" + bodyTypeList[i] + "'>" + bodyTypeList[i] + "</option>";
                                }
                            }
                        }

                        
                        BodyType.html(itemBodyTypes);

                    });
                }

            });


            $("#rebateamount").numeric({ decimal: false, negative: false }, function() {
                ShowWarningMessage("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#FirstRange").numeric({ decimal: false, negative: false }, function() {
                ShowWarningMessage("Positive integers only");
                this.value = "";
                this.focus();
            });


            $('input[id^=chkButtonPermisison_]').live("click", function() {
                console.log('permission clicked');
                var groupId = this.id.split('_')[1];
                var buttonId = this.id.split('_')[2];
                var canSee = $(this).is(':checked') ? 'True' : 'False';
                $.ajax({
                    type: "GET",
                    url: "/Admin/UpdateButtonPermission?groupId=" + groupId + "&buttonId=" + buttonId + "&canSee=" + canSee,
                    data: {},
                    success: function(results) {

                    }
                });
            });

            $(".addUser").fancybox({});
            $(".add_user_popup_close").click(function() {
                $("#opacity-layer").hide();
                $(".add_user_holder").fadeOut();
            });
            $(".clickopenpupop").click(function(event) {
                setFullHeight($("#opacity-layer"));
                $("#opacity-layer").show();
                $(".add_user_holder").show();
                centerElement($(".add_user_holder"));
            });

           

            function IsEmail(email) {
                var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!regex.test(email)) {
                    return false;
                } else {
                    return true;
                }
            }

            $("#addUserBtn").click(function(event) {
                var flag = true;

                if(jQuery('#NewName').validationEngine('validate') == true)
                    flag = false;
                if(jQuery('#NewUsername').validationEngine('validate') == true)
                    flag = false;
                if(jQuery('#NewPassword').validationEngine('validate') == true)
                    flag = false;
                if(jQuery('#NewEmail').validationEngine('validate') == true)
                    flag = false;
                if(jQuery('#NewPhone').validationEngine('validate') == true)
                    flag = false;

                if (flag==true) {
                    $.post('<%= Url.Content("~/Admin/CheckUserExist") %>', { UserName: $("#NewUsername").val(), UserEmail: $("#NewEmail").val(), DealerId: '<%= SessionHandler.Dealer.DealershipId %>' }, function(user) {
                        $('#elementID').addClass('hideLoader');
                        if (user.SessionTimeOut == "TimeOut") {
                            flag = false;
                            ShowWarningMessage("Your session is timed out. Please login back again");
                            var actionLogOutUrl = logOffURL;
                            window.parent.location = actionLogOutUrl;
                        } else if (user.IsUserNameExist == "Exist") {
                            flag = false;

                            ShowWarningMessage("This username already existed");

                        } else if (user.IsUserEmailExist == "Exist") {
                            flag = false;
                            ShowWarningMessage("This email already existed");
                        }
                        if (flag) {
                            if ($('#MutipleDealer').val() == "True") {
                                var actionUrl = '<%= Url.Action("ChoooseDealerForUser", "Admin") %>';
                                $("<a class='iframe' href=" + actionUrl + "></a>").fancybox({
                                    overlayShow: true,
                                    showCloseButton: true,
                                    enableEscapeButton: true,
                                    width: 500,
                                    height: 330,
                                    padding: 0,
                                    margin: 0,
                                    onClosed: function() {
                                        //                                        window.location.reload(true);
                                    }
                                }).click();
                            } else {
                                $('#elementID').removeClass('hideLoader');
                                $.post('<%= Url.Content("~/Admin/AddSingleUser") %>',
                                    {
                                        Name: $("#NewName").val(),
                                        UserName: $("#NewUsername").val(),
                                        Password: $("#NewPassword").val(),
                                        Email: $("#NewEmail").val(),
                                        CellPhone: $("#NewPhone").val(),
                                        roleId: $("#UserLevel").val()
                                    },
                                    function(user) {
                                        $('#elementID').addClass('hideLoader');
                                        if (user.SessionTimeOut == "TimeOut") {
                                            ShowWarningMessage("Your session is timed out. Please login back again");
                                            var actionUrl = logOffURL;
                                            window.parent.location = actionUrl;
                                        } else {
                                            $("#NewName").val("Name");
                                            $("#NewUsername").val("UserName");
                                            $("#NewPassword").val("Password");
                                            $("#NewEmail").val("Email");
                                            $("#NewPhone").val("Cell#");
                                            parent.getUpdatedUserList();
                                        }
                                    });
                            }

                        }

                    });
                }
            });

            CKEDITOR.replace('DealerCraigslistSetting_EndingSentence',
            {
                height: 235,
                //toolbar: 'Custom',
                toolbarCanCollapse: false,
                toolbar_Custom: []
            });
                        
            LoadCraigslistSetting();
        });
        
        function LoadCraigslistSetting()
        {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: "/Admin/LoadCraigslistSetting",
                data: {},
                cache: false,
                traditional: true,
                success: function(result) {
                    $('#divCraiglistSetting').html(result);
                },
                error: function(err) {
                    $("#craigslistLocation").html('<option value="0">-- Choose location --</option>');        
                }
            });
        }

        function getCurrencyValue(currency) {
            var currencyValue = currency;
            currencyValue = deleteCharacter(currencyValue, '$');
            currencyValue = deleteCharacter(currencyValue, ',');
            return currencyValue;
        }

        function checkCurrencyValue(field, rules, i, options) {
            if (field.val() == "") {
                field.val("$0");
            }

            var currency = getCurrencyValue(field.val());

            if (currency.length == 0 || parseInt(currency) < 0) {
                return "Invalid input";
            }

            if (parseInt(currency) > 100000000) {
                return "Value should <= $100,000,000";
            }
        }

        function validateValueIfCostFilledIn(field, rules, i, options) {
            //var currency = getCurrencyValue(field.val());

            //if (currency.length == 0 || parseInt(currency) <= 0) {
            //    return "Invalid input";
            //}

            //if (parseInt(currency) > 100000000) {
            //    return "Value should <= $100,000,000";
            //}

            var a=rules[i+2];
            var fieldToCompared =  getCurrencyValue(jQuery("#"+a).val());
            if(fieldToCompared!='' && parseInt(fieldToCompared)!=0)
            {
                //if(field.val()=='')
                //{
                //    console.log(field.val());
                //    console.log(field.val()=='');
                //    return "Retail is required.";
                //}
                if(parseFloat(getCurrencyValue(field.val())) < parseFloat( getCurrencyValue(jQuery("#"+a).val()) ) ) {
                    return "Retail and should be greater than or equal to cost.";
                    console.log("in");
                }

                console.log("out");

                console.log(field);
                console.log(rules);
                console.log(i);
                console.log(options);
            }
        }

        function validateInput(event) {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault(); 
            }
        }

        function deleteCharacter(stringToDelete, charToDelete) {
            var stringOutPut = stringToDelete;

            for (var index = stringOutPut.length - 1; index >= 0; index --) {
                if (stringOutPut[index] == charToDelete) {
                    stringOutPut = stringOutPut.substr(0, index) + stringOutPut.substr(index + 1, stringOutPut.length);
                }
            }

            return stringOutPut;
        }

        function formatCurrency(event, obj) {
            if (event == null || event.keyCode == 46 || event.keyCode == 8
                 || (event.keyCode >= 48 && event.keyCode <= 57))
            {
                var value = $(obj).val();
                
                var originalLength = value.length;
                var rightPos = value.length - obj.selectionEnd;
                var leftPos = obj.selectionEnd;

                value = deleteCharacter(value, '$');
                value = deleteCharacter(value, ',');

                if (value != '') {
                    value = value.split("").reverse().join("");

                    var index = 0;
                    var j = 0;

                    while (index < value.length - 1) {
                        index++;
                        j++;

                        if (j % 3 == 0) {
                            value = value.substr(0, index) + "," + value.substr(index, value.length);
                            index++;
                        }
                    }

                    value = "$" + value.split("").reverse().join("");

                }
                $(obj).val(value);

                if (value.length > originalLength) {
                    rightPos = value.length - rightPos;

                    obj.selectionStart = rightPos;
                    obj.selectionEnd = rightPos;
                }

                if (value.length <= originalLength) {
                    obj.selectionStart = leftPos;
                    obj.selectionEnd = leftPos;
                }
            }
        }

        $('#DeleteStock').click(function () {
            console.log("Delete");
            $('#elementID').removeClass('hideLoader');

            $.post('<%= Url.Content("~/Admin/DeleteStockImage") %>', function (data) {
                if (data.SessionTimeOut == "TimeOut") {
                    ShowWarningMessage("Your session is timed out. Please login back again");
                    var actionUrl = logOffURL;
                    window.parent.location = actionUrl;
                }
                else {
                    $('#DefaultStockImage').attr("src", "");
                }
                $('#elementID').addClass('hideLoader');

            });
        });

        function HideBuyerGuideDivs() {
            $('#divBuyerGuide1').hide();
            $('#divBuyerGuide2').hide();
            $('#divBuyerGuide3').hide();
            $('#divBuyerGuide4').hide();
        }
        function StringBuilder() {
            var strings = [];
            this.append = function (string) {
                string = verify(string);
                if (string.length > 0) strings[strings.length] = string;
            };

            this.appendLine = function (string) {
                string = verify(string);
                if (this.isEmpty()) {
                    if (string.length > 0) strings[strings.length] = string;
                    else return;
                } else strings[strings.length] = string.length > 0 ? "\r\n" + string : "\r\n";
            };

            this.clear = function () { strings = []; };
            this.isEmpty = function () { return strings.length == 0; };
            this.toString = function () { return strings.join(""); };

            var verify = function (string) {
                if (!defined(string)) return "";
                if (getType(string) != getType(new String())) return String(string);
                return string;
            };

            var defined = function (el) {
                // Changed per Ryan O'Hara's comment:
                return el != null && typeof (el) != "undefined";
            };

            var getType = function (instance) {
                if (!defined(instance.constructor)) throw Error("Unexpected object type");
                var type = String(instance.constructor).match(/function\s+(\w+)/);

                return defined(type) ? type[1] : "undefined";
            };
        }
        ;
        $('a:not(.iframe)').click(function (e) {
            if ($(this).attr('target') == '')
                $('#elementID').removeClass('hideLoader');

        });
        $('a#aAsIs').click(function (e) {
            $(this).fancybox({ 'width': 1000, 'height': 700, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        });
        $("a.iframe").fancybox({ 'width': 800, 'height': 600, 'hideOnOverlayClick': false, 'centerOnScroll': true });
        $("a.fancybox").fancybox();
        $("a[id^='English_']").live('mouseover focus', function (e) {
            $(this).fancybox({
                'width': 1050,
                'height': 700,
                'hideOnOverlayClick': false,
                'centerOnScroll': true,
                autoDimensions: false,
                'onCleanup': function () {
                }
            });
        });
        $("a[id^='Spanish_']").live('mouseover focus', function (e) {
            $(this).fancybox({
                'width': 1050,
                'height': 700,
                'hideOnOverlayClick': false,
                'centerOnScroll': true,
                autoDimensions: false,
                'onCleanup': function () {
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%= Url.Content("~/Content/Admin.css") %>" rel="stylesheet" type="text/css" />
    <link href="../../Content/jquery-ui.css" rel="stylesheet" />
    <link href="/Content/VinControl/multiple-select.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    <%=Html.Partial("../StockingGuide/_TemplateAdminBrand")  %>
</asp:Content>