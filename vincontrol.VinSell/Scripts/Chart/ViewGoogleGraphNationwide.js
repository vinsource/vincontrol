google.load('maps', '3', {
    other_params: 'sensor=false'
});
google.setOnLoadCallback(
    OnGoogleLoad/// <reference path="../../Controllers/SwitchController.cs" />
);

function OnGoogleLoad() {
    InitializeChart();
    AssignEvents();
    AssignSpecificEvents();
}


function AssignSpecificEvents() {
    $('#seeLocalMarketData').click(function () {
        var listingId = ListingId;
        var isCarscom = $('#radioCarsCom').is(":checked");
        var url = "/Chart/NavigateToGoogleLocalMarket?ListingId=" + listingId + "&isCarsCom=" + isCarscom;
        window.location.href = url;
    });

    $('#viewChartLink').click(function () {
        GetFilterDataAndNavigateTo("/Chart/NavigateToNationwide");
    });
}

function LoadValueInHidden(requestHiddenAutoTraderUrl, requestHiddenCarsComUrl) {
}