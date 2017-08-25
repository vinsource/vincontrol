var grid;

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
        if (value != '' && value != null && value.length + 1 > end && start < end) {        
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

function InventoryGrid(currentViewInfo) {
    var viewInfo = currentViewInfo, inventoryObj = [],
        bindSorting = function () {
            $("div[class^='kpi_list_collum']").click(function () {
                //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });

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
                $("#kpicontanier").html("         <div class=\"data-content\" align=\"center\">  <img  src=\"/content/images/ajaxloadingindicator.gif\" /></div>");

                loadGrid();

                //$.unblockUI();

            });
        },
        appendSortedData = function (data) {
            $(".data-content").html($("#kpiTemplate").render(data));

            $("a.iframe").fancybox({ 'width': 1010, 'height': 715 });
            validateForm();
        },
        loadGrid = function (url) {
            //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none'} });

            var dataUrl = url == undefined || url == '' ? '/Market/GetKpiListJson' : url;

            $.ajax({
                type: "POST",
                dataType: "JSON",
                url: dataUrl,
                data: {},
                cache: false,
                traditional: true,
                success: function (result) {
                    inventoryObj = result; //jQuery.parseJSON(result);
                    updateGridFromData(inventoryObj);
                    $('.number_below').html(result.length);
                    $("a.iframeCommon").fancybox({ 'margin': 0, 'padding': 0, 'width': 400, 'height': 180, 'hideOnOverlayClick': false, 'centerOnScroll': true });
                    //$.unblockUI();
                },
                error: function (err) {
                    console.log(err.status + " - " + err.statusText);
                }
            });
        },
        setActive = function (item) {
            
        },
        setChildActive = function (item) {
            
        },

        updateGridFromData = function (inventoryObj) {
            jQuery.each(inventoryObj, function () {
                getDetailUrl(this);
            });

            //sortInventory(inventoryObj);
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
                car.URLDetail = "/Inventory/ViewIProfile?ListingID=" + car.ListingId + "&page=KPI";
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
                $("input[name='odometer']").attr('readonly', 'true');
                $("input[name='price']").attr('readonly', 'true');
            }
        },
        renderHeader = function () {
            //if (viewInfo.currentState == expandMode) {
            //    $(".right_content_nav").html($("#fullInventoryHeaderTemplate").render());
            //} else {
            //    $(".right_content_nav").html($("#inventoryHeaderTemplate").render());
            //}
        },
        showDefaultTab = function () {
            
        };
    return {
        //changeState: changeState,
        bindSorting: bindSorting,
        renderHeader: renderHeader,
        loadGrid: loadGrid,
        showDefaultTab: showDefaultTab,
        //changeView: changeView,
        setActive: setActive,
        setChildActive: setChildActive
    };
}

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

function postCurrentViewInfo() {
    $.post('/Market/UpdateKPIViewInfoStatus', viewInfo, function (data) { });
}