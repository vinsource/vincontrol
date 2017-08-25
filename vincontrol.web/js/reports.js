$(document).ready(function() {
	$(".reports_items_header").click(function(){
		if($(this).hasClass("report_expended")){
			$(this).removeClass("report_expended").addClass("report_minimize");
			$(this).parent().find(".reports_items_content").slideUp();
			$(this).find(".reports_items_header_icon").removeClass("reports_items_expended");
		}else{
			$(".report_expended").trigger("click");
			$(this).removeClass("report_minimize").addClass("report_expended");
			$(this).parent().find(".reports_items_content").slideDown();
			$(this).find(".reports_items_header_icon").addClass("reports_items_expended");
		}
		
	});
	
	$(".reports_inventory").trigger("click");

});
