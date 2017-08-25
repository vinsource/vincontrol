function trimString(text) {
    return text.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}

function validateForm() {
    var flag = true;
    var errorCount = 0;

    //         if ($("#SelectedTrim").val() == null && trimString($("#Vin").val()) == "") {
    //             $("#Vin").val("");
    //             error("Please select a vehicle or vehicle identification number before continuing", "97px");
    //             return false;
    //         }

    //         else if ($("#SelectedTrim").val() == "0" && trimString($("#Vin").val()) == "") {
    //             $("#Vin").val("");
    //             error("Please select a vehicle or vehicle identification number before continuing", "97px");
    //             return false;
    //         }

    //         else {

    var errorString = "We're missing some information : ";
    
    if ($("#SelectedYear").val() == null || $("#SelectedYear").val() == undefined || $("#SelectedYear").val() == '0') {
        errorString += "<li>Year info is required</li>";
        flag = false;
        errorCount++;
    } else {

    }

    if ($("#SelectedMake").val() == null || $("#SelectedMake").val() == undefined || $("#SelectedMake").val() == '') {
        errorString += "<li>Make info is required</li>";
        flag = false;
        errorCount++;
    } else {

    }

    if ($("#SelectedModel").val() == null || $("#SelectedModel").val() == undefined || $("#SelectedModel").val() == '') {
        errorString += "<li>Model info is required</li>";
        flag = false;
        errorCount++;
    } else {

    }
    if ($("#SelectedTrim").val() == null || $("#SelectedTrim").val() == undefined || $("#SelectedTrim").val() == '') {
        errorString += "<li>Trim info is required</li>";
        flag = false;
        errorCount++;
    } else {

    }

    if ($("#Condition").val() == '') {
        errorString += "<li>Condition info is required</li>";
        flag = false;
        errorCount++;
    } else {

    }


    if (trimString($("#Mileage").val()) == "") {
        errorString += "<li>Mileage info is required</li>";
        flag = false;
        errorCount++;
    } else {
        var mileageInfo = trimString($("#Mileage").val());
        var numbermileage = Number(mileageInfo.replace(/[^0-9\.]+/g, ""));
        if (isNaN(numbermileage) || numbermileage > 1000000) {
            errorString += "<li>Valid mileage info is required</li>";
            flag = false;
            errorCount++;
        }
    }
    //         }
    if (flag == false) {
        if (errorCount == 1)
            error(errorString, "60px");
        else if (errorCount == 2)
            error(errorString, "90px");
        else
            error(errorString, "130px");
        return false;
    }

    return true;
}

$(document).ready(function () {

    $("#Mileage").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });

    $("#step2").live('click', function () {
        if (validateForm()) {
            $("#TradeVehicleForm").submit();
            //                 if (trimString($("#Vin").val()) == "") {
            //                     $("#TradeVehicleForm").submit();
            //                 } else {
            //                     $.post('<%= Url.Content("~/Trade/VinDecode") %>', { vin: $("#Vin").val() }, function (result) {
            //                         if (result == "False") {
            //                             error("Invalid vin or no result matched. Please try again", "97px");
            //                         } else {
            //                             $("#TradeVehicleForm").submit();
            //                         }
            //                     });
            //                 }

        }
    });

    $("#SelectedYear").live('change', function () {
        $("#SelectedMake").html("");
        $("#SelectedModel").html("");
        $("#SelectedTrim").html("");
        $("#SelectedMakeValue").val("");
        $("#SelectedModelValue").val("");
        $("#SelectedTrimValue").val("");

        if ($("#SelectedYear").val() != "Year..." && $("#SelectedYear").val() != "0") {

            $('#decode select').each(function () {
                $(this).prop('disabled', true);
            });

            $.ajax({
                type: "GET",
                url: "/Trade/GetMakes",
                data: { yearId: $("#SelectedYear").val() },
                dataType: 'html',
                success: function (data) {
                    $("#divMakes").html(data);
                    EnableSelectList();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status + ' ' + thrownError);
                }
            });

        }
    });

    $("#SelectedMake").live('change', function () {
        $("#SelectedModel").html("");
        $("#SelectedTrim").html("");
        $("#SelectedModelValue").val("");
        $("#SelectedTrimValue").val("");
        $("#SelectedMakeValue").val("");
        if ($("#SelectedMake").val() != "Make..." && $("#SelectedMake").val() != "0") {

            $('#decode select').each(function () {
                $(this).prop('disabled', true);
            });

            $.ajax({
                type: "GET",
                url: "/Trade/GetModels",
                data: { yearId: $("#SelectedYear").val(), makeId: $("#SelectedMake").val() },
                dataType: 'html',
                success: function (data) {
                    $("#divModels").html(data);
                    EnableSelectList();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status + ' ' + thrownError);
                }
            });

            $("#SelectedMakeValue").val($("#SelectedMake :selected").text());

        }
    });

    $("#SelectedModel").live('change', function () {
        $("#SelectedTrim").html("");
        $("#SelectedTrimValue").val("");
        $("#SelectedModelValue").val("");
        if ($("#SelectedModel").val() != "Model..." && $("#SelectedModel").val() != "0") {
            $('#decode select').each(function () {
                $(this).prop('disabled', true);
            });

            $.ajax({
                type: "GET",
                url: "/Trade/GetTrims",
                data: { yearId: $("#SelectedYear").val(), makeId: $("#SelectedMake").val(), modelId: $("#SelectedModel").val() },
                dataType: 'html',
                success: function (data) {
                    $("#divTrims").html(data);
                    EnableSelectList();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status + ' ' + thrownError);
                }
            });

            $("#SelectedModelValue").val($("#SelectedModel :selected").text());

        }
    });

    $("#SelectedTrim").live('change', function () {
        if ($("#SelectedTrim").val() != "Trim..." && $("#SelectedTrim").val() != "0") {

            $("#SelectedTrimValue").val($("#SelectedTrim :selected").text());
        }
    });
});

function EnableSelectList() {
    $("#SelectedYear").removeAttr('disabled');
    $("#SelectedMake").removeAttr('disabled');
    $("#SelectedModel").removeAttr('disabled');
    $("#SelectedTrim").removeAttr('disabled');
    $("#Condition").removeAttr('disabled');
}