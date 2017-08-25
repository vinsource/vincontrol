<%@ Page Title="" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.ModelYear %>
    <%=Model.Make %>
    <%=Model.SelectedModel %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.BeginForm("Action", "Appraisal", FormMethod.Post, new { id = "editiProfileForm", name = "editiProfileForm", onsubmit = "return validateForm()" }); %>

    <div id="container_right_btn_holder">
        <div id="container_right_btns">
            <div class="edit_appraisals_carinfo">
                <%=Html.DynamicHtmlLabel("lblTitleWithoutTrim", "TitleWithoutTrim")%>
                <%=Model.Trim%>
            </div>
            <div style="float: right; width: 400px">
                <a id="manage_images" class="iframe btns_shadow container_right_btns_buttons"
                    href="<%=Url.Action("OpenSilverlightUploadWindow","Appraisal",new{appraisalId=Model.AppraisalGenerateId}) %>">Manage Images</a>

                <% if ((bool)Session["AllowSave"] == true)
                   { %>
                <input class="pad btns_shadow container_right_btns_buttons" type="submit" name="SaveEditAppraisal"
                    value=" Save Changes " id="SaveAppraisal" />
                <% } %>

                <a id="cancel_change_profile" class="btns_shadow container_right_btns_buttons"
                    href="<%=Url.Action("ViewProfileForAppraisal","Appraisal",new{AppraisalId=Model.AppraisalGenerateId}) %>">Cancel</a>
            </div>
        </div>
    </div>
    <div id="divDetailAppraisal">
        <%= Html.Partial("DetailAppraisal", Model) %>
    </div>
    <input type="hidden" id="hdSelectedExteriorColor" />
    <input type="hidden" id="hdCusExteriorColor" />
    <input type="hidden" id="hdSelectedInteriorColor" />
    <input type="hidden" id="hdCusInteriorColor" />
    <%=Html.HiddenFor(x=>x.AfterSelectedOptions) %>
    <%=Html.HiddenFor(x=>x.AfterSelectedPackage) %>
    <%=Html.HiddenFor(x=>x.AfterSelectedOptionCodes) %>
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

     
        function includeItem(arr, obj) {
            for (var i = 0; i < arr.length; i++) {

                if (arr[i].replace(/\s+/g, '') == obj.replace(/\s+/g, '')) return true;
            }
            return false;
        }

        $(document).ready(function () {
            var index = $("#SelectedTrim").val().indexOf("|");
            var id = $("#SelectedTrim").val().substring(0, index);
            var listingId = $('#AppraisalGenerateId').val();
            $.post('<%= Url.Content("~/Ajax/StyleAjaxAppraisalPost") %>', { styleId: id, listingId: listingId }, function (data) {

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

            jQuery("#editiProfileForm").validationEngine();

            $("#MSRP").val(formatDollar(Number($("#MSRP").val().replace(/[^0-9\.]+/g, ""))));
            $("#Mileage").val(formatDollar(Number($("#Mileage").val().replace(/[^0-9\.]+/g, ""))));

            $("#Mileage").numeric({ decimal: false, negative: false }, function () { ShowWarningMessage("Positive integers only"); this.value = ""; this.focus(); });

            if ($("#SelectedExteriorColorValue").val() == '' && $("#SelectedExteriorColor :selected").text()) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            }

            $("#SelectedExteriorColor").live('change', function (e) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
                $('#CusExteriorColor').validationEngine('hide');

                if ($("#SelectedExteriorColorValue").val().indexOf('Other Colors') != -1) {
                    $("#CusExteriorColor").removeAttr('disabled');
                } else {
                    //$("#CusExteriorColor").val('');
                    $("#CusExteriorColor").attr('disabled', true);
                }
                $('#hdSelectedExteriorColor').val($("#SelectedExteriorColor").val());
            });

            $("#SelectedInteriorColor").live('change', function (e) {
                $('#CusInteriorColor').validationEngine('hide');
                if ($("#SelectedInteriorColor").val().indexOf('Other Colors') != -1) {
                    $("#CusInteriorColor").removeAttr('disabled');
                } else {
                  
                    $("#CusInteriorColor").attr('disabled', true);
                }
                $('#hdSelectedInteriorColor').val($("#SelectedInteriorColor").val());
            });

            if ($("#SelectedTrim").val().indexOf('Base/Other Trims') === -1) {
                $("#CusTrim").attr('disabled', true);
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

            var trimValue = $("#SelectedTrim").val();
            if (trimValue) {
                $("#hdnFirstSelectedTrim").val(trimValue.substring(0, trimValue.indexOf("|")));
            }

            $("#imgTruck").click(function () {
                $('#divTruck').show();
                $.fancybox({
                    href: "#divTruck",
                    'onCleanup': function () {
                        $('#divTruck').hide();
                    }
                });
            });
        });

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

        $("#SelectedTrim").change(function () {
            $('#CusTrim').validationEngine('hide');
            $.blockUI({ message: '<div><img src="<%= Url.Content("~/images/ajaxloadingindicator.gif") %>" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });

                if ($("#SelectedTrim").val().indexOf('Base/Other Trims') != -1) {
                    $("#CusTrim").removeAttr('disabled');
                } else {
                    $("#CusTrim").attr('disabled', true);
                }

                var index = $("#SelectedTrim").val().indexOf("|");
                var id = $("#SelectedTrim").val().substring(0, index);
                var listingId = $('#AppraisalGenerateId').val();
                $('#elementID').removeClass('hideLoader');
                $.post('<%= Url.Content("~/Ajax/StyleAjaxAppraisalPost") %>', { styleId: id, listingId: listingId }, function (data) {

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
                        $.unblockUI();
                    });

                });
            });

        function changeMSRP(checkbox, price) {

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