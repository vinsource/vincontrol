$(document).ready(function () {
    $.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    $.ajax({
        type: "POST",
        url: "/StockingGuide/LoadWishList",
        data: { },
        success: function (results) {
            $('#divHasContent').html($("#wishListTemplate").render(results));
            $("a.iframe").fancybox({ 'margin': 0, 'padding': 0, 'width': 1100, 'height': 600 });
            $('[id^=deleteBtn]').click(
                function() {
                    if (confirm('Do you really want to remove this item from wish list?')) {
                        //alert('remove confirmed' + $(this).attr('stocking-id'));
                        $(this).parent().remove();
                        $.post("/StockingGuide/RemoveFromWishList", { id: $(this).attr('stocking-id'), source: $(this).attr('stocking-source') }, function(data) {
                            
                        });
                    }
                });
            $.unblockUI();
        }
    });
});