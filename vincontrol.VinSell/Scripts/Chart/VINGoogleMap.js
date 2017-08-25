var VINControl = VINControl || {};
VINControl.GoogleMap = VINControl.GoogleMap || {};
VINControl.GoogleMap.GetZoomLevel = function (dealerLongtitude, dealerLatitude) {
    var chartLongtitude = dealerLongtitude;
    var chartLatitude = dealerLatitude;
    var chartZoom = 8;
    if (fRange == 10000) {
        chartZoom = 4;
        chartLongtitude = -97;
        chartLatitude = 38;
    } else if (fRange == 250) {
        chartZoom = 6;
    } else if (fRange == 500) {
        chartZoom = 5;
    }
    return {
        longtitude: chartLongtitude,
        latitude: chartLatitude,
        zoom: chartZoom
    };
};

VINControl.GoogleMap.ConvertHelper = function (map) {
    var convertToMarkerPoints = function (dataResult) {
        var markers = [];
        for (var i = 0; i < dataResult.length; i++) {
            markers.push(convertToMarkerPoint(dataResult[i], map));
        }
        return markers;
    },
        convertToMarkerPoint = function (item, map) {
            var markItem = new google.maps.Marker({
                year: item.title.year,
                make: item.title.make,
                model: item.title.model,
                trim: item.title.trim,
                position: new google.maps.LatLng(parseFloat(item.latitude), parseFloat(item.longtitude)),
                title: item.title.year + " " + item.title.make + " " + item.title.model + " " + item.title.trim,
                imageUrl: item.thumbnail,
                miles: item.miles,
                price: item.price,
                seller: item.seller,
                dealerAddress: item.address,
                distance: item.distance,
                certified: item.certified,
                carscom: item.carscom,
                autotrader: item.autotrader
            });

            google.maps.event.addListener(markItem, 'click', function () {
                var infowindow = new google.maps.InfoWindow({
                    content: getContentDetail(this.title, this.imageUrl, this.miles, this.price, this.seller, this.dealerAddress, this.distance)
                });
                infowindow.open(map, this);
            });
            return markItem;
        },
    getContentDetail = function (title,imageUrl, miles, price, seller, dealerAddress, distance) {
        return "<div>"
                + "<h3>Vehicle Info</h3>"
                + "<input id=\"ListingId\" name=\"ListingId\" type=\"hidden\" value=\"41636\">"
                + "<b><span>" + title + "</span></b>"
                + "<br>"
                + "<span ><img width=\"100\" height=\"100\" src=\"" + imageUrl + "\" alt=\"car-thumbnail\"></span>"
                + "<br>"
                + "<span></span>"
                + "<br>"
                + "<span></span>"
                + "<br>"
                + "Miles: <span>" + miles + "</span> <br>"
                + "Price: $<span >" + price + "</span> <br>"
                + "Seller: <span >" + seller + "</span>"
                + "<br>"
                + "Address: <span >" + dealerAddress + "</span>"
                + "<br>"
                + "Distance: <span >" + distance + " mi</span>"
                + "<br>"
                + "<br>"
                + "</div>";
    };
    return { convertToMarkerPoints: convertToMarkerPoints, convertToMarkerPoint: convertToMarkerPoint
    };
};

