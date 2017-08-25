$(document).ready(function() {

	$('.bxslider').bxSlider({
		pagerCustom : '#bx-pager'
	});

	$("#tab_vehicle_information").click(function() {
		$("html, body").animate({
			scrollTop : $('#vehicle_information').offset().top
		}, 1000);
	});

	$("#tab_descriptions").click(function() {
		$("html, body").animate({
			scrollTop : $('#descriptions_area').offset().top
		}, 1000);
	});

	$("#tab_pictures").click(function() {
		$("html, body").animate({
			scrollTop : $('#pictures_area').offset().top
		}, 1000);
	});

	$("#tab_packages_options").click(function() {
		$("html, body").animate({
			scrollTop : $('#standard_options_area').offset().top
		}, 1000);
	});

	$("#tab_warranty").click(function() {
		$("html, body").animate({
			scrollTop : $('#warranty_area').offset().top
		}, 1000);
	});

	$("#tab_terms_conditions").click(function() {
		$("html, body").animate({
			scrollTop : $('#term_conditions_area').offset().top
		}, 1000);
	});

	$(".backtotop").click(function() {
		$("html, body").animate({
			scrollTop : $('#previewListingHeader').offset().top
		}, 1000);
	});

});
