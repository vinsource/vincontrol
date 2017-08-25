 $('#c2 input[type="text"], #c3 input[type="text"]').blur(function(){
		 var val = addCommas(this.value);
		 
		 	var typingTimer = 0;
			var doneTypingInterval = 3000;
		
	
			this.value=val;
	
	
	
	});
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