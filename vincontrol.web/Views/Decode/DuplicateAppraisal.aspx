<%@ Page Title="" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Model.ModelYear %>
    <%=Model.Make %>
    <%=Model.SelectedModel %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.BeginForm("ViewProfileByVin", "Appraisal", FormMethod.Post, new { id = "viewProfileForm", onsubmit = "return validateForm()" }); %>
    <div class="column" style="font-size: 0.9em;">
        <h3>
            <%=Html.DynamicHtmlLabel("lblTitleWithoutTrim", "TitleWithoutTrim")%></h3>
        <h5>
            <%=Model.Trim%></h5>
        <table>
            <tr>
                <td>VIN
                </td>
                <td>
                    <%= Html.TextBoxFor(x=>x.VinNumber,new { @class="z-index"})%>
                </td>
            </tr>
            <tr>
                <td>Stock #
                </td>
                <td>
                    <%= Html.TextBoxFor(x=>x.StockNumber,new { @class="z-index"})%>
                </td>
            </tr>
            <tr>
                <td>Date
                </td>
                <td>
                    <%=  Html.DynamicHtmlControlAppraisal("txtDate", "Date")%>
                </td>
            </tr>
            <tr>
                <td>Year
                </td>
                <td>
                    <%= Html.TextBoxFor(x=>x.ModelYear,new { @class="z-index"})%>
                </td>
            </tr>
            <tr>
                <td>Make
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Make, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>Model
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.SelectedModel, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>Trim
                </td>
                <%--  <td><%= Html.TextBoxFor(x => x.Trim, new { @class = "z-index" })%></td>--%>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedTrim, Model.TrimList, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%= Html.TextBoxFor(x => x.CusTrim, new { @style = "width: 70px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>Exterior Color
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ExteriorColorList, new { id = "SelectedExteriorColor" })%>
                    <input type="hidden" name="SelectedExteriorColorValue" id="SelectedExteriorColorValue"
                        value="<%=Model.SelectedExteriorColorValue %>" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%= Html.TextBoxFor(x => x.CusExteriorColor, new { @style = "width: 70px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>Interior Color
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.InteriorColorList) %>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%= Html.TextBoxFor(x => x.CusInteriorColor, new {  @style = "width: 70px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>Vehicle Type
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedVehicleType, Model.VehicleTypeList)%>
                </td>
            </tr>
            <tr>
                <td>Odometer
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Mileage, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>Cylinders
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.SelectedCylinder, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>Liters
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.SelectedLiters, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>Transmission
                </td>
                <td>
                    <%= Html.DropDownListFor(x => x.SelectedTranmission, Model.TranmissionList)%>
                </td>
            </tr>
            <tr>
                <td>Doors
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.Door,  new { @class = "small" })%>
                </td>
            </tr>
            <tr>
                <td>Style
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.SelectedBodyType, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>Fuel
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.SelectedFuel, new { @class = "z-index" })%>
                </td>
            </tr>
            <tr>
                <td>Drive
                </td>
                <td>
                    <%= Html.DropDownListFor(x => x.SelectedDriveTrain, Model.DriveTrainList)%>
                </td>
            </tr>
            <tr>
                <td>Original MSRP
                </td>
                <td>
                    <%= Html.TextBoxFor(x => x.MSRP, new { @class = "z-index" })%>
                </td>
            </tr>
         
        </table>
        <input type="hidden" id="VehicleType" name="VehicleType" value="<%= Model.VehicleType %>" />
        <input type="hidden" id="DefaultImageUrl" name="DefaultImageUrl" value="<%= Model.DefaultImageUrl %>" />
        <input type="hidden" id="AppraisalModel" name="AppraisalModel" value="<%= Model.AppraisalModel %>" />
        <input type="hidden" id="Location" name="Location" value="<%= Model.Location %>" />
    </div>
    <div class="column" id="column-two" style="font-size: 0.9em;">
        <input id="submit" class="pad" type="submit" name="submit" value="Complete Appraisal >" id="submitBTN" />
        <br />
        <br />
        Packages:<br />
        <div class="scrollable-list">
            <%= Html.CheckBoxGroupPackage("txtFactoryPackageOption", Model.FactoryPackageOptions,Model.ExistPackages)%>
            <input type="hidden" id="hdnFirstSelectedTrim" />
            <input type="hidden" id="hdnFistSelectedOptions" />
            <input type="hidden" id="hdnFistSelectedPackages" />
        </div>
        <%= Html.HiddenFor(x=>x.SelectedPackagesDescription) %>
        <div class="clear">
            Options:
        </div>
        <div class="scrollable-list">
            <div id="optionals" class="" style="font-size: .9em;">
                <div class="clear">
                </div>
                <%= Html.CheckBoxGroupOption("txtNonInstalledOption", Model.FactoryNonInstalledOptions,Model.ExistOptions)%>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div class="clear" id="img-title">
    </div>
    <div id="other-details" class="clear">
        <div id="description-box" class="column">
            <%=Html.TextAreaFor(x=>x.Notes,new {@cols=90, @rows=9})%>
        </div>
    </div>
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
            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());

            var trimValue = $("#SelectedTrim").val();
            if (trimValue) {
                $("#hdnFirstSelectedTrim").val(trimValue.substring(0, trimValue.indexOf("|")));
            }
            getSelectedOption();
            getSelectedPakage();

            $("#SelectedExteriorColor").change(function (e) {
                $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            });

            $("#SelectedVehicleType").change(function (e) {

                $('#elementID').removeClass('hideLoader');

                var VehicleType = $("#SelectedVehicleType").val();

                if (VehicleType == 'Truck') {
                    var actionUrl = '<%= Url.Action("EditAppraisalForTruck", "Appraisal", new { appraisalId = "PLACEHOLDER" } ) %>';

                    actionUrl = actionUrl.replace('PLACEHOLDER', $("#AppraisalGenerateId").val());

                    window.location = actionUrl;
                }

                $('#elementID').addClass('hideLoader');
            });

            var Trim = $("#SelectedTrim");
            Trim.change(function () {

                var index = Trim.val().indexOf("|");
                var id = Trim.val().substring(0, index);
                $('#elementID').removeClass('hideLoader');
                $.post('<%= Url.Content("~/Ajax/StyleAjaxPost") %>', { styleId: id, listingId: 0 }, function (data) {

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
                    var exitems = "";
                    var initems = "";
                    var MSRP = "";
                    var cylinders = "";
                    var fuel = "";
                    var litters = "";
                    var trans = "";
                    var wheeldrive = "";
                    var bodytype = "";
                    var stockPhoto = "";
                    var vehicleoptions = new Array();

                    var vehiclepackages = new Array();
                    var exteriorFlag = true;
                    var doors = "";
                    var selectedTransmission = "";
                    $.each(data, function (i, data) {
                        var text;
                        if (data.toString() == "Automatic") {
                            selectedTransmission = "Automatic";
                        }
                        else if (data.toString() == "Manual") {
                            selectedTransmission = "Manual";
                        }
                        else if (data.toString().indexOf("BodyType") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            bodytype += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Optional") != -1) {
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
                        else if (data.toString().indexOf("Package") != -1) {
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
                        else if (data.toString().indexOf("Fuel") != -1) {
                            text = data.toString().substring(0, data.toString().length - 4);
                            fuel += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Litter") != -1) {
                            text = data.toString().substring(0, data.toString().length - 6);
                            litters += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Cylinder") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            cylinders += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("MSRP") != -1) {
                            MSRP = data.toString().substring(0, data.toString().length - 4);
                            MSRP = Number(MSRP);
                            MSRP = formatDollar(MSRP);
                        }
                        else if (data.toString().indexOf("Exterior") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            var textList = text.split("|");
                            if (exteriorFlag) {
                                $("#SelectedExteriorColorValue").val(textList[0]);
                                exteriorFlag = false;
                            }

                            exitems += "<option value='" + textList[1] + "'>" + textList[0] + "</option>";
                        }
                        else if (data.toString().indexOf("Interior") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            initems += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("PassengerDoors") != -1) {
                            text = data.toString().substring(0, data.toString().length - 14);
                            doors += text;
                        }
                        else if (data.toString().indexOf("WheelDrive") != -1) {
                            text = data.toString().substring(0, data.toString().length - 10);
                            wheeldrive += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("DefaultImage") != -1) {
                            text = data.toString().substring(0, data.toString().length - 12);
                            stockPhoto = text;
                        }
                    });

                    if (selectedTransmission == "Automatic") {
                        trans += "<option value='Automatic' selected='selected'>Automatic</option>";
                    } else {
                        trans += "<option value='Automatic'>Automatic</option>";
                    }

                    if (selectedTransmission == "Manual") {
                        trans += "<option value='Manual' selected='selected'>Manual</option>";
                    } else {
                        trans += "<option value='Manual'>Manual</option>";
                    }

                    trans += "<option value='Shiftable Automatic Transmission'>Shiftable Automatic Transmission</option>";

                    exitems += "<option value='Other Colors'>Other Colors</option>";

                    initems += "<option value='Other Colors'>Other Colors</option>";

                    $("#SelectedExteriorColor").html(exitems);
                    $("#SelectedInteriorColor").html(initems);
                    $("#SelectedCylinder").html(cylinders);
                    $("#SelectedFuel").html(fuel);
                    $("#SelectedLiters").html(litters);
                    $("#SelectedTranmission").html(trans);
                    $("#SelectedDriveTrain").html(wheeldrive);
                    $("#SelectedBodyType").html(bodytype);
                    $("#MSRP").val(MSRP);

                    $("#Door").val(doors);
                    $("#ChromeStyleId").val(id);
                    $("#DefaultImageUrl").val(stockPhoto);

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
        }

        function getSelectedPakage() {
            var result = $("#Packages input[name='txtFactoryPackageOption']");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {
                    descriptionArray.push($(result[i]).attr('value'));
                }
            }
            if (result.length > 0) {
                $("#hdnFistSelectedPackages").val(JSON.stringify(descriptionArray));
            }
        }

        function getSelectedOption() {
            var result = $("#optionals input[name='txtNonInstalledOption']");
            var descriptionArray = new Array();
            for (var i = 0; i < result.length; i++) {
                if ($(result[i]).attr('value') !== undefined && $(result[i]).attr('checked') !== undefined && $(result[i]).attr('checked')) {
                    descriptionArray.push($(result[i]).attr('value'));
                }
            }
            if (result.length > 0) {
                $("#hdnFistSelectedOptions").val(JSON.stringify(descriptionArray));
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #c2
        {
            width: 785px;
            border-right: none;
            overflow: hidden;
            height: 100%;
        }

        h4
        {
            margin-bottom: 0;
            margin-top: 0;
        }

        select
        {
            width: 135px;
        }

        p
        {
            padding: 1em;
            border-bottom: 1px solid #101010;
            border-top: 1px #777 solid;
            margin: 0;
        }

            p.top
            {
                border-top: none;
                padding: 0;
                margin-top: .5em;
            }

            p.bot
            {
                border-bottom: none;
                padding: 0;
            }

        p
        {
            margin-top: 0;
        }

        body
        {
            background: url('../images/cBgRepeatW.png') top center repeat-y;
        }

        #noteList
        {
            padding: 1em;
        }

        #submit
        {
            float: right;
        }

        #optionals
        {
            width: 410px;
            margin-top: 0;
            margin-bottom: 0;
            overflow: hidden;
        }

        #warranty-info, #pricing
        {
            width: 206px;
            padding-left: 0;
        }

            #warranty-info .column, #pricing .column
            {
                padding-left: 0;
            }

        #c2
        {
            padding-top: 0;
        }

        .hider
        {
            display: none;
        }

        input[name="options"]
        {
            margin-bottom: 0;
            margin-left: 0;
        }

        textarea[name="description"]
        {
            width: 420px;
        }

        #optionals ul
        {
            margin-top: .3em;
        }

        .scroll-pane
        {
            height: 100%;
            overflow: auto;
            overflow-x: hidden;
        }

        #notes
        {
            height: 200px;
            margin-bottom: 6em;
        }

        input[name="completeApp"]
        {
            float: right;
        }

        div.scrollable-list
        {
            max-height: 150px;
            overflow: hidden;
            overflow-y: scroll;
            background: #111111;
        }

        input.small
        {
            width: 75px !important;
        }

        #column-two
        {
            height: 510px;
        }

        #c2 h3, #c2 h5
        {
            margin: 0;
            padding: 0;
        }

        #c2 h5
        {
            margin-bottom: 20px;
        }

        #pricing td.label
        {
            display: block;
            width: 140px !important;
        }

        input#edit-images
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 96%;
        }

        input#btndescription
        {
            cursor: pointer;
            background-color: #000000;
            padding-left: 0;
            padding-right: 0;
            width: 100%;
        }

        #description-box
        {
            width: 48%;
            padding: 0;
            padding-top: 15px;
        }

        #other
        {
            background: #333333;
            overflow: hidden;
            width: 726px !important;
            position: relative;
            padding: 0;
            margin-left: 10px;
            padding-bottom: 5px;
        }

        #images
        {
            width: 48%;
            margin-left: 2%;
            padding: 0;
            padding-top: 15px;
        }

        #img-title
        {
            margin-left: 15px;
        }

        #images img
        {
            margin-left: 1%;
            margin-top: 3px;
            padding: 0;
            display: inline-block;
            width: 5%;
        }

        #description-input
        {
            width: 98%;
            height: 100px;
            resize: none;
            margin-left: 1px;
        }

        #fancybox-content
        {
            background: #111111;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
    
</asp:Content>