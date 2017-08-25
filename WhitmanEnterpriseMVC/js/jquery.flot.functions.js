// ################ //
// Chart Functions  //
// ################ //



// ################################################
// NEW FOR VEHICLE LIST PRINTOUT

// Populate list of vehicles
function plotSelectedPoint(row) {
    var index = row.id;
    $('tr').each(function() {
    $(this).removeClass('highlightselected');
        if ($(this).attr('id') == index) {
            $(this).addClass('highlightselected');
        }
    });

    sidebar_update(index, $data, $dCar);

}

/******* GENERATE GRID VIEW  *********/
function pop_smalllist(list, range, dcar) {
    var extraList = [];
    for (var it = 0; it < list.length; it++)
        extraList[it] = list[it];
    if (extraList.length > 0)
        extraList[extraList.length] = dcar;

    // make sure we have table with id 'tblVehicles' exists
    if ($("table#tblVehicles") == undefined || $("table#tblVehicles") == null)
        return false;

    var title = [];
    var filteredList = [];
    $("table#tblVehicles tbody").html("");
    for (var prop in list[0]) {
        if (prop != 'imgs' && prop != 'uptime' && prop != 'thumbnail' && prop != 'carscomlistingurl'   && prop != 'autotraderlistingurl') {
            title.push(prop);
            if (prop === 'title') {
            } else if (prop === 'option') {
            } else if (prop === 'dealertype' || prop === 'address' || prop === 'vin' || prop === 'extcolor' || prop === 'listingid') {
            } else {
            }
        }
    }

    var count = 1;
    for (var i = 0; i < extraList.length; i++) {
        var row = document.createElement('tr');
        row.setAttribute("id", i);
        row.setAttribute("name", "tableRow");
        //        row.setAttribute("onclick", "javascript:plotSelectedPoint(this)");
        //        if (ListingId != undefined && extraList[i].listingid == ListingId) {
        //            row.setAttribute("class", "highlight");
        //        }

        // add column #
        var cell = document.createElement('td');
        cell.appendChild(document.createTextNode(count));
        row.appendChild(cell);

        if (range != "nation") {
            //            if (extraList[i].distance <= range) {
            count++;
            filteredList.push(extraList[i]);

            for (var prop in extraList[i]) {
                if (prop === 'year' || prop === 'make' || prop === 'model' || prop === 'trim' || prop === 'miles' || prop === 'price' || prop === 'distance' || prop === 'certified' || prop === 'seller' || prop === 'carscom' || prop === 'autotrader') {

                    pop_smalllist_with_detail(extraList[i], prop, title, row);
                }
            }
            //            }
        } else {
            count++;
            filteredList.push(extraList[i]);
            for (var prop in extraList[i]) {
                pop_smalllist_with_detail(extraList[i], prop, title, row);
            }
        }
    }

    //$("table#tblVehicles tbody").append(row);

    // update list
    list = [];
    list = filteredList;

    // only enable the table when having at least 1 record
    if (list.length > 0) {
        $("table#tblVehicles").trigger("update");
        $("table#tblVehicles").show();
    }

    //return list;
}

function pop_smalllist_with_detail(item, prop, title, row) {

    if (findInArray(title, prop)) {
        if (prop === 'title') {
            for (var e in item[prop]) {
                var cell = document.createElement('td');

                if (typeof (item[prop]) === 'string') {
                    var text = document.createTextNode(ucfirst(item[prop][e]));
                } else {
                    var text = document.createTextNode(item[prop][e]);
                }
                cell.appendChild(text);
                row.appendChild(cell);
            }
        } else if (prop === 'option') {
        } else if (prop === 'dealertype' || prop === 'address' || prop === 'vin' || prop === 'extcolor' || prop === 'listingid') {

        } else {

            var cell = document.createElement('td');

            if (item[prop] === true) {
                var text = document.createTextNode('Yes');
            } else if (item[prop] === false) {
                var text = document.createTextNode('No');
            } else {
                if (typeof (item[prop]) === 'string') {
                    var text = document.createTextNode(ucfirst(item[prop]));
                } else if (prop === 'price') {
                    var text = document.createElement('b');
                    var btext = document.createTextNode('$' + addCommas(item[prop]));
                    text.appendChild(btext);
                } else if (prop === 'miles') {
                    var text = document.createElement('b');
                    var btext = document.createTextNode(addCommas(item[prop]));
                    text.appendChild(btext);
                } else if (prop === 'year' || prop === 'make' || prop === 'model' || prop === 'trim' || prop === 'miles' || prop === 'price' || prop === 'distance' || prop === 'certified' || prop === 'seller' || prop === 'carscom' || prop === 'autotrader') {
                    var text = document.createTextNode(item[prop]);
                }
            }
            cell.appendChild(text);
            row.appendChild(cell);
        }
    }

    $("table#tblVehicles tbody").append(row);
}


function pop_list(list, range, dcar) {
    var extraList = [];
    for (var it = 0; it < list.length; it++)
        extraList[it] = list[it];
    if (extraList.length > 0)
        extraList[extraList.length] = dcar;

    // make sure we have table with id 'tblVehicles' exists
    if ($("table#tblVehicles") == undefined || $("table#tblVehicles") == null)
        return false;

    var title = [];
    var filteredList = [];
    $("table#tblVehicles tbody").html("");
    for (var prop in list[0]) {
        if (prop != 'imgs' && prop != 'uptime' && prop != 'thumbnail' && prop != 'carscomlistingurl' && prop != 'trim' && prop != 'trims' && prop != 'autotraderlistingurl' && prop !== 'longtitude' && prop !== 'latitude') {
            title.push(prop);
            if (prop === 'title') {
            } else if (prop === 'option') {
            } else if (prop === 'dealertype' || prop === 'address' || prop === 'vin' || prop === 'extcolor' || prop === 'listingid') {
            } else {
            }
        }
    }

    var count = 1;
    for (var i = 0; i < extraList.length; i++) {
        var row = document.createElement('tr');
        row.setAttribute("id", extraList[i].listingid);
        row.setAttribute("name", "tableRow");
        row.setAttribute("onclick", "javascript:plotSelectedPoint(this)");
        if (ListingId != undefined && extraList[i].listingid == ListingId) {
            row.setAttribute("class", "highlight");
        }

        // add column #
        var cell = document.createElement('td');
        cell.appendChild(document.createTextNode(count));
        row.appendChild(cell);

        if (range != "nation") {
            if (extraList[i].distance <= range) {
                count++;
                filteredList.push(extraList[i]);

                for (var prop in extraList[i]) {
                    pop_list_with_detail(extraList[i], prop, title, row);
                }
            }
        } else {
            count++;
            filteredList.push(extraList[i]);
            for (var prop in extraList[i]) {
                pop_list_with_detail(extraList[i], prop, title, row);
            }
        }
    }

    //$("table#tblVehicles tbody").append(row);

    // update list
    list = [];
    list = filteredList;

    // only enable the table when having at least 1 record
    if (list.length > 0) {
        $("table#tblVehicles").trigger("update");
        $("table#tblVehicles").show();
    }

    //return list;
}

function pop_list_with_detail(item, prop, title, row) {
        
    if (findInArray(title, prop)) {
        if (prop === 'title') {
            for (var e in item[prop]) {
                var cell = document.createElement('td');

                if (typeof (item[prop]) === 'string') {
                    var text = document.createTextNode(ucfirst(item[prop][e]));
                } else {
                    var text = document.createTextNode(item[prop][e]);
                }
                cell.appendChild(text);
                row.appendChild(cell);
            }
        } else if (prop === 'option') {
        } else if (prop === 'dealertype' || prop === 'address' || prop === 'vin' || prop === 'extcolor' || prop === 'listingid') {

        } else {

            var cell = document.createElement('td');

            if (item[prop] === true) {
                var text = document.createTextNode('Yes');
            } else if (item[prop] === false) {
                var text = document.createTextNode('No');
            } else {
                if (typeof (item[prop]) === 'string') {
                    var text = document.createTextNode(ucfirst(item[prop]));
                } else if (prop === 'price') {
                    var text = document.createElement('b');
                    var btext = document.createTextNode('$' + addCommas(item[prop]));
                    text.appendChild(btext);
                } else if (prop === 'miles') {
                    var text = document.createElement('b');
                    var btext = document.createTextNode(addCommas(item[prop]));
                    text.appendChild(btext);
                } else {
                    var text = document.createTextNode(item[prop]);
                }
            }
            cell.appendChild(text);
            row.appendChild(cell);
        }
    }

    $("table#tblVehicles tbody").append(row);           
}

function pop_list_with_detail_for_nationwide(item, prop, title, row) {
    if (findInArray(title, prop)) {
        if (prop === 'title') {
            for (var e in list[i][prop]) {
                var cell = document.createElement('td');
                if (typeof (list[i][prop]) === 'string') {
                    var text = document.createTextNode(ucfirst(list[i][prop][e]));
                } else {
                    var text = document.createTextNode(list[i][prop][e]);
                }
                cell.appendChild(text);
                row.appendChild(cell);
            }
        } else if (prop === 'option') {
        } else {

            var cell = document.createElement('td');

            if (list[i][prop] === true) {
                var text = document.createTextNode('Yes');
            } else if (list[i][prop] === false) {
                var text = document.createTextNode('No');
            } else {
                if (typeof (list[i][prop]) === 'string') {
                    var text = document.createTextNode(ucfirst(list[i][prop]));
                } else if (prop === 'price') {
                    var text = document.createElement('b');
                    var btext = document.createTextNode('$' + addCommas(list[i][prop]));
                    text.appendChild(btext);
                } else if (prop === 'miles') {
                    var text = document.createElement('b');
                    var btext = document.createTextNode(addCommas(list[i][prop]));
                    text.appendChild(btext);
                } else {
                    var text = document.createTextNode(list[i][prop]);
                }
            }
            cell.appendChild(text);
            row.appendChild(cell);
        }
    }

    $("table#tblVehicles tbody").append(row);
}

function pop_list_backup(list, range) {

    var tableDiv = document.getElementById('vehicle-list');
    tableDiv.innerHTML = '';
    table = document.createElement('table');
    table.setAttribute("id", "tblVehicles");
    table.setAttribute("class", "tablesorter");
    
    var title = [];
//    console.log(list);
//    console.log(range);
    var tableHeader = document.createElement('thead');
    var tableBody = document.createElement('tbody');

    var header = document.createElement('tr');
    for (var prop in list[0]) {
        if (prop != 'imgs' && prop != 'uptime' && prop != 'thumbnail' && prop != 'carscomlistingurl' && prop != 'trim' && prop != 'trims' && prop != 'autotraderlistingurl') {
            title.push(prop);
            if (prop === 'title') {
                for (var i in list[0][prop]) {
                    var cell = document.createElement('td');
                    var text = document.createTextNode(ucfirst(i));
                    cell.appendChild(text);
                    header.appendChild(cell);
                }
            } else if (prop === 'option') {
                // var cell = document.createElement('td');
                // var text = document.createTextNode(propucfirst());
                // cell.appendChild(text);
                // header.appendChild(cell);
        } else if (prop === 'dealertype' || prop === 'address' || prop === 'vin' || prop === 'extcolor' || prop === 'listingid') {

        } else {
            var cell = document.createElement('td');
            var text = document.createTextNode(ucfirst(prop));
            cell.appendChild(text);
            header.appendChild(cell);
        }
        }
    }
    tableBody.appendChild(header);
    //tableHeader.appendChild(header);

    for (var i = 0; i < list.length; i++) {
        var row = document.createElement('tr');
        row.setAttribute("id", list[i].listingid);
        row.setAttribute("name", "tableRow");
        row.setAttribute("onclick", "javascript:plotSelectedPoint(this)");
        
        if (range != "nation") {
            if (list[i].distance <= range) {
                for (var prop in list[i]) {
                    if (findInArray(title, prop)) {
                        if (prop === 'title') {
                            for (var e in list[i][prop]) {
                                var cell = document.createElement('td');
                                
                                if (typeof(list[i][prop]) === 'string') {
                                    // var text = document.createElement('a');
                                    var text = document.createTextNode(ucfirst(list[i][prop][e]));
                                    // text.appendChild(atext);
                                    // text.setAttribute('href', list[i]['link']);
                                    // text.setAttribute('target', '_blank');
                                } else {
                                    // var text = document.createElement('a');
                                    var text = document.createTextNode(list[i][prop][e]);
                                    // text.appendChild(atext);
                                    // text.setAttribute('href', list[i]['link']);
                                    // text.setAttribute('target', '_blank');
                                }
                                cell.appendChild(text);
                                row.appendChild(cell);
                            }
                        } else if (prop === 'option') {
                            // var cell = document.createElement('td');
                            // for (var e in list[i][prop]) {
                            // 	if (list[i][prop][e]) {
                            // 		var text = document.createTextNode(eucfirst()+' ');
                            // 		cell.appendChild(text);
                            // 	}
                            // }
                            // row.appendChild(cell);
                    } else if (prop === 'dealertype' || prop === 'address' || prop === 'vin' || prop === 'extcolor' || prop === 'listingid') {

                    } else {

                            var cell = document.createElement('td');

                            if (list[i][prop] === true) {
                                var text = document.createTextNode('Yes');
                            } else if (list[i][prop] === false) {
                                var text = document.createTextNode('No');
                            } else {
                                if (typeof(list[i][prop]) === 'string') {
                                    var text = document.createTextNode(ucfirst(list[i][prop]));
                                } else if (prop === 'price') {
                                    var text = document.createElement('b');
                                    var btext = document.createTextNode('$' + addCommas(list[i][prop]));
                                    text.appendChild(btext);
                                } else if (prop === 'miles') {
                                    var text = document.createElement('b');
                                    var btext = document.createTextNode(addCommas(list[i][prop]));
                                    text.appendChild(btext);
                                } else {
                                    var text = document.createTextNode(list[i][prop]);
                                }
                            }
                            cell.appendChild(text);
                            row.appendChild(cell);
                        }
                    }
                }
            }
        } else {
            for (var prop in list[i]) {
                if (findInArray(title, prop)) {
                    if (prop === 'title') {
                        for (var e in list[i][prop]) {
                            var cell = document.createElement('td');
                            if (typeof(list[i][prop]) === 'string') {
                                // var text = document.createElement('a');
                                var text = document.createTextNode(ucfirst(list[i][prop][e]));
                                // text.appendChild(atext);
                                // text.setAttribute('href', list[i]['link']);
                                // text.setAttribute('target', '_blank');
                            } else {
                                // var text = document.createElement('a');
                                var text = document.createTextNode(list[i][prop][e]);
                                // text.appendChild(atext);
                                // text.setAttribute('href', list[i]['link']);
                                // text.setAttribute('target', '_blank');
                            }
                            cell.appendChild(text);
                            row.appendChild(cell);
                        }
                    } else if (prop === 'option') {
                        // var cell = document.createElement('td');
                        // for (var e in list[i][prop]) {
                        // 	if (list[i][prop][e]) {
                        // 		var text = document.createTextNode(eucfirst()+' ');
                        // 		cell.appendChild(text);
                        // 	}
                        // }
                        // row.appendChild(cell);
                    } else {

                        var cell = document.createElement('td');

                        if (list[i][prop] === true) {
                            var text = document.createTextNode('Yes');
                        } else if (list[i][prop] === false) {
                            var text = document.createTextNode('No');
                        } else {
                            if (typeof(list[i][prop]) === 'string') {
                                var text = document.createTextNode(ucfirst(list[i][prop]));
                            } else if (prop === 'price') {
                                var text = document.createElement('b');
                                var btext = document.createTextNode('$' + addCommas(list[i][prop]));
                                text.appendChild(btext);
                            } else if (prop === 'miles') {
                                var text = document.createElement('b');
                                var btext = document.createTextNode(addCommas(list[i][prop]));
                                text.appendChild(btext);
                            } else {
                                var text = document.createTextNode(list[i][prop]);
                            }
                        }
                        cell.appendChild(text);
                        row.appendChild(cell);
                    }
                }
            }
        }

        tableBody.appendChild(row);
    }
    //table.appendChild(tableHeader);
    table.appendChild(tableBody);    
    tableDiv.appendChild(table);
}

// NEW FOR VEHICLE LIST PRINTOUT
// ################################################



//returns total of prices
function totalprice(obj) {
    var r = 0;
    for (var i in obj) {
        if (obj[i].price != undefined)
            r += obj[i].price;
    }
    return r;
}

// returns random franchise/independant dealer designation
function randTrueFalse() { if (Math.floor((Math.random() * 2) + 1) == 1) { return 'franchise'; } else { return 'independant'; } }

// Returns formatted mileage
function mileFormat(v, axis) {
    var miles = v.toFixed(axis.tickDecimals);
    return abbrNum(miles, 0) + " mi";
}

// Returns formatted price
function priceFormat(v, axis) {
    var price = v.toFixed(axis.tickDecimals);
    return '$' + abbrNum(price, 0);
}

// get checked elements in name group
function getChecked(name) { var result = ''; $('input[name="' + name + '"]').each(function() { if (this.checked) { result = this; } }); return result; }

// give positive number indicator
function posNeg(x) {
    if (x > 0) {
        return '+' + x;
    } else {
        return x;
    }
}


// random range generator (for test data set)
function randomFromTo(from, to) {
    return Math.round(Math.random() * (to - from + 1) + from);
}


// checks if x is between min/max range.
function between(x, min, max) {
    return x >= min && x <= max;
}


// add commas to number string
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;

    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
        //x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }

    return x1 + x2;
}


// log object
function logItem(d, subObj) {
    for (var i in d) {
        if (subObj) {
            if (d[i] && typeof d[i] === 'object') {
                //console.log(i + ' Object:');
                for (var b in d[i]) {
                    //console.log(i + "->" + b + ": " + d[i][b]);
                }
            } else {
                //console.log(i + ': ' + d[i]);
            }
        } else {
            //console.log(i + ': ' + d[i]);
        }
    }
}


// abbreviate numbers
function abbrNum(number, decPlaces) {
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
function splitString(string) { return string.split(","); }


// array of certain value
function extractVal(list, val) {
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

// percentage ranges
function setPrecent() {
    var p = [];
    var i = 0;
    while (i <= 1) {
        i = Math.round(i * 100) / 100;
        p.push(i);
        i = i + .1;
        i = Math.round(i * 100) / 100;
    }
    return p;
}






// ################################################
// FILTER RESULTS FUNCTION UPDATED!!!!!
// now checks if item is object and searches its
// properties

// Filters Object based on object settings.
function filterResults(l, f) {
    // create result array
    var r = [];
    // loop through each array element
    for (var i = 0; i < l.length; i++) {
        var failedMatch = false; //flag
        // search each object item for each filter setting
        for (var set in f) {
            if (typeof (f[set]) == 'object') {
                for (var prop in f[set]) {
                    if (f[set].hasOwnProperty(prop) && l[i][set].hasOwnProperty(prop) && l[i][set][prop] != f[set][prop]) {
                        failedMatch = true;
                        break;
                    }
                }
            } else {

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

function filterResultsWithArray(l, f) {
    // create result array
    var r = [];
    
    // loop through each array element
    for (var i = 0; i < l.length; i++) {
        var failedMatch = false; //flag
        // search each object item for each filter setting
        for (var set in f) {
            if (typeof (f[set]) == 'object') {
                for (var prop in f[set]) {
                    if (prop == 'trim') {
                        if (f[set][prop].length == 0) continue;
                        if (f[set].hasOwnProperty(prop) && l[i][set].hasOwnProperty(prop) && f[set][prop].indexOf(l[i][set][prop].toLowerCase()) < 0) {
                            failedMatch = true;
                            break;
                        }
                    }
                    else {
                        if (f[set].hasOwnProperty(prop) && l[i][set].hasOwnProperty(prop) && l[i][set][prop] != f[set][prop]) {
                            failedMatch = true;
                            break;
                        } 
                    }
                }
            } else {

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

// FILTER RESULTS FUNCTION UPDATED!!!!!
// ################################################







// set ranges
function setRange(list) {

    var inRange = [], arr = [], range = [], flag = false; // set arrays

    //generate ranges
    var percent = setPrecent();
    range[0] = {}, range[1] = {}, range[2] = {}, range[3] = {};
    range[0].start = 0, range[0].end = 99;
    range[1].start = 100, range[1].end = 249;
    range[2].start = 250, range[2].end = 499;
    range[3].start = 500, range[3].end = 3500;

    // loop through each range setting and check if each entry matches the range
    for (var r in range) {
        arr[r] = []; // give arr return an array of each object fitting into each range
        for (var i in list) { // loop through each item
            // if the item's distance is within range, push it to the appropriate arr index
            if (between(list[i]['distance'], range[r].start, range[r].end)) {
                arr[r].push(list[i]);
            }
        }
    }

    return arr; // return range incremented array
}
function sidebar_update(index, list, dealerCar) {
    var miles = $('#carInfo #miles');
    var price = $('#carInfo #price');
    var seller = $('#carInfo #seller');
    var address = $('#carInfo #address');
    var carThumb = $('#carInfo #car-thumb');
    var autotrader = $('#carInfo #AutoTraderLink');
    var carscom = $('#carInfo #CarsComLink');
    var diffM = $('#carInfo #diffM');
    var diffP = $('#carInfo #diffP');
    var daysOnMarket = $('#carInfo #daysOnMarket');
    var distance = $('#carInfo #distance');
    //var certified = $('#carInfo #certified-span');
    var car = $('#car');
    if (dealerCar.listingid == index) {
        miles.text(dealerCar.miles);
        price.text(dealerCar.price);
        diffM.text('');
        diffP.text('');
        seller.text(dealerCar.seller);
        address.text(dealerCar.address);
        if (dealerCar.title.trim == 'Other') {
            car.html(dealerCar.title.year + ' ' + dealerCar.title.make + ' ' + dealerCar.title.model);
        } else {
            car.html(dealerCar.title.year + ' ' + dealerCar.title.make + ' ' + dealerCar.title.model + ' ' + dealerCar.title.trim);
        }

        if (dealerCar.thumbnail != '')
            carThumb.html('<img width="100" height="100"  src="' + dealerCar.thumbnail + '" alt="car-thumbnail" />');
        else {
            carThumb.html('');
        }
        if (dealerCar.carscom)
            carscom.html('<a target="_blank"  href="' + dealerCar.carscomlistingurl + '">CarsCom</a>');
        else {
            carscom.html("");
        }
        if (dealerCar.autotrader)
            autotrader.html('<a target="_blank"  href="' + dealerCar.autotraderlistingurl + '">AutoTrader</a>');
        else
            autotrader.html("");

        distance.text('0 mi');

        if (!dealerCar.certified) {
            certified.text('No');
        } else {
            certified.text('Yes');
        }

        if ($selectedCar == undefined) {
        } else {
            $selectedCar = dealerCar;
        }

    } else {
        for (var i = 0; i < list.length; i++) {
            if (list[i].listingid == index) {
                miles.text(addCommas(list[i].miles));
                price.text(addCommas(list[i].price));
                diffM.text(addCommas(posNeg(dealerCar.data[0][0] - list[i].miles)));
                diffP.text(addCommas(posNeg(dealerCar.data[0][1] - list[i].price)));
                seller.text(list[i].seller);
                address.text(list[i].address);

                if (list[i].thumbnail!='')
                    carThumb.html('<img width="100" height="100"  src="' + list[i].thumbnail + '" alt="car-thumbnail" />');
                
                if (list[i].carscom)
                    carscom.html('<a target="_blank"  href="' + list[i].carscomlistingurl + '">CarsCom</a>');
                else {
                    carscom.html("");
                }
                if (list[i].autotrader)
                    autotrader.html('<a target="_blank"  href="' + list[i].autotraderlistingurl + '">AutoTrader</a>');
                else
                    autotrader.html("");

                distance.text(addCommas(list[i].distance) + ' mi');

//                if (!list[i].certified) {
//                    certified.text('No');
//                } else {
//                    certified.text('Yes');
//                }
//                
                if ($selectedCar == undefined) {
                } else {
                    $selectedCar = list[i];
                }
                
                break;

            }
        }
    }


}
