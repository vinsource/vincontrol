$(document).ready(function (e) {
    InitializeChart();
    AssignEvents();
    AssignSpecificEvents();
});

function  AssignSpecificEvents() {
    $('#seeNationwideMarketData').click(function () {
        var listingId = ListingId;
        var isCarscom = $('#radioCarsCom').is(":checked");
        var url = "/Chart/NavigateToNationwide?ListingId=" + listingId + "&isCarsCom=" + isCarscom;
        window.location.href = url;
    });

    $('#viewGoogleMapLink').click(function () {
        GetFilterDataAndNavigateTo("/Chart/ViewGoogleGraph");
    });
}



function LoadValueInHidden(requestHiddenAutoTraderUrl, requestHiddenCarsComUrl) {
    if ($('#IsCarsCom').val() == 'True')
        LoadAutoTraderInHidden(requestHiddenAutoTraderUrl != undefined ? requestHiddenAutoTraderUrl.replace('PLACEHOLDER', ListingId) : null);
    else
        LoadCarsComInHidden(requestHiddenCarsComUrl != undefined ? requestHiddenCarsComUrl.replace('PLACEHOLDER', ListingId) : null);

}





//function OverrideSaved() {
//    var value = $("#FilterOptions").val();

//    if (value) {
//        var obj = $.parseJSON(value.replace(/'/g, "\""));
////        console.log(obj);
////        console.log($("input[key=\"" + obj.dealerType + "\"]"));
////        console.log($("input[key=\"" + obj.dealerType + "\"]").attr('key'));
//        //        $("input[name=dealertype]").attr('checked', false);

//        //                $("input[key=" + obj.dealerType + "]").attr('checked', 'checked');
//        $("input[key=\"" + obj.webSource + "\"]").attr('checked', true);
////        if (obj.webSource === "carscom") {
////            $("input[key=\"" + obj.webSource + "\"]").click();
////        }

//        $("input[key=\"" + obj.dealerType + "\"]").attr('checked', true);
////        $("input[key=\"" + obj.dealerType + "\"]").click();

//        $('input[id^=ddcl-trim-filter-i]').removeAttr('checked');
//        $('label[for^=ddcl-trim-filter-i]').each(function () {
//            console.log($(this).text());
//            console.log(obj.trims);
//            console.log($.inArray($(this).text(), obj.trims));
//            if ($.inArray($(this).text(), obj.trims) > -1) {
//                $('#' + $(this).attr('for')).attr('checked', true);
//            }
//        });


//        // #######################

//        // #######################
//        // OPTION DROPDOWN
//        var _defaultOptions =[];
//        // OPTION DROPDOWN
//        // #######################

//        // #######################
//        // TRIM DROPDOWN
//        var _defaultTrims = GetSelectedTrims();
//        // TRIM DROPDOWN
//        // #######################

//        // #######################
//        // DEALERTYPE
//        GetCheckedDealerType();
//        // DEALERTYPE
//        // #######################

//        // draw chart
//        if (fRange == 'nation')
//            fRange = 10000;


////        $filter.title = {};
////        $filter.title.trim = obj.trims;
////        _defaultTrims = obj.trims;
//        if (default_trim != null) {
//            $filter.title = {}; $filter.title.trim = [];
//            if (default_trim.length == 1 && default_trim[0] == 0)
//                delete $filter.title;
//            else {
//                for (i = 0; i < default_trim.length; i++) {
//                    if (default_trim[i] != "") {
//                        $filter.title.trim.push(default_trim[i]);
//                    }
//                }
//            }
//        }

//        console.log("$data");
//        console.log($data);
//        console.log("fRange");
//        console.log(fRange);
//        console.log("$filter");
//        console.log($filter);
//        console.log("$dCar");
//        console.log($dCar);
//        console.log("expand");
//        console.log(expand);
//        console.log("_defaultOptions");
//        console.log(_defaultOptions);
//        console.log("_defaultTrims");
//        console.log(_defaultTrims);
//        drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);
//    }
//}