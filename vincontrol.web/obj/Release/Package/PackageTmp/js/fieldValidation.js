// JavaScript Document
	function addCommas(nStr){
	nStr += '';
	x = nStr.split('.');
	x1 = x[0];
	x2 = x.length>1?'.'+x[1]:'';
	var rgx = /(\d+)(\d{3})/;
	while (rgx.test(x1)) {
		x1 = x1.replace(rgx,'$1'+','+'$2');
	}
	return x1+x2;
	}
	
		var currentVal;
		var newVal;
		var stripVal;
		var valBuffer;
		var letterCheck = /^[a-zA-Z]*$/;
		var noLetterCheck = /[a-zA-Z]+/;
		$('input, textarea').click(function(){
		 	this.select();	
		});
		$('input[type="text"]').focus(function(){
			currentVal =  this.value;
			// defaultVal = this.value;
		});
		
		$('input[type="text"]').focus(function(){
			if (!letterCheck.test(currentVal)) {
				newVal = 0;
				stripVal = 0;
				stripVal =  currentVal.replace(/[^\d.]/g, "");
				//console.log('old value: '+stripVal);
				// console.log(valBuffer);
				$(this).keyup(function(){
					valBuffer = this.value;
					type = true;
					//console.log(valBuffer);
					if (!noLetterCheck.test(valBuffer)) {
						newVal = valBuffer.replace(/[^\d.]/g, "");
						//console.log('newValue: '+newVal);
					} else {
						newVal = valBuffer;
					}
				});
			} 
		});
		
		$('input[type="text"]').blur(function(){
			if (!letterCheck.test(stripVal)) {
				if (currentVal != this.value) {
					if ($(this).hasClass('dollars')) {
						//console.log('price')
						if (!noLetterCheck.test(newVal)) {
							//console.log("inserting new value 1:"+addCommas(newVal));
							this.value = '$'+addCommas(newVal);
						} else {
							//console.log("inserting new value 2:"+valBuffer);
							this.value = valBuffer;
						}
					} else {
						if (!noLetterCheck.test(newVal)) {
							//console.log("inserting new value 3:"+addCommas(newVal));
							this.value = addCommas(newVal);
						} else {
							//console.log("inserting new value 2:"+valBuffer);
							this.value = valBuffer;
						}
					} 
				}
			} 
		});