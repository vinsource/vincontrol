
var apprailsalFilterList = [];
function initializeFilter(dataList) {
    if (dataList.length == 0) {
        return;
    }

    var arrYear = [];
    var maxPrice = 0;
    var minMile = dataList[0].Mileage;
    var maxMile = dataList[0].Mileage;

    dataList.forEach(function (data) {
        if (arrYear.indexOf(data.ModelYear) == -1) {
            arrYear.push(data.ModelYear);
        }

        if (data.SalePrice > maxPrice) {
            maxPrice = data.SalePrice;
        }

        if (data.Mileage < minMile) {
            minMile = data.Mileage;
        }

        if (data.Mileage > maxMile) {
            maxMile = data.Mileage;
        }
    });

    arrYear.sort(function (a, b) { return b - a });

    var selectYear = $("#selectYear");
    var selectMake = $("#selectMake");
    var selectModel = $("#selectModel");
    var selectTrim = $("#selectTrim");
    var selectPrice = $("#selectPrice");
    var mileFrom = $("#mileFrom");
    var mileTo = $("#mileTo");

    createListItems(selectYear, arrYear);
    resetListItems(selectMake);
    resetListItems(selectModel);
    resetListItems(selectTrim);

    createPriceItems(selectPrice, maxPrice);

    if (parseInt(minMile) == 0) {
        mileFrom.val(0);
    }
    else {
        mileFrom.val(formatNumberString(minMile));
    }

    if (parseInt(maxMile) == 0) {
        mileTo.val(0);
    }
    else {
        mileTo.val(formatNumberString(maxMile));
    }

    $("#selectYear").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedYear = $("#selectYear option:selected").text();
        var arrMake = [];

        dataList.forEach(function (data) {
            if (data.ModelYear == selectedYear && arrMake.indexOf(data.Make) == -1) {
                arrMake.push(data.Make);
            }
        });

        createListItems(selectMake, arrMake);
        resetListItems(selectModel);
        resetListItems(selectTrim);

        $.unblockUI();
    });

    $("#selectMake").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedYear = $("#selectYear option:selected").text();
        var selectedMake = $("#selectMake option:selected").text();
        var arrModel = [];

        dataList.forEach(function (data) {
            if (data.ModelYear == selectedYear && data.Make == selectedMake && arrModel.indexOf(data.Model) == -1) {
                arrModel.push(data.Model);
            }
        });

        createListItems(selectModel, arrModel);
        resetListItems(selectTrim);

        $.unblockUI();
    });

    $("#selectModel").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedYear = $("#selectYear option:selected").text();
        var selectedMake = $("#selectMake option:selected").text();
        var selectedModel = $("#selectModel option:selected").text();
        var arrTrim = [];

        dataList.forEach(function (data) {
            if (data.ModelYear == selectedYear && data.Make == selectedMake && data.Model == selectedModel && arrTrim.indexOf(data.Trim) == -1) {
                arrTrim.push(data.Trim);
            }
        });

        createListItems(selectTrim, arrTrim);

        $.unblockUI();
    });        
}

$(document).ready(function () {
    setTimeout(function () {
        if (appraisalStatus == 1) {
            getAppraisalPendingJson();
        } else {
            getAppraisalJson();
        }
        
    }, 3000);

    $("#submitFilter").click(function () {
        
        var year = $("#selectYear").val();
        var make = $("#selectMake").val();
        var model = $("#selectModel").val();
        var trim = $("#selectTrim").val();
        //  var price = $("#selectPrice").val();
        var mileFrom = $("#mileFrom").val();
        var mileTo = $("#mileTo").val();
        var listResult = [];

        $.each(apprailsalFilterList, function (index, item) {
            var tempYear = year.toLowerCase();
            var tempMake = make.toLowerCase();
            var tempModel = model.toLowerCase();
            var tempTrim = trim.toLowerCase();
            mileFrom = mileFrom.toString().replace(/,/g, "");
            mileFrom = parseFloat(mileFrom);

            mileTo = mileTo.toString().replace(/,/g, "");
            mileTo = parseFloat(mileTo);

            var tempItempMake = item.Make.toLowerCase();
            var tempItempModel = item.Model.toLowerCase();
            var tempItempTrim = item.Trim.toLowerCase();

            var command = 'if(';
            if (tempYear != "all") {
                command += 'item.ModelYear == tempYear';
            }
            if (tempMake != "all") {
                command += '&& tempItempMake == tempMake';
            }
            if (tempModel != "all") {
                command += '&& tempItempModel == tempModel';
            }
            if (tempTrim != "all") {
                command += '&& tempItempTrim == tempTrim';
            }
            if (mileFrom != "") {
                if (tempYear == 'all') {
                    command += 'item.Mileage >= mileFrom';
                } else {
                    command += '&& item.Mileage >= mileFrom';
                }

            }

            if (mileTo != "") {
                if (tempYear == 'all' && mileFrom == "") {
                    command += 'item.Mileage <= mileTo';
                } else {
                    command += '&& item.Mileage <= mileTo';
                }

            }

            command += '){listResult.push(item)}';
            eval(command);

        });

        //console.log(listResult);
        appendFilter(listResult);
        $(".vin_listVehicle_holder").css("height", 495);
    });

    var mousewheelevt = (/Firefox/i.test(navigator.userAgent)) ? "DOMMouseScroll" : "mousewheel";
    $('.vin_listVehicle_holder').live(mousewheelevt, function () {
        
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {            
            $('#divLoading').show();
            var appraisalListUrl = appraisalStatus == 1
                ? "/Appraisal/GetPendingAppraisalListJson?Condition=" + idValue + "&isUp=" + isUp + "&pageIndex=" + (++pageIndex) + "&pageSize=" + pageSize
                : "/Appraisal/GetRecentAppraisalListJson?Condition=" + idValue + "&isUp=" + isUp + "&pageIndex=" + (++pageIndex) + "&pageSize=" + pageSize + '&fromDate=' + $(".appraisal_date_From").val() + '&toDate=' + $(".appraisal_date_To").val();
            $.ajax({
                type: "POST",
                dataType: "JSON",
                url: appraisalListUrl,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#divLoading').hide();
                    //$("#recentAppraisals").append(result);
                    appendFilter(result.UnlimitedAppraisals, '.vin_listVehicle_holder');
                    setHeaderVisibility();
                },
                error: function (err) {
                    $('#divLoading').hide();
                    console.log(err.status + " - " + err.statusText);
                }
            });
        }
    });

    $("div[id^=appraisalsort]").live('click', function () {        
        pageIndex = 1;
        pageSize = 50;
        scrollHeight = 400;
        $("#recentAppraisals").html("<div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>");
        idValue = this.id.split('_')[1];

        if (lastCondition != idValue) { // reset
            if (idValue == "3") {
                isUp = false;
            }
            else {
                isUp = true;
            }
        }
        else { // toggle
            isUp = !isUp;
        }

        lastCondition = idValue;

        var imgID = 'imgSort_' + idValue;
        $('img.imgSort').each(function () {
            $(this).attr('src', '../Content/images/vincontrol/dot.png');
        });

        if (isUp) {
            $('#' + imgID).attr('src', '../Content/images/vincontrol/up.png');
        } else {
            $('#' + imgID).attr('src', '../Content/images/vincontrol/down.png');
        }

        var appraisalListUrl = appraisalStatus == 1 
            ? "/Appraisal/SortPendingAppraisalListJson?Condition=" + idValue + "&isUp=" + isUp
            : "/Appraisal/SortAppraisalListJson?Condition=" + idValue + "&isUp=" + isUp + '&fromDate=' + $(".appraisal_date_From").val() + '&toDate=' + $(".appraisal_date_To").val();

        $.ajax({
            type: "POST",
            dataType: "JSON",
            url: appraisalListUrl,
            data: {},
            cache: false,
            traditional: true,
            success: function (result) {
                //$("#recentAppraisals").html(result);
                //if ($("#filterPanel").is(":visible")) {
                //    $(".vin_listVehicle_holder").css("height", 495);
                //}
                apprailsalFilterList = result.UnlimitedAppraisals;
                initializeFilter(result.UnlimitedAppraisals);
                appendFilter(result.UnlimitedAppraisals);
                $(".vin_listVehicle_holder").css("height", 560);

            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
            }
        });
    });
});

function appendFilter(data, selector) {
    var html = (typeof selector == "undefined") ? '<div class="vin_listVehicle_holder">' : '';
    $.each(data, function (index, item) {
        if (index % 2 == 0) {
            html += '<div class="contain_list list_even ">';
        } else {
            html += '<div class="contain_list">';
        }
        
        html += '<a class="vin_editProfile" href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"></a>';
        html += '<div class="right_content_items right_content_items_img">';
        html += '<a class="vin_editProfile" href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"></a>';

        html += '<a href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"><img alt="" height="61" src="' + item.FirstPhoto + '" width="65"></a>';
        html += '</div>';
        html += '<a class="vin_editProfile" href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"><div class="right_content_items right_content_items_vin nav_long" title="' + item.VinNumber + '">' + item.VinNumber.substring(9,17) + '</div></a>';
        html += '<a class="vin_editProfile" href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"><div class="right_content_items nav_middle">'+item.ModelYear+'</div></a>';

        html += '<a class="vin_editProfile" href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"><div class="right_content_items nav_long" title="' + item.Make + '">' + item.Make + '</div></a>';
        html += '<a class="vin_editProfile" href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"><div class="right_content_items nav_long" title="' + item.Model + '">' + item.Model + '</div></a>';
        html += '<a class="vin_editProfile" href="/Appraisal/ViewProfileForAppraisal?AppraisalId=' + item.AppraisalID + '"><div class="right_content_items nav_long" title="' + item.Trim + '">' + item.Trim + '</div></a>';
        html += '<div class="right_content_items nav_middle" title="' + item.ExteriorColor + '">' + item.ExteriorColor + '</div>';

        html += '<div class="right_content_items right_content_items_owners nav_short">' + item.StrCarFaxOwner + '</div>';
        html += '<div class="right_content_items right_content_items_client">' + item.StrClientName + '</div>';
        html += '<div class="right_content_items right_content_items_appraiser nav_long" title="">' + item.AppraisalBy + '</div>';
        html += '<form name="SaveSalePriceForm" class="SalePriceForm">';

        html += '<div class="right_content_items vehicle_price_appraisal divValue">';
        html += '<input type="text" id="' + item.AppraisalID + '" name="odometer" class="sForm" value="' + CommaFormatted(item.Mileage) + '" data-validation-engine="validate[funcCall[checkMiles]]">';
        html += '<img class="imgLoadingAppraisal" src="../Content/images/ajaxloadingindicator.gif" height="15px">';
        html += '</div>';

        html += '<div class="right_content_items vehicle_price_appraisal divValue">';
        if (!item.ACV) {
            item.ACV = "";
        } else {
            item.ACV = CommaFormatted(item.ACV);
        }
        //console.log(item.CarFax);
        html += '<input type="text" id="' + item.AppraisalID + '" name="Acv" class="sForm" value="' + item.ACV + '" data-validation-engine="validate[funcCall[checkSalePrice]]">';
        html += '<img class="imgLoadingAppraisal" src="../Content/images/ajaxloadingindicator.gif" height="15px">';
        html += '</div></form>';

        html += '<div class="invent_expanded_date_holder invent_expanded" style="display: block;"><label class="invent_expanded_date">' + formatFullDate(item.DateOfAppraisal) + '</label></div>';

        html += '<div class="invent_expanded_action_holder invent_expanded" style="display: block;">';
        html += '<label class="invent_expanded_recon"><input type="checkbox" id="Recon_chk' + item.AppraisalID + '" class="chk chk' + item.AppraisalID + '" onclick="ChangeStatus(\'' + item.VinNumber + '\',' + item.AppraisalID + ',4)"> Recon</label>';
        html += '<label class="invent_expanded_recon"><input type="checkbox" id="Inventory_chk' + item.AppraisalID + '" class="chk chk' + item.AppraisalID + '" onclick="ChangeStatus(\'' + item.VinNumber + '\',' + item.AppraisalID + ',2)"> Inventory</label>';

        html += '<label class="invent_expanded_recon"><input type="checkbox" id="Wholesale_chk' + item.AppraisalID + '" class="chk chk' + item.AppraisalID + '" onclick="ChangeStatus(\'' + item.VinNumber + '\',' + item.AppraisalID + ',3)"> Wholesale</label>';
        html += '<label class="invent_expanded_recon"><input type="checkbox" id="Auction_chk' + item.AppraisalID + '" class="chk chk' + item.AppraisalID + '" onclick="ChangeStatus(\'' + item.VinNumber + '\',' + item.AppraisalID + ',5)"> Auction</label>';
        html += '<label class="invent_expanded_recon"><input type="checkbox" id="Loaner_chk' + item.AppraisalID + '" class="chk chk' + item.AppraisalID + '" onclick="ChangeStatus(\'' + item.VinNumber + '\',' + item.AppraisalID + ',6)"> Loaner</label>';
        html += '<label class="invent_expanded_recon"><input type="checkbox" id="Trade_chk' + item.AppraisalID + '" class="chk chk' + item.AppraisalID + '" onclick="ChangeStatus(\'' + item.VinNumber + '\',' + item.AppraisalID + ',7)"> Trade Not Clear</label>';

        html += '<label class="invent_expanded_recon"><input type="checkbox" id="Sold_chk' + item.AppraisalID + '" class="chk chk' + item.AppraisalID + '" onclick="ChangeStatus(\'' + item.VinNumber + '\',' + item.AppraisalID + ',1)"> Sold</label>';

        if (item.InventoryId != 0) {
            html += '<label class="invent_expanded_recon"><a href="/Inventory/ViewIProfile?ListingId=' + item.InventoryId + '"><div class="btns_shadow loAppSeeInventory">See in Inventory</div></a></label>';
        }
        html += '</div></div>';
       
    });
    html += (typeof selector == "undefined") ? '</div>' : '';

    if (typeof selector == "undefined") $("#recentAppraisals").html(html);
    else $(selector).append(html);
}

var bIsShowingFilterPanel = false;
function toggleFilterPanel() {
    bIsShowingFilterPanel = !bIsShowingFilterPanel;

    if (bIsShowingFilterPanel) {
        $("#filterPanel").show();
        $(".vin_listVehicle_holder").css("height", 495);
    }
    else {
        $("#filterPanel").hide();
        $(".vin_listVehicle_holder").css("height", 560);
    }
}

function getAppraisalPendingJson() {
    var dataUrl = '/Appraisal/GetPendingAppraisalListJson';
    $.ajax({
        type: "POST",        
        dataType: "JSON",
        url: dataUrl,
        data: {

        },
        cache: false,
        traditional: true,
        success: function (result) {
            apprailsalFilterList = result.UnlimitedAppraisals;
            initializeFilter(result.UnlimitedAppraisals);
            appendFilter(result.UnlimitedAppraisals);
            $(".vin_listVehicle_holder").css("height", 560);
            $(".appraisals_number_below").html(result.NumberOfRecords);
        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
        }
    });
}

function getAppraisalJson() {
    var dataUrl = '/Appraisal/GetRecentAppraisalListJson' + '?fromDate=' + $("#appraisal_date_From").val() + '&toDate=' + $("#appraisal_date_To").val();
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: dataUrl,
        data: {},
        cache: false,
        traditional: true,
        success: function (result) {
            apprailsalFilterList = result.UnlimitedAppraisals;
            initializeFilter(result.UnlimitedAppraisals);
            appendFilter(result.UnlimitedAppraisals);
            $(".vin_listVehicle_holder").css("height", 560);
            $(".appraisals_number_below").html(result.NumberOfRecords);
        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
        }
    });
}

function resetListItems(select) {
    $(select).empty();

    var el = document.createElement("option");
    el.textContent = "All";
    el.value = "All";
    select.append(el);
}

function createListItems(select, options) {
    $(select).empty();

    options.unshift("All");

    for (var i = 0; i < options.length; i++) {
        var opt = options[i];
        var el = document.createElement("option");
        el.textContent = opt;
        el.value = opt;
        select.append(el);
    }
}

function createPriceItems(select, maxPrice) {
    $(select).empty();

    var priceIncrement = 10000;
    var price = 10000;
    var el = document.createElement("option");
    el.textContent = "All";
    el.value = 0;
    select.append(el);

    el = document.createElement("option");
    el.textContent = "Less than $" + formatNumberString(price);
    el.value = price / priceIncrement;
    select.append(el);

    while (price < 100000) {
        price = price + priceIncrement;

        el = document.createElement("option");
        el.textContent = "$" + formatNumberString(price - priceIncrement) + " - $" + formatNumberString(price);
        el.value = price / priceIncrement;
        select.append(el);
    }

    el = document.createElement("option");
    el.textContent = "Greater than $" + formatNumberString(price);
    el.value = (price / priceIncrement) + 1;
    select.append(el);
}

function formatNumberString(number) {
    return formatDollar(parseInt(number));
}

function validateInput(event) {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.preventDefault();
    }
}

function getMileValue(mile) {
    return mile.replace(/,/g, "");
}

function formatMile(event, obj) {
    if (event == null || event.keyCode == 46 || event.keyCode == 8
         || (event.keyCode >= 48 && event.keyCode <= 57)) {
        var value = $(obj).val();

        if (value == "") {
            $(obj).val(0);
            return;
        }

        var originalLength = value.length;
        var rightPos = value.length - obj.selectionEnd;
        var leftPos = obj.selectionEnd;

        value = value.replace(/,/g, "")
        value = parseInt(value).toString();
        value = value.split("").reverse().join("");

        var index = 0;
        var j = 0;

        while (index < value.length - 1) {
            index++;
            j++;

            if (j % 3 == 0) {
                value = value.substr(0, index) + "," + value.substr(index, value.length);
                index++;
            }
        }

        value = value.split("").reverse().join("");
        $(obj).val(value);

        if (value.length > originalLength) {
            rightPos = value.length - rightPos;

            obj.selectionStart = rightPos;
            obj.selectionEnd = rightPos;
        }

        if (value.length <= originalLength) {
            obj.selectionStart = leftPos;
            obj.selectionEnd = leftPos;
        }
    }
}

