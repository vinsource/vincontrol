
function error(string, vHeight) {
	$('p.error').html(string);
	//console.log($('p.error'));
	$('.error-wrap').animate({ height: vHeight }, 250);
	$('.error').animate({opacity: 1}, 600);
}
