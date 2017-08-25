$(document).ready(function() {
	$(".v3Kpi_tb_items").click(function() {
		$(".bubble_times").hide();
		$(this).find(".bubble_times").show();
	});

    var value = $('#v3Kpi_bars_segments').val();
    loadData(value);

    $('#v3Kpi_bars_segments option').each(function () {
        if ($(this).val() == '0|0|0|0|$0') {
            $(this).addClass('DDLDisable');
        }
    });
    
	$('#v3Kpi_bars_segments').live("click", function () {
	    var value = $(this).val();
	    loadData(value);
	});
});

function loadData(value) {
    var history = value.split('|')[0];
    var stock = value.split('|')[1];
    var supply = value.split('|')[2];
    var turn = value.split('|')[3];
    var profit = value.split('|')[4];
    $('#divHistory').html(history);
    $('#divStock').html(stock);
    $('#divSupply').html(supply);
    $('#divTurn').html(turn);
    $('#divProfit').html(profit);
}


$(document).mouseup(function(e) {
	var popup_title = $(".v3Kpi_times_bottom");

	if (popup_title.has(e.target).length === 0) {
		$(".bubble_times").hide();
	}
	
	//var OtherListDetail = $(".v3BrandOther_Segments_Detail");

	//if (OtherListDetail.has(e.target).length === 0) {
	//	$("#v3BrandDetailClose").trigger("click");
	//}
});