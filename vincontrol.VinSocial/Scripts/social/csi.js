$(document).ready(function () {
    $("#csi_popup_takingsurvey_btn").fancybox({
        width: 500,
        height: 700
    });

    $("#csi_newcustomer_btn").fancybox({
        width: 500,
        height: 700
    });

    $('button[name="cancel-survey"]').click(function () {
        $('.fancybox-close').trigger('click');
    });

    $('button.client-popup-cancel').click(function () {
        $('.fancybox-close').trigger('click');
    });
});
