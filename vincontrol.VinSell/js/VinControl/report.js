/// <reference path="../jquery-1.7.2.min.js" />

$(document).ready(function () {
    reportHoverManageEvents();
});

function reportHoverManageEvents() {
    $(".reports_items_content_part").mouseenter(function() {

        $(this).find(".report_content_view").show();
        $(this).find(".report_content_view_left").show();
        $(this).find(".report_content_view_middle").show();
    }).mouseleave(function () {
        $(this).find(".report_content_view").hide();
        $(this).find(".report_content_view_left").hide();
        $(this).find(".report_content_view_middle").hide();
    });
};