function block(element, loadingImage) {
    $(element).block({ message: '<div><div style="display:inline-block;width:100%;text-align:center;"><img src="' + loadingImage + '" style="display:inline-block;width:100%;text-align:center;"/></div><div style="display:inline-block;width:100%;text-align:center;color:#fff;">Loading...</div></div>', css: { width: '40%', backgroundColor: 'none', border: 'none'} });
}

function unblock(element) {
    $(element).unblock();    
}

function blockUI(loadingImage) {
    $.blockUI({ message: '<div><div style="display:inline-block;width:auto;text-align:center;"><img src="' + loadingImage + '" style="display:inline-block;width:100%;text-align:center;"/></div><div style="display:inline-block;width:100%;text-align:center;color:#fff;">Loading...</div></div>', css: { width: '40%', backgroundColor: 'none', border: 'none'} });
}

function unblockUI() {
    $.unblockUI();
}

function newPopup(url, h, w) {
    if (h == '' || h == undefined || h == 0) h = 900;
    if (w == '' || w == undefined || w == 0) w = 1000;

    var popupWindow = window.open(url, 'popUpWindow', 'height=' + h + ',width=' + w + ',left=500,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
}
