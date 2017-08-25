var vehicleStatus = {
    appraisal: 1,
    inventory: 2,
    soldOut: 3
};

var CarSearchResultService = function () {
    var searchResult = [],
        loadShowMoreList = function(elLoading, type) {
            elLoading.show();
            //var data = searchResult;
            if (searchResult.length == 0) {
                var searchValue = $("#loSearchInput").val();
                $.ajax({
                    type: "POST",
                    url: "/Decode/FullTextSearchWithFullResult",
                    dataType: "json",
                    data: {
                        "searchTerm": searchValue,
                    },
                    cache: false,
                    success: function(data) {
                        searchResult = data;
                        renderShowMoreList(data, type,elLoading);
                    }
                });
            } else {
                renderShowMoreList(searchResult, type, elLoading);
            }
        },
        renderShowMoreList = function (data, type, elLoading) {
            var textType = '';
            type = parseInt(type);
            var baseLink = '';
            switch (type) {
                case vehicleStatus.appraisal:
                    textType = "Appraisal";
                    baseLink = '/Appraisal/ViewProfileForAppraisal?AppraisalId=';
                    break;
                case vehicleStatus.inventory:
                    textType = "Inventory";
                    baseLink = '/Inventory/ViewIProfile?ListingID=';
                    break;
                case vehicleStatus.soldOut:
                    textType = "Soldout";
                    baseLink = '/Inventory/ViewISoldProfile?ListingID=';
                    break;
            }

            elLoading.hide();

            var result = getVehicleResultByType(data, type);

            $("#leoResultNumber").html(result.length);
            $("#leoResultKeyword").html($("#loSearchInput").val());
            $("#leoResultType").html(textType);

            var html = '';
            $.each(result, function(index, item) {

                html += '<a href="' + baseLink + item.ListingId + '"><div class="losrCarsItems">';
                html += '<div class="losrCarImg">';
                html += '<img onError=" $(this).prop(\'src\',\'/Content/images/noImageAvailable.jpg\');" src="' + (item.PhotoUrl != null ? item.PhotoUrl : '/Content/images/noImageAvailable.jpg') + '"/>';
                html += '</div>';
                html += '<div class="losrCarInfo">';
                html += '<div class="losrInfoYmm">' + item.Year + " " + item.Make + " " + item.Model + '</div>';
                if (item.VehicleStatus != vehicleStatus.appraisal) {
                    html += '<div class="losrInfoItems">';
                    html += '<div class="losrInfoKey">Stock:</div><div class="losrInfoValue">#' + item.Stock + '</div>';
                    html += '</div>';
                }

                html += '<div class="losrInfoItems">';
                if (item.Trim) {
                    html += '<div class="losrInfoKey">Trim:</div><div class="losrInfoValue">' + item.Trim + '</div>';
                } else {
                    html += '<div class="losrInfoKey">Trim:</div><div class="losrInfoValue">Not specificed</div>';
                }

                html += '</div>';
                html += '<div class="losrInfoItems">';
                html += '<div class="losrInfoKey">Exterior Color:</div><div class="losrInfoValue">' + item.ExteriorColor + '</div>';
                html += '</div>';

                html += '</div></div></a>';


            });
            $("#loShowMoreList").html(html);
        },
        getVehicleResultByType = function(data, type) {
            var tempArray = [];
            $.each(data, function(index, item) {
                if (item.VehicleStatus == type) {
                    tempArray.push(item);
                }
            });

            return tempArray;
        },
        renderResultList = function(data) {
            var html = '', htmlInventory = '', htmlAppraisal = '', htmlSoldOut = '';

            var appraisalCount = 0;
            var inventoryCount = 0;
            var soldoutCount = 0;

            $.each(data, function(index, item) {
                var baseLink = '';

                switch (item.VehicleStatus) {
                case vehicleStatus.appraisal:
                    appraisalCount++;
                    if (appraisalCount >= 4) return;
                    baseLink = '/Appraisal/ViewProfileForAppraisal?AppraisalId=';
                    break;
                case vehicleStatus.inventory:
                    inventoryCount++;
                    if (inventoryCount >= 4) return;
                    baseLink = '/Inventory/ViewIProfile?ListingID=';
                    break;
                case vehicleStatus.soldOut:
                    soldoutCount++;
                    if (soldoutCount >= 4) return;
                    baseLink = '/Inventory/ViewISoldProfile?ListingID=';
                    break;
                }

                html = '';
                html += '<a href="' + baseLink + item.ListingId + '"><div class="losrCarsItems">';
                html += '<div class="losrCarImg">';
                html += '<img onError=" $(this).prop(\'src\',\'/Content/images/noImageAvailable.jpg\');" src="' + (item.PhotoUrl != null ? item.PhotoUrl : '/Content/images/noImageAvailable.jpg') + '"/>';
                html += '</div>';
                html += '<div class="losrCarInfo">';
                html += '<div class="losrInfoYmm">' + item.Year + " " + item.Make + " " + item.Model + '</div>';
                if (item.VehicleStatus != vehicleStatus.appraisal) {
                    html += '<div class="losrInfoItems">';
                    html += '<div class="losrInfoKey">Stock:</div><div class="losrInfoValue">#' + item.Stock + '</div>';
                    html += '</div>';
                }

                html += '<div class="losrInfoItems">';
                if (item.Trim) {
                    html += '<div class="losrInfoKey">Trim:</div><div class="losrInfoValue">' + item.Trim + '</div>';
                } else {
                    html += '<div class="losrInfoKey">Trim:</div><div class="losrInfoValue">Not specificed</div>';
                }

                html += '</div>';
                html += '<div class="losrInfoItems">';
                html += '<div class="losrInfoKey">Exterior Color:</div><div class="losrInfoValue">' + item.ExteriorColor + '</div>';
                html += '</div>';

                html += '</div></div></a>';

                switch (item.VehicleStatus) {
                case vehicleStatus.appraisal:
                    htmlAppraisal += html;
                    break;
                case vehicleStatus.inventory:
                    htmlInventory += html;
                    break;
                case vehicleStatus.soldOut:
                    htmlSoldOut += html;
                    break;
                }
            });

            if (htmlAppraisal != '') {
                if (appraisalCount > 3)
                    htmlAppraisal += '<div type="1" class="losrShowmore"> Show all results</div><div class="showMoreLoading"></div>';
                $("#loAppraisalListSearch").html(htmlAppraisal);
                $("#loAppraisalHolder").show();
            } else {
                $("#loAppraisalHolder").hide();
            }
            if (htmlInventory != '') {
                if (inventoryCount > 3)
                    htmlInventory += '<div type="2" class="losrShowmore"> Show all results</div><div class="showMoreLoading"></div>';
                $("#loInventoryListSearch").html(htmlInventory);
                $("#loInventoryHolder").show();
            } else {
                $("#loInventoryHolder").hide();
            }
            if (htmlSoldOut != '') {
                if (soldoutCount > 3)
                    htmlSoldOut += '<div type="3" class="losrShowmore"> Show all results</div><div class="showMoreLoading"></div>';
                $("#loSoldoutListSearch").html(htmlSoldOut);
                $("#loSoleOutHolder").show();
            } else {
                $("#loSoleOutHolder").hide();
            }
        },
        registerEventHandler = function() {
            $("#loSearchInput").keyup(function(event) {
                if (event.keyCode == 13) {
                    $("#loSearchBtn").trigger("click");
                }
            });

            $(document).mouseup(function(e) {
                var searchPopup = $("#loSearchResult");
                var showMorePoup = $("#loShowMoreResult");
                if (searchPopup.has(e.target).length === 0) {
                    var temp = showMorePoup.has(e.target).length;
                    if (!showMorePoup.is(e.target) && (temp === 0 && e.target.id != "loShowMoreResult" && e.target.id != "loSearchBtn" && e.target.id != "loSearchInput")) {
                        $("#loShowMoreResult").animate({ 'width': 0 }, 500);
                        setTimeout(function() {
                            $("#loShowMoreResult").hide();
                            $("#loSearchResult").slideUp();
                        }, 500);
                    }
                }
            });

            $("#loSearchBtn").click(function() {
                $("#searchImgLoading").show();
                var searchValue = $("#loSearchInput").val();
                if (!searchValue) {
                    $("#loSearchInput").focus();
                    return false;
                }

                $("#loSearchResult").slideDown();
                $("#loShowMoreResult").hide();
               
                $.ajax({
                    type: "POST",
                    url: "/Decode/FullTextSearch",
                    dataType: "json",
                    data: {"searchTerm": searchValue},
                    cache: false,
                    success: function(data) {
                        searchResult = [];
                        $("#losrKeyword").text(searchValue);

                        if (data.length == 0) {
                            $("#loNoResultHolder").show();
                            $("#loAppraisalHolder").hide();
                            $("#loInventoryHolder").hide();
                            $("#loSoleOutHolder").hide();
                            $("#searchImgLoading").hide();
                        } else {
                            $("#loNoResultHolder").hide();

                            renderResultList(data);

                            $("#searchImgLoading").hide();
                        }
                        return true;
                    }
                });

                return true;
            });

            $(".losrShowmore").live("click",function () {
                $("#loShowMoreResult").show();
                var type = $(this).attr("type");
                var elLoading = $(this).parent().find(".showMoreLoading");
                loadShowMoreList(elLoading, type);
                $("#loShowMoreResult").animate({ 'width': 620 }, 500);
            });

           

        };
    return { registerEventHandler: registerEventHandler };
}

$(document).ready(function () {
  
    var carSearchResultService = new CarSearchResultService();
    carSearchResultService.registerEventHandler();
});


function GetCsharpDate(date) {

    var re = /-?\d+/;
    var m = re.exec(date);
    var d = new Date(parseInt(m[0]));
    var shortDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
    return shortDate;
}

function GetCsharpTime(date) {

    var re = /-?\d+/;
    var m = re.exec(date);
    var d = new Date(parseInt(m[0]));
    var shortTime = formatAMPM(d);
    return shortTime;
}

function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;
    var strTime = hours + ':' + minutes + ":" + seconds + ' ' + ampm;
    return strTime;
}

function formatFullDate(date) {
    var re = /-?\d+/;
    var m = re.exec(date);
    var d = new Date(parseInt(m[0]));
    var shortDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
    var shortTime = formatAMPM(d);
    return shortDate + " " + shortTime;
}

function CommaFormatted(amount) {
    amount = amount.toString();
    amount = amount.replace(/^0+/, '');
    amount += '';
    var x = amount.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}