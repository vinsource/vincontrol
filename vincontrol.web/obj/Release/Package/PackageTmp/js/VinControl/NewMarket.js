var isMarketSearch = 1;
$("#market_search_yearfrom").live('change', function (e) {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var yearFrom = e.target.options[e.target.selectedIndex].value;
    var yearTo = $('#market_search_yearto').val();
    $.ajax({
        type: "POST",
        url: "/Chart/GetMakesFromChrome",
        data: { yearFrom: yearFrom, yearTo: yearTo },
        success: function (results) {
            reset();
            $("#divMake").html(results);
            $.unblockUI();
        }
    });
    $.unblockUI();
});

$("#market_search_yearto").live('change', function (e) {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var yearFrom = $('#market_search_yearfrom').val();
    var yearTo = e.target.options[e.target.selectedIndex].value;
    $.ajax({
        type: "POST",
        url: "/Chart/GetMakesFromChrome",
        data: { yearFrom: yearFrom, yearTo: yearTo },
        success: function (results) {
            reset();
            $("#divMake").html(results);
            $.unblockUI();
        }
    });
    $.unblockUI();
});

var isUsingTrimSearch = false;

$("#market_search_make").live('change', function (e) {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var makeID = e.target.options[e.target.selectedIndex].value;
    var yearFrom = $('#market_search_yearfrom').val();
    var yearTo = $('#market_search_yearto').val();

    var selectedMake = $("#market_search_make option:selected").text();

    if (selectedMake == "BMW" || selectedMake == "Mercedes-Benz") {
        isUsingTrimSearch = true;
        $("#searchTrimHolder").css("display", "block");
    }
    else {
        isUsingTrimSearch = false;
        $("#searchTrimHolder").css("display", "none");
    }

    $.ajax({
        type: "POST",
        url: "/Chart/GetModelsFromChrome",
        data: { yearFrom: yearFrom, yearTo: yearTo, makeID: makeID },
        success: function (results) {
            $("#divModel").html(results);
            resetSearchTrim();
            $.unblockUI();
        }
    });
    $.unblockUI();
});

$("#market_search_model").live('change', function (e) {
    var selectedMake = $("#market_search_make option:selected").text();

    if (selectedMake != "BMW" && selectedMake != "Mercedes-Benz") {
        return;
    }

    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var yearFrom = $('#market_search_yearfrom').val();
    var yearTo = $('#market_search_yearto').val();
    var makeID = $("#market_search_make option:selected").val();
    var modelID = $("#market_search_model option:selected").val();

    $.ajax({
        type: "POST",
        url: "/Chart/GetTrimsFromChrome",
        data: { yearFrom: yearFrom, yearTo: yearTo, makeID: makeID, modelID:modelID },
        success: function (results) {
            $("#divTrim").html(results);

            if (results.search("option") < 0) {
                isUsingTrimSearch = false;
                $("#searchTrimHolder").css("display", "none");
            }
            else {
                isUsingTrimSearch = true;
                $("#searchTrimHolder").css("display", "block");
            }

            $.unblockUI();
        }
    });
    $.unblockUI();
});

function checkYearRange(field, rules, i, options) {
    if (parseInt($('#market_search_yearfrom').val()) > parseInt($('#market_search_yearto').val())) {
        return "Year From can't be greater than Year To";
    }
    if (parseInt($('#market_search_yearfrom').val()) + 5 <= parseInt($('#market_search_yearto').val())) {
        return "Year Range can't be greater than 5 years";
    }
}

function checkSelectedMake(field, rules, i, options) {
    if ($("#market_search_make option:selected").val() == "-1") {
        return "Make is required";
    }
}

function checkSelectedModel(field, rules, i, options) {
    if ($('#market_search_model option:selected').val() == "-1") {
        return "Model is required";
    }
}

function checkSelectedTrim(field, rules, i, options) {
    if ($('#market_search_trim option:selected').val() == "-1") {
        return "Trim is required";
    }
}


function resetMake() {
    $('#market_search_make').html('<option value="-1">---</option>');
}

function resetModel() {
    $('#market_search_model').html('<option value="-1">---</option>');
}

function resetSearchTrim() {
    $('#market_search_trim').html('<option value="-1">---</option>');
}

function reset() {
    resetMake();
    resetModel();
    resetSearchTrim();
}

$(document).ready(function () {
    jQuery("#FormSearch").validationEngine();
    $('.mp_v2_header').hide();

    $('#rdbAllCertified').live("click", function () {
        $('#IsCertified').val('');
        drawChart($data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, _defaultTrims, GetDataTrim());
    });
    $('#rdbCertified').live("click", function () {
        $('#IsCertified').val('true');
        var filterData = [];
        $.each($data, function (subIndex, subObj) {
            if (subObj.certified == true) {
                filterData.push(subObj);
            }
        });
        if (filterData.length == 0) {
            var emptyData = {};
            drawChart(emptyData, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, _defaultTrims, GetDataTrim());
        } else {
            drawChart($data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, _defaultTrims, GetDataTrim());
        }
    });
    $('#rdbUnCertified').live("click", function () {
        $('#IsCertified').val('false');
        var filterData = [];
        if ($(this).not(':checked')) {
            $.each($data, function (subIndex, subObj) {
                if (subObj.certified == false) {
                    filterData.push(subObj);
                }
            });
        }
        if (filterData.length == 0) {
            var emptyData = {};
            drawChart(emptyData, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, _defaultTrims, GetDataTrim());
        } else {
            drawChart($data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, _defaultTrims, GetDataTrim());
        }
    });
    $('#btnGlobalMarketSearch').live("click", function () {
        ResetTrim();
        var flag = true;

        if ($("#market_search_yearfrom").validationEngine('validate'))
            flag = false;

        if ($("#market_search_yearto").validationEngine('validate'))
            flag = false;

        if ($("#market_search_make").validationEngine('validate'))
            flag = false;

        if ($("#market_search_model").validationEngine('validate'))
            flag = false;

        if (isUsingTrimSearch && $("#market_search_trim").validationEngine('validate'))
            flag = false;

        if (flag) {
            $('.mp_v2_header').show();
            $('#divNoContent').hide();
            $('.mp_v2_content_holder').show();
            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            var yearFrom = $('#market_search_yearfrom').val();
            var yearTo = $('#market_search_yearto').val();
            var make = $("#market_search_make option:selected").text();
            var model = $('#market_search_model option:selected').text();
            var trim = "";

            if (isUsingTrimSearch) {
                trim = $('#market_search_trim option:selected').text();
            }

            $.ajax({
                type: "POST",
                url: "/Chart/GetMarketDataByListingNationwideWithHttpPostByYear",
                data: { yearFrom: yearFrom, yearTo: yearTo, make: make, model: model , trim: trim},
                success: function (results) {
                    //console.log('=====================');
                    //console.log(results);
                    //console.log('=====================');
                    //var jsonObj = JSON.stringify(results.cars);
                    //console.log(jsonObj);
                    $('.mp_v2_content_right').show();
                    $('.mp_v2_cl_header').show();
                    if (results.cars == 0 || results.cars==null) {
                        $(".mp_v2_scroll_holder").hide();
                        $("#NoContent").show();
                    } else {
                        $(".mp_v2_scroll_holder").show();
                        $("#NoContent").hide();
                    }
                    DrawMarket(results);
                    $.unblockUI();
                }
            });
        }
    });
});

function ResetTrim() {
    $('#all').attr("checked", true);
    $('#radioAll').attr("checked", true);
}

function DrawMarket(data) {
    $data = [];
    //console.log(data);
    var carlist = data.cars;
  
    var marketMapping = new VINControl.Chart.MarketMapping();

    if (carlist != null) {
        for (var i = 0; i < carlist.length; i++) {
            var arr = carlist;
            $data[i] = marketMapping.InitializeCar(arr[i]);
        }
    }

    $dCar = null;
  
    default_trim = [];

    if (!(default_trim.length == 1 && default_trim[0] == '0'))
        default_trim = LoadDefaulTrimsInLowerCase(default_trim);

    dataTrims = data.trims;
    dataBodyStyles = data.bodyStyles;

  
    
    drawChart($data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, default_trim, data.trims, data.bodyStyles, default_bodystyle/*, GetDataTrim(), isExcludeCurrentCar*/);
  
    $.unblockUI();


}
