<%@ Page Title="New Appraisal" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    New Appraisal
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/inventory.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/editappraisals.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/ui.datepicker.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #title
        {
            font-size: 25px;
            margin-left: 20px;
            font-weight: bold;
            float: left;
            color: White;
        }

        .formError .formErrorContent
        {
            width: 100%;
            font-style: italic;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientScripts" runat="server">
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("ViewProfileByManualYear", "Appraisal", FormMethod.Post, new { id = "viewProfileForm", onsubmit = "return validateForm()" }); %>
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div id="title" style="color: white!important">
            </div>
            <div style="float: right">
                <%--<% if ((bool)Session["IsEmployee"] == false)
                   { %>--%>
                <input class="pad btns_shadow container_right_btns_buttons" type="submit" name="SaveEditAppraisal"
                    value="Complete Appraisal" id="SaveAppraisal" />
                <%--<% } %>--%>
            </div>
        </div>
    </div>
    <div id="divDetailAppraisal">
        <div id="container_right_content" style="height: 570px">
            <div class="right_cont_column_one">
                <div class="edit_appraisals_img">
                    <img width="160px" height="130px" src="/Content/Images/noImageAvailable.jpg" name="pIdImage" id="pIdImage">
                </div>
                <div class="column_one_part">
                    <div class="column_one_items">
                        <label>
                            Date</label>
                        <label>
                            <%= Model.AppraisalDate %></label>
                    </div>
                    <div class="column_one_items">
                        <label>
                            VIN</label>

                        <input type="text" name="VinNumber" id="VinNumber" value="<%= Model.VinNumber %>" class="column_one_input" />



                    </div>
                    <div class="column_one_items">
                        <label>
                            Year</label>
                        <input type="text" name="ModelYear" id="ModelYear" value="<%= Model.ModelYear %>" class="column_one_input" />

                    </div>
                    <div class="column_one_items">
                        <label>
                            Make</label>

                        <input type="text" name="Make" id="Make" maxlength="50" value="<%= Model.Make %>"
                            class="column_one_input" />
                    </div>
                    <div class="column_one_items">
                        <label>
                            Model</label>

                        <input type="text" name="AppraisalModel" id="AppraisalModel" maxlength="50" value="<%= Model.AppraisalModel %>"
                            class="column_one_input" data-validation-engine="validate[required,funcCall[checkSelectedModel]]"
                            data-errormessage-value-missing="Model is required!" />
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
                <div class="column_two_part" style="margin-top: 5px !important;">
                    <div class="column_two_items" style="height: 0px;">
                        <div>
                            <label>
                                Trim</label>

                            <input type="text" name="SelectedTrim" id="SelectedTrim" maxlength="50" value="<%= Model.SelectedTrim %>"
                                class="column_one_input" />
                        </div>

                    </div>
                    <div class="column_two_items">
                        <label>
                            Cylinders</label>

                        <input type="text" name="SelectedCylinder" id="SelectedCylinder" maxlength="50"
                            class="column_one_input" />
                    </div>
                    <div class="column_two_items">
                        <label>
                            Liters</label>

                        <input type="text" name="SelectedLiters" id="SelectedLiters" maxlength="50"
                            class="column_one_input" />
                    </div>
                    <div class="column_two_items">
                        <label>
                            Doors</label>
                        <input type="text" name="Door" id="Door" maxlength="50"
                            class="column_one_input" />

                    </div>
                    <div class="column_two_items">
                        <label>
                            Style</label>

                        <input type="text" name="SelectedBodyType" id="SelectedBodyType" maxlength="50" value="<%= Model.SelectedBodyType %>"
                            class="column_one_input" />
                    </div>
                    <div class="column_two_items">
                        <label>
                            Fuel</label>

                        <input type="text" name="SelectedFuel" id="SelectedFuel" maxlength="50" value="<%= Model.SelectedFuel %>"
                            class="column_one_input" data-validation-engine="" />
                    </div>
                    <div class="column_two_items">
                        <label>
                            Drive</label>
                        <%= Html.DropDownListFor(x => x.SelectedDriveTrain, Model.DriveTrainList) %>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Original MSRP</label>
                        <input type="text" name="MSRP" id="MSRP" maxlength="50" class="column_one_input" />
                    </div>
                    <div class="column_two_items" style="">
                        <div>
                            <label>
                                Exterior Color</label>

                            <input type="text" name="SelectedExteriorColorValue" id="SelectedExteriorColorValue" maxlength="50" value="<%= Model.SelectedExteriorColorValue %>"
                                class="column_one_input" />

                        </div>

                    </div>
                    <div class="column_two_items" style="">
                        <div>
                            <label>
                                Interior Color</label>

                            <input type="text" name="SelectedInteriorColor" id="SelectedInteriorColor" maxlength="50" value="<%= Model.SelectedInteriorColor %>"
                                class="column_one_input" data-validation-engine=""
                                data-errormessage-value-missing="Interior Color is required!" />
                        </div>

                    </div>
                    <div class="column_two_items">
                        <label style="width: 34%;">Odometer (*)</label>
                        <input type="text" name="Mileage" id="Mileage" value="<%= Model.Mileage %>" class="column_one_input" data-validation-placeholder="0" data-validation-engine="validate[required,funcCall[checkMileage]]" data-errormessage-value-missing="Odometer is required!" autocomplete="off" />
                    </div>
                    <div class="column_two_items">
                        <label>
                            Tranmission (*)</label>

                        <%= Html.CusDropDown("SelectedTranmission", "DDLStyle", string.Empty, Model.TranmissionList, "validate[required]", "Transmission is required!") %>
                    </div>
                </div>
            </div>
            <div class="right_cont_column_three">
                <div id="packages_header">
                    Packages:
                </div>
                <div id="packages_container">
                    <%= Html.CheckBoxGroupPackageByYear("txtFactoryPackageOption") %>
                    <input type="hidden" id="hdnFirstSelectedTrim" />
                    <input type="hidden" id="hdnFistSelectedOptions" />
                    <input type="hidden" id="hdnFistSelectedPackages" />
                    <%= Html.HiddenFor(x => x.SelectedPackagesDescription) %>
                </div>
                <div id="options_header">
                    Options:
                </div>
                <div id="options_container">
                    <%= Html.CheckBoxGroupOptionByYear("txtNonInstalledOption") %>
                </div>
            </div>
            <div style="clear: both">
                <div id="descriptions_header">
                    Notes:
                </div>
                <div id="descriptions_container">
                    <%= Html.TextAreaFor(x => x.Notes, new {@cols = 121, @rows = 3}) %>
                </div>
            </div>
        </div>
        <%= Html.HiddenFor(x => x.AppraisalGenerateId) %>
        <%= Html.HiddenFor(x => x.FuelEconomyCity) %>
        <%= Html.HiddenFor(x => x.FuelEconomyHighWay) %>
        <%= Html.HiddenFor(x => x.DefaultImageUrl) %>
        <%= Html.HiddenFor(x => x.StandardInstalledOption) %>
        <%= Html.HiddenFor(x => x.ChromeModelId) %>
        <%= Html.HiddenFor(x => x.ChromeStyleId) %>
        <%= Html.HiddenFor(x => x.DefaultImageUrl) %>
        <input type="hidden" id="hdCurrentYear" value="<%= Model.ModelYear %>" />

        <input type="hidden" id="hdSelectedExteriorColor" />
        <input type="hidden" id="hdCusExteriorColor" />
        <input type="hidden" id="hdSelectedInteriorColor" />
        <input type="hidden" id="hdCusInteriorColor" />
    </div>
    <% Html.EndForm(); %>
    <script type="text/javascript">
        function checkMileage(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 2000000) {
                return "Mileage should <= 2,000,000";
            }
        }


        function checkSelectedMake(field, rules, i, options) {
            if (field.val() == '0') {
                return "Make is required!";
            }
        }

        function checkSelectedModel(field, rules, i, options) {
            if (field.val() == '0') {
                return "Model is required!";
            }
        }

        function Reset() {
            if ($('#SelectedMake').val() != 0) {
                $('#SelectedMake').val('0').trigger('change');
                $('ul.options').html('');
                $('#SelectedModel').val('0').trigger('change');
                $('#SelectedModel').html('');
                $('#pIdImage').attr("src", "/Content/Images/noImageAvailable.jpg");
            }
        }

        $(function () {
            jQuery("#viewProfileForm").validationEngine();
            if ($('#Mileage').val() == 0) {
                $('#Mileage').val('');
            }
            if ($('#MSRP').val() == 0) {
                $('#MSRP').val('');
            }
            if ($('#Door').val() == 0) {
                $('#Door').val('');
            }

            $("#Mileage").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
            $("#SelectedCylinder").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
            $("#SelectedLiters").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
            $("#Door").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });


            $("#ModelYear").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                    // Allow: Ctrl+A
                    (event.keyCode == 65 && event.ctrlKey === true) ||
                    // Allow: home, end, left, right
                    (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                } else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });

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
        });




        $("#btnCar").click(function () {


            $('#elementID').removeClass('hideLoader');

            var modelYear = $('#ModelYear').val();

            var actionUrl = '<%= Url.Action("DecodeProcessingByYear", "Decode", new { Year = "PLACEHOLDER" } ) %>';

            actionUrl = actionUrl.replace('PLACEHOLDER', modelYear);
            window.location = actionUrl;

        });

        $("#btnTruck").click(function () {

            $('#elementID').removeClass('hideLoader');

            var modelYear = $('#ModelYear').val();

            var actionUrl = '<%= Url.Action("DecodeProcessingByYearForTruck", "Decode", new { Year = "PLACEHOLDER" } ) %>';

            actionUrl = actionUrl.replace('PLACEHOLDER', modelYear);

            window.location = actionUrl;

        });

    </script>
</asp:Content>
