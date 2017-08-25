var ChartConfig = ChartConfig || { isExcludeCurrentCar: false };
ChartConfig.waitingImage = ChartConfig.waitingImage || "/images/ajaxloadingindicator.gif";
var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange: { min: 0, max: 100 }, isSoldView: false, isSmallChart: false };
var UrlPaths = UrlPaths || { requestNationwideUrl: "" };


// JavaScript Document
//$drawDiv = $('#placeholder');

Array.prototype.contains = function (v) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === v) return true;
    }
    return false;
};

Array.prototype.unique = function () {
    var arr = [];
    for (var i = 0; i < this.length; i++) {
        if (!arr.contains(this[i])) {
            arr.push(this[i]);
        }
    }
    return arr;
};

function getTrimList(list) {
    var result = [];
    if (typeof(list) !== 'undefined') {
        for (var i = 0; i < list.length; i++) {
            if (!result.contains(list[i].title.trim.toLowerCase())) {
                result.push(list[i].title.trim.toLowerCase());
            }
        }
    }
    return result.unique();
}

function getBodyStyleList(list) {
    var result = [];
    if (typeof (list) !== 'undefined') {
        for (var i = 0; i < list.length; i++) {
            if (!result.contains(list[i].title.bodyStyle.toLowerCase())) {
                result.push(list[i].title.bodyStyle.toLowerCase());
            }
        }
    }
    return result.unique();
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
//function getUnique(data, find) { var result = []; for (var item in data) { if (data[item][find]) { if (!findInArray(result, data[item][find])) { result.push(data[item][find]); } } } return result; }



function makeTrimList(id, data, fRange, filter, $dCar, expand, defaultTrim, defaultBodyStyle) {
    if (id) {
        isTrimListDirty = false;
        makeTrimCheckList(id, data, fRange, filter, $dCar, expand, defaultTrim, defaultBodyStyle);
    } else { }
}

var isTrimListDirty = false;
function makeTrimCheckList(id, data, fRange, filter, $dCar, expand, defaultTrim, defaultBodyStyle) {
    var currentItemSelection = "#" + id;
    var defaultTrims = defaultTrim; //defaultTrim.split(',');
    var defaultBodyStyles = defaultBodyStyle;
    $(currentItemSelection).dropdownchecklist("destroy");
    $(currentItemSelection + " option").remove();
    //console.log('asdsad');
    //console.log($filter);
    //console.log('End asdsad');
    if (defaultTrims.length == 0 || (defaultTrims.length == 1 && (defaultTrims[0] == '0' || defaultTrims[0] == undefined))) {
        if (data.length > 0) {
            $(currentItemSelection).append($("<option></option>").attr('value', '0').attr('selected', 'selected').text('All Trims'));
        }
        $.each(data, function (index, obj) {
            var count = 0;
            $.each($data, function (i, carItem) {
                if ($('#IsCertified').val().toLowerCase() == 'false') {
                    if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.trim.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min && carItem.certified == false) {
                        if (filter.dealertype != null && filter.marketSource != null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'all') {
                                    count++;
                                }
                            }
                        } else if (filter.dealertype != null && filter.marketSource == null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                count++;
                            }
                        } else if (filter.dealertype == null && filter.marketSource != null) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        } else {
                            count++;
                        }
                    }
                } else if ($('#IsCertified').val().toLowerCase() == 'true') {
                    if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.trim.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min && carItem.certified == true) {
                        if (filter.dealertype != null && filter.marketSource != null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'all') {
                                    count++;
                                }
                            }
                        } else if (filter.dealertype != null && filter.marketSource == null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                count++;
                            }
                        } else if (filter.dealertype == null && filter.marketSource != null) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        } else {
                            count++;
                        }
                    }
                } else {
                    if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.trim.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min) {
                        if (filter.dealertype != null && filter.marketSource != null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                    count++;
                                }
                                else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                    count++;
                                }
                                else if (filter.marketSource.toLowerCase() == 'all') {
                                    count++;
                                }
                            }
                        } else if (filter.dealertype != null && filter.marketSource == null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                count++;
                            }
                        }
                        else if (filter.dealertype == null && filter.marketSource != null) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        } else {
                            count++;
                        }
                    }
                }
            });
            $(currentItemSelection).append($("<option></option>").attr('value', obj).text(ChartHelper.ucfirst(obj) + ' (' + count + ')'));
        });
    }
    else {
        if (data.length > 0) {
            $(currentItemSelection).append($("<option></option>").attr('value', '0').text('All Trims'));
        }

        $.each(data, function (index, obj) {
            var count = 0;
            $.each($data, function (i, carItem) {
                if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.trim.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min) {
                    if (filter.dealertype != null && filter.marketSource != null) {
                        if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            }
                            else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            }
                            else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        }
                    } else if (filter.dealertype != null && filter.marketSource == null) {
                        if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                            count++;
                        }
                    }
                    else if (filter.dealertype == null && filter.marketSource != null) {
                        if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'all') {
                            count++;
                        }
                    } else {
                        count++;
                    }
                }
            });
            var item = $("<option></option>").attr('value', obj).text(ChartHelper.ucfirst(obj) + ' (' + count + ')');
            if (defaultTrims.indexOf(obj) >= 0)
                item = item.attr('selected', 'selected');

            $(currentItemSelection).append(item);
        });
    }

    $(currentItemSelection).dropdownchecklist({
        firstItemChecksAll: true, width: 150, maxDropHeight: 300
            , onComplete: function (selector) {
                
                defaultTrims = [];
                ChartInfo.$filter.title = {}; ChartInfo.$filter.title.trim = [];
                for (i = 0; i < selector.options.length; i++) {
                    if (selector.options[i].selected && (selector.options[i].value != "")) {
                        if (i == 0) {
                            //$filter.title.trim.push(selector.options[i].value);
                            defaultTrims.push(i);
                            delete ChartInfo.$filter.title;
                            break;
                        }
                        ChartInfo.$filter.title.trim.push(selector.options[i].value);
                        //console.log( $filter.title);
                        defaultTrims.push(selector.options[i].value);
                    }
                }

                // #######################
                // NEW FOR DEALERTYPE
                var dealerSelect = ChartHelper.getChecked('dealertype');
                if (dealerSelect.id != 'all') { ChartInfo.$filter.dealertype = dealerSelect.id; } else { delete ChartInfo.$filter.dealertype; }
                // NEW FOR DEALERTYPE
                // #######################

                // check certified
                if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }
                
                var trimList = '';
                if (ChartInfo.$filter.title && ChartInfo.$filter.title.trim) {
                    for (var i = 0; i < ChartInfo.$filter.title.trim.length; i++) {
                        trimList += ChartInfo.$filter.title.trim[i] + ',';
                    }
                }
        
            // draw chart
            if (isTrimListDirty) {
                $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
                setTimeout(function() {

                    drawChart($data, fRange, ChartInfo.$filter, $dCar, expand, defaultTrims, GetDataTrim(), GetDataBodyStyle(), '0');

                    ChartInfo.$filter = {};
                    $.unblockUI();
                }, 400);
            }

            // unset filter
            }
            , onItemClick: function () {
                isTrimListDirty = true;
            }
    });
}

var isBodyStyleListDirty = false;
function makeBodyStyleList(id, data, fRange, filter, $dCar, expand, defaultBodyStyle, defaultTrim) {
    if (id) {
        isBodyStyleListDirty = false;
        makeBodyStyleCheckList(id, data, fRange, filter, $dCar, expand, defaultBodyStyle, defaultTrim);
    } else { }
}

var loSelector = '';

function makeBodyStyleCheckList(id, data, fRange, filter, $dCar, expand, defaultBodyStyle, defaultTrim) {
    var currentItemSelection = "#" + id;
    var defaultBodyStyles = defaultBodyStyle;
    var defaultTrims = defaultTrim;
    $(currentItemSelection).dropdownchecklist("destroy");
    $(currentItemSelection + " option").remove();
    
    if (defaultBodyStyles.length == 0 || (defaultBodyStyles.length == 1 && (defaultBodyStyles[0] == '0' || defaultBodyStyles[0] == undefined))) {
        if (data.length > 0) {
            $(currentItemSelection).append($("<option></option>").attr('value', '0').attr('selected', 'selected').text('All Styles'));
        }
        $.each(data, function (index, obj) {
            var count = 0;
            $.each($data, function (i, carItem) {
                if ($('#IsCertified').val().toLowerCase() == 'false' && (defaultTrims == "0" || defaultTrims.indexOf(carItem.trim.toLowerCase()) > -1)) {
                    if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.bodyStyle.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min && carItem.certified == false) {
                        if (filter.dealertype != null && filter.marketSource != null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'all') {
                                    count++;
                                }
                            }
                        } else if (filter.dealertype != null && filter.marketSource == null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                count++;
                            }
                        } else if (filter.dealertype == null && filter.marketSource != null) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        } else {
                            count++;
                        }
                    }
                } else if ($('#IsCertified').val().toLowerCase() == 'true' && (defaultTrims == "0" || defaultTrims.indexOf(carItem.trim.toLowerCase()) > -1)) {
                    if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.bodyStyle.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min && carItem.certified == true) {
                        if (filter.dealertype != null && filter.marketSource != null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'all') {
                                    count++;
                                }
                            }
                        } else if (filter.dealertype != null && filter.marketSource == null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                count++;
                            }
                        } else if (filter.dealertype == null && filter.marketSource != null) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        } else {
                            count++;
                        }
                    }
                } else if ((defaultTrims == "0" || defaultTrims.indexOf(carItem.trim.toLowerCase()) > -1)) {
                    if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.bodyStyle.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min) {
                        if (filter.dealertype != null && filter.marketSource != null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                    count++;
                                }
                                else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                    count++;
                                }
                                else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                    count++;
                                } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                    count++;
                                }
                                else if (filter.marketSource.toLowerCase() == 'all') {
                                    count++;
                                }
                            }
                        } else if (filter.dealertype != null && filter.marketSource == null) {
                            if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                                count++;
                            }
                        }
                        else if (filter.dealertype == null && filter.marketSource != null) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        } else {
                            count++;
                        }
                    }
                }
            });
            $(currentItemSelection).append($("<option></option>").attr('value', obj).text(ChartHelper.ucfirst(obj) + ' (' + count + ')'));
        });
    }
    else {
        if (data.length > 0) {
            $(currentItemSelection).append($("<option></option>").attr('value', '0').text('All Styles'));
        }

        $.each(data, function (index, obj) {
            var count = 0;
            $.each($data, function (i, carItem) {
                if (ChartHelper.ucfirst(obj).toLowerCase() == carItem.bodyStyle.toLowerCase() && carItem.distance <= fRange.max && carItem.distance >= fRange.min) {
                    if (filter.dealertype != null && filter.marketSource != null && (filter.dealertype == null && filter.marketSource != null)) {
                        if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                            if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                                count++;
                            }
                            else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                                count++;
                            }
                            else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                                count++;
                            } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                                count++;
                            }
                            else if (filter.marketSource.toLowerCase() == 'all') {
                                count++;
                            }
                        }
                    } else if (filter.dealertype != null && filter.marketSource == null && (filter.dealertype == null && filter.marketSource != null)) {
                        if (filter.dealertype.toLowerCase() == carItem.dealertype.toLowerCase()) {
                            count++;
                        }
                    }
                    else if (filter.dealertype == null && filter.marketSource != null && (filter.dealertype == null && filter.marketSource != null)) {
                        if (filter.marketSource.toLowerCase() == 'carscom' && carItem.carscom == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'autotrader' && carItem.autotrader == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'carmax' && carItem.carmax == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'commercialtruck' && carItem.commercialtruck == true) {
                            count++;
                        } else if (filter.marketSource.toLowerCase() == 'all') {
                            count++;
                        }
                    } else if ((filter.dealertype == null && filter.marketSource != null)) {
                        count++;
                    }
                }
            });
            var item = $("<option></option>").attr('value', obj).text(ChartHelper.ucfirst(obj) + ' (' + count + ')');
            if (defaultBodyStyles.indexOf(obj) >= 0)
                item = item.attr('selected', 'selected');

            $(currentItemSelection).append(item);
        });
    }

    $(currentItemSelection).dropdownchecklist({
        firstItemChecksAll: true, width: 150, maxDropHeight: 300
            , onComplete: function (selector) {
            
                defaultBodyStyles = [];
                ChartInfo.$filter.title = {}; ChartInfo.$filter.title.bodyStyle = [];
                for (i = 0; i < selector.options.length; i++) {
                    if (selector.options[i].selected && (selector.options[i].value != "")) {
                        if (i == 0) {
                            //$filter.title.trim.push(selector.options[i].value);
                            defaultBodyStyles.push(i);
                            delete ChartInfo.$filter.title;
                            break;
                        }
                        ChartInfo.$filter.title.bodyStyle.push(selector.options[i].value);
                        //console.log( $filter.title);
                        defaultBodyStyles.push(selector.options[i].value);
                    }
                }

            for (i = 0; i < defaultTrims.length; i++) {
                if (defaultTrims[i] == 0) break;
                if (ChartInfo.$filter.title == undefined) { ChartInfo.$filter.title = {}; }
                if (ChartInfo.$filter.title.trim == undefined) ChartInfo.$filter.title.trim = [];
                ChartInfo.$filter.title.trim.push(defaultTrims[i]);
            }

            // #######################
                // NEW FOR DEALERTYPE
                var dealerSelect = ChartHelper.getChecked('dealertype');
                if (dealerSelect.id != 'all') { ChartInfo.$filter.dealertype = dealerSelect.id; } else { delete ChartInfo.$filter.dealertype; }
                // NEW FOR DEALERTYPE
                // #######################

                // check certified
                if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }

                var trimList = '';
                if (ChartInfo.$filter.title && ChartInfo.$filter.title.bodyStyle) {
                    for (var i = 0; i < ChartInfo.$filter.title.bodyStyle.length; i++) {
                        trimList += ChartInfo.$filter.title.bodyStyle[i] + ',';
                    }
                }
            
                // draw chart
                if (isBodyStyleListDirty) {
                    $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
                    setTimeout(function () {

                        drawChart($data, fRange, ChartInfo.$filter, $dCar, expand, defaultTrims, GetDataTrim(), GetDataBodyStyle(), defaultBodyStyles);

                        ChartInfo.$filter = {};
                        $.unblockUI();
                    }, 400);
                }

                // unset filter

            }
            , onItemClick: function () {
                isBodyStyleListDirty = true;
            }
    });
}

function appendFirstLoading() {
    isTrimListDirty = true;
    var selector = $("#trim-filter")[0];
    var defaultTrims = [];
    ChartInfo.$filter.title = {}; ChartInfo.$filter.title.trim = [];
    for (i = 0; i < selector.options.length; i++) {
        if (selector.options[i].selected && (selector.options[i].value != "")) {
            if (i == 0) {
                //$filter.title.trim.push(selector.options[i].value);
                defaultTrims.push(i);
                delete ChartInfo.$filter.title;
                break;
            }
            ChartInfo.$filter.title.trim.push(selector.options[i].value);
            //console.log( $filter.title);
            defaultTrims.push(selector.options[i].value);
        }
    }

    // #######################
    // NEW FOR DEALERTYPE
    var dealerSelect = ChartHelper.getChecked('dealertype');
    if (dealerSelect.id != 'all') { ChartInfo.$filter.dealertype = dealerSelect.id; } else { delete ChartInfo.$filter.dealertype; }
    // NEW FOR DEALERTYPE
    // #######################

    // check certified
    if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }

    var trimList = '';
    if (ChartInfo.$filter.title && ChartInfo.$filter.title.trim) {
        for (var i = 0; i < ChartInfo.$filter.title.trim.length; i++) {
            trimList += ChartInfo.$filter.title.trim[i] + ',';
        }
    }
 

    // draw chart
    if (isTrimListDirty) {
        
        setTimeout(function () {

            drawChart($data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, defaultTrims, GetDataTrim(), GetDataBodyStyle(), '0');

            ChartInfo.$filter = {};
            $.unblockUI();
        }, 400);
    }
}

function formatNumber(num) {
    var p = parseInt(num).toFixed(2).split(".");
    return p[0].split("").reverse().reduce(function (acc, numPara, i) {
        return numPara + (i && !(i % 3) ? "," : "") + acc;
    }, "");
}

// write sidebar view



// plots object onto FLOT Chart Plugin

// #########################################################
// SET OPTION VARIABLE ADDED TO CHART DRAW FUNCTION
// #########################################################
//
// Allows you to set option default value, please
// make sure all chart draw functions on the graph
// jquery calls have default_option in the arguments
//
// #########################################################

function drawChart(list, fRange, filter, dealerCar, expanded, setTrim, dataTrims, dataBodyStyles, setBodyStyle) {
    //console.log(list);
    var isIncludeCurrentCar = !ChartConfig.isExcludeCurrentCar;
    var currentRanking = 1;
    if (list != null && list.length != 0) {

        list = PrepareChartData(fRange, list, filter);
        //console.log(list);
        filterList = list;
     
        var m = new VINControl.Chart.Marketmetrics(list, dealerCar);
        //if (data.length > 0) {
        //    //$('.num-cars').html(data.length);
        //    $('#low').html('$' + ChartHelper.addCommas(m.smallestPrice));
        //    $('#middle').html('$' + ChartHelper.addCommas(m.averageprice));
        //    $('#high').html('$' + ChartHelper.addCommas(m.largestPrice));
        //}

        if (ChartInfo.isSmallChart && dealerCar != null) {
            var listData = sortData(list, 'price', true);
            for (var j = 0; j < listData.length; j++) {
                if (listData[j].price > dealerCar.price) {
                    //console.log(listData[j].price);
                    currentRanking = j + 1;
                    break;
                }
            }
            m.currentRanking = currentRanking;
        }

        // get current filterred list of car
        if ($currentFilterredList !== undefined)
            $currentFilterredList = list;



        $('#NumberofCarsOnTheChart').text('List of ' + (isIncludeCurrentCar ? list.length + 1 : list.length) + ' Charted Vehicles');
        //$('#NumberofCarsOnTheChart').text('List of ' + (isIncludeCurrentCar ? exactMatchList.length + 1 : exactMatchList.length) + ' Exact Match Charted Vehicles');
        //$('#NumberofSimilarCarsOnTheChart').text('List of ' + (isIncludeCurrentCar ? similartMatchList.length + 1 : similartMatchList.length) + ' Similar Match Charted Vehicles');
        try {
            $('#market_search_v2_number').text(list.length);
        } catch (e) {
        }

     
        $('input#expand').prop('disabled', false);
        $('#filter-wrap input, #filter-wrap select').each(function () {
            $(this).prop('disabled', false);
        });
      
        $('.market-info').show();
        $('#graph-title-bar').show();
        $('canvas').show();
        //$('.tickLabels').show();
        $('#printable-list').show();
        $('#side-nav-wrap').show();

        var trimList = PrepareTrimListData(fRange, dataTrims, list);
        RenderTrimFilter(trimList, fRange, filter, dealerCar, expanded, setTrim, setBodyStyle);
        if (dataBodyStyles != undefined && setBodyStyle != undefined) {
            var bodyStyleList = PrepareBodyStyleListData(fRange, dataBodyStyles, list);
            //console.log(bodyStyleList);
            RenderBodyStyleFilter(bodyStyleList, fRange, filter, dealerCar, expanded, setBodyStyle, setTrim);
        }
       
        RenderChartData(list, dealerCar, expanded, filter, isIncludeCurrentCar);
        
    } else {
        DisplayNoDataChart();
    }

    return m;
}

function PrepareChartData(fRange, list, filter) {
   
    var arr = [];
    var index = 0;
    for (var it = 0; it < list.length; it++) {
        if (list[it].distance <= fRange.max && list[it].distance >= fRange.min) {
            if ($('#IsCertified').val().toLowerCase() == 'true') {
                if (list[it].certified == true) {
                    arr[index] = list[it];
                    index++;
                }
            }
            else if ($('#IsCertified').val().toLowerCase() == 'false') {
                if (list[it].certified == false) {
                    arr[index] = list[it];
                    index++;
                }
            } else {
                arr[index] = list[it];
                index++;
            }
        }
    }
    index = 0;
    list = arr;


    var finalArr = [];
    for (var ih = 0; ih < arr.length; ih++) {
        if (arr[ih].price <= Math.round(ChartHelper.totalprice(list) / list.length) * 2.5) {
            finalArr[index] = arr[ih];
            index++;
        }
    }
    
    return filterResultsWithArray(finalArr, filter);
}

function RenderTrimFilter(trimList, fRange, filter, dealerCar, expanded, setTrim, setBodyStyle) {
    // add options to dropdown
    if (expanded && trimList != undefined) {
        makeTrimList('trim-filter', trimList, fRange, filter, dealerCar, expanded, setTrim, setBodyStyle);
        if (trimList.length > 0) {
            $("#btnSave").show();
        }
        else {
            $("#btnSave").hide();
        }
    }

}

function RenderBodyStyleFilter(bodyStyleList, fRange, filter, dealerCar, expanded, setBodyStyle, setTrim) {
    // add options to dropdown
    if (expanded && bodyStyleList != undefined) {
        makeBodyStyleList('style-filter', bodyStyleList, fRange, filter, dealerCar, expanded, setBodyStyle, setTrim);
        if (bodyStyleList.length > 0) {
            $("#btnSave").show();
        }
        else {
            $("#btnSave").hide();
        }
    }
}

function PrepareTrimListData(fRange, dataTrims,list) {
    var trimsWithDistance = [];

    if (dataTrims) {
        for (var j = 0; j < dataTrims.length; j++) {
            if (dataTrims[j].distance <= fRange.max && dataTrims[j].distance >= fRange.min) {
                trimsWithDistance.push(dataTrims[j].trim);
            }
        }
    }
    if (trimsWithDistance.length>0) {
        return trimsWithDistance;
    } else {
        return getTrimList(list);
    }
}

function PrepareBodyStyleListData(fRange, dataBodyStyles, list) {
    var bodyStylesWithDistance = [];

    if (dataBodyStyles) {
        for (var j = 0; j < dataBodyStyles.length; j++) {
            if (dataBodyStyles[j].distance <= fRange.max && dataBodyStyles[j].distance >= fRange.min) {
                bodyStylesWithDistance.push(dataBodyStyles[j].bodyStyle.toLowerCase());
            }
        }
    }
    if (bodyStylesWithDistance.length > 0) {
        return bodyStylesWithDistance;
    } else {
        return getBodyStyleList(list);
    }
}

function RenderChartData(list, dealerCar, expanded, filter, isIncludeCurrentCar) {
    if (expanded) {
       
        var chartGridView = new VINControl.Chart.GridView();        // update list with selected range
        try {
            if (isMarketSearch == 1) {
                if ($('#hdIsList').val() == 1) {
                   
                    chartGridView.pop_list(list, dealerCar, ChartInfo.selectedId, isIncludeCurrentCar);
                }
            } else {
               
                chartGridView.pop_list(list, dealerCar, ChartInfo.selectedId, isIncludeCurrentCar);
            }
        } catch (e) {
            chartGridView.pop_list(list, dealerCar, ChartInfo.selectedId, isIncludeCurrentCar);
        }
    }

  
    if (dealerCar != null || list.length > 0) {
        try {
            var marketChart = new MarketChart({
                chartDiv: '#graphWrap',
                sidebar: '#sidebar',
                baseVehicle: dealerCar,
                marketList: list,
                dimensions: ChartInfo.isSmallChart ? [400, 230] : [800, 550],
                comparison: true,
                isSmallView: ChartInfo.isSmallChart,
                filter: filter,
                isIncludeCurrentCar: isIncludeCurrentCar
            });
            marketChart.drawMarketChart();
        } catch (e) {

        }

        $("#graphWrap").css("background-image", "none");
        $("#graphWrap svg").show();

        $(".mp_v2_scroll_holder").show();
        $("#NoContent").hide();
     
        if (typeof (VINControl) != "undefined") {
            $('#google-maps').show();
            if ($('#divMap').css('display') == 'block' && $('#google-maps').html().trim() != '') {

                var zoomLevel = VINControl.GoogleMap.GetZoomLevel(dealerLongtitude, dealerLatitude);
             
                if (typeof google !== 'undefined' && typeof google.maps !== 'undefined') {
                    var map = new google.maps.Map(document.getElementById('google-maps'), {
                        zoom: zoomLevel.zoom,
                        center: new google.maps.LatLng(zoomLevel.latitude, zoomLevel.longtitude),
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    });
                  
                    var objconvertHelper = new VINControl.GoogleMap.ConvertHelper(map);
                    var markers = objconvertHelper.convertToMarkerPoints($currentFilterredList);
                    if (dealerCar != null && isIncludeCurrentCar) {
                        dealerCar.longtitude = dealerLongtitude;
                        dealerCar.latitude = dealerLatitude;
                        var marker = objconvertHelper.convertToMarkerPoint(dealerCar, map);
                        marker.setIcon('https://maps.google.com/mapfiles/kml/shapes/schools_maps.png');
                        marker.setMap(map);
                    }
                    var markerCluster = new MarkerClusterer(map, markers);
                    google.maps.event.addListener(markerCluster, 'clusterclick', function (cluster) {
                        var testMarker = cluster.getMarkers();
                        // your code here
                        if (expanded)
                        chartGridView.pop_smalllist(testMarker, isIncludeCurrentCar);
                    });
                }
            }
        }
    } else {
        //alert('There are no cars satisfied you search condition.');
        $(".mp_v2_scroll_holder").hide();
        $("#NoContent").show();
        //$('#high').html('$0');
        //$('#middle').html('$0');
        //$('#low').html('$0');
        $("#graphWrap").css("background-image", "url('../images/No-Data-big.jpg')");

        if (expanded) {
            // update list with selected range
            try {

                if (isMarketSearch == 1) {
                    if ($('#hdIsList').val() == 1) {
                        chartGridView.pop_list(list, dealerCar, ChartInfo.selectedId, isIncludeCurrentCar);
                    }
                } else {
                    chartGridView.pop_list(list, dealerCar, ChartInfo.selectedId, isIncludeCurrentCar);
                }
            } catch (e) {
                chartGridView.pop_list(list, dealerCar, ChartInfo.selectedId, isIncludeCurrentCar);
            }
        }

        $('#market_search_v2_number').html('0');
        $("#graphWrap svg").hide();

    }

}

function DisplayNoDataChart() {
// ReSharper disable once Html.PathError
    $("#graphWrap").css("background-image", "url('../images/No-Data-big.jpg')");
    if (expanded) {
    }
    $('#market_search_v2_number').html('0');
    $("#graphWrap svg").hide();

    $('input#expand').prop('disabled', true);
    $('#filter-wrap input, #filter-wrap select').each(function () {
        $(this).prop('disabled', true);
    });

    $('#google-maps').hide();

    $('.market-info').hide();
    $('#graph-title-bar').hide();
    $('canvas').hide();
    //$('.tickLabels').hide();
    $('#printable-list').hide();
    $('#side-nav-wrap').hide();

}

function sortData(data, prop, asc) {
    return data.sort(function (a, b) {
        if (asc) return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
        else return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
    });
}


//marketPlotting = function (arr) {
//    var N = arr.length,
//	        XY = [],
//	        XX = [],
//	        EX = 0,
//	        EY = 0,
//	        EXY = 0,
//	        EXX = 0;

//    for (var i = 0; i < N; i++) {
//        var y = arr[i].price;
//        var x = arr[i].miles;

//        XY.push(x * y);
//        XX.push(x * x);
//        EX += x;
//        EY += y;
//        EXY += x * y;
//        EXX += x * x;
//    }

//    this.Slope = ((N * EXY) - (EX * EY)) / ((N * EXX) - (EX * EX));
//    this.Intercept = (EY - (this.Slope * EX)) / N;
//    this.RegressionY = function (x) {
//        return this.Intercept + (this.Slope * x);
//    };
//    this.RegressionX = function (y) {
//        return (y - this.Intercept) / this.Slope;
//    };
//};
