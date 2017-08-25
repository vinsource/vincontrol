


// Populate list of vehicles
function plotSelectedPoint(row) {
    var index = row.id;
    $('tr').each(function() {
    $(this).removeClass('highlightselected');
        if ($(this).attr('id') == index) {
            $(this).addClass('highlightselected');
        }
    });
    VINControl.Chart.SideBar.updateSideBar(index, $data, $dCar);
    //VINControl.Chart.SideBar.refreshSidebar(index, $dCar, $data);

}

function between() { }

var ChartHelper =
    {
//returns total of prices
        totalprice: function(obj) {   
    var sum = 0;
    var i = obj.length;
    while (--i >= 0) {
        if (obj[i].price != undefined) {
            sum += obj[i].price;
        }
    }
    return sum;
},
// returns random franchise/independant dealer designation
        //randTrueFalse: function() { if (Math.floor((Math.random() * 2) + 1) == 1) { return 'franchise'; } else { return 'independant'; } },
// Returns formatted mileage
        mileFormat: function(v, axis) {
    var miles = v.toFixed(axis.tickDecimals);
    return ChartHelper.abbrNum(miles, 0) + " miles";
},
// Returns formatted price
    //    priceFormat: function(v, axis) {
    //var price = v.toFixed(axis.tickDecimals);
    //return '$' + abbrNum(price, 0);
    //    },
        // get checked elements in name group
getChecked:function(name) { var result = ''; $('input[name="' + name + '"]').each(function() { if (this.checked) { result = this; } }); return result; }
// give positive number indicator
, posNeg: function (x) {
    if (x > 0) {
        return '+' + x;
    } else {
        return x;
    }
}
// random range generator (for test data set)
//,randomFromTo:function(from, to) {
//    return Math.round(Math.random() * (to - from + 1) + from);
//}
// checks if x is between min/max range.
//,between:function(x, min, max) {
//    return x >= min && x <= max;
//}
// add commas to number string
,addCommas:function(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;

    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
        //x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }

    return x1 + x2;
}
// abbreviate numbers
,abbrNum:function(number, decPlaces) {
    decPlaces = Math.pow(10, decPlaces);
    var abbrev = ["k", "m", "b", "t"];

    for (var i = abbrev.length - 1; i >= 0; i--) {
        var size = Math.pow(10, (i + 1) * 3);

        if (size <= number) {
            number = Math.round(number * decPlaces / size) / decPlaces;
            number += abbrev[i];
            break;
        }

    }

    return number;
}
// split comma strings
//,splitString:function (string) { return string.split(","); }
// array of certain value
,extractVal: function (list, val) {
    var arr = [];

    for (var a = 0; a < list.length; a++) {
        for (var b in list[a]) {
            if (b == val) {
                arr.push(list[a][b]);
            }
        }
    }

    return arr;
}
        ,// Find In Array
//findInArray: function(arr, val) { for (var item in arr) { if (arr[item] == val) { return true; } } },

// check if expanded element in value
expanded: function(a) { if (a === '?e=1') { return true; } else { return false; } },

// capitalize first letter
// function capitalize(s) {return s.charAt(0).toUpperCase() + s.slice(1);}
// string to upper
ucfirst: function(str) {
    if (typeof str == 'string') {
        var firstLetter = str.substr(0, 1);
        return firstLetter.toUpperCase() + str.substr(1);
    } else return null;
}

// percentage ranges
//,setPrecent: function() {
//    var p = [];
//    var i = 0;
//    while (i <= 1) {
//        i = Math.round(i * 100) / 100;
//        p.push(i);
//        i = i + .1;
//        i = Math.round(i * 100) / 100;
//    }
//    return p;
//}
};

function filterResultsWithArray(l, f) {
   
    var r = [];
    // loop through each array element
    for (var i = 0; i < l.length; i++) {
        var failedMatch = false; //flag
        // search each object item for each filter setting
        for (var set in f) {
            //console.log(set);
            if (typeof (f[set]) == 'object') {
                for (var prop in f[set]) {
                    
                    if (prop == 'trim') {
                        if (f[set][prop].length == 0) continue;
                        if (f[set].hasOwnProperty(prop) && l[i][set].hasOwnProperty(prop) && f[set][prop].indexOf(l[i][set][prop].toLowerCase()) < 0) {
                            failedMatch = true;
                            break;
                        }
                    }
                    else if (prop == 'bodyStyle') {
                        if (f[set][prop].length == 0) continue;
                        console.log(f[set][prop]);
                        console.log(l[i][set][prop].toLowerCase());
                        
                        if (f[set].hasOwnProperty(prop) && l[i][set].hasOwnProperty(prop) && f[set][prop].indexOf(l[i][set][prop].toLowerCase()) < 0) {
                            failedMatch = true;
                            break;
                        }
                    }

                    else
                    {
                        if (f[set].hasOwnProperty(prop) && l[i][set].hasOwnProperty(prop) && l[i][set][prop] != f[set][prop]) {
                            failedMatch = true;
                            break;
                        } 
                    }
                }
            } else if (set == 'marketSource') {
                if (f.hasOwnProperty(set) ) {

                    if (f[set] == 'carscom') {
                        if (!l[i].carscom) {
                            failedMatch = true;
                            break;
                        }
                    }
                    else if (f[set] == 'autotrader') {
                        if (!l[i].autotrader) {
                            failedMatch = true;
                            break;
                        }
                    }
                    else if (f[set] == 'carmax') {
                        if (!l[i].carmax) {
                            failedMatch = true;
                            break;
                        }
                    }
                    else if (f[set] == 'commercialtruck') {
                        if (!l[i].commercialtruck) {
                            failedMatch = true;
                            break;
                        }
                    }
                }
            }
            
            else {

                if (f.hasOwnProperty(set) && l[i].hasOwnProperty(set) && l[i][set] != f[set]) {
                    failedMatch = true;
                    break;
                }
            }
        }
        // if flag not changed, push this item.
        if (!failedMatch) { r.push(l[i]); }
    }
   
    return r;
}
