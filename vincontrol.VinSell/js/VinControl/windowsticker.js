function windowSticker(listingId) {
    var nCount = parseInt($("#NumberOfWSTemplate").val());
    if ( nCount > 0) {
        var actionUrl = "/Inventory/WindowSticker?ListingId=" + listingId;
        var usingTemplate = false;
        var templateId = -1;
        var optionSelected = false;
        var minWidth = 315;

        if (110 + nCount * 180 > minWidth) {
            minWidth = 110 + nCount * 180;
        }

        $("<a href=" + actionUrl + "></a>").fancybox({
            height: 416,
            width: minWidth,
            padding: 0,
            overlayShow: true,
            showCloseButton: true,
            enableEscapeButton: true,
            type: 'iframe',
        }).click();
    }
    else {
        printWSWithoutTemplate(listingId);
    }
}

function printWSWithoutTemplate(listingId) {
    var actionUrl = "/PDF/PrintSticker?ListingId=" + listingId;

    $("<a href=" + actionUrl + "></a>").fancybox({
        width: 1000,
        height: 770,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe'
    }).click();
}

function printWSWithTemplate(listingId, templateId) {
    var actionUrl = "/Inventory/PrintStickerWithTemplate?listingId=" + listingId + "&templateId=" + templateId;

    $("<a href=" + actionUrl + "></a>").fancybox({
        width: 1000,
        height: 770,
        showCloseButton: true,
        enableEscapeButton: true,
        type: 'iframe'
    }).click();
}