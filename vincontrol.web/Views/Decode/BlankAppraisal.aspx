<%@ Page Title="New Appraisal" Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.AppraisalViewFormModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    New Appraisal
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="column">
        <% Html.BeginForm("ViewProfileByYearBlank", "Appraisal", FormMethod.Post, new { id = "viewProfileForm", onsubmit = "return validateForm()" }); %>
        <table>
            <tr>
                <td>VIN</td>
                <td><%=Html.TextBoxFor(x=>x.VinNumber) %></td>
            </tr>
            <tr>
                <td>Stock</td>
                <td><%=Html.TextBoxFor(x => x.StockNumber)%> </td>
            </tr>
            <tr>
                <td>Date</td>
                <td><%=Html.TextBoxFor(x=>x.AppraisalDate) %> </td>
            </tr>
            <tr>
                <td>Year</td>
                <td><%=Html.DropDownListFor(x=>x.SelectedModelYear,Model.ModelYearList) %></td>
            </tr>
            <tr>
                <td>Make</td>
                <td><%=Html.DropDownListFor(x=>x.SelectedMake,Model.MakeList) %></td>

            </tr>
            <tr>
                <td>Model</td>
                <td><%=Html.DropDownListFor(x=>x.SelectedModel,Model.ModelList) %></td>

            </tr>
            <tr>
                <td>Trim</td>
                <td><%=Html.DropDownListFor(x=>x.SelectedTrim,Model.TrimList) %></td>

            </tr>
            <tr>
                <td>Exterior Color</td>
                <td>
                    <%=Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ExteriorColorList)%>
                    <input type="hidden" name="SelectedExteriorColorValue" id="SelectedExteriorColorValue" value="<%=Model.SelectedExteriorColorValue %>" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%=Html.TextBoxFor(x=>x.CusExteriorColor, new { @style="width: 50px !important;"})%>
                </td>
            </tr>
            <tr>
                <td>Interior Color</td>
                <td>
                    <%=Html.DropDownListFor(x=>x.SelectedInteriorColor,Model.InteriorColorList) %>
                              
                </td>

            </tr>
            <tr>
                <td></td>
                <td>
                    <em style="font-size: .7em;">Other: </em>
                    <%=Html.TextBoxFor(x => x.CusInteriorColor, new { @style = "width: 50px !important;" })%>
                </td>
            </tr>
            <tr>
                <td>Odometer</td>
                <td><%=Html.TextBoxFor(x=>x.Mileage)%></td>

            </tr>
            <tr>
                <td>Cylinders</td>
                <td><%=Html.DropDownListFor(x=>x.SelectedCylinder,Model.CylinderList )%></td>


            </tr>
            <tr>
                <td>Liters</td>
                <td><%=Html.DropDownListFor(x=>x.SelectedLiters,Model.LitersList )%></td>

            </tr>
            <tr>
                <td>Transmission</td>
                <td><%=Html.DropDownListFor(x => x.SelectedTranmission, Model.TranmissionList)%></td>

            </tr>
            <tr>
                <td>Doors</td>
                <td><%=Html.TextBoxFor(x=>x.Door)%></td>
            </tr>
            <tr>
                <td>Style</td>
                <td><%=Html.DropDownListFor(x => x.SelectedBodyType, Model.BodyTypeList)%></td>


            </tr>
            <tr>
                <td>Fuel</td>
                <td><%=Html.DropDownListFor(x => x.SelectedFuel, Model.FuelList)%></td>


            </tr>
            <tr>
                <td>Drive</td>
                <td><%=Html.DropDownListFor(x => x.SelectedDriveTrain, Model.DriveTrainList)%></td>



            </tr>
            <tr>
                <td>Original MSRP</td>
                <td><%=Html.TextBoxFor(x=>x.MSRP)%></td>

            </tr>

            <tr>
                <td style="font-size: .8em;" colspan="2">You <em>must</em> select an appraisal type:</td>
            </tr>
            <tr>
                <td style="font-size: .8em;" colspan="2">
                    <%=Html.RadioButtonFor(x=>x.AppraisalType,"Customer") %> Customer
                                  <br />
                    <%=Html.RadioButtonFor(x=>x.AppraisalType,"Auction") %> Auction
                </td>
            </tr>
        </table>
    </div>

    <div class="column">
        <input id="submit" class="pad" type="submit" name="submit" value="Complete Appraisal >" id="submitBTN" /><br />
        <br />
        Packages:<br />


        <%= Html.CheckBoxGroupPackageByYear("txtFactoryPackageOption")%>


        <div class="clear">Options: </div>
        <div id="optionals" class="" style="font-size: .9em;">
            <div class="clear"></div>

            <%= Html.CheckBoxGroupOptionByYear("txtNonInstalledOption")%>


            <div class="clear"></div>
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
        <p class="top"></p>
        <%--<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam tristique viverra turpis, in viverra arcu eleifend quis. </p>--%>

        <p class="bot"></p>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ClientScripts" runat="server">
    <script type="text/javascript">


        $(function () {
            var Make = $("#SelectedMake");
            var Model = $("#SelectedModel");
            var Year = $("#SelectedModelYear");
            var Trim = $("#SelectedTrim");


            Year.change(function () {
                $('#elementID').removeClass('hideLoader');

                $.post('<%= Url.Content("~/Ajax/YearAjaxPost") %>', { ModelYear: Year.val() }, function (data) {

                    var items = "<option value='" + 0 + "****" + "Select a make" + "'>" + "Select a make" + "</option>";
                    $.each(data, function (i, data) {
                        items += "<option value='" + data.divisionId + "|" + data.divisionName + "'>" + data.divisionName + "</option>";
                    });
                    $("#SelectedMake").html(items);

                    $('#elementID').addClass('hideLoader');
                });
            });



            Make.change(function () {
                $('#elementID').removeClass('hideLoader');
                //console.log(Make.val());
                var index = Make.val().indexOf("|");
                var id = Make.val().substring(0, index);

                $.post('<%= Url.Content("~/Ajax/MakeAjaxPost") %>', { ModelYear: Year.val(), MakeId: id }, function (data) {

                    if (data != "ManualMode") {
                        var items = "<option value='" + 0 + "|" + "Select a model" + "'>" + "Select a model" + "</option>";
                        $.each(data, function (i, data) {
                            items += "<option value='" + data.modelId + "|" + data.modelName + "'>" + data.modelName + "</option>";
                        });
                        if ($("#SelectedModel").attr("type") == "text") {


                            var actionUrl = '<%= Url.Action("RestoredBlankAppraisal", "Decode" ) %>';

                          window.location = actionUrl;


                      }
                      else {

                          $("#SelectedModel").html(items);
                      }
                      //console.log($("#SelectedModel").val());

                  } else {

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


                      $("#SelectedModel").replaceWith("<input id='SelectedModel' name='SelectedModel' type='text' value=''>");
                      $("#SelectedTrim").replaceWith("<input id='SelectedTrim' name='SelectedTrim' type='text' value=''>");
                      $("#SelectedExteriorColor").replaceWith("<input id='SelectedExteriorColor' name='SelectedExteriorColor' type='text' value=''>");
                      $("#SelectedInteriorColor").replaceWith("<input id='SelectedInteriorColor' name='SelectedInteriorColor' type='text' value=''>");
                      $("#SelectedCylinder").replaceWith("<input id='SelectedCylinder' name='SelectedCylinder' type='text' value=''>");
                      $("#SelectedLiters").replaceWith("<input id='SelectedLiters' name='SelectedLiters' type='text' value=''>");
                      $("#SelectedTranmission").replaceWith("<input id='SelectedTranmission' name='SelectedTranmission' type='text' value=''>");
                      $("#SelectedBodyType").replaceWith("<input id='SelectedBodyType' name='SelectedBodyType' type='text' value=''>");
                      $("#SelectedFuel").replaceWith("<input id='SelectedFuel' name='SelectedFuel' type='text' value=''>");
                      $("#SelectedBodyType").replaceWith("<input id='SelectedBodyType' name='SelectedBodyType' type='text' value=''>");
                      $("#SelectedDriveTrain").replaceWith("<input id='SelectedDriveTrain' name='SelectedDriveTrain' type='text' value=''>");
                      $("#Door").val("");


                  }

                });
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

                    var vehiclepackages = new Array();
                    var flag = true;
                    var doors = "";
                    $.each(data, function (i, data) {
                        if (data.toString().indexOf("TrimStyleId") != -1) {
                            var trimName = data.toString().substring(0, data.indexOf("Trim"));
                            var styleId = data.toString().substring(data.lastIndexOf("*") + 1);

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

                    exitems += "<option value='Other Colors'>Other Colors</option>";

                    initems += "<option value='Other Colors'>Other Colors</option>";

                    $("#SelectedTrim").html(trimitems);

                    $("#SelectedExteriorColor").html(exitems);
                    $("#SelectedInteriorColor").html(initems);
                    $("#SelectedCylinder").html(cylinders);
                    $("#SelectedFuel").html(fuel);
                    $("#SelectedLiters").html(litters);
                    $("#SelectedTranmission").html(trans);
                    $("#SelectedDriveTrain").html(wheeldrive);
                    $("#SelectedBodyType").html(bodytype);
                    $("#MSRP").val(MSRP);
                    $("#ChromeModelId").val(id);

                    $("#Door").val(doors);

                    var indexPackages = 0;

                    var indexOptions = 0;

                    var packageContent = "";
                    $("#Packages").html("");

                    packageContent += "<ul class='options'>";

                    for (var i = 0; i < vehiclepackages.length; i++) {
                        packageContent += '<li>';
                        packageContent += "<input class='z-index' name='txtFactoryPackageOption' onclick='javascript:changeMSRP(this);' type='checkbox' value='" + vehiclepackages[i].name + "' price='" + vehiclepackages[i].price + "'>";
                        packageContent += "<label for='" + vehiclepackages[i].name + "' class='z-index' price='" + vehiclepackages[i].price + "'>" + vehiclepackages[i].name + "</label>";
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
                        optionContent += "<input type='checkbox' value='" + vehicleoptions[i].name + "' onclick='javascript:changeMSRP(this)' name='txtNonInstalledOption' class='z-index'>";
                        optionContent += "<label for='" + vehicleoptions[i].name + "'>" + vehicleoptions[i].name + "</label>";
                        optionContent += '<br>';
                        optionContent += '</li>';
                    }

                    optionContent += "</ul>";


                    $("#Options").html(optionContent);





                    $('#elementID').addClass('hideLoader');

                });
            });


            Trim.change(function () {

                var index = Trim.val().indexOf("|");
                var id = Trim.val().substring(0, index);
                $('#elementID').removeClass('hideLoader');
                $.post('<%= Url.Content("~/Ajax/StyleAjaxPost") %>', { styleId: id, listingId: 0 }, function (data) {

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
                    $.each(data, function (i, data) {
                        if (data.toString() == "Automatic") {
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
                            option = new Object();
                            option.name = result[0];
                            option.price = result[1];
                            option.description = result[2];
                            option.code = result[3];
                            option.checked = result[4];
                            option.pureprice = result[1].substring(1, result[1].indexOf('.')).replace(',', '');
                            vehicleoptions[vehicleoptions.length] = option;
                        }
                        else if (data.toString().indexOf("Package") != -1) {
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

        function removeCommas(str) {
            return str.replace(/,/g, "");
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
        function validateForm() {

            var x = document.forms["viewProfileForm"];

            if (x.txtGroupAppraisalType[0].checked == false && x.txtGroupAppraisalType[1].checked == false) {
                alert("Please select an appraisal type");
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ClientStyles" runat="server">
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Url.Content("~/Content/ui.datepicker.css")%>" rel="stylesheet" type="text/css" />
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
            width: 410px;
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
            width: 400px;
            overflow: hidden;
            overflow-y: scroll;
        }

        #Options
        {
            max-height: 300px;
            width: 400px;
            overflow: hidden;
            overflow-y: scroll;
        }

        .hideLoader
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ClientTemplates" runat="server">
</asp:Content>
