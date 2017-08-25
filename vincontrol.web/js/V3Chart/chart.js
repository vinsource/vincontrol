var ChartInfo = ChartInfo || { selectedId: 0, $filter: {}, fRange: { min: 0, max: 100 }, isSoldView: false, isSmallChart: false };
var fillcolorConstanst = "rgba(0,0,255,.4)";

var VincontrolChartData;
var MarketChart = function (argsPara) {
    var args = argsPara;
    var drawMarketChart = function () {
        VincontrolChartData = args;
        //console.log("start Market chart");
        //console.log(args.filter);
        //    console.log(args.marketList);
        var plotArea = { x: 55, y: 20 };
        var xAxisOffset = 65;
        var xAxisLabelSpace = 20;

        //    console.log(args);
        var isSmallView = args.isSmallView;
        var isIncludeCurrentCar = args.isIncludeCurrentCar;

        var dragged = false;
        var dragOrigin = 0;

        var cloneArray = args.marketList.slice(0);

        if (args.baseVehicle != null)
            cloneArray.push(args.baseVehicle);

        //    console.log(args.baseVehicle);
        var chartConf = new Metrics(cloneArray);

        var regression = new Regression(args.marketList);

        // creates metric object and regression information
        // (will be given by json in future)
        //    var chart_conf = new Metrics(args.marketList);
        //    var regression = new Regression(args.marketList);

        // select draw div and set up drawing space
        var body = d3.select(args.chartDiv);
        body.html("");
        var svg = body.append('svg')
            .attr('height', args.dimensions[1])
            .attr('width', args.dimensions[0]);

        // mask for chart lines, keeps them
        // from overflowing into the axis
        var mask = svg.append('clipPath')
            .attr('id', 'chart-mask');


        mask.append('rect')
            .attr('x', plotArea.x)
            .attr('y', plotArea.y)
            .attr('width', args.dimensions[0] - plotArea.x * 1.5)
            .attr('height', args.dimensions[1] - plotArea.y - xAxisOffset - xAxisLabelSpace)
            .attr('style', 'fill: none;');


        // set scaling of data to viewport size
        var milesScale, priceScale, scaleToPrice;

        // set scaling of data to viewport size
        milesScale = d3.scale.linear()
            .domain([chartConf.getLowest('miles'), chartConf.getHighest('miles')])
            .range([plotArea.x, args.dimensions[0] - plotArea.x]);
        priceScale = d3.scale.linear()
            .domain([chartConf.getHighest('price') * 1.2, chartConf.getLowest('price') / 1.2])
            .range([plotArea.y, args.dimensions[1] - plotArea.y - xAxisOffset]);
        scaleToPrice = d3.scale.linear()
            .domain([plotArea.y, args.dimensions[1] - plotArea.y - xAxisOffset])
            .range([chartConf.getHighest('price') * 1.2, chartConf.getLowest('price') / 1.2]);

        // set drag event
        var drag = d3.behavior.drag()
            .origin(function () {
                var t = d3.select(this);
                var origin = { x: t.attr("cx"), y: t.attr("cy") };
                dragOrigin = origin;
                return origin;
            });

        if ($("#IsEmployee").val() == "False") {
            drag.on("drag", function (d) {
                ///////////////////////////////////
                /* ------------------------------ /
                DURING DRAG EVENT
    
                If you want anything to happen
                during the dragging of the dealer
                car, place it here.
    
                / ------------------------------ */
                ///////////////////////////////////
                d.price = Math.floor(scaleToPrice(d3.event.y));
                if (d.price < 0) {
                    d.price = 0;
                }
                if (d.price < chartConf.getHighest('price') && d.price > chartConf.getLowest('price')) {
                    d3.select(this)
                        .attr("cy", d3.event.y);
                }
                dragged = true;
            }).on('dragend', function (d) {

                if (dragged && typeof dragged != undefined) {

                    ///////////////////////////////////
                    /* ------------------------------ /
                    END OF DRAG EVENT
    
                    You can bind actions for when they
                    are done moving the dealer car.
                    So when they finish, this is where
                    you'd put the events for them to
                    confirm the price change.
    
                    / ------------------------------ */
                    ///////////////////////////////////


                    var check = confirm("You have changed the pricing of this vehicle to $" + ChartHelper.addCommas(d.price) + " Would you like to save the changes?");

                    if (!check) {
                        d3.select(this).attr("cy", dragOrigin.y);
                        d.price = scaleToPrice(dragOrigin.y);
                    } else {
                        //                dealerCar.price = d.price;
                        UpdateSalePrice(d.price);
                    }
                }
                dragged = false;
            });
        }

        //    var formatPrice = d3.format();

        // configure axis scaling
        var xAxis = d3.svg.axis()
            .scale(milesScale)
            .orient("bottom")
            .tickFormat(function (d) {
                return Math.floor(d / 1000) + 'k';
            });
        var yAxis = d3.svg.axis()
            .scale(priceScale)
            .orient("left")
            .tickFormat(function (d) {
                return '$' + Math.floor(d / 1000) + 'k';
            });
        // grid setup
        var xGrid = d3.svg.axis()
            .scale(milesScale)
            .orient("top")
            .ticks(15)
            .tickSize(-args.dimensions[0], 0, 0)
            .tickFormat("");
        var yGrid = d3.svg.axis()
            .scale(priceScale)
            .orient("left")
            .ticks(10)
            .tickSize(-args.dimensions[0], 0, 0)
            .tickFormat("");

        var yValue = Math.floor(regression.getY(chartConf.getLowest('miles')));
        //console.log(yValue);
        //    console.log((priceScale(yValue)));
        //    console.log((priceScale(yValue * 0.975)));
        //    console.log(priceScale(yValue * 0.975)-(priceScale(yValue)) * 2);
        // add market ranges
        //    var tooFar = 
        svg.append("line")
            .attr("x1", milesScale(-10000000))
            .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
            .attr("x2", milesScale(chartConf.getHighest('miles') * 2))
            .attr("y2", priceScale(Math.floor(regression.getY(chartConf.getHighest('miles') * 2))))
            .attr("stroke-width", 10000)
            .attr("stroke", "rgba(255, 100, 100,0.8)")
            .attr('style', 'clip-path: url(#chart-mask);');

        //        var farFrom =
        svg.append("line")
            .attr("x1", milesScale(-10000000))
            .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
            .attr("x2", milesScale(chartConf.getHighest('miles') * 2))
            .attr("y2", priceScale(Math.floor(regression.getY(chartConf.getHighest('miles') * 2))))
            .attr("stroke-width", (priceScale(yValue * 0.8) - priceScale(yValue)) * 2)
            .attr("stroke", "rgba(246, 255, 100,0.8)")
            .attr('style', 'clip-path: url(#chart-mask);');

        //            var atMarket = 
        svg.append("line")
            .attr("x1", milesScale(-10000000))
            .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
            .attr("x2", milesScale(chartConf.getHighest('miles') * 2))
            .attr("y2", priceScale(Math.floor(regression.getY(chartConf.getHighest('miles') * 2))))
            .attr("stroke-width", (priceScale(yValue * 0.975) - priceScale(yValue)) * 2)
            .attr("stroke", "rgba(100, 255, 100,0.8)")
            .attr('style', 'clip-path: url(#chart-mask);');

        //                var target = 
        svg.append("line")
            .attr("x1", milesScale(-10000000))
            .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
            .attr("x2", milesScale(chartConf.getHighest('miles') * 2))
            .attr("y2", priceScale(Math.floor(regression.getY(chartConf.getHighest('miles') * 2))))
            .attr("stroke-width", 2)
            .attr("stroke", "green")
            .attr('style', 'clip-path: url(#chart-mask);');

        // add axis to chart
        var xAxisGroup, yAxisGroup;

        // add axis to chart
        xAxisGroup = svg.append("g")
            .attr("class", "xAxis axis")
            .attr("transform", "translate(0," + (args.dimensions[1] - xAxisOffset) + ")")
            .call(xAxis);
        yAxisGroup = svg.append("g")
            .attr("class", "yAxis axis")
            .attr("transform", "translate(" + plotArea.x * 4 / 7 + ", 0)")
            .call(yAxis);

        // add grid
        //  var xGridGroup = 
        svg.append("g")
            .attr("class", "xAxis grid")
            .call(xGrid)
            .attr('style', 'clip-path: url(#chart-mask);');
        //        var yGridGroup = 
        svg.append("g")
            .attr("class", "yAxis grid")
            .call(yGrid)
            .attr('style', 'clip-path: url(#chart-mask);');

        // add labels
        svg.append("text")      // text label for the x axis
            .attr("x", args.dimensions[0] / 1.9)
            .attr("y", args.dimensions[1] - xAxisOffset - 5)
            .style("text-anchor", "middle")
            .text("Odometer (Miles)")
            .style('font-size', isSmallView ? '0.7em' : '1em')
            .style('font-family', 'Arial')
            .style('font-weight', 'bold');
        svg.append("text")
            .attr("transform", "rotate(-90)")
            .attr("y", plotArea.x - 5)
            .attr("x", xAxisOffset - (args.dimensions[1] / 2))
            .style("text-anchor", "middle")
            .style('font-size', isSmallView ? '0.7em' : '1em')
            .style('font-family', 'Arial')
            .style('font-weight', 'bold')
            .text("Sale Price ($)");

        // sort data by distance, draw farthest first
        // and closest last.
        args.marketList.sort(function (a, b) {
            return b.distance - a.distance; // DESC
        });

        var carscomList = [], autoTraderlist = [], carmaxList = [], commercialTrucklist = [], list = [];
        for (var i = 0; i < args.marketList.length; i++) {
            //if (args.marketList[i].carscom && args.marketList[i].autotrader && (!args.baseVehicle.commercialtruck || (args.baseVehicle.commercialtruck && args.marketList[i].commercialtruck))) {
            if (args.marketList[i].carscom && args.marketList[i].autotrader && args.marketList[i].carmax) {
                list.push(args.marketList[i]);
            } else if (args.marketList[i].carscom) {
                carscomList.push(args.marketList[i]);
            } else if (args.marketList[i].autotrader) {
                autoTraderlist.push(args.marketList[i]);
            } else if (args.marketList[i].carmax) {
                carmaxList.push(args.marketList[i]);
            } else if (args.marketList[i].commercialtruck) {
                commercialTrucklist.push(args.marketList[i]);
            }
        }

        SetPointShape(svg, carscomList, 'rgba(0,0,255,1)', args, milesScale, priceScale);
        SetPointShape(svg, autoTraderlist, 'rgba(0,0,0,1)', args, milesScale, priceScale);
        SetPointShape(svg, carmaxList, 'rgba(245,245,245,1)', args, milesScale, priceScale);
        SetPointShape(svg, commercialTrucklist, 'rgba(0,135,0,1)', args, milesScale, priceScale);
        var mergedGroup;
        if (args.filter.marketSource === "carscom") {
            mergedGroup = SetPointShape(svg, list, 'rgba(0,0,255,1)', args, milesScale, priceScale);
        } else if (args.filter.marketSource === "autotrader") {
            mergedGroup = SetPointShape(svg, list, 'rgba(0,0,0,1)', args, milesScale, priceScale);
        } else if (args.filter.marketSource === "carmax") {
            mergedGroup = SetPointShape(svg, list, 'rgba(245,245,245,1)', args, milesScale, priceScale);
        } else if (args.filter.marketSource === "commercialtruck") {
            mergedGroup = SetPointShape(svg, list, 'rgba(0,135,0,1)', args, milesScale, priceScale);
        } else {
            mergedGroup = SetPointShape(svg, list, 'rgba(255,0,0,1)', args, milesScale, priceScale);
        }
       
        setTimeout(resetAndSetSelectedPoint(ChartInfo.selectedId), 500);

        if (args.baseVehicle != null && isIncludeCurrentCar) {
            // create circle group element
            var dCarGroup = svg.append("g");
            // set up circle elements for each vehicle
            // and bind data to the elements
            var baseVehicleArray = [];
            baseVehicleArray.push(args.baseVehicle);
            var dCar = dCarGroup.selectAll('circle')
                .data(baseVehicleArray)
                .enter();


            // draw circles.
            var dCarCircle =
                dCar
                    .append('circle')
                    .attr('class', 'circle')
                    .attr('cx', function (d) { return milesScale(d.miles); })
                    .attr('cy', function (d) { return priceScale(d.price); })
                    .attr('r', 15)
                    .style('fill', "rgba(255,255,255,1)")
                    .style('stroke-width', 2)
                    .style('stroke', 'rgba(0,0,255,1)');
            if (!isSmallView)
                dCarCircle.call(drag);
        }
    };
    return { drawMarketChart: drawMarketChart };

};

function UpdateSalePrice(price) {
    $.post(updateSalePrice, { ListingId: ListingId, SalePrice: price.toFixed(0), vehicleStatusCodeId: vinCurrentScreen }, function (data) {

        if (data.SessionTimeOut == "TimeOut") {
            alert("Your session has timed out. Please login back again");
            window.parent.location = logOff;
        } else {
            window.parent.location = detailUrl.replace('PLACEHOLDER', ListingId);
        }
    });
}

function SetPointShape(svg, dataSource, strokeColor, args, milesScale, priceScale) {
    // create circle group element

    // set up circle elements for each vehicle
    // and bind data to the elements

    // draw circles.
    //var circleAttributes =
    //    circles
    //.append('circle')
    //.attr('id', function (d) { return "point" + d.listingid; })
    //.attr('class', 'circle circlePoint')
    //.attr('cx', function (d) { return milesScale(d.miles); })
    //.attr('cy', function (d) { return priceScale(d.price); })
    //.attr('r', 3)
    //.style('fill', fillcolorConstanst)
    //.style('stroke-width', 2)
    //.style('stroke', strokeColor);



    var chunk_num = Math.floor(dataSource.length / 1000);
    var data_chunks = array_split(dataSource, chunk_num);
    rotator(data_chunks, addToChart, [svg, milesScale, priceScale, strokeColor, args]);
    //return { circleGroup: circleAttributes, strokeColor: strokeColor };

}

function addToChart(dataGroup, arg) {
    var svg = arg[0];
    var milesScale = arg[1];
    var priceScale = arg[2];
    var strokeColor = arg[3];
    var args = arg[4];
    //setTimeout(
    //    function () {
    if (dataGroup) {
        var circleGroup = svg.append("g");
        var circles = circleGroup.selectAll('circle')
            .data(dataGroup)
            .enter();
        var circleAttributes =
            circles
                .append('circle')
                .attr('id', function (d) { return "point" + d.listingid; })
                .attr('class', 'circle circlePoint')
                .attr('cx', function (d) { return milesScale(d.miles); })
                .attr('cy', function (d) { return priceScale(d.price); })
                .attr('r', 3)
                .style('fill', fillcolorConstanst)
                .style('stroke-width', 2)
                .style('stroke', strokeColor);
        circleAttributes.on("click", function (d) {
            ChartInfo.selectedId = d.listingid;
            VINControl.Chart.SideBar.refreshSidebar(d, args.baseVehicle, args.marketList);
            resetSelected();
            d3.select(this).style('fill', "white")
                .attr("r", 6)
                .style('stroke-width', 5)
                .style('stroke', strokeColor);
            plotSelectedPoint(document.getElementById(ChartInfo.selectedId));
        });
        //}
        //, 100);
    }
};

function resetShape(colorGroupSetting) {

    colorGroupSetting.circleGroup
     .style('fill', fillcolorConstanst)
        .style('stroke-width', 2)
        .attr("r", 3);
}

function BindClickEventsForGroup(groups, args, isSmallView) {
    for (var i = 0; i < groups.length; i++) {
        BindClickEvents(groups, groups[i], args, isSmallView);
    }
}

function BindClickEvents(groups, colorGroupSetting, args, isSmallView) {

    if (!isSmallView)
        colorGroupSetting.circleGroup.on("click", function (d) {
            ChartInfo.selectedId = d.listingid;
            VINControl.Chart.SideBar.refreshSidebar(d, args.baseVehicle, args.marketList);
            resetSelected();
            d3.select(this).style('fill', "white")
                .attr("r", 6)
                .style('stroke-width', 5)
                .style('stroke', colorGroupSetting.strokeColor);
            plotSelectedPoint(document.getElementById(ChartInfo.selectedId));
        });
}

function resetAll(groups) {
    for (var i = 0; i < groups.length; i++) {
        resetShape(groups[i]);
    }
};

function setSelectedPoint(pointid) {
    //console.log('before set', pointid);
    if (typeof (pointid) !== 'undefined') {
        //d3.select("#point" + pointid).on("click")(d3.select("#point" + pointid)[0][0]["__data__"]);
        if (d3.select("#point" + pointid)[0][0] != null) {
            d3.select("#point" + pointid).style('fill', "white").attr("r", 6)
                .style('stroke-width', 5);
            VINControl.Chart.SideBar.refreshSidebar(d3.select("#point" + pointid)[0][0]["__data__"], VincontrolChartData.baseVehicle, VincontrolChartData.marketList);
        } else {
            $('#carInfo').hide();
            $('#divNoCarInfo').show();
        }
    } else {
        $('#carInfo').hide();
        $('#divNoCarInfo').show();
    }
}

function resetSelected() {
    d3.selectAll('.circlePoint').style('fill', fillcolorConstanst)
        .style('stroke-width', 2)
        .attr("r", 3);
}

function resetAndSetSelectedPoint(pointid) {
    resetSelected();
    setSelectedPoint(pointid);
}
