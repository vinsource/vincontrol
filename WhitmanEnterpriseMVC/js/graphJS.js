var fRange = 100;
var us = 3500;
var sortedCars;
$('#rangeNav span').each(function() {
	var id = this.id;	
	$(this).mouseover(function() {
		// // console.log(this.id);
		if (!$(this).hasClass('rSelected')) {
			$(this).click(function(){
				fRange = this.id;
				// // // console.log(fRange);
				$('#rangeNav span').each(function(){
					if ($(this).id != id) {
						$(this).removeClass('rSelected');
					}
				
				});
				$(this).addClass('rSelected');
			});
		}
	});
	
});

function filterSet(array) {
								return function (element) {
									return array.indexOf(element) !=-1
								}
						};

function sortCars(array) 
{
	
var trimFilter = [];
var optionFilter = [];
/*var trimList = document.getElementById('trim');
var optionList = document.getElementById('option');

for (i=0;i<trimList.children.length;i++) {
	if (trimList.children[i].selected == true) {trimFilter.push(trimList.children[i].text);}	
}

for (i=0;i<optionList.children.length;i++) {
	if (optionList.children[i].selected == true) {optionFilter.push(optionList.children[i].text);}	
}*/

//// console.log(trimFilter);
//// console.log(optionFilter);

array = array.slice(0);
var certify = document.getElementById('certifiedChk').checked;
//var trim = document.getElementById('trim').selectedIndex;
//// console.log(trim);
//var option = document.getElementById('option').selectedIndex;

if (certify == false /*&& trim == 0 && option == 0*/) {
	return array;
} else if (certify == true /*|| trim != 0 || option != 0*/) {
	var newArray
		if (certify === true) {
			for (a=0;a<=array.length-1;a++) {
				if (array[a][4] === 0) {
					array.splice(a, 1);
					a--;
				}
			}
		}
		/*if (trim != 0 && trimFilter.length != 0) {
			//// console.log('trim filter enabled');
			for (a=0;a<=array.length-1;a++) {
				var res = array[a][6].filter(filterSet(trimFilter));
				if (res.length != 0) {} else {
					// // console.log(array[a]);
					array.splice(a,1); a--;}
			}
		}*/
				
		/*if (option != 0 && optionFilter.length != 0) {
			// // console.log('option filter enabled');
			for (a=0;a<=array.length-1;a++) {
				 // // console.log(array[a][5]);							 
						var res = array[a][5].filter(filterSet(optionFilter));
						 // // console.log(res);
						if (res.length != 0) {} else {
							//// console.log(array[a]);
							array.splice(a,1); a--;}
				
			}
		}*/
	return array;
}
}

var d11;
var d12;
var d13;
var d14;
var d15;
var d16;
var d17;
var d18;
var d19;
var d20;

var ranges = new Array();
function randomFromTo(from, to){
       return Math.round(Math.random() * (to - from + 1) + from);
    }
function between(x, min, max) {
  return x >= min && x <= max;
}

// ...

var options;
function posNeg(x) {
	if (x > 0) {
		return '+'+x;	
	} else {
		return x;
	}
};

var certified;
function addCommas(nStr) {
	nStr+='';
	x=nStr.split('.');
	x1=x[0];
	x2=x.length>1?'.'+x[1]:'';
	var rgx=/(\d+)(\d{3})/;
	while(rgx.test(x1)){
		x1=x1.replace(rgx,'$1'+','+'$2');
	}
  return x1+x2;
}

var redraw = false;
var carRange = [];
var carDistances = new Array();
var carPrices = new Array();
var carMiles = new Array();
var allCars = new Array();
var percent = new Array();
var i=0;


while (i<=1) {
	i = Math.round(i*100)/100;
	percent.push(i);
	//// // // console.log(i);
	i = i+.1;
	i = Math.round(i*100)/100;
  }
  // console.log(percent);
  
  
if (allCars.length == 0) {
	  for (i=0;i<500;i++) {		  
	  	allCars[i] = new Array();
		  allCars[i][0] = []; // index, year, make, model and vin
		  allCars[i][0][0] = i // index
		  allCars[i][0][1] = 'year' //year
		  allCars[i][0][2] = 'make' //make
		  allCars[i][0][3] = 'model' //model
		  allCars[i][0][4] = randomFromTo(10000000000000000,99999999999999999) // vin
		  allCars[i][1] = randomFromTo(10,50000); //miles
	  	  allCars[i][2] = randomFromTo(5000,30000); //dollars
		  allCars[i][3] = randomFromTo(20,3400); //distance from
		  allCars[i][4] = randomFromTo(0,0);// certified
		  allCars[i][5] = [];
		  for (v=0;v<randomFromTo(0,2);v++) {
		  	allCars[i][5].push('Option '+(v+1)); // options
		  }
		  allCars[i][6] = []
		  allCars[i][6][0] = 'Trim '+randomFromTo(1,3); // trim
		  allCars[i][7] = randomFromTo(0,150); // days on AutoTrader, Cars.com and Ebay
		  allCars[i][8] = 'imageCar'+i+'.jpg';// image
		  allCars[i][9] = [];
		  for (v=0;v<randomFromTo(0,10);v++) {
		  	allCars[i][9][v] = [];
			allCars[i][9][v][0] = '10/10/10';
			allCars[i][9][v][1] = randomFromTo(3000,45000) // options
		  }// price changes
		  allCars[i][10] = 'color';// color
		  
	  }
} else {
	allCars = newArray();
	for (i=0;i<randomFromTo(10,100);i++) {		  
	  	allCars[i] = new Array();
		  allCars[i][0] = []; // index, year, make, model and vin
		  allCars[i][0][0] = i // index
		  allCars[i][0][1] = 'year' //year
		  allCars[i][0][2] = 'make' //make
		  allCars[i][0][3] = 'model' //model
		  allCars[i][0][4] = randomFromTo(10000000000000000,99999999999999999) // vin
		  allCars[i][1] = randomFromTo(10,50000); //miles
	  	  allCars[i][2] = randomFromTo(5000,30000); //dollars
		  allCars[i][3] = randomFromTo(20,3400); //distance from
		  allCars[i][4] = randomFromTo(0,0);// certified
		  allCars[i][5] = [];
		  for (v=0;v<randomFromTo(0,2);v++) {
		  	allCars[i][5].push('Option '+(v+1)); // options
		  }
		  allCars[i][6] = []
		  allCars[i][6][0] = 'Trim '+randomFromTo(1,3); // trim
		  allCars[i][7] = randomFromTo(0,150); // days on AutoTrader, Cars.com and Ebay
		  allCars[i][8] = 'imageCar'+i+'.jpg';// image
		  allCars[i][9] = [];
		  for (v=0;v<randomFromTo(0,10);v++) {
		  	allCars[i][9][v] = [];
			allCars[i][9][v][0] = '10/10/10';
			allCars[i][9][v][1] = randomFromTo(3000,450000); // options
		  }// price changes
		  allCars[i][10] = 'color';// color
		  
	  }
}


	 
	function abbrNum(number, decPlaces) {
		// 2 decimal places => 100, 3 => 1000, etc
		decPlaces = Math.pow(10,decPlaces);
		// Enumerate number abbreviations
		var abbrev = [ "k", "m", "b", "t" ];
		// Go through the array backwards, so we do the largest first
		for (var i=abbrev.length-1; i>=0; i--) {
			// Convert array index to "1000", "1000000", etc
			var size = Math.pow(10,(i+1)*3);
			// If the number is bigger or equal do the abbreviation
			if(size <= number) {
				 // Here, we multiply by decPlaces, round, and then divide by decPlaces.
				 // This gives us nice rounding to a particular decimal place.
				 number = Math.round(number*decPlaces/size)/decPlaces;
				 // Add the letter for the abbreviation
				 number += abbrev[i];
				 // We are done... stop
				 break;
			}
		}
		return number;
	}
	 
	  
	  var totalMile = 0;
	  var totalPrice = 0;
	  if (totalMile == 0 || totalPrice == 0) {
		  
		  for(var i = 0; i < allCars.length - 1; ++i){
				//document.write(totalMile+" + "+allCars[i][1]+" =");
				totalMile = totalMile + allCars[i][1];
				totalPrice = totalPrice + allCars[i][2];
				
				carDistances[i] = allCars[i][3];
				
				carMiles[i] = allCars[i][1];
				carPrices[i] = allCars[i][2];
				
				//document.write(totalMile+'<br />');
			  }
			  
		  //document.write(totalMile);
	  } else {
		 totalMile = 0;
	  	 totalPrice = 0;
		 
		 for (var i = 0; i < allCars.length - 1; ++i){
				//document.write(totalMile+" + "+allCars[i][1]+" =");
				totalMile = totalMile + allCars[i][1];
				totalPrice = totalPrice + allCars[i][2];
				
				 
				
				carMiles[i] = allCars[i][1];
				carPrices[i] = allCars[i][2];
				
				//document.write(totalMile+'<br />');
			  }
	  }
	  //// // // // console.log(carDistances);
	  var totalCars = allCars.length;
	  
	  var largestPrice = Math.max.apply(Math, carPrices);
	  var largestMiles = Math.max.apply(Math, carMiles);
	  var smallestPrice = Math.min.apply(Math, carPrices);
	  var smallestMiles = Math.min.apply(Math, carMiles);
	  var nearestCar = Math.min.apply(Math, carDistances);
	  var farthestCar = Math.max.apply(Math, carDistances); 

	  var yourCar = new Array();
	  yourCar[0] = new Array();
		  yourCar[0][0] = []; // index, year, make, model and vin
		  yourCar[0][0][0] = i // index
		  yourCar[0][0][1] = 'Year' //year
		  yourCar[0][0][2] = 'Your' //make
		  yourCar[0][0][3] = 'Car' //model
		  yourCar[0][0][4] = randomFromTo(10000000000000000,99999999999999999) // vin
		  yourCar[0][1] = randomFromTo(10,50000); //miles
	  	  yourCar[0][2] = randomFromTo(5000,30000); //dollars
		  yourCar[0][3] = randomFromTo(20,3400); //distance from
		  yourCar[0][4] = randomFromTo(0,0);// certified
		  yourCar[0][5] = [];
		  for (v=0;v<randomFromTo(0,2);v++) {
		  	yourCar[0][5].push('option '+(v+1)); // options
		  }
		  yourCar[0][6] = 'Trim '+randomFromTo(1,3); // trim
		  yourCar[0][7] = randomFromTo(0,150); // days on AutoTrader, Cars.com and Ebay
		  yourCar[0][8] = 'yourCar.jpg';// image
		  yourCar[0][9] = [];
		  for (v=0;v<randomFromTo(0,10);v++) {
		  	yourCar[0][9][v] = [];
			yourCar[0][9][v][0] = '10/10/10';
			yourCar[0][9][v][1] = randomFromTo(3000,45000) // options
		  }// price changes
		  yourCar[0][10] = 'color';// color
	  
	  
	  	
	  
	  //var bufferMiles = largestMiles/2;
	  //var bufferPrice = largestPrice/2;
	 // document.write('Your car is '+yourCar[0][0]+' with '+yourCar[0][1]+' miles and is priced at $'+yourCar[0][2]+'<br />');
	  var yourCarMiles = yourCar[0][1];
	  var yourCarPrice = yourCar[0][2];
	  
	  var startPriceRange = smallestPrice-(largestPrice*.25);
	  var startMileRange = smallestMiles;
	  
	  var mileRangeEnd = largestMiles;
	  var priceRangeEnd = largestPrice+(largestPrice*.25);
	
	  var mileRange = mileRangeEnd;
	  var priceRange = priceRangeEnd;
	  
	  var averageMile = totalMile/totalCars;
	  var averagePrice = totalPrice/totalCars;
	  // // // console.log(allCars);
	  // // // console.log(allCars);
	
	  
	  for (i=0;i<=9;i++) {
		  ranges[i] = new Array();
		  ranges[i][0] = Math.round((percent[i]*100)*100)/100;
		  ranges[i][1] = Math.round((percent[i+1]*100)*100)/100;
	  }
	  var addEnd = ranges.length;
	  //// console.log(addEnd);
	  ranges[addEnd] = [];
	  ranges[addEnd][0] = 100;
	  ranges[addEnd][1] = 3500;
	  
	  $('#any').attr('id', us);
	  //// // // // console.log(ranges[7][2]);
//	  ranges[]
	  //// // // // console.log($('#'+farthestCar));
	   var rDistances = [];
	   
	   for (i=0;i<allCars.length;i++) {
			
			if (allCars[i][3] <= fRange) {
			carRange[i] = [];
			carRange[i][0] = allCars[i][0];
			carRange[i][1] = allCars[i][1];
			carRange[i][2] = allCars[i][2];
			carRange[i][3] = allCars[i][3];
		
			//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
			}
	   }
	   
	   carRange = $.grep(carRange,function(n){return(n);});
	   
	   for (v=0;v<=ranges.length-1;v++) {
		 	//// // // // console.log('searching for cars between: '+ranges[v][0]+' - '+(ranges[v][1]-1)); 
	  		ranges[v][2] = new Array();
			var b = 0;
			for (i=0; i<carRange.length;i++) {
				// // // // // console.log(carRange[i][3]);
				if (between(carRange[i][3],ranges[v][0],ranges[v][1])) {
						//// // // // console.log('car id: '+allCars[i][0]);
						//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
						ranges[v][2][b] = [carRange[i][1], carRange[i][2]];
					b++;
				} else if (between(carRange[i],100,3500)) {
					ranges[10][2][b] = [carRange[i][1], carRange[i][2]];
				}
	  		}
			//// // // // console.log(ranges[v][2]);
	  }
		//// // // // console.log(carRange);
	  	// // // // console.log(ranges);
	    
	
	 
$(function () {
	d11 = []; for (i=0;i<ranges[0][2].length;i++) {d11.push(ranges[0][2][i])}
	d12 = []; for (i=0;i<ranges[1][2].length;i++) {d12.push(ranges[1][2][i])}
	d13 = []; for (i=0;i<ranges[2][2].length;i++) {d13.push(ranges[2][2][i])}
	d14 = []; for (i=0;i<ranges[3][2].length;i++) {d14.push(ranges[3][2][i])}
	d15 = []; for (i=0;i<ranges[4][2].length;i++) {d15.push(ranges[4][2][i])}
	d16 = []; for (i=0;i<ranges[5][2].length;i++) {d16.push(ranges[5][2][i])}
	d17 = []; for (i=0;i<ranges[6][2].length;i++) {d17.push(ranges[6][2][i])}
	d18 = []; for (i=0;i<ranges[7][2].length;i++) {d18.push(ranges[7][2][i])}
	d19 = []; for (i=0;i<ranges[8][2].length;i++) {d19.push(ranges[8][2][i])}
	d20 = []; for (i=0;i<ranges[9][2].length;i++) {d20.push(ranges[9][2][i])}
	d21 = []; for (i=0;i<ranges[10][2].length;i++) {d21.push(ranges[10][2][i])}
	
	$('#rangeNav span').click(function(){
		fRange = this.id;
	});
	sortedCars = allCars.slice();
	$('#sorting input, #trim, #option, .ui-dropdownchecklist ').focus(function(){
		
		sortedCars = 0;
		  sortedCars = sortCars(allCars);
		  // console.log(sortedCars);
		  
	  	  ranges = new Array();
			  for (i=0;i<=9;i++) {
		  ranges[i] = new Array();
		  ranges[i][0] = Math.round((percent[i]*100)*100)/100;
		  ranges[i][1] = Math.round((percent[i+1]*100)*100)/100;
	  }
	  var addEnd = ranges.length;
	  //// console.log(addEnd);
	  ranges[addEnd] = [];
	  ranges[addEnd][0] = 100;
	  ranges[addEnd][1] = 3500;
		  
		  //// // // // console.log(fRange);
		  carRange = [];
		  if (fRange == us) {
			  for (i=0;i<=sortedCars.length-1;i++) {
				carRange[i] = [];
				carRange[i][0] = sortedCars[i][0];
				carRange[i][1] = sortedCars[i][1];
				carRange[i][2] = sortedCars[i][2];
				carRange[i][3] = sortedCars[i][3];
				//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
				}
			  } else {
			  for (i=0;i<sortedCars.length;i++) {
				if (sortedCars[i][3] <= fRange) {
				carRange[i] = [];
				carRange[i][0] = sortedCars[i][0];
				carRange[i][1] = sortedCars[i][1];
				carRange[i][2] = sortedCars[i][2];
				carRange[i][3] = sortedCars[i][3];
				//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
				}
		  
			  }
		  }
		  //// // // // console.log(redraw);
		  carRange = $.grep(carRange,function(n){return(n);});
		  
		  
		  
		  //// // // // console.log(carRange);
		  for (v=0;v<=ranges.length-1;v++) {
		 	//// // // // console.log('searching for cars between: '+ranges[v][0]+' - '+(ranges[v][1]-1)); 
	  		ranges[v][2] = new Array();
			var b = 0;
			for (i=0; i<=carRange.length-1;i++) {
				//// // // // console.log(carRange[i][3]);
				if (between(carRange[i][3],ranges[v][0],ranges[v][1])) {
						//// // // // console.log('car id: '+allCars[i][0]);
						//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
						ranges[v][2][b] = [carRange[i][1], carRange[i][2]];
					b++;
				}
	  		}
			// // // // console.log(ranges[v][2]);
	  }	
		 // // // console.log(ranges);
			d11 = []; for (i=0;i<ranges[0][2].length;i++) {d11.push(ranges[0][2][i])}
			d12 = []; for (i=0;i<ranges[1][2].length;i++) {d12.push(ranges[1][2][i])}
			d13 = []; for (i=0;i<ranges[2][2].length;i++) {d13.push(ranges[2][2][i])}
			d14 = []; for (i=0;i<ranges[3][2].length;i++) {d14.push(ranges[3][2][i])}
			d15 = []; for (i=0;i<ranges[4][2].length;i++) {d15.push(ranges[4][2][i])}
			d16 = []; for (i=0;i<ranges[5][2].length;i++) {d16.push(ranges[5][2][i])}
			d17 = []; for (i=0;i<ranges[6][2].length;i++) {d17.push(ranges[6][2][i])}
			d18 = []; for (i=0;i<ranges[7][2].length;i++) {d18.push(ranges[7][2][i])}
			d19 = []; for (i=0;i<ranges[8][2].length;i++) {d19.push(ranges[8][2][i])}
			d20 = []; for (i=0;i<ranges[9][2].length;i++) {d20.push(ranges[9][2][i])}
			d21 = []; for (i=0;i<ranges[10][2].length;i++) {d21.push(ranges[10][2][i])}
			
		
			$.plot($("#placeholder"), [ 
			d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},
			{data: d12, points: {radius: 9}},
			{data: d13, points: {radius: 8}},
			{data: d14, points: {radius: 7}},
			{data: d15, points: {radius: 6}},
			{data: d16, points: {radius: 5}},
			{data: d17, points: {radius: 4}},
			{data: d18, points: {radius: 3}},
			{data: d19, points: {radius: 2}},
			{data: d20, points:{radius: 2}},
			{data: d21, points:{radius: 2}}], options);
	});
	$('#sorting input, #rangeNav span').click(function(){
		
		  sortedCars = 0;
		  sortedCars = sortCars(allCars);
		 // // console.log(sortedCars);
		  
	  	  ranges = new Array();
			  for (i=0;i<=9;i++) {
		  ranges[i] = new Array();
		  ranges[i][0] = Math.round((percent[i]*100)*100)/100;
		  ranges[i][1] = Math.round((percent[i+1]*100)*100)/100;
	  }
	  var addEnd = ranges.length;
	  //// console.log(addEnd);
	  ranges[addEnd] = [];
	  ranges[addEnd][0] = 100;
	  ranges[addEnd][1] = 3500;
		  
		  //// // // // console.log(fRange);
		  carRange = [];
		  if (fRange == us) {
			  for (i=0;i<=sortedCars.length-1;i++) {
				carRange[i] = [];
				carRange[i][0] = sortedCars[i][0];
				carRange[i][1] = sortedCars[i][1];
				carRange[i][2] = sortedCars[i][2];
				carRange[i][3] = sortedCars[i][3];
				//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
				}
			  } else {
			  for (i=0;i<sortedCars.length;i++) {
				if (sortedCars[i][3] <= fRange) {
				carRange[i] = [];
				carRange[i][0] = sortedCars[i][0];
				carRange[i][1] = sortedCars[i][1];
				carRange[i][2] = sortedCars[i][2];
				carRange[i][3] = sortedCars[i][3];
				//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
				}
		  
			  }
		  }
		  //// // // // console.log(redraw);
		  carRange = $.grep(carRange,function(n){return(n);});
		  
		  
		  
		  //// // // // console.log(carRange);
		  for (v=0;v<=ranges.length-1;v++) {
		 	//// // // // console.log('searching for cars between: '+ranges[v][0]+' - '+(ranges[v][1]-1)); 
	  		ranges[v][2] = new Array();
			var b = 0;
			for (i=0; i<=carRange.length-1;i++) {
				//// // // // console.log(carRange[i][3]);
				if (between(carRange[i][3],ranges[v][0],ranges[v][1])) {
						//// // // // console.log('car id: '+allCars[i][0]);
						//// // // // console.log('car '+carRange[i][0]+' has been added to this range with '+carRange[i][3]+' miles');
						ranges[v][2][b] = [carRange[i][1], carRange[i][2]];
					b++;
				}
	  		}
			// // // // console.log(ranges[v][2]);
	  }	
		 // // // console.log(ranges);
			d11 = []; for (i=0;i<ranges[0][2].length;i++) {d11.push(ranges[0][2][i])}
			d12 = []; for (i=0;i<ranges[1][2].length;i++) {d12.push(ranges[1][2][i])}
			d13 = []; for (i=0;i<ranges[2][2].length;i++) {d13.push(ranges[2][2][i])}
			d14 = []; for (i=0;i<ranges[3][2].length;i++) {d14.push(ranges[3][2][i])}
			d15 = []; for (i=0;i<ranges[4][2].length;i++) {d15.push(ranges[4][2][i])}
			d16 = []; for (i=0;i<ranges[5][2].length;i++) {d16.push(ranges[5][2][i])}
			d17 = []; for (i=0;i<ranges[6][2].length;i++) {d17.push(ranges[6][2][i])}
			d18 = []; for (i=0;i<ranges[7][2].length;i++) {d18.push(ranges[7][2][i])}
			d19 = []; for (i=0;i<ranges[8][2].length;i++) {d19.push(ranges[8][2][i])}
			d20 = []; for (i=0;i<ranges[9][2].length;i++) {d20.push(ranges[9][2][i])}
			d21 = []; for (i=0;i<ranges[10][2].length;i++) {d21.push(ranges[10][2][i])}
			
		
			$.plot($("#placeholder"), [ 
			d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},
			{data: d12, points: {radius: 9}},
			{data: d13, points: {radius: 8}},
			{data: d14, points: {radius: 7}},
			{data: d15, points: {radius: 6}},
			{data: d16, points: {radius: 5}},
			{data: d17, points: {radius: 4}},
			{data: d18, points: {radius: 3}},
			{data: d19, points: {radius: 2}},
			{data: d20, points:{radius: 2}},
			{data: d21, points:{radius: 2}}], options);	
 
	});
			
	var d3 = {label: yourCar[0][0], 
				color: 'blue',
				points: {radius: 6, symbol: 'diamond', fillColor: 'white'},
				data: [[yourCarMiles,yourCarPrice]]
	};
		
	var d4 = {  label: 'range',
				points: {show:false},
				lines: {show:true, fill:false,},
				color: 'green',
				grid: {hoverable: false, clickable: false},
				data: [
					[startMileRange,averagePrice+(averagePrice*.20)],
					[mileRangeEnd,averagePrice-(averagePrice*.20)]
				]
	}
	var d5 = {  label: 'range',
				points: {show:false},
				lines: {show:true, fill: true},
				color: 'lime',
				grid: {hoverable: false, clickable: false},
				data: [
					[startMileRange,averagePrice+(averagePrice*.225)],
					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.025)],
					[mileRangeEnd,averagePrice-(averagePrice*.225)],
					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.025)],
					[startMileRange,averagePrice+(averagePrice*.225)]
				]
	}
	var d6 = {  label: 'range',
				points: {show:false},
				lines: {show:true, fill: true},
				color: 'yellow',
				grid: {hoverable: false, clickable: false},
				data: [
					[startMileRange,averagePrice+(averagePrice*.275)],
					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.075)],
					[mileRangeEnd,averagePrice-(averagePrice*.275)],
					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.075)],
					[startMileRange,averagePrice+(averagePrice*.275)]
				]
	}
	var d7 = {  label: 'range',
				points: {show:false},
				lines: {show:true, fill: true},
				color: 'orange',
				grid: {hoverable: false, clickable: false},
				data: [
					[startMileRange,averagePrice+(averagePrice*.425)],
					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.225)],
					[mileRangeEnd,averagePrice-(averagePrice*.425)],
					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.225)],
					[startMileRange,averagePrice+(averagePrice*.425)]
				]
	}
	var d8 = {  label: 'range',
				points: {show:false},
				lines: {show:true, fill: true},
				color: 'red',
				grid: {hoverable: false, clickable: false},
				data: [
					[startMileRange,averagePrice+(averagePrice*.7)],
					[mileRangeEnd,averagePrice-(averagePrice*.20)+(averagePrice*.5)],
					[mileRangeEnd,averagePrice-(averagePrice*.7)],
					[startMileRange,averagePrice+(averagePrice*.20)-(averagePrice*.5)],
					[startMileRange,averagePrice+(averagePrice*.7)]
				]
	}
	
	

	//for (i = 0; i <= totalCars - 1; i++){
	//			d1.push([allCars[i][1], allCars[i][2]]);
	//	}
		
	function mileFormat(v, axis) {
        var miles = v.toFixed(axis.tickDecimals);
		return abbrNum(miles,0)+" mi";
    }
	
	function priceFormat(v, axis) {
        var price = v.toFixed(axis.tickDecimals);
		return '$'+abbrNum(price,0);
    }
		
    options = {
			legend: {show: false},
			series: {color: '#000'},
			lines: {show: false},
			points: {show: true, radius: 4, fill: true, fillColor: false},
			xaxis: {show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat},
			yaxis: {show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat},
			grid: {hoverable: false, clickable: false}
		}
		
	$.plot($("#placeholder"), [ 
	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},
			{data: d12, points: {radius: 9}},
			{data: d13, points: {radius: 8}},
			{data: d14, points: {radius: 7}},
			{data: d15, points: {radius: 6}},
			{data: d16, points: {radius: 5}},
			{data: d17, points: {radius: 4}},
			{data: d18, points: {radius: 3}},
			{data: d19, points: {radius: 2}},
			{data: d20, points:{radius: 2}},
			{data: d21, points:{radius: 2}}], options);
			//$.plot($("#placeholderL"), [d1, d2, d3 ], options);
	
	$('input[name="toggleGraph"]').click(function(){
		// // // // console.log(ranges)
		var width = document.getElementById('placeholder').clientWidth;
		var height = document.getElementById('placeholder').clientHeight;
		//alert('width '+this.clientWidth+' height '+this.clientHeight);
			//// // // // console.log(item);		
		if (width <= 300 && height <= 143) {
			document.getElementById('placeholder').style.width=553+'px';
			document.getElementById('placeholder').style.height=325+'px';
			//this.style.backgroundImage="url(images/graphBG.png)";
			document.getElementById('placeholder').title = '';
			document.getElementById('graphButton').value = 'Close';
			options = {
			legend: {show: false},
			series: {color: '#000'},
			lines: {show: false},
			points: {show: true, radius: 4, fill: true, fillColor: false},
			xaxis: {show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat},
			yaxis: {show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat},
			grid: {hoverable: false, clickable: true }
		}
			//this.style.backgounrd = "url('images/graphBG.png') top right no-repeat";
			$.plot($("#placeholder"), [ 
	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},
			{data: d12, points: {radius: 9}},
			{data: d13, points: {radius: 8}},
			{data: d14, points: {radius: 7}},
			{data: d15, points: {radius: 6}},
			{data: d16, points: {radius: 5}},
			{data: d17, points: {radius: 4}},
			{data: d18, points: {radius: 3}},
			{data: d19, points: {radius: 2}},
			{data: d20, points:{radius: 2}},
			{data: d21, points:{radius: 2}}], options);
	//$.plot($("#placeholderL"), [d1, d2, d3 ], options);

	$("#placeholder").bind("plotclick", function (event, pos, item) {
				
					// // // // console.log(item);
					if (item.datapoint[0] == yourCar[0][1] && item.datapoint[1] == yourCar[0][2]) {
						//document.getElementById('graphCarImage').src = allCars[i][8];
								$('#graphCarImage').attr('src', yourCar[0][8]);
								document.getElementById('car').innerHTML = 
										yourCar[0][0][1]+'<br />'+
										yourCar[0][0][2]+'<br />'+
										yourCar[0][0][3]+' '+
										yourCar[0][6]+'<br /><div class="clear"></div>'+
										yourCar[0][10]+'<br /> VIN: '+
										yourCar[0][0][4]+'<br />';
								document.getElementById('daysOnMarket').innerHTML = addCommas(yourCar[0][7])+' days';
								var changeList = ''
								for (b=0;b<yourCar[0][9].length;b++) {
										changeList += '<li>'+yourCar[0][9][b][0]+' - $'+addCommas(yourCar[0][9][b][1])+'</li>';
								}
								//// console.log(changeList);
								document.getElementById('priceChanges').innerHTML = changeList;
								document.getElementById('miles').innerHTML = addCommas(yourCar[0][1]);
								document.getElementById('price').innerHTML = addCommas(yourCar[0][2]);
								document.getElementById('diffM').innerHTML = addCommas(posNeg(yourCar[0][1]-yourCar[0][1]));
								document.getElementById('diffP').innerHTML = addCommas(posNeg(yourCar[0][2]-yourCar[0][2]));
								document.getElementById('distance').innerHTML = addCommas(yourCar[0][3])+' Miles';
								if (yourCar[0][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} 
								else {document.getElementById('certified').innerHTML = 'No';}			
					} else if (item.datapoint[0] == averageMile && item.datapoint[1] == averagePrice) {
						document.getElementById('car').innerHTML = '';
						document.getElementById('miles').innerHTML = 'Midrange Mileage: '+mileMedian;
						document.getElementById('price').innerHTML = priceMedian+' is your Midrange Pricing';			
					} else if (item.series.label == 'range') {
						document.getElementById('car').innerHTML = '';
						document.getElementById('miles').innerHTML = '';
						document.getElementById('price').innerHTML = '';
					} else {
						for (i=0;i<allCars.length;i++) {
							if (item.datapoint[0] == allCars[i][1] && item.datapoint[1] == allCars[i][2]) {
								//document.getElementById('graphCarImage').src = allCars[i][8];
								$('#graphCarImage').attr('src',allCars[i][8]);
								document.getElementById('car').innerHTML = 
										allCars[i][0][1]+'<br />'+
										allCars[i][0][2]+'<br />'+
										allCars[i][0][3]+' '+
										allCars[i][6]+'<br /><div class="clear"></div>'+
										allCars[i][10]+'<br /> VIN: '+
										allCars[i][0][4]+'<br />';
								document.getElementById('daysOnMarket').innerHTML = addCommas(allCars[i][7])+' days';
								var changeList = ''
								for (b=0;b<allCars[i][9].length;b++) {
										changeList += '<li>'+allCars[i][9][b][0]+' - $'+addCommas(allCars[i][9][b][1])+'</li>';
								}
								// console.log(changeList);
								document.getElementById('priceChanges').innerHTML = changeList;
								document.getElementById('miles').innerHTML = addCommas(allCars[i][1]);
								document.getElementById('price').innerHTML = addCommas(allCars[i][2]);
								document.getElementById('diffM').innerHTML = addCommas(posNeg(allCars[i][1]-yourCar[0][1]))
								document.getElementById('diffP').innerHTML = addCommas(posNeg(allCars[i][2]-yourCar[0][2]));
								document.getElementById('distance').innerHTML = addCommas(allCars[i][3])+' Miles';
								if (allCars[i][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} 
								else {document.getElementById('certified').innerHTML = 'No';}
							} 
						}
						
					}
				// // console.log(item.datapoint[0]+', '+item.datapoint[1]);
				
				
				
			});
			
		} else {
			document.getElementById('placeholder').title = 'Click on the graph to change its size.';
			document.getElementById('placeholder').style.width=300+'px';
			document.getElementById('placeholder').style.height=143+'px';
			document.getElementById('graphButton').value = 'Expand';
			options = {
			legend: {show: false},
			series: {color: '#000'},
			lines: {show: false},
			points: {show: true, radius: 4, fill: true, fillColor: false},
			xaxis: {show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat},
			yaxis: {show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat},
			grid: {hoverable: false, clickable: false}
		}
			//this.style.backgroundImage = "url(images/graphBGwide.png)";
			$.plot($("#placeholder"), [ 
	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},
			{data: d12, points: {radius: 9}},
			{data: d13, points: {radius: 8}},
			{data: d14, points: {radius: 7}},
			{data: d15, points: {radius: 6}},
			{data: d16, points: {radius: 5}},
			{data: d17, points: {radius: 4}},
			{data: d18, points: {radius: 3}},
			{data: d19, points: {radius: 2}},
			{data: d20, points:{radius: 2}},
			{data: d21, points:{radius: 2}}], options);
			//$.plot($("#placeholderL"), [d1, d2, d3 ], options);
			//this.style.background = "url('images/graphBGwide.png') top right no-repeat";
			
		}
		
	});
	
	$('#placeholder').dblclick(function(){
		var width = document.getElementById('placeholder').clientWidth;
		var height = document.getElementById('placeholder').clientHeight;
		//alert('width '+this.clientWidth+' height '+this.clientHeight);
			//// // // // console.log(item);		
		if (width <= 300 && height <= 143) {
			document.getElementById('placeholder').style.width=553+'px';
			document.getElementById('placeholder').style.height=350+'px';
			//this.style.backgroundImage="url(images/graphBG.png)";
			document.getElementById('placeholder').title = '';
			document.getElementById('graphButton').value = 'Close';
			options = {
			legend: {show: false},
			series: {color: '#000'},
			lines: {show: false},
			points: {show: true, radius: 4, fill: true, fillColor: false},
			xaxis: {show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat},
			yaxis: {show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat},
			grid: {hoverable: false, clickable: true }
		}
			//this.style.backgounrd = "url('images/graphBG.png') top right no-repeat";
			$.plot($("#placeholder"), [ 
	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},
			{data: d12, points: {radius: 9}},
			{data: d13, points: {radius: 8}},
			{data: d14, points: {radius: 7}},
			{data: d15, points: {radius: 6}},
			{data: d16, points: {radius: 5}},
			{data: d17, points: {radius: 4}},
			{data: d18, points: {radius: 3}},
			{data: d19, points: {radius: 2}},
			{data: d20, points:{radius: 2}},
			{data: d21, points:{radius: 2}}], options);
	//$.plot($("#placeholderL"), [d1, d2, d3 ], options);
			
			
				
				$("#placeholder").bind("plotclick", function (event, pos, item) {
				
					// // // // console.log(item);
					if (item.datapoint[0] == yourCar[0][1] && item.datapoint[1] == yourCar[0][2]) {
						//document.getElementById('graphCarImage').src = allCars[i][8];
								$('#graphCarImage').attr('src',yourCar[0][8]);
								document.getElementById('car').innerHTML = 
										yourCar[0][0][1]+'<br />'+
										yourCar[0][0][2]+'<br />'+
										yourCar[0][0][3]+' '+
										yourCar[0][6]+'<br /><div class="clear"></div>'+
										yourCar[0][10]+'<br /> VIN: '+
										yourCar[0][0][4]+'<br />';
								document.getElementById('daysOnMarket').innerHTML = addCommas(yourCar[0][7])+' days';
								var changeList = ''
								for (b=0;b<yourCar[0][9].length;b++) {
										changeList += '<li>'+yourCar[0][9][b][0]+' - $'+addCommas(yourCar[0][9][b][1])+'</li>';
								}
								// console.log(changeList);
								document.getElementById('priceChanges').innerHTML = changeList;
								document.getElementById('miles').innerHTML = addCommas(yourCar[0][1]);
								document.getElementById('price').innerHTML = addCommas(yourCar[0][2]);
								document.getElementById('diffM').innerHTML = addCommas(posNeg(yourCar[0][1]-yourCar[0][1]));
								document.getElementById('diffP').innerHTML = addCommas(posNeg(yourCar[0][2]-yourCar[0][2]));
								document.getElementById('distance').innerHTML = addCommas(yourCar[0][3])+' Miles';
								if (yourCar[0][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} 
								else {document.getElementById('certified').innerHTML = 'No';}			
					} else if (item.datapoint[0] == averageMile && item.datapoint[1] == averagePrice) {
						document.getElementById('car').innerHTML = '';
						document.getElementById('miles').innerHTML = 'Midrange Mileage: '+mileMedian;
						document.getElementById('price').innerHTML = priceMedian+' is your Midrange Pricing';			
					} else if (item.series.label == 'range') {
						document.getElementById('car').innerHTML = '';
						document.getElementById('miles').innerHTML = '';
						document.getElementById('price').innerHTML = '';
					} else {
						for (i=0;i<allCars.length;i++) {
							if (item.datapoint[0] == allCars[i][1] && item.datapoint[1] == allCars[i][2]) {
								//document.getElementById('graphCarImage').src = allCars[i][8];
								$('#graphCarImage').attr('src',allCars[i][8]);
								document.getElementById('car').innerHTML = 
										allCars[i][0][1]+'<br />'+
										allCars[i][0][2]+'<br />'+
										allCars[i][0][3]+' '+
										allCars[i][6]+'<br /><div class="clear"></div>'+
										allCars[i][10]+'<br /> VIN: '+
										allCars[i][0][4]+'<br />';
								document.getElementById('daysOnMarket').innerHTML = addCommas(allCars[i][7])+' days';
								var changeList = ''
								for (b=0;b<allCars[i][9].length;b++) {
										changeList += '<li>'+allCars[i][9][b][0]+' - $'+addCommas(allCars[i][9][b][1])+'</li>';
								}
								// console.log(changeList);
								document.getElementById('priceChanges').innerHTML = changeList;
								document.getElementById('miles').innerHTML = addCommas(allCars[i][1]);
								document.getElementById('price').innerHTML = addCommas(allCars[i][2]);
								document.getElementById('diffM').innerHTML = addCommas(posNeg(allCars[i][1]-yourCar[0][1]))
								document.getElementById('diffP').innerHTML = addCommas(posNeg(allCars[i][2]-yourCar[0][2]));
								document.getElementById('distance').innerHTML = addCommas(allCars[i][3])+' Miles';
								if (allCars[i][4] == 1) {document.getElementById('certified').innerHTML = 'Yes';} 
								else {document.getElementById('certified').innerHTML = 'No';}
							} 
						}
						
					}
				
			});
			
		} else {
			document.getElementById('placeholder').title = 'Double-click on the graph to change its size.';
			document.getElementById('placeholder').style.width=300+'px';
			document.getElementById('placeholder').style.height=143+'px';
			document.getElementById('graphButton').value = 'Expand';
			options = {
			legend: {show: false},
			series: {color: '#000'},
			lines: {show: false},
			points: {show: true, radius: 4, fill: true, fillColor: false},
			xaxis: {show: true, label: 'Miles', min: startMileRange, max: mileRangeEnd, tickFormatter: mileFormat},
			yaxis: {show: true, label: 'Price', min: startPriceRange, max: priceRangeEnd, tickFormatter: priceFormat},
			grid: {hoverable: false, clickable: false}
		}
			//this.style.backgroundImage = "url(images/graphBGwide.png)";
			$.plot($("#placeholder"), [ 
	d8, d7, d6, d5, d4, d3, {data: d11, points: {radius: 10}},
			{data: d12, points: {radius: 9}},
			{data: d13, points: {radius: 8}},
			{data: d14, points: {radius: 7}},
			{data: d15, points: {radius: 6}},
			{data: d16, points: {radius: 5}},
			{data: d17, points: {radius: 4}},
			{data: d18, points: {radius: 3}},
			{data: d19, points: {radius: 2}},
			{data: d20, points:{radius: 2}},
			{data: d21, points:{radius: 2}}], options);
			//$.plot($("#placeholderL"), [d1, d2, d3 ], options);
			//this.style.background = "url('images/graphBGwide.png') top right no-repeat";	
		}
		
	});
	
	document.getElementById('totalCars').innerHTML = totalCars;
});

// // // // console.log(fRange);