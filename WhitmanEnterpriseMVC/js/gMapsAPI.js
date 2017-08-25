// JavaScript Document<script type="text/javascript" src="https://www.google.com/jsapi"></script>

	  var chartWidth = 300;
	  var chartHeight = 143;
	  var totalCars = 3;
	  
	  var yourCar = new Array();
	  yourCar[0] = new Array();
	  yourCar[0][0] = 'car1';
	  yourCar[0][1] = 54000;
	  yourCar[0][2] = 15000;
	  
	 // document.write('Your car is '+yourCar[0][0]+' with '+yourCar[0][1]+' miles and is priced at $'+yourCar[0][2]+'<br />');
	  
	  var highestMile = 500000;
	  var highestPrice = 50000;
	  var lowestMile = 7000;
	  var lowestPrice = 9000;
	  var averageMile = (highestMile+lowestMile+yourCar[0][1])/totalCars;
	  var averagePrice = (highestPrice+lowestPrice+yourCar[0][2])/totalCars;
	  var mileRangeEnd = highestMile + (highestMile*.1);
	  var priceRangeEnd = highestPrice + (highestPrice*.1);
	  
	  
	 // document.write('Highest Price: $' + highestPrice +'<br />');
	 // document.write('Lowest Price: $' + lowestPrice +'<br />');
	 // document.write('Average Price: $' + averagePrice +'<br />');
	 // document.write('Highest Mileage: ' + highestMile +'<br />');
	 // document.write('Lowest Mileage: ' + lowestMile +'<br />');
	 // document.write('Average Mileage: ' + averageMile +'<br />');
	  
      google.load("visualization", "1", {packages:["corechart"]});
      google.setOnLoadCallback(drawChart);
      function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('number', 'Mileage');
        data.addColumn('number', 'Market');
        data.addColumn('number', yourCar[0][0]);
        data.addColumn('number', 'Target');
        data.addRows(totalCars+1);
		data.setValue(0, 0, averageMile);
        data.setValue(0, 3, averagePrice);
        data.setValue(1, 0, 242478);
        data.setValue(1, 1, 24058);
		data.setValue(2, 0, yourCar[0][1]);
        data.setValue(2, 2, yourCar[0][2]);
        data.setValue(3, 0, highestMile);
        data.setValue(3, 1, lowestPrice);

        var chart = new google.visualization.ScatterChart(document.getElementById('chart_div'));
        chart.draw(data, 
				{width: chartWidth, 
				height: chartHeight, 
				backgroundColor: 'none', 
				colors: ['fff', 'e60000', '#66FE00'], 
				titleTextStyle: {color: '#fff'}, 
				backgroundColor: {stroke: '#aaaaaa', fill: 'none'},
                title: 'Market Target (Price vs. Mileage)',
                hAxis: {title: 'Mileage',
					titleTextStyle: {fontName: 'Trebuchet MS', 
					color: 'white', 
					fontSize: '11'}, 
					gridlineColor: '#777', 
					baselineColor: 'white', 
					textStyle: {color: '#fff'}, 
					baseline: averageMile, 
					minValue: 0, 
					maxValue: mileRangeEnd},
                vAxis: {
					titleTextStyle: {fontName: 'Trebuchet MS', 
					color: 'white', 
					fontSize: '11'}, 
					gridlineColor: '#777',
					baselineColor: 'white', 
					textStyle: {color: '#fff'}, 
					baseline: averagePrice, 
					minValue: 0, 
					maxValue: priceRangeEnd},
                legend: 'none'   
                         });
      }
