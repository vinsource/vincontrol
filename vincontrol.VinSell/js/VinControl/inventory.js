/// <reference path="../../Views/Inventory/ViewSmallInventory.aspx" />
var grid;
var inventoryObj = [];
var totalInventoryObj = 0;

var vm = {
    formatCarfax: function (carFaxOwner) {
        var value = parseInt(carFaxOwner);
        if (value < 1)
            return "NA";
        else if (value == 1)
            return value + " Owner";
        else
            return value + " Owners";
    },
    formatDaysInInventory: function (daysInInvenotry) {
        var value = parseInt(daysInInvenotry);
        if (value == 1)
            return value + " Day";
        else {
            return value + " Days";
        }
    },
    formatStock: function (stock) {
        return "#" + stock;
    },
    formatPrice: function (salePrice) {
        return formatDollar(parseInt(salePrice));
    },
    formatMarket: function (ranking, numberOfCar) {
        
        if (ranking == 0 && numberOfCar == 0)
            return 'NA';
        else {
            return ranking + '/' + numberOfCar;
        }
    },
    getImageUrl: function (marketRange) {
        if (marketRange == 3)
            return "/Content/images/market_higher.gif";
        else if (marketRange == 2)
            return "/Content/images/market_equal.gif";
        else if (marketRange == 1)
            return "/Content/images/market_lower.gif";
        else
            return "/Content/images/market_question.gif";
    },
    getColorStyle: function (marketRange) {
       
        if (marketRange == 3)
            return "orange_color";
        else if (marketRange == 2)
            return "green_color";
        else if (marketRange == 1)
            return "blue_color";
        else
            return "black_color";

    },
    getsubString: function (value, start, end) {
        //            alert(value.length);
        if (value != '' && value != null && value.length + 1 > end && start < end) {
            //                alert(value.substring(0, value.length));
            return value.substring(start, end);
        } else
            return value;
    },
    getVin: function (value) {
        if (value != '' && value != null && value.length >= 8) {

            return value.substring(value.length - 8, value.length);
        } else
            return value;
    }
    , getEbayUrl: function (value) {
        return "/Market/ViewEbay?ListingId=" + value;
    }, getWSUrl: function (value) {
        return "/PDF/PrintSticker?ListingId=" + value;
    }, getBGUrl: function (value) {
        return "/Report/ViewBuyerGuide?ListingId=" + value;
    }, getPhoto: function (value) {
        if (value == null || value == '') {
            return '/Content/Images/noImageAvailable.jpg';
        } else {
            return value;
        }
    }
};

function updateSortIcon(sortFieldName, isUp) {
    switch (sortFieldName) {
        case "market":
            $('#imgSortMarket').css('display', 'inline');

            if (isUp) {
                $('#imgSortMarket').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortMarket').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "year":
            $('#imgSortYear').css('display', 'inline');

            if (isUp) {
                $('#imgSortYear').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortYear').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "miles":
            $('#imgSortMiles').css('display', 'inline');

            if (isUp) {
                $('#imgSortMiles').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortMiles').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "price":
            $('#imgSortPrice').css('display', 'inline');

            if (isUp) {
                $('#imgSortPrice').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortPrice').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "make":
            $('#imgSortMake').css('display', 'inline');

            if (isUp) {
                $('#imgSortMake').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortMake').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "model":
            $('#imgSortModel').css('display', 'inline');

            if (isUp) {
                $('#imgSortModel').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortModel').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "trim":
            $('#imgSortTrim').css('display', 'inline');

            if (isUp) {
                $('#imgSortTrim').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortTrim').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "color":
            $('#imgSortColor').css('display', 'inline');

            if (isUp) {
                $('#imgSortColor').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortColor').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "age":
            $('#imgSortAge').css('display', 'inline');

            if (isUp) {
                $('#imgSortAge').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortAge').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "stock":
            $('#imgSortStock').css('display', 'inline');

            if (isUp) {
                $('#imgSortStock').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortStock').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "vin":
            $('#imgSortVin').css('display', 'inline');

            if (isUp) {
                $('#imgSortVin').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortVin').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
        case "owners":
            $('#imgSortOwners').css('display', 'inline');

            if (isUp) {
                $('#imgSortOwners').attr('src', '../Content/images/vincontrol/up.png');
            } else {
                $('#imgSortOwners').attr('src', '../Content/images/vincontrol/down.png');
            }

            break;
    }
}

function InventoryGrid(currentViewInfo) {
    var viewInfo = currentViewInfo,
        bindSorting = function () {
            $(".right_content_nav_items").click(function () {
                var quote = $(this).attr("value");

                if (viewInfo.sortFieldName != quote) { // reset
                    if (quote == "year") {
                        viewInfo.isUp = false;
                    }
                    else {
                        viewInfo.isUp = true;
                    }
                }
                else { // toggle
                    viewInfo.isUp = !viewInfo.isUp;
                }

                viewInfo.sortFieldName = quote;
                postCurrentViewInfo(viewInfo);
                $('img.imgSort').each(function () {
                    $(this).attr('src', '../Content/images/vincontrol/dot.png');
                    $(this).css('display', 'none');
                });

                updateSortIcon(viewInfo.sortFieldName, viewInfo.isUp);
                loadGrid();
            });
        },
        changeState = function () {
            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            toggleState();
            renderHeader();
            bindSorting();
            updateSortIcon(viewInfo.sortFieldName, viewInfo.isUp);
            loadGrid();
            $.unblockUI();
        },
        toggleState = function () {
            if (viewInfo.currentState == expandMode) {
                viewInfo.currentState = normalMode;
            } else {
                viewInfo.currentState = expandMode;
            }
        },
        appendSortedData = function (data) {
            if (viewInfo.currentState == expandMode) {
                $(".vin_listVehicle_holder").html($("#fullInventoryTemplate").render(data));
            } else {
                $(".vin_listVehicle_holder").html($("#inventoryTemplate").render(data));
            }

            $("a.iframe").fancybox({ 'width': 1010, 'height': 715 });
            validateForm();
        },
        loadGrid = function () {
            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            $('#divNoContent').hide();
            
            showSubMenu();
            switch (viewInfo.currentView) {
                case usedScreen:
                    showSubMenu();
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case auctionScreen:
                    hideSubMenu();
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case newScreen:
                    hideSubMenu();
                    hideNewUsedSubMenu();
                    showBrochureMenu();
                    break;
                case loanerScreen:
                    hideSubMenu();
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case reconScreen:
                    hideSubMenu();
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case wholesaleScreen:
                    hideSubMenu();
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case tradeNotClearScreen:
                    hideSubMenu();
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case soldScreen:
                    hideSubMenu();
                    showNewSubMenu();
                    hideBrochureMenu();
                    break;
                case todayBucketJumpScreen:
                    viewInfo.currentState = expandMode;
                    renderHeader();
                    bindSorting();
                    updateSortIcon(viewInfo.sortFieldName, viewInfo.isUp);
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case expressBucketJumpScreen:
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case aCarScreen:
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case missingContentScreen:
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case noContentScreen:
                    hideNewUsedSubMenu();
                    hideBrochureMenu();
                    break;
                case newSoldScreen:
                    hideSubMenu();
                    hideBrochureMenu();
                    break;
                case usedSoldScreen:
                    hideSubMenu();
                    hideBrochureMenu();
                    break;
            }

            switch (viewInfo.currentView) {
                case noContentScreen:
                    $('#divTitle').html('No Content');
                    break;
                case missingContentScreen:
                    $('#divTitle').html('Missing Content');
                    break;
                case aCarScreen:
                    $('#divTitle').html('A Cars');
                    break;
                case todayBucketJumpScreen:
                    $('#divTitle').html('Today Bucket Jump');
                    break;
                default:
                    $('#divTitle').html('Inventory List');
                    break;
            }

            $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
            resetScrollValues();

            var fromDate = "";
            var toDate = "";

            if (viewInfo.currentTab == soldTab) {
                if ($("#sold_car_date_From").val() == '') $("#sold_car_date_From").val(previousDate(true, 90));
                fromDate = $("#sold_car_date_From").val();
                if ($("#sold_car_date_To").val() == '') $("#sold_car_date_To").val(todayDate(true));
                toDate = $("#sold_car_date_To").val();
            }

            var dataUrl = '/Inventory/ViewInventoryJson';
            $.ajax({
                type: "POST",
                //                contentType: "text/JSON; charset=utf-8",
                dataType: "JSON",
                url: dataUrl,
                data: {
                    screen: viewInfo.currentView,
                    sortBy: viewInfo.sortFieldName,
                    isUp: viewInfo.isUp,
                    pageSize: pageSize,
                    fromDate: fromDate,
                    toDate: toDate,
                },
                cache: false,
                traditional: true,
                success: function (result) {
                    totalInventoryObj = result.count;

                    if (result.count == 0) {
                        $('#divHasContent').hide();

                        $('#divNoContent').text('There are no vehicles in this category.');
                        $('#divNoContent').show();
                        
                    } else {
                        $('#divHasContent').show();
                        $('#divNoContent').hide();
                        initializeFilter(result.list);
                    }
                    inventoryObj = result.list;
                    updateGridFromData(inventoryObj);
                    
                    $(".SalePriceForm").validationEngine({ promptPosition: "centerRight", scroll: false });
                    $.unblockUI();
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                }
            });
        },
        setActive = function (item) {
            $(".admin_top_btns_active").removeClass("admin_top_btns_active");
            $(".number_belowActive").removeClass("number_belowActive");
            $(item.parentNode).addClass("admin_top_btns_active");
            $(item.parentNode).children('div').first().addClass('number_belowActive');
        },
        setChildActive = function (item) {
            $(".admin_top_btns_active").removeClass("admin_top_btns_active");
            $(".number_belowActive").removeClass("number_belowActive");
            $(item).addClass("admin_top_btns_active");
            $(item).children('div').first().addClass('number_belowActive');
        },
        hideSubMenu = function () {
            $('#TodayBucketJump').hide();
            $('#expressBucketJump').hide();
            $('#NoContentDiv').hide();
            $('#MissingContentDiv').hide();
            $('#ACarDiv').hide();
            $('#filterPanel').hide();
        },
        showSubMenu = function () {
            $('#TodayBucketJump').show();
            $('#expressBucketJump').show();
            $('#NoContentDiv').show();
            $('#MissingContentDiv').show();
            $('#ACarDiv').show();

            if (bIsShowingFilterPanel) {
                $('#filterPanel').show();
            }
        },
        hideNewUsedSubMenu = function () {
            $('#divSoldCarBtns').hide();
        },
        showNewSubMenu = function () {
            $('#divSoldCarBtns').show();
        },
        hideBrochureMenu = function () {
            hideBrochureContent();
            $('#brochureMenuDiv').hide();
            $('#brochureMenuDivCancel').hide();
            
            $('#DivContent').show();
            $('#right_content_nav').show();
        },
        showBrochureMenu = function () {
            hideBrochureContent();
            resetBrochure();
            $('#brochureMenuDiv').show();
            $('#brochureMenuDivCancel').hide();
            
            $('#DivContent').show();
            $('#right_content_nav').show();
        },
        showBrochureContent = function () {
            $('#titleBrochure').text('');
            $('#brochureMenuDiv').hide();
            $('#brochureMenuDivCancel').show();
            $('#divTitle').text('New Vehicle Brochure');
            
            $('#DivContent').hide();
            $('#right_content_nav').hide();
            $('#filterPanel').hide();
            $('#brochureContent').show();
        },
        hideBrochureContent = function () {
            resetAll();
            $('#titleBrochure').text('');
            $('#brochureMenuDiv').show();
            $('#brochureMenuDivCancel').hide();
            $('#divTitle').text('Inventory List');
            
            $('#DivContent').show();
            $('#right_content_nav').show();
            
            if (bIsShowingFilterPanel) {
                $('#filterPanel').show();
            }

            $('#brochureContent').hide();
        },
        changeView =
            function (screenType) {
                viewInfo.currentView = screenType;
                var currentTab = getTab(viewInfo.currentView);

                if (viewInfo.currentTab != currentTab) {
                    $("#sold_car_date_To").val(todayDate(true));
                    $("#sold_car_date_From").val(previousDate(true, 90));
                }

                viewInfo.currentTab = getTab(viewInfo.currentView);

                loadGrid();

                $("#cbSoldNewUsed").val("SoldInventory");
            },
        updateGridFromData = function (inventoryObj) {
            jQuery.each(inventoryObj, function () {
                getDetailUrl(this);
            });

            $('div.number_below').each(function () {
                $(this).removeClass('number_belowActive');
                if ($(this).attr('id') == viewInfo.currentTab + '_number') {
                    $(this).addClass('number_belowActive');
                    $(this).text(totalInventoryObj);
                }
            });

            if (inventoryObj.length == 0) {
                $('#right_content_nav').hide();
            } else {
                $('#right_content_nav').show();
            }

            appendSortedData(inventoryObj);
        },
        getDetailUrl = function (car) {

            if (car.Type == 2) {
                car.ClassFilter = "Wholesale";
                car.URLDetail = "/Inventory/ViewIProfile?ListingID=" + car.ListingId;
                car.URLEdit = "/Inventory/ViewIProfile?ListingID=" + car.ListingId;
            } else if (car.Type == 4) {
                car.ClassFilter = "Sold";
                car.URLDetail = "/Inventory/ViewISoldProfile?ListingID=" + car.ListingId;

                car.URLEdit = "/Inventory/ViewISoldProfile?ListingID=" + car.ListingId;

            } else if (car.Type == 3) {
                car.ClassFilter = "Appraisals";
                car.URLDetail = "/Appraisal/EditAppraisal?appraisalId=" + car.ListingId;
                car.URLEdit = "/Appraisal/EditAppraisal?appraisalId=" + car.ListingId;

            } else {
                if (car.Type == 0) {
                    car.ClassFilter = "New";
                } else if (car.Type == 1) {
                    car.ClassFilter = "Used";
                }
                car.URLDetail = "/Inventory/ViewIProfile?ListingID=" + car.ListingId;
                car.URLEdit = "/Inventory/EditIProfile?ListingID=" + car.ListingId;
            }
        },
        validateForm = function () {
            $(".sForm").numeric({ decimal: false, negative: false }, function () {
                alert("Positive integers only");
                this.value = "";
                this.focus();
            });

            if ($("#IsEmployee").val() == 'True') {
                $("input[name='price']").attr('disabled', 'disabled');
            }
        },
        renderHeader = function () {
            if (viewInfo.currentState == expandMode) {
                $(".right_content_nav").html($("#fullInventoryHeaderTemplate").render());
            } else {
                $(".right_content_nav").html($("#inventoryHeaderTemplate").render());
            }
        },
        showDefaultTab = function () {
            $('div.admin_top_btns').each(function () {
                $(this).removeClass('admin_top_btns_active');
                if ($(this).attr('id') == viewInfo.currentTab) {
                    $(this).addClass('admin_top_btns_active');
                }
            });
        };
    return {
        changeState: changeState,
        bindSorting: bindSorting,
        renderHeader: renderHeader,
        loadGrid: loadGrid,
        showDefaultTab: showDefaultTab,
        changeView: changeView,
        hideBrochureContent: hideBrochureContent,
        showBrochureContent: showBrochureContent,
        setActive: setActive,
        setChildActive: setChildActive,
        updateGridFromData: updateGridFromData
    };
}


function postCurrentViewInfo() {
    $.post(updateViewInfoStatus, viewInfo, function (data) { });
}

function updateStatus(txtBox, inventoryStatus, message) {
    var r = confirm("Do you want to change status to " + message + " ?");
    if (r == true) {
        if (viewInfo.currentTab == soldTab) {
            if (inventoryStatus == 0) {
                $.post(updateStatusUpdateStatusForSoldTab, { ListingId: txtBox.id }, function (data) {
                    $(txtBox).closest('.contain_list').fadeOut(500);
                    var count = $('div.number_belowActive').html();
                    $('div.number_belowActive').html(parseInt(count) - 1);
                });
            } else {
                $.post(updateStatusFromSoldPageUrl, { currentStatus: 1, status: inventoryStatus, ListingId: txtBox.id }, function (data) {
                    $(txtBox).closest('.contain_list').fadeOut(500);
                    var count = $('div.number_belowActive').html();
                    $('div.number_belowActive').html(parseInt(count) - 1);
                });
            }
        } else {
            $.post(updateStatusFromInventoryPageUrl, { status: inventoryStatus, ListingId: txtBox.id }, function (data) {
                $(txtBox).closest('.contain_list').fadeOut(500);
                var count = $('div.number_belowActive').html();
                $('div.number_belowActive').html(parseInt(count) - 1);
            });
        }
    } else {
        $('.uncheck').each(function () {
            $(this).attr('checked', false);
        });

        $('.check').each(function () {
            $(this).attr('checked', true);
        });
    }
}

function doneTodayBucketJump(listingId, day) {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var urlString = "/Inventory/DoneTodayBucketJump?listingId=" + listingId + "&day=" + day;
    $.ajax({
        type: "GET",
        dataType: "html",
        url: urlString,
        data: {},
        cache: false,
        traditional: true,
        success: function (result) {
            $('#doneTodayBucketJump_' + listingId + '_' + day).closest('.contain_list').fadeOut(500);
            var count = $('div.number_belowActive').html();
            $('div.number_belowActive').html(parseInt(count) - 1);
            $.each(inventoryObj, function (index, obj) {
                if (obj != null) {
                    if (obj.ListingId == listingId) {
                        inventoryObj.splice(index, 1);
                        return;
                    }
                }
            });
            $.unblockUI();
        },
        error: function (err) {
            $.unblockUI();
        }
    });
}

function openMarkSoldIframe(txtBox) {
    var markSoldUrl = '/Inventory/ViewMarkSold?listingId=' + txtBox.id;

    $(".invent_expanded_sold").fancybox({
        'type': 'iframe',
        'width': 455,
        'height': 306,
        'autoDimensions': false,
        'autoScale': false,
        href: markSoldUrl,
    });
}

function removeItem(listingId) {
    $('#' + listingId).closest('.contain_list').fadeOut(500);
    var count = $('div.number_belowActive').html();
    $('div.number_belowActive').html(parseInt(count) - 1);
}

function showBrochureInfo() {
    $('#divCustomerInfo').show();
    $('#divSendBrochure').show();
}

function hideBrochureInfo() {
    $('#divCustomerInfo').hide();
    $('#divSendBrochure').hide();
    $('#titleBrochure').text('');
    $('#imgCar').attr('src', '/Content/images/vincontrol/car_default.png');
}

function resetBrochure() {
    resetAll();
    $('#divCustomerInfo').hide();
    $('#divSendBrochure').hide();
    $('#titleBrochure').text('');
}

function getTab(screentype) {
    switch (screentype) {
        case usedScreen:
            return usedTab;

        case auctionScreen:
            return auctionTab;

        case newScreen:
            return newTab;

        case loanerScreen:
            return loanerTab;

        case reconScreen:
            return reconTab;

        case wholesaleScreen:
            return wholesaleTab;

        case tradeNotClearScreen:
            return tradeNotClearTab;

        case soldScreen:
            return soldTab;

        case todayBucketJumpScreen:
            return usedTab;

        case expressBucketJumpScreen:
            return usedTab;

        case aCarScreen:
            return usedTab;

        case missingContentScreen:
            return usedTab;

        case noContentScreen:
            return usedTab;

        case newSoldScreen:
            return soldTab;

        case usedSoldScreen:
            return soldTab;
    }
}

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    return pattern.test(emailAddress);
};

function ValidateBrochure(index) {
    flag = true;
    if ($('#vinbrochure_customer_name').val() == '') {
        $('#divErrorName').show();
        flag = false;
    } else {
        $('#divErrorName').hide();
    }
    if ($('#vinbrochure_customer_email').val() == '') {
        $('#divErrorEmail').show();
        flag = false;
    } else {
        $('#divErrorEmail').hide();
        if (!isValidEmailAddress($('#vinbrochure_customer_email').val())) {
            $('#divErrorEmailValid').show();
            flag = false;
        } else {
            $('#divErrorEmailValid').hide();
        }
    }
    if (flag == true)
        SendBrochure(index);
}

var pageIndex = 1;
var pageSize = 50;
var scrollHeight = 400;

function resetScrollValues() {
    pageIndex = 1;
    pageSize = 50;
    scrollHeight = 400;
    dataAdded = true;
    $('.vin_listVehicle_holder').scrollTop(0);
}

$(document).ready(function () {
    $("#btnSend").click(function (event) {
        ValidateBrochure(0);
    });

    $("#btnSend1").click(function (event) {
        ValidateBrochure(1);
    });
    
    viewInfo.currentTab = getTab(viewInfo.currentView);

    grid = new InventoryGrid(viewInfo);
    grid.renderHeader();
    grid.bindSorting();
    updateSortIcon(viewInfo.sortFieldName, viewInfo.isUp);

    $("input[id^=IsFeatured_]").live('click', function () {
        var id = this.id.split('_')[1];
        var urlString = updateIsFeaturedUrl + "/" + id;
        $.ajax({
            type: "POST",
            contentType: "text/hmtl; charset=utf-8",
            dataType: "html",
            url: urlString,
            data: {},
            cache: false,
            traditional: true,
            success: function (result) {
                //alert(result);
            },
            error: function (err) {
                console.log(err.status + " - " + err.statusText);
            }
        });
    });

    var tempMiles = 0;
    $('input[id^=miles]').live("focus", function () {
        tempMiles = $(this).val();
    }).live("focusout", function () {
        if ($(this).val() == "") {
            $(this).val(0);
        }

        var tempCurrentMileValue = $(this).val();
        if (tempCurrentMileValue != tempMiles) {
            var id = this.id.split('_')[1];
            var miles = this.value;
            var updateMileageFromInventoryPageUrl = '/Inventory/UpdateMileageFromInventoryPage';
            if (parseInt(miles.replace(/,/g, "")) <= 2000000) {
                var img = $(this).parent().find('.imgLoading');
                img.show();

                $.post(updateMileageFromInventoryPageUrl, { Mileage: miles, ListingId: id }, function (data) {
                    img.hide();

                    $.each(inventoryObj, function(index, obj) {
                        if (obj.ListingId == id) {
                            obj.Mileage = miles.replace(/,/g, "");
                            return;
                        }
                    });
                });
            } else {
                jQuery(".SalePriceForm").validationEngine('attach', { promptPosition: "centerRight" });
            }
        }
    });

    var tempSaleprice = 0;
    $('input[id^=saleprice]').live("focus", function () {
        tempSaleprice = $(this).val();
    }).live("focusout", function () {
        if ($(this).val() == "") {
            $(this).val(0);
        }

        var tempCurrentSalepriceValue = $(this).val();
        if (tempCurrentSalepriceValue != tempSaleprice) {
            var id = this.id.split('_')[1];
            var saleprice = this.value;
            var vehicleStatusCodeId = $('#hdVehicleStatusCodeId').val();
            var updateSalePriceFromInventoryPageUrl = '/Inventory/UpdateSalePriceFromInventoryPage';
            if (parseInt(saleprice.replace(/,/g, "")) <= 100000000) {
                var img = $(this).parent().find('.imgLoading');
                img.show();

                $.post(updateSalePriceFromInventoryPageUrl, { SalePrice: saleprice, ListingId: id, vehicleStatusCodeId: vehicleStatusCodeId }, function (data) {
                    img.hide();

                    $.each(inventoryObj, function (index, obj) {
                        if (obj.ListingId == id) {
                            obj.SalePrice = saleprice.replace(/,/g, "");
                            return;
                        }
                    });
                });
            }
            else {
                jQuery(".SalePriceForm").validationEngine('attach', { promptPosition: "centerRight" });
            }
        }
    });

    

    grid.showDefaultTab();
    grid.loadGrid();

    $("#cbSoldNewUsed").live('change', function (e) {
        var selectedScreen = e.target.options[e.target.selectedIndex].value;
        grid.changeView(selectedScreen);
        $("#cbSoldNewUsed").val(selectedScreen);
    });

    $("#sold_car_date_From").datepicker({
        onSelect: function (dateText, inst) {
            grid.loadGrid();
        }
    });

    $("#sold_car_date_To").datepicker({
        onSelect: function (dateText, inst) {
            grid.loadGrid();
        }
    });

    $(".calendar_logo_From").click(function () {
        $("#sold_car_date_From").trigger("focus");
    });

    $(".calendar_logo_To").click(function () {
        $("#sold_car_date_To").trigger("focus");
    });

    
    $("#vinbrochure_select_year").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedValue = e.target.options[e.target.selectedIndex].value;
        hideBrochureInfo();
        resetMake();
        resetModel();
        $.ajax({
            type: "POST",
            url: "/Inventory/GetMakesFromChrome",
            data: { year: selectedValue },
            success: function (results) {
                $("#divMake").html(results);
                $.unblockUI();
            }
        });
        $.unblockUI();
    });

    $("#vinbrochure_select_make").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedValue = e.target.options[e.target.selectedIndex].value;
        var selectedYear = $("#vinbrochure_select_year").val();
        
        hideBrochureInfo();
        resetModel();
        $.ajax({
            type: "POST",
            url: "/Inventory/GetModelsFromChrome",
            data: { year: selectedYear, makeID: selectedValue },
            success: function(results) {
                $("#divModel").html(results);
                $.unblockUI();
            }
        });
        $.unblockUI();
    });
    
    $("#vinbrochure_select_model").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var yearID = $('#vinbrochure_select_year').val();
        var makeID = $('#vinbrochure_select_make').val();
        var modelID = $('#vinbrochure_select_model').val();
        resetNameAndEmail();
        if (modelID == 'Model...') {
            hideBrochureInfo();
        } else {
            showBrochureInfo();
            var year = $('#vinbrochure_select_year :selected').text();
            var make = $('#vinbrochure_select_make :selected').text();
            var model = $('#vinbrochure_select_model :selected').text();
            $('#titleBrochure').text(year + ' ' + make + ' ' + model);
            $.ajax({
                type: "POST",
                url: "/Inventory/GetImageFromChrome",
                data: { year: yearID, makeID: makeID, modelID: modelID, make: make, model: model },
                success: function(results) {
                    $("#imgCar").attr('src', results.photoUrl);
                    $('#hdPhotoUrl').val(results.photoEndCodeUrl);
                    if (results.brochureFiles > 1) {
                        $('#brochure1').show();
                    } else {
                        $('#brochure1').hide();
                    }
                    $.unblockUI();
                }
            });
        }
        
        $.unblockUI();
    });

    $("#filterPanel").hide();

    $("#submitFilter").click(function (event) {
        var dataUrl = '/Inventory/ViewInventoryJson';

        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var fromMile = getMileValue($("#mileFrom").val());
        var toMile = getMileValue($("#mileTo").val());

        var fromDate = "";
        var toDate = "";

        if (viewInfo.currentTab == soldTab) {
            if ($("#sold_car_date_From").val() == '') $("#sold_car_date_From").val(previousDate(true, 90));
            fromDate = $("#sold_car_date_From").val();
            if ($("#sold_car_date_To").val() == '') $("#sold_car_date_To").val(todayDate(true));
            toDate = $("#sold_car_date_To").val();
        }

        $.ajax({
            type: "POST",
            dataType: "JSON",
            url: dataUrl,
            data: {
                screen: viewInfo.currentView,
                sortBy: viewInfo.sortFieldName,
                isUp: viewInfo.isUp,
                pageSize: pageSize,
                fromDate: fromDate,
                toDate: toDate,
                usingFilter: true,
                make: $("#selectMake option:selected").text(),
                model: $("#selectModel option:selected").text(),
                trim: $("#selectTrim option:selected").text(),
                year: $("#selectYear option:selected").text(),
                price: $("#selectPrice option:selected").val(),
                fromMile: fromMile,
                toMile: toMile
            },
            success: function (result) {
                if (result.count == 0) {
                    $('#divHasContent').hide();

                    $('#divNoContent').text('There are no results which match your filter criteria.');
                    $('#divNoContent').show();

                } else {
                    $('#divHasContent').show();
                    $('#divNoContent').hide();
                }

                inventoryObj = result.list;
                grid.updateGridFromData(inventoryObj);
                resetScrollValues();
                $(".SalePriceForm").validationEngine({ promptPosition: "centerRight", scroll: false });
                $.unblockUI();
            },
        });
    });
});

var dataAdded = true;

$('.vin_listVehicle_holder').scroll(function () {
    if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight && dataAdded) {
        var dataUrl = '/Inventory/ViewCacheInventoryJson';

        $.ajax({
            type: "POST",
            dataType: "JSON",
            url: dataUrl,
            data: {
                pageIndex: ++pageIndex,
                pageSize: pageSize,
            },
            success: function (result) {
                if (result.list.length > 0) {
                    dataAdded = true;
                    inventoryObj.push.apply(inventoryObj, result.list);
                    grid.updateGridFromData(inventoryObj);
                    $(".SalePriceForm").validationEngine({ promptPosition: "centerRight", scroll: false });
                }
                else {
                    dataAdded = false;
                }
            },
        });
    }
}); validateInput

$('a:not(.iframe)').click(function (e) {
    if ($(this).attr('target') == '')
        $('#elementID').removeClass('hideLoader');
});

$("a.iframe").fancybox({ 'width': 1010, 'height': 715 });

$('.rowOuter').each(function () {
    var status = $(this).children('.imageCell').children('.imageWrap').children('input').attr('value');
    //console.log(status);
    if (status == 0 || status == undefined) {
        //console.log('good status');
        $(this).removeClass('border');
    } else {
        //console.log('bad status');
        $(this).addClass('border');
        if (status == 1 || status == 2) { console.log('med status'); $(this).addClass('med'); }
    }
});
$('#sold').hide('fast');

var toggle = false;

function compareString(x, y) {
    if (x != y) {
        return x.toLowerCase() > y.toLowerCase() ? 1 : -1;
    }
    return 0;
}

function resetMake() {
    var html = '<select id="vinbrochure_select_make"><option>Make</option></select>';
    $('#divMake').html(html);
}

function resetNameAndEmail() {
    $('#vinbrochure_customer_name').val('');
    $('#vinbrochure_customer_email').val('');
}

function resetModel() {
    var html = '<select id="vinbrochure_select_model"><option>Model</option></select>';
    $('#divModel').html(html);
    resetNameAndEmail();
}

function resetAll() {
    hideBrochureInfo();
    $('#vinbrochure_select_year').val('0');
    resetMake();
    resetModel();
    resetNameAndEmail();
}

function ViewBrochure(index) {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var year = $('#vinbrochure_select_year :selected').text();
    var make = $('#vinbrochure_select_make :selected').text();
    var model = $('#vinbrochure_select_model :selected').text();

    var url = viewBrochureUrl.replace('_year', year).replace('_make', make).replace('_model', model).replace('_index', index);
    
    var urlCheckBrochure = checkBrochureUrl.replace('_year', year).replace('_make', make).replace('_model', model).replace('_index', index);
    $.ajax({
        type: "POST",
        url: urlCheckBrochure,
        success: function (results) {
            if (results.isExisted == false)
                alert('A brochure for this ' + year + ' ' + make + ' ' + model + ' is not available at this time.');
            else {
                window.open(
                    url,
                    '_blank' // <- This is what makes it open in a new window.
                );
            }
            $.unblockUI();
        }
    });
}

function SendBrochure(index) {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    var year = $('#vinbrochure_select_year :selected').text();
    var make = $('#vinbrochure_select_make :selected').text();
    var model = $('#vinbrochure_select_model :selected').text();
    var email = $('#vinbrochure_customer_email').val();
    var name = $('#vinbrochure_customer_name').val();
    var urlCheckBrochure = checkBrochureUrl.replace('_year', year).replace('_make', make).replace('_model', model).replace('_index', index);
    
    $.ajax({
        type: "POST",
        url: urlCheckBrochure,
        success: function (results) {
            if (results.isExisted == false) {
                alert('A brochure for this ' + year + ' ' + make + ' ' + model + ' is not available at this time.');
                $.unblockUI();
            } else {
                $.ajax({
                    type: "POST",
                    url: sendBrochureUrl,
                    data: { year: year, make: make, model: model, email: email, name: name, photoUrl: $('#hdPhotoUrl').val(), index: index },
                    success: function(result) {
                        alert('Brochure was sent to customer successfully.');
                        resetAll();
                        $.unblockUI();
                    }
                });
            }
        }
    });
}

var bIsShowingFilterPanel = false;
function toggleFilterPanel() {
    bIsShowingFilterPanel = !bIsShowingFilterPanel;

    if (bIsShowingFilterPanel) {
        $("#filterPanel").show();
        $("#divHasContent").css("height", 495);
    }
    else {
        $("#filterPanel").hide();
        $("#divHasContent").css("height", 560);
    }
}

function initializeFilter(dataList) {
    if (dataList.length == 0) {
        return;
    }

    var arrYear = [];
    var maxPrice = 0;
    var minMile = dataList[0].Mileage;
    var maxMile = dataList[0].Mileage;

    dataList.forEach(function (data) {
        if (arrYear.indexOf(data.Year) == -1) {
            arrYear.push(data.Year);
        }

        if (data.SalePrice > maxPrice) {
            maxPrice = data.SalePrice;
        }

        if (data.Mileage < minMile) {
            minMile = data.Mileage;
        }

        if (data.Mileage > maxMile) {
            maxMile = data.Mileage;
        }
    });

    arrYear.sort(function (a, b) { return b - a });

    var selectYear = $("#selectYear");
    var selectMake = $("#selectMake");
    var selectModel = $("#selectModel");
    var selectTrim = $("#selectTrim");
    var selectPrice = $("#selectPrice");
    var mileFrom = $("#mileFrom");
    var mileTo = $("#mileTo");

    createListItems(selectYear, arrYear);
    resetListItems(selectMake);
    resetListItems(selectModel);
    resetListItems(selectTrim);

    createPriceItems(selectPrice, maxPrice);

    if (parseInt(minMile) == 0) {
        mileFrom.val(0);
    }
    else {
        mileFrom.val(formatNumberString(minMile));
    }

    if (parseInt(maxMile) == 0) {
        mileTo.val(0);
    }
    else {
        mileTo.val(formatNumberString(maxMile));
    }

    $("#selectYear").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedYear = $("#selectYear option:selected").text();
        var arrMake = [];

        dataList.forEach(function (data) {
            if (data.Year == selectedYear && arrMake.indexOf(data.Make) == -1) {
                arrMake.push(data.Make);
            }
        });

        createListItems(selectMake, arrMake);
        resetListItems(selectModel);
        resetListItems(selectTrim);

        $.unblockUI();
    });

    $("#selectMake").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedYear = $("#selectYear option:selected").text();
        var selectedMake = $("#selectMake option:selected").text();
        var arrModel = [];

        dataList.forEach(function (data) {
            if (data.Year == selectedYear && data.Make == selectedMake && arrModel.indexOf(data.Model) == -1) {
                arrModel.push(data.Model);
            }
        });

        createListItems(selectModel, arrModel);
        resetListItems(selectTrim);

        $.unblockUI();
    });

    $("#selectModel").live('change', function (e) {
        $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
        var selectedYear = $("#selectYear option:selected").text();
        var selectedMake = $("#selectMake option:selected").text();
        var selectedModel = $("#selectModel option:selected").text();
        var arrTrim = [];

        dataList.forEach(function (data) {
            if (data.Year == selectedYear && data.Make == selectedMake && data.Model == selectedModel && arrTrim.indexOf(data.Trim) == -1) {
                arrTrim.push(data.Trim);
            }
        });

        createListItems(selectTrim, arrTrim);

        $.unblockUI();
    });
}

function resetListItems(select) {
    $(select).empty();

    var el = document.createElement("option");
    el.textContent = "All";
    el.value = "All";
    select.append(el);
}

function createListItems(select, options) {
    $(select).empty();

    options.unshift("All");

    for (var i = 0; i < options.length; i++) {
        var opt = options[i];
        var el = document.createElement("option");
        el.textContent = opt;
        el.value = opt;
        select.append(el);
    }
}

function createPriceItems(select, maxPrice) {
    $(select).empty();

    var priceIncrement = 10000;
    var price = 10000;
    var el = document.createElement("option");
    el.textContent = "All";
    el.value = 0;
    select.append(el);

    el = document.createElement("option");
    el.textContent = "Less than $" + formatNumberString(price);
    el.value = price / priceIncrement;
    select.append(el);

    while (price < 100000) {
        price = price + priceIncrement;

        el = document.createElement("option");
        el.textContent = "$" + formatNumberString(price - priceIncrement) + " - $" + formatNumberString(price);
        el.value = price / priceIncrement;
        select.append(el);
    }

    el = document.createElement("option");
    el.textContent = "Greater than $" + formatNumberString(price);
    el.value = (price / priceIncrement) + 1;
    select.append(el);
}

function formatNumberString(number) {
    return formatDollar(parseInt(number));
}

function validateInput(event) {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.preventDefault();
    }
}

function getMileValue(mile) {
    return mile.replace(/,/g, "");
}

function formatMile(event, obj) {
    if (event == null || event.keyCode == 46 || event.keyCode == 8
         || (event.keyCode >= 48 && event.keyCode <= 57)) {
        var value = $(obj).val();

        if (value == "") {
            $(obj).val(0);
            return;
        }

        var originalLength = value.length;
        var rightPos = value.length - obj.selectionEnd;
        var leftPos = obj.selectionEnd;

        value = value.replace(/,/g, "")
        value = parseInt(value).toString();
        value = value.split("").reverse().join("");

        var index = 0;
        var j = 0;

        while (index < value.length - 1) {
            index++;
            j++;

            if (j % 3 == 0) {
                value = value.substr(0, index) + "," + value.substr(index, value.length);
                index++;
            }
        }

        value = value.split("").reverse().join("");
        $(obj).val(value);

        if (value.length > originalLength) {
            rightPos = value.length - rightPos;

            obj.selectionStart = rightPos;
            obj.selectionEnd = rightPos;
        }

        if (value.length <= originalLength) {
            obj.selectionStart = leftPos;
            obj.selectionEnd = leftPos;
        }
    }
}

function previousDate(isFlash, noOfDate) {
    var beforeDate = new Date();
    var today = new Date();
    today.setDate(beforeDate.getDate() - noOfDate);

    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }

    if (isFlash) {
        today = mm + '/' + dd + '/' + yyyy;
    } else {
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}

function todayDate(isFlash) {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }

    if (isFlash) {
        today = mm + '/' + dd + '/' + yyyy;
    } else {
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}