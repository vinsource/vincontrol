var yearTo = new Date().getFullYear();
var yearFrom = yearTo - 4;
var dataMarket;
var dataAuction;
$(document).ready(function () {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    $.ajax({
        type: "POST",
        url: "/Chart/GetWishListMarketDataByListingNationwideWithHttpPostByYear100Mies",
        data: { yearFrom: yearFrom, yearTo: yearTo, make: $('#hdMake').val(), model: $('#hdModel').val(), trim: $('#hdTrim').val() },
        success: function (results) {
            dataMarket = results;
            $('#divHasContent').html($("#wishListDetailTemplate").render(dataMarket.ListCarInfo));
            $('#divMarketLink').addClass('WishListMarket_tabActive');
            $('#divAuctionLink').removeClass('WishListMarket_tabActive');

            $('#Highest').html(formatDolar(dataMarket.Highest));
            $('#Average').html(formatDolar(dataMarket.Average));
            $('#Lowest').html(formatDolar(dataMarket.Lowest));
            $.unblockUI();
        }
    });
    
    $.ajax({
        type: "POST",
        url: "/Chart/GetWishListAuctionDataByListingNationwideWithHttpPostByYear100Mies",
        data: { yearFrom: yearFrom, yearTo: yearTo, make: $('#hdMake').val(), model: $('#hdModel').val(), trim: $('#hdTrim').val() },
        success: function (results) {
            dataAuction = results;
        }
    });
});

function formatDolar(num) {
    var p = parseInt(num).toFixed(2).split(".");
    return '$' + p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num + (i && !(i % 3) ? "," : "") + acc;
    }, "").replace('-,', '-');
}

function ViewMarket() {
    $('#divHasContent').html($("#wishListDetailTemplate").render(dataMarket.ListCarInfo));
    $('#divMarketLink').addClass('WishListMarket_tabActive');
    $('#divAuctionLink').removeClass('WishListMarket_tabActive');
    $('#market_right_content_nav').show();
    $('#auction_right_content_nav').hide();
}

function ViewAuction() {
    $('#divHasContent').html($("#auctionWishListDetailTemplate").render(dataAuction.ListCarInfo));
    $('#divMarketLink').removeClass('WishListMarket_tabActive');
    $('#divAuctionLink').addClass('WishListMarket_tabActive');
    $('#market_right_content_nav').hide();
    $('#auction_right_content_nav').show();
}