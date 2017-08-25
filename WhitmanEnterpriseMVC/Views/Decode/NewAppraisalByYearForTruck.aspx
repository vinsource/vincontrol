<%@ Page Title="New Appraisal" MasterPageFile="~/Views/Shared/Site.Master" Language="C#"
    Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>New Appraisal</title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/ui.datepicker.css")%>" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />

    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>

    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>

    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <style type="text/css">
        #noteBox
        {
            width: 735px;
            margin-bottom: 1em;
        }
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
            width: 450px;
            margin-top: 0;
            margin-bottom: 0;
            overflow: hidden;
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
        #Packages
        {
            max-height: 175px;
            width: 450px;
            overflow: hidden;
            overflow-y: scroll;
            font-size: .9em;
            background-color:#111111;
        }
        #Options
        {
            max-height: 300px;
            width: 450px;
            overflow: hidden;
            overflow-y: scroll;
            font-size: .9em;
            background-color:#111111;
        }
        .hideLoader
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="topNav">
        <input class="btn" type="button" name="inventory" value=" Car " id="btnCar" />
        <input class="btn onadmin" type="button" name="notifications" value=" Commercial Truck "
            id="btnTruck" />
    </div>
    <div class="column">
        <% Html.BeginForm("ViewProfileByYearForTruck", "Appraisal", FormMethod.Post, new { id = "viewProfileForm", onsubmit = "return validateForm()" }); %>
        <table>
            <tr>
                <td>
                    VIN
                </td>
                <td>
                    <%=Html.TextBoxFor(x => x.VinNumber)%>
                </td>
            </tr>
            <tr>
                <td>
                    Stock
                </td>
                <td>
                    <%=Html.TextBoxFor(x => x.StockNumber)%>
                </td>
            </tr>
            <tr>
                <td>
                    Date
                </td>
                <td>
                    <%=Html.TextBoxFor(x => x.AppraisalDate, new { @readonly = "readonly" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Year
                </td>
                <td>
                    <%=Html.TextBoxFor(x => x.ModelYear, new { @readonly = "readonly" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Make
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedMake,Model.MakeList) %>
                </td>
            </tr>
            <tr>
                <td>
                    Model
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedModel,Model.ModelList) %>
                </td>
            </tr>
            <tr>
                <td>
                    Trim
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedTrim,Model.TrimList) %>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%= Html.TextBoxFor(x => x.CusTrim, new { @style = "width: 70px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Truck Type
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedTruckType, Model.TruckTypeList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Truck Class
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedTruckClass, Model.TruckClassList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Truck Category
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedTruckCategory, Model.TruckCategoryList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Exterior Color
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ExteriorColorList, new { id = "SelectedExteriorColor" })%>
                    <input type="hidden" name="SelectedExteriorColorValue" id="SelectedExteriorColorValue"
                        value="<%=Model.SelectedExteriorColorValue %>" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%=Html.TextBoxFor(x=>x.CusExteriorColor, new { @style="width: 50px !important;"})%>
                </td>
            </tr>
            <tr>
                <td>
                    Interior Color
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.InteriorColorList) %>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%=Html.TextBoxFor(x => x.CusInteriorColor, new { @style = "width: 50px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>
                    Odometer
                </td>
                <td>
                    <%=Html.TextBoxFor(x=>x.Mileage)%>
                </td>
            </tr>
            <tr>
                <td>
                    Cylinders
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedCylinder,Model.CylinderList )%>
                </td>
            </tr>
            <tr>
                <td>
                    Liters
                </td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedLiters,Model.LitersList )%>
                </td>
            </tr>
            <tr>
                <td>
                    Transmission
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedTranmission, Model.TranmissionList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Doors
                </td>
                <td>
                    <%=Html.TextBoxFor(x=>x.Door)%>
                </td>
            </tr>
            <tr>
                <td>
                    Style
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedBodyType, Model.BodyTypeList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Fuel
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedFuel, Model.FuelList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Drive
                </td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedDriveTrain, Model.DriveTrainList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Original MSRP
                </td>
                <td>
                    <%=Html.TextBoxFor(x=>x.MSRP, new { @readonly = "readonly" })%>
                </td>
            </tr>
            <tr>
                <td style="font-size: .8em;" colspan="2">
                    You <em>must</em> select an appraisal type:
                </td>
            </tr>
            <tr>
                <td style="font-size: .8em;" colspan="2">
                    <%=Html.RadioButtonFor(x=>x.AppraisalType,"Customer") %>
                    Customer
                    <br />
                    <%=Html.RadioButtonFor(x=>x.AppraisalType,"Auction") %>
                    Auction
                </td>
            </tr>
        </table>
    </div>
    <div class="column">
        <input id="submit" class="pad" type="submit" name="submit" value="Complete Appraisal >"
            id="submitBTN" /><br />
        <br />
        Packages:<br />
        <%= Html.CheckBoxGroupPackageByYear("txtFactoryPackageOption")%>
          <%= Html.HiddenFor(x=>x.SelectedPackagesDescription) %>
        <div class="clear">
            Options:
        </div>
        <div id="optionals" class="" style="font-size: .9em;">
            <div class="clear">
            </div>
            <%= Html.CheckBoxGroupOptionByYear("txtNonInstalledOption")%>
            <div class="clear">
            </div>
        </div>
        <div class="clear">
            <script type="text/javascript">
                $('input[name="options"]').click(function () {
                    $("#optionals").slideToggle("slow");
                });
                $('input[name="options"]').click(function () {
                    $("#descriptionWrapper").slideToggle("slow");
                });
                </script>
        </div>
    </div>
    <div id="notes" class="clear">
        Notes:
        <br />
        <%=Html.TextAreaFor(x=>x.Notes,new {@cols=87, @rows=3})%>
        <%=Html.HiddenFor(x=>x.ChromeModelId) %>
        <%=Html.HiddenFor(x=>x.ChromeStyleId) %>
        <% Html.EndForm(); %>
        <p class="top">
        </p>
        <p class="bot">
        </p>
    </div>
    <script type="text/javascript">


        $(function () {
            $("#Mileage").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
            var Make = $("#SelectedMake");
            var Model = $("#SelectedModel");
            var Year = $("#ModelYear");
            var Trim = $("#SelectedTrim");


            Make.change(function () {
                $('#elementID').removeClass('hideLoader');
                //console.log(Make.val());
                var index = Make.val().indexOf("|");
                var id = Make.val().substring(0, index);
                if (id > 0) {

                    $.post('<%= Url.Content("~/Ajax/MakeAjaxPost") %>', { ModelYear: Year.val(), MakeId: id }, function (data) {

                        var items = "<option value='" + 0 + "|" + "Select a model" + "'>" + "Select a model" + "</option>";
                        $.each(data, function (i, data) {
                            items += "<option value='" + data.id + "|" + data.Value + "'>" + data.id + "</option>";
                        });
                        $("#SelectedModel").html(items);
                        $("#SelectedTrim").html("<select id=\"SelectedTrim\" name=\"SelectedTrim\"></select>");
                        $('#elementID').addClass('hideLoader');

                    });
                }
                $('#elementID').addClass('hideLoader');
            });

            Model.change(function () {

                $('#elementID').removeClass('hideLoader');
                var index = Model.val().indexOf("|");
                var id = Model.val().substring(0, index);
                $.post('<%= Url.Content("~/Ajax/ModelAjaxPost") %>', { ModelID: id }, function (data) {

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

                    var trimitems = "";
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
                    var exteriorflag = true;

                    var vehiclepackages = new Array();
                    var flag = true;
                    var doors = "";
                    var trimId = 0;
                    $.each(data, function (i, data) {
                        if (data.toString().indexOf("TrimStyleId") != -1) {
                            var trimName = data.toString().substring(0, data.indexOf("Trim"));
                            var styleId = data.toString().substring(data.lastIndexOf("*") + 1);
                            if (trimId == 0) {
                                trimId = styleId;
                            }
                            if (flag) {
                                trimitems += "<option selected='selected' value='" + styleId + "|" + trimName + "'>" + trimName + "</option>";
                                flag = false;
                            }
                            else
                                trimitems += "<option value='" + styleId + "|" + trimName + "'>" + trimName + "</option>";
                        }
                        else if (data.toString() == "Automatic") {
                            var text = data.toString();
                            trans += "<option value='Automatic'>" + text + "</option>";
                        }
                        else if (data.toString() == "Manual") {
                            var text = data.toString();
                            trans += "<option value='Manual'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("BodyType") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            bodytype += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Optional") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            var result = text.split("*");
                            text = result[0];
                            var optionPrice = text.substring(text.lastIndexOf(" ") + 1);

                            var option = new Object();
                            option.price = optionPrice;
                            option.name = text;
                            option.description = result[1];
                            vehicleoptions[vehicleoptions.length] = option;
                        }
                        else if (data.toString().indexOf("Package") != -1) {
                            text = data.toString().substring(0, data.toString().length - 7);
                            var result = text.split("*");
                            text = result[0];
                            var packagePrice = text.substring(text.lastIndexOf(" ") + 1);

                            var option = new Object();
                            option.price = packagePrice;
                            option.name = text;
                            option.description = result[1];
                            vehiclepackages[vehiclepackages.length] = option;
                        }
                        else if (data.toString().indexOf("Fuel") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 4);
                            fuel += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Litter") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 6);
                            litters += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Cylinder") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            cylinders += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("MSRP") != -1) {
                            MSRP = data.toString().substring(0, data.toString().length - 4);
                            MSRP = Number(MSRP);
                            MSRP = formatDollar(MSRP);
                        }
                        else if (data.toString().indexOf("Exterior") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            var textList = text.split("|");
                            if (exteriorflag) {
                                $("#SelectedExteriorColorValue").val(textList[0]);
                                exteriorflag = false;
                            }
                            exitems += "<option value='" + textList[1] + "'>" + textList[0] + "</option>";
                        }
                        else if (data.toString().indexOf("Interior") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            initems += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("PassengerDoors") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 14);
                            doors += text;
                        }
                        else if (data.toString().indexOf("WheelDrive") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 10);
                            wheeldrive += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else {
                            var text = data.toString();
                            stockPhoto = text;
                        }
                    });

                    trimitems += "<option value='" + trimId + "|Base/Other Trims'>Base/Other Trims</option>";

                    exitems += "<option value='Other Colors'>Other Colors</option>";

                    initems += "<option value='Other Colors'>Other Colors</option>";

                    $("#SelectedTrim").html(trimitems);

                    $("#SelectedExteriorColor").html(exitems);
                    $("#SelectedInteriorColor").html(initems);
                    $("#SelectedCylinder").html(cylinders);
                    $("#SelectedFuel").html(fuel);
                    $("#SelectedLiters").html(litters);
                    //$("#SelectedTranmission").html(trans);
                    $("#SelectedDriveTrain").html(wheeldrive);
                    $("#SelectedBodyType").html(bodytype);
                    $("#MSRP").val(MSRP);
                    $("#ChromeModelId").val(id);

                    $("#Door").val(doors);

                    var indexPackages = 0;

                    var indexOptions = 0;

                    console.log(vehiclepackages);
                    console.log(vehicleoptions);
                    $("#Packages li").children().each(
                  function () {
                      if (vehiclepackages.length > indexPackages) {

                          $(this).val(vehiclepackages[indexPackages].name);
                          $(this).attr("price", vehiclepackages[indexPackages].price);
                          $(this).attr("title", vehiclepackages[indexPackages].description);
                          $(this).attr("class", "z-index");
                          $(this).next("label:first").text(vehiclepackages[indexPackages].name);
                          $(this).next("label:first").attr("class", "z-index");
                          $(this).next("label:first").attr("title", vehiclepackages[indexPackages].description);

                          indexPackages = indexPackages + 1;
                      }
                  }
                );

                    $("#Options li").children().each(
                                  function () {
                                      if (vehicleoptions.length > indexOptions) {

                                          $(this).val(vehicleoptions[indexOptions].name);
                                          $(this).attr("price", vehicleoptions[indexOptions].price);
                                          $(this).attr("class", "z-index");
                                          $(this).next("label:first").text(vehicleoptions[indexOptions].name);
                                          $(this).next("label:first").attr("class", "z-index");

                                          indexOptions = indexOptions + 1;
                                      }
                                  }
                                );

                    $('#elementID').addClass('hideLoader');

                });
            });
            
            Trim.change(function () {

                var index = Trim.val().indexOf("|");
                var id = Trim.val().substring(0, index);
                $('#elementID').removeClass('hideLoader');
                $.post('<%= Url.Content("~/Ajax/StyleAjaxPost") %>', { styleId: id }, function (data) {

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
                    var doors = "";
                    var exteriorFlag = true;
                    var selectedTransmission = "";
                    $.each(data, function (i, data) {
                        if (data.toString() == "Automatic") {
                            selectedTransmission = "Automatic";
                        }
                        else if (data.toString() == "Manual") {
                            selectedTransmission = "Manual";
                        } else if (data.toString().indexOf("BodyType") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            bodytype += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Optional") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            var optionPrice = text.substring(text.lastIndexOf(" ") + 1);

                            var option = new Object();
                            option.price = optionPrice;
                            option.name = text;
                            vehicleoptions[vehicleoptions.length] = option;
                        }
                        else if (data.toString().indexOf("Package") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 7);
                            var packagePrice = text.substring(text.lastIndexOf(" ") + 1);

                            var option = new Object();
                            option.price = packagePrice;
                            option.name = text;
                            vehiclepackages[vehiclepackages.length] = option;
                        }
                        else if (data.toString().indexOf("Fuel") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 4);
                            fuel += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Litter") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 6);
                            litters += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("Cylinder") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            cylinders += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("MSRP") != -1) {
                            MSRP = data.toString().substring(0, data.toString().length - 4);
                            MSRP = Number(MSRP);
                            MSRP = formatDollar(MSRP);
                        }
                        else if (data.toString().indexOf("Exterior") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            var textList = text.split("|");
                            if (exteriorFlag) {
                                $("#SelectedExteriorColorValue").val(textList[0]);
                                exteriorFlag = false;
                            }
                            exitems += "<option value='" + textList[1] + "'>" + textList[0] + "</option>";
                        }
                        else if (data.toString().indexOf("Interior") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 8);
                            initems += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else if (data.toString().indexOf("PassengerDoors") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 14);
                            doors += text;
                        }
                        else if (data.toString().indexOf("WheelDrive") != -1) {
                            var text = data.toString().substring(0, data.toString().length - 10);
                            wheeldrive += "<option value='" + text + "'>" + text + "</option>";
                        }
                        else {
                            var text = data.toString();
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
                    $("#SelectedDriveTrain").html(wheeldrive);
                    $("#SelectedBodyType").html(bodytype);
                    $("#MSRP").val(MSRP);

                    $("#Door").val(doors);
                    $("#ChromeStyleId").val(id);
                    var indexPackages = 0;

                    var indexOptions = 0;
                    var packageContent = "";
                    $("#Packages").html("");

                    packageContent += "<ul class='options'>";

                    for (var i = 0; i < vehiclepackages.length; i++) {
                        packageContent += '<li>';
                        packageContent += "<input class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this);' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price + "' title='" + vehiclepackages[i].description + "'>";
                        packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'  title='" + vehiclepackages[i].description + "'>" + vehiclepackages[i].name + "</label>";

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
                        optionContent += "<input type='checkbox' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this)' name='txtNonInstalledOption' class='z-index' title='" + vehicleoptions[i].description + "'>";
                        optionContent += "<label for='" + vehicleoptions[i].name + "'  title='" + vehicleoptions[i].description + "'>" + vehicleoptions[i].name + "</label>";

                        optionContent += '<br>';
                        optionContent += '</li>';
                    }

                    optionContent += "</ul>";
                    $("#Options").html(optionContent);
                    $('#elementID').addClass('hideLoader');

                });
            });

        });

        $("#SelectedExteriorColor").change(function (e) {
            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
        });
        if ($("#SelectedExteriorColorValue").val() == '' && $("#SelectedExteriorColor :selected").text()) {
            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
        }

        function changeMSRP(checkbox) {

            if (checkbox.checked) {

                var optionPrice = checkbox.value;
                optionPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));
                var currency = $("#MSRP").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(optionPrice.replace(/[^0-9\.]+/g, ""));
                var total = Number(number1) + Number(number2);

            }
            else {

                var optionPrice = checkbox.value;
                optionPrice = optionPrice.substring(optionPrice.lastIndexOf("$"));
                var currency = $("#MSRP").val();
                var number1 = Number(currency.replace(/[^0-9\.]+/g, ""));
                var number2 = Number(optionPrice.replace(/[^0-9\.]+/g, ""));
                var total = Number(number1) - Number(number2);
            }
            
            $("#MSRP").val(formatDollar(total));
        }

        function validateForm() {

            var x = document.forms["viewProfileForm"];

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

            if ($("#SelectedExteriorColor").val() == "Other Colors" && $("#CusExteriorColor").val() == "") {
                //$("#ErrorCusEx").remove();
                $("#CusExteriorColor").parent().append("<strong id='CusErrorEx'><font color='Red'>Required</font></strong>");
                flag = false;
            }

            if ($("#SelectedInteriorColor").val() == "Other Colors" && $("#CusInteriorColor").val() == "") {
                $("#CusInteriorColor").parent().append("<strong id='CusErrorIn'><font color='Red'>Required</font></strong>");
                flag = false;
            }

            if ($("#SelectedTranmission").val() == "") {
                $("#SelectedTranmission").parent().append("<div><strong id='CusErrorTransmission'><font color='Red'>Required</font></strong></div>");
                flag = false;
            } 
            if ($("#Mileage").val() == "") {
                $("#Mileage").parent().append("<div><strong id='CusErrorMileage'><font color='Red'>Required</font></strong></div>");
                flag = false;
            }


//            if (x.AppraisalType[0].checked == false && x.AppraisalType[1].checked == false) {
//                $("#AppraisalType").parent().parent().parent().append("<strong id='CusErrorApp'><font color='Red'>Required</font></strong>");
//                flag = false;
//            }

            if (flag == false)
                return false;
            else
                $('#elementID').removeClass('hideLoader');

            var result = $("#Packages input[name='txtFactoryPackageOption']:checked");
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
