function AssignEvents() {
    // add parser through the tablesorter addParser method 
    $.tablesorter.addParser({
        // set a unique id 
        id: 'price',
        is: function (s) {
            // return false so this parser is not auto detected 
            return false;
        },
        format: function (s) {
            // format your data for normalization 
            return s.replace('$', '').replace(/,/g, '');
        },
        // set type, either numeric or text 
        type: 'numeric'
    });

    // add custom numbering widget
    $.tablesorter.addWidget({
        id: "numbering",
        format: function (table) {
            var c = table.config;
            $("tr:visible", table.tBodies[0]).each(function (i) {
                $(this).find('td').eq(0).text(i + 1);
            });
        }
    });

    $("table#tblVehicles").tablesorter({
        // prevent first column from being sortable
        headers: {
            0: { sorter: false },
            8: { sorter: 'price' }, // miles
            9: { sorter: 'price', sortInitialOrder: 'asc'}// prices
        },
        // apply custom widget
        widgets: ['numbering']
    });

    // #######################
    // certified Click Handler
    $('#certified').click(function () {
        // check for selected range setting
        var rangeSet = $('.selected').attr('id');
        // check to see if certified is checked or unchecked, if checked apply filter
        if (this.checked) { $filter.certified = 1; }

        // #######################
        // OPTION DROPDOWN
        var _defaultOptions = GetSelectedOptions();
        // OPTION DROPDOWN
        // #######################

        // #######################
        // TRIM DROPDOWN
        var _defaultTrims = GetSelectedTrims();
        // TRIM DROPDOWN
        // #######################

        // #######################
        // DEALERTYPE
        GetCheckedDealerType();
        // DEALERTYPE
        // #######################

        // draw chart
        drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);

        // unset filter
        $filter = {};
    });

    // #######################
    // showtrim Click Handler
    $('#showtrim').click(function () {
        console.log("showtrime");
        // check for selected range setting
        var rangeSet = $('.selected').attr('id');

        // #######################
        // OPTION DROPDOWN
        var _defaultOptions = GetSelectedOptions();
        // OPTION DROPDOWN
        // #######################

        // #######################
        // TRIM DROPDOWN
        var _defaultTrims = GetSelectedTrims();
        // TRIM DROPDOWN
        // #######################

        // #######################
        // DEALERTYPE
        GetCheckedDealerType();
        // DEALERTYPE
        // #######################

        // #######################
        // CERTIFIED
        IsCertified();
        // CERTIFIED
        // #######################

        // draw chart;
        drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);

        // unset filter
        $filter = {};
    });

    // #######################
    // range navigation script
    $('#rangeNav span').click(function () {
        // remove selected class from all range nav elements
        $('.selected').removeClass('selected');
        // apply selected class to clicked element
        $(this).addClass('selected');
        // apply clicked element id to fRange
        fRange = this.id;



        // #######################
        // CERTIFIED
        IsCertified();
        // CERTIFIED
        // #######################

        // #######################
        // OPTION DROPDOWN
        var _defaultOptions = GetSelectedOptions();
        // OPTION DROPDOWN
        // #######################

        // #######################
        // TRIM DROPDOWN
        var _defaultTrims = GetSelectedTrims();
        // TRIM DROPDOWN
        // #######################

        // #######################
        // DEALERTYPE
        GetCheckedDealerType();
        // DEALERTYPE
        // #######################

        // draw chart
        if (fRange == 'nation')
            fRange = 10000;

        drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);

        // unset filter
        $filter = {};
    });

    // #######################
    // dealertype Click Handler
    $('input[name="dealertype"]').click(function () {
//        console.log("delaert type clicked");
//        console.log(this);
        if (this.checked == true && this.id != 'all') {
            $filter.dealertype = this.id;
        } else {
            delete $filter.dealertype;
        }

        // #######################
        // CERTIFIED
        IsCertified();
        // CERTIFIED
        // #######################

        // #######################
        // OPTION DROPDOWN
        var _defaultOptions = GetSelectedOptions();
        // OPTION DROPDOWN
        // #######################

        // #######################
        // TRIM DROPDOWN
        var _defaultTrims = GetSelectedTrims();
        // TRIM DROPDOWN
        // #######################

        // draw chart
        drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);

        // unset filter
        $filter = {};
    });

    // chart expand/contract script
    $('#expand').click(function () {
        if (expand) {
        } else {
            var gwrap_fbox = $('#graph-fancybox');
            // open fancybox
            gwrap_fbox.click();
        }
    });

    // set plotclick function
    $("#placeholder").bind("plotclick", function (event, pos, item) {
        if (item != null) {
            if (item.datapoint[0] == $dCar.data[0][0] && item.datapoint[1] == $dCar.data[0][1]) {

            } else {
                //        console.log(item);
                //        console.log($dCar);
                sidebar(item, $dCar, $data, default_option, default_trim);
            }
        }
    });

    // set dragging
    $("#placeholder").bind("plotFinalSeriesChange", function (event, seriesIndex, dataIndex, x, y) {
        // flag movement
        newY = true;
        // make sure the plot clicked is the vehicle, then rest its values
        if (x.toFixed(0) != $dCar.data[0][0] && y.toFixed(0) != $dCar.data[0][1]) {
            // do nothing
        } else {
            // generate metrics for testing.
            var graphM = new gCon($data, $dCar);
            var m = new metrics($data, $dCar);


            // check if vehicle marker has left the chart boundaries
            if (y.toFixed(0) > graphM.priceRangeEnd) {
                // if vehicle has higher price that chart displays, set it to 100 below chart maximum price
                $dCar.data[0][1] = graphM.priceRangeEnd.toFixed(0) - 100;
            } else if (y.toFixed(0) < 0) {
                // if vehicle is set to below 0 or threshold of the chart, set its price to 0 or above threshold
                if (0 > graphM.startPriceRange) { $dcar.data[0][1] = 0; } else { $dCar.data[0][1] = graphM.startPriceRange + 100; }
            } else {
                // if none of the above are true, then place vehicle on the graph at its new price
                // sidebar(item, $dCar, $data);
                //var priceinput = document.getElementById('price-box');
                var check = confirm("You have changed the pricing of this vehicle to $" + addCommas(y.toFixed(0)) + " Would you like to save the changes?");
                if (check) {
                    $dCar.data[0][1] = y.toFixed(0);
                    //priceinput.value = $dCar.data[0][1];

                    $.post(updateSalePrice, { ListingId: ListingId, SalePrice: y.toFixed(0) }, function (data) {

                        if (data.SessionTimeOut == "TimeOut") {
                            alert("Your session has timed out. Please login back again");
                            var actionUrl = logOff;
                            window.parent.location = actionUrl;
                        } else {
                            var actionUrl = detailUrl;
                            actionUrl = actionUrl.replace('PLACEHOLDER', ListingId);
                            window.parent.location = actionUrl;
                        }
                    });
                }
            }
        }

        // #######################
        // CERTIFIED
        IsCertified();
        // CERTIFIED
        // #######################

        // #######################
        // OPTION DROPDOWN
        var _defaultOptions = GetSelectedOptions();
        // OPTION DROPDOWN
        // #######################

        // #######################
        // TRIM DROPDOWN
        var _defaultTrims = GetSelectedTrims();
        // TRIM DROPDOWN
        // #######################

        // #######################
        // DEALERTYPE
        GetCheckedDealerType();
        // DEALERTYPE
        // #######################

        // redraw chart to replot vehicle.
        drawChart($data, fRange, $filter, $dCar, expand, _defaultOptions, _defaultTrims);

        // unset filter
        $filter = {};
    });

    //    // check for changes on graph.
    //    $('a').click(function() {
    //        var link = $(this).attr('class');
    //        if (link != 'iframe' && link != 'btn-print') {
    //            if (newY) {
    //                // if external link clicked and the graph has been changed, do stuff
    //                confirm('The pricing was changed from $' + addCommas(originalPrice) + ' to $' + addCommas($dCar.data[0][1]) + '. Save changes?');
    //            } else {
    //                // if external link is clicked and the graph has not been changed, do stuff
    //            }
    //        } else {
    //            // if not an external link (fancybox/iframe class), do stuff
    //        }
    //    });

    // #########################
    // IFRAME DIMENSIONS CHANGE TO 960 x 650
    // #########################
    $('#graph-fancybox.iframe').fancybox({
        autoDimensions: true,
        width: 960,
        height: 650
    });

    // ###########################
    // PRINT BUTTON
    // ###########################    
    $('.btn-print').click(function () {
        DownloadCSV();
    });

    // ###########################
    // PRINT BUCKET JUMP BUTTON
    // ###########################
    $('#btnPrintBucketJump').click(function () {
        if ($selectedCar.listingid == undefined)
            alert('Please select a car on market');
        else if ($dCar.title.year == $selectedCar.title.year && $dCar.title.make == $selectedCar.title.make && $dCar.title.model == $selectedCar.title.model && $dCar.price == $selectedCar.price && $dCar.miles == $selectedCar.miles)
            alert('Please select another car on market');
        else {

            //            var url = "/PDF/PrintBucketJump?listingId=" + $dCar.listingid;            
            //            url += "&dealer=" + $selectedCar.seller;
            //            url += "&price=" + $selectedCar.price;
            //            url += "&year=" + $selectedCar.title.year;
            //            url += "&make=" + $selectedCar.title.make;
            //            url += "&model=" + $selectedCar.title.model;
            //            url += "&color=" + $selectedCar.extcolor;
            //            url += "&miles=" + $selectedCar.miles;

            //            window.location.href = url;

            var dealerName = $selectedCar.seller.replace("#", "");
            dealerName = dealerName.replace("-", "");

            var url = "/Market/GetKarPowerSummaryForBuckerJump?listingId=" + $dCar.listingid;
            url += "&dealer=" + dealerName;
            url += "&price=" + $selectedCar.price;
            url += "&year=" + $selectedCar.title.year;
            url += "&make=" + $selectedCar.title.make;
            url += "&model=" + $selectedCar.title.model;
            url += "&color=" + $selectedCar.extcolor;
            url += "&miles=" + $selectedCar.miles;

            $.fancybox({
                href: url,
                'type': 'iframe',
                'width': 1000,
                'height': 700,
                'hideOnOverlayClick': false,
                'centerOnScroll': true,
                'onCleanup': function () {
                },
                onClosed: function () {

                }
            });

        }
    });

    //    $('#btnLinkCarfax').click(function () {

    //        //alert($selectedCar.autotraderlistingurl);

    //        var url = "/Chart/ViewCARFAX";
    ////        url += "&dealer=" + dealerName;
    ////        url += "&price=" + $selectedCar.price;
    ////        url += "&year=" + $selectedCar.title.year;
    ////        url += "&make=" + $selectedCar.title.make;
    ////        url += "&model=" + $selectedCar.title.model;
    ////        url += "&color=" + $selectedCar.extcolor;
    ////        url += "&miles=" + $selectedCar.miles;

    //        $.fancybox({
    //            href: url,
    //            'type': 'iframe',
    //            'width': 1000,
    //            'height': 700,
    //            'hideOnOverlayClick': false,
    //            'centerOnScroll': true,
    //            'onCleanup': function () {
    //            },
    //            onClosed: function () {

    //            }
    //        });
    //   

    //     

    //    });

  
    // ###########################
    // SAVE BUTTON
    // ###########################
    $("#btnSave").click(function () {

        $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

        var listingId = ListingId;
        var isCarscom = $('#radioCarsCom').is(":checked");
        var options = $('#option-filter').val();
        var trims = $('#trim-filter').val();
        var isCertified = $('#certified').is(":checked");
        var isAll = $('#all').is(":checked");
        var isFranchise = $('#franchise').is(":checked");
        var isIndependant = $('#independant').is(":checked");
        //var url = "<%= Url.Content('~/Chart/SaveSelections') %>" + "listingId=" + listingId + "&isCarsCom=" + isCarscom + "&options=" + options + "&trims=" + trims + "&isCertified=" + isCertified + "&isAll=" + isAll + "&isFranchise=" + isFranchise + "&isIndependant=" + isIndependant + "&screen=" + chartType; 
        var url = "/Chart/SaveSelections?listingId=" + listingId + "&isCarsCom=" + isCarscom + "&options=" + options + "&trims=" + trims + "&isCertified=" + isCertified + "&isAll=" + isAll + "&isFranchise=" + isFranchise + "&isIndependant=" + isIndependant + "&screen=" + chartType;

        $.ajax({
            type: "POST",
            url: url,
            data: {},
            success: function (results) {
                $.unblockUI();
                if (isCarscom == true) {
                    $('#CarsCom_Options').val(options.indexOf('0') >= 0 ? '0' : options);
                    $('#CarsCom_Trims').val(trims.indexOf('0') >= 0 ? '0' : trims);
                    $('#CarsCom_IsCertified').val(isCertified ? 'True' : 'False');
                    $('#CarsCom_IsFranchise').val(isFranchise ? 'True' : 'False');
                    $('#CarsCom_IsIndependant').val(isIndependant ? 'True' : 'False');
                    $('#CarsCom_IsAll').val(isAll ? 'True' : 'False');
                }
                else {
                    $('#Options').val(options.indexOf('0') >= 0 ? '0' : options);
                    $('#Trims').val(trims.indexOf('0') >= 0 ? '0' : trims);
                    $('#IsCertified').val(isCertified ? 'True' : 'False');
                    $('#IsFranchise').val(isFranchise ? 'True' : 'False');
                    $('#IsIndependant').val(isIndependant ? 'True' : 'False');
                    $('#IsAll').val(isAll ? 'True' : 'False');
                }

                // Set a flag to know if we need to reload profile page after closing the Chart                
                window.parent.document.getElementById('NeedToReloadPage').value = true;

                // Currently, we only update ranking for AutoTrader within 100 miles
                if (fRange == 100) {
                    //$('#content').val(htmlEncode($("#printable-list")[0].innerHTML));
                    var content = htmlEncode($("#printable-list")[0].innerHTML);
                    var carRanking = 1;
                    var numberOfCars = 0;
                    if ($currentFilterredList != undefined && $currentFilterredList.length > 0) {
                        numberOfCars = $currentFilterredList.length + 1;
                        $.each($currentFilterredList, function (index, obj) {
                            if ($dCar.price >= obj.price)
                                carRanking++;
                        });
                        var prices = extractVal($currentFilterredList, "price");
                        var largestPrice = Math.max.apply(Math, prices);
                        var smallestPrice = Math.min.apply(Math, prices);
                        var averagePrice = Math.round(totalprice($currentFilterredList) / $currentFilterredList.length);

                        if (numberOfCars > 0) {
                            $('#PdfContent').val(htmlEncode($("#printable-list")[0].innerHTML));
                            var updateRankingUrl = "/Chart/UpdateCarRanking?listingId=" + listingId + "&carRanking=" + carRanking + "&numberOfCars=" + numberOfCars + "&oldCarRanking=" + $dCar.ranking + "&oldNumberOfCars=" + $dCar.carsOnMarket + "&isCarscom=" + isCarscom + "&smallestPrice=" + smallestPrice + "&averagePrice=" + averagePrice + "&largestPrice=" + largestPrice;
                            $.ajax({
                                type: "POST",
                                url: "/Chart/KeepPDFContentForMarketPriceRangeChange",
                                data: $("form").serialize(),
                                success: function (results) {
                                    $.ajax({
                                        type: "POST",
                                        url: updateRankingUrl,
                                        data: {},
                                        success: function (results) {

                                        }
                                    });
                                }
                            });
                        }
                    }
                }
                // reload parent page after selections are saved
                //window.parent.location.reload(true);
                //var parentUrl = window.parent.location.href;
                alert(results);
            }
        });
    });

    $('#radioCarsCom').click(function () {
        $data = [];

        var request_url = requestCarsComUrl;
        request_url = request_url.replace('PLACEHOLDER', ListingId);

        $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
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

                if (carlist != null) {
                    for (var i = 0; i < carlist.length; i++) {
                        var arr = carlist;

                        $imgstring = "";
                        $car = InitializeCar(arr[i]);
                        $data[i] = $car;
                    }
                }

                $dCar = InitializeCurrentCar(target, marketinfo == null ? 0 : marketinfo.carsOnMarket);
                InitializeSidebar($dCar);
                $filter = {};

                if ($('.selected').attr("id") == "nation") {
                    $('.selected').removeClass('selected');
                    $("#100").addClass('selected');
                }

                // load saved selection                
                default_option = ($('#CarsCom_Options').val() == '' || $('#CarsCom_Options').val() == '0') ? [0] : $('#CarsCom_Options').val().split(',');
                default_trim = ($('#CarsCom_Trims').val() == '' || $('#CarsCom_Trims').val() == '0') ? [0] : $('#CarsCom_Trims').val().split(',');
                if (!(default_trim.length == 1 && default_trim[0] == '0'))
                    default_trim = LoadDefaulTrimsInLowerCase(default_trim);
                LoadSavedSelectionsForCarsCom();

                drawChart($data, fRange, $filter, $dCar, expand, default_option, default_trim);

                // unset filter
                $filter = {};
                $.unblockUI();

            }
        });
    });

    $('#radioAutoTrader').click(function () {
        $data = [];

        var request_url = requestAutoTraderUrl;
        request_url = request_url.replace('PLACEHOLDER', ListingId);

        $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });
        $.ajax({
            type: "POST",
            url: request_url,
            data: {},
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var carlist = data.carlist;
                var target = data.target;
                //console.log(carlist);
                CheckErrorOnServer(data);
                var marketinfo = data.market;
                var hasCurrentCarInTheList = false;

                if (carlist != null) {
                    for (var i = 0; i < carlist.length; i++) {
                        var arr = carlist;

                        $imgstring = "";
                        $car = InitializeCar(arr[i]);
                        $data[i] = $car;
                    }
                }

                $dCar = InitializeCurrentCar(target, marketinfo == null ? 0 : marketinfo.carsOnMarket);
                InitializeSidebar($dCar);
                if ($('.selected').attr("id") == "nation") {
                    $('.selected').removeClass('selected');
                    $("#100").addClass('selected');
                }

                // load saved selection                
                default_option = ($('#Options').val() == '' || $('#Options').val() == '0') ? [0] : $('#Options').val().split(',');
                default_trim = ($('#Trims').val() == '' || $('#Trims').val() == '0') ? [0] : $('#Trims').val().split(',');
                if (!(default_trim.length == 1 && default_trim[0] == '0'))
                    default_trim = LoadDefaulTrimsInLowerCase(default_trim);
                LoadSavedSelections();

                drawChart($data, fRange, $filter, $dCar, expand, default_option, default_trim);

                // unset filter
                $filter = {};
                $.unblockUI();
            }
        });
    });

}


function LoadSavedSelections() {
    $('#radioCarsCom').attr('checked', false);
    $('#radioAutoTrader').attr('checked', true);

    //if ($('#IsCertified').val() == 'True') { $filter.certified = true; $('#certified').attr('checked', true); }
    if ($('#IsFranchise').val() == 'True') {
        $filter.dealertype = 'franchise';
        $('#franchise').attr('checked', true);
        $('#independant').attr('checked', false);
        $('#all').attr('checked', false);
    } else if ($('#IsIndependant').val() == 'True') {
        $filter.dealertype = 'independant';
        $('#independant').attr('checked', true);
        $('#franchise').attr('checked', false);
        $('#all').attr('checked', false);
    } else {
        delete $filter.dealertype;
        $('#independant').attr('checked', false);
        $('#franchise').attr('checked', false);
        $('#all').attr('checked', true);
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

function LoadSavedSelectionsForCarsCom() {
    $('#radioCarsCom').attr('checked', true);
    $('#radioAutoTrader').attr('checked', false);

    //if ($('#CarsCom_IsCertified').val() == 'True') { $filter.certified = true; $('#certified').attr('checked', true); }
    if ($('#CarsCom_IsFranchise').val() == 'True') {
        $filter.dealertype = 'franchise';
        $('#franchise').attr('checked', true);
        $('#independant').attr('checked', false);
        $('#all').attr('checked', false);
    } else if ($('#CarsCom_IsIndependant').val() == 'True') {
        $filter.dealertype = 'independant';
        $('#independant').attr('checked', true);
        $('#franchise').attr('checked', false);
        $('#all').attr('checked', false);
    } else {
        delete $filter.dealertype;
        $('#independant').attr('checked', false);
        $('#franchise').attr('checked', false);
        $('#all').attr('checked', true);
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

function removeSpaces(input) {

    if (input == undefined || input == null || input == "")
        return "";

    var nsText = input.replace(/(\n\r|\n|\r)/gm, "<1br />");
    nsText = nsText.replace(/\t/g, "");


    var re1 = /\s+/g;
    nsText = nsText.replace(re1, " ");

    var re2 = /\<1br \/>/gi;
    nsText = nsText.replace(re2, "\n");
    return nsText;
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
        listingid: item.listingid,
        longtitude: item.longtitude,
        latitude: item.latitude,
        
    }; // after construction add object to array
    //console.log($car.trim);
    return $car;
}

function InitializeCurrentCar(target, carsOnMarket) {
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
        draggabley: (isEmployee.toUpperCase() == 'FALSE'),
        draggablex: false,
        ranking: target.ranking,
        carsOnMarket: carsOnMarket,
          longtitude: target.longtitude,
        latitude: target.latitude,
        
    };  // after construction add object to array

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

function LoadCarsComInHidden(request_url) {
    if (request_url != null) {
        $('#radioCarsCom').attr('disabled', 'disabled');
        $.ajax({
            type: "POST",
            url: request_url,
            data: {},
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#radioCarsCom').removeAttr('disabled');
            }
        });
    }
}

function LoadAutoTraderInHidden(request_url) {
    if (request_url != null) {
        $('#radioAutoTrader').attr('disabled', 'disabled');
        $.ajax({
            type: "POST",
            url: request_url,
            data: {},
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#radioAutoTrader').removeAttr('disabled');
            }
        });
    }
}

function InitializeSidebar(dealerCar) {
    var miles = $('#carInfo #miles');
    var price = $('#carInfo #price');
    var seller = $('#carInfo #seller');
    var address = $('#carInfo #address');
    var carThumb = $('#carInfo #car-thumb');
    var autotrader = $('#carInfo #AutoTraderLink');
    var carscom = $('#carInfo #CarsComLink');
    var diffM = $('#carInfo #diffM');
    var diffP = $('#carInfo #diffP');
    //var daysOnMarket = $('#carInfo #daysOnMarket');
    var distance = $('#carInfo #distance');
    //    var certified = $('#carInfo #certified-span');
    var car = $('#car');
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

    //    if (!dealerCar.certified) {
    //        certified.text('No');
    //    } else {
    //        certified.text('Yes');
    //    }
}

function LoadDefaulTrimsInLowerCase(default_trim) {
    var lowerCaseTrims = [];
    $.each(default_trim, function (index, obj) {
        lowerCaseTrims.push(obj.toLowerCase());
    });

    return lowerCaseTrims;
}

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function DownloadCSV() {
    $('#content').val(htmlEncode($("#printable-list")[0].innerHTML));
    $('#myform').submit();
}

function GetSelectedOptions() {
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

    return _defaultOptions;
}

function GetSelectedTrims() {
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

    return _defaultTrims;
}

function GetCheckedDealerType() {
    var dealer_select = getChecked('dealertype');
    if (dealer_select.id != 'all') { $filter.dealertype = dealer_select.id; } else { delete $filter.dealertype; }
}

function IsCertified() {
    if ($('#certified').attr('checked') == 'checked') { $filter.certified = 1; }
}

function CheckErrorOnServer(data) {
    if (!(data.error == undefined || data.error == null || data.error == "")) {
        //alert(data.error);
        return false;
    }
    return false;
}

function InitializeChart() {
    // initialize data set array
    $data = [];
    var request_url = $('#IsCarsCom').val() == 'True' ? requestCarsComUrl : requestAutoTraderUrl; //requestAutoTraderUrl;
    request_url = request_url.replace('PLACEHOLDER', ListingId);
    $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none'} });

    $.ajax({
        type: "POST",
        url: request_url,
        data: {},
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var carlist = data.carlist;
//            console.log(JSON.stringify(carlist));
            var target = data.target;
            var marketinfo = data.market;
            var hasCurrentCarInTheList = false;

            if (carlist != null) {
                for (var i = 0; i < carlist.length; i++) {
                    var arr = carlist;

                    $imgstring = "";
                    $car = InitializeCar(arr[i]);
                    $data[i] = $car;
                }
            }

            $dCar = InitializeCurrentCar(target, marketinfo == null ? 0 : marketinfo.carsOnMarket);
            InitializeSidebar($dCar);

            // load saved selections
            if (!(default_trim.length == 1 && default_trim[0] == '0'))
                default_trim = LoadDefaulTrimsInLowerCase(default_trim);
            if ($('#IsCarsCom').val() == 'True')
                LoadSavedSelectionsForCarsCom();
            else
                LoadSavedSelections();


            // draw chart

            drawChart($data, fRange, $filter, $dCar, expand, default_option, default_trim);
//            OverrideSaved();

            // unset filter
            $filter = {};
            $.unblockUI();

            LoadValueInHidden(requestHiddenAutoTraderUrl, requestHiddenCarsComUrl);

        }
    });
}

function GetFilterDataAndNavigateTo(destination) {
    var listingId = ListingId;

    var filterObject = {};
    filterObject.trims = [];
    $("#result input:checked").each(function (index) {
        if ($(this).attr("name") === 'webSource') {
            filterObject.webSource = $(this).attr("key");
        } else if ($(this).attr("name") === 'dealertype') {
            filterObject.dealerType = $(this).attr("key");
        } else {
            filterObject.trims.push($("#result label[for='" + $(this).attr("id") + "']").text());
        }
    });

    //     console.log(filterObject);
    //     console.log(JSON.stringify(filterObject));
    //    var url = "/Chart/ViewFullChart?ListingId=" + listingId;
    //    window.location.href = url;
    $("#CarListingId").val(listingId);
    $("#FilterOptions").val(JSON.stringify(filterObject));
    $("#myform").attr("action", destination);
    $("#myform").submit();
}


