$(function () {
    var d1 = [];
	var d2 = {  label: 'Target', 
				color: 'blue', 
				points: {radius: 5},
				data: [[mileMedian, priceMedian]]
	};
	var d3 = {label: yourCar[0][0], 
				color: 'green',
				points: {radius: 5}, 
				data: [[yourCarMiles,yourCarPrice]]
	};

	for (i = 0; i <= totalCars - 1; i++){
			//document.write(i+3+', 0, allCars['+i+'][1] = '+allCars[i][1]+'<br />');
			//document.write(i+3+', 3, allCars['+i+'][2] = '+allCars[i][2]+'<br />');
			d1.push([allCars[i][1], allCars[i][2]]);
			//document.write(totalMile+'<br />');
		}
    var options = {
			legend: {show: false},
			series: {},
			lines: {show: false},
			points: {show: true, radius: 1},
			xaxis: {label: 'Miles', min: startMileRange, max: mileRangeEnd},
			yaxis: {label: 'Price', min: startPriceRange, max: priceRangeEnd},
			grid: {hoverable: true, markings: [{xaxis: {from: mileMedian, to: mileMedian}, color: 'black'}, {yaxis: {from: priceMedian, to: priceMedian}, color: 'black'}]}
		}
		
	
    $.plot($("#placeholderL"), [d1, d2, d3 ], options);
	
		
	$("#placeholderL").bind("plothover", function (event, pos, item) { 
		//console.log(item);
		document.getElementById('car').innerHTML = allCars[item.dataIndex][0];
		document.getElementById('miles').innerHTML = allCars[item.dataIndex][1];
		document.getElementById('price').innerHTML = allCars[item.dataIndex][2];
	});
	
	document.getElementById('totalCars').innerHTML = totalCars;
});