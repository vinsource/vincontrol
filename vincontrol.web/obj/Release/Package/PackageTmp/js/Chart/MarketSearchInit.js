google.load('maps', '3', {
    other_params: 'sensor=false'
});
google.setOnLoadCallback(
    OnGoogleLoad/// <reference path="../../Controllers/SwitchController.cs" />

);

function OnGoogleLoad() {
    //InitializeChart();
    //GetTempData();
    AssignEvents();
    //    AssignSpecificEvents();
}

//function GetTempData() {
//    // initialize data set array
//    $data = [];
//    var request_url = requestNationwideUrl; //requestAutoTraderUrl;
//    request_url = request_url.replace('PLACEHOLDER', 106152);
//    $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });

//    $.ajax({
//        type: "POST",
//        url: request_url,
//        data: {},
//        dataType: 'json',
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {
//            DrawMarket(data);

//        }
//    });
//}


