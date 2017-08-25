var listInfo = {};

function LoadData() {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var dataUrl = '/StockingGuide/StockingGuideOtherBrandJson';
    $.ajax({
        type: "POST",
        //                contentType: "text/JSON; charset=utf-8",
        dataType: "JSON",
        url: dataUrl,
        data: {},
        cache: false,
        traditional: true,
        success: function (result) {
            listInfo = result;
            $('#v3OtherListHolder').html('<div class="v3BrandOther_list">' + $("#brandHeaderOtherTemplate").render(result.HeaderInfo) + '<div id="divOther">' + $("#brandOtherTemplate").render(result.StockingGuideBrandOtherData) + '</div>' + '</div>');
            $("a.iframe").fancybox({ 'margin': 0, 'padding': 0, 'width': 250, 'height': 142 });
            $.unblockUI();
        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
        }
    });
}

function openListCarIframe(make, model) {
    var listCarUrl = '/StockingGuide/ViewListCars?make=' + make.replace(/ /g, '+') + '&model=' + model.replace(/ /g, '+');
    $("<a href=" + listCarUrl + "></a>").fancybox({
        height: 400,
        width: 1090,
        margin: 0,
        padding: 0,
        overlayShow: true,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe'
    }).click();
}

function openListAuctionIframe(make, model) {
    var listCarUrl = '/StockingGuide/ViewListAuctions?make=' + make.replace(/ /g, '+') + '&model=' + model.replace(/ /g, '+');
    $("<a href=" + listCarUrl + "></a>").fancybox({
        height: 400,
        width: 1090,
        margin: 0,
        padding: 0,
        overlayShow: true,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe'
    }).click();
}


function openListMarketIframe(make, model) {
    var listCarUrl = '/Chart/StockingGuideMarketSearch?make=' + make.replace(/ /g, '+') + '&model=' + model.replace(/ /g, '+');

    $("<a href=" + listCarUrl + "></a>").fancybox({
        height: 700,
        width: 1090,
        margin: 0,
        padding: 0,
        overlayShow: true,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe'
    }).click();
}

$(document).ready(function () {
    console.log("before Load Data");
    LoadData();

    $("div.v3BrandOther_rowTitle > div.v3BrandOther_collumn").live("click", function () {
        var sortedField = $(this).attr("value");
        var sortedDirection = $(this).attr("sortDirection");
        console.log(listInfo.StockingGuideBrandOtherData);
        switch (sortedField) {
            case "HeaderSegments":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Model.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Model.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderHistory":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.History; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.History; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderStock":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Stock; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Stock; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderGuide":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Guide; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Guide; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderBalance":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Balance; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Balance; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderSupply":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Supply; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Supply; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderAge":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Age; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Age; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderTurn":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.TurnOver; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.TurnOver; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderGrossPerUnit":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.GrossPerUnit; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.GrossPerUnit; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderRecon":
                if (sortedDirection == 'up') {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Recon; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    listInfo.StockingGuideBrandOtherData = _(listInfo.StockingGuideBrandOtherData).sortBy(function (a) { return a.Recon; });
                    $(this).attr("sortDirection", "up");
                }
                break;
        }
        $('#divOther').html($("#brandOtherTemplate").render(listInfo.StockingGuideBrandOtherData) + '</div>');
    });

    $("div[id^='SDTableMarket'] > div > div.v3BrandOtherSD_collumn").live("click", function () {
        console.log(listInfo);

        console.log("clicked");

        var sortedField = $(this).attr("value");
        var sortedDirection = $(this).attr("sortDirection");
        var groupId = parseInt($(this).attr("dealerSegmentId"));
        console.log(groupId);
        var itemId = parseInt($(this).attr("detailId"));
        console.log("item Id" + itemId);
        var group = _(listInfo.StockingGuideBrandOtherData).find(
            function (x) {
                return x.SGDealerSegmentId == groupId;
            }
        );

        switch (sortedField) {
            case "marketMake":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Make.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Make.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketModel":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Model.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Model.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketYourStock":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.YourStock; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.YourStock; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketMarketStock":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.MarketStock; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.MarketStock; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketHistory":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.MarketHistory; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.MarketHistory; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketSupply":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Supply; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Supply; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketAge":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Age; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.Age; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketTurn":
                if (sortedDirection == 'up') {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.TurnOver; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGMarketDealerSegmentDetails = _(group.SGMarketDealerSegmentDetails).sortBy(function (a) { return a.TurnOver; });
                    $(this).attr("sortDirection", "up");
                }
                break;
        }

        //console.log("sort field name:" + quote + " " + );
        console.log(group);
        $("#SDRowsMarket_" + groupId).html($("#brandOtherDetailTemplate").render(group.SGMarketDealerSegmentDetails));

    });
    
    $("div[id^='SDTableInventory'] > div > div.v3BrandOtherSD_collumn").live("click", function () {
        console.log(listInfo);

        console.log("clicked");

        var sortedField = $(this).attr("value");
        var sortedDirection = $(this).attr("sortDirection");
        var groupId = parseInt($(this).attr("dealerSegmentId"));
        console.log(groupId);
        var itemId = parseInt($(this).attr("detailId"));
        console.log("item Id" + itemId);
        var group = _(listInfo.StockingGuideBrandOtherData).find(
            function (x) {
                return x.SGDealerSegmentId == groupId;
            }
        );

        switch (sortedField) {
            case "inventoryMake":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Make.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Make.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventoryModel":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Model.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Model.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventoryInStock":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.InStock; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.InStock; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventoryOU":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.OU; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.OU; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventoryGuide":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Guide; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Guide; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventoryHistory":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.History; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.History; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventoryRecon":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Recon; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Recon; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventoryTurnOver":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.TurnOver; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.TurnOver; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "inventorySupply":
                if (sortedDirection == 'up') {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Supply; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.SGInventoryDealerSegmentDetails = _(group.SGInventoryDealerSegmentDetails).sortBy(function (a) { return a.Supply; });
                    $(this).attr("sortDirection", "up");
                }
                break;
        }

        //console.log("sort field name:" + quote + " " + );
        console.log(group);
        $("#SDRowsInventory_" + groupId).html($("#brandOtherInventoryDetailTemplate").render(group.SGInventoryDealerSegmentDetails));

    });

    $("input[id^='txtGuide']").live("keydown", function (event) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(event.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    $("input[id^='txtSubGuide']").live("keydown", function (event) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(event.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    $("input[id^='txtGrossPerUnit']").live("keydown", function (event) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(event.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    $("input[id^='chkMarket']").live("click", function () {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var dataUrl = '/StockingGuide/UpdateWishListOtherMarket';
        var id = $(this).attr('id').replace('chkMarket_', '');
        var isWishList = $(this).is(':checked');
        $.ajax({
            type: "POST",
            //                contentType: "text/JSON; charset=utf-8",
            dataType: "JSON",
            url: dataUrl,
            data: { id: id, isWishList: isWishList },
            cache: false,
            traditional: true,
            success: function (result) {
                $.unblockUI();
            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
                $.unblockUI();
            }
        });
    });

    $("input[id^='chkInventory']").live("click", function () {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var dataUrl = '/StockingGuide/UpdateWishListOtherInventory';
        var id = $(this).attr('id').replace('chkInventory_', '');
        var isWishList = $(this).is(':checked');
        $.ajax({
            type: "POST",
            //                contentType: "text/JSON; charset=utf-8",
            dataType: "JSON",
            url: dataUrl,
            data: { id: id, isWishList: isWishList },
            cache: false,
            traditional: true,
            success: function (result) {
                $.unblockUI();
            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
                $.unblockUI();
            }
        });
    });

    $("input[id^='chkWishList']").live("click", function () {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var dataUrl = '/StockingGuide/UpdateWishListOther';
        var id = $(this).attr('id').replace('chkWishList_', '');
        var isWishList = $(this).is(':checked');
        $.ajax({
            type: "POST",
            //                contentType: "text/JSON; charset=utf-8",
            dataType: "JSON",
            url: dataUrl,
            data: { id: id, isWishList: isWishList },
            cache: false,
            traditional: true,
            success: function (result) {
                $.unblockUI();
            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
                $.unblockUI();
            }
        });
    });

    $("input[id^='txtGrossPerUnit']").live("blur", function () {
        var dataUrl = '/StockingGuide/UpdateGrossPerUnitOther';
        var id = $(this).attr('id').replace('txtGrossPerUnit_', '');
        var grossPerUnit = $(this).val();

        var oldValue = $('#hdGrossPerUnit_' + id).val();

        var sum = 0;
        if (oldValue != $(this).val()) {
            $('#hdGrossPerUnit_' + id).val($(this).val());
            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            $.ajax({
                type: "POST",
                //                contentType: "text/JSON; charset=utf-8",
                dataType: "JSON",
                url: dataUrl,
                data: { id: id, grossPerUnit: grossPerUnit },
                cache: false,
                traditional: true,
                success: function (result) {
                    $(".txtGrossPerUnit").each(function () {
                        sum += parseInt($(this).val());
                    });
                    $('#divGrossPerUnitHeader').html('$' + sum);
                    
                    var group = _(listInfo.StockingGuideBrandOtherData).find(
                        function (x) {
                            return x.SGDealerSegmentId == id;
                        }
                    );
                    group.GrossPerUnit = parseInt(grossPerUnit);
                    $.unblockUI();
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                    $.unblockUI();
                }
            });
        }
    });

    $("input[id^='txtSubGuide']").live("blur", function () {
        var dataUrl = '/StockingGuide/UpdateSubGuideOther';
        var id = $(this).attr('id').replace('txtSubGuide_', '');
        var guide = $(this).val();
        var stock = $('#divSubStock_' + id).html();
        var balance = parseInt(stock) - parseInt(guide);
        var parentID = $(this).parent().find('#hdParentID').val();
        var divBalance = 'divSubBalance_' + id;
        var divSubWishList = 'divSubWishList_' + id;
        var oldValue = $('#hdSubGuide_' + id).val();
        console.log(parentID);
        var divID = '#SDTableInventory_' + parentID;
        if (oldValue != $(this).val()) {
            $('#hdSubGuide_' + id).val($(this).val());
            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            $.ajax({
                type: "POST",
                //                contentType: "text/JSON; charset=utf-8",
                dataType: "JSON",
                url: dataUrl,
                data: { id: id, guide: guide },
                cache: false,
                traditional: true,
                success: function (result) {
                    $('#' + divBalance).removeClass('v3BalanceOver');
                    $('#' + divBalance).removeClass('v3BalanceUnder');
                    $('#' + divBalance).removeClass('v3BalanceEqual');
                    if (balance > 0) {
                        $('#' + divBalance).addClass('v3BalanceOver');
                        $('#' + divBalance).html('+' + balance);
                        $('#' + divSubWishList).html('PREPRICE');
                    }
                    else if (balance < 0) {
                        $('#' + divBalance).addClass('v3BalanceUnder');
                        $('#' + divBalance).html(balance);
                        $('#' + divSubWishList).html('BUY');
                    } else {
                        $('#' + divBalance).addClass('v3BalanceEqual');
                        $('#' + divBalance).html(balance);
                        $('#' + divSubWishList).html('.................');
                    }
                    var sumGuide = 0;
                    $(divID).find('input.txtSubGuide').each(function () {
                        console.log($(this).val());
                        sumGuide += parseInt($(this).val());
                    });
                    
                    var group = _(listInfo.StockingGuideBrandOtherData).find(
                       function (x) {
                           return x.SGDealerSegmentId == parentID;
                       }
                   );
                    var row = _(group.SGInventoryDealerSegmentDetails).find(
                       function (x) {
                           return x.SGInventoryDealerSegmentDetailId == id;
                       }
                   );
                    
                    row.Guide = guide;
                    $('#txtGuide_' + parentID).val(sumGuide);
                    $('#txtGuide_' + parentID).trigger('change');
                    $.unblockUI();
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                    $.unblockUI();
                }
            });
        }
    });

    $("input[id^='txtGuide']").live("change", function () {
        var dataUrl = '/StockingGuide/UpdateGuideOther';
        var id = $(this).attr('id').replace('txtGuide_', '');
        var newValue = $(this).val();
        var stock = $('#divStock_' + id).html();
        var balance = parseInt(stock) - parseInt(newValue);
        var balancePercent = 100;
        if (Math.abs(balance) <= 10) {
            balancePercent = Math.round((parseFloat(Math.abs(balance)) / 10) * 100);
        }
        var html = '';
        var divBalance = 'divBalance_' + id;

        var oldValue = $('#hdGuide_' + id).val();
        if (oldValue != newValue) {
            var different = parseInt(newValue) - parseInt(oldValue);
            $('#hdGuide_' + id).val(newValue);
            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            $.ajax({
                type: "POST",
                //                contentType: "text/JSON; charset=utf-8",
                dataType: "JSON",
                url: dataUrl,
                data: { id: id, guide: newValue },
                cache: false,
                traditional: true,
                success: function (result) {
                    if (balance > 0) {
                        $('#divWishListText_' + id).html('<label style="color:blue">BUY</label>');
                        html += '<div class="v3BrandOther_barTotal_holder v3BalanceOver">';
                        html += '+' + balance;
                        html += '</div>';
                        html += '<div class="v3BrandOther_barColor_holder">';
                        html += '<div class="v3BrandOther_barColor_right"></div>';
                        html += '<div class="v3BrandOther_barColor_left">';
                        html += '<div class="v3BrandOther_barLeft_value v3BalanceOver" style="width: ' + balancePercent + '%"></div>';
                        html += '</div>';
                        html += '</div>';
                    } else if (balance < 0) {
                        $('#divWishListText_' + id).html('OVERSTOCK');
                        html += '<div class="v3BrandOther_barTotal_holder v3BalanceUnder">';
                        html += balance;
                        html += '</div>';
                        html += '<div class="v3BrandOther_barColor_holder">';
                        html += '<div class="v3BrandOther_barColor_right">';
                        html += '<div class="v3BrandOther_barRight_value v3BalanceUnder" style="width: ' + balancePercent + '%"></div>';
                        html += '</div>';
                        html += '<div class="v3BrandOther_barColor_left"></div>';
                        html += '</div>';
                    } else {
                        $('#divWishListText_' + id).html('.................');
                        html += '<div class="v3BrandOther_barTotal_holder v3BalanceEqual">';
                        html += balance;
                        html += '</div>';
                        html += '<div class="v3BrandOther_barColor_holder v3BalanceEqualBar">';
                        html += '<div class="v3BrandOther_barColor_right"></div>';
                        html += '<div class="v3BrandOther_barColor_left">';
                        html += '</div>';
                        html += '</div>';
                    }
                    $('#' + divBalance).html(html);
                    var currentValue = $('#divGuideOtherHeader').html();
                    console.log(currentValue);
                    console.log(different);
                    $('#divGuideOtherHeader').html(parseInt(currentValue) + parseInt(different));
                    
                    var group = _(listInfo.StockingGuideBrandOtherData).find(
                        function (x) {
                            return x.SGDealerSegmentId == id;
                        }
                    );
                    group.Guide = parseInt(newValue);
                    $.unblockUI();
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                    $.unblockUI();
                }
            });
        }
    });

    $(".v3BrandOther_btn_item").live("click", function () {
        $(".v3BrandOther_btn_item").removeClass("v3BrandOther_btn_itemActive");
        $(this).addClass("v3BrandOther_btn_itemActive");
    });

    $("div[id^='v3BrandOtherInventory']").live("click", function () {
        $('#' + $(this).parent().parent().find("div[id^='SDTableInventory']")[0].id).show();
        $('#' + $(this).parent().parent().find("div[id^='SDTableMarket']")[0].id).hide();
    });

    $("div[id^='v3BrandOtherMarket']").live("click", function () {
        $('#' + $(this).parent().parent().find("div[id^='SDTableInventory']")[0].id).hide();
        $('#' + $(this).parent().parent().find("div[id^='SDTableMarket']")[0].id).show();
    });

    $("div.v3BrandOther_row:not(.v3BrandOther_rowZero) > div.collumFirst").live("click", function (e) {
        $(".v3BrandOther_Segments_Detail").each(function () {
            $(this).hide();
        });
        $(this).parent().find(".v3BrandOther_Segments_Detail").show();
        $(this).parent().find("div[id^='v3BrandOtherInventory']").trigger("click");
    });

    $("div[id^='v3BrandDetailClose']").live("click", function () {
        $(".v3BrandOther_Segments_Detail").each(function () {
            $(this).hide();
        });
    });
});