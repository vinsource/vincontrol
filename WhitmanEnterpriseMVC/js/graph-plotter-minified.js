$drawDiv=$('#placeholder');$filter={};function findInArray(arr,val){for(item in arr){if(arr[item]==val){return true;}}}

	// check if expanded element in value
	function expanded(a) {if(a === '?e=1'){ return true; } else { return false; } }

	// capitalize first letter
	// function capitalize(s) {return s.charAt(0).toUpperCase() + s.slice(1);}
	// string to upper
	function ucfirst(str) {
	    if (typeof str == 'string') {var firstLetter = str.substr(0, 1); return firstLetter.toUpperCase() + str.substr(1);}
	    return firstLetter.toUpperCase() + str.substr(1);
	}

	function getTrimList(list) {
		var result = [];
		var trims = list[0].trims;
		for(var i=0;i<trims.length;i++) {
			var trim = trims[i];
			result.push(ucfirst(trim));
		}
		return result;
	}

	function totalmiles(obj)
	{var r = 0; for(var i in obj) {r+=obj[i].miles;
	            } return r; }; // returns total of miles
	
	// get list of unique
	function getUnique(data, find) {var result = [];for (var item in data) {if (data[item][find]) {if (!findInArray(result, data[item][find])) {result.push(data[item][find]);}}}return result;}



	// ###############################
	// NEW FOR OPTION LIST
	function makeOptionList(list, id, setOption) {
		if (id) {
			var select = document.getElementById(id);
			select.options.length = 0;
			select.options[select.options.length] = new Option('All Options', 0);
			for (var item in list) {
		    	select.options[select.options.length] = new Option(ucfirst(item), item);
			}
			select.selectedIndex = setOption;
		} else {}
		
	}
	function makeTrimList(list, id, setTrim) {
		if (id) {
			var select = document.getElementById(id);
			select.options.length = 0;
			select.options[select.options.length] = new Option('All Trims', 0);
			for (var item in list) {
		    	select.options[select.options.length] = new Option(ucfirst(list[item]), list[item]);
			}
			select.selectedIndex = setTrim;
		} else {}
		
	}
	// NEW FOR OPTION LIST
	// ###############################



	// write sidebar view
		function sidebar(i, dcar, data, setOption, setTrim) {
		    $filter.miles = i.datapoint[0];
		    $filter.price = i.datapoint[1];

		    var carResult = filterResults($data, $filter);
		    var auto = carResult[0];

		    $filter = { };

		    var is_certified = function(val) {
		        if (!val) {
		            return 'No';
		        } else {
		            return 'Yes';
		        }
		    };
		    //console.log(data);
		  var m = new metrics(data, dcar);
		    // get sidebar elements and assign to variables
		    var car = $('#car'),
		        onMarket = $('#daysOnMarket'),
		        pChange = $('#priceChanges'),
		        mileage = $('#miles'),
		        price = $('#price'),
		        diffM = $('#diffM'),
		        diffP = $('#diffP'),
		        distance = $('#distance'),
		        //certified = $('#certified-span'),
		        seller = $('#seller'),
		        thumb = $('#car-thumb'),
		        address = $('#address');
		        var autotraderLink = $('#AutoTraderLink');
		        var carscomLink = $('#CarsComLink');
		    

		    makeOptionList(m.option_list, 'option-filter', setOption);
		    makeTrimList(m.trim_list, 'trim-filter', setTrim);
		    car.html('<a target="_blank"  href="' + auto.carscomlistingurl + '">' + auto.title.year + ' ' + auto.title.make + ' ' + auto.title.model + ' ' + auto.title.trim + '</a>');
		    mileage.html(auto.miles);
		    price.html(auto.price);
		    thumb.html('<a target="_blank" href="' + auto.carscomlistingurl + '"><img src="' + auto.thumbnail + '" alt="car-thumbnail" /></a>');
		    if(auto.carscom)
		        carscomLink.html('<a target="_blank"  href="' + auto.carscomlistingurl + '">CarsCom</a>');
		    else {
		        carscomLink.html("");
		    }
		    if(auto.autotrader)
		        autotraderLink.html('<a target="_blank"  href="' + auto.autotraderlistingurl + '">AutoTrader</a>');
		    else
		        autotraderLink.html("");
		    distance.html(auto.distance + " miles");
		    seller.html(auto.seller);
		    address.html(auto.address);
		    diffM.html(m.difference(auto.miles, dcar.data[0][0]));
		    diffP.html(m.difference(auto.price, dcar.data[0][1]));
		    certified.html(is_certified(auto.certified));
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

	    if (list!=null && list.length > 0) {

	        var m = new metrics(list, dealerCar);
	        var arr = [];
	        for (var car in list) {
	            if (list[car]['price'] <= m.averageprice * 2.5) {
	                arr.push(list[car]);
	            }
	        }
	        list = arr;

	        // ###############################
	        // NEW FOR OPTION LIST

	        // add options to dropdown
	        makeOptionList(m.option_list, 'option-filter', setOption);
	        makeTrimList(m.trim_list, 'trim-filter', setTrim);

	        // NEW FOR OPTION LIST
	        // ###############################


	        // initialize variables
	        var dealerCar = dealerCar,
	            carRange = [],
	            carDistances = [],
	            redraw = false,
	            carPrices = [],
	            carMiles = [],
	            allCars = [],
	            percent = setPrecent(),
	            config = new gCon(list, dealerCar),
	            m = new metrics(list, dealerCar);

	        // filter results based on form selection
	        var list = filterResults(list, filter);
	        m = new metrics(list, dealerCar);
	        config = new gCon(list, dealerCar);

	        // var market = findMarketLocation(
	        // 		[m.averagemile, m.averageprice],
	        // 		[config.startMileRange, config.mileRangeEnd],
	        // 		[dealerCar.data[0][0],dealerCar.data[0][1]]
	        // 	);

	        // console.log(market);


	        // ###############################
	        // NEW FOR LISTING PRINTOUT

	        // populate list of vehicles
	        var vehi_list = pop_list(list, fRange);

	        // NEW FOR LISTING PRINTOUT
	        // ###############################


	        // set range divisions
	        var ranges = setRange(list);

	        // set global graph options
	        if (expanded) {

	            var d100 = { data: [], points: { radius: 3 } },
	                d250 = { data: [], points: { radius: 3 } },
	                d500 = { data: [], points: { radius: 2 } },
	                dNation = { data: [], points: { radius: 2 } };

	            options = {
	                legend: { show: false },
	                series: { color: '#000' },
	                lines: { show: false },
	                points: { show: true, radius: 4, fill: true, fillColor: false },
	                xaxis: { show: true, label: 'Miles', min: config.startMileRange, max: config.mileRangeEnd, tickFormatter: mileFormat },
	                yaxis: { show: true, label: 'Price', min: config.startPriceRange, max: config.priceRangeEnd, tickFormatter: priceFormat },
	                grid: { hoverable: false, clickable: true, backgroundColor: '#fdfdfd' }
	            };
	        } else {

	            var d100 = { data: [], points: { radius: 3 } },
	                d250 = { data: [], points: { radius: 3 } },
	                d500 = { data: [], points: { radius: 2 } },
	                dNation = { data: [], points: { radius: 2 } };

	            options = {
	                legend: { show: false },
	                series: { color: '#000' },
	                lines: { show: false },
	                points: { show: true, radius: 4, fill: true, fillColor: false },
	                xaxis: { show: true, label: 'Miles', min: config.startMileRange, max: config.mileRangeEnd, tickFormatter: mileFormat },
	                yaxis: { show: true, label: 'Price', min: config.startPriceRange, max: config.priceRangeEnd, tickFormatter: priceFormat },
	                grid: { hoverable: false, clickable: false, backgroundColor: '#fafafa' }
	            };
	        }

	        // generate data sets from range divisions.
	        for (var i = 0; i < ranges[0].length; i++) {
	            d100.data.push([ranges[0][i].miles, ranges[0][i].price]);
	        }
	        for (var i = 0; i < ranges[1].length; i++) {
	            d250.data.push([ranges[1][i].miles, ranges[1][i].price]);
	        }
	        for (var i = 0; i < ranges[2].length; i++) {
	            d500.data.push([ranges[2][i].miles, ranges[2][i].price]);
	        }
	        for (var i = 0; i < ranges[3].length; i++) {
	            dNation.data.push([ranges[3][i].miles, ranges[3][i].price]);
	        }

	        // Set Range Fields
	        var target = {
	            label: 'range',
	            points: { show: false },
	            lines: { show: true, fill: false,  },
	            color: 'green',
	            grid: { hoverable: false, clickable: false },

	            data: [[config.startMileRange, m.averageprice + (m.averageprice * 0.2)], [config.mileRangeEnd, m.averageprice - (m.averageprice * 0.2)]]
	        };
	        var good = {
	            label: 'range',
	            points: { show: false },
	            lines: { show: true, fill: true },
	            color: 'lime',
	            grid: { hoverable: false, clickable: false },

	            data: [
	                [config.startMileRange, m.averageprice + (m.averageprice * 0.225)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * 0.20) + (m.averageprice * 0.025)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * 0.225)],
	                [config.startMileRange, m.averageprice + (m.averageprice * 0.20) - (m.averageprice * 0.025)],
	                [config.startMileRange, m.averageprice + (m.averageprice * 0.225)]
	            ]
	        };
	        var moderate = {
	            label: 'range',
	            points: { show: false },
	            lines: { show: true, fill: true },
	            color: 'yellow',
	            grid: { hoverable: false, clickable: false },

	            data: [
	                [config.startMileRange, m.averageprice + (m.averageprice * .275)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * .20) + (m.averageprice * .075)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * .275)],
	                [config.startMileRange, m.averageprice + (m.averageprice * .20) - (m.averageprice * .075)],
	                [config.startMileRange, m.averageprice + (m.averageprice * .275)]
	            ]
	        };
	        var bad = {
	            label: 'range',
	            points: { show: false },
	            lines: { show: true, fill: true },
	            color: 'yellow',
	            grid: { hoverable: false, clickable: false },

	            data: [
	                [config.startMileRange, m.averageprice + (m.averageprice * .425)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * .20) + (m.averageprice * .225)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * .425)],
	                [config.startMileRange, m.averageprice + (m.averageprice * .20) - (m.averageprice * .225)],
	                [config.startMileRange, m.averageprice + (m.averageprice * .425)]
	            ]
	        };
	        var horrid = {
	            label: 'range',
	            points: { show: false },
	            lines: { show: true, fill: true },
	            color: 'red',
	            grid: { hoverable: false, clickable: false },

	            data: [
	                [config.startMileRange, m.averageprice + (m.averageprice * .7)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * .20) + (m.averageprice * .5)],
	                [config.mileRangeEnd, m.averageprice - (m.averageprice * .7)],
	                [config.startMileRange, m.averageprice + (m.averageprice * .20) - (m.averageprice * .5)],
	                [config.startMileRange, m.averageprice + (m.averageprice * .7)]
	            ]
	        }; // create data set array
	        dataArr = [horrid, bad, good, target];

	        // append data array with range information
	        if (fRange == 100) {
	            dataArr.push(d100);
	        } else if (fRange == 250) {
	            dataArr.push(d100);
	            dataArr.push(d250);
	        } else if (fRange == 500) {
	            dataArr.push(d100);
	            dataArr.push(d250);
	            dataArr.push(d500);
	        } else {
	            dataArr.push(d100);
	            dataArr.push(d250);
	            dataArr.push(d500);
	            dataArr.push(dNation);
	        }

	        // append data array with dealer car information
	        var dcarx = dealerCar.data[0][0];
	        var dcary = dealerCar.data[0][1];
	        var dealer_car_chart = dealerCar;
	        if (dcarx > config.mileRangeEnd) {
	            dealer_car_chart.data[0][0] = config.mileRangeEnd - 20;
	        } else if (dcarx < config.startMileRange) {
	            dealer_car_chart.data[0][0] = config.startMileRange + 20;
	        }

	        if (dcary > config.priceRangeEnd) {
	            dealer_car_chart.data[0][1] = config.priceRangeEnd - 20;
	        } else if (dcary < config.startPriceRange) {
	            dealer_car_chart.data[0][1] = config.startPriceRange + 20;
	        }
	        dataArr.push(dealer_car_chart);

	        // plot graph
	        $.plot($drawDiv, dataArr, options);
	    }
	

		else {
	        // if there are no results, print out placeholder
	        $drawDiv.css("background-image", "url('../images/no-data.jpg')");
	        $('#graph #graphButton').prop('disabled', true);
	        $('#filter-wrap input, #filter-wrap select').each(function() {
	            $(this).prop('disabled', true);
	        });
	        $('#carInfo').css('display', 'none');
	        $('#graph-title-bar').css('display', 'none');
	    }

	    // ######################### //
	// Object Creation Functions //
	// ######################### //

	// metrics object
	metrics = function (list, dealerCar) {
	
	 	/* returns list length */ 					this.cars = list.length;									
		/* returns array of distances in list */ 	this.distances = extractVal(list, "distance");				
		/* returns array of prices */ 				this.prices = extractVal(list, "price");					
		/* returns array of miles */ 				this.miles = extractVal(list, "miles");						
		/* returns total of prices */ 				this.totalprice = totalprice(list);							
		/* returns total of miles */ 				this.totalmiles = totalmiles(list);							
		/* returns total of all prices */ 			this.averageprice = Math.round(this.totalprice/this.cars);			
		/* returns total of all miles */ 			this.averagemile = Math.round(this.totalmiles/this.cars);
		/* returns num value with commas */ 		this.comma = function (e) {return addCommas(e); };
		/* difference */							this.difference = function (a, b) {return posNeg(a-b); };			
		/* returns largest price */ 				this.largestPrice = Math.max.apply(Math, this.prices);		
		/* returns largest mileage */ 				this.largestMiles = Math.max.apply(Math, this.miles);		
		/* returns smallest price */ 				this.smallestPrice = Math.min.apply(Math, this.prices);		
		/* returns smallest mileage */ 				this.smallestMiles = Math.min.apply(Math, this.miles);		
		/* returns closest car */ 					this.nearestCar = Math.min.apply(Math, this.distances);		
		/* returns farthest car */ 					this.farthestCar = Math.max.apply(Math, this.distances);
	    if (typeof list[0].option != "undefined") {
	        		this.option_list = list[0].option;
	    }
		/* returns list of options */ 		
		/* returns list of trims */					this.trim_list = getTrimList(list);
													this.dealerMiles = dealerCar.data[0][0];
													this.dealerPrice = dealerCar.data[0][1];

		// Conditional Logic in event the
		// chart has only one or no cars
		if (list.length === 1) {
			this.largestMiles = list[0].miles*1.5;
			this.smallestMiles = list[0].miles*0.5;
			if (this.largestMiles < this.dealerMiles) {
				this.largestMiles = this.dealerMiles*1.5;
			} else if (this.smallestMiles > this.dealerMiles) {
				this.largestMiles = this.dealerMiles*0.5;
			}
		} else if (list.length === 0) {
			this.largestMiles = 100000;
			this.smallestMiles = 0;
			if (this.largestMiles < this.dealerMiles) {
				this.largestMiles = this.dealerMiles*1.5;
			} else if (this.smallestMiles > dealerMiles) {
				this.largestMiles = this.dealerMiles*0.5;
			}
		}

		};	
	

	// graph settings object
	gCon = function(list, dealerCar) {
			// initiate metrics object
			this.m = new metrics(list, dealerCar);
			// math for chart parameters
			this.startPriceRange = this.m.smallestPrice-(this.m.largestPrice*.25);
			this.startMileRange = this.m.smallestMiles;
			this.mileRangeEnd = this.m.largestMiles;
			this.priceRangeEnd = this.m.largestPrice+(this.m.largestPrice*.25);
			this.mileRange = this.mileRangeEnd;
			this.priceRange = this.priceRangeEnd;
		};

