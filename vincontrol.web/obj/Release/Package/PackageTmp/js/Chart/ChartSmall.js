var $dCar;
var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange:{min:0,max:100}, isSoldView:false, isSmallChart:false };

var myArray = [];
var trimsData = [];
var selectedtrimsData = [];
var $data = [];

var targetPrice;

var isCarComs;

jQuery.removeFromArray = function (value, arr) {
    return jQuery.grep(arr, function (elem, index) {
        return elem !== value;
    });
};

function RefreshChartWithTrim(data, trimArray) {
   // console.log("RefreshChartWithTrim");
    //console.log(trimArray);
    ChartInfo.$filter.title = {};
    ChartInfo.$filter.title.trim = trimArray;
    var obj;
    
    if (trimArray.length == 0) {
        //console.log(fRange);
        drawChart([], ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, default_trim, dataTrims);
        $('#txtMaxPrice').text('0');
        $('#txtAveragePrice').text('0');
        $('#txtMinPrice').text('0');
        $('#txtCarsOnMarket').text('0');
        $('#txtRanking').text('0');
        initRecommendation(0, 0, 0);
    } else {
        //console.log(fRange);

        obj = drawChart(data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, default_trim, dataTrims);
        try {
            $('#txtMaxPrice').text(money_format(obj.largestPrice));
            $('#txtAveragePrice').text(money_format(obj.averageprice));
            $('#txtMinPrice').text(money_format(obj.smallestPrice));
            if (obj.cars != null && obj.cars != "")
                $('#txtCarsOnMarket').text(parseInt(obj.cars) + 1);
            else
                $('#txtCarsOnMarket').text('1');
            $('#txtRanking').text(obj.currentRanking);
            
            initRecommendation(obj.largestPrice, obj.averageprice, obj.smallestPrice);
        } catch (e) {

        } 
    }
}

function SaveSelectionSmallChart(listingId, trims, vinCurrentScreen) {
    var url = "/Chart/SaveSmallSelections?listingId=" + listingId + "&trims=" + trims + "&screen=" + vinCurrentScreen;

    $.ajax({
        type: "POST",
        url: url,
        data: {},
        success: function (results) {
            UpdateCarRanking(vinCurrentScreen);
            alert(results);
        }
    });
}

function UpdateCarRanking(vinCurrentScreen) {
    var listingId = ListingId;

    var carRanking = 1;
    var numberOfCars = 0;
    if ($currentFilterredList != undefined && $currentFilterredList.length > 0) {
        numberOfCars = $currentFilterredList.length + 1;
        $.each($currentFilterredList, function (index, obj) {
            if ($dCar.price >= obj.price)
                carRanking++;
        });
        var prices = ChartHelper.extractVal($currentFilterredList, "price");
        var largestPrice = Math.max.apply(Math, prices);
        var smallestPrice = Math.min.apply(Math, prices);
        var averagePrice = Math.round(ChartHelper.totalprice($currentFilterredList) / $currentFilterredList.length);

        if (numberOfCars > 0) {
          
            var updateRankingUrl = "/Chart/UpdateCarRanking?listingId=" + listingId + "&carRanking=" + carRanking + "&numberOfCars=" + numberOfCars + "&oldCarRanking=" + $dCar.ranking + "&oldNumberOfCars=" + $dCar.carsOnMarket  + "&smallestPrice=" + smallestPrice + "&averagePrice=" + averagePrice + "&largestPrice=" + largestPrice + "&screen=" + vinCurrentScreen;
            $.ajax({
                type: "POST",
                url: updateRankingUrl,
                data: $("form").serialize(),
                success: function(results) {

                }
            });
        }
    }
}

function ucfirst(str) {
    if (typeof str == 'string') {
        var firstLetter = str.substr(0, 1);
        return firstLetter.toUpperCase() + str.substr(1);
    }
}



$(document).ready(function (e) {
  
    $('#SaveSelectionSmallChart').click(function () {
       // console.log(ListingId);
        SaveSelectionSmallChart(ListingId, myArray.join(','), 2);
    });
    
    $('#SaveSelectionSmallChartAppraisal').click(function () {
       // console.log(ListingId);
        SaveSelectionSmallChart(ListingId, myArray.join(','), 1);
    });
    
    initSmallChart();

    $('#checkAll').live('click', function () {
        if ($(this).is(':checked')) {
            $("input[id^=chk]").each(function () {
                $(this).attr('checked', false);
                myArray = jQuery.removeFromArray($('#lbl' + $(this).attr("name")).text().toLowerCase(), myArray);
            });
            $("input[id^=chk]").each(function () {
                $(this).attr('checked', true);
                myArray.push($('#lbl' + $(this).attr("name")).text().toLowerCase());
                $('#divTrims').html(myArray.join(','));
            });
        } else {
            $("input[id^=chk]").each(function () {
                $(this).attr('checked', false);
                myArray = jQuery.removeFromArray($('#lbl' + $(this).attr("name")).text().toLowerCase(), myArray);
                $('#divTrims').html('&nbsp;');
            });
        }
        RefreshChartWithTrim($data, myArray);
    });

    $("input[id^=chk]").live('click', function () {
        if ($(this).is(':checked')) {
            myArray.push($(this).next().text().toLowerCase());
            $('#divTrims').html(myArray.join(','));
            if (myArray.length == trimsData.length) {
                $('#checkAll').attr('checked', true);
            }
        } else {
            $('#checkAll').attr('checked', false);
            myArray = jQuery.removeFromArray($(this).next().text().toLowerCase(), myArray);
            if (myArray.length == 0)
                $('#divTrims').html("&nbsp;");
            else
                $('#divTrims').html(myArray.join(','));
        }

        RefreshChartWithTrim($data, myArray);
    });
    //console.log(requestNationwideUrl);
   
});

function sortResults(data, prop, asc) {
    return data.sort(function (a, b) {
        if (asc) return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
        else return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
    });
}

function initSmallChart() {
    //console.log("initSmallChart");
    var count = 0;
    //console.log(requestNationwideUrl);
    var globalData;
    $.ajax({
        type: "POST",
        url: UrlPaths.requestNationwideUrl,
        data: {},
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            //console.log(data);
            globalData = data;
            dataTrims = data.trims;
            isCarComs = data.isCarComs;
            $('#IsCertified').val(data.isCertified);
            //if ($('#IsCertified').val() == 'true') {
            //    $('#certified').attr('checked', true);
            //} else {
            //    $('#certified').removeAttr('checked');
            //}
            if (data.trims != null) {
                $.each(data.trims, function (index, obj) {
                    if (parseInt(obj.distance) <= parseInt(100)) {
                        trimsData.push(obj.trim.toLowerCase());
                    }
                });
            }
            if (data.userselectedtrims != null) {
                $.each(data.userselectedtrims, function (index, obj) {
                    selectedtrimsData.push(obj.toLowerCase());
                });
            }

            if (trimsData.length > 0) {
                $('#trim-filter').append($("<input type='checkbox' name='All' id='checkAll' />"));
                $('#trim-filter').append($("<label id='lblAll' for='checkAll'>All trims</label>"));
                $("#SaveSelectionSmallChartAppraisal").show();
                $("#SaveSelectionSmallChart").show();
            }
            else {
                $("#SaveSelectionSmallChartAppraisal").hide();
                $("#SaveSelectionSmallChart").hide();

                $("#graphWrap").css("background-image", "url('../images/No-Data.png')");
                $('#placeholder').hide();
            }

            var checkedExisted = false;
            $.each(trimsData, function(index, obj) {
                if (jQuery.inArray(obj, selectedtrimsData) != -1) {
                    checkedExisted = true;
                }
            });

            if (!checkedExisted)
                data.userselectedtrims = null;
            //console.log("selectedtrimsData");
            //console.log(selectedtrimsData.length);
            $.each(trimsData, function (index, obj) {
                var text = obj.split(' ').join('_').split('.').join('_').split('/').join('_');
                //does't have saveSelectionTrim default check all
                if (data.userselectedtrims == null || data.userselectedtrims == '') {
                    $('#trim-filter').append("<br/>");
                    $('#trim-filter').append($("<input type='checkbox' checked='checked' name='" + text + "' id='chk" + text + "' />"));
                    $('#trim-filter').append($("<label id='lbl" + text + "' for='chk" + text + "'>" + ChartHelper.ucfirst(obj) + "</label>"));
                    myArray.push(ChartHelper.ucfirst(obj).toLowerCase());
                    $('#divTrims').html(myArray.join(','));
                    $('#checkAll').attr('checked', true);
                } else {
                    if (jQuery.inArray(obj, selectedtrimsData) == -1) {
                        $('#trim-filter').append("<br/>");
                        $('#trim-filter').append($("<input type='checkbox' name='" + text + "' id='chk" + text + "' />"));
                        $('#trim-filter').append($("<label id='lbl" + text + "' for='chk" + text + "'>" + ChartHelper.ucfirst(obj) + "</label>"));
                    } else {
                        $('#trim-filter').append("<br/>");
                        $('#trim-filter').append($("<input type='checkbox' checked='checked' name='" + text + "' id='chk" + text + "' />"));
                        $('#trim-filter').append($("<label id='lbl" + text + "' for='chk" + text + "'>" + ChartHelper.ucfirst(obj) + "</label>"));
                        myArray.push(ChartHelper.ucfirst(obj).toLowerCase());
                        $('#divTrims').html(myArray.join(','));
                        count++;
                    }
                }
                
                var carlist = globalData.cars;
                var target = globalData.target;

                var marketinfo = globalData.market;

                var hasCurrentCarInTheList = false;

                // CHECKS IF RESULT FROM AJAX IS EMPTY
                if (carlist == null || carlist.length == 0) {
                    $('#txtDistance').html('Click to Display Nationwide Market');
                    $('#age').html('<em style="display: block; margin-top: 35px;">Market Data Not Available</em>');
                    carlist = [];
                    //var request_nationwide_url = UrlPaths.requestNationwideUrl;
                    //request_nationwide_url = request_nationwide_url.replace('PLACEHOLDER', ListingId);
                    //$.ajax({
                    //    type: "POST",
                    //    url: request_nationwide_url,
                    //    data: {},
                    //    dataType: 'json',
                    //    contentType: "application/json; charset=utf-8",
                    //    success: function (data) {
                    //        carlist = data.cars;
                    //        target = data.target;
                    //        marketinfo = data.market;
                    //        hasCurrentCarInTheList = false;


                    //        if (carlist == null || carlist.length == 0) {
                    //            $('#age').html('<em style="display: block; margin-top: 35px;">Market Data Not Available</em>');
                    //            carlist = [];
                    //        }
                    //        else {
                    //            if (marketinfo != null) {
                    //                $("#txtCarsOnMarket").text(marketinfo.carsOnMarket);
                    //                $("#txtMinPrice").text(money_format(marketinfo.minimumPrice));
                    //                $("#txtAveragePrice").text(money_format(marketinfo.averagePrice));
                    //                $("#txtMaxPrice").text(money_format(marketinfo.maximumPrice));
                    //                $("#txtAvgDays").text(marketinfo.averageDays);

                    //                if (marketinfo.maximumPrice == 0 && marketinfo.averagePrice == 0 && marketinfo.minimumPrice == 0) {
                    //                    initRecommendation(abovePrice, averagePrice, belowPrice);
                    //                } else {
                    //                    abovePrice = marketinfo.maximumPrice;
                    //                    averagePrice = marketinfo.averagePrice;
                    //                    belowPrice = marketinfo.minimumPrice;
                    //                    initRecommendation(marketinfo.maximumPrice, marketinfo.averagePrice, marketinfo.minimumPrice);
                    //                }
                    //            }

                    //            // set ranking
                    //            if (target != null) {
                    //                $("#txtRanking").text(target.ranking);
                    //                targetPrice = target.salePrice;
                    //            };

                    //            var marketMapping = new VINControl.Chart.MarketMapping();
                    //            if (carlist != null) {
                    //                for (var i = 0; i < carlist.length; i++) {
                    //                    var arr = carlist;
                    //                    $data[i] = marketMapping.InitializeCar(arr[i]);
                    //                }
                    //            }

                    //            if (marketinfo != null) {
                    //                $dCar = marketMapping.InitializeCurrentCar(target, marketinfo.carsOnMarket);
                    //            }
                    //            // unset filter
                    //            ChartInfo.$filter = {};
                    //        }
                    //    }
                    //});
                }
                else {
                    if (marketinfo != null) {
                        $("#txtCarsOnMarket").text(marketinfo.carsOnMarket);
                        $("#txtMinPrice").text(money_format(marketinfo.minimumPrice));
                        $("#txtAveragePrice").text(money_format(marketinfo.averagePrice));
                        $("#txtMaxPrice").text(money_format(marketinfo.maximumPrice));
                        $("#txtAvgDays").text(marketinfo.averageDays);

                        if (marketinfo.maximumPrice == 0 && marketinfo.averagePrice == 0 && marketinfo.minimumPrice == 0) {
                            initRecommendation(abovePrice, averagePrice, belowPrice);
                        } else {
                            abovePrice = marketinfo.maximumPrice;
                            averagePrice = marketinfo.averagePrice;
                            belowPrice = marketinfo.minimumPrice;
                            initRecommendation(marketinfo.maximumPrice, marketinfo.averagePrice, marketinfo.minimumPrice);
                        }
                    }

                    // set ranking
                    if (target != null) {
                        $("#txtRanking").text(target.ranking);
                        targetPrice = target.salePrice;
                    };

                    var marketMapping = new VINControl.Chart.MarketMapping();
                    if (carlist != null) {
                        for (var i = 0; i < carlist.length; i++) {
                            var arr = carlist;
                            $data[i] = marketMapping.InitializeCar(arr[i]);
                        }
                    }

                    if (marketinfo != null) {
                        $dCar = marketMapping.InitializeCurrentCar(target, marketinfo.carsOnMarket);
                    }
                    // unset filter
                    ChartInfo.$filter = {};
                }
                //else {
                //    if (marketinfo != null) {
                //        $("#txtCarsOnMarket").text(marketinfo.carsOnMarket);
                //        $("#txtMinPrice").text(money_format(marketinfo.minimumPrice));
                //        $("#txtAveragePrice").text(money_format(marketinfo.averagePrice));
                //        $("#txtMaxPrice").text(money_format(marketinfo.maximumPrice));
                //        $("#txtAvgDays").text(marketinfo.averageDays);

                //        if (marketinfo.maximumPrice == 0 && marketinfo.averagePrice == 0 && marketinfo.minimumPrice == 0) {
                //            initRecommendation(abovePrice, averagePrice, belowPrice);
                //        } else {
                //            abovePrice = marketinfo.maximumPrice;
                //            averagePrice = marketinfo.averagePrice;
                //            belowPrice = marketinfo.minimumPrice;
                //            initRecommendation(marketinfo.maximumPrice, marketinfo.averagePrice, marketinfo.minimumPrice);
                //        }
                //    }

                //    // set ranking
                //    if (target != null) {
                //        $("#txtRanking").text(target.ranking);
                //        targetPrice = target.salePrice;
                //    };
                //    var marketMapping = new VINControl.Chart.MarketMapping();
                //    if (carlist != null) {
                //        for (var i = 0; i < carlist.length; i++) {
                //            var arr = carlist;
                //            //$imgstring = "";
                //            //var marketMapping = new VINControl.Chart.MarketMapping();
                //            $data[i] = marketMapping.InitializeCar(arr[i]);
                //        }
                //    }

                //    // load saved selections
                //    // we already filtered the results in code behind, so we don't need to do on the js code
                //    //LoadSavedSelections();
                //    if (marketinfo != null) {
                        
                //        $dCar = marketMapping.InitializeCurrentCar(target, marketinfo.carsOnMarket);
                //    }
                //    // draw chart

                //    // unset filter
                //    ChartInfo.$filter = {};

                //    //UpdateMarketUpAndDown(target, $data);
                //}
                //console.log('Start********************');
                //console.log($data);
                //console.log("before RefreshChartWithTrim");
                //RefreshChartWithTrim($data, myArray);
                //console.log('End********************');
            });
            
           
            
            RefreshChartWithTrim($data, myArray);
            if (trimsData.length == count) {
                $('#checkAll').attr('checked', true);
            }

            isSmallChartCompleted = true;
            
        }
    });

}

function initRecommendation(above, aver, below) {
    var averageCost = $('#hdAverageCost').val();
    var averageProfit = $('#hdAverageProfit').val();
    var averageProfitPercentage = $('#hdAverageProfitPercentage').val();
    var averageProfitUsage = $('#hdAverageProfitUsage').val();

    var costAbove = 0, costAverage = 0, costBelow = 0;
    if (above != 0 && aver != 0 && below != 0) {
        if (averageProfitUsage == 1) {
            costAbove = parseFloat(above) - parseFloat(averageCost) - parseFloat(averageProfit);
            costAverage = parseFloat(aver) - parseFloat(averageCost) - parseFloat(averageProfit);
            costBelow = parseFloat(below) - parseFloat(averageCost) - parseFloat(averageProfit);
        } else {
            costAbove = parseFloat(above) - parseFloat(averageCost) - parseFloat(above * averageProfitPercentage / 100);
            costAverage = parseFloat(aver) - parseFloat(averageCost) - parseFloat(aver * averageProfitPercentage / 100);
            costBelow = parseFloat(below) - parseFloat(averageCost) - parseFloat(below * averageProfitPercentage / 100);
            //console.log('*******averageCost**********');
            //console.log(averageCost);
            //console.log('*****end averageCost************');
            
            //console.log('********averageProfitPercentage*********');
            //console.log(averageProfitPercentage);
            //console.log('******** end averageProfitPercentage*********');
        }
    }
    $('#tdAbove').html('$' + formatNumber(costAbove));
    $('#tdAverage').html('$' + formatNumber(costAverage));
    $('#tdBelow').html('$' + formatNumber(costBelow));
}

function formatNumber(num) {
    var p = parseInt(num).toFixed(2).split(".");
    return p[0].split("").reverse().reduce(function (acc, num, i, orig) {
        return num + (i && !(i % 3) ? "," : "") + acc;
    }, "").replace('-,', '-');
}