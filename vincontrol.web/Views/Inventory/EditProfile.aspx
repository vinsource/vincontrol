<%@ Page Title="" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<vincontrol.Application.ViewModels.CommonManagement.CarInfoFormViewModel>" %>

<%@ Import Namespace="vincontrol.Constant" %>
<%@ Import Namespace="Vincontrol.Web.Handlers" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.ModelYear %>
    <%=Model.Make %>
    <%=Model.Model %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("Action", "Inventory", FormMethod.Post, new { id = "editiProfileForm", name = "editiProfileForm", onsubmit = "return validateForm()" }); %>
    <input type="hidden" id="IsEmployee" name="IsEmployee" value="<%= (bool)Session["IsEmployee"]%>" />
    <input type="hidden" id="EnableAutoDescriptionSetting" name="EnableAutoDescriptionSetting"
        value="<%= Model.EnableAutoDescriptionSetting %>" />
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div>
                <% if (vincontrol.ConfigurationManagement.ConfigurationHandler.DisableSilverlight == "1") {%>
                <a id="manage_images" class="iframe btns_shadow container_right_btns_buttons" href="<%=Url.Action("OpenUploadWindow","Inventory",new{ListingId=Model.ListingId,InventoryStatus=Model.InventoryStatus}) %>">Manage Images</a>
                <%} else {%>
                <a id="A1" class="iframe btns_shadow container_right_btns_buttons" href="<%=Url.Action("OpenSilverlightUploadWindow","Inventory",new{ListingId=Model.ListingId,InventoryStatus=Model.InventoryStatus}) %>">Manage Images</a>
                <%} %>
                <% InventoryUserRight userRight = SessionHandler.UserRight.Inventory; %>

                <% if (userRight.ViewProfile_EditProfile && Model.Type != Constanst.CarInfoType.Sold) %>
                <% { %>
                <input class="pad btns_shadow container_right_btns_buttons" type="submit" name="SaveIProfile"
                    value=" Save Changes " id="SaveIProfile" />
                <% } %>

                <% if (SessionHandler.CurrentView == CurrentViewEnum.SoldInventory.ToString()) %>
                <% { %>
                <a id="cancel_change_profile" class="btns_shadow container_right_btns_buttons"
                    href="<%=Url.Action("ViewISoldProfile","Inventory",new{listingId=Model.ListingId}) %>">Cancel</a>
                <% } %>
                <% else %>
                <% { %>
                <a id="cancel_change_profile" class="btns_shadow container_right_btns_buttons"
                    href="<%=Url.Action("ViewIProfile","Inventory",new{listingId=Model.ListingId}) %>">Cancel</a>
                <% } %>


                <%--<input class="pad btns_shadow container_right_btns_buttons" type="button" name="CancelIProfile"
                    value=" Cancel " id="CancelIProfile" onclick="ViewProfile();" />--%>
            </div>
            <div style="float: left; width: 50%;">
                <div id="vehicle_yearmakemodel" style="color: white!important" title="<%=Model.Trim%>">
                    <%=Html.DynamicHtmlLabel("lblTitleWithoutTrim", "TitleWithoutTrim")%>
                    <%=Model.Trim%>
                </div>

                <div id="errorStock" style="color: red; display: none; padding-left: 20px;">
                    This stock already existed, please enter another stock.
                </div>
            </div>
        </div>
    </div>
    <div id="container_right_content" style="height: 570px">
        <div class="right_cont_column_one">
            <a class="vin_viewprofile" href="/index/inventorypf">
                <div class="market_logo">
                    <%=Html.DynamicHtmlLabel("lblMarketTitle", "MarketRangeEditProfile")%>
                </div>
            </a>
            <div class="datetime">
                <%=DateTime.Now %>
            </div>
            <div class="column_one_part">
                <div class="column_one_items">
                    <label>
                        VIN</label>
                    <% if (Model.VinDecodeSuccess)
                       {%>
                    <%= Html.TextBoxFor(x => x.Vin, new { @class = "column_one_input readonly", @readonly = "readonly" })%>
                    <%}
                       else
                       {%>
                    <%--<%= Html.TextBoxFor(x => x.Vin, new { @class = "column_one_input" })%>--%>
                    <input type="text" name="Vin" id="Vin" value="<%= Model.Vin %>"
                        class="column_one_input" data-errormessage-value-missing="test" />
                    <%}%>
                </div>
                <div class="column_one_items">
                    <label>
                        Stock</label>
                    <%= Html.TextBoxFor(x => x.Stock, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Year</label>
                    <%--<% if (Model.VinDecodeSuccess)
                       {%>
                    <%= Html.TextBoxFor(x => x.ModelYear, new { @class = "column_one_input readonly", @readonly = "readonly" })%>
                    <%}
                       else
                       {%>
                    <%= Html.TextBoxFor(x => x.ModelYear, new { @class = "column_one_input" })%>
                    <%}%>--%>
                    <%= Html.TextBoxFor(x => x.ModelYear, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Make</label>
                  
                    <%= Html.TextBoxFor(x => x.Make, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Model</label>
                   
                    <%= Html.TextBoxFor(x => x.VehicleModel, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items" style="height: 50px;">
                    <div>
                        <label>
                            Trim</label>
                        <% if (Model.EditTrimList != null) %>
                        <%{ %>
                        <%= Html.DropDownListFor(x => Model.SelectedTrimItem, Model.EditTrimList, new {@class = "DDLEdit", id = "SelectedTrim"}) %>

                        <% } %>
                    </div>
                    <div class="others_left">
                        Other:
                        <%= Html.TextBoxFor(x => x.CusTrim, new { @class = "exterior_others", MaxLength = 50 })%>
                    </div>
                </div>
            </div>
            <div class="column_one_part">
                <div class="column_one_items">
                    <label>
                        Cylinders</label>
                   
                    <%= Html.TextBoxFor(x => x.Cylinder, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Liters</label>
                 
                    <%= Html.TextBoxFor(x => x.Litter, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Doors</label>
                  
                    <%= Html.TextBoxFor(x => x.Door, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Style</label>
                    <%= Html.TextBoxFor(x => x.BodyType, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Fuel</label>
                    <%= Html.TextBoxFor(x => x.Fuel, new { @class = "column_one_input" })%>
                </div>
                <div class="column_one_items">
                    <label>
                        Drive</label>
                    <%= Html.DropDownListFor(x => x.SelectedDriveTrain, Model.ChromeDriveTrainList, new { @class = "DDLEdit" })%>
                </div>
            </div>
            <div>
                <div id="descriptions_header">
                    Description:
                    <div id="divEditDescription" class="edit_profile_manual">
                        <input type="checkbox" id="chbManual" <%= Model.IsManual ? "checked" : "" %> />
                        Edit Manually
                    </div>
                </div>
                <div id="descriptions_nav">
                    <div class="btns_shadow des_nav_items" id="btnGeneratedescription">
                        Generate
                    </div>
                    <div class="btns_shadow des_nav_items" id="btnauctiondescription">
                        Auction
                    </div>
                    <div class="btns_shadow des_nav_items" id="btnLoanerDescription">
                        Loaner
                    </div>
                </div>
                <div id="descriptions_container" style="width: 251px;">
                    <textarea cols="28" <%= Model.EnableAutoDescriptionSetting ? (Model.IsManual ? "" : "disabled") : "" %>
                        id="Description" name="Description" rows="7"><%= Model.Description %></textarea>
                </div>
            </div>
        </div>
        <div class="right_cont_column_two">
            <div class="column_two_items" style="padding-top: 4px; height: 21px;">
                <label>
                    Vehicle Type</label>
                <% if (Model.VehicleTypeList != null) %>
                <%{%>

                <%= Html.Partial("../Inventory/_TruckPartial", Model)%>
                <%= Html.DropDownListFor(x => x.SelectedVehicleType, Model.VehicleTypeList, new {@class = "DDLEditColum2", @style="width:70px;float:left;"}) %>
                <%}%>
            </div>
            <div class="column_two_part" style="margin-top: 5px !important;">
                <div class="column_two_items" style="height: 50px;">
                    <div>
                        <label>
                            Exterior Color</label>
                        <%=Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ChromeExteriorColorList, new { @class = "DDLEditColum2" })%>
                        <%=Html.HiddenFor(x => x.SelectedExteriorColorValue, Model.SelectedExteriorColorValue)%>
                    </div>
                    <div class="others">
                        Other:
                     
                        <input type="text" name="CusExteriorColor" id="CusExteriorColor" value="<%= Model.CusExteriorColor %>" data-validation-placeholder="0"
                            class="exterior_others" maxlength="50" data-validation-engine="validate[required,funcCall[checkOtherExteriorColor]]" data-errormessage-value-missing="Other Exterior Color is required!" />
                    </div>
                </div>
                <div class="column_two_items" style="height: 50px;">
                    <div>
                        <label>
                            Interior Color</label>
                        <%=Html.DropDownListFor(x => x.SelectedInteriorColor, Model.ChromeInteriorColorList, new { @class = "DDLEditColum2" })%>
                    </div>
                    <div class="others">
                        Other:
                        <%--<%= Html.TextBoxFor(x => x.CusInteriorColor, new { @class = "interior_others" })%>--%>
                        <input type="text" name="CusInteriorColor" id="CusInteriorColor" value="<%= Model.CusInteriorColor %>" data-validation-placeholder="0"
                            class="interior_others" maxlength="50" data-validation-engine="validate[funcCall[checkOtherInteriorColor]]" data-errormessage-value-missing="Other Interior Color is required!" />
                    </div>
                </div>
                <div class="column_two_items">
                    <label style="width: 33%;">
                        Odometer (*)</label>
                    <%--<span id="errorOdometer" class="errorEdit">*</span><%= Html.TextBoxFor(x => x.Mileage, new { @class = "column_one_input_Column2", maxlength = "7", @autocomplete = "off" })%>--%>
                    <input type="text" name="Mileage" id="Mileage" value="<%= Model.Mileage %>" data-validation-placeholder="0"
                        class="column_one_input_Column2" data-validation-engine="validate[required,funcCall[checkMileage]]" data-errormessage-value-missing="Odometer is required!" autocomplete="off" />
                </div>
                <div class="column_two_items">
                    <label>
                        Transmission (*)</label>
                    <span id="errorTranmission" class="errorEdit">*</span><%= Html.DropDownListFor(x => x.SelectedTranmission, Model.ChromeTranmissionList, new { @class = "DDLEditColum2" })%>
                </div>
                <div class="column_two_items">
                    <label style="width: 33%">
                        Warranty Type</label>
                    <select class="DDLEditColum2" id="ddlWarrantyType" onchange="javascript:warrantyInfoUpdate(this);">
                        <option value="0"></option>
                        <% if (Model.WarrantyTypes.Any())
                           {%>
                        <% foreach (var item in Model.WarrantyTypes)
                           {%>
                        <option value="<%= item.Id %>" <%= (item.Id == Model.WarrantyInfo) ? "selected" : "" %>>
                            <%= item.Name %></option>
                        <%}%>
                        <%}%>
                    </select>
                </div>
            </div>
            <div class="column_two_part" style="height: 150px;">
                <div class="column_two_items">
                    <label>
                        Certified</label>
                    <%= Html.CheckBoxFor(x=>x.IsCertified)%>
                </div>
                <div class="column_two_items">
                    <label>
                        Prior Rental</label>
                    <%=  Html.DynamicHtmlControlForIprofile("txtPriorRental", "EditPriorRental")%>
                </div>
                <div class="column_two_items">
                    <label>
                        Dealer Demo</label>
                    <%=  Html.DynamicHtmlControlForIprofile("txtDealerDemo", "EditDealerDemo")%>
                </div>
                <div class="column_two_items">
                    <label>
                        Unwind?</label>
                    <%=  Html.DynamicHtmlControlForIprofile("txtUnwind", "EditUnwind")%>
                </div>
                <div class="column_two_items">
                    <label>
                        A Car</label>
                    <input type="checkbox" id="ACar" name="ACar" <%= Model.ACar ? "checked" : "" %> />
                </div>
                <div class="column_two_items">
                    <label>
                        RAV(Branded Title)</label>
                    <%= Html.CheckBoxFor(x=>x.BrandedTitle)%>
                </div>
            </div>
            <div class="in_adding_pricing">
                <div class="column_two_items" style="margin-top: 20px;">
                    <label>
                        Additional Title Info</label>
                    <%= Html.TextBoxFor(x => x.Title, new { @class = "addt_title_info" })%>
                </div>
                <div class="column_two_items">
                    <label>
                        Original MSRP</label>
                    <%= Html.TextBoxFor(x => x.Msrp, new { @class = "original_msrp readonly", @readonly = "readonly" })%>
                </div>
            </div>
            <div class="in_adding_pricing">
                <div id="pricing_header">
                    Pricing Information:
                </div>
                <div class="column_two_items">
                    <label>
                        Retail Price</label>
                    <%--<%= Html.TextBoxFor(x => x.RetailPrice, new { @class = "retail_price" })%>--%>
                    <input type="text" name="RetailPrice" id="RetailPrice" value="<%= Model.RetailPrice %>"
                        class="retail_price" data-validation-engine="validate[funcCall[checkRetailPrice]]" autocomplete="off" />
                </div>
                <div class="column_two_items">
                    <label>
                        Dealer Discount</label>
                    <%--<%= Html.TextBoxFor(x => x.DealerDiscount, new { @class = "dealer_discount" })%>--%>
                    <input type="text" name="DealerDiscount" id="DealerDiscount" value="<%= Model.DealerDiscount %>"
                        class="dealer_discount" data-validation-engine="validate[funcCall[checkDealerDiscount]]" autocomplete="off" />
                </div>
                <div class="column_two_items">
                    <label>
                        MR Rebate</label>
                    <%--<%= Html.TextBoxFor(x => x.ManufacturerRebate, new { @class = "mr_rebate" })%>--%>
                    <% if (!Model.Condition.Value) %>
                    <%
                       { %>
                    <input type="text" name="ManufacturerRebate" id="ManufacturerRebate" value="<%= Model.ManufacturerRebate %>" readonly="readonly"
                        class="mr_rebate readonly" data-validation-engine="validate[funcCall[checkManufacturerRebate]]" autocomplete="off" />
                    <% } %>
                    <%
                       else
                       { %>
                    <input type="text" disabled="disabled" value="Not available" />
                    <input type="text" name="ManufacturerRebate" id="ManufacturerRebate" value="0"
                        class="mr_rebate readonly" data-validation-engine="validate[funcCall[checkManufacturerRebate]]" autocomplete="off" style="display: none" />
                    <% } %>
                </div>
                <div class="column_two_items">
                    <label>
                        WS Price</label>
                    <%= Html.TextBoxFor(x => x.WindowStickerPrice, new { @class = "ws_price readonly", @readonly = "readonly" })%>
                </div>
            </div>
        </div>
        <div class="right_cont_column_three">
            <div id="packages_header">
                <% if (Model.Make.Equals("Ford"))
                   { %>
        
        
            Packages:  <a class="iframe" href="http://fordlabels.webview.biz/webviewhybrid/WindowSticker.aspx?vin=<%= Model.Vin %>">Original WS</a>


                <% }
                   else if (Model.Make.Equals("Chrysler"))
                   { %>
           
              
            Packages:  <a class="iframe" href="http://www.chrysler.com/hostd/windowsticker/getWindowStickerPdf.do?vin=<%= Model.Vin %>">Original WS</a>

                <% }
                   else if (Model.Make.Equals("Dodge"))
                   { %>
           
              
            Packages:  <a class="iframe" href="http://www.chrysler.com/hostd/windowsticker/getWindowStickerPdf.do?vin=<%= Model.Vin %>">Original WS</a>

                <% }
                   else if (Model.Make.Equals("Jeep"))
                   { %>
           
              
            Packages:  <a class="iframe" href="http://www.chrysler.com/hostd/windowsticker/getWindowStickerPdf.do?vin=<%= Model.Vin %>">Original WS</a>

                <% }
                   else
                   { %>
            Packages:  
        <% } %>
            </div>
            <div id="packages_container">
                <% if (Model.ChromeFactoryPackageOptions.Any())
                   { %>
                <%= Html.CheckBoxGroupPackage("txtFactoryPackageOption", Model.ChromeFactoryPackageOptions, Model.ExistPackages) %>
                <% }
                   else
                   { %>
                <%= Html.CheckBoxGroupPackageByYear("txtFactoryPackageOption")%>
                <% } %>
            </div>
            <div id="options_header">
                Options:
            </div>
            <div id="options_container">
                <div id="optionals">
                    <% if (Model.ChromeFactoryNonInstalledOptions.Any())
                       { %>
                    <%= Html.CheckBoxGroupOption("txtNonInstalledOption", Model.ChromeFactoryNonInstalledOptions,Model.ExistOptions)%>
                    <% }
                       else
                       { %>
                    <%= Html.CheckBoxGroupOptionByYear("txtNonInstalledOption")%>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hdnFirstSelectedTrim" />
    <%=Html.HiddenFor(x=>x.ListingId) %>
    <%=Html.HiddenFor(x=>x.AfterSelectedOptions) %>
    <%=Html.HiddenFor(x=>x.AfterSelectedPackage) %>
    <%=Html.HiddenFor(x=>x.AfterSelectedOptionCodes) %>
    <%= Html.HiddenFor(x=>x.SelectedPackagesDescription) %>
    <input type="hidden" id="hdRetailPrice" value="<%=Model.RetailPrice %>" />
    <input type="hidden" id="hdDealerDiscount" value="<%=Model.DealerDiscount %>" />
    <input type="hidden" id="hdManufacturerRebate" value="<%=Model.ManufacturerRebate %>" />
    <input type="hidden" id="hdWindowStickerPrice" value="<%=Model.WindowStickerPrice %>" />
    <% Html.EndForm(); %>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">
        function checkVin(field, rules, i, options) {
            if (field.val() != "") {
                $.post('<%= Url.Content("~/Decode/CheckVin") %>', { vin: field.val() }, function(data) {
                    if (data.success == true) {
                        if (data.isExisted == true) {
                            var didConfirm = confirm("This vin is already existed, do you want to go to detail page?");
                            if (didConfirm == true) {
                                var url;
                                if (data.isAppraisal == true) {
                                    url = '/Appraisal/ViewProfileForAppraisal?AppraisalId=' + data.id;
                                    window.location = url;
                                } else {
                                    url = '/Inventory/ViewIProfile?ListingID=' + data.id;
                                    window.location = url;
                                }
                            } else {
                                field.val('');
                            }
                        }
                    }
                });
            }
        }
        
        function checkMileage(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 2000000) {
                return "Mileage should <= 2,000,000";
            }
        }
        
        function checkRetailPrice(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
                return "Retail Price should <= 100,000,000";
            }
        }
        
        function checkDealerDiscount(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
                return "Dealer Discount should <= 100,000,000";
            }
        }
        
        function checkManufacturerRebate(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 100000000) {
                return "MR Rebate should <= 100,000,000";
            }
        }
        
        function checkOtherExteriorColor(field, rules, i, options) {
            if ($("#SelectedExteriorColor").val() == "Other Colors" && field.val().length == 0) {
                rules.push('required');
            }
        }

        function checkOtherInteriorColor(field, rules, i, options) {
            if ($("#SelectedInteriorColor").val() == "Other Colors" && field.val().length == 0) {
                rules.push('required');
            }
        }
        
    
        $("#SelectedExteriorColorCode").change(function() {
            $('#CusExteriorColor').validationEngine('hide');
            if ($("#SelectedExteriorColorCode").val().indexOf('Other Colors') != -1) {
                $("#CusExteriorColor").removeAttr('disabled');
            } else {
                $("#CusExteriorColor").attr('disabled', true);
            }
        });
        
        $("#SelectedInteriorColor").change(function() {
            $('#CusInteriorColor').validationEngine('hide');
            if ($("#SelectedInteriorColor").val().indexOf('Other Colors') != -1) {
                $("#CusInteriorColor").removeAttr('disabled');
            } else {
                $("#CusInteriorColor").attr('disabled', true);
            }
        });

        $("#SelectedTrim").change(function() {
            $('#CusTrim').validationEngine('hide');
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            
            if ($("#SelectedTrim").val().indexOf('Base/Other Trims') != -1) {
                $("#CusTrim").removeAttr('disabled');
            } else {
                $("#CusTrim").attr('disabled', true);
            }
            
            var index = $("#SelectedTrim").val().indexOf("|");
            var id = $("#SelectedTrim").val().substring(0, index);

            $('#elementID').removeClass('hideLoader');
            var listingId = $('#ListingId').val();
            $.post('<%= Url.Content("~/Ajax/StyleAjaxPost") %>', { styleId: id,listingId:listingId }, function(data) {
               

                var MSRP = "";
                var vehicleoptions = new Array();
                var vehiclepackages = new Array();

                $.each(data, function(i, data) {
                    var text;
                    var option;
                    if (data.toString().indexOf("Optional") != -1) {
                        text = data.toString().substring(0, data.toString().length - 8);
                        var result = text.split("*");
                        option = new Object();
                        option.name = result[0];
                        option.price = result[1];
                        option.description = result[2];
                        option.code = result[3];
                        option.checked = result[4];
                        option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',','');
                        vehicleoptions[vehicleoptions.length] = option;
                    } else if (data.toString().indexOf("Package") != -1) {
                        text = data.toString().substring(0, data.toString().length - 7);
                        var result = text.split("*");
                        option = new Object();
                        option.name = result[0];
                        option.price = result[1];
                        option.description = result[2];
                        option.code = result[3];
                        option.checked = result[4];
                        option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',','');
                        vehiclepackages[vehiclepackages.length] = option;
                    } else if (data.toString().indexOf("MSRP") != -1) {
                        MSRP = data.toString().substring(0, data.toString().length - 4);
                        MSRP = Number(MSRP);
                        MSRP = formatDollar(MSRP);
                    }

                    $("#Msrp").val(MSRP);
                    $("#ChromeStyleId").val(id);
                 
                    var packageContent = "";
                    $("#Packages").html("");

                    packageContent += "<ul class='options'>";

                    for (var i = 0; i < vehiclepackages.length; i++) {
                        packageContent += '<li>';
                        if(vehiclepackages[i].checked =='checked')
                            packageContent += "<input checked='checked' class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this," + (vehiclepackages[i].pureprice == '$' || vehiclepackages[i].pureprice == "" ? 0 : vehiclepackages[i].pureprice) + ");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price +   "' code='" + vehiclepackages[i].code+ "' title='" + vehiclepackages[i].description + "'>";
                        else
                            packageContent += "<input class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this," + (vehiclepackages[i].pureprice == '$' || vehiclepackages[i].pureprice == "" ? 0 : vehiclepackages[i].pureprice) +");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price +   "' code='" + vehiclepackages[i].code +"' title='" + vehiclepackages[i].description + "'>";
                        if(vehiclepackages[i].code !='')
                            packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>"+"("+vehiclepackages[i].code + ")"  + vehiclepackages[i].name +" "+ vehiclepackages[i].price + "</label>";
                        else
                            packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>"+ vehiclepackages[i].name +" "+ vehiclepackages[i].price+ "</label>";
                        packageContent += "<br class='z-index' price='" + vehiclepackages[i].price + "'>";
                        packageContent += '</li>';
                    }
            
                    packageContent += "</ul>";

                    $("#Packages").html(packageContent);

                    $("#Options").html("");
                    var optionContent = "";

                    optionContent += "<ul class='options'>";

                    for (var i = 0; i < vehicleoptions.length; i++) {
                        optionContent += '<li>';
                        if(vehicleoptions[i].checked =='checked')
                            optionContent += "<input  checked='checked' type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this,"+ (vehicleoptions[i].pureprice == '$' || vehicleoptions[i].pureprice == "" ? 0 : vehicleoptions[i].pureprice) +")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                        else
                            optionContent += "<input type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this,"+ (vehicleoptions[i].pureprice == '$' || vehicleoptions[i].pureprice == "" ? 0 : vehicleoptions[i].pureprice) +")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                        if(vehicleoptions[i].code !='')
                            optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>"+"("+vehicleoptions[i].code + ")"  + vehicleoptions[i].name+" "+ vehicleoptions[i].price + "</label>";
                        else
                            optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>"+ vehicleoptions[i].name+" "+ vehicleoptions[i].price + "</label>";
                        optionContent += '<br>';
                        optionContent += '</li>';
                    }

                    optionContent += "</ul>";
                    $("#Options").html(optionContent);
              
                    $('#elementID').addClass('hideLoader');
                    $.unblockUI();
                });

            });
        });

        $("#CancelIProfile").click(function () {

            $('#elementID').removeClass('hideLoader');
        });

        $("#SaveIProfile").click(function () {

            var retailPrice = Number($('#RetailPrice').val().replace(/[^0-9\.]+/g, ""));
            var dealerDiscount = Number($('#DealerDiscount').val().replace(/[^0-9\.]+/g, ""));
            var manufacturerRebate = Number($('#ManufacturerRebate').val().replace(/[^0-9\.]+/g, ""));

            if (Number(retailPrice) - Number(dealerDiscount) - Number(manufacturerRebate) < 0) {
                alert('Retail Price can\'t be less than Dealer Discount + MR Rebate');
                return false;
            }
            $('#elementID').removeClass('hideLoader');
        });

        $("#btnGeneratedescription").click(function () {

            $.post('<%= Url.Content("~/Inventory/GetGenerateAutoDescription") %>', {listingId:<%=Model.ListingId %>}, function (data) {
                if (data == "SessionTimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.location = actionUrl;
                } else {
                    $('#Description').val(data);
                }
            });
        });

        $("#btnauctiondescription").click(function () {

            $.post('<%= Url.Content("~/Inventory/GetDealerAuctionDescription") %>', {}, function (data) {
                if (data == "SessionTimeOut") {

                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.location = actionUrl;
                } else {
                    $('#Description').val(data);
                }
            });
        });


        $("#btnLoanerDescription").click(function () {

            $.post('<%= Url.Content("~/Inventory/GetDealerLoanerDescription") %>', {}, function (data) {
                if (data == "SessionTimeOut") {

                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.location = actionUrl;
                } else {
                    $('#Description').val(data);
                }
            });
        });

        $("#ACar").click(function () {
            if ($('#ACar').is(':checked')) {
                $('#ACar').val('True');
            } else {
                $('#ACar').val('False');
            }
        });

        $("#BrandedTitle").click(function () {
            if ($('#BrandedTitle').is(':checked')) {
                $('#BrandedTitle').val('True');
            } else {
                $('#BrandedTitle').val('False');
            }
        });
        
        $("a.iframe").fancybox({ 'width': 1010, 'height': 700, centerOnScroll: true });

        $("a.smalliframe").fancybox();

       
        function CalculateMSRP(optionPrice) {
            optionPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));
            var currency = $("#Msrp").val();
            var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
            var number2 = Number(optionPrice.replace(/[^0-9\.]+/g, ""));
            var total = Number(number1) + Number(number2);
            return total;
        }

        function includeItem(arr, obj) {
            for (var i = 0; i < arr.length; i++) {
                console.log( "arr = "+arr[i]);
                console.log("obj=" +obj);
                if ( arr[i].replace(/\s+/g, '').substring(obj.replace(/\s+/g, '') )>0) return true;
            }
            return false;
        }

        $(document).ready(function() {
            
            var firstType = '<%= Model.IsTruck ? "Truck" : "Car" %>';
            if (firstType == 'Car') $('#divTruckContainer').hide();
            else $('#divTruckContainer').show();
            $('#SelectedVehicleType').change(function() {
                var type = $(this).val();
                if (type == 'Car') {
                    
                    $('#SelectedTruckType option[value=""]').attr("selected","selected");
                    $("#SelectedTruckCategoryId").html('<option value="">Select...</option>');
                    $('#SelectedTruckCategoryId option[value=""]').attr("selected","selected");
                    $('#SelectedTruckClassId option[value=""]').attr("selected","selected");
                    $('#divTruckContainer').hide();
                }
                else $('#divTruckContainer').show();
            });
          
            $('#SelectedTruckType').live('change', function() {
                var truckType = $(this).val();
                if (truckType == '') {
                    $('#SelectedTruckCategoryId').html('<option value="">Select...</option>');
                } else {
                    $.ajax({
                        type: "GET",
                        dataType: "json",
                        url: '/Ajax/GetTruckCategoriesByType?type=' + truckType,
                        data: {},
                        cache: false,
                        traditional: true,
                        success: function (categories) {
                            var html = '<option value="">Select...</option>';
                            $.each(categories, function(index, category) {
                                html += ('<option value="' + category.Value + '">' + category.Text + '</option>');
                            });
                            $('#SelectedTruckCategoryId').html(html);
                        },
                        error: function (err, status) {
                            $('#SelectedTruckCategoryId').html('<option value="">Select...</option>');
                        }
                    });
                }
            });

            $("#imgTruck").click(function () {
                $('#divTruck').show();
                $.fancybox({
                    href: "#divTruck",
                    'onCleanup': function () {
                        $('#divTruck').hide();
                    }
                });
            });
            
            if ($("#IsEmployee").val() == 'True') {
                $("#RetailPrice").attr('readonly', 'true');
                $("#RetailPrice").addClass('readonly');

                $("#DealerDiscount").attr('readonly', 'true');
                $("#DealerDiscount").addClass('readonly');
            }

            jQuery("#editiProfileForm").validationEngine();
            
            $("#Mileage").numeric({ decimal: false, negative: false }, function() {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#RetailPrice").numeric({ decimal: false, negative: false }, function() {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#DealerDiscount").numeric({ decimal: false, negative: false }, function() {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#ManufacturerRebate").numeric({ decimal: false, negative: false }, function() {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#WindowStickerPrice").numeric({ decimal: false, negative: false }, function() {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#Door").numeric({ decimal: false, negative: false }, function() {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });
            
            $("#Cylinder").numeric({ decimal: false, negative: false }, function() {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            $("#Msrp").val(formatDollar(Number($("#Msrp").val().replace(/[^0-9\.]+/g, ""))));
            $("#Mileage").val(formatDollar(Number($("#Mileage").val().replace(/[^0-9\.]+/g, ""))));
            $("#RetailPrice").val(formatDollar(Number($("#RetailPrice").val().replace(/[^0-9\.]+/g, ""))));
            $("#DealerDiscount").val(formatDollar(Number($("#DealerDiscount").val().replace(/[^0-9\.]+/g, ""))));
            $("#ManufacturerRebate").val(formatDollar(Number($("#ManufacturerRebate").val().replace(/[^0-9\.]+/g, ""))));
            $("#WindowStickerPrice").val(formatDollar(Number($("#WindowStickerPrice").val().replace(/[^0-9\.]+/g, ""))));
            
            $('#RetailPrice').blur(function() {
                var retailPrice = Number($('#RetailPrice').val().replace(/[^0-9\.]+/g, ""));
                var dealerDiscount = Number($('#DealerDiscount').val().replace(/[^0-9\.]+/g, ""));
                var manufacturerRebate = Number($('#ManufacturerRebate').val().replace(/[^0-9\.]+/g, ""));

                if (Number(retailPrice) - Number(dealerDiscount) - Number(manufacturerRebate) > 0) {
                    $('#WindowStickerPrice').val(formatDollar(Number((Number(retailPrice) - Number(dealerDiscount) - Number(manufacturerRebate)).toString().replace(/[^0-9\.]+/g, ""))));
                    $('#hdWindowStickerPrice').val($('#WindowStickerPrice').val());
                    $('#hdRetailPrice').val($('#RetailPrice').val());
                } else {
                    alert('Retail Price can\'t be less than Dealer Discount + MR Rebate');
                    $('#RetailPrice').val(formatDollar(Number($('#hdRetailPrice').val().replace(/[^0-9\.]+/g, ""))));
                }
            });

            $('#DealerDiscount').blur(function() {
                var retailPrice = Number($('#RetailPrice').val().replace(/[^0-9\.]+/g, ""));
                var dealerDiscount = Number($('#DealerDiscount').val().replace(/[^0-9\.]+/g, ""));
                var manufacturerRebate = Number($('#ManufacturerRebate').val().replace(/[^0-9\.]+/g, ""));

                if (Number(retailPrice) - Number(dealerDiscount) - Number(manufacturerRebate) > 0) {
                    $('#WindowStickerPrice').val(formatDollar(Number((Number(retailPrice) - Number(dealerDiscount) - Number(manufacturerRebate)).toString().replace(/[^0-9\.]+/g, ""))));
                    $('#hdWindowStickerPrice').val($('#WindowStickerPrice').val());
                    $('#hdDealerDiscount').val($('#DealerDiscount').val());
                } else {
                    alert('Retail Price can\'t be less than Dealer Discount + MR Rebate');
                    $('#DealerDiscount').val(formatDollar(Number($('#hdDealerDiscount').val().replace(/[^0-9\.]+/g, ""))));
                }
            });

            $('#ManufacturerRebate').blur(function() {
                var retailPrice = Number($('#RetailPrice').val().replace(/[^0-9\.]+/g, ""));
                var dealerDiscount = Number($('#DealerDiscount').val().replace(/[^0-9\.]+/g, ""));
                var manufacturerRebate = Number($('#ManufacturerRebate').val().replace(/[^0-9\.]+/g, ""));
                if (Number(retailPrice) - Number(dealerDiscount) - Number(manufacturerRebate) > 0) {
                    $('#WindowStickerPrice').val(formatDollar(Number((Number(retailPrice) - Number(dealerDiscount) - Number(manufacturerRebate)).toString().replace(/[^0-9\.]+/g, ""))));
                    $('#hdWindowStickerPrice').val($('#WindowStickerPrice').val());
                    $('#hdManufacturerRebate').val($('#ManufacturerRebate').val());
                } else {
                    alert('Retail Price can\'t be less than Dealer Discount + MR Rebate');
                    $('#ManufacturerRebate').val(formatDollar(Number($('#hdManufacturerRebate').val().replace(/[^0-9\.]+/g, ""))));
                }
            });

            $('#WindowStickerPrice').blur(function() {
                var retailPrice = Number($('#RetailPrice').val().replace(/[^0-9\.]+/g, ""));
                var windowStickerPrice = Number($('#WindowStickerPrice').val().replace(/[^0-9\.]+/g, ""));

                if (Number(windowStickerPrice) - Number(retailPrice) > 0) {
                    alert('Retail Price can\'t be less than windowStickerPrice');
                    $('#WindowStickerPrice').val(formatDollar(Number($('#hdWindowStickerPrice').val().replace(/[^0-9\.]+/g, ""))));
                }
            });

            if ($("#EnableAutoDescriptionSetting").val() == 'False') {
                $("#btnGenerateAutoDescription").hide();
                $("#divEditDescription").hide();
            }

            $("#btnGenerateAutoDescription").click(function() {
                $.ajax({
                    type: "GET",
                    url: "/Inventory/GenerateAutoDescription?listingId=" + '<%=Model.ListingId %>',
                    data: {},
                    success: function(results) {
                        if (results == 'Failed') {
                            alert("You need to uncheck \'Edit description manually\' to use this function");
                            return false;
                        } else {
                            $("#Description").val(results);
                            $("#Description").attr('disabled', 'disabled');
                        }
                        //;$.unblockUI();
                    }
                });
            });

            $("#ShowImages").click(function(e) {
                $.post('<%= Url.Content("~/Inventory/GetImages") %>', { id: '<%=Model.ListingId %>' }, function(data) {
                    $("#photos").html('<ul id="listImages">' + data + '</ul>');
                    var count = document.getElementById('listImages').getElementsByTagName('li').length;
                    var currentheight = 55 * (count / 3);
                    document.getElementById('listImages').style.height = currentheight + 'px';
                });
            });

            $("a.image").live('mouseover', function() {
                $(this).fancybox();
              
            });
            
            if ($("#SelectedTrim").val().indexOf('Base/Other Trims') === -1) {
                $("#CusTrim").attr('disabled', true);
            }
            
            if ($("#SelectedExteriorColorCode").val().indexOf('Other Colors') === -1) {
                $("#CusExteriorColor").attr('disabled', true);
            }
            
            if ($("#SelectedInteriorColor").val().indexOf('Other Colors') === -1) {
                $("#CusInteriorColor").attr('disabled', true);
            }

            $("#chbManual").click(function(e) {
                if ($(this).is(':checked')) {
                    var manual = confirm('If you edit description manually, the description won\'t be updated automatically. Are you sure to continue?');
                    if (manual) {
                        $("#Description").removeAttr('disabled');
                        $.ajax({
                            type: "GET",
                            url: "/Inventory/UpdateAutoDescriptionStatus?listingId=" + '<%=Model.ListingId %>' + "&status=false",
                            data: {},
                            success: function() {

                            }
                        });

                    } else {
                        $("#chbManual").attr('checked', false);
                    }
                } else {
                    var auto = confirm('If you let the system create description automatically, all the description you typed in will be lost. Are you sure to continue?');
                    if (auto) {
                        $("#Description").attr('disabled', 'disabled');
                        $.ajax({
                            type: "GET",
                            url: "/Inventory/UpdateAutoDescriptionStatus?listingId=" + '<%=Model.ListingId %>' + "&status=true",
                            data: {},
                            success: function() {

                            }
                        });
                    } else {
                        $("#chbManual").attr('checked', true);
                    }
                }
            });

            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColorCode :selected").text());

            var trimValue = $("#SelectedTrim").val();
            //console.log(trimValue);
            if (trimValue) {
                $("#hdnFirstSelectedTrim").val(trimValue.substring(0, trimValue.indexOf("|")));
            }
       
            
            $("#SelectedExteriorColorCode").change(function(e) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColorCode :selected").text());
            });

            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            var index = $("#SelectedTrim").val().indexOf("|");
            var id = $("#SelectedTrim").val().substring(0, index);
            var listingId = $('#ListingId').val();
            $.post('<%= Url.Content("~/Ajax/StyleAjaxPost") %>', { styleId: id,listingId:listingId }, function(data) {

                $("#Packages li").children().each(
                    function() {

                        $(this).val("");
                        $(this).removeAttr("checked");
                        $(this).attr("class", "z-index hider");
                        $(this).next("label:first").text("");
                        $(this).next("label:first").attr("class", "z-index hider");

                    }
                );

                $("#Options li").children().each(
                    function() {

                        $(this).val("");
                        $(this).removeAttr("checked");
                        $(this).attr("class", "z-index hider");
                        $(this).next("label:first").text("");
                        $(this).next("label:first").attr("class", "z-index hider");

                    }
                );
                
                var vehicleoptions = new Array();

                var vehiclepackages = new Array();

                $.each(data, function(i, data) {
                    var option;
                    if (data.toString().indexOf("Optional") != -1) {
                        var text = data.toString().substring(0, data.toString().length - 8);
                        var result = text.split("*");
                        option = new Object();
                        option.name = result[0];
                        option.price = result[1];
                        option.description = result[2];
                        option.code = result[3];
                        option.checked = result[4];
                        option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',','');
                        vehicleoptions[vehicleoptions.length] = option;
                    } else if (data.toString().indexOf("Package") != -1) {
                        text = data.toString().substring(0, data.toString().length - 7);
                        result = text.split("*");
                        option = new Object();
                        option.name = result[0];
                        option.price = result[1];
                        option.description = result[2];
                        option.code = result[3];
                        option.checked = result[4];
                        option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',','');
                        vehiclepackages[vehiclepackages.length] = option;
                    }

                });

                var packageContent = "";
                $("#Packages").html("");

                packageContent += "<ul class='options'>";

                for (var i = 0; i < vehiclepackages.length; i++) {
                    packageContent += '<li>';
                    if(vehiclepackages[i].checked =='checked')
                        packageContent += "<input checked='checked' class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this,"+ (vehiclepackages[i].pureprice == "$" || vehiclepackages[i].pureprice == "" ? 0 : vehiclepackages[i].pureprice) +");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price +   "' code='" + vehiclepackages[i].code+ "' title='" + vehiclepackages[i].description + "'>";
                    else
                        packageContent += "<input class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this,"+ (vehiclepackages[i].pureprice == "$" || vehiclepackages[i].pureprice == "" ? 0 : vehiclepackages[i].pureprice) +");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price +   "' code='" + vehiclepackages[i].code +"' title='" + vehiclepackages[i].description + "'>";
                    if(vehiclepackages[i].code !='')
                        packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>"+"("+vehiclepackages[i].code + ")"  + vehiclepackages[i].name +" "+ vehiclepackages[i].price + "</label>";
                    else
                        packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>"+ vehiclepackages[i].name +" "+ vehiclepackages[i].price+ "</label>";
                    packageContent += "<br class='z-index' price='" + vehiclepackages[i].price + "'>";
                    packageContent += '</li>';
                }
            
                packageContent += "</ul>";

                $("#Packages").html(packageContent);

                $("#Options").html("");
                var optionContent = "";

                optionContent += "<ul class='options'>";

                for (var i = 0; i < vehicleoptions.length; i++) {
                    optionContent += '<li>';
                    if(vehicleoptions[i].checked =='checked')
                        optionContent += "<input  checked='checked' type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this,"+ (vehicleoptions[i].pureprice == '$' || vehicleoptions[i].pureprice == "" ? 0 : vehicleoptions[i].pureprice) +")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                    else
                        optionContent += "<input type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this,"+ (vehicleoptions[i].pureprice == "$" || vehicleoptions[i].pureprice == "" ? 0 : vehicleoptions[i].pureprice) +")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                    if(vehicleoptions[i].code !='')
                        optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>"+"("+vehicleoptions[i].code + ")"  + vehicleoptions[i].name+" "+ vehicleoptions[i].price + "</label>";
                    else
                        optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>"+ vehicleoptions[i].name+" "+ vehicleoptions[i].price + "</label>";
                    optionContent += '<br>';
                    optionContent += '</li>';
                }

                optionContent += "</ul>";
                $("#Options").html(optionContent);
                $.unblockUI();
            });

        });

        function validateForm() {
            var flag = true;

            if ($("#CusErrorEx").length > 0) {
                $("#CusErrorEx").remove();
            }

            if ($("#CusErrorIn").length > 0) {
                $("#CusErrorIn").remove();
            }

            if ($("#CusErrorApp").length > 0) {
                $("#CusErrorApp").remove();
            }

            if ($("#CusErrorTransmission").length > 0) {
                $("#CusErrorTransmission").parent().remove();
            }

            if ($("#CusErrorMileage").length > 0) {
                $("#CusErrorMileage").parent().remove();
            }

            if ($("#SelectedTranmission").val() == "") {
                flag = false;
                $('#errorTranmission').show();
            }
            else {
                $('#errorTranmission').hide();
            }
            
            if (flag == false)
                return false;
            else
                $('#elementID').removeClass('hideLoader');

            $("#Description").removeAttr('disabled');
            var result = $("#Packages input[name='txtFactoryPackageOption']");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('title') !== undefined) {
                    descriptionArray.push($(result[i]).attr('title'));
                }
            }
            if (result.length > 0) {
                $("#SelectedPackagesDescription").val(JSON.stringify(descriptionArray));
            }
            getAfterSelectedPakage();
            getAfterSelectedOption();

            $.ajax({
                type: 'POST',
                url: '<%=Url.Action("CheckStockNumber","Inventory") %>',
                data: { ListingId: $('#ListingId').val(), stock: $('#Stock').val() },
                async: false,
                success: function (result) {
                    if (result.success == true) {
                        flag = true;
                        $('#errorStock').hide();
                    }
                    else {
                        flag = false;
                        $('#errorStock').show();
                    }
                }
            });

            return flag;
        }
        
        function getAfterSelectedPakage() {
            var result = $("#Packages input[name='txtFactoryPackageOption']");
            var builderstring = '';
            var buildercodestring = '';
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {

                    builderstring += $(result[i]).attr('value') +",";
                    
                    if($(result[i]).attr('code') !='')
                        buildercodestring+= $(result[i]).attr('code') +",";
                }
            }
            if (result.length > 0) {
                $("#AfterSelectedPackage").val(builderstring.substring(0, builderstring.length - 1));

                if (buildercodestring != '')
                    $("#AfterSelectedOptionCodes").val(buildercodestring);

            }
        }

        function getAfterSelectedOption() {
            var result = $("#optionals input[name='txtNonInstalledOption']");
            var builderstring = '';
            var buildercodestring = '';
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {

                    builderstring += $(result[i]).attr('value')+",";
                    if($(result[i]).attr('code') !='')
                        buildercodestring+= $(result[i]).attr('code') +",";
                }
            }
            if (result.length > 0) {
                $("#AfterSelectedOptions").val(builderstring.substring(0,builderstring.length-1));
                
                if (buildercodestring != '') {
                    var selectedoptionCode = $("#AfterSelectedOptionCodes").val();
                    selectedoptionCode += buildercodestring.substring(0, buildercodestring.length - 1);
                    $("#AfterSelectedOptionCodes").val(selectedoptionCode);
                } else {
                    selectedoptionCode = $("#AfterSelectedOptionCodes").val();
                    selectedoptionCode = selectedoptionCode.substring(0, selectedoptionCode.length - 1);
                    $("#AfterSelectedOptionCodes").val(selectedoptionCode);
                }

            }
        }

        function warrantyInfoUpdate(checkbox) {
            if (checkbox.value != '0') {
                $.post('<%= Url.Content("~/Inventory/UpdateWarrantyInfo") %>', { WarrantyInfo: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                    if (data.SessionTimeOut == "TimeOut") {
                        alert("Your session has timed out. Please login back again");
                        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                        window.parent.location = actionUrl;
                    }
                });
            }
        }

        function priorRentalUpdate(checkbox) {
          $.post('<%= Url.Content("~/Inventory/PriorRentalUpdate") %>', { PriorRental: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                }


            });
        }
        function dealerDemoUpdate(checkbox) {
            $.post('<%= Url.Content("~/Inventory/DealerDemoUpdate") %>', { DealerDemo: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                }


            });
        }

        function unwindUpdate(checkbox) {
            $.post('<%= Url.Content("~/Inventory/UnwindUpdate") %>', { Unwind: checkbox.value, ListingId: $("#ListingId").val() }, function (data) {

                if (data.SessionTimeOut == "TimeOut") {
                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                }


            });
        }

        function changeMSRP(checkbox,price) {
           
            if (checkbox.checked) {
                $(checkbox).attr('checked', 'checked');
                var currency = $("#Msrp").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(price);
                var total = Number(number1) + Number(number2);

            } else {
                $(checkbox).removeAttr('checked');
                currency = $("#Msrp").val();
                number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                number2 = Number(price);
                total = Number(number1) - Number(number2);
            }

            $("#Msrp").attr('style', 'background-color:#3366cc');
            $("#Msrp").val(formatDollar(total));
        }

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/profile.css")%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>
