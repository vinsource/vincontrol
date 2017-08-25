google.load('maps', '3', {
    other_params: 'sensor=false'
});
google.setOnLoadCallback(
    OnGoogleLoad/// <reference path="../../Controllers/SwitchController.cs" />

);

function OnGoogleLoad() {
    InitializeChart();
    console.log("Google Load");
    AssignEvents();
    AssignSpecificEvents();
}

function AssignSpecificEvents() {
    $('#seeNationwideMarketData').click(function () {
        var listingId = ListingId;
        var isCarscom = $('#radioCarsCom').is(":checked");
        var url = "/Chart/NavigateToGoogleNationwide?ListingId=" + listingId + "&isCarsCom=" + isCarscom;
        window.location.href = url;
    });

    $('#viewChartLink').click(function () {
        GetFilterDataAndNavigateTo("/Chart/ViewFullChart");
    });
}

function LoadValueInHidden(requestHiddenAutoTraderUrl, requestHiddenCarsComUrl) {
    if ($('#IsCarsCom').val() == 'True')
        LoadAutoTraderInHidden(requestHiddenAutoTraderUrl != undefined ? requestHiddenAutoTraderUrl.replace('PLACEHOLDER', ListingId) : null);
    else
        LoadCarsComInHidden(requestHiddenCarsComUrl != undefined ? requestHiddenCarsComUrl.replace('PLACEHOLDER', ListingId) : null);

}





   