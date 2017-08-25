$(document).ready(function (e) {
    InitializeChart();
    AssignEvents();
    AssignSpecificEvents();
});

function AssignSpecificEvents() {
    $('#seeLocalMarketData').click(function () {
        var listingId = ListingId;
        var isCarscom = $('#radioCarsCom').is(":checked");
        var url = "/Chart/NavigateToLocalMarket?ListingId=" + listingId + "&isCarsCom=" + isCarscom;
        window.location.href = url;
    });

    $('#viewGoogleMapLink').click(function () {
        GetFilterDataAndNavigateTo("/Chart/NavigateToGoogleNationwide");
    });
}

function LoadValueInHidden(requestHiddenAutoTraderUrl, requestHiddenCarsComUrl) {
}
