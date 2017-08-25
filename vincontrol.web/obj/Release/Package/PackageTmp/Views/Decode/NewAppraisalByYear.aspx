<%@ Page Title="New Appraisal" MasterPageFile="~/Views/Shared/NewSite.Master" Language="C#"
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
    <script src="<%=Url.Content("~/js/jquery-1.7.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.validationEngine-en.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.validationEngine.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.numeric.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/resize.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/js/jquery.alerts.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        function checkMileage(field, rules, i, options) {
            if (parseInt(field.val().replace(/,/g, "")) > 2000000) {
                return "Mileage should <= 2,000,000";
            }
        }

        function checkVin(field, rules, i, options) {
            if (field.val() != "") {
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
                                    field.val('');
                                }
                            }
                        }
                    });
                } else {
                    return "Invalid Vin number";
                }
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
            $("#ModelYear").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                    // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                    // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });
            $("#ModelYear").blur(function () {
                var currentYear = (new Date).getFullYear();
                if (parseInt($("#ModelYear").val()) > parseInt(currentYear) + 1 || parseInt($("#ModelYear").val()) < 1988) {
                    ShowWarningMessage('The Year should be in 1988 to ' + parseInt(parseInt(currentYear) + parseInt(1)));
                    $("#ModelYear").val($('#hdCurrentYear').val());
                }
                else {
                    $('#hdCurrentYear').val($("#ModelYear").val());

                }
            });

            var Make = $("#SelectedMake");
            var Model = $("#SelectedModel");
            var Year = $("#ModelYear");
            var Trim = $("#SelectedTrim");

            Year.live("change", function () {
                blockUI();
                $('#elementID').removeClass('hideLoader');

                Reset();

                $.post('<%= Url.Content("~/Ajax/YearAjaxPost") %>', { ModelYear: Year.val() }, function (data) {
                    var items = "<option value='0'>" + "Select a make" + "</option>";
                    $.each(data, function (i, data) {
                        items += "<option value='" + data.id + "|" + data.Value + "'>" + data.Value + "</option>";
                    });

                    Make.html(items);
                    $('#elementID').addClass('hideLoader');
                    unblockUI();
                });


                $('#elementID').addClass('hideLoader');
                unblockUI();
            });

            Make.live("change", function () {
                blockUI();
                $('#elementID').removeClass('hideLoader');

                var index = Make.val().indexOf("|");
                var id = Make.val().substring(0, index);
                if (id > 0) {
                    $.post('<%= Url.Content("~/Ajax/MakeAjaxPost") %>', { ModelYear: Year.val(), MakeId: id }, function (data) {
                        var items = "<option value='0'>" + "Select a model" + "</option>";
                        $.each(data, function (i, data) {

                            items += "<option value='" + data.id + "|" + data.Value + "'>" + data.Value + "</option>";
                        });
                        $("#SelectedModel").html(items);
                        $("#SelectedTrim").html("<select id=\"SelectedTrim\" name=\"SelectedTrim\"></select>");
                        $('#elementID').addClass('hideLoader');
                        unblockUI();
                    });
                }
                $('#elementID').addClass('hideLoader');
                unblockUI();
            });

            Model.live("change", function () {
                blockUI();
                $('#elementID').removeClass('hideLoader');
                var index = Model.val().indexOf("|");
                var id = Model.val().substring(0, index);

                $.post('<%= Url.Content("~/Ajax/ModelAjaxPostWithYearMakeModel") %>', { year: $("#ModelYear").val(), make: $("#SelectedMake option:selected").text(), model: $("#SelectedModel option:selected").text() }, function (data) {


                    var trimitems = "";
                    var exitems = "";
                    var initems = "";
                    var MSRP = "";
                    var cylinders = "";
                    var fuel = "";
                    var litters = "";
                   
                    var wheeldrive = "";
                    var bodytype = "";
                    var stockPhoto = "";
                    var vehicleoptions = new Array();

                    var vehiclepackages = new Array();
                    var flag = true;
                    var exteriorflag = true;
                    var doors = "";
                    var trimId = 0;
                    $.each(data, function (i, data) {
                        if (data.toString().indexOf("TrimStyleId") != -1) {
                            var trimName = data.toString().substring(0, data.lastIndexOf("Trim"));
                            var styleId = data.toString().substring(data.lastIndexOf("*") + 1);
                            if (trimId == 0) {
                                trimId = styleId;
                            }
                            if (flag) {
                                trimitems += "<option selected='selected' value='" + styleId + "|" + trimName + "'>" + trimName + "</option>";
                                flag = false;
                            } else
                                trimitems += "<option value='" + styleId + "|" + trimName + "'>" + trimName + "</option>";
                        } else if (data.toString().indexOf("BodyType") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            bodytype += "<option value='" + text + "'>" + text + "</option>";
                        } else if (data.toString().indexOf("Optional") != -1) {
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

                        } else if (data.toString().indexOf("Fuel") != -1) {
                            text = data.toString().substring(0, data.toString().length - 4);
                            fuel += "<option value='" + text + "'>" + text + "</option>";
                        } else if (data.toString().indexOf("Litter") != -1) {
                            text = data.toString().substring(0, data.toString().length - 6);
                            litters += "<option value='" + text + "'>" + text + "</option>";
                        } else if (data.toString().indexOf("Cylinder") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            cylinders += "<option value='" + text + "'>" + text + "</option>";
                        } else if (data.toString().indexOf("MSRP") != -1) {
                            MSRP = data.toString().substring(0, data.toString().length - 4);
                            MSRP = Number(MSRP);
                            MSRP = formatDollar(MSRP);
                        } else if (data.toString().indexOf("Exterior") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            var textList = text.split("|");
                            if (exteriorflag) {
                                $("#SelectedExteriorColorValue").val(textList[0]);
                                exteriorflag = false;
                            }
                            exitems += "<option value='" + textList[1] + "'>" + textList[0] + "</option>";
                        } else if (data.toString().indexOf("Interior") != -1) {
                            text = data.toString().substring(0, data.toString().length - 8);
                            initems += "<option value='" + text + "'>" + text + "</option>";
                        } else if (data.toString().indexOf("PassengerDoors") != -1) {
                            text = data.toString().substring(0, data.toString().length - 14);
                            doors += text;
                        } else if (data.toString().indexOf("WheelDrive") != -1) {
                            text = data.toString().substring(0, data.toString().length - 10);
                            wheeldrive += "<option value='" + text + "'>" + text + "</option>";
                        } else if (data.toString().indexOf("DefaultImage") != -1) {
                            text = data.toString().substring(0, data.toString().length - 12);
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
                    $("#SelectedDriveTrain").html(wheeldrive);
                    $("#SelectedBodyType").html(bodytype);
                    $("#MSRP").val(MSRP);
                    $("#ChromeModelId").val(id);
                    $("#DefaultImageUrl").val(stockPhoto);
                    $("#Door").val(doors);

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
                    unblockUI();
                    $("#SelectedTrim").change();

                });
                unblockUI();
            });

            Trim.change(function () {
                $('#hdSelectedExteriorColor').val($("#SelectedExteriorColor").val());
                $('#hdSelectedInteriorColor').val($("#SelectedInteriorColor").val());

                $('#hdCusExteriorColor').val($("#CusExteriorColor").val());
                $('#hdCusInteriorColor').val($("#CusInteriorColor").val());
                try {
                    if ($("#SelectedTrim").val().indexOf('Base/Other Trims') != -1) {
                        $("#CusTrim").removeAttr('disabled');
                    } else {
                        //$("#CusTrim").val('');
                        $("#CusTrim").attr('disabled', true);
                        $('#CusTrim').validationEngine('hide');
                    }


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
                        var exteriorFlag = true;
                        var doors = "";

                        $.each(data, function (i, data) {
                            var text;
                            if (data.toString().indexOf("BodyType") != -1) {
                                text = data.toString().substring(0, data.toString().length - 8);
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

                        trans += "<option value=''>Select...</option>";
                        trans += "<option value='Automatic'>Automatic</option>";
                        trans += "<option value='Manual'>Manual</option>";
                        trans += "<option value='Shiftable Automatic Transmission'>Shiftable Automatic Transmission</option>";

                        exitems += "<option value='Other Colors'>Other Colors</option>";

                        initems += "<option value='Other Colors'>Other Colors</option>";

                        $("#SelectedExteriorColor").html(exitems);
                        $("#SelectedInteriorColor").html(initems);

                        $('#SelectedExteriorColor').val($("#hdSelectedExteriorColor").val());
                        $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
                        $('#CusExteriorColor').val($("#hdCusExteriorColor").val());

                        $('#SelectedInteriorColor').val($("#hdSelectedInteriorColor").val());
                        $('#CusInteriorColor').val($("#hdCusInteriorColor").val());

                        $("#SelectedCylinder").html(cylinders);
                        $("#SelectedFuel").html(fuel);
                        $("#SelectedLiters").html(litters);
                        $("#SelectedDriveTrain").html(wheeldrive);
                        $("#SelectedBodyType").html(bodytype);
                        $("#MSRP").val(MSRP);

                        $("#Door").val(doors);
                        $("#ChromeStyleId").val(id);
                        $("#DefaultImageUrl").val(stockPhoto);
                        $('#pIdImage').attr("src", stockPhoto)
                        $('#title').html($('#ModelYear').val() + ' ' + $('#SelectedMake').val().split('|')[1] + ' ' + $('#SelectedModel').val().split('|')[1] + ' ' + $('#SelectedTrim').val().split('|')[1]);

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

                        if ($("#SelectedTrim").val().indexOf('Base/Other Trims') === -1) {
                            $("#CusTrim").attr('disabled', true);
                        }

                        if ($("#SelectedExteriorColor").val().indexOf('Other Colors') === -1) {
                            $("#CusExteriorColor").attr('disabled', true);
                        }

                        if ($("#SelectedInteriorColor").val().indexOf('Other Colors') === -1) {
                            $("#CusInteriorColor").attr('disabled', true);
                        }

                    });
                } catch (e) {

                }
            });

            var firstType = '<%= Model.IsTruck ? "Truck" : "Car" %>';
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

        $("#SelectedExteriorColor").live('change', function (e) {
            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
            $('#CusExteriorColor').validationEngine('hide');

            if ($("#SelectedExteriorColor").val().indexOf('Other Colors') != -1) {
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
                //$("#CusInteriorColor").val('');
                $("#CusInteriorColor").attr('disabled', true);
            }
            $('#hdSelectedInteriorColor').val($("#SelectedInteriorColor").val());
        });

        if ($("#SelectedExteriorColorValue").val() == '' && $("#SelectedExteriorColor :selected").text()) {
            $("#SelectedExteriorColorValue").val($("#SelectedExteriorColor :selected").text());
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

        
        function checkOtherExteriorColor(field, rules, i, options) {
            try {
                if ($("#SelectedExteriorColor").val() == "Other Colors" && field.val().length == 0) {
                    rules.push('required');
                }
            } catch (e) {

            }
        }

        function checkOtherInteriorColor(field, rules, i, options) {
            try {
                if ($("#SelectedInteriorColor").val() == "Other Colors" && field.val().length == 0) {
                    rules.push('required');
                }
            } catch (e) {

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

            //if ($('#SelectedVehicleType').val() == 'Truck' && ($('#SelectedTruckType').val() == '' || $('#SelectedTruckCategoryId').val() == '' || $('#SelectedTruckClassId').val() == '')) {
            //    ShowWarningMessage('Please choose Truck Type, Truck Category and Truck Class by clicking on Truck image button');
            //    return false;
            //}
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
<asp:Content ID="Content1" ContentPlaceHolderID="SubMenu" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.BeginForm("ViewProfileByYear", "Appraisal", FormMethod.Post, new { id = "viewProfileForm", onsubmit = "return validateForm()" }); %>
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
                    <img width="160px" height="130px" src="/Content/images/noImageAvailable.jpg" name="pIdImage" id="pIdImage">
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
                        <% if (Model.VinDecodeSuccess)
                           { %>
                        <%--<%= Html.TextBoxFor(x => x.VinNumber, new { @class = "column_one_input readonly", @readonly = "readonly" })%>--%>
                        <input type="text" name="VinNumber" id="VinNumber" value="<%= Model.VinNumber %>"
                            class="column_one_input readonly" readonly="readonly" data-validation-engine="validate[funcCall[checkVin]" data-errormessage-value-missing="test" />
                        <% }
                           else
                           { %>
                        <%--<%= Html.TextBoxFor(x => x.VinNumber, new { @class = "column_one_input" })%>--%>
                        <input type="text" name="VinNumber" id="VinNumber" value="<%= Model.VinNumber %>"
                            class="column_one_input" data-validation-engine="validate[funcCall[checkVin]" data-errormessage-value-missing="test" />
                        <% } %>
                    </div>
                    <div class="column_one_items">
                        <label>
                            Year</label>
                        <% if (Model.VinDecodeSuccess)
                           { %>
                        <%= Html.TextBoxFor(x => x.ModelYear, new {@class = "column_one_input readonly", @readonly = "readonly", maxlength = "4"}) %>
                        <% }
                           else
                           { %>
                        <%= Html.TextBoxFor(x => x.ModelYear, new {@class = "column_one_input", maxlength = "4"}) %>
                        <% } %>
                    </div>
                    <div class="column_one_items">
                        <label>
                            Make</label>
                        <%--<%=Html.DropDownListFor(x => x.SelectedMake, Model.MakeList, new { @class = "validate[required] text-input" })%>--%>
                        <%= Html.CusDropDown("SelectedMake", string.Empty, Model.SelectedMake, Model.MakeList, "validate[required,funcCall[checkSelectedMake]]", "Make is required!") %>
                    </div>
                    <div class="column_one_items">
                        <label>
                            Model</label>
                        <%--<%=Html.DropDownListFor(x => x.SelectedModel, Model.ModelList, new { @class = "validate[required] text-input" })%>--%>
                        <%= Html.CusDropDown("SelectedModel", string.Empty, Model.SelectedModel, Model.ModelList, "validate[required,funcCall[checkSelectedModel]]", "Model is required!") %>
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
                    <div class="column_two_items" style="height: 50px;">
                        <div>
                            <label>
                                Trim</label>
                            <%= Html.DropDownListFor(x => x.SelectedTrim, Model.TrimList, new {@class = "column_one_input"}) %>
                        </div>
                        <div class="others">
                            Other:
                            <%= Html.TextBoxFor(x => x.CusTrim, new {@class = "exterior_others", MaxLength = 50 }) %>
                            <%--<input type="text" name="CusTrim" id="CusTrim" value="<%= Model.CusTrim %>"
                                class="exterior_others" data-validation-engine="validate[funcCall[checkOtherCusTrim]"
                                data-errormessage-value-missing="Other Trim is required!" />--%>
                        </div>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Cylinders</label>
                        <%= Html.DropDownListFor(x => x.SelectedCylinder, Model.CylinderList) %>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Liters</label>
                        <%= Html.DropDownListFor(x => x.SelectedLiters, Model.LitersList) %>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Doors</label>
                        <%= Html.TextBoxFor(x => x.Door, new {@class = "column_one_input readonly", @readonly = "readonly"}) %>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Style</label>
                        <%= Html.DropDownListFor(x => x.SelectedBodyType, Model.BodyTypeList) %>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Fuel</label>
                        <%= Html.DropDownListFor(x => x.SelectedFuel, Model.FuelList) %>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Drive</label>
                        <%= Html.DropDownListFor(x => x.SelectedDriveTrain, Model.DriveTrainList) %>
                    </div>
                    <div class="column_two_items">
                        <label>
                            Original MSRP</label>
                        <%= Html.TextBoxFor(x => x.MSRP, new {@class = "readonly", @readonly = "readonly"}) %>
                    </div>
                    <div class="column_two_items" style="height: 50px;">
                        <div>
                            <label>
                                Exterior Color</label>
                            <%= Html.DropDownListFor(x => x.SelectedExteriorColorCode, Model.ExteriorColorList, new {id = "SelectedExteriorColor", @style = "width: 59%;"}) %>
                            <input type="hidden" name="SelectedExteriorColorValue" id="SelectedExteriorColorValue"
                                value="<%= Model.SelectedExteriorColorValue %>" />
                        </div>
                        <div class="others">
                            Other:
                            <%--<%=Html.TextBoxFor(x => x.CusExteriorColor, new { @class = "exterior_others" })%>--%>
                            <input type="text" name="CusExteriorColor" id="CusExteriorColor" maxlength="50" value="<%= Model.CusExteriorColor %>"
                                class="exterior_others" data-validation-engine="validate[funcCall[checkOtherExteriorColor]"
                                data-errormessage-value-missing="Other exterior color is required!" />
                        </div>
                    </div>
                    <div class="column_two_items" style="height: 50px;">
                        <div>
                            <label>
                                Interior Color</label>
                            <%= Html.DropDownListFor(x => x.SelectedInteriorColor, Model.InteriorColorList, new {@style = "width: 59%;"}) %>
                        </div>
                        <div class="others">
                            Other:
                            <%--<%=Html.TextBoxFor(x => x.CusInteriorColor, new { @class = "interior_others" })%>--%>
                            <input type="text" name="CusInteriorColor" id="CusInteriorColor" maxlength="50" value="<%= Model.CusInteriorColor %>"
                                class="exterior_others" data-validation-engine="validate[funcCall[checkOtherInteriorColor]"
                                data-errormessage-value-missing="Other interior color is required!" />
                        </div>
                    </div>
                    <div class="column_two_items">
                        <label style="width: 34%;">
                            Odometer (*)</label>
                        <%--<input id="Mileage" name="Mileage" type="text" value="<%=Model.Mileage %>" class="column_one_input validate[required] text-input" data-validation-placeholder="0" />--%>
                        <%--<%=Html.TextBoxFor(x => x.Mileage, new { @class = "column_one_input" })%>--%>
                        <input type="text" name="Mileage" id="Mileage" value="<%= Model.Mileage %>" data-validation-placeholder="0"
                            class="column_one_input" data-validation-engine="validate[required,funcCall[checkMileage]]" data-errormessage-value-missing="Odometer is required!" autocomplete="off" />
                    </div>
                    <div class="column_two_items">
                        <label>
                            Tranmission (*)</label>
                        <%--<%=Html.DropDownListFor(x => x.SelectedTranmission, Model.TranmissionList, new { @style = "width: 59%;", @class = "validate[required] text-input" })%>--%>
                        <%= Html.CusDropDown("SelectedTranmission", "DDLStyle", string.Empty, Model.TranmissionList, "validate[required]", "Transmission is required!") %>
                        <%--<select name="SelectedTranmission" id="SelectedTranmission" style="width: 59%;" data-validation-engine="validate[required]"
                            data-errormessage-value-missing="Transmission is required!">
                            <% foreach (var item in Model.TranmissionList)
                               {%>
                            <option value="<%= item.Value %>">
                                <%= item.Text %></option>
                            <%}%>
                        </select>--%>
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
    
</asp:Content>
