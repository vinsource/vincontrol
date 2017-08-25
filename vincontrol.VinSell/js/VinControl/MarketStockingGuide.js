//var isMarketSearch = 1;
//var isFirstTime = false;

//function checkYearRange(field, rules, i, options) {
//    if (parseInt($('#market_search_yearfrom').val()) > parseInt($('#market_search_yearto').val())) {
//        return "Year From can't be greater than Year To";
//    }
//    if (parseInt($('#market_search_yearfrom').val()) + 5 <= parseInt($('#market_search_yearto').val())) {
//        return "Year Range can't be greater than 5 years";
//    }
//    jQuery('#market_search_yearfrom').validationEngine('hide');
//    jQuery('#market_search_yearto').validationEngine('hide');
//}

//function resetMake() {
//    $('#market_search_make').html('<option>---</option>');
//}

//function resetModel() {
//    $('#market_search_model').html('<option>---</option>');
//}

//function reset() {
//    resetMake();
//    resetModel();
//}

$(document).ready(function () {
    //alert('duyvo');
    //var yearTo = new Date().getFullYear();
    //$('#market_search_yearfrom').val(yearTo - 4);
    //$('#market_search_yearto').val(yearTo);
    //var make = $("#hdnMake").val();
    //var model = $("#hdnModel").val();
    //$('#market_search_make').html('<option>' + make + '</option>');
    //$('#market_search_make').val(make);
    //$('#market_search_model').html('<option>' + model + '</option>');
    //$('#market_search_model').val(model);
    jQuery("#FormSearch").validationEngine();
    //$('#btnMarketStockSearch').live("click", function () {
    //    ResetTrim();
    //    jQuery("#FormSearch").validationEngine();
    //    if (!$("#market_search_yearfrom").validationEngine('validate') && !$("#market_search_yearto").validationEngine('validate') && !$("#market_search_make").validationEngine('validate') && !$("#market_search_model").validationEngine('validate')) {
    //        $('#divHolder').show();
    //        $('.mp_v2_header').show();
    //        $('#divNoContent').hide();
    //        $('.mp_v2_content_holder').show();
    //        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    //        var yearFrom = $('#market_search_yearfrom').val();
    //        var yearTo = $('#market_search_yearto').val();
    //        var make = $("#market_search_make option:selected").text();
    //        var model = $('#market_search_model option:selected').text();
    //        console.log(yearFrom + yearTo+ make+model);
    //        $.ajax({
    //            type: "POST",
    //            url: "/Chart/GetMarketDataByListingNationwideWithHttpPostByYear100Mies",
    //            data: { yearFrom: yearFrom, yearTo: yearTo, make: make, model: model, isFirstTime: isFirstTime },
    //            success: function (results) {
                   
    //                $('.mp_v2_content_right').show();
    //                $('.mp_v2_cl_header').show();
    //                if (results.cars == 0 || results.cars==null) {
    //                    $(".mp_v2_scroll_holder").hide();
    //                    $("#NoContent").show();
    //                } else {
    //                    $(".mp_v2_scroll_holder").show();
    //                    $("#NoContent").hide();
    //                }
    //                DrawMarket(results);
    //                isFirstTime = false;
    //                $.unblockUI();
    //            }
    //        });
    //    }
    //});
    setTimeout(function () {
        //isFirstTime = true;
        $('#btnGlobalMarketSearch').trigger("click");
    }, 100);
});

//function ResetTrim() {
//    $('#all').attr("checked", true);
//    $('#radioAll').attr("checked", true);
//}

//function DrawMarket(data) {
//    $data = [];
 
//    var carlist = data.cars;
//    var target = data.target;
//    var marketinfo = data.market;
    
//    if (carlist != null) {
//        for (var i = 0; i < carlist.length; i++) {
//            var arr = carlist;
//            //$imgstring = "";
//            var marketMapping = new VINControl.Chart.MarketMapping();
//            $data[i] = marketMapping.InitializeCar(arr[i]);
//        }
//    }
//    console.log($data);
//    $dCar = null;
  
//    default_trim = [];

//    LoadSavedSelections();
//    // draw chart

//    dataTrims = data.trims;

//    drawChart($data, ChartInfo.fRange, ChartInfo.$filter, null, expand, [], data.trims, GetDataTrim());
    
//    // unset filter
//    ChartInfo.$filter = {};
//    $.unblockUI();

    
//}
