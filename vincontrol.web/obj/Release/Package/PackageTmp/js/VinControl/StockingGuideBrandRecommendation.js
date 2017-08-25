var listInfo = {};
var SGMarketList = [];
function LoadData() {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var dataUrl = '/StockingGuide/StockingGuideRecommendationBrandJson';
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: dataUrl,
        data: {},
        cache: false,
        traditional: true,
        success: function (result) {
            listInfo = result;
            $('#header').html($("#brandHeaderOtherTemplate").render());
            //$('#body').html($("#brandOtherTemplate").render(result.StockingGuideBrandOtherData));

            var makes = [];
            var models = [];
            $.each(result.StockingGuideBrandOtherData, function (index, brandOtherData) {
                $.each(brandOtherData.SGInventoryDealerSegmentDetails, function (index01, inventorySegment) {
                    if ($.inArray(inventorySegment.Make, makes) == -1) {
                        makes.push(inventorySegment.Make);
                    }

                    if ($.inArray(inventorySegment.Make + "_" + inventorySegment.Model, models) == -1) {
                        models.push(inventorySegment.Make + "_" + inventorySegment.Model);
                    }
                });

                $.each(brandOtherData.SGMarketDealerSegmentDetails, function (index02, marketSegment) {
                    SGMarketList.push(marketSegment);
                    if ($.inArray(marketSegment.Make, makes) == -1) {
                        makes.push(marketSegment.Make);
                    }

                    if ($.inArray(marketSegment.Make + "_" + marketSegment.Model, models) == -1) {
                        models.push(marketSegment.Make + "_" + marketSegment.Model);
                    }
                });

            });
            $('#body').html($("#brandOtherTemplate").render(SGMarketList));

            $.each(makes, function (key, value) {
                $('#ddlMake').append($("<option/>", {
                    value: value,
                    text: value
                }));
            });
            $.each(models, function (key, value) {
                $('#ddlModel').append($("<option/>", {
                    value: value,
                    text: value.split('_')[1]
                }));
            });

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

function filterSGMarketList(list, make, model) {
    return _.filter(list, function (element) {
        return (make == '' || element.Make == make) && (model == '' || element.Model == model);

    });
}

$(document).ready(function () {
    console.log("before Load Data");
    LoadData();
    
    $("div.v3BrandOtherSD_collumn").live("click", function () {
        console.log(listInfo);
        console.log("clicked");

        var sortedField = $(this).attr("value");
        var sortedDirection = $(this).attr("sortDirection");
        var make = $('#ddlMake').val();
        var model = $('#ddlModel').val();
        var tmpSGMarketList = SGMarketList;

        switch (sortedField) {
            case "marketMake":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Make.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Make.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketModel":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Model.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Model.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketYourStock":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.YourStock; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.YourStock; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketMarketStock":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.MarketStock; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.MarketStock; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketHistory":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.MarketHistory; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.MarketHistory; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketSupply":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Supply; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Supply; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketAge":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Age; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.Age; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "marketTurn":
                if (sortedDirection == 'up') {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.TurnOver; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    tmpSGMarketList = _(tmpSGMarketList).sortBy(function (a) { return a.TurnOver; });
                    $(this).attr("sortDirection", "up");
                }
                break;
        }
        
        $("#body").html($("#brandOtherTemplate").render(filterSGMarketList(tmpSGMarketList, make, model)));
        //$("#body").html($("#brandOtherTemplate").render(tmpSGMarketList));

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

    $('#ddlMake').live('change', function() {
        //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedMake = $(this).val();
        var selectedModel = selectedMake == '' || $('#ddlModel').val() == '' ? '' : $('#ddlModel').val().split('_')[1];

        $("#body").html($("#brandOtherTemplate").render(SGMarketList));
        if (selectedMake == '') {
            $("#ddlModel option").each(function() {
                if ($(this).val() == '') {
                    $(this).attr('selected', true);
                }
                $(this).show();
            });
        } else
            $("#ddlModel option").each(function() {
                if ($(this).val() == '') {
                    $(this).attr('selected', true);
                } else if ($(this).val().split('_')[0] == selectedMake) $(this).show();
                else $(this).hide();
            });

        $(".v3BrandOther_SD_row").not(":first").each(function () {
            if (selectedMake == '') $(this).parent().show();
            else {
                if ($.trim($(this).children()[0].innerText) == selectedMake)
                    $(this).parent().show();
                else $(this).parent().hide();
            }
        });
    });

    $('#ddlModel').live('change', function() {
        var selectedModel = $(this).val() == '' ? '' : $(this).val().split('_')[1];
        var selectedMake = $(this).val() == '' ? '' : $(this).val().split('_')[0];
        $("#body").html($("#brandOtherTemplate").render(SGMarketList));

        $("#ddlMake option").each(function() {
            if ($(this).val() == selectedMake) $(this).attr('selected', true);
        });

        $(".v3BrandOther_SD_row").not(":first").each(function () {
            if (selectedMake == '') $(this).parent().show();
            else {
                if ($.trim($(this).children()[0].innerText) == selectedMake && $.trim($(this).children()[1].innerText) == selectedModel)
                    $(this).parent().show();
                else $(this).parent().hide();
            }
        });
    });
});