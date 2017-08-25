var ChartConfig = ChartConfig || {isExcludeCurrentCar:false};
ChartConfig.waitingImage = ChartConfig.waitingImage || "/images/ajaxloadingindicator.gif";
var UrlPaths = UrlPaths || { requestNationwideUrl: "" };

function GetDataTrim() {
    return dataTrims;
}

function GetDataBodyStyle() {
    return dataBodyStyles;
}

function AssignGridViewEvents() {
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
            //var c = table.config;
            $("tr:visible", table.tBodies[0]).each(function (i) {
                $(this).find('td').eq(0).text(i + 1);
            });
        }
    });

    $("table#tblVehicles").tablesorter({
        // prevent first column from being sortable
        headers: {
            0: { sorter: false },
            7: { sorter: 'price' }, // miles
            8: { sorter: 'price', sortInitialOrder: 'asc' }// prices
        },
        // apply custom widget
        widgets: ['numbering']
    });

}

function AssignFilterEvents()
{
    $('#rangeNav span').click(function () {

        $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });

        if ($(this).attr('id') != 'viewLinkChart' && $(this).attr('id') != 'viewGoogleMapLink') {
            // remove selected class from all range nav elements
            $('.selected').removeClass('selected');
            // apply selected class to clicked element
            $(this).addClass('selected');
        }
        $('#divMore').removeClass('mp_v2_hl_items_active');
        $('#divMore a').text('...');
     
      
        var maxRangeValue = $('#rangeNav span.selected').attr('id');
        ChartInfo.fRange.min = 0;
        GetCheckedDealerType();
        if (maxRangeValue == 'nation') {
            ChartInfo.fRange.max = 10000;
        } else {
            ChartInfo.fRange.max = maxRangeValue;
        }

        //console.log('frange click');
        //console.log($filter);
        var trimList = '';
        if (ChartInfo.$filter.title && ChartInfo.$filter.title.trim) {
            for (var i = 0; i < ChartInfo.$filter.title.trim.length; i++) {
                trimList += ChartInfo.$filter.title.trim[i] + ',';
            }
        }

        if (typeof (requestsoldInfoUrl) !== 'undefined' && requestsoldInfoUrl != '') {
            
            $.get(requestsoldInfoUrl + "&stateDistance=" + ChartInfo.fRange.max + "&trimList=" + trimList, function (data) {
                if ($("#btnSoldView").hasClass("selected_view")) {
                    $("#soldMarketInfo").html('<span style="font-weight: bold">Last 30 days:</span> <span id="s30days" >' + data.Last30Days + '</span> cars <span style="font-weight: bold">30-60 days:</span> <span id ="s30to60days">' + data.Last30To60Days + '</span> cars <span style="font-weight: bold">60-90 days:</span><span id="s60to90days">' + data.Last60To90Days + '</span> cars');
                }
  
            });
        }
        
        FilterHandler();
    });
   
    $('input[name="dealertype"]').click(function () {
        $('#franchise').parent().find('label').removeClass('labelHighlight');
        $('#independant').parent().find('label').removeClass('labelHighlight');
        $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
        FilterHandler();
    });

    $('#radioCarsCom').click(function () {
        $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
        FilterHandler();
    });

    $('#radioAll').click(function () {
        $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
        FilterHandler();
    });

    $('#radioAutoTrader').click(function () {
        $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
        FilterHandler();
    });

    $('#radioCarMax').click(function () {
        $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
        FilterHandler();
    });

    $('#radioCommercialTruck').click(function () {
        $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
        FilterHandler();
    });

    $('#radioFilter').click(function () {
        if (!$('#extraFilter').is(':hidden')) {
            $('#extraFilter').hide();
            $(this).parent().find('label').css("opacity", 0.5);
        } else {
            $('#extraFilter').show();
            $(this).parent().find('label').css("opacity", 1);
        }
    });
    //$("#extraFilter").mouseout(function () {
    //    $(this).hide();
    //});
}

function FilterHandler() {
    ChartInfo.$filter.marketSource = $('input[name="webSource"]:checked').attr("key");
    
    if ($('input[name="dealertype"]:checked').attr("id") != 'all')
        ChartInfo.$filter.dealertype = $('input[name="dealertype"]:checked').attr("id");
    else
        delete ChartInfo.$filter.dealertype;
   
    setTimeout(function () {
      
        drawChart($data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, /*GetSelectedTrims()*/'0', GetDataTrim(), GetDataBodyStyle(), /*GetSelectedBodyStyles()*/'0');
        $.unblockUI();
    }, 400);
}

function AssignEvents() {

    AssignGridViewEvents();
    AssignFilterEvents();
  
    $('.btn-print').click(function () {
        DownloadCSV();
    });

    $('#btnPrintBucketJump').click(function () {
        //console.log($selectedCar);
        if ($selectedCar.listingid == undefined)
            alert('Please select a car on market');
        else if ($dCar.title.year == $selectedCar.title.year && $dCar.title.make == $selectedCar.title.make && $dCar.title.model == $selectedCar.title.model && $dCar.price == $selectedCar.price && $dCar.miles == $selectedCar.miles)
            alert('Please select another car on market');
        else {
            
            var dealerName = $selectedCar.seller.replace("#", "");
            dealerName = dealerName.replace("-", "");

            var isCarscom = $('#radioCarsCom').is(":checked");
            var isAuto = $('#radioAutoTrader').is(":checked");
            var isAllCar = $('#radioAll').is(":checked");
            var trims = $('#trim-filter').val();
            var isAll = $('#radioAll').is(":checked");
            var isFranchise = $('#franchise').is(":checked");
            var isIndependant = $('#independant').is(":checked");
            var ranges = $('#rangeNav span.selected').attr('id');
            var inventoryType = $('#inventorytype');
            

            var url = "/Market/GetKarPowerSummaryForBuckerJump?listingId=" + $dCar.listingid;
            url += "&dealer=" + $selectedCar.seller;
            url += "&price=" + $selectedCar.price;
            url += "&year=" + $selectedCar.title.year;
            url += "&make=" + $selectedCar.title.make;
            url += "&model=" + $selectedCar.title.model;
            url += "&color=" + $selectedCar.extcolor;
            url += "&miles=" + $selectedCar.miles;
            url += "&isAllCar=" + isAllCar;
            url += "&isAuto=" + isAuto;
            url += "&isCarscom=" + isCarscom;
            url += "&trims=" + trims;
            url += "&isAll=" + isAll;
            url += "&isFranchise=" + isFranchise;
            url += "&isIndependant=" + isIndependant;
            url += "&inventoryType=" + inventoryType.val();
            
            if (ranges == 'nation')
                url += "&ranges=10000";
            else
                url += "&ranges=" + ranges;
            
            url += "&selectedVin=" + $selectedCar.vin;
            url += "&image=" + $selectedCar.thumbnail;

            $.fancybox({
                href: url,
                'type': 'iframe',
                'width': 850,
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

    function openCarFaxReport(vin, popupWidth, popupHeight) {
     
        $.post('<%= Url.Content("~/Chart/CarFaxReportFromAutoTrader") %>', { vin: vin }, function (data) {
       
            window.open('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=' + data + '&vin=' + $selectedCar.vin);

        });
        window.open('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=C416523&vin=' + $selectedCar.vin);
        
    }


    $('a[id^=checkCarFax_]').live('click', function () {

        var vin = $selectedCar.vin;

        var url = "/Chart/CarFaxReportFromAutoTrader?vin=" + vin;

        $.ajax({
            type: "POST",
            url: url,
            data: {},
            success: function (results) {

                window.open('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID=' + results + '&vin=' + $selectedCar.vin);

            }
        });
        
  
    });
   
    $("#btnSave").click(function () {

        $.blockUI({ message: '<div><img src="' + waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });

        var listingId = ListingId;        
        //var options = $('#option-filter').val();        
        var bodyStyles = $('#style-filter').val();
        var isCertified = $('#IsCertified').val();
        var isCarscom = $('#radioCarsCom').is(":checked");
        var isAuto = $('#radioAutoTrader').is(":checked");
        var isAllCar = $('#radioAll').is(":checked");
        var trims = $('#trim-filter').val();
        var isAll = $('#radioAll').is(":checked");
        var isFranchise = $('#franchise').is(":checked");
        var isIndependant = $('#independant').is(":checked");

        var url = "/Chart/SaveSelections?listingId=" + listingId + "&isCarsCom=" + isCarscom + "&options=" + bodyStyles + "&trims=" + trims + "&isCertified=" + isCertified + "&isAll=" + isAll + "&isFranchise=" + isFranchise + "&isIndependant=" + isIndependant + "&screen=" + vinCurrentScreen;

        $.ajax({
            type: "POST",
            url: url,
            data: {},
            success: function (results) {

                $('#Trims').val(trims.indexOf('0') >= 0 ? '0' : trims);
                $('#BodyStyles').val(bodyStyles.indexOf('0') >= 0 ? '0' : bodyStyles);
                $('#IsCertified').val(isCertified ? 'True' : 'False');
                $('#IsFranchise').val(isFranchise ? 'True' : 'False');
                $('#IsIndependant').val(isIndependant ? 'True' : 'False');
                $('#IsAll').val(isAll ? 'True' : 'False');
                if (ChartInfo.fRange.max == 100) {
                    UpdateCarRanking(vinCurrentScreen);
                }
                alert(results);
                $.unblockUI();

            }
        });
    });

}

function LoadSavedSelections() {
   
    if ($('#IsFranchise').val() == 'True') {
        ChartInfo.$filter.dealertype = 'franchise';
        $('#franchise').attr('checked', true);
        $('#franchise').parent().find('label').addClass('labelHighlight');
        $('#independant').attr('checked', false);
        $('#all').attr('checked', false);
        
    } else if ($('#IsIndependant').val() == 'True') {
        ChartInfo.$filter.dealertype = 'independant';
        $('#independant').attr('checked', true);
        $('#independant').parent().find('label').addClass('labelHighlight');
        $('#franchise').attr('checked', false);
        $('#all').attr('checked', false);
    } else {
        delete ChartInfo.$filter.dealertype;
        $('#independant').attr('checked', false);
        $('#franchise').attr('checked', false);
        $('#all').attr('checked', true);
    }
    if ($('#IsCertified').val().toLowerCase() == 'true') {
        $('#rdbAllCertified').attr('checked', false);
        $('#rdbUnCertified').attr('checked', false);
        $('#rdbCertified').attr('checked', true);
    }
    else if ($('#IsCertified').val().toLowerCase() == 'false') {
        $('#rdbAllCertified').attr('checked', false);
        $('#rdbCertified').attr('checked', false);
        $('#rdbUnCertified').attr('checked', true);
    } else {
        $('#rdbUnCertified').attr('checked', false);
        $('#rdbCertified').attr('checked', false);
        $('#rdbAllCertified').attr('checked', true);
    }

    if (default_trim != null) {
        ChartInfo.$filter.title = {}; ChartInfo.$filter.title.trim = [];
        if (default_trim.length == 1 && default_trim[0] == 0)
            delete ChartInfo.$filter.title;
        else {
            for (i = 0; i < default_trim.length; i++) {
                if (default_trim[i] != "") {
                    ChartInfo.$filter.title.trim.push(default_trim[i]);
                }
            }
        }
    }

    if (default_bodystyle != null) {
        ChartInfo.$filter.title = {}; ChartInfo.$filter.title.bodyStyle = [];
        if (default_bodystyle.length == 1 && default_bodystyle[0] == 0)
            delete ChartInfo.$filter.title;
        else {
            for (i = 0; i < default_bodystyle.length; i++) {
                if (default_bodystyle[i] != "") {
                    ChartInfo.$filter.title.bodyStyle.push(default_bodystyle[i]);
                }
            }
        }
    }
}

function LoadDefaulTrimsInLowerCase(defaultTrim) {
    var lowerCaseTrims = [];
    $.each(defaultTrim, function (index, obj) {
        lowerCaseTrims.push(obj.toLowerCase());
    });

    return lowerCaseTrims;
}

function LoadDefaulBodyStylesInLowerCase(defaultBodyStyle) {
    var lowerCaseTrims = [];
    $.each(defaultBodyStyle, function (index, obj) {
        lowerCaseTrims.push(obj.toLowerCase());
    });

    return lowerCaseTrims;
}

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function DownloadCSV() {
    $('#content').val(htmlEncode($("#printable-list")[0].innerHTML));
// ReSharper disable once Html.IdNotResolved
    $('#myform').submit();
}

function GetSelectedTrims() {
    var defaultTrims = [];
    ChartInfo.$filter.title = {}; ChartInfo.$filter.title.trim = [];
    for (i = 0; i < $('#trim-filter option').length; i++) {
        if ($('#trim-filter option')[i].selected && ($('#trim-filter option')[i].value != "")) {
            if (i == 0) {
                defaultTrims.push(i);
                delete ChartInfo.$filter.title;
                break;
            }
            ChartInfo.$filter.title.trim.push($('#trim-filter option')[i].value);
            defaultTrims.push($('#trim-filter option')[i].value);
        }
    }

    return defaultTrims;
}

function GetSelectedBodyStyles() {
    var defaultBodyStyles = [];
    ChartInfo.$filter.title = {}; ChartInfo.$filter.title.bodyStyle = [];
    for (i = 0; i < $('#style-filter option').length; i++) {
        if ($('#style-filter option')[i].selected && ($('#style-filter option')[i].value != "")) {
            if (i == 0) {
                defaultBodyStyles.push(i);
                delete ChartInfo.$filter.title;
                break;
            }
            ChartInfo.$filter.title.bodyStyle.push($('#style-filter option')[i].value);
            defaultBodyStyles.push($('#style-filte option')[i].value);
        }
    }
    
    return defaultBodyStyles;
}

function GetCheckedDealerType() {
    var dealerSelect = ChartHelper.getChecked('dealertype');
    if (dealerSelect.id != 'all') { ChartInfo.$filter.dealertype = dealerSelect.id; } else { delete ChartInfo.$filter.dealertype; }
}

function InitializeChart(isExcludeCurrentCar) {
    
    $data = [];
    var requestUrl = UrlPaths.requestNationwideUrl; //requestAutoTraderUrl;
    requestUrl = requestUrl.replace('PLACEHOLDER', ListingId);
    $.blockUI({ message: '<div><img src="' + ChartConfig.waitingImage + '"/></div>', css: { width: '400px', backgroundColor: 'none', border: 'none' } });
    $.ajax({
        type: "POST",
        url: requestUrl,
        data: {},
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var carlist = data.cars;
            var target = data.target;
            var marketinfo = data.market;
            var marketMapping = new VINControl.Chart.MarketMapping();
            
            if (carlist != null) {
                for (var i = 0; i < carlist.length; i++) {
                    var arr = carlist;
                    $data[i] = marketMapping.InitializeCar(arr[i]);
                }
            }
           
            $dCar = marketMapping.InitializeCurrentCar(target, marketinfo == null ? 0 : marketinfo.carsOnMarket);
            VINControl.Chart.SideBar.InitializeSidebar($dCar);

            default_trim = ($('#Trims').val() == '' || $('#Trims').val() == '0') ? [0] : $('#Trims').val().split(',');
            default_bodystyle = ($('#BodyStyles').val() == '' || $('#BodyStyles').val() == '0') ? [0] : $('#BodyStyles').val().split(',');
            // load saved selections
            if (!(default_trim.length == 1 && default_trim[0] == '0'))
                default_trim = LoadDefaulTrimsInLowerCase(default_trim);

            if (!(default_bodystyle.length == 1 && default_bodystyle[0] == '0'))
                default_bodystyle = LoadDefaulTrimsInLowerCase(default_bodystyle);
            
            LoadSavedSelections();
            
            dataTrims = data.trims;
            dataBodyStyles = data.bodyStyles;
            
            
            drawChart($data, ChartInfo.fRange, ChartInfo.$filter, $dCar, expand, default_trim, data.trims, data.bodyStyles, default_bodystyle/*, GetDataTrim(), isExcludeCurrentCar*/);
           
            // Settings for Truck
            if (!$dCar.commercialtruck) {
               
                $('#radioCommercialTruck').parent().hide();
                $('th[id="thCommercialTruck"]').hide();
                $('td[name="tdCommercialTruck"]').hide();
            }

            //$filter = {};
            // $.unblockUI();
            
            setTimeout(function () {
                
                appendFirstLoading();
            }, 1000);

        }
    });
}

function UpdateCarRanking(vinCurrentScreen) {
    var listingId = ListingId;
    
    var carRanking = 1;
    var numberOfCars = 0;
    if ($currentFilterredList != undefined && $currentFilterredList.length > 0) {
        numberOfCars = $currentFilterredList.length + 1;
        $.each($currentFilterredList, function (index, obj) {
            if ($dCar.price >= obj.price)
                carRanking++;
        });
        var prices = ChartHelper.extractVal($currentFilterredList, "price");
        var largestPrice = Math.max.apply(Math, prices);
        var smallestPrice = Math.min.apply(Math, prices);
        var averagePrice = Math.round(ChartHelper.totalprice($currentFilterredList) / $currentFilterredList.length);

        if (numberOfCars > 0) {
            $('#PdfContent').val(htmlEncode($("#printable-list")[0].innerHTML));
            var updateRankingUrl = "/Chart/UpdateCarRanking?listingId=" + listingId + "&carRanking=" + carRanking + "&numberOfCars=" + numberOfCars + "&oldCarRanking=" + $dCar.ranking + "&oldNumberOfCars=" + $dCar.carsOnMarket + "&smallestPrice=" + smallestPrice + "&averagePrice=" + averagePrice + "&largestPrice=" + largestPrice + "&screen=" + vinCurrentScreen;
            $.ajax({
                type: "POST",
                url: updateRankingUrl,
                data: {},
                success: function () {
              
                }
            });
        }
    }
}