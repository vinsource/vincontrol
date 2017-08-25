var VINControl = VINControl || {};
VINControl.Chart = VINControl.Chart || {};

VINControl.Chart.MarketMapping = function() {
    var listingIdField = 0;
    var vinField = 1;
    var yearField = 2;
    var makeField = 3;
    var modelField = 4;
    var trimField = 5;
    //var interiorColorField = 6;
    var exteriorColorField = 7;
    var milesField = 8;
    var priceField = 9;
    var latitudeField = 10;
    var longitudeField = 11;
    var autotraderField = 12;
    var autotraderDealerIdField = 13;
    var autotraderListingIdField = 25;
    var carscomField = 14;
    var carsComListingIdField = 15;
    var carfaxField = 16;
    //var imagesField = 17;
    var sellerAddressField = 18;
    var sellerNameField = 19;
    var thumbnailField = 20;
    //var istargetCarField = 21;
    var franchiseField = 22;
    var uptimeField = 23;
    var distanceField = 24;
    var certified = 26;
    var commercialtruckField = 27;
    var commercialtruckUrl = 28;
    var bodyStyleField = 29;
    var bodyType = 30;
    var carmax = 31;
    var carmaxListingId = 32;
    var highlighted = 33;
    var initializeCar = function(item) {
        var $car = {
            title: // year make model
            {
                year: item[yearField],
                make: ChartHelper.ucfirst(item[makeField]),
                model: ChartHelper.ucfirst(item[modelField]),
                trim: ChartHelper.ucfirst(item[trimField]),
                bodyStyle: ChartHelper.ucfirst(item[bodyStyleField]),
            },
            vin: item[vinField],
            extcolor: item[exteriorColorField],
            distance: item[distanceField],
            trim: item[trimField],
            bodyStyle: item[bodyStyleField],
            thumbnail: (item[thumbnailField] == "" || item[thumbnailField] == "http://www.autotraderstatic.com/img/blank_dot.gif?v=3.67.262267") ? "http://vincontrol.com/alpha/no-images.jpg" : item[thumbnailField],
            carscomlistingurl: getCarscomUrl(item[carsComListingIdField]), // img string, comma deliminated
            commercialTruckListingURL: item[commercialtruckUrl],
            dealertype: item[franchiseField], // dealertype, can be franchise or independant
            seller: item[sellerNameField],
            address: item[sellerAddressField],
            miles: item[milesField],
            price: item[priceField],

            uptime: item[uptimeField], // time on cars.com, autotrader.com and ebay
            carscom: item[carscomField],
            autotrader: item[autotraderField],
            commercialtruck: item[commercialtruckField],
            carfax: item[carfaxField],
            autotraderlistingurl: getAutoTraderUrl(item[autotraderDealerIdField], item[autotraderListingIdField]),
            listingid: item[listingIdField],
            longtitude: item[longitudeField],
            latitude: item[latitudeField],
            certified: item[certified],
            highlighted: item[highlighted],
            bodyType: item[bodyType],
            carmax: item[carmax],
            carmaxlistingurl: getCarMaxUrl(item[carmaxListingId]),
        }; 
       
        return $car;
    },
        initializeCurrentCar = function(target, carsOnMarket) {
            var $car = {
                title: // year make model
                {
                    year: target != null ? target.title.year : '',
                    make: target != null ? ChartHelper.ucfirst(target.title.make) : '',
                    model: target != null ? ChartHelper.ucfirst(target.title.model) : '',
                    trim: target != null ? ChartHelper.ucfirst(target.title.trim) : '',
                    bodyStyle: target != null ? ChartHelper.ucfirst(target.title.bodyStyle) : ''
                },
                vin: target != null ? target.vin : '',
                extcolor: '',
                distance: target != null ? target.distance : '',
                option: '', // options string, comma deliminated
                trim: target != null ? target.trim : '',
                bodyStyle: target != null ? (target.bodyStyle) : '',
                trims: '',
                thumbnail: target != null ? target.thumbnail : '',
                carscomlistingurl: '', // img string, comma deliminated
                dealertype: '', // dealertype, can be franchise or independant
                seller: target != null ? target.seller.sellername : '',
                address: target != null ? target.seller.selleraddress : '',
                miles: target != null ? target.mileage : '',
                price: target != null ? target.salePrice : '',
                certified: target != null ? target.certified : '', // certified 0/1 (false/true)
                highlighted: false,
                uptime: '', // time on cars.com, autotrader.com and ebay
                carscom: false,
                autotrader: false,
                carmax: false,
                commercialtruck: target != null ? target.commercialtruck : false,
                carfax: '',
                autotraderlistingurl: '',
                carmaxlistingurl: '',
                listingid: target != null ? target.listingId : '',
                label: '',
                color: '#003cff',
                shadowSize: 5,
                points: {
                    radius: 10,
                    fillColor: 'white'
                },
                data: target != null ? [[target.mileage /* miles */, target.salePrice /* price */]] : [0, 0],
                clickable: false,
                draggabley: typeof(isEmployee) !== 'undefined' && (isEmployee.toUpperCase() == 'FALSE'),
                draggablex: false,
                ranking: target != null ? target.ranking : '',
                carsOnMarket: carsOnMarket,
                longtitude: target != null ? target.longtitude : '',
                latitude: target != null ? target.latitude : '',
                bodyType: target.title.bodyStyle

            }; // after construction add object to array
            //console.log('-----------------');
            //console.log($car);
            //console.log('-----------------');
            return $car;
        },
        getCarscomUrl = function(carId) {
            return "http://www.cars.com/go/search/detail.jsp?listingId=" + carId;
        },
        getAutoTraderUrl = function(dealerId, carId) {
            return "http://www.autotrader.com/fyc/vdp.jsp?car_id=" + carId;
        };
        getCarMaxUrl = function(carId) {
            return "http://www.carmax.com/enus/view-car/default.html?id=" + carId;
        };

    return { InitializeCar: initializeCar, InitializeCurrentCar: initializeCurrentCar};
};

VINControl.Chart.Marketmetrics = function (list, dealerCar) {
    if (list.length > 0) {

        this.cars = list.length;
        this.distances = ChartHelper.extractVal(list, "distance");
        this.prices = ChartHelper.extractVal(list, "price");
        this.miles = ChartHelper.extractVal(list, "miles");
        this.totalprice = ChartHelper.totalprice(list);
        this.totalmiles = totalmiles(list);
        this.averageprice = Math.round(this.totalprice / this.cars);
        this.averagemile = Math.round(this.totalmiles / this.cars);
        this.comma = function (e) { return ChartHelper.addCommas(e); };
        this.difference = function (a, b) { return ChartHelper.posNeg(a - b); };
        this.largestPrice = Math.max.apply(Math, this.prices);
        this.largestMiles = Math.max.apply(Math, this.miles);
        this.smallestPrice = Math.min.apply(Math, this.prices);
        this.smallestMiles = Math.min.apply(Math, this.miles);
        this.nearestCar = Math.min.apply(Math, this.distances);
        this.farthestCar = Math.max.apply(Math, this.distances);
        this.option_list = list[0].option;
        this.dealerMiles = dealerCar == null ? 0 : dealerCar.data[0][0];
        this.dealerPrice = dealerCar == null ? 0 : dealerCar.data[0][1];

        // Conditional Logic in event the
        // chart has only one or no cars


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
    } else {
        //alert("No vehicles found");
    }
};
