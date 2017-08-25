<%@ Page Title="" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.ModelYear %>
    <%=Model.Make %>
    <%=Model.SelectedModel %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.BeginForm("ViewProfileByVin", "Appraisal", FormMethod.Post, new { id = "viewProfileForm", name = "viewProfileForm", onsubmit = "return validateForm()" }); %>
    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="edit_appraisals_carinfo">
                <%=Html.DynamicHtmlLabel("lblTitleWithoutTrim", "TitleWithoutTrim")%>
                <%=Model.Trim%>
            </div>
            <div style="float: right">

                <input class="pad btns_shadow container_right_btns_buttons" type="submit" name="SaveEditAppraisal"
                    value="Complete Appraisal" id="SaveAppraisal" />

            </div>
        </div>
    </div>
    <div id="divDetailAppraisal">
        <%= Html.Partial("../Appraisal/DetailAppraisal", Model)%>
    </div>
      <%=Html.HiddenFor(x=>x.AfterSelectedOptions) %>
    <%=Html.HiddenFor(x=>x.AfterSelectedPackage) %>
    <%=Html.HiddenFor(x=>x.AfterSelectedOptionCodes) %>
    <input type="hidden" id="hdSelectedExteriorColor" />
    <input type="hidden" id="hdCusExteriorColor" />
    <input type="hidden" id="hdSelectedInteriorColor" />
    <input type="hidden" id="hdCusInteriorColor" />
    <% Html.EndForm(); %>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    
    <script type="text/javascript">

        $("#CancelIProfile").click(function () {

            $('#elementID').removeClass('hideLoader');
        });
        $("#SaveIProfile").click(function () {

            $('#elementID').removeClass('hideLoader');
        });

        $("a.iframe").fancybox({ 'width': 1000, 'height': 700, 'hideOnOverlayClick': false, 'centerOnScroll': true });

        $("a.smalliframe").fancybox();

        var LoadPackagesAndOptions = function (id) {
            if ($("#hdnFirstSelectedTrim").val() == id) {
                var selectedOption = $("#hdnFistSelectedOptions").val();
                var selectedPackages = $("#hdnFistSelectedPackages").val();

                if (selectedOption != '') {
                    var optionJson = JSON.parse(selectedOption);
                    $("#optionals input[name='txtNonInstalledOption']").each(function (index) {
                        if (includeItem(optionJson, $(this).attr('value'))) {
                            $(this).attr("checked", "checked");
                        }
                    });
                }

                if (selectedPackages != '') {
                    var packagesJson = JSON.parse(selectedPackages);

                    $("#Packages input[name='txtFactoryPackageOption']").each(function (index) {
                        if (includeItem(packagesJson, $(this).attr('value'))) {
                            $(this).attr("checked", "checked");
                        }
                    });
                }
            }
        };

        function includeItem(arr, obj) {
            for (var i = 0; i < arr.length; i++) {
                //                if (arr[i].replace(/^\s+|\s+$/g, '').replace(/\s\s+/g, ' ') == obj.replace(/^\s+|\s+$/g, '').replace(/\s\s+/g, ' ')) return true;
                if (arr[i].replace(/\s+/g, '') == obj.replace(/\s+/g, '')) return true;
            }
            return false;
        }

        $(document).ready(function () {
            $("#viewProfileForm").validationEngine();

            $("#Mileage").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });

            $('#MSRP').val(formatDollar(Number($('#MSRP').val().replace(/[^0-9\.]+/g, ""))));
            $('#Mileage').val(formatDollar(Number($('#Mileage').val().replace(/[^0-9\.]+/g, ""))));
            if ($("#SelectedExteriorColorValue").val() == '' && $("#SelectedExteriorColor :selected").text()) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            }

            var index = $("#SelectedTrim").val().indexOf("|");
            var id = $("#SelectedTrim").val().substring(0, index);
            
            $.post('<%= Url.Content("~/Ajax/StyleAjaxPost") %>', { styleId: id, listingId: 0 }, function (data) {

                var vehicleoptions = new Array();

                var vehiclepackages = new Array();

                $.each(data, function (i, data) {
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
                        option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',', '');
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
                        option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',', '');
                        vehiclepackages[vehiclepackages.length] = option;
                    }

                });

                var packageContent = "";
                $("#Packages").html("");

                packageContent += "<ul class='options'>";

                for (var i = 0; i < vehiclepackages.length; i++) {
                    packageContent += '<li>';
                    if (vehiclepackages[i].checked == 'checked')
                        packageContent += "<input checked='checked' class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this," + vehiclepackages[i].pureprice + ");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price + "' code='" + vehiclepackages[i].code + "' title='" + vehiclepackages[i].description + "'>";
                    else
                        packageContent += "<input class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this," + vehiclepackages[i].pureprice + ");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price + "' code='" + vehiclepackages[i].code + "' title='" + vehiclepackages[i].description + "'>";
                    if (vehiclepackages[i].code != '')
                        packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>" + "(" + vehiclepackages[i].code + ")" + vehiclepackages[i].name + " " + vehiclepackages[i].price + "</label>";
                    else
                        packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>" + vehiclepackages[i].name + " " + vehiclepackages[i].price + "</label>";
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
                    if (vehicleoptions[i].checked == 'checked')
                        optionContent += "<input  checked='checked' type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this," + vehicleoptions[i].pureprice + ")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                    else
                        optionContent += "<input type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this," + vehicleoptions[i].pureprice + ")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                    if (vehicleoptions[i].code != '')
                        optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>" + "(" + vehicleoptions[i].code + ")" + vehicleoptions[i].name + " " + vehicleoptions[i].price + "</label>";
                    else
                        optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>" + vehicleoptions[i].name + " " + vehicleoptions[i].price + "</label>";
                    optionContent += '<br>';
                    optionContent += '</li>';
                }

                optionContent += "</ul>";
                $("#Options").html(optionContent);
                $.unblockUI();
            });

            $("#SelectedExteriorColor").live('change', function (e) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
                $('#CusExteriorColor').validationEngine('hide');

                if ($("#SelectedExteriorColor").val().indexOf('Other Colors') != -1) {
                    $("#CusExteriorColor").removeAttr('disabled');
                } else {
                    //$("#CusExteriorColor").val('');
                    $("#CusExteriorColor").attr('disabled', true);
                }
            });

            $("#SelectedInteriorColor").live('change', function (e) {
                $('#CusInteriorColor').validationEngine('hide');

                if ($("#SelectedInteriorColor").val().indexOf('Other Colors') != -1) {
                    $("#CusInteriorColor").removeAttr('disabled');
                } else {
                    //$("#CusInteriorColor").val('');
                    $("#CusInteriorColor").attr('disabled', true);
                }
            });

            if ($('#Mileage').val() == "0") {
                $('#Mileage').val('');
            }
            if ($('#MSRP').val() == "0") {
                $('#MSRP').val('');
            }
            if ($('#Door').val() == "0") {
                $('#Door').val('');
            }

            var trimValue = $("#SelectedTrim").val();
            if (trimValue) {
                $("#hdnFirstSelectedTrim").val(trimValue.substring(0, trimValue.indexOf("|")));
            }
        
            $("#SelectedTrim").live('change', function () {
                blockUI();
                var name = $("#SelectedTrim option:selected").text();

                $('#hdSelectedExteriorColor').val($("#SelectedExteriorColor").val());
                $('#hdSelectedInteriorColor').val($("#SelectedInteriorColor").val());

                $('#hdCusExteriorColor').val($("#CusExteriorColor").val());
                $('#hdCusInteriorColor').val($("#CusInteriorColor").val());

                $('#CusTrim').validationEngine('hide');

                if ($("#SelectedTrim").val().indexOf('Base/Other Trims') != -1) {
                    $("#CusTrim").removeAttr('disabled');
                } else {
                    //$("#CusTrim").val('');
                    $("#CusTrim").attr('disabled', true);
                }
               
                //$.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });

                 if ($("#SelectedTrim").val().indexOf('Base/Other Trims') != -1) {
                     $("#CusTrim").removeAttr('disabled');
                 } else {
                     $("#CusTrim").attr('disabled', true);
                 }

                 var index = $("#SelectedTrim").val().indexOf("|");
                 var id = $("#SelectedTrim").val().substring(0, index);
               
                 $('#elementID').removeClass('hideLoader');
                 $.post('<%= Url.Content("~/Ajax/StyleAjaxAppraisalPost") %>', { styleId: id, listingId: 0 }, function (data) {

                    $("#Packages li").children().each(
                        function () {
                            $(this).val("");
                            $(this).removeAttr("checked");
                            $(this).attr("class", "z-index hider");
                            $(this).next("label:first").text("");
                            $(this).next("label:first").attr("class", "z-index hider");
                        }
                    );

                    $("#Options li").children().each(
                        function () {
                            $(this).val("");
                            $(this).removeAttr("checked");
                            $(this).attr("class", "z-index hider");
                            $(this).next("label:first").text("");
                            $(this).next("label:first").attr("class", "z-index hider");
                        }
                    );

                    var MSRP = "";
                    var vehicleoptions = new Array();
                    var vehiclepackages = new Array();

                    $.each(data, function (i, data) {
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
                            option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',', '');
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
                            option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',', '');
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
                            if (vehiclepackages[i].checked == 'checked')
                                packageContent += "<input checked='checked' class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this," + vehiclepackages[i].pureprice + ");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price + "' code='" + vehiclepackages[i].code + "' title='" + vehiclepackages[i].description + "'>";
                            else
                                packageContent += "<input class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this," + vehiclepackages[i].pureprice + ");' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price + "' code='" + vehiclepackages[i].code + "' title='" + vehiclepackages[i].description + "'>";
                            if (vehiclepackages[i].code != '')
                                packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>" + "(" + vehiclepackages[i].code + ")" + vehiclepackages[i].name + " " + vehiclepackages[i].price + "</label>";
                            else
                                packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>" + vehiclepackages[i].name + " " + vehiclepackages[i].price + "</label>";
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
                            if (vehicleoptions[i].checked == 'checked')
                                optionContent += "<input  checked='checked' type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this," + vehicleoptions[i].pureprice + ")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                            else
                                optionContent += "<input type='checkbox' code= '" + vehicleoptions[i].code + "' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this," + vehicleoptions[i].pureprice + ")' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                            if (vehicleoptions[i].code != '')
                                optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>" + "(" + vehicleoptions[i].code + ")" + vehicleoptions[i].name + " " + vehicleoptions[i].price + "</label>";
                            else
                                optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>" + vehicleoptions[i].name + " " + vehicleoptions[i].price + "</label>";
                            optionContent += '<br>';
                            optionContent += '</li>';
                        }

                        optionContent += "</ul>";
                        $("#Options").html(optionContent);

                        $('#elementID').addClass('hideLoader');
                        //$.unblockUI();
                        unblockUI();
                    });

                });
                if (name != '') {
                    $('#elementID').removeClass('hideLoader');
                    var id = $("#SelectedTrim").val().split('|')[0];
                    var style = $("#SelectedTrim").val().split('|')[1];
                    $.ajax({
                        type: "GET",
                        url: "/Decode/GetVehicleInformationFromStyleId?vin=" + $('#VinNumber').val() + "&styleId=" + id + "&isTruck=False&styleName=" + style + "&cusStyle=" + $("#CusTrim").val(),
                        data: {},
                        success: function (results) {
                            $("#divDetailAppraisal").html(results);

                            $('#SelectedExteriorColor').val($("#hdSelectedExteriorColor").val());
                            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
                            $('#CusExteriorColor').val($("#hdCusExteriorColor").val());

                            $('#SelectedInteriorColor').val($("#hdSelectedInteriorColor").val());
                            $('#CusInteriorColor').val($("#hdCusInteriorColor").val());

                            if ($("#SelectedTrim").val().indexOf('Base/Other Trims') === -1) {
                                $("#CusTrim").attr('disabled', true);
                            } else {
                                $("#CusTrim").removeAttr('disabled');
                            }

                            if ($("#SelectedExteriorColor").val().indexOf('Other Colors') === -1) {
                                $("#CusExteriorColor").attr('disabled', true);
                            } else {
                                $("#CusExteriorColor").removeAttr('disabled');
                            }

                            if ($("#SelectedInteriorColor").val().indexOf('Other Colors') === -1) {
                                $("#CusInteriorColor").attr('disabled', true);
                            } else {
                                $("#CusInteriorColor").removeAttr('disabled');
                            }

                            $('#elementID').addClass('hideLoader');
                            $("#Mileage").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });
                            $('#MSRP').val(formatDollar(Number($('#MSRP').val().replace(/[^0-9\.]+/g, ""))));
                            $('#Mileage').val(formatDollar(Number($('#Mileage').val().replace(/[^0-9\.]+/g, ""))));
                            unblockUI();
                        },
                        error: function (err) {
                            console.log('Server Error: ' + err.status + " - " + err.statusText);
                            $('#elementID').addClass('hideLoader');
                        }
                    });
                }
            });

        });

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

        function validateForm() {
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
           
        }

        function getAfterSelectedPakage() {
            var result = $("#Packages input[name='txtFactoryPackageOption']");
            var builderstring = '';
            var buildercodestring = '';
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {

                    builderstring += $(result[i]).attr('value') + ",";
                    if ($(result[i]).attr('code') != '')
                        buildercodestring += $(result[i]).attr('code') + ",";
                }
            }
            if (result.length > 0) {
                $("#AfterSelectedPackage").val(builderstring.substring(0, builderstring.length - 1));

                if (buildercodestring != '')
                    $("#AfterSelectedOptionCodes").val(buildercodestring);

            }
        }

        function getAfterSelectedOption() {
            var result = $("#Options input[name='txtNonInstalledOption']");
            var builderstring = '';
            var buildercodestring = '';

            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {

                    builderstring += $(result[i]).attr('value') + ",";
                    if ($(result[i]).attr('code') != '')
                        buildercodestring += $(result[i]).attr('code') + ",";
                }
            }

            if (result.length > 0) {

                $("#AfterSelectedOptions").val(builderstring.substring(0, builderstring.length - 1));

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


        function changeMSRP(checkbox, price) {

            if (checkbox.checked) {
                $(checkbox).attr('checked', 'checked');
                var currency = $("#MSRP").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(price);
                var total = Number(number1) + Number(number2);

            } else {
                $(checkbox).removeAttr('checked');
                currency = $("#MSRP").val();
                number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                number2 = Number(price);
                total = Number(number1) - Number(number2);
            }

            $("#MSRP").attr('style', 'background-color:#3366cc');
            $("#MSRP").val(formatDollar(total));
        }
    </script>
    
    <script type="text/javascript">
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
                    $.post('<%= Url.Content("~/Decode/CheckVin") %>', { vin: field.val() }, function (data) {
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

        $(document).ready(function () {
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
        } catch (e) {

        }

        try {
            if ($("#hdSelectedInteriorColor").val().indexOf('Other Colors') === -1) {
                $("#CusInteriorColor").attr('disabled', true);
            } else {
                $("#CusInteriorColor").removeAttr('disabled');
            }
        } catch (e) {

        }

        try {
            console.log($("#SelectedInteriorColor").val().indexOf('Other Colors'));
            if ($("#SelectedInteriorColor").val().indexOf('Other Colors') != -1) {
                $("#CusInteriorColor").removeAttr('disabled');
            } else {

                $("#CusInteriorColor").attr('disabled', true);
            }
        } catch (e) {

        }

    });
    </script>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Content/editappraisals.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .edit_appraisals_carinfo label
        {
            font-size: 22px !important;
        }

        .formError .formErrorContent
        {
            width: 100%;
            font-style: italic;
        }
    </style>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>