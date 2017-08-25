/* accepts parameters
* h  Object = {h:x, s:y, v:z}
* OR
* h, s, v
*/
function hsl2rgb(h, s, l) {
    var m1, m2, hue;
    var r, g, b
    s /= 100;
    l /= 100;
    if (s == 0)
        r = g = b = (l * 255);
    else {
        if (l <= 0.5)
            m2 = l * (s + 1);
        else
            m2 = l + s - l * s;
        m1 = l * 2 - m2;
        hue = h / 360;
        r = HueToRgb(m1, m2, hue + 1 / 3);
        g = HueToRgb(m1, m2, hue);
        b = HueToRgb(m1, m2, hue - 1 / 3);
    }
    return { r: Math.floor(r), g: Math.floor(g), b: Math.floor(b) };
}

function HueToRgb(m1, m2, hue) {
    var v;
    if (hue < 0)
        hue += 1;
    else if (hue > 1)
        hue -= 1;

    if (6 * hue < 1)
        v = m1 + (m2 - m1) * hue * 6;
    else if (2 * hue < 1)
        v = m2;
    else if (3 * hue < 2)
        v = m1 + (m2 - m1) * (2 / 3 - hue) * 6;
    else
        v = m1;

    return 255 * v;
}

function finder(cmp, arr, attr) {
    var val = arr[0][attr];
    for (var i = 1; i < arr.length; i++) {
        val = cmp(val, arr[i][attr]);
    }
    return val;
}

function sumOf(prop) {

}



// METRICS
function Metrics(list) {
    //console.log(list);
    this.list = list;
    this.numCars = this.list.length;

    this.getTotal = function(type) {
        var sum = 0;
        for (var i in this.list) {
            sum += this.list[i][type];
        }
        return sum;
    };

    this.getHighest = function(type) {
        return finder(Math.max, this.list, type);
    };

    this.getLowest = function(type) {
        return finder(Math.min, this.list, type);
    };

    this.average = function(type) {
        return Math.floor(this.getTotal(type) / this.numCars);
    };

}

// REGRESSION
// Takes market data and calculates
// y-intercept, slope and takes an
// arg of x in this.Regression to
// calculate the y of any x in relation
// to the target market slope.
function Regression(arr) {
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

    this.getY = function(x) {
        return this.Slope * x + this.Intercept;
    };

    this.RegressionY = function(x) {
        return this.Intercept + (this.Slope * x);
    };

    this.RegressionX = function(y) {
        return (y - this.Intercept) / this.Slope;
    };

}

function difference(a, b) { return Math.abs(a - b) }

function updateSidebar(data, el, dCar) {

    var commas = d3.format(',');


    el.html("");
    var invert = 0 - 1;

    // selected car

    var selectedCar = $(document.createElement('div')).appendTo(el)
        .attr("class", 'selected-vehicle');

    $(document.createElement('b')).appendTo(selectedCar)
          .text(data.title.year + ' ' + data.title.make + ' ' + data.title.model + ' ' + data.title.trim);
    var imgDiv = $(document.createElement('div'))
          .appendTo(selectedCar)
          .attr("style", " text-align: center;");

    $(document.createElement('img')).appendTo(imgDiv)
          .attr("src", data.thumbnail)
          .attr("style", 'width: 80%;');

    if (data.autotrader) {
        $(document.createElement('hl'));
        $(document.createElement('a')).appendTo(selectedCar)
            .attr('href', data.autotraderlistingurl).attr('target', '_blank')
            .text('AutoTrader Listing');
    } else if (data.carscom) {
        $(document.createElement('a')).appendTo(selectedCar)
            .attr('href', data.carscomlistingurl).attr('target', '_blank')
            .text('Cars.com Listing');
    }

    var diffMiles = data.miles - dCar.miles;
    var diffPrice = data.price - dCar.price;

    var colorPrice = $(document.createElement('div'))
          .html('Price: $' + commas(data.price) + ' (' + posNeg(diffPrice) + ')');
    var colorMiles = $(document.createElement('div'))
          .html('Miles: ' + commas(data.miles) + 'mi (' + posNeg(diffMiles) + ')');

    if (diffMiles > 0) { colorMiles.attr("class", 'red'); } else { colorMiles.attr("class", 'green'); }
    if (diffPrice < 0) { colorPrice.attr("class", 'green'); } else { colorMiles.attr("class", 'red'); }

    colorPrice.appendTo(selectedCar);
    colorMiles.appendTo(selectedCar);
    $(document.createElement('div'))
          .attr('class', 'info')
          .html('Seller: ' + data.seller)
          .appendTo(selectedCar);
    $(document.createElement('div'))
          .attr('class', 'info')
          .html('Address: ' + data.address)
          .appendTo(selectedCar);
    $(document.createElement('div'))
          .attr('class', 'info')
          .html('Distance: ' + data.distance + 'mi')
          .appendTo(selectedCar);

    // dealers car
    var baseCar = $(document.createElement('div')).appendTo(el)
        .attr("class", 'base-vehicle');

    $(document.createElement('b')).appendTo(baseCar)
          .text(dCar.title.year + ' ' + dCar.title.make + ' ' + dCar.title.model + ' ' + dCar.title.trim);

    var dMiles = dCar.miles;
    var dPrice = dCar.price;
    var dPriceDiv = $(document.createElement('div'))
          .html('Price: $' + commas(dCar.price));
    var dMilesDiv = $(document.createElement('div'))
          .html('Miles: ' + commas(dCar.miles) + 'mi');

    dPriceDiv.appendTo(baseCar);
    dMilesDiv.appendTo(baseCar);

}

function posNeg(num) {
    if (num > 0) {
        return '+' + num;
    } else {
        return num;
    }
}