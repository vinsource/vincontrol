var VINControl = VINControl || {};
VINControl.Chart = VINControl.Chart || {};

VINControl.Chart.GridView = function() {
    var popSmalllist = function (list, range, dcar, isIncludeCurrentCar) {
        var extraList = [];
        for (var it = 0; it < list.length; it++)
            extraList[it] = list[it];
        if (dcar != null && isIncludeCurrentCar) {
            if (extraList.length > 0)
                extraList[extraList.length] = dcar;
            else
                extraList[0] = dcar;
        }
       
        // make sure we have table with id 'tblVehicles' exists
        if ($("table#tblVehicles") == undefined || $("table#tblVehicles") == null)
            return false;

        //var title = [];
        //var filteredList = [];
        $("table#tblVehicles").html("");

        //var count = 1;
        if ($("#marketChartHeaderTemplate").length) {
            var header = $("#marketChartHeaderTemplate").render();
            $("#NumberofCarsOnTheChart").html("List of " + extraList.length + " Charted Vehicles");
            var body = $("#smallMarketChartTemplate").render(extraList);
            $("table#tblVehicles").html(header + '<tbody>' + body + '</tbody>');
            
        }

        if (ListingId != undefined) {
            $("table#tblVehicles #" + ListingId).attr("class", "highlight");
        }
        // only enable the table when having at least 1 record
        if (list.length > 0) {
            $("table#tblVehicles").trigger("update");
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
            $("table#tblVehicles").show();
        }
        if (typeof (ChartInfo.selectedId) !== 'undefined') {
            var item = document.getElementById(ChartInfo.selectedId);
            if (item != null) {
                plotSelectedPoint(item);
            }
        }
        return true;
    },
        popList = function (list, dcar, selectedId, isIncludeCurrentCar) {
            //var extraList = [];
            //for (var it = 0; it < list.length; it++)
            //    extraList[it] = list[it];
            if ($("table#tblVehicles") == undefined || $("table#tblVehicles") == null)
                return false;


            $("table#tblVehicles").html("");
            $("table#tblsimilarVehicles").html("");

            var exactMatchList = [];
            var similartMatchList = [];

            if (dcar != null) {


                var matchResult = ExactvsSimilarMatchlist(list, dcar);

                exactMatchList = matchResult.exact;
                similartMatchList = matchResult.similar;
            }

            if (exactMatchList.length <= 0) {
                for (var i = 0; i < list.length; i++) {
                    exactMatchList.push(list[i]);
                }
                if (dcar != null && isIncludeCurrentCar) {
                    exactMatchList.push(dcar);
                }

            } else {
                $('#NumberofCarsOnTheChart').text('List of ' + (exactMatchList.length) + ' Exact Match Charted Vehicles');
                $('#NumberofSimilarCarsOnTheChart').text('List of ' + (similartMatchList.length) + ' Similar Match Charted Vehicles');

                if (dcar != null && isIncludeCurrentCar) {
                    exactMatchList.push(dcar);
                }
                if ($("#marketChartHeaderTemplate").length) {
                    var similarheader = $("#marketChartHeaderTemplate").render();
                    var similarbody = $("#marketChartTemplate").render(similartMatchList);
                    $("table#tblsimilarVehicles").html(similarheader + '<tbody>' + similarbody + '</tbody>');
                    if (similartMatchList.length > 0) {
                        $("#similarlistholder").show();
                        
                    }

                }
            }

            if ($("#marketChartHeaderTemplate").length) {
                var header = $("#marketChartHeaderTemplate").render();
                var body = $("#marketChartTemplate").render(exactMatchList);
                $("table#tblVehicles").html(header + '<tbody>' + body + '</tbody>');
            }
            if (ListingId != undefined) {
                $("table#tblVehicles #" + ListingId).attr("class", "highlight");
            }
            // only enable the table when having at least 1 record
            //if (list.length > 0 || extraList.length > 0) {
            if (exactMatchList.length > 0) {
                if ($("table#tblVehicles").length) {
                    $("table#tblVehicles").trigger("update");
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
                    $("table#tblVehicles").show();
                }
            }

            //if (list.length > 0 || extraList.length > 0) {
            if (similartMatchList.length > 0) {
                if ($("table#tblsimilarVehicles").length) {
                    $("table#tblsimilarVehicles").trigger("update");
                    $("table#tblsimilarVehicles").tablesorter({
                        // prevent first column from being sortable
                        headers: {
                            0: { sorter: false },
                            7: { sorter: 'price' }, // miles
                            8: { sorter: 'price', sortInitialOrder: 'asc' }// prices
                        },
                        // apply custom widget
                        widgets: ['numbering']
                    });
                    $("table#tblsimilarVehicles").show();
                }
            }

            if (typeof(selectedId) !== 'undefined') {
                var item = document.getElementById(selectedId);
                if (item != null) {
                    plotSelectedPoint(item);
                }
            }

            return true;
        };
    return { pop_smalllist: popSmalllist, pop_list: popList };
};



