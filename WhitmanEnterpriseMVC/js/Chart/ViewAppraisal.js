$(document).ready(function(e) {

    InitializeChart();

    // chart expand/contract script
    $('#graphButton').click(function() {
        // mark flag as open chart
        if(openChart != undefined)
            openChart = true;
        
        if (expand) {
        } else {
            var gwrap_fbox = $('#graph-fancybox');
            // open fancybox
            gwrap_fbox.click();
        }
    });

    $('#graphButton.expandediframe').fancybox({
        autoDimensions: true,
        width: 940,
        height: 615
    });
});

function LoadSavedSelections() {
    
    //if ($('#IsCertified').val() == 'True') { $filter.certified = true; }
    if ($('#IsFranchise').val() == 'True') {
        $filter.dealertype = 'franchise';        
    } else if ($('#IsIndependant').val() == 'True') {
        $filter.dealertype = 'independant';        
    } else {
        delete $filter.dealertype;        
    }

//    if (default_option != null) {
//        $filter.option = {};
//        if (default_option.length == 1 && default_option[0] == 0)
//            delete $filter.option;
//        else {
//            for (i = 0; i < default_option.length; i++) {
//                if (default_option[i] != "") {
//                    $filter.option[default_option[i]] = true;
//                }
//            }
//        }
//    }

    if (default_trim != null) {
        $filter.title = {}; $filter.title.trim = [];
        if (default_trim.length == 1 && default_trim[0] == 0)
            delete $filter.title;
        else {
            for (i = 0; i < default_trim.length; i++) {
                if (default_trim[i] != "") {
                    $filter.title.trim.push(default_trim[i]);
                }
            }
        }
    }
}

function InitializeCar(item) {
    var $car = {
        title: // year make model
                                    {
                                    year: item.title.year,
                                    make: ucfirst(item.title.make),
                                    model: ucfirst(item.title.model),
                                    trim: ucfirst(item.title.trim)
                                },
        vin: item.vin,
        extcolor: item.color.exterior,
        distance: item.distance,
        certified: item.certified, // certified 0/1 (false/true)
        option: item.option, // options string, comma deliminated
        trim: item.trim,
        trims: item.trims,
        uptime: item.uptime, // time on cars.com, autotrader.com and ebay
        thumbnail: item.thumbnail,
        carscomlistingurl: item.carscomlistingurl, // img string, comma deliminated
        dealertype: item.franchise, // dealertype, can be franchise or independant
        seller: item.seller.sellername,
        address: item.seller.selleraddress,
        miles: item.miles,
        price: item.price,
        carscom: item.carscom,
        autotrader: item.autotrader,
        autotraderlistingurl: item.autotraderlistingurl,
        listingid: item.listingid        
    }; // after construction add object to array

    return $car;
}

function InitializeCarSetting(target) {
    var $dCar = {
        label: '',
        color: '#003cff',
        shadowSize: 5,
        points: {
            radius: 10,
            fillColor: 'white'
        },
        data: [[target.mileage /* miles */, target.salePrice /* price */]],
        clickable: false,
        draggabley: true,
        draggablex: false
    };
    //    console.log(target.mileage);
    //    console.log(target.salePrice);
    //    console.log($dCar);
    return $dCar;
}

function InitializeCarSetting() {
    var $dCar = {
        label: '',
        color: '#003cff',
        shadowSize: 5,
        points: {
            radius: 10,
            fillColor: 'white'
        },
        data: [[0 /* miles */, 0 /* price */]],
        clickable: false,
        draggabley: true,
        draggablex: false
    };

    return $dCar;
}

function InitializeChart() {
    // initialize data set array
    $data = [];

    var request_url = requestUrl;
    request_url = request_url.replace('PLACEHOLDER', ListingId);

    $.ajax({
        type: "POST",
        url: request_url,
        data: {},
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            var carlist = data.carlist;
            var target = data.target;
            var marketinfo = data.market;
            var hasCurrentCarInTheList = false;
            // CHECKS IF RESULT FROM AJAX IS EMPTY
            if (carlist == null || carlist.length == 0) {
                carlist = [];
                $('#txtDistance').html('Click to See Nationwide Market Data');

                var request_nationwide_url = requestNationwideUrl;
                request_nationwide_url = request_nationwide_url.replace('PLACEHOLDER', ListingId);
                $.ajax({
                    type: "POST",
                    url: request_nationwide_url,
                    data: {},
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        carlist = data.carlist;
                        target = data.target;
                        marketinfo = data.market;
                        hasCurrentCarInTheList = false;

                        if (carlist == null || carlist.length == 0) {
                            $('#age').html('<em style="display: block; margin-top: 35px;">Market Data Not Available</em>');
                            carlist = [];
                        }
                        else {
                            if (marketinfo != null) {
                                $("#txtCarsOnMarket").text(marketinfo.carsOnMarket);
                                $("#txtMinPrice").text(marketinfo.minimumPrice);
                                $("#txtAveragePrice").text(marketinfo.averagePrice);
                                $("#txtMaxPrice").text(marketinfo.maximumPrice);
                                $("#txtAvgDays").text(marketinfo.averageDays);
                            }

                            // set ranking
                            if (target != null) {
                                $("#txtRanking").text(target.ranking);
                            };

                            if (carlist != null) {
                                for (var i = 0; i < carlist.length; i++) {
                                    var arr = carlist;

                                    $imgstring = "";
                                    $car = InitializeCar(arr[i]);
                                    $data[i] = $car;
                                }
                            }

                            $dCar = InitializeCurrentCar(target);

                            // set ranges
                            var ranged = setRange($data);

                            // save original pricing
                            var originalPrice = $dCar.data[0][1];

                            // load saved selections
                            // we already filtered the results in code behind, so we don't need to do on the js code
                            //LoadSavedSelections();

                            // draw chart
                            drawChart($data, fRange, $filter, $dCar, expand, default_option, default_trim);

                            // unset filter
                            $filter = {};
                        }
                    }
                });
            }
            else {
                if (marketinfo != null) {
                    $("#txtCarsOnMarket").text(marketinfo.carsOnMarket);
                    $("#txtMinPrice").text(marketinfo.minimumPrice);
                    $("#txtAveragePrice").text(marketinfo.averagePrice);
                    $("#txtMaxPrice").text(marketinfo.maximumPrice);
                    $("#txtAvgDays").text(marketinfo.averageDays);
                }

                // set ranking
                if (target != null) {
                    $("#txtRanking").text(target.ranking);
                };

                if (carlist != null) {
                    for (var i = 0; i < carlist.length; i++) {
                        var arr = carlist;

                        $imgstring = "";
                        $car = InitializeCar(arr[i]);
                        $data[i] = $car;
                    }
                }

                $dCar = InitializeCurrentCar(target);

                // set ranges
                var ranged = setRange($data);

                // save original pricing
                var originalPrice = $dCar.data[0][1];

                // load saved selections
                // we already filtered the results in code behind, so we don't need to do on the js code
                //LoadSavedSelections();

                // draw chart
                drawChart($data, fRange, $filter, $dCar, expand, default_option, default_trim);

                // unset filter
                $filter = {};

                // don't need to update market up & down for appraisals
                //UpdateMarketUpAndDown(target, $data);
            }
        }
    });
}
function InitializeCurrentCar(target) {
    var $car = {
        title: // year make model
                                    {
                                    year: target.title.year,
                                    make: ucfirst(target.title.make),
                                    model: ucfirst(target.title.model),
                                    trim: ucfirst(target.title.trim)
                                },
        vin: '',
        extcolor: '',
        distance: target.distance,
        certified: target.certified, // certified 0/1 (false/true)
        option: '', // options string, comma deliminated
        trim: target.trim,
        trims: '',
        uptime: '', // time on cars.com, autotrader.com and ebay
        thumbnail: target.thumbnail,
        carscomlistingurl: '', // img string, comma deliminated
        dealertype: '', // dealertype, can be franchise or independant
        seller: target.seller.sellername,
        address: target.seller.selleraddress,
        miles: target.mileage,
        price: target.salePrice,
        carscom: false,
        autotrader: false,
        autotraderlistingurl: '',
        listingid: target.listingId,
        label: '',
        color: '#003cff',
        shadowSize: 5,
        points: {
            radius: 10,
            fillColor: 'white'
        },
        data: target != null ? [[target.mileage /* miles */, target.salePrice /* price */]] : [0, 0],
        clickable: false,
        draggabley: false,
        draggablex: false
    };  // after construction add object to array

    return $car;
}
function UpdateMarketUpAndDown(target, $data) {
    if (target != null && ($data != null || $data.length > 0)) {
        var marketStatus;
        var urlString;
        var marketView = new marketPlotting($data);
        var aOver = marketView.Intercept * 1.025;
        var aUnder = marketView.Intercept * 0.975;
        if (target.salePrice > aOver + marketView.Slope * target.mileage) {
            marketStatus = 3;
            urlString = "/Inventory/UpdateMarketUp/";
        } else if (target.salePrice < aUnder + marketView.Slope * target.mileage) {
            marketStatus = 1;
            urlString = "/Inventory/UpdateMarketDown/";
        } else if ((target.salePrice >= aUnder + marketView.Slope * target.mileage) && (target.salePrice <= aOver + marketView.Slope * target.mileage)) {
            marketStatus = 2;
            urlString = "/Inventory/UpdateMarketIn/";
        }

        if (marketStatus != null && marketStatus != "") {
            //var urlString = "/Inventory/UpdateMarketUpAndDown?id='" + ListingId + "'&status='" + marketStatus + "'";
            var id = $('#ListingId').val();
            $.ajax({
                type: "POST",
                contentType: "text/hmtl; charset=utf-8",
                dataType: "html",
                url: urlString + id,
                data: { id: ListingId, status: marketStatus },
                cache: false,
                traditional: true,
                success: function(result) {
                    //alert(result);
                },
                error: function(err) {
                    //alert(err.status + " - " + err.statusText);
                }
            });
        }
    }
}

// calculation
marketPlotting = function(arr) {
    var N = arr.length,
	        XY = [],
	        XX = [],
	        EX = 0,
	        EY = 0,
	        EXY = 0,
	        EXX = 0;

    for (var i = 0; i < N; i++) {
        var y = arr[i].price;
        var x = arr[i].miles;

        XY.push(x * y);
        XX.push(x * x);
        EX += x;
        EY += y;
        EXY += x * y;
        EXX += x * x;
    }

    this.Slope = ((N * EXY) - (EX * EY)) / ((N * EXX) - (EX * EX));
    this.Intercept = (EY - (this.Slope * EX)) / N;
    this.RegressionY = function(x) {
        return this.Intercept + (this.Slope * x);
    }
    this.RegressionX = function(y) {
        return (y - this.Intercept) / this.Slope;
    }
};