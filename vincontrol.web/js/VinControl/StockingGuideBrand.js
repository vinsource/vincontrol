var dataUrl = '/StockingGuide/StockingGuideBrandJson';
var dataUrlFilter = '/StockingGuide/StockingGuideBrandJsonByIDs';
var saveBrandSelectionUrl = '/StockingGuide/SaveBrandSelection';
var oldBrand;

var listInfo = {};

function LoadData() {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
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
            console.log('**************');
            console.log(listInfo);
            console.log('**************');
            $.each(result.ListInfo, function (index, obj) {
                $('#DDLFilterModel').html($("#filterModelsTemplate").render(result.ListInfo));
                var myArray = result.BrandSelection.split(',');
                $("#DDLFilterModel").multipleSelect({
                    multiple: true,
                    multipleWidth: 200,
                    placeholder: "Click here to filter your brand",
                    onOpen: function () {
                        oldBrand = $('.ms-choice >span').html();
                    },
                    onClose: function () {
                        if ($('.ms-choice >span').html() != oldBrand) {
                            LoadBrandFilter();
                        }
                    }
                });
                console.log(myArray);
                $("#DDLFilterModel").multipleSelect("setSelects", myArray);
                $('.ms-parent li.multiple').addClass("subOpt");
            });
            LoadBrandFilter();
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

function LoadBrandFilter() {
    //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var count = 0;
    $.ajax({
        type: "POST",
        //                contentType: "text/JSON; charset=utf-8",
        dataType: "JSON",
        url: dataUrlFilter,
        data: { ids: $('#DDLFilterModel').multipleSelect('getSelects').toString() },
        cache: false,
        traditional: true,
        success: function (resultSub) {
            listInfo = resultSub;
            $.each(resultSub.ListInfo, function (subIndex, subObj) {
                if (subIndex == 0) {
                    $('#divBrand').html('<div class="v3BrandOther_list">' + $("#brandHeaderTemplate").render(subObj.HeaderInfo) + '<div id="div' + subObj.Make + '">' + $("#brandTemplate").render(subObj.StockingGuideBrandData) + '</div>' + '</div>');
                } else {
                    $('#divBrand').append('<div class="v3BrandOther_list">' + $("#brandHeaderTemplate").render(subObj.HeaderInfo) + '<div id="div' + subObj.Make + '">' + $("#brandTemplate").render(subObj.StockingGuideBrandData) + '</div>' + '</div>');
                }
                console.log(subObj.StockingGuideBrandData);
                count += subObj.StockingGuideBrandData.length;
            });
            $('#inventory_used_tab_number').html(count);
            $("a.iframe").fancybox({ 'margin': 0, 'padding': 0, 'width': 250, 'height': 142 });
            $.unblockUI();
        }
    });
}

$(document).ready(function () {

    LoadData();

    $("div.v3BrandOther_rowTitle > div.v3BrandOther_collumn").live("click", function () {
        var sortedField = $(this).attr("value");
        var sortedDirection = $(this).attr("sortDirection");
        var make = $(this).parent().attr("make");
        
        var group = _(listInfo.ListInfo).find(
            function (x) {
                return x.Make == make;
            }
        );
        
        switch (sortedField) {
            case "HeaderModel":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Model.toUpperCase(); }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Model.toUpperCase(); });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderHistory":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.History; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.History; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderStock":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Stock; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Stock; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderGuide":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Guide; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Guide; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderBalance":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Balance; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Balance; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderSupply":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Supply; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Supply; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderAge":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Age; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Age; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderTurn":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Turn; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Turn; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderGrossPerUnit":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.GrossPerUnit; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.GrossPerUnit; });
                    $(this).attr("sortDirection", "up");
                }
                break;
            case "HeaderRecon":
                if (sortedDirection == 'up') {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Recon; }).reverse();
                    $(this).attr("sortDirection", "down");
                } else {
                    group.StockingGuideBrandData = _(group.StockingGuideBrandData).sortBy(function (a) { return a.Recon; });
                    $(this).attr("sortDirection", "up");
                }
                break;
        }
        $('#div' + make).html($("#brandTemplate").render(group.StockingGuideBrandData));
    });
    
    $('#btnSaveBrand').live("click", function () {
        $.ajax({
            type: "POST",
            //                contentType: "text/JSON; charset=utf-8",
            dataType: "JSON",
            url: saveBrandSelectionUrl,
            data: { brandSelection: $('#DDLFilterModel').multipleSelect('getSelects').toString() },
            cache: false,
            traditional: true,
            success: function (result) {
                alert('Saved successful');
                $.unblockUI();
            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
                $.unblockUI();
            }
        });
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
    
    $("input[id^='chkWishList']").live("click", function () {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var dataUrl = '/StockingGuide/UpdateWishList';
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
        var dataUrl = '/StockingGuide/UpdateGrossPerUnit';
        var id = $(this).attr('id').replace('txtGrossPerUnit_', '');
        var make = $(this).attr('make');
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
                    $(".txtGrossPerUnit_" + make).each(function () {
                        sum += parseInt($(this).val());
                    });
                    $('#divGrossPerUnitHeader_' + make).html('$' + sum);
                    //$('#divGrossPerUnitHeader').html('$' + sum);
                    
                    var group = _(listInfo.ListInfo).find(
                        function (x) {
                            return x.Make == make;
                        }
                    );
                    console.log('////////////////');
                    console.log(group);
                    console.log('////////////////');
                    var obj = _(group.StockingGuideBrandData).find(
                        function (x) {
                            return x.SGDealerBrandId == id;
                        }
                    );
                    
                    obj.GrossPerUnit = parseInt(grossPerUnit);
                    $.unblockUI();
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                    $.unblockUI();
                }
            });
        }
    });
    
    $("input[id^='txtGuide']").live("blur", function () {
        var dataUrl = '/StockingGuide/UpdateGuide';
        var id = $(this).attr('id').replace('txtGuide_', '');
        var guide = $(this).val();
        var stock = $('#divStock_' + id).html();
        var balance = parseInt(stock) - parseInt(guide);
        var balancePercent = 100;
        if (Math.abs(balance)<=10) {
            balancePercent = Math.round((parseFloat(Math.abs(balance)) / 10) * 100);
        }
        var html = '';
        var divBalance = 'divBalance_' + id;

        var oldValue = $('#hdGuide_' + id).val();
        if (oldValue != $(this).val()) {
            $('#hdGuide_' + id).val($(this).val());
            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            $.ajax({
                type: "POST",
                //                contentType: "text/JSON; charset=utf-8",
                dataType: "JSON",
                url: dataUrl,
                data: { id: id, guide: guide },
                cache: false,
                traditional: true,
                success: function(result) {
                    if (balance > 0) {
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
                    $.ajax({
                        type: "POST",
                        //                contentType: "text/JSON; charset=utf-8",
                        dataType: "JSON",
                        url: dataUrlFilter,
                        data: { ids: $('#DDLFilterModel').multipleSelect('getSelects').toString() },
                        cache: false,
                        traditional: true,
                        success: function (resultSub) {
                            listInfo = resultSub;
                            $.each(resultSub.ListInfo, function (subIndex, subObj) {
                                if (subIndex == 0) {
                                    $('#divBrand').html('<div class="v3BrandOther_list">' + $("#brandHeaderTemplate").render(subObj.HeaderInfo) + '<div id="div' + subObj.Make + '">' + $("#brandTemplate").render(subObj.StockingGuideBrandData) + '</div>' + '</div>');
                                } else {
                                    $('#divBrand').append('<div class="v3BrandOther_list">' + $("#brandHeaderTemplate").render(subObj.HeaderInfo) + '<div id="div' + subObj.Make + '">' + $("#brandTemplate").render(subObj.StockingGuideBrandData) + '</div>' + '</div>');
                                }
                            });
                            $.unblockUI();
                        }
                    });
                },
                error: function(err) {
                    console.log(err.status + " - " + err.statusText);
                    $.unblockUI();
                }
            });
        }
    });
});