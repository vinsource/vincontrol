var MarketChart = function (args) {

//    console.log(args);

    dragged = false;
    dragOrigin = 0;

//    var cloneArray = args.marketList.slice(0);
//    cloneArray.push(args.baseVehicle);
//    var chart_conf = new Metrics(cloneArray);

//    var regression = new Regression(args.marketList);

    // creates metric object and regression information
    // (will be given by json in future)
        var chart_conf = new Metrics(args.marketList);
        var regression = new Regression(args.marketList);

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
    var maskRectangle = mask.append('rect')
    .attr('x', 100)
    .attr('y', 50)
    .attr('width', args.dimensions[0] - 115)
    .attr('height', args.dimensions[1] - 100)
    .attr('style', 'fill: none;');

    // set scaling of data to viewport size
    var milesScale = d3.scale.linear()
    .domain([chart_conf.getLowest('miles'), chart_conf.getHighest('miles')])
    .range([100, args.dimensions[0] - (args.dimensions[0] * 0.02)]);
    var priceScale = d3.scale.linear()
    .domain([chart_conf.getHighest('price'), chart_conf.getLowest('price')])
    .range([50, args.dimensions[1] - (args.dimensions[0] * 0.083)]);
    var scaleToPrice = d3.scale.linear()
    .domain([50, args.dimensions[1] - (args.dimensions[0] * 0.083)])
    .range([chart_conf.getHighest('price'), chart_conf.getLowest('price')]);

    // set drag event
    var drag = d3.behavior.drag()
    .origin(function () {
        var t = d3.select(this);
        var origin = { x: t.attr("cx"), y: t.attr("cy") };
        dragOrigin = origin;
        return origin;
    })
    .on("drag", function (d, i) {


        ///////////////////////////////////
        /* ------------------------------ /
        DURING DRAG EVENT

        If you want anything to happen
        during the dragging of the dealer
        car, place it here.

        / ------------------------------ */
        ///////////////////////////////////
        d.price = Math.floor(scaleToPrice(d3.event.y));
        if (d.price < 0) { d.price = 0; }
        if (d.price < chart_conf.getHighest('price') && d.price > chart_conf.getLowest('price')) {
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
            if (!window.confirm('Do you want to change the price to: $' + d.price + '?')) {

                d3.select(this)
            .attr("cy", dragOrigin.y);

                d.price = scaleToPrice(dragOrigin.y);
            } else {
                dealerCar.price = d.price;
            }
        }
        dragged = false;
    });

    var formatPrice = d3.format()

    // configure axis scaling
    var xAxis = d3.svg.axis()
    .scale(milesScale)
    .orient("bottom")
    .tickFormat(function (d, i) {
        return Math.floor(d / 1000) + 'k';
    });
    var yAxis = d3.svg.axis()
    .scale(priceScale)
    .orient("left")
    .tickFormat(function (d, i) {
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

    // add market ranges
    var tooFar = svg.append("line")
    .attr("x1", milesScale(-10000000))
    .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
    .attr("x2", milesScale(chart_conf.getHighest('miles') * 2))
    .attr("y2", priceScale(Math.floor(regression.getY(chart_conf.getHighest('miles') * 2))))
    .attr("stroke-width", 10000)
    .attr("stroke", "rgba(255, 100, 100,0.8)")
    .attr('style', 'clip-path: url(#chart-mask);');

    var farFrom = svg.append("line")
    .attr("x1", milesScale(-10000000))
    .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
    .attr("x2", milesScale(chart_conf.getHighest('miles') * 2))
    .attr("y2", priceScale(Math.floor(regression.getY(chart_conf.getHighest('miles') * 2))))
    .attr("stroke-width", 150)
    .attr("stroke", "rgba(246, 255, 100,0.8)")
    .attr('style', 'clip-path: url(#chart-mask);');

    var atMarket = svg.append("line")
    .attr("x1", milesScale(-10000000))
    .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
    .attr("x2", milesScale(chart_conf.getHighest('miles') * 2))
    .attr("y2", priceScale(Math.floor(regression.getY(chart_conf.getHighest('miles') * 2))))
    .attr("stroke-width", 25)
    .attr("stroke", "rgba(100, 255, 100,0.8)")
    .attr('style', 'clip-path: url(#chart-mask);');

    var target = svg.append("line")
    .attr("x1", milesScale(-10000000))
    .attr("y1", priceScale(Math.floor(regression.getY(-10000000))))
    .attr("x2", milesScale(chart_conf.getHighest('miles') * 2))
    .attr("y2", priceScale(Math.floor(regression.getY(chart_conf.getHighest('miles') * 2))))
    .attr("stroke-width", 2)
    .attr("stroke", "green")
    .attr('style', 'clip-path: url(#chart-mask);');

    // add axis to chart
    var xAxisGroup = svg.append("g")
    .attr("class", "xAxis axis")
    .attr("transform", "translate(0," + (args.dimensions[1] - 25) + ")")
    .call(xAxis);
    var yAxisGroup = svg.append("g")
    .attr("class", "yAxis axis")
    .attr("transform", "translate(" + 75 + ", 0)")
    .call(yAxis);


    // add grid
    var xGridGroup = svg.append("g")
    .attr("class", "xAxis grid")
    .call(xGrid)
    .attr('style', 'clip-path: url(#chart-mask);');
    var yGridGroup = svg.append("g")
    .attr("class", "yAxis grid")
    .call(yGrid)
    .attr('style', 'clip-path: url(#chart-mask);');


    // add labels
    svg.append("text")      // text label for the x axis
        .attr("x", args.dimensions[0] / 1.9)
        .attr("y", args.dimensions[1] - 35)
        .style("text-anchor", "middle")
        .text("Odometer (Miles)")
        .style('font-size', '1em')
        .style('font-family', 'Arial')
        .style('font-weight', 'bold');
    svg.append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 92)
        .attr("x", 0 - (args.dimensions[1] / 2))
        .style("text-anchor", "middle")
        .style('font-size', '1em')
        .style('font-family', 'Arial')
        .style('font-weight', 'bold')
        .text("Sale Price ($)");


    // sort data by distance, draw farthest first
    // and closest last.
    args.marketList.sort(function (a, b) {
        return b.distance - a.distance; // DESC
    });

    // create circle group element
    var circleGroup = svg.append("g");
    // set up circle elements for each vehicle
    // and bind data to the elements
    var circles = circleGroup.selectAll('circle')
    .data(args.marketList)
    .enter();
    // draw circles.
    var circleAttributes = circles
    .append('circle')
    .attr('class', 'circle')
    .on("click", function (d, i) {

        ///////////////////////////////////
        /* ------------------------------ /
        CLICK EVENT FOR MARKET VEHICLES

        Here you can bind actions to the
        click event on the chart for
        clicking on a car. You can access
        the cars data by using d.{property},
        ie d.miles for mileage or d.vin for
        vin number.

        / ------------------------------ */
        ///////////////////////////////////
        //console.log(d);

        updateSidebar(d, $(args.sidebar), args.baseVehicle);

        circleGroup.selectAll('circle')
        .style('fill', "rgba(0,0,0,.4)")
        .style('stroke-width', 2)
        .attr("r", 3)
        .style('stroke', 'rgba(0,0,0,1)');
        d3.select(this).style('fill', "white")
        .attr("r", 6)
        .style('stroke-width', 5)
        .style('stroke', 'rgba(0,0,0,1)');
    })
    .attr('cx', function (d) { return milesScale(d.miles); })
    .attr('cy', function (d) { return priceScale(d.price); })
    .attr('r', 3)
    .style('fill', "rgba(0,0,0,.4)")
    .style('stroke-width', 2)
    .style('stroke', 'rgba(0,0,0,1)');


    // create circle group element
    var dCarGroup = svg.append("g");
    // set up circle elements for each vehicle
    // and bind data to the elements
    var dCar = dCarGroup.selectAll('circle')
    .data(args.baseVehicle)
    .enter();

    // draw circles.
    var dCarCircle = dCar
        .append('circle')
        .attr('class', 'circle')
        .attr('cx', function(d) { return milesScale(d.miles); })
        .attr('cy', function(d) { return priceScale(d.price); })
        .attr('r', 10)
        .style('fill', "rgba(255,255,255,1)")
        .style('stroke-width', 2)
        .style('stroke', 'rgba(0,0,255,1)');
    //    .call(drag);
}