// JavaScript Document
$drawDiv = $('#placeholder');
$filter = {};

// Find In Array
function findInArray(arr, val) { for (var item in arr) { if (arr[item] == val) { return true; } } }

// check if expanded element in value
function expanded(a) { if (a === '?e=1') { return true; } else { return false; } }

// capitalize first letter
// function capitalize(s) {return s.charAt(0).toUpperCase() + s.slice(1);}
// string to upper
function ucfirst(str) { if (typeof str == 'string') { var firstLetter = str.substr(0, 1); return firstLetter.toUpperCase() + str.substr(1); } }

function getTrimList(list) {
    var result = [];
    //console.log(list);
    var trims = list;

    if (trims != null && trims.length > 0) {

        for (var i = 0; i < trims.length; i++) {
            var trim = trims[i];
            // we should convert trim to lower case for filter
            //result.push(ucfirst(trim));
            result.push(trim.toLowerCase());
        }
    }
    return result;
}

function totalmiles(obj) {
    var r = 0;
    for (var i in obj) {
        if (obj[i].miles != undefined)
            r += obj[i].miles;
    }
    return r;
}; // returns total of miles

// get list of unique
function getUnique(data, find) { var result = []; for (var item in data) { if (data[item][find]) { if (!findInArray(result, data[item][find])) { result.push(data[item][find]); } } } return result; }

function containsObject(obj, list) {
    var i;
    for (x in list) {
        if (list.hasOwnProperty(x) && list[x] === obj) {
            return true;
        }
    }

    return false;
}

// ###############################
// NEW FOR OPTION LIST
//function makeOptionList(list, id, setOption) {
//	if (id) {
//		var select = document.getElementById(id);
//		select.options.length = 0;
//		select.options[select.options.length] = new Option('All Options', 0);
//		for (var item in list) {
//	    	select.options[select.options.length] = new Option(ucfirst(item), item);
//		}
//		select.selectedIndex = setOption;            

//	} else {}

//}

function makeOptionList(id, data, fRange, $filter, $dCar, expand, defaultOption) {
    if (id) {
        makeOptionCheckList(id, data, fRange, $filter, $dCar, expand, defaultOption);
    } else { }
}

function makeOptionCheckList(id, data, fRange, $filter, $dCar, expand, defaultOption) {
    var _this = "#" + id;
    var _defaultOptions = defaultOption; //defaultTrim.split(',');
    $(_this).dropdownchecklist("destroy");
    $(_this + " option").remove();
    if (_defaultOptions.length == 0 || (_defaultOptions.length == 1 && (_defaultOptions[0] == '0' || _defaultOptions[0] == undefined))) {
        $(_this).append($("<option></option>").attr('value', '0').attr('selected', 'selected').text('All Options'));
        $.each(data, function(index, obj) {
            $(_this).append($("<option></option>").attr('value', index).text(ucfirst(index)));
        });
    }
    else {
        $(_this).append($("<option></option>").attr('value', '0').text('All Options'));
        $.each(data, function(index, obj) {
            var item = $("<option></option>").attr('value', index).text(ucfirst(index));
            if (_defaultOptions.indexOf(index) >= 0)
                item = item.attr('selected', 'selected');

            $(_this).append(item);
        });
    }

    $(_this).dropdownchecklist({ firstItemChecksAll: true, width: 150, maxDropHeight: 300
            , onComplete: function (selector) {
                _defaultOptions = [];
                $filter.option = {};
                for (i = 0; i < selector.options.length; i++) {
                    if (selector.options[i].selected && (selector.options[i].value != "")) {
                        if (i == 0) {
                            _defaultOptions.push(i);
                            delete $filter.option;
                            break;
                        }
                        $filter.option[selector.options[i].value] = true;
                        _defaultOptions.push(selector.options[i].value);
                    }
                }

                var _defaultTrims = [];
                $filter.title = {}; $filter.title.trim = [];
                for (i = 0; i < $('#trim-filter option').length; i++) {
                    if ($('#trim-filter option')[i].selected && ($('#trim-filter option')[i].value != "")) {
                        if (i == 0) {
                            _defaultTrims.push(i);
                            delete $filter.title;
                            break;
                        }
                        $filter.title.trim.push($('#trim-filter option')[i].value);
                        _defaultTrims.push($('#trim-filter option')[i].value);
                    }
                }

                // #######################
                // NEW FOR DEALERTYPE
                var dealer_select = getChecked('dealertype');
                if (dealer_select.id != 'all') { $filter.dealertype = dealer_select.id; } else { delete $filter.dealertype; }
                // NEW FOR DEALERTYPE
                // #######################

                // check certified
                if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }


                
                // draw chart                
                drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);

                // unset filter
                $filter = {};
            }
            , onItemClick: function (checkbox, selector) {
            }
    });
}


function makeTrimList(id, data, fRange, $filter, $dCar, expand, defaultTrim) {
    if (id) {
        makeTrimCheckList(id, data, fRange, $filter, $dCar, expand, defaultTrim);
    } else { }
}

function makeTrimCheckList(id, data, fRange, $filter, $dCar, expand, defaultTrim) {
    var _this = "#" + id;
    var _defaultTrims = defaultTrim; //defaultTrim.split(',');
    $(_this).dropdownchecklist("destroy");
    $(_this + " option").remove();
    if (_defaultTrims.length == 0 || (_defaultTrims.length == 1 && (_defaultTrims[0] == '0' || _defaultTrims[0] == undefined))) {
        $(_this).append($("<option></option>").attr('value', '0').attr('selected', 'selected').text('All Trims'));
        $.each(data, function(index, obj) {
        $(_this).append($("<option></option>").attr('value', obj).text(ucfirst(obj)));
        });
    }
    else {
        $(_this).append($("<option></option>").attr('value', '0').text('All Trims'));
        $.each(data, function(index, obj) {
        var item = $("<option></option>").attr('value', obj).text(ucfirst(obj));
            if (_defaultTrims.indexOf(obj) >= 0)
                item = item.attr('selected', 'selected');

            $(_this).append(item);
        });
    }

    $(_this).dropdownchecklist({ firstItemChecksAll: true, width: 150, maxDropHeight: 300
            , onComplete: function(selector) {
                _defaultTrims = [];
                $filter.title = {}; $filter.title.trim = [];
                for (i = 0; i < selector.options.length; i++) {
                    if (selector.options[i].selected && (selector.options[i].value != "")) {
                        if (i == 0) {
                            //$filter.title.trim.push(selector.options[i].value);
                            _defaultTrims.push(i);
                            delete $filter.title;
                            break;
                        }
                        $filter.title.trim.push(selector.options[i].value);
                        _defaultTrims.push(selector.options[i].value);
                    }
                }

                var _defaultOptions = [];
                $filter.option = {};
                for (i = 0; i < $('#option-filter option').length; i++) {
                    if ($('#option-filter option')[i].selected && ($('#option-filter option').value != "")) {
                        if (i == 0) {
                            _defaultOptions.push(i);
                            delete $filter.option;
                            break;
                        }
                        $filter.option[$('#option-filter option')[i].value] = true;
                        _defaultOptions.push($('#option-filter option')[i].value);
                    }
                }

                // #######################
                // NEW FOR DEALERTYPE
                var dealer_select = getChecked('dealertype');
                if (dealer_select.id != 'all') { $filter.dealertype = dealer_select.id; } else { delete $filter.dealertype; }
                // NEW FOR DEALERTYPE
                // #######################

                // check certified
                if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }

                // draw chart
                //drawChart($data, 100, $filter, $dCar, expand, 0, 0);
                drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);

                // unset filter
                $filter = {};
            }
            , onItemClick: function(checkbox, selector) {
            }
    });
}

// NEW FOR OPTION LIST
// ###############################



// write sidebar view
function sidebar(i, dcar, data, setOption, setTrim) {
    $filter.miles = i.datapoint[0];
    $filter.price = i.datapoint[1];

    var carResult = filterResults($data, $filter);
    // set selected car for global varaiable $selectedCar
    $selectedCar = carResult[0];
    
    var auto = carResult[0];

    $filter = {};

    var is_certified = function(val) {
        if (!val) {
            return 'No';
        } else {
            return 'Yes';
        }
    };
    
    var m = new metrics(data,$trimList, dcar);
    // get sidebar elements and assign to variables
    console.log('Lime');
    console.log(dcar);
    console.log('End Lime');
    var car = $('#car'),
    //		        onMarket = $('#daysOnMarket'),
    //		        pChange = $('#priceChanges'),
		        mileage = $('#miles'),
		        price = $('#price'),
		        diffM = $('#diffM'),
		        diffP = $('#diffP'),
		        distance = $('#distance'),
		       // certified = $('#certified-span'),
		        seller = $('#seller'),
		        thumb = $('#car-thumb'),
		        address = $('#address');
    var autotraderLink = $('#AutoTraderLink');
    var carscomLink = $('#CarsComLink');


    //		    makeOptionList(m.option_list, 'option-filter', setOption);
    //		    
    //		    makeTrimList(m.trim_list, 'trim-filter', setTrim);

    $('#carInfo').show();
    $('#divNoCarInfo').hide();
    
    if (auto.title.trim == 'Other') {
        car.html(auto.title.year + ' ' + auto.title.make + ' ' + auto.title.model);
    } else {
        car.html(auto.title.year + ' ' + auto.title.make + ' ' + auto.title.model + ' ' + auto.title.trim);
    }
    mileage.html(auto.miles);
    price.html(auto.price);
    thumb.html('<img src="' + auto.thumbnail + '" alt="car-thumbnail" />');
    if (auto.carscom)
        carscomLink.html('<a target="_blank"  href="' + auto.carscomlistingurl + '">CarsCom</a>');
    else {
        carscomLink.html("");
    }
    if (auto.autotrader)
        autotraderLink.html('<a target="_blank"  href="' + auto.autotraderlistingurl + '">AutoTrader</a>');
    else
        autotraderLink.html("");
    distance.html(auto.distance + " miles");
    seller.html(auto.seller);
    address.html(auto.address);
    diffM.html(m.difference(auto.miles, dcar.data[0][0]));
    diffP.html(m.difference(auto.price, dcar.data[0][1]));
    //certified.html(is_certified(auto.certified));
}

// plots object onto FLOT Chart Plugin

function drawChart(list, fRange, filter, dealerCar, expanded, setOption, setTrim) {

    // #########################################################
    // SET OPTION VARIABLE ADDED TO CHART DRAW FUNCTION
    // #########################################################
    //
    // Allows you to set option default value, please
    // make sure all chart draw functions on the graph
    // jquery calls have default_option in the arguments
    //
    // #########################################################
 
    
    if (list != null && list.length != 0) {

        var arr = [];
        var index = 0;
        for (var it = 0; it < list.length; it++) {
            if (list[it].distance <= fRange) {
                arr[index] = list[it];
                index++;

            }
        }


        if (expanded && $trimList != undefined) {
            makeTrimList('trim-filter', $trimList, fRange, filter, dealerCar, expanded, setTrim);
        }

        list = filterResultsWithArray(arr, filter); //filterResults(list, filter);
        // get current filterred list of car
        
        if ($currentFilterredList != undefined)
            $currentFilterredList = list;

        $('#NumberofCarsOnTheChart').text('List of ' + (list.length + 1) + ' Charted Vehicles');
        try {
            $('#market_search_v2_number').text(list.length);
        } catch(e) {

        } 
        if (expanded) {
            // update list with selected range
            //console.log('Liem 1');
            //pop_list(list, fRange, dealerCar);
            try {
                if (isMarketSearch == 1) {
                    if ($('#hdIsList').val() == 1) {
                        pop_list(list, fRange, dealerCar);
                    }
                } else {
                    pop_list(list, fRange, dealerCar);
                }
            } catch (e) {
                pop_list(list, fRange, dealerCar);
            }
        }
        var chart = new MarketChart({
            chartDiv: '#chart',
            sidebar: '#sidebar',
            baseVehicle: dealerCar,
            marketList: list,
            dimensions: [730, 600],
            comparison: true
        });


        $drawDiv.css("background-image", "none");
        $('input#expand').prop('disabled', false);
        $('#filter-wrap input, #filter-wrap select').each(function () {
            $(this).prop('disabled', false);
        });
        //$('#carInfo').show();
        $('.market-info').show();
        $('#graph-title-bar').show();
        $('canvas').show();
        $('.tickLabels').show();
        $('#printable-list').show();
        $('#side - nav - wrap').show();
        //console.log("end draw chart");
    } else {

        $drawDiv.css("background-image", "url('../images/no-data.jpg')");
        $('input#expand').prop('disabled', true);
        $('#filter-wrap input, #filter-wrap select').each(function() {
            $(this).prop('disabled', true);
        });
        //$('#carInfo').hide();
        $('.market-info').hide();
        $('#graph-title-bar').hide();
        $('canvas').hide();
        $('.tickLabels').hide();
        $('#printable-list').hide();
        $('#side - nav - wrap').hide();
    }
    
    $('#carInfo').hide();
    $('#divNoCarInfo').show();
}

// ######################### //
// Object Creation Functions //
// ######################### //

// ######################### //
// Object Creation Functions //
// ######################### //

// metrics object
metrics = function(list,trimlist, dealerCar) {
    if (list.length > 0) {

        this.cars = list.length;
        this.distances = extractVal(list, "distance");
        this.prices = extractVal(list, "price");
        this.miles = extractVal(list, "miles");
        this.totalprice = totalprice(list);
        this.totalmiles = totalmiles(list);
        this.averageprice = Math.round(this.totalprice / this.cars);
        this.averagemile = Math.round(this.totalmiles / this.cars);
        this.comma = function(e) { return addCommas(e); };
        this.difference = function(a, b) { return posNeg(a - b); };
        this.largestPrice = Math.max.apply(Math, this.prices);
        this.largestMiles = Math.max.apply(Math, this.miles);
        this.smallestPrice = Math.min.apply(Math, this.prices);
        this.smallestMiles = Math.min.apply(Math, this.miles);
        this.nearestCar = Math.min.apply(Math, this.distances);
        this.farthestCar = Math.max.apply(Math, this.distances);
        //this.option_list = list[0].option;
        this.trim_list = getTrimList(trimlist);
        this.dealerMiles = dealerCar.data[0][0];
        this.dealerPrice = dealerCar.data[0][1];

        // Conditional Logic in event the
        // chart has only one or no cars

        $('#low').html('$' + addCommas(this.smallestPrice));
        $('#middle').html('$' + addCommas(this.averageprice));
        $('#high').html('$' + addCommas(this.largestPrice));

        if (list.length === 1) {
            this.largestMiles = list[0].miles * 1.5;
            this.smallestMiles = list[0].miles * 0.5;
            if (this.largestMiles < this.dealerMiles) {
                this.largestMiles = this.dealerMiles * 1.5;
            } else if (this.smallestMiles > this.dealerMiles) {
                this.largestMiles = this.dealerMiles * 0.5;
            }
        } else if (list.length === 0) {
            this.largestMiles = 100000;
            this.smallestMiles = 0;
            if (this.largestMiles < this.dealerMiles) {
                this.largestMiles = this.dealerMiles * 1.5;
            } else if (this.smallestMiles > dealerMiles) {
                this.largestMiles = this.dealerMiles * 0.5;
            }
        }
        $('.num-cars').html(list.length);
    } else {
        //alert("No vehicles found");
    }
};



// graph settings object
gCon = function(list, dealerCar) {
    // initiate metrics object
    this.m = new metrics(list,$trimList, dealerCar);
    // math for chart parameters
    this.startPriceRange = this.m.smallestPrice - (this.m.largestPrice * .25);
    this.startMileRange = this.m.smallestMiles;
    this.mileRangeEnd = this.m.largestMiles;
    this.priceRangeEnd = this.m.largestPrice + (this.m.largestPrice * .25);
    this.mileRange = this.mileRangeEnd;
    this.priceRange = this.priceRangeEnd;
};
// Takes market data and calculates
// y-intercept, slope and takes an
// arg of x in this.Regression to
// calculate the y of any x in relation
// to the target market slope.
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
    };
    this.RegressionX = function(y) {
        return (y - this.Intercept) / this.Slope;
    };
};
