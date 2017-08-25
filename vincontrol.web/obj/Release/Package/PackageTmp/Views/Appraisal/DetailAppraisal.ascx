<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Vincontrol.Web.Models.AppraisalViewFormModel>" %>
<%@ Import Namespace="vincontrol.DomainObject" %>
<link href="<%= Url.Content("~/Content/inventory.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/editappraisals.css") %>" rel="stylesheet" type="text/css" />
<%--<script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>--%>
<%--<script type="text/javascript">
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
  
    function checkMileage(field, rules, i, options) {
        if (parseInt(field.val().replace(/,/g, "")) > 2000000) {
            return "Mileage should <= 2,000,000";
        }
    }
    
    function checkVin(field, rules, i, options) {
        if (field.val() != "" && $('#hdVin').val() != field.val()) {
            if (field.val().length == 17) {
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
                                field.val($('#hdVin').val());
                            }
                        }
                    }
                });
            } else {
                return "Invalid Vin number";
            }
        }
    }

    $(document).ready(function() {
        var firstType = '<%= Model.SelectedVehicleType %>';
        if (firstType == 'Car') $('#divTruckContainer').hide();
        else $('#divTruckContainer').show();
        $('#SelectedVehicleType').live('change', function () {
            var type = $(this).val();
            if (type == 'Car') {

                $('#SelectedTruckType option[value=""]').attr("selected", "selected");
                $("#SelectedTruckCategoryId").html('<option value="">Select...</option>');
                $('#SelectedTruckCategoryId option[value=""]').attr("selected", "selected");
                $('#SelectedTruckClassId option[value=""]').attr("selected", "selected");
                $('#divTruckContainer').hide();
            }
            else $('#divTruckContainer').show();
        });

        $('#SelectedTruckType').live('change', function () {
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
                        $.each(categories, function (index, category) {
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

        $("#imgTruck").live('click', function () {
            $('#divTruck').show();
            $.fancybox({
                href: "#divTruck",
                'onCleanup': function () {
                    $('#divTruck').hide();
                }
            });
        });

        if ($("#SelectedTrim").val().indexOf('Base/Other Trims') === -1) {
            $("#CusTrim").attr('disabled', true);
        }

        try {
            if ($("#hdSelectedExteriorColor").val() != 'Other Colors') {
                $("#CusExteriorColor").attr('disabled', true);
            }
            else {
                $("#CusExteriorColor").removeAttr('disabled');
            }
        } catch(e) {

        }

        try {
            if ($("#hdSelectedInteriorColor").val().indexOf('Other Colors') === -1) {
                $("#CusInteriorColor").attr('disabled', true);
            } else {
                $("#CusInteriorColor").removeAttr('disabled');
            }
        } catch(e) {

        }

        try {
            console.log($("#SelectedInteriorColor").val().indexOf('Other Colors'));
            if ($("#SelectedInteriorColor").val().indexOf('Other Colors') != -1) {
                $("#CusInteriorColor").removeAttr('disabled');
            } else {
               
                $("#CusInteriorColor").attr('disabled', true);
            }
        } catch(e) {

        }
        
    });
</script>--%>
<input type="hidden" id="hdVin" value="<%=Model.VinNumber %>" />
<div id="container_right_content" style="height: 570px">
   
    <div class="right_cont_column_one">
        <div class="edit_appraisals_img">
            
                    <img alt="" style="max-height: 240px;"src="<%= Model.FirstPhoto %>" width="160">
                  
            <%= Html.HiddenFor(x => x.AppraisalGenerateId) %>
        </div>
       
        <div class="column_one_part">
                <div class="column_one_items">
                    <label>Date</label>
                    <label><%= Model.AppraisalDate %></label>
                       
                    
                </div>
            <div class="column_one_items">
                <label>
                    VIN</label>
                <% if (Model.VinDecodeSuccess)
                   { %>
                 
                <input type="text" name="VinNumber" id="VinNumber" value="<%= Model.VinNumber %>"
                                class="column_one_input readonly" readonly="readonly" data-validation-engine="validate[funcCall[checkVin]" data-errormessage-value-missing="test" />
                <% }
                   else
                   { %>
                  
                <input type="text" name="VinNumber" id="VinNumber" value="<%= Model.VinNumber %>"
                                class="column_one_input" data-errormessage-value-missing="test" />
                <% } %>
            </div>
           
            <div class="column_one_items">
                <label>
                    Year</label>
                <% if (Model.VinDecodeSuccess)
                   { %>
                <%= Html.TextBoxFor(x => x.ModelYear, new {@class = "column_one_input readonly", @readonly = "readonly"}) %>
                <% }
                   else
                   { %>
                <%= Html.TextBoxFor(x => x.ModelYear, new {@class = "column_one_input"}) %>
                <% } %>
            </div>
            <div class="column_one_items">
                <label>
                    Make</label>
                <% if (Model.VinDecodeSuccess)
                   { %>
                <%= Html.TextBoxFor(x => x.Make, new {@class = "column_one_input readonly", @readonly = "readonly"}) %>
                <% }
                   else
                   { %>
                <%= Html.TextBoxFor(x => x.Make, new {@class = "column_one_input"}) %>
                <% } %>
            </div>
            <div class="column_one_items">
                <label>
                    Model</label>
                <% if (Model.VinDecodeSuccess)
                   { %>
                <%= Html.TextBoxFor(x => x.SelectedModel, new {@class = "column_one_input readonly", @readonly = "readonly"}) %>
                <% }
                   else
                   { %>
                <%= Html.TextBoxFor(x => x.SelectedModel, new {@class = "column_one_input"}) %>
                <% } %>
            </div>
            <div class="column_one_items" style="">
                <label>
                    Vehicle Type</label>
                <% if (Model.VehicleTypeList != null) %>
                <%{%>
                <%= Html.DropDownListFor(x => x.SelectedVehicleType, Model.VehicleTypeList, new {@class = "DDLEditColum2", @style=""}) %>
                <%= Html.Partial("../Appraisal/_TruckPartial", Model)%>
                <%}%>
            </div>

        </div>
    </div>
    <div class="right_cont_column_two">
        <div class="column_two_part" style="margin-top: 5px !important; padding: 8px 0px 5px 0px;">
            <div class="column_two_items" style="height: 50px;">
                <div>
                    <label>
                        Trim</label>
                    <%= Html.DropDownListFor(x => Model.SelectedTrimItem, Model.TrimList, new {@class = "column_one_input", id = "SelectedTrim"}) %>
                </div>
                <div class="others">
                    Other:
                    <%= Html.TextBoxFor(x => x.CusTrim, new { @class = "exterior_others", MaxLength = 50 })%>
                </div>
            </div>
            <div class="column_two_items">
                <label>
                    Cylinders</label>
                   
                <% if (Model.VinDecodeSuccess && Model.CylinderList != null)
                   {
                %>
                    <%= Html.DropDownListFor(x => x.SelectedCylinder, Model.CylinderList) %>
                <%
                   }
                   else
                   { %>
            
                  <%= Html.TextBoxFor(x => x.SelectedCylinder, new {@class = "column_one_input"}) %>
                <% } %>
            </div>
            <div class="column_two_items">
                <label>
                    Liters</label>
                <% if (Model.VinDecodeSuccess && Model.LitersList != null)
                   {
                %>
                    <%= Html.DropDownListFor(x => x.SelectedLiters, Model.LitersList) %>
                <%
                   }
                   else
                   { %>
            
                  <%= Html.TextBoxFor(x => x.SelectedLiters, new {@class = "column_one_input"}) %>
                <% } %>
            
            </div>
            <div class="column_two_items">
                <label>
                    Doors</label>
                <%= Html.TextBoxFor(x => x.Door, new {@class = "column_one_input"}) %>
            </div>
            <div class="column_two_items">
                <label>
                    Style</label>
                     
                <% if (Model.VinDecodeSuccess && Model.BodyTypeList != null)
                   {
                %>
                    <%= Html.DropDownListFor(x => x.SelectedBodyType, Model.BodyTypeList) %>
                <%
                   }
                   else
                   { %>
            
                  <%= Html.TextBoxFor(x => x.SelectedBodyType, new {@class = "column_one_input"}) %>
                <% } %>
            
            </div>
            <div class="column_two_items">
                <label>
                    Fuel</label>
                      <% if (Model.VinDecodeSuccess && Model.FuelList != null)
                   {
                %>
                    <%= Html.DropDownListFor(x => x.SelectedFuel, Model.FuelList) %>
                <%
                   }
                   else
                   { %>
            
                  <%= Html.TextBoxFor(x => x.SelectedFuel, new {@class = "column_one_input"}) %>
                <% } %>
               
            </div>
            <div class="column_two_items">
                <label>
                    Drive</label>
                <%= Html.DropDownListFor(x => x.SelectedDriveTrain, Model.DriveTrainList) %>
            </div>
            <div class="column_two_items">
                <label>
                    Original MSRP</label>
                <%= Html.TextBoxFor(x => x.MSRP, new {@class = "column_one_input"}) %>
            </div>
            <div class="column_two_items" style="height: 50px;">
                <div>
                    <label>
                        Exterior Color</label>
                  <%if (Model.ExteriorColorList == null) { Model.ExteriorColorList = new List<ExtendedSelectListItem>(); } %>

                    <%= Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ExteriorColorList, new {id = "SelectedExteriorColor", @style = "width: 59%;"}) %>
                    <input type="hidden" name="SelectedExteriorColorValue" id="SelectedExteriorColorValue"
                        value="<%= Model.SelectedExteriorColorValue %>" />
                </div>
                <div class="others">
                    Other:
                    <input type="text" name="CusExteriorColor" id="CusExteriorColor" value="<%= Model.CusExteriorColor %>" maxlength="50" class = "exterior_others" data-validation-engine="validate[funcCall[checkOtherExteriorColor]" data-errormessage-value-missing="Other exterior color is required!" />
                </div>
            </div>
            <div class="column_two_items" style="height: 50px;">
                <div>
                    <label>
                        Interior Color</label>
                  <%if (Model.InteriorColorList == null) { Model.InteriorColorList = new List<ExtendedSelectListItem>(); } %>

                    <%= Html.DropDownListFor(x => x.SelectedInteriorColor, Model.InteriorColorList, new {@style = "width: 59%;"}) %>
                </div>
                <div class="others">
                    Other:
                    <input type="text" name="CusInteriorColor" id="CusInteriorColor"  value="<%= Model.CusInteriorColor %>" maxlength="50" class = "interior_others" data-validation-engine="validate[funcCall[checkOtherInteriorColor]" data-errormessage-value-missing="Other interior color is required!" />
                </div>
            </div>
            <div class="column_two_items">
                <label style="width: 34%;">Odometer (*)</label>
                    <input type="text" name="Mileage" id="Mileage" value="<%= Model.Mileage %>" class = "column_one_input" data-validation-placeholder="0" data-validation-engine="validate[required,funcCall[checkMileage]]" data-errormessage-value-missing="Odometer is required!" autocomplete="off" />
            </div>
            <div class="column_two_items" style="padding-bottom: 5px;">
                <label>
                    Tranmission (*)</label>
           
                <select name="SelectedTranmission" id="SelectedTranmission" style = "width: 59%;" data-validation-engine="validate[required]" data-errormessage-value-missing="Transmission is required!">
                    <% foreach (var item in Model.TranmissionList)
                       { %>
                    <option value="<%= item.Value %>" <%= item.Value.Equals(Model.SelectedTranmission) ? "selected" : "" %>><%= item.Text %></option>                             
                    <% } %>
                </select>
            </div>
        </div>
    </div>
    <div class="right_cont_column_three">
          <div id="packages_header"> 
        <% if (Model.Make != null)
           {
               if (Model.Make.Equals("Ford"))
               { %>
        
        
            Packages:  <a class="iframe" href="http://fordlabels.webview.biz/webviewhybrid/WindowSticker.aspx?vin=<%= Model.VinNumber %>">Original WS</a>
        
       
        <% }
               else if (Model.Make.Equals("Chrysler"))
               { %>
           
              
            Packages:  <a class="iframe" href="http://www.chrysler.com/hostd/windowsticker/getWindowStickerPdf.do?vin=<%= Model.VinNumber %>">Original WS</a>
              
              <% }
               else if (Model.Make.Equals("Dodge"))
               { %>
           
              
            Packages:  <a class="iframe" href="http://www.chrysler.com/hostd/windowsticker/getWindowStickerPdf.do?vin=<%= Model.VinNumber %>">Original WS</a>
              
                <% }
               else if (Model.Make.Equals("Jeep"))
               { %>
           
              
            Packages:  <a class="iframe" href="http://www.chrysler.com/hostd/windowsticker/getWindowStickerPdf.do?vin=<%= Model.VinNumber %>">Original WS</a>

        <% }
           }
           else
           { %>
            Packages:  
        <% } %>
  </div>
        <div id="packages_container">
            <%= Html.CheckBoxGroupPackage("txtFactoryPackageOption", Model.FactoryPackageOptions,Model.ExistPackages)%>
            <input type="hidden" id="hdnFirstSelectedTrim" />
           <%= Html.HiddenFor(x=>x.SelectedPackagesDescription) %>
        </div>
        <div id="options_header">
            Options:
        </div>
        <div id="options_container">
            <%= Html.CheckBoxGroupOption("txtNonInstalledOption", Model.FactoryNonInstalledOptions,Model.ExistOptions)%>
        </div>
    </div>
    <div style="clear: both">
        <div id="descriptions_header">
            Notes:
        </div>
        <div id="descriptions_container">
            <%=Html.TextAreaFor(x=>x.Notes,new {@cols=121, @rows=7})%>
        </div>
    </div>
</div>
<%=Html.HiddenFor(x=>x.AppraisalGenerateId) %>
<%=Html.HiddenFor(x=>x.FuelEconomyCity) %>
<%=Html.HiddenFor(x=>x.FuelEconomyHighWay) %>
<%=Html.HiddenFor(x=>x.DefaultImageUrl) %>
<%=Html.HiddenFor(x => x.StandardInstalledOption)%>

