$(document).ready( function() {
		
		$(".imageWrap img").each( function() {
			
			var newWidth = $(".imageWrap").width();
			var newHeight = $(".imageWrap").height();			
			var difference;
			
			//alert("imageWrap: "+newWidth+"x"+newHeight);
			
			if ( $(this).width() > newWidth ) {
				//console.log ()
				//alert("width too big");
				var oldWidth = $(this).width();
				$(this).width(newWidth);
				difference = $(this).width() / oldWidth;
				$(this).height($(this).height()*difference);
			} else if ( $(this).width() < newWidth ) {
				//alert("width too small");
				$(this).width(newWidth);
				difference = $(this).width() / newWidth;
				$(this).height($(this).height()*difference);
			}
			
			if ( $(this).height() < newHeight ) {
				//alert("height too small");
				var oldHeight = $(this).height();
				$(this).height(newHeight);
				difference = $(this).height() / oldHeight;
				$(this).width($(this).width()*difference);
			}
			
			var imageWidth = this.width;
			var imageHeight = this.height;
			//console.log(imageWidth);
			//console.log(imageHeight);
			if (imageWidth > newWidth) {
				var margin = (imageWidth-newWidth)/2;
				//console.log(this.style.marginLeft);
				this.style.marginLeft = '-'+margin+'px';
			} else if (imageHeight > newHeight) {
				var margin = (imageHeight-newHeight)/2;
				//console.log(this.style.marginLeft);
				this.style.marginLeft = '-'+margin+'px';	
			}
			
		});
		
	});